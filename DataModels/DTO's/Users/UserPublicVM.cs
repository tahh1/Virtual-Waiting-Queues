using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s
{
    public class UserPublicVM
    {

        public Guid Id { get; init; }

        public string UserName { get; init; }

        public string Name { get; init; }

        public string Email { get; init; }

        public string Role { get; init; }

        public string Phone_Number { get; init; }
    }
}
