using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Estudo.Framework.VSSelenium.Utils;

namespace Estudo.Framework.VSSelenium.PageObjects
{
    //Herdar esta classe em todos PageObjects 
    public abstract class PageObjectsBase
    {
        #region :: Construtor 
        public PageObjectsBase()
        {
            PageFactory.InitElements(WebDriver, this);
        }
        #endregion

        #region :: Declarations 
        private IWebDriver _WebDriver;

        protected IWebDriver WebDriver
        {
            get
            {
                if (_WebDriver is null)
                    _WebDriver = WebDriverFactory.WebDriverFactoryVS.GetDriver();

                return _WebDriver;
            }
        }

        protected WebDriverWait Wait
        {
            get
            {
                return new WebDriverWait(WebDriver, TimeSpan.FromSeconds(200));
            }
        }

        protected Decrypt Decrypt
        {
            get
            {
                if (this.mDecrypt == null)
                    mDecrypt = new Decrypt();

                return this.mDecrypt;
            }
        }

        private Decrypt mDecrypt;
        #endregion

        #region :: Ações 

        /// <summary>
        /// Aguarda que um elemento desapareça da tela
        /// </summary>
        /// <param name="acao">By com a condição de busca do elemento</param>
        protected void Loading(By acao)
        {
            Thread.Sleep(1000);
            Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(acao));
        }

        /// <summary>
        /// Aguarda que um elemento seja clicavel 
        /// </summary>
        /// <param name="acao">By com a condição de busca do elemento</param>
        protected void WaitElement(By acao)
        {
            Wait.Until(ExpectedConditions.ElementToBeClickable(acao));
        }
        #endregion
    }
}
