using FinalProject.DataModels.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Repos
{
    public class BusinessRepos : IBusinessRepos
    {
        private readonly String DatabaseName = "Project";
        private readonly String CollectionName = "Businesses";
        private IMongoCollection<BusinessModel> ItemsCollections;
        private readonly FilterDefinitionBuilder<BusinessModel> filterbuilder = Builders<BusinessModel>.Filter;


        public BusinessRepos(IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            ItemsCollections = db.GetCollection<BusinessModel>(CollectionName);
        }
        public void DeleteBusiness(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            ItemsCollections.DeleteOne(filter);
        }

        public BusinessModel GetBusinessById(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }


        //public IEnumerable<BusinessModel> GetBusinessByName(string name)
        //{
        //    var filter = filterbuilder.Eq(item => item.Name.ToLower(), name.ToLower());
        //    return ItemsCollections.Find(filter).ToList();
        //}

        //public BusinessModel GetUserByEmail(string email)
        //{
        //    var filter = filterbuilder.Eq(item => item., email);
        //    return ItemsCollections.Find(filter).SingleOrDefault();
        //}

        public IEnumerable<BusinessModel> GetBusinesses()
        {
            return ItemsCollections.Find(new BsonDocument()).ToList();
        }

        public void BusinessRegister(BusinessModel NewItem)
        {
            ItemsCollections.InsertOne(NewItem);
        }

        public void UpdateBusiness(BusinessModel NewVersion)
        {
            var filter = filterbuilder.Eq(item => item.Id, NewVersion.Id);
            ItemsCollections.ReplaceOne(filter, NewVersion);
        }

    }
}
