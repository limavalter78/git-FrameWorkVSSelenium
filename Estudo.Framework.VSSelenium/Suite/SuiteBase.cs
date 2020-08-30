using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudo.Framework.VSSelenium.ScreenShot;
using Estudo.Framework.VSSelenium.Utils;
using Estudo.Framework.VSSelenium.WebDriverFactory;
using System.IO;
using System.Diagnostics;

namespace Estudo.Framework.VSSelenium.Suite
{
    //Herdar esta classe em todos Suites
    public abstract class SuiteBase
    {

        #region :: Declarações      



        public TestContext TestContext
        {
            get => testContextInstance;
            set
            {
                testContextInstance = value;
                Util.SetTestContext(value);


            }
        }



        private TestContext testContextInstance;

        #endregion

        #region :: Métodos pré e pós execução. 

        [TestInitialize()]
        public virtual void TesteInit()
        {
            //Armazena o nome do teste e token para o screenshot
            ScreenShotFields.Token = DateTime.Now.ToLongTimeString().Replace(":", "");
            ScreenShotFields.TestName = TestContext.TestName;
            var a = testContextInstance.Properties;
            WebDriverFactoryVS.AbrirNavegador(Utils.Environment.RetornarUrl());
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// THIAGO 

        //[TestInitialize()]
        //public virtual void TesteInit()
        //{
        //    //Armazena o nome do teste e token para o screenshot
        //    WebDriverFactoryVS.AbrirNavegador(Environment.RetornarUrl());
        //    var browser = WebDriverFactory.GetDriver();
        //    Evidencia.Iniciar(browser);
        //    Evidencia.ParametrizaEvidencia("Pré-condições de teste", "0", nomeArquivo: TestContext.TestName, diretorio: TestContext.FullyQualifiedTestClassName);
        //}

        //public void Logar(string usuario = null)
        //{
        //    var browser = WebDriverFactory.GetDriver();
        //    if (browser.Url.ToUpper().Contains("/RHF/") && browser.Url.ToUpper().Contains("/LOGIN/"))
        //        new LoginPO(browser).RealizarLogin(usuario);
        //    Thread.Sleep(3000);
        //}

        ///// //////////////////////////////////////////////////////////////////////////////////////////////////////////////


        [TestCleanup()]
        public void TestFinish()
        {
            try
            {
                new ScreenshotCustom().ScreenShot();

            }
            catch (Exception)
            {


            }
            WebDriverFactory.WebDriverFactoryVS.Close();

            string folder = Util.caminhoDiretório;
            string ambiente = Util.Ambiente;
            using (PDF pdf = new PDF())
            {
                pdf.CriaPdfEvidencia(ScreenShotFields.TestName + "\n" + Util.TestName, ambiente);
            }

        }
        #endregion










    }
}
