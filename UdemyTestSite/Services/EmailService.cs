using System.Net.Mail;
using UdemyTestSite.Interfaces;
using UdemyTestSite.ViewModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Core.Logging;
using Lucene.Net.Analysis;
using Microsoft.AspNetCore.SignalR;  //add umbraco logging
using UdemyTestSite.Helpers;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models.Email;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Web.Common.PublishedModels;
using System.Web;

namespace UdemyTestSite.Services
{
    //the home to all outbound emails from the site
    public class EmailService : IEmailService
    {
        private UmbracoHelper _umbracoHelper;
        private IContentService _contentService; //added
        private readonly ILogger<EmailService> _logger; //added
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailService(UmbracoHelper umbracoHelper, 
                            IContentService contentService,
                            ILogger<EmailService> logger,
                            IOptions<SmtpSettings> smtpSettings,
                            IConfiguration configuration,
                            IHttpContextAccessor httpContextAccessor)
        {
            _umbracoHelper = umbracoHelper;
            _contentService = contentService; //added
            _logger = logger;  //added
            _smtpSettings = smtpSettings.Value;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }

        public IPublishedContent GetSiteSettings()
        {
            var siteSettings = _umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
            return siteSettings != null ? siteSettings : throw new Exception("No site settings found");
        }

        //Snd a notification to the user when their password is changed
        public bool SendPasswordChangedNotification(string membersEmail)
        {
            //Get Template
            //Get Template - create a new template in umbraco
            var emailTemplate = GetEmailTemplate("Password Changed");//_umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("emailTemplate").FirstOrDefault();

            if (emailTemplate == null)
            {
                throw new Exception("No email template found");
            }

            //get site settings
            var siteSettings = GetSiteSettings();

            //read the email From and To Addresses
            var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
            //var toAddresses = siteSettings.Value<string>("emailSettingsAdminAccounts");

            //Get the data
            //Fields from template
            //get the template data
            var subject = emailTemplate.Value<string>("emailTemplateSubjectLine");
            var htmlContent = emailTemplate.Value<string>("emailTemplateHtmlContent");
            var textContent = emailTemplate.Value<string>("emailTemplateTextContent");

            //Send the Email
            var result = SendMail(fromAddress, membersEmail, subject, htmlContent, textContent, siteSettings);
            return result;

        }

        //Send the reset password link to the user
        public bool SendResetPasswordNotification(string membersEmail, string resetToken)
        {
            //Get Template
            //Get Template - create a new template in umbraco
            var emailTemplate = GetEmailTemplate("Reset Password");//_umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("emailTemplate").FirstOrDefault();

            if (emailTemplate == null)
            {
                throw new Exception("No email template found");
            }

            //get site settings
            var siteSettings = GetSiteSettings();

            //read the email From and To Addresses
            var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
            //var toAddresses = siteSettings.Value<string>("emailSettingsAdminAccounts");


            //Get the data
            //Fields from template
            //get the template data
            var subject = emailTemplate.Value<string>("emailTemplateSubjectLine");
            var htmlContent = emailTemplate.Value<string>("emailTemplateHtmlContent");
            var textContent = emailTemplate.Value<string>("emailTemplateTextContent");

            //Mail Merge
            string url = GetBaseUrl();

            string encodedResetToken = HttpUtility.UrlEncode(resetToken); // URL encode the resetToken

            url += $"/reset-password?token={encodedResetToken}"; // Append the encoded resetToken to the URL

            //url += $"/reset-password?token={resetToken}";

            //##reset-url##
            MailMerge("reset-url", url, ref htmlContent, ref textContent);

            //Send the Email
            var result = SendMail(fromAddress, membersEmail, subject, htmlContent, textContent, siteSettings);
            return result;
        }


        //Send a notification to the admin when a contact form is submitted
        public bool SendContactNotificationToAdmin(ContactFormViewModel vm)
        {
            //get email template from umbraco for this notification
            var emailTemplate = GetEmailTemplate("New Contact Form Notification");//_umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("emailTemplate").FirstOrDefault();
        
            if (emailTemplate == null)
            {
                throw new Exception("No email template found");
            }
        
            //get the template data
            var subject = emailTemplate.Value<string>("emailTemplateSubjectLine");
            var htmlContent = emailTemplate.Value<string>("emailTemplateHtmlContent");
            var textContent = emailTemplate.Value<string>("emailTemplateTextContent");
        
            //Mail merge the necessary fields
            MailMerge("name", vm.Name, ref htmlContent, ref textContent); //##name##
            MailMerge("email", vm.EmailAddress, ref htmlContent, ref textContent); //##email##
            MailMerge("comment", vm.Comment, ref htmlContent, ref textContent); //##comment##

            //get site settings
            var siteSettings = GetSiteSettings();

            //read the email From and To Addresses
            var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
            var toAddresses = siteSettings.Value<string>("emailSettingsAdminAccounts");

            //Send the Email refactored, add this method and remove rest of the code
            var result = SendMail(fromAddress, toAddresses, subject, htmlContent, textContent, siteSettings);
            return result;
        }

        //Send the email verification link to the new member
        public bool SendVerifyEmailAddressNotification(string membersEmail, string verificationToken)
        {
            //Get Template - create a new template in umbraco
            var emailTemplate = GetEmailTemplate("Verify Email");//_umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("emailTemplate").FirstOrDefault();

            if (emailTemplate == null)
            {
                throw new Exception("No email template found");
            }

            //get site settings
            var siteSettings = GetSiteSettings();

            //read the email From and To Addresses
            var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
            //var toAddresses = siteSettings.Value<string>("emailSettingsAdminAccounts");

            //Fields from template
            //get the template data
            var subject = emailTemplate.Value<string>("emailTemplateSubjectLine");
            var htmlContent = emailTemplate.Value<string>("emailTemplateHtmlContent");
            var textContent = emailTemplate.Value<string>("emailTemplateTextContent");

            //Mail Merge
            //Build the url to be the absolute url to the verify page
            string url = GetBaseUrl();

            url += $"/verify-email?token={verificationToken}";

            MailMerge("verify-url", url, ref htmlContent, ref textContent);

            //Log the Email

            //Send the Email
            var result = SendMail(fromAddress, membersEmail, subject, htmlContent, textContent, siteSettings);
            return result;
        }

        //A generic send mail that logs the email in umbraco and sends via smtp
        private bool SendMail(string fromAddresses, string toAddresses, string subject, string htmlContent, string textContent, IPublishedContent siteSettings)
        {
            //get site settings
            //var siteSettings = _umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
            //if (siteSettings == null)
            //{
            //    throw new Exception("No site settings found");
            //}
            if (string.IsNullOrEmpty(fromAddresses))
            {
                throw new Exception("There needs to be a from address in site settings");
            }
            if (string.IsNullOrEmpty(toAddresses))
            {
                throw new Exception("There needs to be a to address in site settings");
            }

            //read the email From Addresses
            //var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");

            //Debug Mode
            var debugMode = siteSettings.Value<bool>("testMode");  //added
            var testEmailAddress = siteSettings.Value<string>("emailTestAccounts");  //added

            var recipients = toAddresses;
            if (debugMode)
            {
                //change the To - testEmailAccounts
                recipients = testEmailAddress;  //.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //Alter subject to show test mode
                subject += "(Test Mode) - " + toAddresses;
                //toAddress = testEmailAddress;
            }

            //Log the email in umbraco
            var emails = _umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("emails").FirstOrDefault();
            var newEmail = _contentService.Create(toAddresses, emails.Id, "Email");
            newEmail.SetValue("emailSubject", subject);
            newEmail.SetValue("emailToAddress", recipients);
            newEmail.SetValue("emailHtmlContent", htmlContent);
            newEmail.SetValue("emailTextContent", textContent);
            newEmail.SetValue("emailSent", false);
            _contentService.SaveAndPublish(newEmail);  //create new email record in umbraco

            //Send the email via smtp or whatever

            //functionality to add html text
            var mimeType = new System.Net.Mime.ContentType("text/html");
            var alternateView = AlternateView.CreateAlternateViewFromString(htmlContent, mimeType);

            var smtpMessage = new MailMessage();
            smtpMessage.AlternateViews.Add(alternateView);

            //To - collection or one email
            foreach (var recipient in recipients.Split(','))
            {
                if (!string.IsNullOrEmpty(recipient))
                {
                    smtpMessage.To.Add(recipient);
                }
            }

            //From
            smtpMessage.From = new MailAddress(fromAddresses);
            //Subject
            smtpMessage.Subject = subject;
            //Body
            smtpMessage.Body = textContent;
            smtpMessage.IsBodyHtml = true;

            //Sending the email
            using (var smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _smtpSettings.PickupDirectoryLocation;
                    smtpClient.Send(smtpMessage);

                    newEmail.SetValue("emailSent", true);
                    newEmail.SetValue("emailSentDate", DateTime.Now);
                    _contentService.SaveAndPublish(newEmail);  //update email record in umbraco
                }
                catch (Exception ex)
                {
                    //log the error
                    _logger.LogError(ex, "Error sending email");
                    throw new Exception("Error sending email", ex);
                }

            }
            return true;

        }

        private void MailMerge(string token, string value, ref string htmlContent, ref string textContent)
        {
            htmlContent = htmlContent.Replace($"##{token}##", value);
            textContent = textContent.Replace($"##{token}##", value);
        }

        //returns the email template as IPublishedContent where the title matches the tmplate name
        private IPublishedContent GetEmailTemplate(string templateName)
        {
            var template = _umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("emailTemplate").Where(w => w.Name == templateName).FirstOrDefault();
            return template;
        }



        //Origional SendEmail method
        public void SendBasicEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = _smtpSettings.PickupDirectoryLocation;
                smtpClient.Send(mailMessage);
            }
        }

        //Origional SendEmail[] method
        public void SendBasicEmail(string[] toAddresses, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            foreach (var address in toAddresses)
            {
                if (!string.IsNullOrEmpty(address))
                {
                    mailMessage.To.Add(address);
                }
            }

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = _smtpSettings.PickupDirectoryLocation;
                smtpClient.Send(mailMessage);
            }
        }


    }
}
