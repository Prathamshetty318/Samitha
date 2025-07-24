using DubaiChaRaja.Models;

namespace DubaiChaRaja.Service{
    public interface IUserService
    {
        void RegisterUser(string username, string email, string password);
        User ValidateUser(string username, string password);
        bool UserExists(string username);

        bool EmailExists(string email);


        void SaveVerificationCode(string email, string code);
        bool IsCodeValid(string email, string code);
        void UpdatePassword(string email, string newPassword);


    }
}