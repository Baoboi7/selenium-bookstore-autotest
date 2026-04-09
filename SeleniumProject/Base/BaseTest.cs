using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProject.Utilities;

namespace SeleniumProject.Base
{
    public class BaseTest
    {
        protected IWebDriver driver = null!;
        protected WaitHelpers wait = null!;

        [SetUp]
        public virtual void SetUp()
        {
            driver = DriverFactory.CreateDriver();
            wait = new WaitHelpers(driver);
        }

        [TearDown]
        public virtual void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}