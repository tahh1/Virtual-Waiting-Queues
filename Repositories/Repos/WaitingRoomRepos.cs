using FinalProject.DataModels.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Repos
{
    public class WaitingRoomRepos : IWaitingRoomRepos
    {


        private readonly String DatabaseName = "Project";
        private readonly String CollectionName = "WaitingRooms";
        private IMongoCollection<WaitingRoomModel> ItemsCollections;
        private readonly FilterDefinitionBuilder<WaitingRoomModel> filterbuilder = Builders<WaitingRoomModel>.Filter;


        public WaitingRoomRepos(IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(DatabaseName);
            ItemsCollections = db.GetCollection<WaitingRoomModel>(CollectionName);
        }
        public void DeleteRoom(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            ItemsCollections.DeleteOne(filter);
        }

        public WaitingRoomModel GetRoomById(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }


        //public WaitingRoomModel GetUserByName(string name)
        //{
        //    var filter = filterbuilder.Eq(item => item.UserName, name);
        //    return ItemsCollections.Find(filter).SingleOrDefault();
        //}

        public WaitingRoomModel GetRoomByEmailBusinessName(string bname)
        {
            var filter = filterbuilder.Eq(item => item.Business_name, bname);
            return ItemsCollections.Find(filter).SingleOrDefault();
        }

        public IEnumerable<WaitingRoomModel> GetWaitingRooms()
        {
            return ItemsCollections.Find(new BsonDocument()).ToList();
        }

        public void CreateWaitingRoom(WaitingRoomModel NewItem)
        {
            ItemsCollections.InsertOne(NewItem);
        }

        public void UpdateWaitingRoom(WaitingRoomModel NewVersion)
        {
            var filter = filterbuilder.Eq(item => item.Id, NewVersion.Id);
            ItemsCollections.ReplaceOne(filter, NewVersion);
        }
    }
}

