using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s
{
    public class UserUpdateDTO
    {

        
        public string username { get; init; }

        [EmailAddress]
        
        public string email { get; init; }

       
        public string password { get; init; }


        [Phone]
        public string phone_number { get; init; }
    }
}
