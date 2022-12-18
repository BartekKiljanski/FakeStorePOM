using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakestorePageObjects
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected readonly string baseUrl = "https://fakestore.testelka.pl";
        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;

        }

        protected void WaitForLoadersDisappear()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(d => driver.FindElements(By.CssSelector(".blockUI")).Count == 0);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Loaders didn't disappear in 10 seconds.");
                throw;
            }
        }
       

    }
}
