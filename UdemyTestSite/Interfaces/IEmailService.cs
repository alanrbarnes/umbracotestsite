using UdemyTestSite.ViewModels;

namespace UdemyTestSite.Interfaces
{
    public interface IEmailService
    {
        bool SendContactNotificationToAdmin(ContactFormViewModel vm);
        bool SendVerifyEmailAddressNotification(string membersEmail, string verificationToken);
        bool SendResetPasswordNotification(string membersEmail, string resetToken);
        bool SendPasswordChangedNotification(string membersEmail);
    }
}
