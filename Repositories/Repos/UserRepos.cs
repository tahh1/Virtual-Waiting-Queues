using FinalProject.DataModels.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Repos
{
    public class UserRepos : IUserRepos
    {

        private readonly String DatabaseName = "Project";
        private readonly String CollectionName = "Users";
        private IMongoCollection<UserModel> ItemsCollections;
        private readonly FilterDefinitionBuilder<UserModel> filterbuilder = Builders<UserModel>.Filter;


        public UserRepos(IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            ItemsCollections = db.GetCollection<UserModel>(CollectionName);
        }
        public void DeleteUser(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            ItemsCollections.DeleteOne(filter);
        }

        public UserModel GetUserById(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }


        public UserModel GetUserByName(string name)
        {
            var filter = filterbuilder.Eq(item => item.UserName, name);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }

        public UserModel GetUserByEmail(string email)
        {
            var filter = filterbuilder.Eq(item => item.Email, email);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return ItemsCollections.Find(new BsonDocument()).ToList();
        }

        public void UserRegister(UserModel NewItem)
        {
            ItemsCollections.InsertOne(NewItem);
        }

        public void UpdateUser(UserModel NewVersion)
        {
            var filter = filterbuilder.Eq(item => item.Id, NewVersion.Id);
            ItemsCollections.ReplaceOne(filter, NewVersion);
        }
    }
}
