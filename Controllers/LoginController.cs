using FinalProject.DataModels.DTO_s;
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
    [Route("user/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IUserRepos repository;

        public LoginController(IUserRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen)
        {
            this.repository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginDTO login)
        {
            var UserExist= repository.GetUserByEmail(login.email);

            if (UserExist is not null && UserExist.Password == login.password) {
                string TokenString = TokenGenerator.GenerateToken(UserExist);
                return new OkObjectResult(new  { Token = TokenString });
            }

            else { return NotFound("User Not Found"); }
            

        }
    }
}
