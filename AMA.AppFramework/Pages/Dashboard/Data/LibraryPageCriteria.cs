using Browser.Core.Framework;

namespace AMA.AppFramework
{
    public class LibraryPageCriteria
    {
        public readonly ICriteria<LibraryPage> ResetFilterBtnEnabled = new Criteria<LibraryPage>(p =>
        {
            return p.Exists(Bys.LibraryPage.ResetFiltersBtn, ElementCriteria.IsEnabled);

        }, "Reset Filter Button enabled");

        public readonly ICriteria<LibraryPage> PageReady;

        public LibraryPageCriteria()
        {
            PageReady = ResetFilterBtnEnabled;
        }
    }
}
