using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;

        private By GoToCheckoutButton => By.XPath("/html/body/main/div/aside/a");
        private By FullNameInput => By.XPath("/html/body/main/div/div[1]/section[1]/div[2]/input[1]");
        private By PhoneInput => By.XPath("/html/body/main/div/div[1]/section[1]/div[2]/input[2]");
        private By EmailInput => By.XPath("/html/body/main/div/div[1]/section[1]/div[2]/input[3]");
        private By AddressInput => By.XPath("/html/body/main/div/div[1]/section[1]/div[2]/input[4]");
        private By PlaceOrderButton => By.XPath("/html/body/main/div/div[2]/section/button");
        private By TotalAmount => By.XPath("/html/body/main/div/div[2]/section/div[3]/div[3]");
        private By SuccessHeading => By.XPath("/html/body/main/h1");

        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        private void Pause(int ms = 1500)
        {
            Thread.Sleep(ms);
        }

        public void GoToCheckout()
        {
            _wait.WaitForElementClickable(GoToCheckoutButton).Click();
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

        public void EnterEmail(string email)
        {
            var input = _wait.WaitForElementVisible(EmailInput);
            input.Clear();
            input.SendKeys(email);
            Pause(1000);
        }

        public void EnterAddress(string address)
        {
            var input = _wait.WaitForElementVisible(AddressInput);
            input.Clear();
            input.SendKeys(address);
            Pause(1000);
        }

        public void ClickPlaceOrder()
        {
            _wait.WaitForElementClickable(PlaceOrderButton).Click();
            Pause(2000);
        }

        public void FillCheckoutForm(string fullName, string phone, string email, string address)
        {
            EnterFullName(fullName);
            EnterPhone(phone);
            EnterEmail(email);
            EnterAddress(address);
        }

        public string GetTotalAmountText()
        {
            return _wait.WaitForElementVisible(TotalAmount).Text.Trim();
        }

        public bool IsSuccessPage()
        {
            return _driver.Url.Contains("/checkout/success", StringComparison.OrdinalIgnoreCase);
        }

        public string GetSuccessHeadingText()
        {
            return _wait.WaitForElementVisible(SuccessHeading).Text.Trim();
        }

        public string GetAlertTextAndAccept()
        {
            try
            {
                IAlert alert = _driver.SwitchTo().Alert();
                string text = alert.Text;
                alert.Accept();
                Pause(1000);
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}