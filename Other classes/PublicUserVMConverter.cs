using FinalProject.DataModels.DTO_s;
using FinalProject.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Other_classes
{
    public static class PublicUserVMConverter
    {


        public static UserPublicVM PublicVM(this UserModel item)
        {
            return new UserPublicVM()
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Role = item.Role,
                Phone_Number = item.Phone_Number,
                Name=item.Name
                
               
            };

        }
    }
}
