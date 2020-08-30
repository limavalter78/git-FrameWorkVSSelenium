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
    public class LoginPO : PageObjects.PageObjectsBase
    {


        #region: Mapeamento Login Simulação

        //[FindsBy(How = How.Id, Using = "Usuario")]
        //public IWebElement Usuario_SSO;

        private IWebElement Usuario_SSO => WebDriver.FindElement(By.Id("Usuario"));
        private IWebElement Usuario_Simula => WebDriver.FindElement(By.Id("UsuarioSimulacao"));
        private IWebElement Senha_SSO => WebDriver.FindElement(By.Id("Senha"));
        private IWebElement Senha_Simula => WebDriver.FindElement(By.Id("SenhaSimulacao"));
        private IWebElement Matricula_Simula => WebDriver.FindElement(By.Id("MatriculaSimulado"));
        private IWebElement BtnEntrar_SSO => WebDriver.FindElement(By.XPath("//button"));
        private IWebElement BtnEntrar_Simula => WebDriver.FindElement(By.TagName("button"));
        private IWebElement BtnFecharBarra_Ambiente => WebDriver.FindElement(By.Id("fechaBarra"));
        private IWebElement TelaPrincipalDesignacao => WebDriver.FindElement(By.Id("58cddcec-c660-4295-8cfe-4b902a227af9"));


        private IWebElement Logouf => WebDriver.FindElement(By.XPath("//*[@id='navbarDropdown']/span"));
        private IWebElement MenuPerfil => WebDriver.FindElement(By.XPath("//div[@aria-labelledby='navbarDropdown']"));
        private IWebElement Sair => WebDriver.FindElement(By.LinkText("Sair"));


        private IWebElement MaisInfo => WebDriver.FindElement(By.Id("moreInfoContainer"));
        private IWebElement Continuar => WebDriver.FindElement(By.Id("overridelink"));


        private IWebElement BtnAdvancedFirefox => WebDriver.FindElement(By.Id("advanced_button"));
        private IWebElement BtnContinuarFirefox => WebDriver.FindElement(By.Id("exception_button"));

        #endregion




        [TestMethod]
        public void Login_SSO(string C = "")
        {
            //Aguarda apresentação do campo usuario
            WaitElement(By.Id("Usuario"));

            Usuario_SSO.SendKeys(C + Utils.Util.GetTestContext().DataRow["UsuarioSSO"].ToString());
            Senha_SSO.SendKeys(C + Utils.Util.GetTestContext().DataRow["SenhaSSO"].ToString());

            BtnEntrar_SSO.Click();

        }


        [TestMethod]
        public void Login_Simula(string C = "")
        {

            WaitElement(By.Id("UsuarioSimulacao"));

            Usuario_Simula.SendKeys(Utils.Util.GetTestContext().DataRow[C + "UsuarioADM"].ToString());
            Senha_Simula.SendKeys(Utils.Util.GetTestContext().DataRow[C + "SenhaADM"].ToString());
            Matricula_Simula.SendKeys(Utils.Util.GetTestContext().DataRow[C + "Matricula"].ToString());

            BtnEntrar_Simula.ClickCustom();

            WaitElement(By.Id("58cddcec-c660-4295-8cfe-4b902a227af9"));



        }


        [TestMethod]
        public void SairSistema()
        {
            Thread.Sleep(50);
            Utils.Util.RolaTela_ParaCima(WebDriver);
            Thread.Sleep(50);
            Logouf.Click();
            Thread.Sleep(50);
            Sair.Click();
            Thread.Sleep(50);

        }



        public void VerificaSiteNaoSeguro()
        {
            try
            {
                MaisInfo.Click();
                Thread.Sleep(200);
                Continuar.Click();
                Thread.Sleep(200);
            }
            catch (Exception)
            {

            }
        }

    }


}
