using Browser.Core.Framework;

namespace RCP.AppFramework
{
    public class CBPObserverPageCriteria
    {
        public readonly ICriteria<CBPObserverPage> UsernameVisible = new Criteria<CBPObserverPage>(p =>
        {
            return p.Exists(Bys.CBDLearnerPage.UserNameLbl, ElementCriteria.IsVisible);

        }, "Username heading visible");

        public readonly ICriteria<CBPObserverPage> PageReady;

        public CBPObserverPageCriteria()
        {
            PageReady = UsernameVisible;
        }
    }
}
