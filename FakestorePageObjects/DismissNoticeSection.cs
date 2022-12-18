using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakestorePageObjects
{
    public class DismissNoticeSection : BasePage
    {
        public DismissNoticeSection(IWebDriver driver) : base(driver)
        { }

             public IWebElement DismissNoticeLink => driver.FindElement(By.CssSelector(".woocommerce-store-notice__dismiss-link"));

        public void DismissNotice()
        {
            DismissNoticeLink.Click();
        }
        
    }
    
}
