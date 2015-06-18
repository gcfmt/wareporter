using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            CaminhoFotoPessoal = "";
            CaminhoAvatares = new List<String>();

            var audiosAmr = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".amr");
            foreach(var audioAmr in audiosAmr)
            {
                var startInfo = new ProcessStartInfo();
                startInfo.Arguments = "-i \""+audioAmr+"\" -ar 22050 \""+audioAmr.Replace(".amr", ".mp3")+"\" -y";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                var process = new Process();
                process.StartInfo = startInfo;
                startInfo.FileName = "C:\\ffmpeg\\bin\\ffmpeg.exe";
                process.Start();
            }

            var audios3ga = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".3ga");
            foreach (var audio3ga in audios3ga)
            {
                var startInfo = new ProcessStartInfo();
                startInfo.Arguments = "-i \"" + audio3ga + "\" -ar 22050 \"" + audio3ga.Replace(".3ga", ".mp3") + "\" -y";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                var process = new Process();
                process.StartInfo = startInfo;
                startInfo.FileName = "C:\\ffmpeg\\bin\\ffmpeg.exe";
                process.Start();
            }


            var arquivosVideo =  UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".mp4").ToList();
            foreach(var arquivoVideo in arquivosVideo)
            {
                if (arquivoVideo.EndsWith("_.mp4") || File.Exists(arquivoVideo.Replace(".mp4", "_.mp4"))) continue;

                var arq = UtilitariosVideo.GetVideoMetadata("C:\\ffmpeg\\bin\\ffmpeg.exe", arquivoVideo);
                var i = arq.IndexOf("Stream #0:0") + 24;
                var j = arq.IndexOf("Stream #0:1");

                if (!arq.Substring(i, j-i).Contains("h264"))
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.Arguments = "-i \"" + arquivoVideo + "\" -an -vcodec libx264 -crf 23 \""+ arquivoVideo.Replace(".mp4", "_.mp4") + "\"";
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    var process = new Process();
                    process.StartInfo = startInfo;
                    startInfo.FileName = "C:\\ffmpeg\\bin\\ffmpeg.exe";
                    process.Start();
                }
            }



            CaminhoImagens = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".jpg").ToList();
            CaminhoVideos = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".mp4").ToList();
            CaminhoAudios = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".aac" || Path.GetExtension(p.Name) == ".m4a" || Path.GetExtension(p.Name) == ".mp3" || Path.GetExtension(p.Name) == ".amr" || Path.GetExtension(p.Name) == ".3ga").ToList();

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
                var caminho = CaminhoImagens.FirstOrDefault(p => p.Contains(nomeArquivoImagem));
                    return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"" + caminho + "\">";
            }
            else                
                return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
        }

        public static String ObterAvatar(String Id)
        {
            return CaminhoAvatares.FirstOrDefault(p => p.Contains(Id)) ?? "";
        }

        public static string ObterAudioDaMensagem(Message mensagem)
        {
            var dataPesquisa = mensagem.Timestamp.Date;
            while(dataPesquisa < DateTime.Today)
            {
                foreach(var audio in CaminhoAudios.Where(p => p.Contains(dataPesquisa.ToString("yyyyMMdd"))))
                {
                    var fi = new FileInfo(Path.Combine(CaminhoPastaWhatsApp, audio.Replace("..\\", "")));
                    if (fi.Length == mensagem.MediaSize)
                            return "<audio width=\"480\" controls><source src=\"" + audio.Replace(".amr", ".mp3").Replace(".3ga", ".mp3") + "\">Seu navegador não suporta áudio HTML5.</audio>";
                }
                dataPesquisa = dataPesquisa.AddDays(1);
            }

            return "<span style=\"color: red\" font-weight:bold>ARQUIVO DE ÁUDIO AUSENTE</span>";
        }

        //public static string ObterVideoDaMensagem2(Message mensagem)
        //{
           
        //}

        public static string ObterVideoDaMensagem(Message mensagem)
        {
            var conteudoHtml = "";
            if(mensagem.ThumbImage.Contains("VID-"))
            {
                var nomeArquivo = mensagem.ThumbImage.Substring(mensagem.ThumbImage.IndexOf("VID-"), 23);

                var caminhoVideo = CaminhoVideos.FirstOrDefault(p => p.Contains(nomeArquivo));

                if (!String.IsNullOrWhiteSpace(caminhoVideo))
                {
                    if (File.Exists(Path.Combine(CaminhoPastaWhatsApp, caminhoVideo.Replace("..\\", "").Replace(".mp4", "_.mp4"))))
                        conteudoHtml = "<video width=\"480\" controls><source src=\"" + caminhoVideo.Replace(".mp4", "_.mp4") + "\" type=\"video/mp4\">Seu navegador não suporta vídeo HTML5.</video>";
                    else
                        conteudoHtml = "<video width=\"480\" controls><source src=\"" + caminhoVideo + "\" type=\"video/mp4\">Seu navegador não suporta vídeo HTML5.</video>";
                }
            }
            
            var dataPesquisa = mensagem.Timestamp.Date;
            while (dataPesquisa < DateTime.Today)
            {
                foreach (var video in CaminhoVideos.Where(p => p.Contains(dataPesquisa.ToString("yyyyMMdd"))))
                {
                    var fi = new FileInfo(Path.Combine(CaminhoPastaWhatsApp, video.Replace("..\\", "")));
                    if (fi.Length == mensagem.MediaSize)
                        if (File.Exists(Path.Combine(CaminhoPastaWhatsApp, video.Replace("..\\", "").Replace(".mp4", "_.mp4"))))
                            conteudoHtml = "<video width=\"480\" controls><source src=\"" + video.Replace(".mp4", "_.mp4") + "\" type=\"video/mp4\">Seu navegador não suporta vídeo HTML5.</video>";
                        else
                            conteudoHtml = "<video width=\"480\" controls><source src=\"" + video + "\" type=\"video/mp4\">Seu navegador não suporta vídeo HTML5.</video>";
                }
                dataPesquisa = dataPesquisa.AddDays(1);
            }



            if(conteudoHtml == "")
            {
                conteudoHtml = "<span style=\"color: red\" font-weight:bold>ARQUIVO DE VÍDEO AUSENTE</span>";
                //conteudoHtml = "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
            }
            if (!String.IsNullOrWhiteSpace(mensagem.MediaCaption))
                conteudoHtml += "<span style=\"color: green; margin-left:5px; margin-right:5px;\" font-weight=bold>LEGENDA DO VÍDEO: " + mensagem.MediaCaption+"</span>";

            return conteudoHtml;
        }
    }
}
