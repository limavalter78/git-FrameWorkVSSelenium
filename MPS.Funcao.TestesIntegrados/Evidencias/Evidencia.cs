using MPS.Funcao.TestesIntegrados.Evidencias;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace MPS.Funcao.TestesIntegrados.Evidencias
{
    public static class Evidencia
    {
        private static LogEvidencias log;
        private static bool isInicializado;
        private static string NomeCenarioTeste;
        private static string IdTFS;
        private static string NomeArquivo;
        private static TipoEvidencias TipoEvidencia;
        private static FormatoEvidencia FormatoEvidencia;
        private static string Diretorio;

        public static void Iniciar(IWebDriver webDriver)
        {
            if (!isInicializado)
            {
                log = new LogEvidencias(webDriver);
                isInicializado = true;
            }
        }

        public static void ParametrizaEvidencia(string nomeCenarioTeste, string idTFS, string nomeArquivo, TipoEvidencias tipoEvidencia = TipoEvidencias.PADRÃO_SEMPRE, FormatoEvidencia formato = FormatoEvidencia.HTML, string diretorio = "")
        {
            NomeCenarioTeste = nomeCenarioTeste;
            IdTFS = idTFS;
            NomeArquivo = nomeArquivo;
            TipoEvidencia = tipoEvidencia;
            FormatoEvidencia = formato;
            Diretorio = diretorio;
        }

        public static void GerarEvidencia(string mensagem, string valorEsperado, string valorObtido, bool status)
        {
            log.GerarEvidencia(mensagem, valorEsperado, valorObtido, status);
        }

        public static void GerarEvidencia(string mensagem, string valorEsperado, string valorObtido, string testFolder, bool status)
        {
            log.GerarEvidencia(mensagem, valorEsperado, valorObtido, status);
        }

        public static List<EvidenciaDTO> GetEvidencias()
        {
            return log.GetEvidencias();
        }

        public static void GerarEvidenciaElement(IWebElement webElement, string mensagem, string valorEsperado, string valorObtido, bool status)
        {
            log.GerarEvidenciaElement(webElement, mensagem, valorEsperado, valorObtido, status);
        }

        public static void Finalizar()
        {
            if (isInicializado)
            {
                if (string.IsNullOrWhiteSpace(NomeCenarioTeste))
                    NomeCenarioTeste = "Cenário de testes Não nomeado";

                if (string.IsNullOrWhiteSpace(NomeArquivo))
                    NomeArquivo = "Arquivo_Teste";

                if (string.IsNullOrWhiteSpace(IdTFS))
                    IdTFS = "00000";

                switch (FormatoEvidencia)
                {
                    case FormatoEvidencia.HTML:

                        GeraHtmlEvidencia.GeraHtml(NomeCenarioTeste, IdTFS, NomeArquivo, TipoEvidencia, log, Diretorio);
                        break;
                    default:
                        Console.WriteLine("");
                        Console.WriteLine("*----------------------------------------*");
                        Console.WriteLine("Formato de Evidencia não definido,");
                        Console.WriteLine("por favor escolha um formato disponivel");
                        Console.WriteLine("*----------------------------------------*");
                        break;
                }
                isInicializado = false;
            }
        }
    }
}
