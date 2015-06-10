using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WAReporter.Modelo;
using WAReporter.Utilitarios;

namespace WAReporter
{
    public static class Midia
    {
        public static List<String> CaminhoImagens { get; set; }
        public static List<String> CaminhoVideos { get; set; }
        public static List<String> CaminhoAudios { get; set; }

        public static string Procurar(string caminhoPastaMidias)
        {
            var resultado = "";
            
            string mask = Path.Combine(caminhoPastaMidias, "*.*");
            CaminhoImagens = UtilitariosArquivo.ObterArquivos(mask, (p) => Path.GetExtension(p.Name) == ".jpg").ToList();
            CaminhoVideos = UtilitariosArquivo.ObterArquivos(mask, (p) => Path.GetExtension(p.Name) == ".mp4").ToList();
            CaminhoAudios = UtilitariosArquivo.ObterArquivos(mask, (p) => Path.GetExtension(p.Name) == ".aac").ToList();

            //Muda os caminhos de absolutos para relativos, em relação à pasta de banco de dados (também é a pasta em que o relatório html é gerado)
            for(int i = 0; i < CaminhoImagens.Count; i++) CaminhoImagens[i] = CaminhoImagens[i].Replace(caminhoPastaMidias, "..");
            for(int i = 0; i < CaminhoVideos.Count; i++) CaminhoVideos[i] = CaminhoVideos[i].Replace(caminhoPastaMidias, "..");
            for(int i = 0; i < CaminhoAudios.Count; i++) CaminhoAudios[i] = CaminhoAudios[i].Replace(caminhoPastaMidias, "..");

            return resultado;
        }

        public static String ObterImagemDaMensagem(Message mensagem)
        {
            var propriedadesImagemMensagem = mensagem.ThumbImage;
            if (propriedadesImagemMensagem.Contains("IMG"))
            {
                var nomeArquivoImagem = propriedadesImagemMensagem.Substring(propriedadesImagemMensagem.IndexOf("IMG"), 23);
                return CaminhoImagens.FirstOrDefault(p => p.Contains(nomeArquivoImagem)) ?? "";
            }
            else                
                return "data:image/jpg;base64,"+ Convert.ToBase64String(mensagem.RawData);
        }
    }
}
