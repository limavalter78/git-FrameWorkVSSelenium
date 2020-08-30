using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.IO;
using Microsoft.VisualStudio.TestTools.UITesting;
using System.Threading;

namespace Estudo.Framework.VSSelenium.Suite
{
    [TestClass]
    public class ST_Orgao : SuiteBase
    {

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\Massa\\Dados.xml", "Orgao", DataAccessMethod.Sequential)]
        [TestCategory("Orgao")]
        public void CT_FB_CRUD_Orgao()
        {


            try
            {

                PageObjects.LoginPO chamadaLogar = new PageObjects.LoginPO();
                PageObjects.MenuPO chamadaMenu = new PageObjects.MenuPO();


                //chamadaLogar.VerificaSiteNaoSeguro();

                chamadaLogar.Login_SSO();
                chamadaLogar.Login_Simula();



                chamadaLogar.SairSistema();



                #region Add Rotulo [Erro] Na pasta de evidência
            }
            catch (Exception)
            {
                //Este tratamento marca o teste que deu errado, la na pasta de evidencia.
                Utils.Util.FinalizaEvidencia(true);
                throw;
            }
            finally { Utils.Util.metodoConfigurado = true; }

            #endregion

        }


    }
}
