using FinalProject.DataModels.DTO_s.Businesses;
using FinalProject.DataModels.DTO_s.WaitingRoom;
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
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessRepos repository;
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IWaitingRoomRepos Waitingrepository;


        public BusinessController(IBusinessRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen , IWaitingRoomRepos wait)
        {
            this.repository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
            Waitingrepository = wait;
        }

        [HttpGet("mybusiness")]
        [Authorize(Roles ="Business Owner")]
        //User //Anyonewith Token(remove id field and work with id provided in token)
        public ActionResult<BusinessModel> MyBusiness()
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Business = repository.GetBusinessById(CurrentUser.Id);
            if (Business is null)
            {
                return NotFound("Business Not found");
            }
            return Business;
        }

        [HttpGet("all")]
        [Authorize]
        public IEnumerable<BusinessPublicVM> GetAllBusinesses()
        {
            var Businesses = repository.GetBusinesses().Select(item => item.BPVM());
            return Businesses;
        }



        //[HttpGet] (leave it for client side filtering)
        //public IEnumerable<BusinessPublicVM> SearchBusinessByName([FromBody] string name)
        //{
        //    var Businesses = repository.GetBusinessByName(name).Select(item => item.BPVM());
        //    return Businesses;
        //}

        [HttpPatch]
        [Authorize(Roles = "Business Owner")]
        //Anyonewith Token(remove id field and work with id provided in token)
        public ActionResult UpdateBusiness(BusinessUpdateDTO newversion)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var found = repository.GetBusinessById(CurrentUser.Id);
     

            if (found is null) { return NotFound("Business Not Found"); }

            BusinessModel model = found with
            {
                Name = newversion.Name is not null ? newversion.Name : found.Name,
                Email = newversion.Email is not null ? newversion.Email : found.Email,
                Description = newversion.Description is not null ? newversion.Description : found.Description,
                Type = newversion.Type is not null ? newversion.Type : found.Type,
                Phone_Number=newversion.Phone_Number is not null ? newversion.Phone_Number : found.Phone_Number

            };
            if (newversion.Name is not null)
            {
                var Room = Waitingrepository.GetRoomById(found.Id);
                WaitingRoomModel newRoom = Room with
                {
                    Business_name = newversion.Name
                };
                Waitingrepository.UpdateWaitingRoom(newRoom);

            }
            repository.UpdateBusiness(model);
            return NoContent();
        }


        [HttpPost]
        [Authorize(Roles = "Business Owner")]
        public ActionResult<BusinessModel> Register(BusinessrRegisterDTO NewBusiness)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var haveBusiness = repository.GetBusinessById(CurrentUser.Id);
            if (haveBusiness is not null)
            {
                return BadRequest("You Already Have A business with this Account");
            }
            BusinessModel model = new BusinessModel
            {
                Id = CurrentUser.Id,
                Name = NewBusiness.Name,
                Email = NewBusiness.Email is not null ? NewBusiness.Email : "This Business Has No Email",
                Type = NewBusiness.Type,
                Description = NewBusiness.Description is not null ? NewBusiness.Description: "This Business Has No Description",
                CreateDate = DateTimeOffset.UtcNow,
                Phone_Number = NewBusiness.Phone_Number,
                Owner = CurrentUser.UserName
               
            };

            WaitingRoomModel RoomModel = new WaitingRoomModel
            {
                Id = CurrentUser.Id,
                Business_name = NewBusiness.Name,
                is_open = false,
                queue = new List<UserWaitingRoomDTO>(),
                Owner = CurrentUser.UserName

            };
            repository.BusinessRegister(model);
            Waitingrepository.CreateWaitingRoom(RoomModel);

            return CreatedAtAction(nameof(MyBusiness), model);

        }
    }
}
