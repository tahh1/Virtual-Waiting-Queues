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
    [Route("queue/[controller]")]
    [ApiController]
    public class UserQueueController : ControllerBase
    {

        private readonly IWaitingRoomRepos Roomrepository;
        private readonly ITokenGenerator TokenGenerator;
        private readonly ITokenCurrentUser TokenCurrentUser;
        private readonly IUserRepos UserRepos;


        public UserQueueController(IWaitingRoomRepos repository, ITokenCurrentUser TokenUser, ITokenGenerator TokenGen, IUserRepos User)
        {
            this.Roomrepository = repository;
            TokenGenerator = TokenGen;
            TokenCurrentUser = TokenUser;
            UserRepos = User;
        }



        [HttpPost]
        [Authorize]
        //Add yourself to room if its open (Room Id In Body)
        public ActionResult AddMe(IdDTO Id)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(Id.Id);
            if (Room is null) { return BadRequest(new { message = "There is no room with this Id" }); }
            if (Room.is_open == true)
            {
                int index = Room.queue.FindIndex(y => y.Id == CurrentUser.Id);
                if (index != -1) { return BadRequest( new { message = "You Already Are in the room" }); }
                UserWaitingRoomDTO User = new UserWaitingRoomDTO
                {
                    Id = CurrentUser.Id,
                    Name =CurrentUser.Name,
                    Phone_Number = CurrentUser.Phone_Number,
                    TimeAdded = DateTimeOffset.UtcNow

                };
                List<UserWaitingRoomDTO> NewList = Room.queue;
                NewList.Add(User);
                Room.queue = NewList;
            Roomrepository.UpdateWaitingRoom(Room);
                return NoContent();
            }
            else
            {
                return BadRequest(new {message = "The Room is closed and can't Accept new people" });
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        //Remove user from room : your id from token and room id in body 
        public ActionResult RemoveMe(IdDTO Id)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(Id.Id);
            int index = Room.queue.FindIndex(y => y.Id == CurrentUser.Id);
            if (index == -1) { return BadRequest(new {message = "You're not in the room" }); }
            List<UserWaitingRoomDTO> NewList = Room.queue;
            NewList.RemoveAt(index);
            WaitingRoomModel NewRoom = Room with { queue = NewList };
            Roomrepository.UpdateWaitingRoom(NewRoom);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize]
        //Enter a queue (queeue id in body)

        public ActionResult<WaitingRoomModel> EnterQueue(IdDTO Id)
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Room = Roomrepository.GetRoomById(Id.Id);
            if (Room is null) { return NotFound( new { message ="There no room found"}); }

            return Room;

        }

        [HttpGet]
        [Authorize]
        //Get all queues where you waiting.

        public ActionResult<IEnumerable<WaitingRoomModel>> MyQueues()
        {
            var CurrentUser = TokenCurrentUser.GetCurrenttUser();
            var Rooms = Roomrepository.GetWaitingRooms().Where(room => (room.queue.FindIndex(y => y.Id == CurrentUser.Id))!=-1);
            if (Rooms is null) { return NotFound(); }

            return Rooms.ToList();

        }
    }
}
