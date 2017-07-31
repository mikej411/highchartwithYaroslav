using Browser.Core.Framework;

namespace RCP.AppFramework
{
    public class EnterCPDActivityPageCriteria
    {
        public readonly ICriteria<EnterCPDActivityPage> iFrameVisible = new Criteria<EnterCPDActivityPage>(p =>
        {
            return p.Exists(Bys.EnterCPDActivityPage.EnterACPDFrame, ElementCriteria.IsVisible);

        }, "iFrame is visible");

        public readonly ICriteria<EnterCPDActivityPage> IsActivityAccreditedYesVisible = new Criteria<EnterCPDActivityPage>(p =>
        {
            return p.Exists(Bys.EnterCPDActivityPage.ActivityAccrYesRdo, ElementCriteria.IsVisible);

        }, "IsActivityAccreditedYes radio button visible");

        public readonly ICriteria<EnterCPDActivityPage> PageReady;
        public EnterCPDActivityPageCriteria()
        {
            
            PageReady = iFrameVisible;
        }
    }
}
