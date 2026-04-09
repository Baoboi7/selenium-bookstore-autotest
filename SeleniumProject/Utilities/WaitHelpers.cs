using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumProject.Utilities
{
    public class WaitHelpers
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WaitHelpers(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(ConfigReader.ExplicitWaitInSeconds));
        }

        public IWebElement WaitForElementVisible(By by)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public IWebElement WaitForElementClickable(By by)
        {
            return _wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }
    }
}