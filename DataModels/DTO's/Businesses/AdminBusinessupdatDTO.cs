using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s.Businesses
{
    public class AdminBusinessupdatDTO
    {

      
        [Required]
        public Guid Id { get; init; }
  
        public string Name { get; init; }

   
        public string Type { get; init; }


        public string Description { get; init; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone_Number { get; init; }
    }
}
