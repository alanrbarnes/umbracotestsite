using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Security;

namespace UdemyTestSite.Helpers
{
    public class MemberHelper
    {

        private readonly IMemberService _memberService;
        private readonly IMemberManager _memberManager;

        public MemberHelper(IMemberService memberService, IMemberManager memberManager)
        {
            _memberService = memberService;
            _memberManager = memberManager;
        }

        // Example method that uses IMemberService and IMemberManager
        public bool VerifyMemberAsync(string token)
        {
            // Logic to verify member using _memberService and _memberManager
            var member = _memberService.GetMembersByPropertyValue("emailVerifyToken", token)?.SingleOrDefault();

            if (member == null)
            {
                return false;
            }

            var alreadyVerified = member.GetValue<bool>("emailVerified");
            if (alreadyVerified)
            {
                return false;
            }

            member.SetValue("emailVerified", true);
            member.SetValue("emailVerifiedDate", DateTime.Now);
            _memberService.Save(member);

            return true;
        }
    }
}
