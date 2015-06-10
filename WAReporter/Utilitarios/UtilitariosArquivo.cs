using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
