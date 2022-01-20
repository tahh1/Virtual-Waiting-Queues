using FinalProject.DataModels.Entities;
using System;
using System.Collections.Generic;

namespace FinalProject.Repositories.Repos
{
    public interface IUserRepos
    {
        void DeleteUser(Guid id);
        UserModel GetUserById(Guid id);
        IEnumerable<UserModel> GetUsers();
        void UserRegister(UserModel NewItem);
        void UpdateUser(UserModel NewVersion);
        UserModel GetUserByName(string name);
        UserModel GetUserByEmail(string email);
    }
}