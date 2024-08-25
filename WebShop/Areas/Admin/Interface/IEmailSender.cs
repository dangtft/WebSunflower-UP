namespace WebShop.Areas.Admin.Interface
{
    public interface IEmailSender
    {
        Task SenderEmailAsync(string email,string subject,string message);
    }
}
