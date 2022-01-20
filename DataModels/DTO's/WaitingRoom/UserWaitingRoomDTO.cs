using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s.WaitingRoom
{
    public class UserWaitingRoomDTO
    {

        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Phone_Number { get; init; }

        public DateTimeOffset TimeAdded { get; init; }
    }
}
