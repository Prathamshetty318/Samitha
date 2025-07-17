namespace DubaiChaRaja.Service
{
    public interface IEmailService
    {
        Task SendResetEmail(string email, string code);
    }
}