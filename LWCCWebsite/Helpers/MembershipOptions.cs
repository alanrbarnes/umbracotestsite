namespace LWCCWebsite.Helpers
{
    public class MembershipOptions
    {
        public int MinimumPasswordLength { get; set; }
        public bool PasswordRequiresDigit { get; set; }
        public bool PasswordRequiresLowercase { get; set; }
        public bool PasswordRequiresUppercase { get; set; }
        public bool PasswordRequiresNonAlphanumeric { get; set; }
        public bool AllowManuallyChangingPassword { get; set; }
    }
}
