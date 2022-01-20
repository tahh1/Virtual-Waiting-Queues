using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s
{
    public class LoginDTO
    {


        [EmailAddress]
        [Required]
        public string email { get; init; }

        [Required]
        public string password { get; init; }
    }
}
