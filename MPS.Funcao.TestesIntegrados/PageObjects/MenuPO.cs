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
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Runtime.InteropServices;
using MPS.Funcao.TestesIntegrados.Wrapper;
using MPS.Funcao.TestesIntegrados.Utils;
using MPS.Funcao.TestesIntegrados.ScreenShot;
using System.Diagnostics;

namespace MPS.Funcao.TestesIntegrados.PageObjects
{
    [TestClass]
    public class MenuPO : PageObjects.PageObjectsBase
    {



        #region: Mapemento menu


        private IWebElement MenuTabelaApoio => WebDriver.FindElement(By.Id("ca7195de-8591-4114-84a2-f192e8835836"));




        #endregion



        #region: Action


        [TestMethod]
        public void AcessarCadastroOrgao()
        {

            //MenuDesignacao.Click();
            //Thread.Sleep(50);
            //SubMenuCadastroDesignacao.ClickCustom();
            //WaitElement(By.Id("autocomplete"));


        }



        #endregion


    }
}
