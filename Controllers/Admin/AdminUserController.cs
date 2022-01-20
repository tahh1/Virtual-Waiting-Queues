using FinalProject.DataModels.DTO_s;
using FinalProject.DataModels.DTO_s.Users;
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
    [Route("resource/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {


        private readonly IUserRepos repository;
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;

        public AdminUserController(IUserRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen)
        {
            this.repository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserModel> AdminGetUser(IdDTO id)
        {
            var user = repository.GetUserById(id.Id);
            if (user is null)
            {
                return NotFound();
            }
            return user;
        }



        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<UserModel> AdminGetAllUsers()
        {
            var Users = repository.GetUsers();
            return Users;
        }



        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(IdDTO id)
        {
            var found = repository.GetUserById(id.Id);
            if (found is null) { return NotFound(); }

            repository.DeleteUser(id.Id);
            return NoContent();
        }


        [HttpPatch]
        [Authorize(Roles = "Admin")]

        public ActionResult UpdateUser(AdminUserUpdateDTO newversion)
        {
            var found = repository.GetUserById(newversion.Id);
            if (found is null) { return BadRequest("There is no user with provided Id"); }
            if (newversion.email is not null && repository.GetUserByEmail(newversion.email) is not null)
            {
                return BadRequest("Email  Already Exists ");
            }

            if (newversion.username is not null && repository.GetUserByName(newversion.username) is not null)
            {
                return BadRequest("Name Already Exits");
            }

            if (found is null) { return NotFound(); }

            UserModel model = found with
            {
                UserName = newversion.username is not null ? newversion.username : found.UserName,
                Email = newversion.email is not null ? newversion.email : found.Email,
                Password = newversion.username is not null ? newversion.username : found.Password,
                Phone_Number = newversion.phone_number is not null ? newversion.phone_number : found.Phone_Number

            };
            repository.UpdateUser(model);

            return NoContent();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserModel> Register(RegistrationResponseDTO NewUser)
        {
            var username = repository.GetUserByName(NewUser.username);
            var useremail = repository.GetUserByEmail(NewUser.email);
            if (username is not null || useremail is not null)
            {
                return BadRequest("Username or email already exist");
            }
            UserModel model = new UserModel
            {
                Id = Guid.NewGuid(),
                UserName = NewUser.username,
                Email = NewUser.email,
                Password = NewUser.password,
                Role = NewUser.Role
                ,
                CreateDate = DateTimeOffset.UtcNow,
                Phone_Number = NewUser.phone_number,
                Name = NewUser.name
            };
            repository.UserRegister(model);
            string TokenString = TokenGenerator.GenerateToken(model);
            var FinalModel = model.PrivateVM(TokenGenerator.GenerateToken(model));


            return CreatedAtAction(nameof(AdminGetUser), new { id = model.Id }, FinalModel);

        }
    }
}
