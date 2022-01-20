using FinalProject.DataModels.DTO_s;
using FinalProject.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Other_classes
{
    public static class PrivateUserVMConverter
    {

        public static UserPrivateVM PrivateVM(this UserModel item , string TokenString)
        {
            return new UserPrivateVM()
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Role = item.Role,
                Phone_Number = item.Phone_Number,
                Token = TokenString,
                CreateDate = DateTimeOffset.UtcNow,
                Name = item.Name
               
            };

        }
    }
}
