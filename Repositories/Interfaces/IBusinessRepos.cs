using FinalProject.DataModels.Entities;
using System;
using System.Collections.Generic;

namespace FinalProject.Repositories.Repos
{
    public interface IBusinessRepos
    {
        void BusinessRegister(BusinessModel NewItem);
        void DeleteBusiness(Guid id);
        BusinessModel GetBusinessById(Guid id);
        //IEnumerable<BusinessModel> GetBusinessByName(string name);
        IEnumerable<BusinessModel> GetBusinesses();
        void UpdateBusiness(BusinessModel NewVersion);
    }
}