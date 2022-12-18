using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace FakestorePageObjects
{
    public class SuccessfulOrderPage : BasePage
    {
      

        public SuccessfulOrderPage(IWebDriver driver) : base(driver) { }
       

        public IWebElement EntryHeader => driver.FindElement(By.CssSelector(".entry-header"), 2);
    }
}
