using Browser.Core.Framework;

namespace RCP.AppFramework
{
    public class ActivitiesListPageCriteria
    {
        public readonly ICriteria<ActivitiesListPage> EnterACPDActivityBtnEnabled = new Criteria<ActivitiesListPage>(p =>
        {
            return p.Exists(Bys.ActivitiesListPage.EnterACPDActivityBtn, ElementCriteria.IsEnabled);

        }, "Enter A CPD Activity Button enabled");

        public readonly ICriteria<ActivitiesListPage> PageReady;

        public ActivitiesListPageCriteria()
        {
            PageReady = EnterACPDActivityBtnEnabled;
        }
    }
}
