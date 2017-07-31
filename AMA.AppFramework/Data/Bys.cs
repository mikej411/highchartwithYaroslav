namespace AMA.AppFramework
{
    /// <summary>
    /// Provides access to all known selenium "By"s
    /// "By"s provide a way to find a particular element on a page.
    /// </summary>
    public static class Bys
    {
        /// <summary>
        /// Locators to find elements on the skeleton of the AMA page. i.e. The menu items, header items and footer items.
        /// </summary>
        public static readonly AMAPageBys AMAPage = new AMAPageBys();

        /// <summary>
        /// Locators to find elements on the login page
        /// </summary>
        public static readonly LoginPageBys LoginPage = new LoginPageBys();

        /// <summary>
        /// Locators to find elements on the My CPD Activities List page
        /// </summary>
        public static readonly LibraryPageBys LibraryPage = new LibraryPageBys();


        
    }
}