﻿using Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FakestorePageObjects
{
    public class ProductPage : BasePage
    {
        
        private string ProductUrl => baseUrl + "/product";

        private IWebElement AddToCartButton => driver.FindElement(By.CssSelector("[name='add-to-cart']"), 2);
        public IWebElement GoToCartButton => driver.FindElement(By.CssSelector(".woocommerce-message .wc-forward"), 2);

        private IWebElement QuantityField => driver.FindElement(By.CssSelector("input.qty"), 2);

        public int NumberOfProductsInStock
        {
            get
            {
                string stock = driver.FindElement(By.CssSelector("p.in-stock")).Text.Replace(" w magazynie", "");
                int.TryParse(stock, out int stockNumber);
                return stockNumber;
            }
        }

        public ProductPage(IWebDriver driver) : base(driver) { }
       

        public ProductPage GoTo(string productSlug)
        {
            driver.Navigate().GoToUrl(ProductUrl + productSlug);
            return this;
        }

        public ProductPage AddToCart(int quantity = 1)
        {
            if (quantity <= 0)
            {
                QuantityField.Clear();
                QuantityField.SendKeys(quantity.ToString());
                AddToCartButton.Click();

            }
            else if (quantity != 1)
            {
                QuantityField.Clear();
                QuantityField.SendKeys(quantity.ToString());
                AddToCartButton.Click();
                _ = GoToCartButton;

            }
            else
            {
                AddToCartButton.Click();
                _ = GoToCartButton;
            }

            return this;

        }

        public CartPage GoToCart()
        {

            GoToCartButton.Click();
            return new CartPage(driver);
        }

        public bool IsQuantityFieldRangeUnderflowPresent()
        {

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            return (bool)js.ExecuteScript("return arguments[0].validity.rangeUnderflow", QuantityField);
        }
    }
}