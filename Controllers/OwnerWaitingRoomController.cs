using FinalProject.DataModels.DTO_s;
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
    [Route("waitingroom/[controller]")]
    [ApiController]
    public class OwnerWaitingRoomController : ControllerBase
    {
        private readonly IWaitingRoomRepos Roomrepository;
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IUserRepos UserRepos;


        public OwnerWaitingRoomController(IWaitingRoomRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen, IUserRepos User)
        {
            this.Roomrepository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
            UserRepos = User;
        }



        [HttpPost]
        [Authorize(Roles = "Business Owner")]

        public ActionResult AddAnonymous()
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(CurrentUser.Id);
            if (Room.is_open == true)
            {
                UserWaitingRoomDTO Anonymous = new UserWaitingRoomDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Anonymous",
                    Phone_Number = "0000000",
                    TimeAdded = DateTimeOffset.UtcNow

                };
                List<UserWaitingRoomDTO> NewList = Room.queue;
                NewList.Add(Anonymous);
                Room.queue = NewList;
                Roomrepository.UpdateWaitingRoom(Room);
                return NoContent();
            }
            else
            {
                return BadRequest("The Room is closed and can't Accept new people");
            }
        }

        [HttpPatch]
        [Authorize(Roles = "Business Owner")]

        public ActionResult OpenClose()
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(CurrentUser.Id);
            if (Room.is_open == true)
            {
                Room.is_open = false;
            }
            else { Room.is_open = true; }
            Roomrepository.UpdateWaitingRoom(Room);
            return NoContent();
        }


        [HttpDelete("delete")]
        [Authorize(Roles = "Business Owner")]

        public ActionResult RemoveUser(IdDTO Id)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(CurrentUser.Id);
            int index = Room.queue.FindIndex(y => y.Id == Id.Id);
            if(index == -1) { return BadRequest(new { message = "No user with this id in your Room" }); }
            Room.queue.RemoveAt(index);
            Roomrepository.UpdateWaitingRoom(Room);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Business Owner")]

        public ActionResult<WaitingRoomModel> MyWaitingRoom()
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(CurrentUser.Id);
            if (Room is null) { return NotFound(); }

            return Room;

        }
    }
}
