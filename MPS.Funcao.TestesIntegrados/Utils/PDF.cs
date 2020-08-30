using MPS.Funcao.TestesIntegrados.ScreenShot;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.IO;

namespace MPS.Funcao.TestesIntegrados.Utils
{

    public class PDF : IDisposable
    {
        private int _linha;

        public int linha
        {
            get { return _linha; }
            set { _linha = value; }
        }

        #region Gerador PDF
        #region Salva o Arquivo
        public void SalvarPdf(string fileName, PdfDocument document)
        {
            document.Save(fileName);
            document.Close();
            document.Dispose();

        }
        #endregion

        #region Incluir Imagem
        public void IncluirImagemPdf(XGraphics gfx, PdfDocument doc, string testCase, PdfPage page)
        {
            //var page = pdfDocument.AddPage();

            //Captura a Tela
            //var arqImagem = Util.CapturaEvidencia();
            //parametros de desenho da Imagem      (x=50,y=120 ,Width 500, Height 300)

            //Inclui todas as imagens capturadas que estão na pasta
            //var dir = @"C:\Evidencias_Holos\08-10-2019\CT_60996_AlterarEtapaAvisoResponsavelQualquerFuncionariodaUnidade_103854";
            //string[] pngList = Directory.GetFiles(dir, "*.png");
            string[] pngList = Directory.GetFiles(Util.renomeiarPasta ? Util.caminhoDiretório : Util.caminhoDiretório.Replace("[Error]", ""), "*.png", SearchOption.TopDirectoryOnly);

            if (pngList.Length > 0)
            {
                var totImagens = pngList.Length;
                var indexImg = 0;
                foreach (string f in pngList)
                {
                    indexImg++;
                    //if (linha == 0)
                    gfx.DrawImage(XImage.FromFile(f), 50, linha + 10, 500, 300);
                    //else
                    //gfx.DrawImage(XImage.FromFile(f), 50, linha + 10, 500, 300);

                    linha += 310;
                    if ((indexImg % 2 == 0) && pngList.Length > 1 && indexImg < totImagens)
                    {
                        linha = 0;
                        page = doc.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        //IncluirCabecalho(gfx, testCase, page, doc);

                    }
                }
            }

            //return arqImagem;
        }
        #endregion
        #region Incluir Cabeçalho
        public int IncluirCabecalho(XGraphics gfx, string testCase, PdfPage page, PdfDocument doc, string Ambiente = "")
        {
            linha += 10;

            //fonts
            var fontTitulo = new XFont("Arial", 12);
            var font = new XFont("Arial", 10);

            //Data Geração
            var dataGeracao = DateTime.Now.ToString("dd/MM/yyyy_HH:mm:ss");

            var status = "";

            if (Util.metodoConfigurado)
            {
                status = Util.caminhoDiretório.Contains("[Error]") ? "FALHOU" : "SUCESSO";
            }


            //Inclui o Logo Tipo do TJ
            //string startupPath = System.IO.Directory.GetParent(@"../../").FullName;// AppDomain.CurrentDomain.BaseDirectory;// Environment.CurrentDirectory;
            //Debug.Print("EdyTest->" + startupPath);
            //var arqLogoTipoTj = @"C:\Users\" + Environment.UserName + @"\source\repos\HolosTesteIntegrado\MPS.HOLOS.TestesIntegrados.CrossBrowser\Massa\Img\LogoTJ.png";
            //var arqLogoTipoTj = @"C:\Imagens\LogoTJ.png";
            var arqLogoTipoTj = @"C:\Arquivos\Imagens\LogoTJ.png";

            if (File.Exists(arqLogoTipoTj))
            {
                //(local de desenho da Imagem (y=50,x=10,Width=170, Height=50)
                gfx.DrawImage(XImage.FromFile(arqLogoTipoTj), 50, 10, 100, 50);

            }

            //Inclui o LogoTipo da Mps
            //var arqLogoTipoMps = @"C:\Users\" + Environment.UserName + @"\source\repos\HolosTesteIntegrado\MPS.HOLOS.TestesIntegrados.CrossBrowser\Massa\Img\logo-mps-small.png";
            var arqLogoTipoMps = @"C:\Arquivos\Imagens\logo-mps-small.png";
            if (File.Exists(arqLogoTipoMps))
            {
                gfx.DrawImage(XImage.FromFile(arqLogoTipoMps), page.Width - 450, 10, 400, 48);
            }

            //Inclui o Cabeçalho
            linha += 75;
            gfx.DrawString("Relatório de Evidências ", fontTitulo, XBrushes.Black, 50, linha, XStringFormats.Default);
            linha += 15;

            gfx.DrawString(String.Format("Data :  {0}  ", dataGeracao), fontTitulo, XBrushes.Black, 50, linha, XStringFormats.Default);
            linha += 15;

            gfx.DrawString(String.Format("Ambiente : {0} ", Ambiente), fontTitulo, XBrushes.Black, 50, linha, XStringFormats.Default);
            linha += 15;

            var rect = new XRect() { X = 150, Y = linha - 10, Height = 15, Width = 300 };
            XStringFormat format = new XStringFormat() { Alignment = XStringAlignment.Near, LineAlignment = XLineAlignment.Near };
            //gfx.DrawString(String.Format("Status :- {0} ", status), font, XBrushes.Black, 50, linha, XStringFormats.Default);
            gfx.DrawString("Status Execução: ", fontTitulo, XBrushes.Black, 50, linha, XStringFormats.Default);
            var txtStatus = new XTextFormatter(gfx);


            linha += 15;

            if (Util.caminhoDiretório.Contains("[Error]"))
            { txtStatus.DrawString(status, fontTitulo, XBrushes.Red, rect, format); }
            else
            { txtStatus.DrawString(status, fontTitulo, XBrushes.Green, rect, format); }

            gfx.DrawString(String.Format("Caso de Teste: "), fontTitulo, XBrushes.Black, 50, linha, XStringFormats.Default);
            var tf = new XTextFormatter(gfx);

            int altura = 0;
            if (testCase.ToString().Length <= 60)
            {
                altura = 15;
            }
            else if (testCase.ToString().Length > 60 && testCase.ToString().Length <= 130)
            {
                altura = 30;
                linha += 15;
            }
            else if (testCase.ToString().Length > 130 && testCase.ToString().Length < 190)
            {
                altura = 45;
                linha += 30;
            }
            else
            {
                altura = 60;
                linha += 45;
            }

            //var rect2 = new XRect(140, 95, 400, 100);
            rect = new XRect();
            if (testCase.ToString().Length <= 60)
            {
                rect.X = 140;
                rect.Y = linha - 10;
                rect.Width = 400;
                rect.Height = altura;
            }
            else
            {
                rect.X = 140;
                rect.Y = linha - 25;
                rect.Width = 400;
                rect.Height = altura;

            }
            XPen xpen = new XPen(XColors.Transparent, 0.4);

            gfx.DrawRectangle(xpen, rect);
            format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;

            XBrush brush = XBrushes.Black;
            tf.DrawString(testCase, font, brush, new XRect(rect.X + 5, rect.Y, rect.Width - 50, altura), format);

            //Desenha linha
            linha += 5;

            gfx.DrawLine(XPens.LightGray, 50, linha, page.Width - 50, linha);
            return linha + 50;
        }
        #endregion

        #region Configura Arquivo
        public string ConfiguraArquivo(string testCase)
        {
            //if (!Directory.Exists(Util.caminhoDiretório))
            //{
            //    Directory.CreateDirectory(Util.caminhoDiretório);
            //}
            string arquivoPdf = (Util.renomeiarPasta ? Util.caminhoDiretório : Util.caminhoDiretório.Replace("[Error]", "")) + @"\" + testCase.Replace(":", "_") + ".pdf";
            return arquivoPdf;
        }
        #endregion


        #region Cria Arquivo Incluir Imagens e Salva
        public void CriaPdfEvidencia(string testCase, string Ambiente = "")
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            //Cria Diretorio
            //Util.CriaDiretorioEvidencias();

            //Configura o arquivo Pdf
            string fileName = ConfiguraArquivo(ScreenShotFields.TestName + ScreenShotFields.Token).ToString();

            //Inclui Cabeçalho
            IncluirCabecalho(gfx, testCase, page, doc, Ambiente);

            //Captura a tela e inclui no arquivo
            IncluirImagemPdf(gfx, doc, testCase, page);//.ToString();

            //Salva o arquivo
            SalvarPdf(fileName, doc);

            gfx.Dispose();
        }
        #endregion

        #endregion

        public void Dispose()
        {
            //Console.WriteLine("Disposed!");
        }
    }
}
