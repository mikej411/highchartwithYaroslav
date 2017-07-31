using OpenQA.Selenium;

namespace AMA.AppFramework
{
    public class LoginPageBys
    {     
        public readonly By UserNameTxt = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_UserName");
        public readonly By UserNameWarningLbl = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_UserNameRequired");
        public readonly By PasswordTxt = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_Password");
        public readonly By PasswordWarningLbl = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_PasswordRequired");
        public readonly By RememberMeChk = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_RememberMe");
        public readonly By LoginBtn = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_Login");
        public readonly By LoginUnsuccessfullWarningLbl = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_FailureText");
        public readonly By ForgotPasswordLnk = By.Id("ctl00_ctl00_ContentPlaceHolderBase1_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_forgotPasswordbutton");


    
    }
}