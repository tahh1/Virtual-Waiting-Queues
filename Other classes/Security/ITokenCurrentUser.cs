using FinalProject.DataModels.DTO_s;

namespace FinalProject.Other_classes.Security
{
    public interface ITokenCurrentUser
    {
        UserPublicVM GetCurrenttUser();
    }
}