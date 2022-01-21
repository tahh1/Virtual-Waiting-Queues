using FinalProject.DataModels.DTO_s;
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

namespace FinalProject.Controllers.Admin
{
    [Route("waitingroom/[controller]")]
    [ApiController]
    public class AdminRoomManagerController : ControllerBase
    {
        
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IUserRepos UserRepos;
        private readonly IWaitingRoomRepos Waitingrepository;
        private readonly IBusinessRepos BusinessRepos;


        public AdminRoomManagerController(ITokenCurrentUser TokenUser, ITokenGenerator TokenGen, IUserRepos User , IWaitingRoomRepos wait , IBusinessRepos business)
        {
            
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
            UserRepos = User;
            Waitingrepository = wait;
            BusinessRepos = business;
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<BusinessModel> RoomRegister(IdDTO Id)
        {
            
            var haveBusiness = BusinessRepos.GetBusinessById(Id.Id);
            var haveRoom = Waitingrepository.GetRoomById(Id.Id);
            var user = UserRepos.GetUserById(Id.Id);
            if (user is null)  { return NotFound(); }
            if (haveRoom is not null ) { return BadRequest(new  { message ="User already have a waiting room" }); }
            if(user is not null & user.Role=="Business Owner" & haveBusiness is null) { return BadRequest(new { message = "User needs to create a business first" }); }
            if(user is not null & user.Role!="Business Owner") { return BadRequest(new { message = "User can't create a business or waiting room" }); }

            WaitingRoomModel model = new WaitingRoomModel
            {
                Id = Id.Id,
                Business_name = haveBusiness.Name,
                Owner = user.Name,
                is_open = false,
                CreatedDate = DateTimeOffset.UtcNow,
                queue = new List<UserWaitingRoomDTO>()
              

            };

            Waitingrepository.CreateWaitingRoom(model);

            return Ok(model);

        }










        [HttpPatch]
        [Authorize(Roles = "Admin")]

        //Add yourself to room if its open (Room Id In Body)
        public ActionResult OpenCloseRoom(IdDTO Id)
        {
            var Room = Waitingrepository.GetRoomById(Id.Id);
            if (Room.is_open == true)
            {   
                Room.is_open = false;
            }
            else { Room.is_open = true; }
            Waitingrepository.UpdateWaitingRoom(Room);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteRoom(IdDTO Id)
        {
            var Room = Waitingrepository.GetRoomById(Id.Id);
            if (Room is null) { return NotFound(); }
            Waitingrepository.DeleteRoom(Room.Id);
            return NoContent();
        }
 

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        //Enter a room

        public ActionResult<WaitingRoomModel> GetRoom(IdDTO Id)
        {
            var Room = Waitingrepository.GetRoomById(Id.Id);
            if (Room is null) { return NotFound(); }

            return Room;

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        //Get all queues where you waiting.

        public ActionResult<IEnumerable<WaitingRoomModel>> AllRooms()
        {
           
            var Rooms = Waitingrepository.GetWaitingRooms();
            if (Rooms is null) { return NotFound(); }

            return Rooms.ToList();

        }
    }
}

