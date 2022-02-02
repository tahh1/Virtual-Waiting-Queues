namespace FinalProject.Other_classes.Security
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshToken);
    }
}