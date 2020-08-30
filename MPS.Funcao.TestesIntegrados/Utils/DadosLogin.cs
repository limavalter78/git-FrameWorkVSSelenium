using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Funcao.TestesIntegrados.Utils
{
    public static class DadosLogin
    {
        public static string Login
        {
            get
            {
                return ConfigurationManager.AppSettings["H8/V2pwAVby7hwGAaARTSw=="].ToString();
            }
        }

        public static string Senha
        {
            get
            {
                return ConfigurationManager.AppSettings["DF4mfxyvK8fMR5MMSizIjQ=="].ToString();
            }
        }


    }
}
