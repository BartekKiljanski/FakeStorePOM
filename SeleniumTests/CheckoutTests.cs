using FakestorePageObjects;
using Helpers;
using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTests
{
    [AllureNUnit]
    public class CheckoutTests : BaseTest
    {
        
        IList<string> productsURLs = new List<string>() {
            "/product/wyspy-zielonego-przyladka-sal/",
            "/product/zmien-swoja-sylwetke-yoga-na-malcie/"
        };
        IList<string> productsPricesText = new List<string>() {
            "5 399,00 zł",
            "3 299,00 zł"
        };

        IList<int> productsPrices = new List<int>() {
            5399,
            3299
        };
        By Loaders => By.CssSelector(".blockUI");

        string cardNumber = "4242424242424242";
        string cardExpirationDate = "0223";
        string cardCvc = "223";
      


        [Test]
        public void FieldsValidationTests()
        {

            ProductPage productPage = new ProductPage(driver);
            CheckoutPage checkoutPage = productPage.
                GoTo(productsURLs[0])
                .AddToCart()
                .GoToCart()
                .GoToCheckout()
                .FillInCardData(cardNumber, cardExpirationDate, cardCvc)
                .PlaceOrder<CheckoutPage>();




           /* driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            AddToCartButton.Click();
            GoToCartButton.Click();
            GoToCheckoutButton.Click();
            _ = CheckoutForm;
            driver.SwitchTo().Frame(CardNumberFrame);
            CardNumberInput.SendKeys(cardNumber);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(CardExpirationDateFrame);
            CardExpirationDateInput.SendKeys(cardExpirationDate);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(CardCvcFrame);
            CardCvcInput.SendKeys(cardCvc);
            driver.SwitchTo().DefaultContent();
            PlaceOrderButton.Click();*/

            IList<string> errorMessages = checkoutPage.ErrorMessagesElements.Select(element => element.Text).ToList();
            IList<string> expectedErrorMessages = new List<string> {
                "Imię płatnika jest wymaganym polem.",
                "Nazwisko płatnika jest wymaganym polem.",
                "Ulica płatnika jest wymaganym polem.",
                "Kod pocztowy płatnika nie jest prawidłowym kodem pocztowym.",
                "Miasto płatnika jest wymaganym polem.",
                "Telefon płatnika jest wymaganym polem.",
                "Adres email płatnika jest wymaganym polem.",
                "Proszę przeczytać i zaakceptować regulamin sklepu aby móc sfinalizować zamówienie."
            };
            
            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => _ = checkoutPage.ErrorsList, "Error list was not found. There was no validation error.");
                //Assert.AreEqual(8, ErrorMessagesElements.Count, "Number of error messages is not correct.");
                Assert.AreEqual(expectedErrorMessages.OrderBy(message => message), errorMessages.OrderBy(message => message));
            });
        }


        [Test]
        public void ReviewOrderOneProductTest()
        {



            ProductPage productPage = new ProductPage(driver);
            CheckoutPage checkoutPage = productPage.
                GoTo(productsURLs[0])
                .AddToCart()
                .GoToCart()
                .GoToCheckout();



          /*  driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            AddToCartButton.Click();
            GoToCartButton.Click();
            GoToCheckoutButton.Click();
            _ = CheckoutForm;
            waitForElementsDisappear(Loaders);*/
            float tax = CalculateTax(productsPrices[0]);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(productsPricesText[0], checkoutPage.ProductTotalElement.Text);
                Assert.AreEqual(productsPricesText[0], checkoutPage.CartSubtotalElement.Text);
                Assert.AreEqual(productsPricesText[0] + " (zawiera " + FormatNumber(tax) + " VAT)", checkoutPage.OrderTotalElement.Text);
            });
        }

        [Test]
        public void ReviewOrderMultipleProductsTest()
        {


            ProductPage productPage = new ProductPage(driver);
            CheckoutPage checkoutPage = productPage.
                GoTo(productsURLs[0])
                .AddToCart(2)
                .GoTo(productsURLs[1])
                .AddToCart(3)
                .GoToCart()
                .GoToCheckout();


           /* driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            QuantityField.Clear();
            QuantityField.SendKeys("2");
            AddToCartButton.Click();
            _ = GoToCartButton;
            driver.Navigate().GoToUrl(baseURL + productsURLs[1]);
            QuantityField.Clear();
            QuantityField.SendKeys("3");
            AddToCartButton.Click();
            GoToCartButton.Click();
            GoToCheckoutButton.Click();
            _ = CheckoutForm;

            waitForElementsDisappear(Loaders);*/
            float totalPrice = productsPrices[0] * 2 + productsPrices[1] * 3;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(FormatNumber(productsPrices[0] * 2), checkoutPage.ProductTotalElements[0].Text);
                Assert.AreEqual(FormatNumber(productsPrices[1] * 3), checkoutPage.ProductTotalElements[1].Text);
                Assert.AreEqual(FormatNumber(totalPrice), checkoutPage.CartSubtotalElement.Text);
                Assert.AreEqual(FormatNumber(totalPrice) + " (zawiera " + FormatNumber(CalculateTax(totalPrice)) + " VAT)",
                    checkoutPage.OrderTotalElement.Text);
            });
        }
        [Test]
        public void ChangingNumberOfItemsTest()
        {
            ProductPage productPage = new ProductPage(driver);
            CheckoutPage checkoutPage = productPage.
                GoTo(productsURLs[0])
                .AddToCart().GoToCart().ChangeQuantity(2).GoToCheckout();




         /*
            driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            AddToCartButton.Click();
            GoToCartButton.Click();
            _ = CartTable;
            QuantityField.Clear();
            QuantityField.SendKeys("2");
            UpdateCartButton.Click();
            waitForElementsDisappear(Loaders);

            GoToCheckoutButton.Click();*/

            //Chrome issue with clicking on the button
            if (browser == "chrome")
            {
                try
                {
                    _ = checkoutPage.CheckoutForm;
                }
                catch (WebDriverTimeoutException)
                {
                   new CartPage(driver).GoToCheckout();
                    _ = checkoutPage.CheckoutForm;
                }
            }
            else _ = checkoutPage.CheckoutForm;

            float total = productsPrices[0] * 2;
            float tax = CalculateTax(total);
           
            Assert.Multiple(() =>
            {
                Assert.AreEqual(FormatNumber(total), checkoutPage.ProductTotalElement.Text);
                Assert.AreEqual(FormatNumber(total), checkoutPage.CartSubtotalElement.Text);
                Assert.AreEqual(FormatNumber(total) + " (zawiera " + FormatNumber(tax) + " VAT)", checkoutPage.OrderTotalElement.Text);
            });
        }


        [Test]
        public void SuccesfullPaymentTest()
        {


            ProductPage productPage = new ProductPage(driver);
            SuccessfulOrderPage successfulOrderPage = productPage.
                GoTo(productsURLs[0])
                .AddToCart()
                .GoToCart()
                .GoToCheckout()
                .FillInCardData(cardNumber, cardExpirationDate, cardCvc)
                  .FillInUserData("Karolina", "Kowalska", "ul. Kwiatowa 12/44", "31-333", "Kraków", "6666666", "hoho@hohoho.pl")
                .CheckTerms()
                .PlaceOrder<SuccessfulOrderPage>();

          /*  driver.Navigate().GoToUrl(baseURL + productsURLs[0]);
            AddToCartButton.Click();
            GoToCartButton.Click();
            GoToCheckoutButton.Click();
            _ = CheckoutForm;
            driver.SwitchTo().Frame(CardNumberFrame);
            CardNumberInput.SendKeys(cardNumber);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(CardExpirationDateFrame);
            CardExpirationDateInput.SendKeys(cardExpirationDate);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(CardCvcFrame);
            CardCvcInput.SendKeys(cardCvc);
            driver.SwitchTo().DefaultContent();*/

           /* BillingFirstNameInput.SendKeys("Karolina");
            BillingLastNameInput.SendKeys("Kowalska");
            BillingAddressInput.SendKeys("ul. Kwiatowa 12/44");
            BillingPostcodeInput.SendKeys("31-333");
            BillingCityInput.SendKeys("Kraków");
            BillingPhoneInput.SendKeys("6666666");
            BillingEmailInput.SendKeys("hoho@hohoho.pl");
            waitForElementsDisappear(Loaders);
            BillingTermsCheckbox.Click();
            PlaceOrderButton.Click();
            waitForElementsDisappear(Loaders);*/

            Assert.AreEqual("Zamówienie otrzymane", successfulOrderPage.EntryHeader.Text, "Page header is not what expected. Order was not sucessful.");
        }
        private float CalculateTax(float total)
        {
            return (float)Math.Round(total - (total / 1.23), 2);
        }

        private string FormatNumber(float number)
        {
            return string.Format("{0:### ###.00}", number) + " zł";
        }
    }

}
