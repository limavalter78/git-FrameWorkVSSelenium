using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Funcao.TestesIntegrados.Utils
{
    public class Decrypt
    {
        #region :: Declaration 
        private string Chave
        {
            get
            {
                return ConfigurationManager.AppSettings["HZsedx9xqTbn8/SJP5a44A=="].ToString();
            }
        }

        private string IV
        {
            get
            {
                return ConfigurationManager.AppSettings["mXCJODBwcDeDFj8YL0/erQ=="].ToString();
            }
        }

        #endregion

        private byte[] GerarIV(string iv)
        {
            List<string> lstBite = new List<string>();

            lstBite = iv.Split(';').ToList();

            lstBite.RemoveAll(x => string.IsNullOrEmpty(x));

            byte[] retorno = new byte[lstBite.Count];

            int count = 0;

            lstBite.ForEach(x => { retorno.SetValue(Convert.ToByte(x), count); count++; });

            return retorno;
        }

        public string Decrypting(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    byte[] bKey = Convert.FromBase64String(Chave);
                    byte[] bText = Convert.FromBase64String(text);

                    Rijndael rijndael = new RijndaelManaged();

                    rijndael.KeySize = 128;

                    MemoryStream mStream = new MemoryStream();

                    CryptoStream decryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateDecryptor(bKey, GerarIV(IV)),
                        CryptoStreamMode.Write);

                    decryptor.Write(bText, 0, bText.Length);
                    decryptor.FlushFinalBlock();

                    UTF8Encoding utf8 = new UTF8Encoding();
                    var retorno = utf8.GetString(mStream.ToArray());
                    decryptor.Close();

                    return retorno;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        }
    }
}
