using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class ProfilePage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;
        private readonly string _url = $"{ConfigReader.BaseUrl}/profile";

        public ProfilePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        private By ProfileMenu => By.XPath("/html/body/header/nav/div[2]/div[2]/div/div[2]/a");
        private By FullNameInput => By.XPath("/html/body/div[2]/form/div[1]/input");
        private By PhoneInput => By.XPath("/html/body/div[2]/form/div[3]/input");
        private By AddressInput => By.XPath("/html/body/div[2]/form/div[4]/textarea");
        private By SaveButton => By.XPath("/html/body/div[2]/form/button");
        private By MessageBox => By.XPath("/html/body/div[2]/form/div[5]");

        private void Pause(int ms = 1500)
        {
            Thread.Sleep(ms);
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl(_url);
            Pause(2000);
        }

        public void ClickProfileMenu()
        {
            _wait.WaitForElementClickable(ProfileMenu).Click();
            Pause(2000);
        }

        public void EnterFullName(string fullName)
        {
            var input = _wait.WaitForElementVisible(FullNameInput);
            input.Clear();
            input.SendKeys(fullName);
            Pause(1000);
        }

        public void EnterPhone(string phone)
        {
            var input = _wait.WaitForElementVisible(PhoneInput);
            input.Clear();
            input.SendKeys(phone);
            Pause(1000);
        }

        public void EnterAddress(string address)
        {
            var input = _wait.WaitForElementVisible(AddressInput);
            input.Clear();
            input.SendKeys(address);
            Pause(1000);
        }

        public void ClickSave()
        {
            _wait.WaitForElementClickable(SaveButton).Click();
            Pause(2000);
        }

        public void UpdateProfile(string fullName, string phone, string address)
        {
            EnterFullName(fullName);
            EnterPhone(phone);
            EnterAddress(address);
            ClickSave();
        }

        public string GetMessageText()
        {
            return _wait.WaitForElementVisible(MessageBox).Text.Trim();
        }
    }
}