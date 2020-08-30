using MPS.Funcao.TestesIntegrados.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Funcao.TestesIntegrados.ScreenShot
{
    public class ScreenshotCustom
    {
        #region Construtor 
        public ScreenshotCustom()
        {
        }
        #endregion

        #region :: Declarations         

        public IWebDriver _WebDriver;

        public IWebDriver WebDriver
        {
            get
            {
                if (_WebDriver is null)
                    _WebDriver = WebDriverFactory.WebDriverFactoryMps.GetDriver();

                return _WebDriver;
            }
        }

        #endregion

        #region :: Ações 
        public void ScreenShot()
        {
            var ss = ((ITakesScreenshot)WebDriver).GetScreenshot();
            bool incluiNomeTeste = false;
            //string CampoMassa = "";

            //try
            //{
            //    CampoMassa = "IncluirNomeTesteNaPastaDeEvidencia";
            //    CampoMassa = Utils.Util.GetTestContext().DataRow[CampoMassa] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassa].ToString();
            //    bool.TryParse(CampoMassa, out incluiNomeTeste);
            //}
            //catch (Exception) { }

            var arquivo = "";
            //if (!incluiNomeTeste)
            //arquivo = string.Format(@"{0} \{1}.png", CriarPasta(), DateTime.Now.Ticks.ToString());
            //else
            arquivo = string.Format(@"{0} \{1}.png", CriarPasta(true), DateTime.Now.Ticks.ToString());

            ss.SaveAsFile(arquivo, ScreenshotImageFormat.Png);
        }
        private string CriarPasta()
        {
            string CampoMassa = "";
            try
            {
                CampoMassa = "NomeTeste";
                CampoMassa = Utils.Util.GetTestContext().DataRow[CampoMassa] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassa].ToString();
                CampoMassa = "_" + CampoMassa;
            }
            catch (Exception) { }

            // TestName é populado no testInitialize 
            string folder = string.Format(@"C:\EvidenciasTestes\{0}\{1}{2}", DateTime.Now.ToShortDateString().Replace("/", "-"), ScreenShotFields.TestName, ScreenShotFields.Token);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        private string CriarPasta(bool incluiNomeTeste = false)
        {
            string CampoMassa = "";
            string _ambiente = "";
            try
            {
                CampoMassa = "NomeTeste";
                CampoMassa = Utils.Util.GetTestContext().DataRow[CampoMassa] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassa].ToString();
                CampoMassa = "_" + CampoMassa;
            }
            catch (Exception)
            {
                CampoMassa = "";
            }

            try
            {
                _ambiente = "Ambiente";
                // _ambiente = Utils.Util.GetTestContext().DataRow[_ambiente] == null ? "" : Utils.Util.GetTestContext().DataRow[_ambiente].ToString();
                string ambiente = Util.GetTestContext().DataRow["Url_Test"].ToString();
                _ambiente = ambiente.Split(':')[1].Split('.')[0].Replace("//", "").ToUpper();
                Util.Ambiente = _ambiente;
                _ambiente = _ambiente + "_";
            }
            catch (Exception)
            {
                _ambiente = "";
            }

            // TestName é populado no testInitialize 
            string folder = string.Format(@"C:\EvidenciasTestes\{0}\{4}{1}{2}{3}", DateTime.Now.ToShortDateString().Replace("/", "-"), ScreenShotFields.TestName, ScreenShotFields.Token, CampoMassa.Replace("/", "-"), _ambiente);

            try
            {
                if (!(Utils.Util.caminhoDiretório.Contains("[Error]")))
                {
                    Utils.Util.caminhoDiretório = folder;
                }
            }
            catch (Exception) { }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Utils.Util.caminhoDiretório = folder;
            }

            if (Utils.Util.caminhoDiretório != "" && (Utils.Util.caminhoDiretório != folder) && Util.renomeiarPasta)
            {
                string caminhoAntigo = folder;
                folder = Utils.Util.caminhoDiretório;

                Directory.Move(caminhoAntigo, folder);
            }

            //if (Util.TesteFinalizado)
            //{
            //    Util.CriaPdfEvidencia(ScreenShotFields.TestName + "\n" + Util.TestName);
            //    Util.TesteFinalizado = false;
            //}

            return folder;
        }
        #endregion
    }
}
