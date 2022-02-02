using FinalProject.DataModels.DTO_s.security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Repos
{
    public class RefreshTokenRepos : IRefreshTokenRepos
    {

        private readonly String DatabaseName = "Project";
        private readonly String CollectionName = "RefreshTokenrecords";
        private IMongoCollection<RefreshTokenModel> ItemsCollections;
        private readonly FilterDefinitionBuilder<RefreshTokenModel> filterbuilder = Builders<RefreshTokenModel>.Filter;


        public RefreshTokenRepos(IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            ItemsCollections = db.GetCollection<RefreshTokenModel>(CollectionName);
        }


        public RefreshTokenModel GetrefreshtokenById(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.UserId, id);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }

        public RefreshTokenModel GetrefreshtokenByToken(String token)
        {
            var filter = filterbuilder.Eq(item => item.refreshtoken, token);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }


        public void Delete(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            ItemsCollections.DeleteMany(filter);
        }

        public void DeleteAll(Guid userid)
        {
            var filter = filterbuilder.Eq(item => item.UserId, userid);
            ItemsCollections.DeleteMany(filter);
        }

        public void createtoken(RefreshTokenModel NewItem)
        {
            ItemsCollections.InsertOne(NewItem);
        }
    }
}
