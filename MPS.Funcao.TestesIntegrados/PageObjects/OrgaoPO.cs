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
using MPS.Funcao.TestesIntegrados.Wrapper;
using MPS.Funcao.TestesIntegrados.ScreenShot;
namespace MPS.Funcao.TestesIntegrados.PageObjects
{
    [TestClass]
    public class OrgaoPO : PageObjects.PageObjectsBase
    {


        #region: Mapeamento Cadastro Órgão

        private IWebElement Orgao => WebDriver.FindElement(By.Id(""));
        private IWebElement PeriodoVigencia_Inicio => WebDriver.FindElement(By.Id(""));
        private IWebElement PeriodoVigencia_Fim => WebDriver.FindElement(By.Id(""));
        private IWebElement DataPlenaria => WebDriver.FindElement(By.Id(""));
        private IWebElement DataPublicacao => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnVoltar => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnLimparCadastro => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnSalvar => WebDriver.FindElement(By.Id(""));

        private IWebElement ValidaMensagem => WebDriver.FindElement(By.ClassName("toast-message"));

        #endregion

        #region: Mapeamento Pesquisa órgão

        private IWebElement BtnNovo => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnLimparPesquisa => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnPesquisar => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnExportar => WebDriver.FindElement(By.Id(""));
        private IWebElement BtnPaginaInicial => WebDriver.FindElement(By.Id(""));
     


        #endregion



    }
}
