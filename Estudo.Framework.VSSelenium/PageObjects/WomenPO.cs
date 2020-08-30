using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.InteropServices;
using Estudo.Framework.VSSelenium.Wrapper;
using Estudo.Framework.VSSelenium.ScreenShot;

namespace Estudo.Framework.VSSelenium.PageObjects
{
    [TestClass]
   public class WomenPO : PageObjects.PageObjectsBase
    {


        #region: Mapeamento Object

        private IWebElement Pesquisar => WebDriver.FindElement(By.Id("search_query_top"));
        private IWebElement BtnBuscar => WebDriver.FindElement(By.Name("submit_search"));

        private IWebElement ProdutoEncontrado => WebDriver.FindElement(By.ClassName("page-heading  product-listing"));

        #endregion




        [TestMethod]
        public void PesquisarProduto()
        {
            //Aguarda apresentação do campo usuario
            WaitElement(By.Id("search_query_top"));

            Pesquisar.SendKeys(Utils.Util.GetTestContext().DataRow["Produto"].ToString());
            BtnBuscar.ClickCustom();

            WaitElement(By.ClassName("page-heading  product-listing"));
            ProdutoEncontrado.ClickCustom();
        }

    }
}
