using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using FakestorePageObjects;
using Helpers;
using NUnit.Allure.Core;

namespace SeleniumTests
{
    [AllureNUnit]
    public class CartTests : BaseTest
    {
       


        IList<string> productsIDs = new List<string>() { "389", "62" };
       
        IList<string> productsURLs = new List<string>() {
            "/wyspy-zielonego-przyladka-sal/",
            "/zmien-swoja-sylwetke-yoga-na-malcie/"
        };

        

        [Test]
        public void ProductAddedToCartTest()
        {

            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage.GoTo(productsURLs[0]).AddToCart().GoToCart();
            
           
            Assert.Multiple(() => {
                Assert.AreEqual(1, cartPage.CartItems.Count, "Number of product in cart is not 1");
                Assert.AreEqual(productsIDs[0], cartPage.ItemId,
                    "Product's in cart id is not " + productsIDs[0]);
            });


        }

        [Test]
        public void TwoItemsOfProductAddedToCartTest()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage.GoTo(productsURLs[0]).AddToCart(2).GoToCart();





          
            Assert.Multiple(() => {
                Assert.AreEqual(1, cartPage.CartItems.Count, "Number of product in cart is not 1");
                Assert.AreEqual(productsIDs[0], cartPage.ItemId,
                    "Product's in cart id is not " + productsIDs[0]);
                Assert.AreEqual("2", cartPage.QuantityField.GetAttribute("value"), "Number of items of the product is not 2");
            });
        }

        [Test]
        public void TwoProductsAddedToCartTest()
        {


            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage.GoTo(productsURLs[0]).AddToCart().GoTo(productsURLs[1]).AddToCart().GoToCart();





           /* driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            AddToCartButton.Click();
            _ = GoToCartButton;
            driver.Navigate().GoToUrl(baseURL + productsURLs[1]);
            AddToCartButton.Click();
            GoToCartButton.Click();*/
            /*_ = CartTable;*/
            Assert.Multiple(() => {
                Assert.AreEqual(2, cartPage.CartItems.Count, "Number of product in cart is not 1");
                Assert.AreEqual(productsIDs[0], cartPage.ItemIds[0],
                    "Product's in cart id is not " + productsIDs[0]);
                Assert.AreEqual(productsIDs[1], cartPage.ItemIds[1],
                    "Product's in cart id is not " + productsIDs[1]);
            });
        }
        [Test]
        public void CartEmptyAtStartTest()
        {

            CartPage cartPage = new CartPage(driver);
            cartPage.GoTo();

            Assert.DoesNotThrow(() => _= cartPage.CartEmptyMessage, " There is no \"Empty Cart\" message");

            /* driver.Navigate().GoToUrl(baseURL + "/koszyk/");*/
            /* Assert.DoesNotThrow(() => driver.FindElement(By.CssSelector(".cart-empty.woocommerce-info")), "There is no \"Empty Cart\" message");
            }
         }*/

        }
        [Test]
        public void CantAddZeroItemsTest()
        {
            ProductPage productPage = new ProductPage(driver);
            productPage.GoTo(productsURLs[0]).AddToCart(0);


            /*  driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
              QuantityField.Clear();
              QuantityField.SendKeys("0");
              AddToCartButton.Click();
           
            bool isNotPositiveNumber = (bool)js.ExecuteScript("return arguments[0].validity.rangeUnderflow", QuantityField);
             */
            Assert.Multiple(() =>
            {
                Assert.IsTrue(productPage.IsQuantityFieldRangeUnderflowPresent(), "Test was probably able to add 0 items to cart. Range Underflow validation didn't return \"true\".");

                CustomAssert.ThrowsWebDriverTimeoutException(() => _ = productPage.GoToCartButton,
                    "\"Go to cart\" link was found, but it shouldn't. Nothing should be added to cart when you try add 0 items.");
            });

             }
        [Test]
        public void CanRemoveProductFromCartTest()
        {
            ProductPage productPage = new ProductPage(driver);
           CartPage cartPage = productPage
                .GoTo(productsURLs[0])
                .AddToCart()
                .GoToCart()
                .RemoveItem(productsIDs[0]);
            





            /* driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
             AddToCartButton.Click();
             GoToCartButton.Click();
             _ = CartTable;
             CartItems[0].FindElement(By.CssSelector("a[data-product_id='" + productsIDs[0] + "']")).Click();
             waitForElementsDisappear(Loaders);*/

            Assert.DoesNotThrow(() => _= cartPage.CartEmptyMessage, "There is no \"Empty Cart\" message. Product was not removed from cart.");
        }
        [Test]
        public void CanIncreaseNumberOfItemsTest()
        {

            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage.GoTo(productsURLs[0]).AddToCart().GoToCart().ChangeQuantity(5);
            /*driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            AddToCartButton.Click();
            GoToCartButton.Click();
            _ = CartTable;
            QuantityField.Clear();
            QuantityField.SendKeys("5");
            UpdateCartButton.Click();
            waitForElementsDisappear(Loaders);*/
            Assert.AreEqual("5", cartPage.QuantityField.GetAttribute("value"), "Number of items didn't change");
        }
        [Test]
        public void ChangingNumberOfItemsToZeroRemovesProductTest()
        {


            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage.GoTo(productsURLs[0]).AddToCart().GoToCart().ChangeQuantity(0);

            Assert.DoesNotThrow(() => _ = cartPage.CartEmptyMessage,
               "There is no \"Empty Cart\" message. Product was not removed from cart.");
            /* driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
             AddToCartButton.Click();
             GoToCartButton.Click();
             _ = CartTable;
             QuantityField.Clear();
             QuantityField.SendKeys("0");
             UpdateCartButton.Click();
             waitForElementsDisappear(Loaders);
             Assert.DoesNotThrow(() => driver.FindElement(By.CssSelector(".cart-empty.woocommerce-info")), "There is no \"Empty Cart\" message. Product was not removed from cart.");
            */
        }
        [Test]
        public void CantChangeToMoreThanStockTest()
        {

            ProductPage productPage = new ProductPage(driver);
            int stockNumber = productPage.GoTo(productsURLs[0]).NumberOfProductsInStock;
            CartPage cartPage = productPage.AddToCart().GoToCart().ChangeQuantity(stockNumber + 1);
            Assert.IsTrue(cartPage.IsQuantityFieldRangeOverflowPresent(),
                "Test was probably able to add more items than available in stock.Range Overflow validation didn't return \"true\".");
            /*driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            string stock = driver.FindElement(By.CssSelector("p.in-stock")).Text.Replace(" w magazynie", "");
            int.TryParse(stock, out int stockNumber);
            AddToCartButton.Click();
            GoToCartButton.Click();
            _ = CartTable;
            QuantityField.Clear();
            QuantityField.SendKeys((stockNumber + 1).ToString());
            UpdateCartButton.Click();
            waitForElementsDisappear(Loaders);
            bool isMoreThanStock = (bool)js.ExecuteScript("return arguments[0].validity.rangeOverflow", QuantityField);
            Assert.IsTrue(isMoreThanStock, "Test was probably able to add more items than available in stock. Range Overflow validation didn't return \"true\".");

            */
        }


     
    }
}