using FinalProject.DataModels.DTO_s.security;
using System;

namespace FinalProject.Repositories.Repos
{
    public interface IRefreshTokenRepos
    {
        void Delete(Guid id);
        void DeleteAll(Guid userid);
        RefreshTokenModel GetrefreshtokenById(Guid id);
        RefreshTokenModel GetrefreshtokenByToken(string token);

        void createtoken(RefreshTokenModel NewItem);
    }
}