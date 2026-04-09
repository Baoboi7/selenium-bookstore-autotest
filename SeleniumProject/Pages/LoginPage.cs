using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;
        private readonly string _url = $"{ConfigReader.BaseUrl}/login";

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        private By EmailOrPhoneInput => By.Id("identity");
        private By PasswordInput => By.Id("password");
        private By LoginButton => By.CssSelector("button[type='submit']");
        private By RememberMeCheckbox => By.Id("remember");
        private By ForgotPasswordLink => By.XPath("/html/body/main/div[2]/div/form/div[2]/div[1]/div/a[1]");
        private By RegisterNowLink => By.XPath("/html/body/main/div[2]/div/form/div[2]/div[1]/div/a[2]");
        private By LoginErrorMessage => By.XPath("/html/body/main/div[2]/div/form/p");

        private void Pause(int milliseconds = 1500)
        {
            Thread.Sleep(milliseconds);
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl(_url);
            Pause(2000);
        }

        public bool IsAtLoginPage()
        {
            return _driver.Url.Contains("/login");
        }

        public void EnterEmailOrPhone(string value)
        {
            var element = _wait.WaitForElementVisible(EmailOrPhoneInput);
            Pause(1000);
            element.Clear();
            Pause(1000);
            element.SendKeys(value);
            Pause(1500);
        }

        public void EnterPassword(string value)
        {
            var element = _wait.WaitForElementVisible(PasswordInput);
            Pause(1000);
            element.Clear();
            Pause(1000);
            element.SendKeys(value);
            Pause(1500);
        }

        public void ClickLogin()
        {
            Pause(1500);
            _wait.WaitForElementClickable(LoginButton).Click();
            Pause(2500);
        }

        public void Login(string emailOrPhone, string password)
        {
            EnterEmailOrPhone(emailOrPhone);
            EnterPassword(password);
            ClickLogin();
        }

        public void SetRememberMe(bool shouldCheck)
        {
            var checkbox = _wait.WaitForElementClickable(RememberMeCheckbox);
            Pause(1000);

            if (checkbox.Selected != shouldCheck)
            {
                checkbox.Click();
                Pause(1500);
            }
        }

        public void ClickForgotPassword()
        {
            Pause(1000);
            _wait.WaitForElementClickable(ForgotPasswordLink).Click();
            Pause(2000);
        }

        public void ClickRegisterNow()
        {
            Pause(1000);
            _wait.WaitForElementClickable(RegisterNowLink).Click();
            Pause(2000);
        }

        public string GetErrorMessage()
        {
            Pause(1000);
            return _wait.WaitForElementVisible(LoginErrorMessage).Text.Trim();
        }

        public bool IsErrorMessageDisplayed()
        {
            return _driver.FindElements(LoginErrorMessage).Count > 0;
        }

        public bool IsLoginSuccessful(string successUrlKeyword = "")
        {
            Pause(2000);

            if (!string.IsNullOrWhiteSpace(successUrlKeyword))
            {
                return _driver.Url.Contains(successUrlKeyword, StringComparison.OrdinalIgnoreCase);
            }

            return !_driver.Url.Contains("/login", StringComparison.OrdinalIgnoreCase);
        }
    }
}