using Browser.Core.Framework;

namespace RCP.AppFramework
{
    public class DashboardPageCriteria
    {
        public readonly ICriteria<DashboardPage> EnterACPDActivityBtnEnabled = new Criteria<DashboardPage>(p =>
        {
            return p.Exists(Bys.DashboardPage.EnterACPDActivityBtn, ElementCriteria.IsEnabled);

        }, "Enter A CPD Activity Button enabled");

        public readonly ICriteria<DashboardPage> PageReady;

        public DashboardPageCriteria()
        {
            PageReady = EnterACPDActivityBtnEnabled;
        }
    }
}
