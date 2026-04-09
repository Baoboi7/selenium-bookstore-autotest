using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumProject.Utilities
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            IWebDriver driver;

            switch (ConfigReader.Browser.ToLower())
            {
                case "chrome":
                default:
                    var options = new ChromeOptions();
                    options.AddArgument("--start-maximized");
                    driver = new ChromeDriver(options);
                    break;
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigReader.ImplicitWaitInSeconds);
            return driver;
        }
    }
}