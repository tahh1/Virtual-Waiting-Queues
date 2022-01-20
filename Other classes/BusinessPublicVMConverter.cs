using FinalProject.DataModels.DTO_s.Businesses;
using FinalProject.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Other_classes
{
    public static class BusinessPublicVMConverter
    {

        public static BusinessPublicVM BPVM(this BusinessModel item)
        {
            return new BusinessPublicVM()
            {
                Id = item.Id,
                Phone_Number = item.Phone_Number,
                Description = item.Description,
                Name = item.Name,
                Owner = item.Owner,
                Type= item.Type,
                Email = item.Email
               


            };

        }
    }
}
