using FinalProject.DataModels.Entities;

namespace FinalProject.Other_classes
{
    public interface ITokenGenerator
    {
        string GenerateToken(UserModel user);
    }
}