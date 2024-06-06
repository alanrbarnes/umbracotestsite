using UdemyTestSite.ViewModels;

namespace UdemyTestSite.Interfaces
{
    public interface IEmailService
    {
        void SendContactNotificationToAdmin(ContactFormViewModel vm);
        void SendVerifyEmailAddressNotification(string membersEmail, string verificationToken);
    }
}
