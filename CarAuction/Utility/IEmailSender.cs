namespace CarAuction.Utility
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string msg, string registrationCode);
    }
}
