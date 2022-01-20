using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.Entities
{
    public record BusinessModel
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Type { get; init; }

        public string Description { get; init; }

        public string Owner { get; init; }

        public string Phone_Number { get; init; }

        public string Email { get; set; }

        public DateTimeOffset CreateDate { get; init; }
    }
}
