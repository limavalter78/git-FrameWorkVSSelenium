using Estudo.Framework.VSSelenium.Extensoes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Estudo.Framework.VSSelenium.Evidencias
{
	public enum TipoEvidencias
    {
        //	ULTIMA,PADRÃO,FALHA
        PADRÃO_SEMPRE, ULTIMO_SCREENSHOT, SEM_SCREENSHOT, FALHA_FINALIZA, FALHA_CONTINUA
    }
    public enum FormatoEvidencia
    {
        //	HTML,DOC,EXCEL,PDF...
        HTML
    }

    public class LogEvidencias
    {
        IWebDriver driver;
        private List<EvidenciaDTO> stepList = new List<EvidenciaDTO>();

        public LogEvidencias(IWebDriver driver)
        {
            this.driver = driver;
        }

        public List<EvidenciaDTO> GetEvidencias()
        {
            return new List<EvidenciaDTO>(stepList);
        }

        public EvidenciaDTO GetEvidencia(int index)
        {
            return stepList[index];
        }

        public void GerarEvidencia(string mensagem, string valorEsperado, string valorObtido, bool status)
        {
            try
            {
                string imagem = CapturaTela(driver);
                string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                stepList.Add(new EvidenciaDTO(timestamp, mensagem, valorEsperado, valorObtido, status, imagem));
            }
            catch { Console.WriteLine("Ocorreu um erro ao salvar uma evidencia"); }
        }

        public void GerarEvidenciaElement(IWebElement webElement, string mensagem, string valorEsperado, string valorObtido, bool status)
        {
            try
            {
                webElement.SetHighLight((status ? "forestgreen" : "red"));
                GerarEvidencia(mensagem, valorEsperado, valorObtido, status);
                webElement.UnsetHighLight();
            }
            catch { Console.WriteLine("Ocorreu um erro ao salvar uma evidencia"); }
        }

        private static string CapturaTela(IWebDriver driver)
        {
            try
            {
                ITakesScreenshot newScreen = (ITakesScreenshot)driver;
                string scnShot = newScreen.GetScreenshot().AsBase64EncodedString;
                return "data:image/jpg;base64, " + scnShot;
            }
            catch
            {
                Console.WriteLine("Ocorreu um erro ao realizar a captura de tela");
                return "";
            }
        }
    }

    public class EvidenciaDTO
    {
        public readonly string DataHora;
        public readonly string ValidacaoStep;
        public readonly string Esperado;
        public readonly string Obtido;
        public readonly bool Status;
        public readonly string ImageAsBase64;

        public EvidenciaDTO(string dataHora, string validacaoStep, string esperado, string obtido, bool status, string imageAsBase64)
        {
            this.DataHora = dataHora;
            this.ValidacaoStep = validacaoStep;
            this.Esperado = esperado;
            this.Obtido = obtido;
            this.Status = status;
            this.ImageAsBase64 = imageAsBase64;
        }
    }
}
