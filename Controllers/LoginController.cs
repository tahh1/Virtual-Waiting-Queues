using FinalProject.DataModels.DTO_s;
using FinalProject.DataModels.DTO_s.security;
using FinalProject.DataModels.Entities;
using FinalProject.Other_classes;
using FinalProject.Other_classes.Security;
using FinalProject.Repositories.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ITokenGenerator TokenGenerator;
        private readonly IrefreshTokenGenerator refreshTokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IUserRepos repository;
        private readonly IRefreshTokenValidator tokenvalidator;
        private readonly IRefreshTokenRepos refreshRepos;

        public LoginController(IUserRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen, IrefreshTokenGenerator refreshTokenGen, IRefreshTokenValidator valid, IRefreshTokenRepos refreshTokenRepository)
        {
            this.repository = repository;
            TokenGenerator = TokenGen;
            refreshTokenGenerator = refreshTokenGen;
            TokenCurrentUser = TokenUser;
            tokenvalidator = valid;
            refreshRepos = refreshTokenRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login(LoginDTO login)
        {
            var UserExist = repository.GetUserByEmail(login.email);

            if (UserExist is not null && UserExist.Password == login.password)
            {
                string refreshToken = refreshTokenGenerator.GeneraterefreshToken();
                string TokenString = TokenGenerator.GenerateToken(UserExist);
                RefreshTokenModel newrefreshToken = new RefreshTokenModel()
                {
                    Id = Guid.NewGuid(),
                    refreshtoken = refreshToken,
                    UserId = UserExist.Id
                };

                refreshRepos.createtoken(newrefreshToken);
                return new OkObjectResult(new { Token = TokenString, refreshtoken = refreshToken });
            }

            else { return NotFound(new { message = "User Not Found" }); }


        }


        [HttpPost("refresh")]
        public ActionResult Refresh([FromBody] refreshrequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "refresh token is required" });
            }

            bool isValidRefreshToken = tokenvalidator.Validate(refreshRequest.refreshtoken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new { message = "refresh token not validated" });
            }

            RefreshTokenModel refreshTokenDTO =  refreshRepos.GetrefreshtokenByToken(refreshRequest.refreshtoken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new { message = "refresh token not found" });
            }

            refreshRepos.Delete(refreshTokenDTO.Id);

            
            var user = repository.GetUserById(refreshTokenDTO.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            string refreshToken = refreshTokenGenerator.GeneraterefreshToken();
            string TokenString = TokenGenerator.GenerateToken(user);

            RefreshTokenModel newrefreshToken = new RefreshTokenModel()
            {
                Id = Guid.NewGuid(),
                refreshtoken= refreshToken,
                UserId = user.Id
            };

            refreshRepos.createtoken(newrefreshToken);


            return new OkObjectResult(new { Token = TokenString, refreshToken = refreshToken });
        }


        [HttpDelete("logout")]
        [Authorize]
        public ActionResult  Logout()
        {
         

            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var user = repository.GetUserById(CurrentUser.Id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }


            refreshRepos.DeleteAll(user.Id);


            return NoContent();
        }





    }
}
