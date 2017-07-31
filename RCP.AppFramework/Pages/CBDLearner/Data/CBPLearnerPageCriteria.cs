using Browser.Core.Framework;

namespace RCP.AppFramework
{
    public class CBDLearnerPageCriteria
    {
        public readonly ICriteria<CBDLearnerPage> UsernameVisible = new Criteria<CBDLearnerPage>(p =>
        {
            return p.Exists(Bys.CBDLearnerPage.UserNameLbl, ElementCriteria.IsVisible);

        }, "Username heading visible");

        public readonly ICriteria<CBDLearnerPage> PageReady;

        public CBDLearnerPageCriteria()
        {
            PageReady = UsernameVisible;
        }
    }
}
