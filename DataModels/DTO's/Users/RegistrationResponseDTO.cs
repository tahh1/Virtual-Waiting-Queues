using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s
{
    public class RegistrationResponseDTO
    {
        [Required]
        public string username { get; init; }

        [Required]
        public string name { get; init; }

        [EmailAddress]
        [Required]
        public string email { get; init; }

        [Required]
        public string password { get; init; }

        [Required]
        public string Role { get; init; }

        [Required]
        [Phone]
        public string phone_number { get; init; }

    }
}
