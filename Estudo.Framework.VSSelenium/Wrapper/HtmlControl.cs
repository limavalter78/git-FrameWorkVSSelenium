using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudo.Framework.VSSelenium.Wrapper
{
    public class HtmlControl
    {
        protected readonly IWebElement webElement;

        public HtmlControl(IWebElement webElement)
        {
            this.webElement = webElement;
        }

        public void Click()
        {
            webElement.ClickCustom();
        }

        public string Text
        {
            get { return webElement.Text; }
        }

        public void Submit()
        {
            webElement.SubmitCustom();
        }

        //All the Find Methods and other methods you want to expose
    }

    public class HtmlEditBox : HtmlControl
    {
        public HtmlEditBox(IWebElement webElement) : base(webElement)
        {
        }

        public new string Text
        {
            get { return webElement.Text; }
            set
            {
                ((IJavaScriptExecutor)((RemoteWebElement)webElement).WrappedDriver).
                ExecuteScript("arguments[0].setAttribute('value', '" + value + "')",
                    webElement);
            }
        }

        public void Clear()
        {
            webElement.ClearCustom();
        }

        public void SendKeysCustom(string keys)
        {
            webElement.SendKeysCustom(keys);
        }
    }

    public class HtmlCheckBox : HtmlControl
    {
        public HtmlCheckBox(IWebElement webElement) : base(webElement)
        {

        }

        public bool IsChecked { get { return webElement.Selected; } }
    }

    public class HtmlHyperlink : HtmlControl
    {
        public HtmlHyperlink(IWebElement webElement) : base(webElement)
        {

        }

        public void Go()
        {
            webElement.SendKeysCustom(Keys.Enter);
        }
    }
}
