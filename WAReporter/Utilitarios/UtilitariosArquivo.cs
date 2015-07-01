using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WAReporter.Utilitarios
{
    public static class UtilitariosArquivo
    {
        /// <summary>
        /// Lista todos os arquivos presentes em determinado caminho, incluindo subpastas
        /// </summary>
        /// <param name="caminho">String contendo o caminho do diretório desejado</param>
        /// <param name="checarArquivo">Filtro de arquivo (extensão, nome, etc.)</param>
        /// <returns>Lista com os caminhos dos arquivos que atendem o critério contidos no caminho desejado.</returns>
        public static IEnumerable<string> ObterArquivos(string caminho, Func<FileInfo, bool> checarArquivo = null)
        {
            // var lastQuarter = DateTime.Now.AddMonths(-3);
            //list = UT.GetAllFiles(mask, (info) => info.CreationTime >= lastQuarter).ToList();

            string mascara = Path.GetFileName(caminho);
            if (string.IsNullOrEmpty(mascara))
                mascara = "*.*";
            caminho = Path.GetDirectoryName(caminho);
            string[] arquivos = Directory.GetFiles(caminho, mascara, SearchOption.AllDirectories);
            foreach (string arquivo in arquivos)
            {
                if (checarArquivo == null || checarArquivo(new FileInfo(arquivo)))
                    yield return arquivo;
            }
        }


        public static void CopiarDiretorio(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            foreach (string folder in Directory.GetDirectories(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(folder));
                CopiarDiretorio(folder, dest);
            }
        }

        public static bool Descriptografar(string inputFile, string outputFile)
        {
            string password = @"346a23652a46392b4d73257c67317e352e3372482177652c"; // Your Key Here
            
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(password);

                    /* This is for demostrating purposes only. 
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(password);

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
