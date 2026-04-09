using OpenQA.Selenium;
using SeleniumProject.Utilities;
using System.Collections.ObjectModel;
using System.Threading;

namespace SeleniumProject.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelpers _wait;
        private readonly string _cartUrl = $"{ConfigReader.BaseUrl}/cart";

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WaitHelpers(driver);
        }

        // Product detail page
        private By MinusButton => By.XPath("/html/body/main/div[2]/div[2]/div[2]/div/button[1]");
        private By PlusButton => By.XPath("/html/body/main/div[2]/div[2]/div[2]/div/button[2]");
        private By AddToCartButton => By.XPath("/html/body/main/div[2]/div[2]/div[3]/button[2]");
        private By CartIcon => By.XPath("/html/body/header/nav/div[2]/a/span");

        // Cart page
        private By CartItemList => By.XPath("/html/body/main/div/div/div[1]");
        private By EmptyCartMessage => By.XPath("/html/body/main/h1");
        private By RemoveButton => By.XPath("/html/body/main/div/div/div[1]/div[2]/div/button");
        private By DecreaseQuantityInCartButton => By.XPath("/html/body/main/div/div/div[1]/div[3]/div/button[1]/span");
        private By IncreaseQuantityInCartButton => By.XPath("/html/body/main/div/div/div[1]/div[3]/div/button[2]/span");
        private By SubtotalLabel => By.XPath("/html/body/main/div/aside/div[3]/div");

        private void Pause(int ms = 1500)
        {
            Thread.Sleep(ms);
        }

        public void OpenCart()
        {
            _driver.Navigate().GoToUrl(_cartUrl);
            Pause(2000);
        }

        public void ClickPlusOnDetailPage(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                _wait.WaitForElementClickable(PlusButton).Click();
                Pause(1000);
            }
        }

        public void ClickMinusOnDetailPage(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                _wait.WaitForElementClickable(MinusButton).Click();
                Pause(1000);
            }
        }

        public void ClickAddToCart()
        {
            _wait.WaitForElementClickable(AddToCartButton).Click();
            Pause(1000);
            AcceptAlertIfPresent();
            Pause(2000);
        }

        public void ClickCartIcon()
        {
            _wait.WaitForElementClickable(CartIcon).Click();
            Pause(2000);
        }

        public bool IsCartEmpty()
        {
            return _driver.FindElements(CartItemList).Count == 0;
        }

        public bool IsEmptyCartMessageDisplayed()
        {
            return _driver.FindElements(EmptyCartMessage).Count > 0;
        }

        public int GetCartItemCount()
        {
            ReadOnlyCollection<IWebElement> items = _driver.FindElements(CartItemList);
            return items.Count;
        }

        public void RemoveItemFromCart()
        {
            _wait.WaitForElementClickable(RemoveButton).Click();
            Pause(2000);
        }

        public void IncreaseQuantityInCart(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                _wait.WaitForElementClickable(IncreaseQuantityInCartButton).Click();
                Pause(1000);
            }
        }

        public void DecreaseQuantityInCart(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                _wait.WaitForElementClickable(DecreaseQuantityInCartButton).Click();
                Pause(1000);
            }
        }

        public string GetSubtotalText()
        {
            return _wait.WaitForElementVisible(SubtotalLabel).Text.Trim();
        }
        private void AcceptAlertIfPresent()
        {
            try
            {
                IAlert alert = _driver.SwitchTo().Alert();
                alert.Accept();
                Pause(1500);
            }
            catch
            {
            }
        }
    }
}