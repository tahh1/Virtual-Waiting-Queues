using FinalProject.DataModels.DTO_s.WaitingRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.Entities
{
    public record WaitingRoomModel
    {

        public Guid Id { get; init; }

        public string Business_name { get; set; }


        public string Owner { get; set; }

        public Boolean is_open { get; set; }

        public List<UserWaitingRoomDTO> queue { get; set; }

        public DateTimeOffset CreatedDate { get; init; }



    }
}
