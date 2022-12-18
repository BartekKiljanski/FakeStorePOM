using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class TestHelper
    {

        public static void DoOnTimeout(TestDelegate throwsTimeout, TestDelegate catchAction)
        {
            try { throwsTimeout(); }
            catch (WebDriverTimeoutException) { catchAction(); }
        }
    }
}
