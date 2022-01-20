using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s
{
    public record UserPrivateVM
    {
        public Guid Id { get; init; }

        public String Token { get; init; }

        public string UserName { get; init; }

        public string Name { get; init; }

        public string Email { get; init; }

        public string Role { get; init; }

        public string Phone_Number { get; init; }

        public DateTimeOffset CreateDate { get; init; }
    }
}
