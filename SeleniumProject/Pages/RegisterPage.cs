using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProject.Pages
{
    public class RegisterPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;
        private readonly string _url = $"{ConfigReader.BaseUrl}/register";

        public RegisterPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        private By FullNameInput => By.Id("fullname");
        private By EmailInput => By.Id("email");
        private By PhoneInput => By.Id("phone");
        private By PasswordInput => By.Id("password");
        private By ConfirmPasswordInput => By.Id("confirm-password");
        private By AcceptTermsCheckbox => By.Id("terms");
        private By RegisterButton => By.CssSelector("button[type='submit']");
        private By RegisterButtonFallback => By.XPath("/html/body/main/section/div/div[2]/form/button");
        private By RegisterErrorMessage => By.XPath("/html/body/main/section/div/div[2]/form/div[5]");

        private void Pause(int milliseconds = 1500)
        {
            Thread.Sleep(milliseconds);
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl(_url);
            Pause(2000);
        }

        public bool IsAtRegisterPage()
        {
            return _driver.Url.Contains("/register");
        }

        public void EnterFullName(string value)
        {
            var element = _wait.WaitForElementVisible(FullNameInput);
            Pause(1000);
            element.Clear();
            Pause(1000);
            element.SendKeys(value);
            Pause(1500);
        }

        public void EnterEmail(string value)
        {
            var element = _wait.WaitForElementVisible(EmailInput);
            Pause(1000);
            element.Clear();
            Pause(1000);
            element.SendKeys(value);
            Pause(1500);
        }

        public void EnterPhone(string value)
        {
            var element = _wait.WaitForElementVisible(PhoneInput);
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

        public void EnterConfirmPassword(string value)
        {
            var element = _wait.WaitForElementVisible(ConfirmPasswordInput);
            Pause(1000);
            element.Clear();
            Pause(1000);
            element.SendKeys(value);
            Pause(1500);
        }

        public void SetAcceptTerms(bool shouldCheck)
        {
            var checkbox = _wait.WaitForElementClickable(AcceptTermsCheckbox);
            Pause(1000);

            if (checkbox.Selected != shouldCheck)
            {
                checkbox.Click();
                Pause(1500);
            }
        }

        public void ClickRegister()
        {
            Pause(1500);

            try
            {
                _wait.WaitForElementClickable(RegisterButton).Click();
            }
            catch
            {
                _wait.WaitForElementClickable(RegisterButtonFallback).Click();
            }

            Pause(1000);
            AcceptSuccessAlertIfPresent();
            Pause(2000);
        }

        public void Register(string fullName, string email, string phone, string password, string confirmPassword, bool acceptTerms = true)
        {
            EnterFullName(fullName);
            EnterEmail(email);
            EnterPhone(phone);
            EnterPassword(password);
            EnterConfirmPassword(confirmPassword);
            SetAcceptTerms(acceptTerms);
            ClickRegister();
        }

        public string GetErrorMessage()
        {
            Pause(1000);
            return _wait.WaitForElementVisible(RegisterErrorMessage).Text.Trim();
        }

        public bool IsErrorMessageDisplayed()
        {
            return _driver.FindElements(RegisterErrorMessage).Count > 0;
        }

        public bool IsRegisterSuccessful(string successUrlKeyword = "/login")
        {
            Pause(2000);
            return _driver.Url.Contains(successUrlKeyword, StringComparison.OrdinalIgnoreCase);
        }
        private void AcceptSuccessAlertIfPresent()
        {
            try
            {
                WebDriverWait alertWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
                IAlert alert = alertWait.Until(driver => driver.SwitchTo().Alert());
                alert.Accept();
                Pause(1500);
            }
            catch
            {
            }
        }
    }
}