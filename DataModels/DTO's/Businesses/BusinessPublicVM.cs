using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s.Businesses
{
    public class BusinessPublicVM
    {

        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Type { get; init; }

        public string Description { get; init; }

        public string Email { get; set; }

        public string Owner { get; init; }

        public string Phone_Number { get; init; }
    }
}
