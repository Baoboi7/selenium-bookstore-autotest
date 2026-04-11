using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class BookDetailPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;

        private By PlusQuantityButton => By.XPath("/html/body/main/div[2]/div[2]/div[2]/div/button[2]");
        private By QuantityValue => By.XPath("/html/body/main/div[2]/div[2]/div[2]/div/span");
        private By BookPrice => By.XPath("/html/body/main/div[2]/div[2]/p");
        private By BookTitle => By.XPath("/html/body/main/div[2]/div[2]/h1");

        public BookDetailPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        private void Pause(int ms = 1500)
        {
            Thread.Sleep(ms);
        }

        public void ClickPlusQuantity(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                _wait.WaitForElementClickable(PlusQuantityButton).Click();
                Pause(1000);
            }
        }

        public int GetQuantityValue()
        {
            string text = _wait.WaitForElementVisible(QuantityValue).Text.Trim();
            return int.Parse(text);
        }

        public string GetBookPriceText()
        {
            return _wait.WaitForElementVisible(BookPrice).Text.Trim();
        }

        public string GetBookTitleText()
        {
            return _wait.WaitForElementVisible(BookTitle).Text.Trim();
        }
    }
}