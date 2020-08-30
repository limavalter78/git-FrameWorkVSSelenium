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
    public class ST_AutomationPractice : SuiteBase
    {

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\Massa\\Dados.xml", "Categoria", DataAccessMethod.Sequential), TestMethod]
        [TestCategory("Women")]
        public void CT_PesquisarCategoria_Feminina()
        {

            try
            {

                PageObjects.WomenPO searchProduct = new PageObjects.WomenPO();


                searchProduct.PesquisarProduto();



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
