using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudo.Framework.VSSelenium.Evidencias;
using Estudo.Framework.VSSelenium.Extensoes;
using OpenQA.Selenium;

namespace Estudo.Framework.VSSelenium.Utils
{
    public class MessageUtils
    {

        /// <summary>
        /// Valida mensagem de alerta via TOAST
        /// </summary>
        /// <param name="webDriver">Instancia do driver</param>
        /// <param name="mensagemEsperada">Mensagem esperada</param>
        public static void ValidarMensagemAlertas(IWebDriver webDriver, string mensagemEsperada, bool isContains = false, bool FecharToast = true)
        {
            var toast = webDriver.FindElement(By.Id("toast-container"));
            var mensagemObtida = toast.FindElement(By.ClassName("toast-message")).Text;

            Console.WriteLine("--- Validar Mensagens de Alerta ---");
            Console.WriteLine("Esperado: " + mensagemEsperada);
            Console.WriteLine("Obtido: " + mensagemObtida);
            Console.WriteLine("");

            toast.SetHighLight();
            if (isContains)
                Ensure.StringContains(mensagemObtida, mensagemEsperada, "Validar mensagem Toast (contendo o trecho desejado)");
            else
                Ensure.AreEquals(mensagemEsperada, mensagemObtida, "Validar mensagem Toast (exatamente igual ao esperado)");
            toast.UnsetHighLight();
            if (FecharToast)
                FecharMensagemAlertas(toast);
        }

        /// <summary>
        /// Valida mensagem de span embaixo do campo
        /// </summary>
        /// <param name="htmlSpan">Mapear o span</param>
        /// <param name="msgEsperada">Mensagem esperada</param>
        public static void ValidarMsgSpanCampoObrigatorio(IWebElement htmlSpan, string msgEsperada)
        {

            Ensure.IsTrue(htmlSpan.Displayed, "Mensagem de campo não está visivel.");
            Ensure.IsTrue(htmlSpan.Enabled && htmlSpan.Displayed, "Mensagem de campo obrigatório não foi exibida");
            Ensure.AreEquals(msgEsperada, htmlSpan.Text.Trim(), "Mensagem de campo não é igual a mensagem esperada para este campo");
        }


        public static void FecharMensagemAlertas(IWebElement msgToastDIV)
        {
            try
            {
                msgToastDIV.FindElement(By.ClassName("toast-close-button")).Click();
            }
            catch (Exception) { }
        }

        public static void FecharMensagemAlertas(IWebDriver webDriver)
        {
            try
            {
                var toast = webDriver.FindElement(By.Id("toast-container"));
                toast.FindElement(By.ClassName("toast-close-button")).Click();
            }
            catch (Exception) { }
        }


}
}
