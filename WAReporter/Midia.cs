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
        public static List<String> CaminhoAvatares { get; set; }
        public static String CaminhoFotoPessoal { get; set; }
        public static String CaminhoPastaWhatsApp { get; set; }
        public static Dictionary<String, String> PropriedadesUsuario { get; set; }

        public static string Procurar(string caminhoPastaWhatsApp)
        {
            CaminhoPastaWhatsApp = caminhoPastaWhatsApp;
            var resultado = "";
            
            CaminhoImagens = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".jpg").ToList();
            CaminhoVideos = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".mp4").ToList();
            CaminhoAudios = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".aac" || Path.GetExtension(p.Name) == ".m4a").ToList();

            var caminhoPastaFiles = "";
            if (Directory.Exists(Path.Combine(caminhoPastaWhatsApp, "f"))) caminhoPastaFiles = Path.Combine(caminhoPastaWhatsApp, "f");
            else if (Directory.Exists(Path.Combine(caminhoPastaWhatsApp, "files"))) caminhoPastaFiles = Path.Combine(caminhoPastaWhatsApp, "files");

            if (File.Exists(Path.Combine(caminhoPastaFiles, "me.jpg"))) CaminhoFotoPessoal = Path.Combine(caminhoPastaFiles, "me.jpg");
        
            if (!String.IsNullOrWhiteSpace(caminhoPastaFiles))
            {
                var pastaAvatars = Path.Combine(caminhoPastaFiles, "Avatars");
                if (Directory.Exists(pastaAvatars))
                    foreach(var arquivoAvatar in Directory.GetFiles(pastaAvatars).Where(p => p.EndsWith(".j")))
                        if(!File.Exists(arquivoAvatar + "pg"))
                            File.Copy(arquivoAvatar, arquivoAvatar + "pg");
                CaminhoAvatares = UtilitariosArquivo.ObterArquivos(Path.Combine(pastaAvatars, "*.*"), (p) => Path.GetExtension(p.Name) == ".jpg").ToList();
            }

            //Muda os caminhos de absolutos para relativos, em relação à pasta de banco de dados (também é a pasta em que o relatório html é gerado)
            for (int i = 0; i < CaminhoImagens.Count; i++) CaminhoImagens[i] = CaminhoImagens[i].Replace(caminhoPastaWhatsApp, "..");
            for (int i = 0; i < CaminhoVideos.Count; i++) CaminhoVideos[i] = CaminhoVideos[i].Replace(caminhoPastaWhatsApp, "..");
            for (int i = 0; i < CaminhoAudios.Count; i++) CaminhoAudios[i] = CaminhoAudios[i].Replace(caminhoPastaWhatsApp, "..");
            for (int i = 0; i < CaminhoAvatares.Count; i++) CaminhoAvatares[i] = CaminhoAvatares[i].Replace(caminhoPastaWhatsApp, "..");
            CaminhoFotoPessoal = CaminhoFotoPessoal.Replace(caminhoPastaWhatsApp, "..");


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

        public static String ObterAvatar(String Id)
        {
            return CaminhoAvatares.FirstOrDefault(p => p.Contains(Id)) ?? "";
        }

        public static string ObterAudioDaMensagem(Message mensagem)
        {
            var dataPesquisa = mensagem.ReceivedTimestamp.Date;
            while(dataPesquisa < DateTime.Today)
            {
                foreach(var audio in CaminhoAudios.Where(p => p.Contains(dataPesquisa.ToString("yyyyMMdd"))))
                {
                    var fi = new FileInfo(Path.Combine(CaminhoPastaWhatsApp, audio.Replace("..\\", "")));
                    if (fi.Length == mensagem.MediaSize)
                        return audio;
                }
                dataPesquisa = dataPesquisa.AddDays(1);
            }

            return "";
        }

        public static string ObterVideoDaMensagem(Message mensagem)
        {
            var dataPesquisa = mensagem.ReceivedTimestamp.Date;
            while (dataPesquisa < DateTime.Today)
            {
                foreach (var audio in CaminhoVideos.Where(p => p.Contains(dataPesquisa.ToString("yyyyMMdd"))))
                {
                    var fi = new FileInfo(Path.Combine(CaminhoPastaWhatsApp, audio.Replace("..\\", "")));
                    if (fi.Length == mensagem.MediaSize)
                        return audio;
                }
                dataPesquisa = dataPesquisa.AddDays(1);
            }

            return "";
        }
    }
}
