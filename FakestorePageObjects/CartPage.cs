using Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace FakestorePageObjects
{
    public class CartPage : BasePage
    {

        
        private string CartUrl => baseUrl + "/koszyk";
       

        public IList<IWebElement> CartItems
        {
            get
            {
                _ = CartTable;
                return driver.FindElements(By.CssSelector("tr.cart_item"), 2);
            }
        }

        private By Loaders => By.CssSelector(".blockUI");
        public string ItemId => CartItems[0].FindElement(By.CssSelector("a")).GetAttribute("data-product_id");
        public IList<string> ItemIds =>
            CartItems.Select(item => item.FindElement(By.CssSelector("a")).GetAttribute("data-product_id")).ToList();

        public IWebElement CartTable => driver.FindElement(By.CssSelector("table.shop_table.cart"), 2);

        public IWebElement QuantityField
        {
            get
            {
                _ = CartTable;
                return driver.FindElement(By.CssSelector("input.qty"), 2);
            }
        }

        public IWebElement CartEmptyMessage => driver.FindElement(By.CssSelector(".cart-empty.woocommerce-info"), 3);
        public IWebElement UpdateCartButton => driver.FindElement(By.CssSelector("[name='update_cart']"), 2);
        public IWebElement GoToCheckoutButton => driver.FindElement(By.CssSelector(".checkout-button"), 2);

        public CartPage(IWebDriver driver) : base(driver)
        {

        }

        public CartPage GoTo()
        {
            driver.Navigate().GoToUrl(CartUrl);
            return this;
        }

        public CartPage RemoveItem(string productId)
        {
            driver.FindElement(By.CssSelector("a[data-product_id='" + productId + "']"), 2).Click();
            WaitForLoadersDisappear();
            return this;
        }
        public CheckoutPage GoToCheckout()
        {
            GoToCheckoutButton.Click();
            return new CheckoutPage(driver);
        }

        private new void WaitForLoadersDisappear()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait.Until(d => driver.FindElements(Loaders).Count == 0);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Elements located by " + Loaders + " didn't disappear in 5 seconds.");
                throw;
            }
        }

        public CartPage ChangeQuantity(int quantity)
        {
            QuantityField.Clear();
            QuantityField.SendKeys(quantity.ToString());
            UpdateCartButton.Click();
            WaitForLoadersDisappear();
            return this;
        }

        public bool IsQuantityFieldRangeOverflowPresent()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript("return arguments[0].validity.rangeOverflow", QuantityField);
        }


    }
}