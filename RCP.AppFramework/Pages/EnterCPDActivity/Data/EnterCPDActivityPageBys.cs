using OpenQA.Selenium;

namespace RCP.AppFramework
{
    public class EnterCPDActivityPageBys
    {
        public readonly By EnterACPDFrame = By.XPath("//iframe[@name='wndAddActivity']");
        public readonly By Sec1GroupLearnActSel = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_fb1_ctl06_ctl11_CEComboBox3126029");
        public readonly By ActivityAccrYesRdo = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_fb1_ctl06_ctl19_3126066|1");
        public readonly By ActivityAccrNoRdo = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_fb1_ctl06_ctl19_3126066|2");
        public readonly By HoursTxt = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_fb1_ctl06_ctl59_t3126006");
        public readonly By GroupActivityNameTxt = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_fb1_ctl06_ctl60_t3126028");
        public readonly By DateTxt = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_fb1_ctl06_ctl63_dtbDatePicker3126032_dateInput");
        public readonly By WhatDidYouLearnTxt = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_txtLearn");
        public readonly By WhatAdditLearningTxt = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_txtAffirm");
        public readonly By WhatChangesTxt = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_txtPractice");
        public readonly By AddFilesBtn = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_DocUpload1_RadUploadFilesfile0");
        public readonly By CancelBtn = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_btnCancel");
        public readonly By SendToHoldingBtn = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_btnSaveAndFinishLater");
        public readonly By ContinueBtn = By.Id("ctl00_ContentPlaceHolder1_AddEditExternalFormActivity1_btnSubmit");

        public readonly By SubmitBtn = By.Id("ctl00_ContentPlaceHolder1_btnSubmitOptional");

        public readonly By CloseBtn = By.Id("ctl00_ContentPlaceHolder1_btnClose3");


        
    }
}