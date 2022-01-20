using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s.Businesses
{
    public class BusinessrRegisterDTO
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Type { get; init; }

        
        public string Description { get; init; }

       
        public string Email { get; set; }

        [Required]
        public string  Phone_Number { get; init; }
    }
}
