using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudo.Framework.VSSelenium.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Estudo.Framework.VSSelenium.ScreenShot;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using PdfSharp.Drawing.Layout;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenQA.Selenium.Interactions;

namespace Estudo.Framework.VSSelenium.Utils
{
    public static class Util
    {
        //public static string caminhoDiretório = string.Empty;
        private static string _caminhoDiretório;

     

        public static string caminhoDiretório
        {
            get { return _caminhoDiretório; }
            set { _caminhoDiretório = value; }
        }
        public static string Ambiente = "";
        public static bool renomeiarPasta = false;
        public static bool metodoConfigurado = false;

        /// <summary>
        /// Retorna texto da tabela 
        /// </summary>
        /// <param name="linha">int indice linha</param>
        /// <param name="coluna">int indice coluna</param>
        /// <param name="elementoTabela">HtmlTable tabela</param>
        /// <returns>string texto tabela</returns>
        public static string RetornarTextoTabela(int linha, int coluna, HtmlTable elementoTabela)
        {
            //Retorna Conteudo da Linha x Coluna
            return elementoTabela.GetCell(linha, coluna).WebElement.Text;
        }

        /// <summary>
        /// Acessa os menus dinamicamente, baseado nos indices
        /// </summary>
        /// <param name="indexsAcesso">list de int indices de acesso</param>
        /// <param name="menu">IwebElement menu</param>
        public static void AcessarMenu(List<int> indexsAcesso, IWebElement menu)
        {
            var listaMmenu = menu.FindElements(By.CssSelector("li"));

            for (int i = 0; i < indexsAcesso.Count; i++)
            {
                listaMmenu.ElementAt(indexsAcesso[i]).ClickCustom();

                //Se for o ultimo ciclo
                if (i == indexsAcesso.Count - 1)
                    continue;

                listaMmenu = listaMmenu.ElementAt(indexsAcesso[i]).FindElements(By.CssSelector("li"));
            }
        }

        public static void SelecionarOpcaoCombobox(IWebElement elemento, String opcao)
        {
            if (String.IsNullOrEmpty(opcao))
                throw new ArgumentException("Você esqueceu de apontar a opção desejada", nameof(opcao));

            SelectElement select = new SelectElement(elemento);

            try
            {
                select.SelectByText(opcao);
                //Ensure.AreEquals(opcao, select.SelectedOption.Text, "Validação ao selecionar item (" + opcao + ") na lista de opções");
                Assert.AreEqual(opcao, select.SelectedOption.Text, "Validação ao selecionar item (" + opcao + ") na lista de opções");
            }
            catch (NotFoundException nfe)
            {
                //Ensure.Fail("Selecionar uma opção [" + string.Join(", ", select.Options) + "]", "Opção " + opcao + "deve estar contido na lista de opções", "Item selecionado (" + opcao + ") não existe nas opções");
                Assert.Fail("Selecionar uma opção [" + string.Join(", ", select.Options) + "]", "Opção " + opcao + "deve estar contido na lista de opções", "Item selecionado (" + opcao + ") não existe nas opções");
                throw new Exception(nfe.Message, nfe.InnerException);
            }
        }

        public static void RolaTela_ParaBaixo(IWebDriver WebDriver, string qtd = "1000")
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            js.ExecuteScript("window.scrollBy(0," + qtd + ")", "");
        }

        public static void RolaTela_ParaCima(IWebDriver WebDriver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            js.ExecuteScript("window.scrollBy(0,-2000)", "");
        }

        private static TestContext testContext;

        public static TestContext GetTestContext()
        {
            return testContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ListaObjMsg"></param>
        /// <param name="CampoMassa"></param>
        /// <param name="WebDriver"></param>
        /// <param name="tipo"></param>
        /// <param name="msgError"></param>
        /// <returns></returns>
        public static bool MsgObrigatorias(Dictionary<string, string> _ListaObjMsg, string CampoMassa, IWebDriver WebDriver, TipoComparacao tipo, out string msgError)
        {
            bool result = true;
            string _CM = "";
            string MsgCampo = "";
            IWebElement Obj;
            string MsgEsperada = "";
            msgError = "";

            try
            {
                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                List<string> LstCamposSub = _CM.Split(';').ToList();
                if ((LstCamposSub.Count > 0) && (LstCamposSub[0] != ""))
                {
                    foreach (string item in LstCamposSub)
                    {
                        string[] _i = item.Split(':');
                        MsgEsperada = item.Split(':')[1];
                        if (_ListaObjMsg.ContainsKey(item.Split(':')[0]))
                        {
                            //MsgCampo = WebDriver.FindElement(By.Id(_ListaObjMsg[_i[0]])).Text;
                            Obj = WebDriver.FindElement(By.Id(_ListaObjMsg[_i[0]]));
                            MsgCampo = Obj.Text;
                            //if (NmCampo == _i[0])
                            //{
                            if (_i.Count() > 2)
                            {
                                if (item.Split(':')[2] == "+") { RolaTela_ParaCima(WebDriver); }

                                if (item.Split(':')[2] == "-") { RolaTela_ParaBaixo(WebDriver); }
                            }

                            if (tipo.ToString() == TipoComparacao.exata.ToString())
                            {
                                Assert.AreEqual(MsgEsperada, MsgCampo);
                                Obj.ClickCustom();
                            }
                            else if (tipo.ToString() == TipoComparacao.contem.ToString())
                            {
                                Assert.IsTrue(MsgCampo.Contains(MsgEsperada));
                                Obj.ClickCustom();
                            }
                            //}
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao validar mensagem obrigatória esperada ['{1}'] no campo esta ['{2}']\n\n Mensagem de Erro: {3}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], MsgEsperada, MsgCampo, ex.Message);

                result = false;
            }


            return result;
        }

        internal static void FinalizaEvidencia(bool RenomeiaPasta)
        {
            metodoConfigurado = true;
            renomeiarPasta = RenomeiaPasta;
            if (caminhoDiretório == null)
            {
                caminhoDiretório = "teste\\teste\\teste\\teste";
            }
            //Este tratamento marca o teste que deu errado, la na pasta de evidencia.
            string CaminhoAntigo = caminhoDiretório;
            string Pasta = CaminhoAntigo.Split('\\')[3];
            string CaminhoNovo = string.Format("{0}\\{1}\\{2}\\[Error]{3}", CaminhoAntigo.Split('\\')[0], CaminhoAntigo.Split('\\')[1], CaminhoAntigo.Split('\\')[2], Pasta);
            caminhoDiretório = CaminhoNovo;
        }

        public static bool Digitar_CampoData(IWebElement ObjDigitar, DateTime Data, out string msgError, IWebDriver WebDriver, string CampoMassaAddDias = "", DepoisDeDigitar tecla = DepoisDeDigitar.Nada)
        {
            bool result = true;
            DateTime _CM = new DateTime();
            string NmCampo = Data.ToString();
            msgError = "";
            int addDias = 0;
            if (CampoMassaAddDias != "")
            {
                int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

            }
            try
            {


                _CM = Data;
                //_CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != DateTime.MinValue)
                {
                    ObjDigitar.Clear();

                    //if (_CM == "[Hoje]")
                    //{
                    //    _CM = DateTime.Now.ToShortDateString();
                    //}

                    if (addDias != 0)
                    {
                        _CM = _CM.AddDays(addDias);

                    }

                    Digitar(ObjDigitar, _CM.ToString(), tecla, WebDriver);

                    result = ObjDigitar.GetAttribute("value").Contains(_CM.ToShortDateString());

                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao digitar o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message);

                result = false;
            }

            return result;
        }
        public static bool Digitar_CampoData(IWebElement ObjDigitar, string CampoMassa, out string msgError, IWebDriver WebDriver, string CampoMassaAddDias = "", DepoisDeDigitar tecla = DepoisDeDigitar.Nada, TipoComparacaoDataHora TpComparacao = TipoComparacaoDataHora.SomenteData)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            int addDias = 0;
            if (CampoMassaAddDias != "")
            {
                int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

            }
            try
            {


                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "")
                {
                    ObjDigitar.Clear();


                    if (_CM == "[Hoje]")
                    {
                        switch (TpComparacao)
                        {
                            case TipoComparacaoDataHora.SomenteData:
                                _CM = DateTime.Now.ToShortDateString();
                                break;
                            case TipoComparacaoDataHora.DataEHora:
                                _CM = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                break;
                        }
                    }

                    if (addDias != 0)
                    {
                        switch (TpComparacao)
                        {
                            case TipoComparacaoDataHora.SomenteData:
                                _CM = DateTime.Parse(_CM).AddDays(addDias).ToShortDateString();
                                break;
                            case TipoComparacaoDataHora.DataEHora:
                                _CM = DateTime.Parse(_CM).AddDays(addDias).ToString("dd/MM/yyyy HH:mm");
                                break;
                        }

                    }

                    if (_CM != "[Vazio]")
                    {

                        Digitar(ObjDigitar, _CM, tecla, WebDriver);

                        result = ObjDigitar.GetAttribute("value").ToUpper().Contains(_CM.Replace(" 00:00:00", "").ToUpper());


                        if (!result)
                        {
                            throw new Exception();
                        }
                    }

                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao digitar o valor ['{1}']\n No campo esta aparecendo ['{2}'] \n Mensagem de Erro: {3}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ObjDigitar.GetAttribute("value"), ex.Message);

                result = false;
            }

            return result;
        }
        /// <summary>
        /// Digita o conteúdo que vem da massa no componente [Select 1], 
        /// </summary>
        /// <param name="ObjClick"></param>
        /// <param name="CampoMassa"></param>
        /// <param name="msgError"></param>
        /// <param name="ContemPartedoTexto"></param>
        /// <param name="CompararSomente">Utilizado se você quer somente comparar o valor que esta no objeto com o que esta na massa</param>
        /// <returns></returns>
        public static bool CampoSelecao(IWebElement ObjClick, string CampoMassa, out string msgError, bool ContemPartedoTexto = false, bool CompararSomente = false)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            string VlrConferencia = "";
            msgError = "";
            try
            {
                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "")
                {
                    var selectElement = new SelectElement(ObjClick);
                    if (!CompararSomente)
                    {
                        selectElement.SelectByText(_CM, ContemPartedoTexto);
                    }
                    new ScreenshotCustom().ScreenShot();
                    Thread.Sleep(500);
                    VlrConferencia = selectElement.SelectedOption.Text.ToUpper();
                    Assert.IsTrue(selectElement.SelectedOption.Text.ToUpper().Contains(_CM.ToUpper()));

                }

            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao tentar Selecionar o valor ['{1}'], valor encontrado foi ['{3}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message, VlrConferencia);

                result = false;
            }

            return result;
        }

        public static bool MensagemEsperada(IWebElement ObjClick, string CampoMassa, out string msgError)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            //////string VlrConferencia = "";
            msgError = "";

            try
            {
                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "")
                {

                    new ScreenshotCustom().ScreenShot();

                    Assert.AreEqual(_CM, ObjClick.GetAttribute("innerText"));

                    Thread.Sleep(500);

                    ObjClick.Click();

                }

            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro ao validar a msg do teste [{0}] \n ****Erro ao tentar validar a msg ['{1}'], \n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message);

                result = false;
            }

            return result;

        }
        /// <summary>
        /// Digita o conteúdo que vem da massa no componente [Select 2]
        /// </summary>
        /// <param name="ObjClick">Objeto que vai ser clicado</param>
        /// <param name="Class_ObjDigitar">string da classe do objeto que vai ser digitado</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="webDriver">passar o WebDriver</param>
        /// <param name="msgError">mensagem de retorno caso de erro</param>
        /// <param name="TextoExato">Serve caso você queira procurar o texto exato ou somente parte do texto</param>
        /// <param name="CompararSomente">Caso queira somente comparar o texto que esta no campo, com o valor da massa</param>
        /// <param name="select">Tipo do Select que esta sendo usado, [Select_2 = Select com o campo para digitar separado do campo click], [Select_3 = Select com o campo para digitar junto com o campo click]</param>
        /// 
        /// <returns></returns>
        public static bool CampoSelecao(IWebElement ObjClick, string Class_ObjDigitar, string CampoMassa, IWebDriver webDriver, out string msgError, bool TextoExato = false, bool CompararSomente = false, TipoDeSelect select = TipoDeSelect.Select_2)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {
                _CM = CampoMassa;
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }


                if (_CM != "")
                {
                    if (select == TipoDeSelect.Select_2)
                    {
                        if (!CompararSomente)
                        {
                            ObjClick.Click();

                            var a = webDriver.FindElement(By.ClassName(Class_ObjDigitar));
                            Thread.Sleep(500);


                            //****Foi alterado porque os comando "SendKeys" e "SendKeysCustom" do objeto não estava funcionando em algumas telas....****************************
                            //a.SendKeys(_CM); 
                            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(webDriver);
                            actions.SendKeys(a, _CM).Build().Perform();
                            //********************************************************************************


                            if (TextoExato)
                            {

                                bool next = false;
                                String Ult = "";
                                do
                                {
                                    OpenQA.Selenium.Interactions.Actions actions2 = new OpenQA.Selenium.Interactions.Actions(webDriver);
                                    if (next)
                                    {
                                        actions2.SendKeys(a, Keys.Down).Perform();
                                    }

                                    var b = webDriver.FindElement(By.ClassName("select2-results__option--highlighted"));

                                    if (b.Text.ToUpper() == _CM.ToUpper())
                                    {
                                        break;
                                    }

                                    next = false;

                                    if (Ult == b.Text)
                                    {
                                        throw new Exception("Não foi possivel localizar o texto exato solicitado =[" + _CM + "]");
                                    }

                                    next = true;

                                    Ult = b.Text;

                                } while (true);

                            }

                            OpenQA.Selenium.Interactions.Actions actions3 = new OpenQA.Selenium.Interactions.Actions(webDriver);



                            Thread.Sleep(2000);
                            a.SendKeys(Keys.Enter);
                            //Thread.Sleep(10000);
                        }
                    }
                    else if (select == TipoDeSelect.Select_3)
                    {
                        if (!CompararSomente)
                        {
                            ObjClick.Click();
                            string tab = ObjClick.GetAttribute("tabindex");
                            bool usarMesmoObj = false;
                            //var a = webDriver.FindElement(By.ClassName(Class_ObjDigitar));
                            var a = ObjClick;
                            if (Class_ObjDigitar != "")
                            {
                                a = webDriver.FindElement(By.XPath(string.Format("//input[@tabindex='{0}' and @class='{1}']", tab, Class_ObjDigitar)));

                                Thread.Sleep(500);
                            }
                            else
                            {
                                usarMesmoObj = true;
                            }


                            //****Foi alterado porque os comando "SendKeys" e "SendKeysCustom" do objeto não estava funcionando em algumas telas....****************************
                            //a.SendKeys(_CM); 
                            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(webDriver);
                            actions.SendKeys(a, _CM).Build().Perform();
                            //********************************************************************************


                            if (TextoExato)
                            {

                                bool next = false;
                                String Ult = "";
                                do
                                {
                                    OpenQA.Selenium.Interactions.Actions actions2 = new OpenQA.Selenium.Interactions.Actions(webDriver);
                                    if (next)
                                    {
                                        actions2.SendKeys(a, Keys.Down).Perform();
                                    }

                                    var b = webDriver.FindElement(By.ClassName("select2-results__option--highlighted"));

                                    if (b.Text.ToUpper() == _CM.ToUpper())
                                    {
                                        break;
                                    }

                                    next = false;

                                    if (Ult == b.Text)
                                    {
                                        throw new Exception("Não foi possivel localizar o texto exato solicitado =[" + _CM + "]");
                                    }

                                    next = true;

                                    Ult = b.Text;

                                } while (true);

                            }

                            OpenQA.Selenium.Interactions.Actions actions3 = new OpenQA.Selenium.Interactions.Actions(webDriver);


                            Thread.Sleep(2000);
                            a.SendKeys(Keys.Enter);

                            //Pega o irmão posterior do objeto que tem id
                            //var c = webDriver.FindElement(By.XPath("//select[@id='LotacoesLotadoId']//following-sibling::span[contains(@class, 'select2-container')]"));
                            //Pega o pai
                            //var c = webDriver.FindElements(By.XPath("//input[@tabindex='"+tab+"']//preceding::select[@tabindex='-1']"));
                            if (!usarMesmoObj)
                            {
                                var c = webDriver.FindElements(By.XPath(string.Format("//input[@tabindex='{0}' and @class='{1}']", tab, Class_ObjDigitar) + "//preceding::select[@tabindex='-1']"));
                                var count = c.Count;
                                ObjClick = c[count - 1];
                                new ScreenshotCustom().ScreenShot();
                                Thread.Sleep(500);

                                Assert.IsTrue(ObjClick.Text.ToUpper().Contains(_CM.ToUpper()));
                            }
                            else
                            {
                                Thread.Sleep(500);
                                //var c = webDriver.FindElements(By.XPath("//div[@class='ng-star-inserted']//preceding::div[@class='ng-input']//input[@role='combobox']"));
                                IWebElement c = webDriver.FindElement(By.XPath("//div[contains(@class,'ng-star-inserted') and contains(@class, 'ng-value')]"));
                                //var count = c.Count;
                                // new ScreenshotCustom().ScreenShot();
                                Thread.Sleep(500);

                                Assert.IsTrue(c.Text.ToUpper().Contains(_CM.ToUpper()));

                            }

                            //var c = webDriver.FindElements(By.XPath("//input[@tabindex='"+text+ "']//preceding::span[@class='select2-selection__rendered']"));
                            //var c = webDriver.FindElements(By.XPath("//select[@tabindex='-1']"));

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                string MdComparar = CompararSomente ? "[MODO SOMENTE COMPARANDO]" : "";
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao tentar Selecionar o valor ['{1}']\n\n {2}\n\n Mensagem de Erro: {3}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, MdComparar, ex.Message);

                result = false;
            }

            return result;
        }

        public static bool CampoSelecaoNew(IWebElement ObjClick, string Class_ObjDigitar, string CampoMassa, DepoisDeDigitar tecla, IWebDriver webDriver, out string msgError, bool TextoExato = false, bool CompararSomente = false, TipoDeSelect select = TipoDeSelect.Select_2)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {
                _CM = CampoMassa;
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }


                if (_CM != "")
                {
                    if (select == TipoDeSelect.Select_2)
                    {
                        if (!CompararSomente)
                        {
                            ObjClick.Clear();
                            ObjClick.Click();

                            var a = webDriver.FindElement(By.Id(Class_ObjDigitar));
                            Thread.Sleep(300);

                            var procura = webDriver.FindElement(By.Id("idprocura"));
                            int i = 0;
                            while (procura.Displayed)
                            {
                                i++;
                                if (i > 50)
                                    break;
                            }


                            //****Foi alterado porque os comando "SendKeys" e "SendKeysCustom" do objeto não estava funcionando em algumas telas....****************************
                            //a.SendKeys(_CM); 
                            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(webDriver);
                            actions.SendKeys(a, _CM).Build().Perform();
                            while (procura.Displayed)
                            {
                                i++;
                                if (i > 50)
                                    break;
                            }
                            //********************************************************************************


                            if (TextoExato)
                            {

                                bool next = false;
                                String Ult = "";
                                do
                                {
                                    OpenQA.Selenium.Interactions.Actions actions2 = new OpenQA.Selenium.Interactions.Actions(webDriver);
                                    if (next)
                                    {
                                        actions2.SendKeys(a, Keys.Down).Perform();
                                        while (procura.Displayed)
                                        {
                                            i++;
                                            if (i > 30)
                                                break;
                                        }
                                    }

                                    var b = webDriver.FindElement(By.ClassName("autocomplete-suggestion"));

                                    if (b.Text.ToUpper() == _CM.ToUpper())
                                    {
                                        break;
                                    }

                                    next = false;

                                    if (Ult == b.Text)
                                    {
                                        throw new Exception("Não foi possivel localizar o texto exato solicitado =[" + _CM + "]");
                                    }

                                    next = true;

                                    Ult = b.Text;

                                } while (true);

                            }

                            OpenQA.Selenium.Interactions.Actions actions3 = new OpenQA.Selenium.Interactions.Actions(webDriver);

                            Thread.Sleep(6000);
                            a.SendKeys(Keys.Enter);
                        }
                    }
                    else if (select == TipoDeSelect.Select_3)
                    {
                        if (!CompararSomente)
                        {
                            ObjClick.Click();
                            string tab = ObjClick.GetAttribute("tabindex");
                            bool usarMesmoObj = false;
                            //var a = webDriver.FindElement(By.ClassName(Class_ObjDigitar));
                            var a = ObjClick;
                            if (Class_ObjDigitar != "")
                            {
                                a = webDriver.FindElement(By.XPath(string.Format("//input[@tabindex='{0}' and @class='{1}']", tab, Class_ObjDigitar)));
                              
                                Thread.Sleep(500);
                            }
                            else
                            {
                                usarMesmoObj = true;
                            }


                            //****Foi alterado porque os comando "SendKeys" e "SendKeysCustom" do objeto não estava funcionando em algumas telas....****************************
                            //a.SendKeys(_CM); 
                            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(webDriver);
                            actions.SendKeys(a, _CM).Build().Perform();
                            //********************************************************************************


                            if (TextoExato)
                            {

                                bool next = false;
                                String Ult = "";
                                do
                                {
                                    OpenQA.Selenium.Interactions.Actions actions2 = new OpenQA.Selenium.Interactions.Actions(webDriver);
                                    if (next)
                                    {
                                        actions2.SendKeys(a, Keys.Down).Perform();
                                    }

                                    var b = webDriver.FindElement(By.ClassName("autocomplete-suggestion"));

                                    if (b.Text.ToUpper() == _CM.ToUpper())
                                    {
                                        break;
                                    }

                                    next = false;

                                    if (Ult == b.Text)
                                    {
                                        throw new Exception("Não foi possivel localizar o texto exato solicitado =[" + _CM + "]");
                                    }

                                    next = true;

                                    Ult = b.Text;

                                } while (true);

                            }

                            OpenQA.Selenium.Interactions.Actions actions3 = new OpenQA.Selenium.Interactions.Actions(webDriver);


                            Thread.Sleep(2000);
                            a.SendKeys(Keys.Enter);

                            //Pega o irmão posterior do objeto que tem id
                            //var c = webDriver.FindElement(By.XPath("//select[@id='LotacoesLotadoId']//following-sibling::span[contains(@class, 'select2-container')]"));
                            //Pega o pai
                            //var c = webDriver.FindElements(By.XPath("//input[@tabindex='"+tab+"']//preceding::select[@tabindex='-1']"));
                            if (!usarMesmoObj)
                            {
                                var c = webDriver.FindElements(By.XPath(string.Format("//input[@tabindex='{0}' and @class='{1}']", tab, Class_ObjDigitar) + "//preceding::select[@tabindex='-1']"));
                                var count = c.Count;
                                ObjClick = c[count - 1];
                                new ScreenshotCustom().ScreenShot();
                                Thread.Sleep(500);

                                Assert.IsTrue(ObjClick.Text.ToUpper().Contains(_CM.ToUpper()));
                            }
                            else
                            {
                                Thread.Sleep(500);
                                //var c = webDriver.FindElements(By.XPath("//div[@class='ng-star-inserted']//preceding::div[@class='ng-input']//input[@role='combobox']"));
                                IWebElement c = webDriver.FindElement(By.XPath("//div[contains(@class,'ng-star-inserted') and contains(@class, 'ng-value')]"));
                                //var count = c.Count;
                                // new ScreenshotCustom().ScreenShot();
                                Thread.Sleep(500);

                                Assert.IsTrue(c.Text.ToUpper().Contains(_CM.ToUpper()));

                            }

                            //var c = webDriver.FindElements(By.XPath("//input[@tabindex='"+text+ "']//preceding::span[@class='select2-selection__rendered']"));
                            //var c = webDriver.FindElements(By.XPath("//select[@tabindex='-1']"));

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                string MdComparar = CompararSomente ? "[MODO SOMENTE COMPARANDO]" : "";
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao tentar Selecionar o valor ['{1}']\n\n {2}\n\n Mensagem de Erro: {3}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, MdComparar, ex.Message);

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Este Metodo retorna somente se o campo esta no estado habilitado ou não, tentando clicar nele.
        /// Não tira print da tela
        /// </summary>
        /// <param name="ObjClick"></param>
        /// <param name="Class_ObjDigitar"></param>
        /// <param name="webDriver"></param>
        /// <param name="msgError"></param>
        /// <returns></returns>
        public static bool CampoSelecao(IWebElement ObjClick, string Class_ObjDigitar, IWebDriver webDriver, out string msgError)
        {
            bool result = true;
            string _CM = "";
            //string NmCampo = "";
            msgError = "";
            try
            {
                //_CM = CampoMassa;
                //_CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();

                //if (_CM != "")
                //{
                ObjClick.Click();

                try
                {
                    var a = webDriver.FindElement(By.ClassName(Class_ObjDigitar));
                }
                catch (Exception)
                {
                    result = false;
                }


                Assert.IsTrue(result);
                //}
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao tentar Selecionar o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message);

                result = false;
            }

            return result;
        }

        public static bool ValorCampoMassa(string CampoMassa, out string msgError)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {
                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();

                bool.TryParse(_CM, out result);

            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao pegar o valor do campo ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], NmCampo, ex.Message);

                result = false;
            }
            return result;
        }
        /// <summary>
        /// Este metodo clica no objeto usando Interactions Actions do Selenium.
        /// </summary>
        /// <param name="ObjDigitar">Objeto a ser clicado</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="msgError">Mensagem de Erro que é retornado, caso ocorra</param>
        /// <param name="WebDriver">WebDriver para uso interno do metodo</param>
        /// <returns></returns>
        public static bool Clica_Botao(IWebElement ObjDigitar, string CampoMassa, out string msgError, IWebDriver WebDriver)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";

            try
            {
                _CM = CampoMassa;
                //Se começa com "=", remove o igual e assume o valor passado
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }
                //_CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "" && bool.Parse(_CM))
                {
                    new ScreenshotCustom().ScreenShot();

                    OpenQA.Selenium.Interactions.Actions actions2 = new OpenQA.Selenium.Interactions.Actions(WebDriver);
                    actions2.Click(ObjDigitar).Build().Perform();
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao clicar no Botão o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], NmCampo, ex.Message);

                result = false;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjDigitar">Objeto a ser clicado</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="msgError">Mensagem de Erro que é retornado, caso ocorra</param>
        /// <returns></returns>
        public static bool Clica_Botao(IWebElement ObjDigitar, string CampoMassa, out string msgError, bool TiraPrint = true)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {
                _CM = CampoMassa;

                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }

                if (_CM != "" && bool.Parse(_CM))
                {
                    ObjDigitar.Click();
                }

                if (TiraPrint)
                    new ScreenshotCustom().ScreenShot();


                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao clicar no Botão o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], NmCampo, ex.Message);

                result = false;
            }
            return result;
        }
        public static bool Clica_Botao(string IDObjDigitar, string CampoMassa, IWebDriver WebDriver, out string msgError)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {
                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "" && bool.Parse(_CM))
                {
                    IWebElement obj = WebDriver.FindElement(By.Id(IDObjDigitar));
                    obj.ClickCustom();
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao clicar no Botão o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], NmCampo, ex.Message);

                result = false;
            }
            return result;
        }
        public static bool Digitar_CampoTexto(IWebElement ObjDigitar, string CampoMassa, out string msgError, DepoisDeDigitar tecla, IWebDriver WebDriver)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {


                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "")
                {
                    ObjDigitar.Clear();
                    if (_CM != "[Vazio]")
                    {
                        Digitar(ObjDigitar, _CM, tecla, WebDriver);
                    }


                    result = ObjDigitar.GetAttribute("value") == _CM;
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao digitar o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message);

                result = false;
            }

            return result;
        }
        /// <summary>
        /// Manipula ou pega o valor do estado do flag [SIM/NÃO]
        /// </summary>
        /// <param name="objFlag">Id do objeto da tela do tipo string que será usado</param>
        /// <param name="CampoMassa">Valor que vem da massa para comparação, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="acao">Tipo de Ação [Click]/[Status]: Click = vai clicar no objeto para ficar igual o que vem da massa: [Status] verifica se o valor do objeto esta igual o que vem da massa</param>
        /// <param name="msgerro">mensagem de retorno</param>
        /// <param name="WebDriver">WebDriver que tem no momento</param>
        /// <returns></returns>
        public static bool Flag(string objFlag, string CampoMassa, TipoDeAcao acao, out string msgerro, IWebDriver WebDriver, TipoDeFlag tipodeflag = TipoDeFlag.Tipo_1_SimNão)
        {
            bool result = true;
            string _CM = CampoMassa;
            msgerro = "";
            //Se começa com "=", remove o igual e assume o valor passado
            if (_CM.Substring(0, 1) == "=")
            {
                int a = _CM.Length;
                _CM = _CM.Substring(1, a - 1);
            }
            else
            {
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
            }

            string xpathToFind = "";
            bool valorconvertido = false;
            bool obj = false;
            IWebElement F;

            switch (acao)
            {

                case TipoDeAcao.Click:
                    try
                    {

                        if (_CM != "")
                        {
                            switch (tipodeflag)
                            {
                                case TipoDeFlag.Tipo_1_SimNão:
                                    bool.TryParse(_CM.ToUpper() == "SIM" ? "true" : _CM, out valorconvertido);
                                    if (Flag_Value(objFlag, WebDriver) != valorconvertido)
                                        Flag_Click(objFlag, WebDriver);
                                    break;

                                case TipoDeFlag.Tipo_2_RadioButon:
                                    obj = false;
                                    xpathToFind = string.Format(@"//input[@id='{0}']", objFlag);
                                    F = WebDriver.FindElement(By.XPath(xpathToFind));
                                    bool.TryParse(_CM.ToUpper() == "SIM" ? "True" : _CM, out valorconvertido);

                                    bool.TryParse(F.GetAttribute("checked"), out obj);

                                    if (obj.ToString().ToUpper() != valorconvertido.ToString().ToUpper())
                                    {
                                        F = WebDriver.FindElement(By.XPath(xpathToFind + "//following-sibling::label"));

                                        F.Click();
                                    }

                                    bool.TryParse(F.GetAttribute("checked"), out obj);

                                    break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        msgerro = string.Format("Não foi possivel clicar no objeto [{0}], campo vindo da massa [{1}]", objFlag, CampoMassa);
                        result = false;
                    }

                    break;
                case TipoDeAcao.Status:
                    try
                    {
                        if (_CM != "")
                        {

                            switch (tipodeflag)
                            {
                                case TipoDeFlag.Tipo_1_SimNão:

                                    xpathToFind = string.Format(@"//*[@id='{0}']/ancestor::div[contains(@class,'bootstrap-switch')]", objFlag);
                                    F = WebDriver.FindElement(By.XPath(xpathToFind));
                                    bool.TryParse(_CM.ToUpper() == "SIM" ? "True" : _CM, out valorconvertido);
                                    _CM = valorconvertido.ToString();
                                    result = F.GetAttribute("Class").Contains("bootstrap-switch-on").ToString().ToUpper() == _CM.ToUpper();

                                    break;
                                case TipoDeFlag.Tipo_2_RadioButon:

                                    xpathToFind = string.Format(@"//*[@id='{0}']", objFlag);
                                    F = WebDriver.FindElement(By.XPath(xpathToFind));
                                    bool.TryParse(_CM.ToUpper() == "SIM" ? "True" : _CM, out valorconvertido);
                                    obj = false;
                                    bool.TryParse(F.GetAttribute("checked"), out obj);


                                    _CM = valorconvertido.ToString();


                                    result = obj.ToString().ToUpper() == _CM.ToUpper();

                                    break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        msgerro = string.Format("Erro ao tentar pegar valor do objeto [{0}] , valor vindo da massa [{1}]", objFlag, CampoMassa);
                        result = false;
                    }

                    break;
            }
            return result;

        }

        /// <summary>
        /// Metodo que verifica o valor que vem da massa, com o atributo inserido no parametro [Atributo].
        /// </summary>
        /// <param name="Obj">Objeto mapeado da tela</param>
        /// <param name="CampoMassa">Valor que deve ser comparado, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="Atributo">Atualmente esta aceitando os atributos "innerText", "placeholder" e "value"</param>
        /// <param name="msgError">mensagem qeu vai retornar caso de erro</param>
        /// <param name="TiraPrint">Configura se tira print nesta etapa ou não, esta default como True</param>
        /// <returns>retorna se encontrou o valor da massa no atributo desejado</returns>
        public static bool ConferirAtributo(IWebElement Obj, string CampoMassa, string Atributo, out string msgError, bool TiraPrint = true)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            string VlrConferencia = "";

            try
            {

                _CM = CampoMassa;
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }

                if (_CM != "")
                {
                    if (_CM == "[Vazio]")
                    {
                        _CM = "";
                    }

                    if (Atributo.ToLower() == "placeholder")
                    {
                        VlrConferencia = Obj.GetAttribute("placeholder");
                    }

                    if (Atributo.ToLower() == "value")
                    {
                        VlrConferencia = Obj.GetAttribute("value");
                    }

                    if (Atributo.ToLower() == "innertext")
                    {
                        VlrConferencia = Obj.GetAttribute("innerText");
                    }

                    if (TiraPrint)
                    {
                        Obj.ClickCustom();

                    }
                    result = VlrConferencia.ToUpper().Contains(_CM.Replace(" 00:00:00", "").ToUpper());

                    if (!result)
                    {
                        throw new Exception();
                    }
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao validar o valor ['{1}'], no campo esta ['{3}'] \n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message, VlrConferencia);

                result = false;
            }

            return result;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjDigitar">Objeto mapeado da tela</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="msgError">Mensagem de Erro que é retornado, caso ocorra</param>
        /// <param name="tecla">Após digitar, qual acão que ele vai fazer [Enter], [Tab] ou [nada]</param>
        /// <param name="tecla2">Após digitar, qual acão que ele vai fazer [Enter], [Tab] ou [nada]</param>
        /// <param name="SomenteConferir">Se passar [true], ele apenas compara o que esta no campo com o que foi passado no CampoMassa</param>
        /// <param name="TiraPrint">Em alguns casos esta dando problema na hora do print, então, passa como false que o print não sera tirado</param>
        /// <returns></returns>
        public static bool Digitar_CampoTexto(IWebElement ObjDigitar, string CampoMassa, out string msgError, DepoisDeDigitar tecla, bool SomenteConferir = false, bool TiraPrint = true)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            string VlrConferencia = "";
            msgError = "";
            try
            {


                _CM = CampoMassa;
                //Se começa com "=", remove o igual e assume o valor passado
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }

                if (_CM != "")
                {
                    if (_CM != "[Vazio]")
                    {
                        if (!SomenteConferir)
                        {
                            ObjDigitar.Clear();
                            Digitar(ObjDigitar, _CM, tecla);
                        }

                    }
                    else
                    {
                        _CM = "";
                    }
                    VlrConferencia = ObjDigitar.GetAttribute("value");
                    result = ObjDigitar.GetAttribute("value").ToUpper().Contains(_CM.Replace(" 00:00:00", "").ToUpper());

                    if (!result)
                    {
                        throw new Exception();
                    }
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao " + string.Format("{0}", SomenteConferir ? "validar" : "digitar") + " o valor ['{1}'], no campo esta ['{3}'] \n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message, VlrConferencia);

                result = false;
            }

            return result;
        }
       
        public static bool Digitar_CampoTexto(IWebElement ObjDigitar, string CampoMassa, string parametro1, out string msgError, DepoisDeDigitar tecla, IWebDriver webDriver)
        {
            bool result = true;
            string _CM = "";
            string NmCampo = CampoMassa;
            msgError = "";
            try
            {


                _CM = CampoMassa;
                _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                if (_CM != "")
                {
                    ObjDigitar.Clear();
                    if (_CM != "[Vazio]")
                    {
                        _CM = string.Format(_CM, parametro1);

                        Digitar(ObjDigitar, _CM, tecla, webDriver);

                        result = ObjDigitar.GetAttribute("value") == _CM;

                    }
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao digitar o valor ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message);

                result = false;
            }

            return result;
        }
        public static object GetCell(IWebElement Tabela, int Row, int Col)
        {
            string idTable = Tabela.GetAttribute("id");
            var _row = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//tr[@role='row']", idTable)));
            //var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
            object _value = "::Item não encontrado.::";
            for (int i = 1; i <= _row.Count; i++)
            {
                if (i == Row)
                {
                    var _col = _row[i - 1].FindElements(By.TagName("td"));
                    _value = _col[Col];
                    break;
                }
            }

            string result = _value.ToString();
            if (result == "::Item não encontrado.::")
            {
                //var _col = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//td[@class='dataTables_empty']", idTable)));
                //var _col = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//tr[@class='odd']", idTable)));
                var _col = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//td[@class='dataTables_empty']", idTable)));
                _value = _col[Col];
            }

            return _value;
        }

        public static string GetCellValueSHF(IWebElement Tabela, int Row, int Col)
        {
            string idTable = Tabela.GetAttribute("id");
            var _row = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//tbody//tr[@role='row']", idTable)));
            //var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
            object _value = "::Item não encontrado.::";
            for (int i = 1; i <= _row.Count; i++)
            {
                if (i == Row)
                {
                    var _col = _row[i - 1].FindElements(By.TagName("td"));
                    _value = _col[Col];
                    break;
                }
            }

            string result = _value.ToString();
            if (result == "::Item não encontrado.::")
            {
                //var _col = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//td[@class='dataTables_empty']", idTable)));
                //var _col = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//tr[@class='odd']", idTable)));
                var _col = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//td[@class='dataTables_empty']", idTable)));
                _value = _col[Col];
            }

            return ((IWebElement)_value).Text;
        }
        //public static bool GetTable_FindCellContainSHF(IWebElement Tabela, int Col, string value, string CampoMassaAddDias = "")
        //{
        //    bool result = false;
        //    try
        //    {
        //        var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
        //        string _value = "::Item não encontrado.::";
        //        int addDias = 0;
        //        if (CampoMassaAddDias != "")
        //        {
        //            int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

        //        }

        //        for (int i = 1; i < _row.Count; i++)
        //        {
        //            var _col = _row[i].FindElements(By.TagName("td"));
        //            _value = _col[Col].Text;

        //            if (value == "[Hoje]")
        //            {
        //                value = DateTime.Now.ToShortDateString();
        //            }

        //            if (addDias != 0)
        //            {
        //                value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

        //            }
        //            if (_value.ToUpper().Contains(value.ToUpper()))
        //            {
        //                result = true;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //    }


        //    return result;
        //}


        public static Boolean ClickIfExistBy(By byObject, int tentativas, IWebDriver webDriver, bool javaScript = false, bool byAction = false)
        {

            bool obj = false;
            IWebElement element = null;
            for (int x = 1; x <= tentativas; x++)
            {
                try
                {
                    obj = webDriver.FindElement(byObject).Displayed;
                 

                    if (obj)
                    {
                        element = webDriver.FindElement(byObject);
                        if (javaScript)
                        {
                            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                            js.ExecuteScript("arguments[0].click();", element);
                            return true;
                        }
                        else if (byAction)
                        {
                            Actions action = new Actions(webDriver);
                            action.MoveToElement(element).Click().Build().Perform();
                            return true;
                        }
                        else
                        {
                            element.Click();
                            return true;
                        }
                    }

                }
                catch
                {
                    //tempo de esperar do loop
                    Thread.Sleep(500);
                }
            }
            return false;
        }
        public static bool GetTable_FindCellEqual(IWebElement Tabela, int Col, string value)
        {
            bool result = false;
            try
            {
                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (_value == value)
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }
        public static bool GetTable_FindCellNotEqual(IWebElement Tabela, int Col, string value)
        {
            bool result = false;

            try
            {
                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                if (_row.Count == 1)
                {
                    result = true;
                    return result;

                }
                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (_value != value)
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                result = true;
            }
            return result;
        }
        public static bool GetTable_FindCellContain(IWebElement Tabela, int Col, string value, string CampoMassaAddDias = "")
        {
            bool result = false;
            try
            {
                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                }

                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (value == "[Hoje]")
                    {
                        value = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                    }
                    if (_value.ToUpper().Contains(value.ToUpper()))
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }


            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tabela">Tabela mapeada que será feita a busca</param>
        /// <param name="Col">Index da Coluna que será feita a procura</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="CampoMassaAddDias">Caso queira adicionar dias no data que for passado no campomassa</param>
        /// <returns></returns>
        public static int GetTable_ContaCelulasIguais(IWebElement Tabela, int Col, string CampoMassa, string CampoMassaAddDias = "")
        {
            int result = 0;
            try
            {

                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                }

                string value = CampoMassa;
                if (value.Substring(0, 1) == "=")
                {
                    int a = value.Length;
                    value = value.Substring(1, a - 1);
                }
                else
                {
                    value = Utils.Util.GetTestContext().DataRow[value] == null ? "" : Utils.Util.GetTestContext().DataRow[value].ToString();
                }

                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    try
                    {
                        _value = _col[Col].Text;

                    }
                    catch (Exception)
                    {
                        _value = "";

                    }

                    if (value == "[Hoje]")
                    {
                        value = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                    }
                    if (_value.ToUpper().Contains(value.ToUpper()))
                    {
                        result++;
                    }
                }
            }
            catch (Exception)
            {
                result = 0;
            }


            return result;
        }
        /// <summary>
        /// Procura na coluna da Tabela alguma celula na coluna especificada que não esteja igual ao valor passado no CampoMassa
        /// </summary>
        /// <param name="Tabela">Tabela mapeada que será feita a busca</param>
        /// <param name="Col">Index da Coluna que será feita a procura</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="CampoMassaAddDias">Caso queira adicionar dias no data que for passado no campomassa</param>
        /// <returns></returns>
        public static bool GetTable_FindCellContain_CampoMassa(IWebElement Tabela, int Col, string CampoMassa, string CampoMassaAddDias = "", bool TiraPrint = true)
        {
            bool result = false;
            try
            {
                if (TiraPrint)
                    new ScreenshotCustom().ScreenShot();


                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                }

                string value = CampoMassa;
                if (value.Substring(0, 1) == "=")
                {
                    int a = value.Length;
                    value = value.Substring(1, a - 1);
                }
                else
                {
                    value = Utils.Util.GetTestContext().DataRow[value] == null ? "" : Utils.Util.GetTestContext().DataRow[value].ToString();
                }

                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (value == "[Hoje]")
                    {
                        value = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                    }
                    if (_value.ToUpper().Contains(value.ToUpper()))
                    {
                        result = true;
                        break;
                    }
                }



            }
            catch (Exception)
            {
                result = false;
            }


            return result;
        }
        /// <summary>
        /// Metodo que verifica se na coluna da tabela especificada, contém pelo menos 1 resultado da massa.
        /// </summary>
        /// <param name="Tabela">Nome da Tabela que vai fazer a varedura</param>
        /// <param name="Col">Coluna que vai ser feita a busca</param>
        /// <param name="value">Valor será pesquisado, ATENÇÃO "NÃO É O NOME DO CAMPO MASSA"</param>
        /// <param name="msgerro">mensagem que irá retornar caso não ache o valor esperado</param>
        /// <param name="CampoMassaAddDias">Caso o valor do CampoMassa for uma data o valor contido aqui será adicionado para comparação</param>
        /// <returns>Retorna valor "true" ou "false"</returns>
        public static bool GetTable_FindCellContain(IWebElement Tabela, int Col, string value, out string msgerro, string CampoMassaAddDias = "")
        {
            bool result = false;
            msgerro = "";
            new ScreenshotCustom().ScreenShot();
            try
            {

                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                }

                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (value == "[Hoje]")
                    {
                        value = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                    }
                    if (_value.Contains(value))
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            if (result == false)
                msgerro = string.Format("Não foi possivel localizar o valor esperado, Campo massa [{0}]", value);
            else
                msgerro = "";

            return result;
        }
        /// <summary>
        /// Metodo que verifica se na coluna da tabela especificada, contém pelo menos 1 resultado da massa.
        /// </summary>
        /// <param name="Tabela">Nome da Tabela que vai fazer a varedura</param>
        /// <param name="Col">Coluna que vai ser feita a busca</param>
        /// <param name="valor">Valor será pesquisado</param>
        /// <param name="msgerro">mensagem que irá retornar caso não ache o valor esperado</param>
        /// <param name="CampoMassaAddDias">aso o valor do CampoMassa for uma data o valor contido aqui será adicionado para comparação</param>
        /// <param name="tipoEntrada">Este define se a informação inserida no paramentro [valor] será o nome da coluna na massa ou o valor direto.</param>
        /// <returns>Retorna valor "true" ou "false"</returns>
        public static bool GetTable_FindCellContain(IWebElement Tabela, int Col, string valor, out string msgerro, TipoEntrada tipoEntrada, string CampoMassaAddDias = "")
        {
            string idTable = Tabela.GetAttribute("id");
            bool result = false;
            msgerro = "";
            string V = string.Empty;
            new ScreenshotCustom().ScreenShot();
            try
            {

                switch (tipoEntrada)
                {
                    case TipoEntrada.CampoMassa:
                        V = Utils.Util.GetTestContext().DataRow[valor] == null ? "" : Utils.Util.GetTestContext().DataRow[valor].ToString();
                        break;
                    case TipoEntrada.Valor:
                        V = valor;
                        break;
                }

                var _row = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//tbody//tr", idTable)));


                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);
                }

                for (int i = 0; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (V == "[Hoje]")
                    {
                        V = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        V = DateTime.Parse(V).AddDays(addDias).ToShortDateString();

                    }
                    if (_value.Contains(V.Replace(" 00:00:00", "")))
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            if (result == false)
                msgerro = string.Format("Não foi possivel localizar o valor esperado, Campo massa [{0}]", valor);
            else
                msgerro = "";

            return result;
        }
        /// <summary>
        /// Procura na coluna da Tabela alguma celula na coluna especificada que não esteja igual ao valor passado no CampoMassa
        /// </summary>
        /// <param name="Tabela">Tabela mapeada que será feita a busca</param>
        /// <param name="Col">Index da Coluna que será feita a procura</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <returns></returns>
        public static bool GetTable_FindCellNotContain(IWebElement Tabela, int Col, string CampoMassa)
        {
            bool result = true;
            string value = "";
            try
            {
                value = CampoMassa;
                if (value.Substring(0, 1) == "=")
                {
                    int a = value.Length;
                    value = value.Substring(1, a - 1);
                }
                else
                {
                    value = Utils.Util.GetTestContext().DataRow[value] == null ? "" : Utils.Util.GetTestContext().DataRow[value].ToString();
                }


                if (value != "")
                {
                    var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                    string _value = "::Item não encontrado.::";
                    int addDias = 0;

                    for (int i = 1; i < _row.Count; i++)
                    {
                        var _col = _row[i].FindElements(By.TagName("td"));
                        _value = _col[Col].Text;

                        if (value == "[Hoje]")
                        {
                            value = DateTime.Now.ToShortDateString();
                        }

                        if (addDias != 0)
                        {
                            value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                        }
                        if ((!_value.Contains(value)))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            result = false;
                        }
                    }


                }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }
        public static bool GetTable_FindCellNotContain(IWebElement Tabela, int Col, string value, string CampoMassaAddDias = "")
        {
            bool result = true;
            try
            {

                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                }

                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (value == "[Hoje]")
                    {
                        value = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                    }
                    if (!_value.Contains(value))
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }

                }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tabela">Tabela mapeada que será feita a busca</param>
        /// <param name="Col">Index da Coluna que será feita a procura</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="CampoMassaAddDias">Insira a quantidade de dias que queira adicionar ou subtrair do campomassa caso seja no formato de data</param>
        /// <param name="OpcaoParaTodos">Valor que é válido qualquer valor na coluna, exemplo: caso neste parametro esteja [Selecione...] e no CampoMassa for passado (Selecione...) será considerado válido qualquer valor na coluna</param>
        /// <returns></returns>
        public static bool GetTable_FindCellNotContain_CampoMassa(IWebElement Tabela, int Col, string CampoMassa, string CampoMassaAddDias = "", string OpcaoParaTodos = "Selecione...")
        {
            bool result = true;
            try
            {
                string value = CampoMassa;
                if (value.Substring(0, 1) == "=")
                {
                    int a = value.Length;
                    value = value.Substring(1, a - 1);
                }
                else
                {
                    value = Utils.Util.GetTestContext().DataRow[value] == null ? "" : Utils.Util.GetTestContext().DataRow[value].ToString();
                }


                var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                string _value = "::Item não encontrado.::";
                int addDias = 0;
                if (CampoMassaAddDias != "")
                {
                    int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                }

                for (int i = 1; i < _row.Count; i++)
                {
                    var _col = _row[i].FindElements(By.TagName("td"));
                    _value = _col[Col].Text;

                    if (value == "[Hoje]")
                    {
                        value = DateTime.Now.ToShortDateString();
                    }

                    if (addDias != 0)
                    {
                        value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                    }
                    if ((!_value.Contains(value)) && (value != OpcaoParaTodos))
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }

                }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tabela">Tabela mapeada que será feita a busca</param>
        /// <param name="Col">Index da Coluna que será feita a procura</param>
        /// <param name="CampoMassa">Nome da Coluna que esta na massa, caso queira passar o valor direto adicione o sinal de "=" antes da palavra Ex. =Sim ele vai pegar o valor [Sim]</param>
        /// <param name="CampoMassaAddDias">Insira a quantidade de dias que queira adicionar ou subtrair do campomassa caso seja no formato de data</param>
        /// <param name="OpcaoParaTodos">Valor que é válido qualquer valor na coluna, exemplo: caso neste parametro esteja [Selecione...] e no CampoMassa for passado (Selecione...) será considerado válido qualquer valor na coluna</param>
        /// <returns></returns>
        public static bool GetTable_TodasCelulasDaColunaIguais(IWebElement Tabela, int Col, string CampoMassa, string CampoMassaAddDias = "", string OpcaoParaTodos = "Selecione...")
        {
            bool result = false;
            try
            {
                string value = CampoMassa;
                if (value.Substring(0, 1) == "=")
                {
                    int a = value.Length;
                    value = value.Substring(1, a - 1);
                }
                else
                {
                    value = Utils.Util.GetTestContext().DataRow[value] == null ? "" : Utils.Util.GetTestContext().DataRow[value].ToString();
                }

                if (value != "")
                {


                    var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                    string _value = "::Item não encontrado.::";
                    int addDias = 0;
                    if (CampoMassaAddDias != "")
                    {
                        int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                    }

                    for (int i = 1; i < _row.Count; i++)
                    {
                        var _col = _row[i].FindElements(By.TagName("td"));
                        _value = _col[Col].Text;

                        if (value == "[Hoje]")
                        {
                            value = DateTime.Now.ToShortDateString();
                        }

                        if (addDias != 0)
                        {
                            value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                        }
                        if ((!_value.Contains(value)) && (value != OpcaoParaTodos))
                        {
                            result = false;
                            break;
                        }
                        else
                        {
                            result = true;
                        }

                    }
                }
                else { result = true; }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }

        public static bool GetCell_ExistActions(IWebElement Tabela, string IdObj, out string msgError, int Row = 1, int Col = 1)
        {

            bool result = false;
            string _CM = "";
            msgError = "";

            try
            {

                string idTable = Tabela.GetAttribute("id");
                IList<IWebElement> ArrayCell = Tabela.FindElements(By.XPath(string.Format("//table[@id='{0}']//tr[@role='row'][{1}]//td[{2}]//a", idTable, Row, Col)));

                int l = ArrayCell.Count;

                for (int i = 0; i < l; i++)
                {
                    if (ArrayCell[i].GetProperty("id") == IdObj)
                    {
                        result = true;
                        break;
                    };
                }

                new ScreenshotCustom().ScreenShot();

            }
            catch (Exception ex)
            {
                msgError = string.Format("\n Erro na linha da massa [{0}] \n ****Erro ao tentar verificar se existe link de ações o id do link ['{1}']\n\n Mensagem de Erro: {2}",
                    Utils.Util.GetTestContext().DataRow["NomeTeste"], _CM, ex.Message);

                result = false;
            }

            return result;

        }
        public static string AjuntaDataeDias(string Data, string Dias)
        {
            string result = "";
            int addDias = 0;
            int.TryParse(Dias, out addDias);

            if (Data.ToLower() == "[hoje]")
            {
                Data = DateTime.Now.Date.ToShortDateString();
            }

            result = DateTime.Parse(Data).AddDays(addDias).ToShortDateString();
            return result;
        }
        public static bool GetTable_FindCell_DataInicioMenorQueMassa(IWebElement Tabela, int Col, string CampoMassa, string CampoMassaAddDias = "")
        {
            bool result = true;
            string _CM = "";
            string value = "";
            try
            {

                _CM = CampoMassa;
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }

                if (_CM != "")
                {
                    result = false;

                    value = _CM;

                    var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                    string _value = "::Item não encontrado.::";
                    int addDias = 0;
                    if (CampoMassaAddDias != "")
                    {
                        int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                    }

                    for (int i = 1; i < _row.Count; i++)
                    {
                        var _col = _row[i].FindElements(By.TagName("td"));
                        _value = _col[Col].Text;

                        if (value == "[Hoje]")
                        {
                            value = DateTime.Now.ToShortDateString();
                        }

                        if (addDias != 0)
                        {
                            value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                        }

                        DateTime Dt1 = new DateTime();
                        DateTime.TryParse(value, out Dt1);

                        DateTime Dt2 = new DateTime();
                        DateTime.TryParse(_value.Split(' ')[0], out Dt2);

                        if (Dt2.Date.ToOADate() <= Dt1.Date.ToOADate())
                        {
                            result = true;
                            break;
                        }

                    }
                }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }
        public static bool GetTable_FindCell_DataFimMaiorQueMassa(IWebElement Tabela, int Col, string CampoMassa, string CampoMassaAddDias = "")
        {
            bool result = true;
            string _CM = "";
            string value = "";

            try
            {
                _CM = CampoMassa;
                if (_CM.Substring(0, 1) == "=")
                {
                    int a = _CM.Length;
                    _CM = _CM.Substring(1, a - 1);
                }
                else
                {
                    _CM = Utils.Util.GetTestContext().DataRow[_CM] == null ? "" : Utils.Util.GetTestContext().DataRow[_CM].ToString();
                }

                if (_CM != "")
                {
                    result = false;

                    value = _CM;
                    var _row = Tabela.FindElements(By.XPath("//tr[@role='row']"));
                    string _value = "::Item não encontrado.::";
                    int addDias = 0;
                    if (CampoMassaAddDias != "")
                    {
                        int.TryParse(Utils.Util.GetTestContext().DataRow[CampoMassaAddDias] == null ? "" : Utils.Util.GetTestContext().DataRow[CampoMassaAddDias].ToString(), out addDias);

                    }

                    for (int i = 1; i < _row.Count; i++)
                    {
                        var _col = _row[i].FindElements(By.TagName("td"));
                        _value = _col[Col].Text;

                        if (value == "[Hoje]")
                        {
                            value = DateTime.Now.ToShortDateString();
                        }

                        if (addDias != 0)
                        {
                            value = DateTime.Parse(value).AddDays(addDias).ToShortDateString();

                        }

                        DateTime Dt1 = new DateTime();
                        DateTime.TryParse(value, out Dt1);

                        int C = _value.Split(' ').Count() - 1;
                        DateTime Dt2 = new DateTime();
                        DateTime.TryParse(_value.Split(' ')[C], out Dt2);

                        if (Dt2.Date.ToOADate() >= Dt1.Date.ToOADate())
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

                result = false;
            }


            return result;
        }
        public static void VerificaCarregPag(IWebDriver _WebDriver)
        {

            try
            {
                Thread.Sleep(300);
                int i = 0;
                IWebElement DivModalCarregando = _WebDriver.FindElement(By.Id("modalCarregando"));
                while (DivModalCarregando.Displayed)
                {
                    Thread.Sleep(200);
                    i++;
                    if (i > 30)
                        break;
                }

            }
            catch (Exception)
            {

            }
        }




        public static void SetTestContext(TestContext value)
        {
            testContext = value;
        }

        private static string baseURL = "http://vm06-webapp:8080";

        //public static string diretorio { get; internal set; }

        private static string _diretorio;
        ////private static string sequencial;

        public static string diretorio
        {
            get { return _diretorio; }
            set { _diretorio = value; }
        }

        //public static string TestName { get; internal set; }

        private static string _TestName;
        //private static int linha = 0;



        public static string TestName
        {
            get
            {
                try
                {
                    _TestName = GetTestContext().DataRow["NomeTeste"] == null ? "" : Utils.Util.GetTestContext().DataRow["NomeTeste"].ToString();

                }
                catch (Exception)
                {

                }
                return _TestName;

            }
            set { _TestName = value; }
        }

        //public static bool TesteFinalizado { get; internal set; }
        //////private static bool _TesteFinalizado;

        #region Captura Evidência
        //public static string CapturaEvidencia()
        //{
        //    //CriaDiretorioEvidencias();
        //    string caminhoImg = caminhoDiretório + @"\" + "Imagem_" + sequencial + ".png";
        //    var arqImagem = ((ITakesScreenshot)WebDriver).GetScreenshot();
        //    arqImagem.SaveAsFile(caminhoImg, ScreenshotImageFormat.Png);
        //    return caminhoImg;

        //}
        #endregion


        private static void Digitar(IWebElement wd, string text, DepoisDeDigitar _tecla, bool Tp = true)
        {
            try
            {

                wd.Clear();
            }
            catch (Exception)
            {
            }

            wd.SendKeys(text);
            Thread.Sleep(100);
            if (Tp)
                new ScreenshotCustom().ScreenShot();

            switch (_tecla)
            {
                case DepoisDeDigitar.Enter:
                    wd.SendKeys(Keys.Enter);
                    break;
                case DepoisDeDigitar.Tab:
                    wd.SendKeys(Keys.Tab);
                    break;
                case DepoisDeDigitar.Nada:
                    break;

            }

        }
        private static void Digitar(IWebElement wd, string text, DepoisDeDigitar _tecla, IWebDriver WebDriver)
        {
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(WebDriver);

            wd.Clear();
            wd.Click();

            switch (_tecla)
            {
                case DepoisDeDigitar.Enter:
                    actions.SendKeys(text + Keys.Enter).Build().Perform();
                    new ScreenshotCustom().ScreenShot();

                    //wd.SendKeysCustom(text + Keys.Enter);
                    break;
                case DepoisDeDigitar.Tab:
                    actions.SendKeys(text + Keys.Tab).Build().Perform();
                    new ScreenshotCustom().ScreenShot();

                    //wd.SendKeysCustom(text + Keys.Tab);
                    break;
                case DepoisDeDigitar.Nada:
                    actions.SendKeys(text).Build().Perform();
                    new ScreenshotCustom().ScreenShot();

                    //wd.SendKeysCustom(text);
                    break;

            }

        }

        [TestInitialize]
        public static void SetupTest()
        {
            if (GetTestContext().Properties["Url"] != null)
            {
                baseURL = GetTestContext().Properties["Url"].ToString();
            }
            else
            {
                Assert.Fail();
            }
        }
        public enum TipoComparacaoDataHora
        {
            SomenteData,
            DataEHora
        }
        public enum TipoComparacao
        {
            exata,
            contem
        }
        public enum TipoEntrada
        {
            CampoMassa,
            Valor
        }
        public enum DepoisDeDigitar
        {
            Enter,
            Tab,
            Down,
            Nada
        }
        public enum TipoDeAcao
        {
            Click,
            Status
        }

        public enum TipoDeFlag
        {
            Tipo_1_SimNão,
            Tipo_2_RadioButon
        }

        public enum TipoDeSelect
        {
            Select_2,
            Select_3
        }

        public static bool Flag_Value(string Id, IWebDriver WebDriver)
        {
            //string xpathToFind = string.Format(@"//*[@id='{0}']/div", Id);
            string xpathToFind = string.Format(@"//*[@id='{0}']/ancestor::div[contains(@class,'bootstrap-switch')]", Id);
            IWebElement F = WebDriver.FindElement(By.XPath(xpathToFind));
            return F.GetAttribute("Class").Contains("bootstrap-switch-on");
        }

        public static void Flag_Click(string Id, IWebDriver WebDriver)
        {
            string xpathToFind = string.Format(@"//*[@id='{0}']", Id);
            IWebElement F = WebDriver.FindElement(By.XPath(xpathToFind));
            ClickJScript(F, WebDriver);

            //var a = WebDriver.FindElement(By.XPath(xpathToFind)).GetAttribute("checked");
        }

        public static void ClickJScript(IWebElement elemento, IWebDriver WebDriver)
        {
            ((IJavaScriptExecutor)WebDriver).ExecuteScript("arguments[0].click(); return false;", elemento);
            new ScreenshotCustom().ScreenShot();
        }


    }


}
