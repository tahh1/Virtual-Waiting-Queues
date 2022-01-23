using FinalProject.DataModels.DTO_s;
using FinalProject.DataModels.Entities;
using FinalProject.Other_classes;
using FinalProject.Repositories.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Other_classes.Security;

namespace FinalProject.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class UserPortalController : ControllerBase
    {

        private readonly IUserRepos repository;
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;


        public UserPortalController(IUserRepos repository , ITokenCurrentUser TokenUser, ITokenGenerator TokenGen)
        {
            this.repository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
        }

        [HttpGet]
        [Authorize]
        //User //Anyonewith Token(remove id field and work with id provided in token)
        public ActionResult<UserPublicVM> GetUser()
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var user = repository.GetUserById(CurrentUser.Id);
            if (user is null)
            {
                return NotFound(new { message = "User Not Found" });
            }
            return user.PublicVM();
        }


        [HttpPut]
        [Authorize]
        //Anyonewith Token(remove id field and work with id provided in token)
        public ActionResult UpdateUser(UserUpdateDTO newversion)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var found = repository.GetUserById(CurrentUser.Id);
            if (newversion.email is not null && repository.GetUserByEmail(newversion.email) is not null)
            {
                return BadRequest(new { message = "Email  Already Exists " });
            }

            if(newversion.username is not null && repository.GetUserByName(newversion.username) is not null)
            {
                return BadRequest(new {message =  "Name Already Exits" });
            }

            if (found is null) { return NotFound(new { message = "User is not found" }); }

            UserModel model = found with { UserName = newversion.username is not null ? newversion.username : found.UserName,
                Email = newversion.email is not null ? newversion.email : found.Email,
                Password = newversion.password is not null ? newversion.password : found.Password,
                Phone_Number = newversion.phone_number is not null ? newversion.phone_number : found.Phone_Number

            };
            repository.UpdateUser(model);
            string TokenString = TokenGenerator.GenerateToken(model);
            var FinalModel = model.PrivateVM(TokenGenerator.GenerateToken(model));
            return Ok(FinalModel);
        }


        [AllowAnonymous]
        [HttpPost]
        //Anyone even without Token
        public ActionResult<UserModel> Register(RegistrationResponseDTO NewUser)
        {
            var username = repository.GetUserByName(NewUser.username);
            var useremail = repository.GetUserByEmail(NewUser.email);
            if (username is not null || useremail is not null)
            {
                return BadRequest(new { message = "Username or email already exist" });
            }
            UserModel model = new UserModel
            {
                Id = Guid.NewGuid(),
                UserName = NewUser.username,
                Email = NewUser.email,
                Password = NewUser.password,
                Role = NewUser.Role,
                CreateDate = DateTimeOffset.UtcNow,
                Phone_Number = NewUser.phone_number,
                Name=NewUser.name
            };
            repository.UserRegister(model);
            string TokenString = TokenGenerator.GenerateToken(model);
            var FinalModel = model.PrivateVM(TokenGenerator.GenerateToken(model));
        

            return CreatedAtAction(nameof(GetUser), new { id = model.Id }, FinalModel);

        }

    }
}
