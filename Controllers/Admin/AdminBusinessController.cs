using FinalProject.DataModels.DTO_s;
using FinalProject.DataModels.DTO_s.Businesses;
using FinalProject.DataModels.DTO_s.WaitingRoom;
using FinalProject.DataModels.Entities;
using FinalProject.Other_classes;
using FinalProject.Other_classes.Security;
using FinalProject.Repositories.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers.Admin
{

    [Route("resource/[controller]")]
    [ApiController]

    public class AdminBusinessController : ControllerBase
    {

        private readonly IBusinessRepos repository;
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IUserRepos UserRepo;
        private readonly IWaitingRoomRepos Waitingrepository;

        public AdminBusinessController(IBusinessRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen , IUserRepos repo, IWaitingRoomRepos wait)
        {
            this.repository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
            UserRepo = repo;
            Waitingrepository = wait;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        //User //Anyonewith Token(remove id field and work with id provided in token)
        public ActionResult<BusinessModel> AnyBusiness( IdDTO Id)
        {
            var Business = repository.GetBusinessById(Id.Id);
            if (Business is null)
            {
                return NotFound("Business Not Found");
            }
            return Business;
        }



        [HttpPatch]
        [Authorize(Roles = "Admin")]
        //Anyonewith Token(remove id field and work with id provided in token)
        public ActionResult UpdateBusiness(AdminBusinessupdatDTO newversion )
        {
          
            var found = repository.GetBusinessById(newversion.Id);


            if (found is null) { return NotFound("Business Not Found Or id not provided"); }

            BusinessModel model = found with
            {
                Name = newversion.Name is not null ? newversion.Name : found.Name,
                Email = newversion.Email is not null ? newversion.Email : found.Email,
                Description = newversion.Description is not null ? newversion.Description : found.Description,
                Type = newversion.Type is not null ? newversion.Type : found.Type,
                Phone_Number= newversion.Phone_Number is not null ? newversion.Phone_Number : found.Phone_Number

            };
            repository.UpdateBusiness(model);
            return NoContent();
        }



        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(IdDTO id)
        {
            var found = repository.GetBusinessById(id.Id);
            if (found is null) { return NotFound(); }

            repository.DeleteBusiness(id.Id);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<BusinessModel> Register(AdminBusinessRegDTO NewBusiness )
        {
            var User = UserRepo.GetUserById(NewBusiness.Id);
            if (User is null) { return BadRequest("There is no such user with this Id"); }
            if (User.Role !="Business Owner") { return BadRequest($"{User.Name} is not a business Owner , You can't create a business for him"); }
            var haveBusiness = repository.GetBusinessById(NewBusiness.Id);
            if (haveBusiness is not null)
            {
                return BadRequest($"{User.Name} Already Have A business with this Account");
            }
            BusinessModel model = new BusinessModel
            {
                Id = User.Id,
                Name = NewBusiness.Name,
                Email = NewBusiness.Email,
                Type = NewBusiness.Type,
                Description = NewBusiness.Description,
                CreateDate = DateTimeOffset.UtcNow,
                Phone_Number = NewBusiness.Phone_Number,
                Owner = User.UserName

            };

            WaitingRoomModel RoomModel = new WaitingRoomModel
            {
                Id = model.Id,
                Business_name = NewBusiness.Name,
                is_open = false,
                queue = new List<UserWaitingRoomDTO>(),
                Owner=User.UserName
            };
            repository.BusinessRegister(model);
            Waitingrepository.CreateWaitingRoom(RoomModel);

            return CreatedAtAction(nameof(AnyBusiness), model);

        }
    }  
}
