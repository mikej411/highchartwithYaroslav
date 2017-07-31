namespace RCP.AppFramework
{
    /// <summary>
    /// Provides access to all the known "Criteria" for the the application.
    /// Criteria are typically used when waiting for elements.  I often wait until some
    /// "Criteria" is met before continuing with a test.
    /// </summary>
    public static class Criteria
    {
        public static readonly LoginPageCriteria LoginPage = new LoginPageCriteria();

        public static readonly ActivitiesListPageCriteria ActivitiesListPage = new ActivitiesListPageCriteria();

        public static readonly DashboardPageCriteria DashboardPage = new DashboardPageCriteria();

        public static readonly EnterCPDActivityPageCriteria EnterCPDActivityPage = new EnterCPDActivityPageCriteria();

        public static readonly CBDLearnerPageCriteria CBDLearnerPage = new CBDLearnerPageCriteria();

        public static readonly CBPObserverPageCriteria CBPObserverPage = new CBPObserverPageCriteria();

        
    }
}