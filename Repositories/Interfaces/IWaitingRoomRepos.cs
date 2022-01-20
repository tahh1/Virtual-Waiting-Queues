using FinalProject.DataModels.Entities;
using System;
using System.Collections.Generic;

namespace FinalProject.Repositories.Repos
{
    public interface IWaitingRoomRepos
    {
        void CreateWaitingRoom(WaitingRoomModel NewItem);
        void DeleteRoom(Guid id);
        WaitingRoomModel GetRoomByEmailBusinessName(string bname);
        WaitingRoomModel GetRoomById(Guid id);
        IEnumerable<WaitingRoomModel> GetWaitingRooms();
        void UpdateWaitingRoom(WaitingRoomModel NewVersion);
    }
}