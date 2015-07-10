using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        public static String StatusUsuario { get; set; }
        public static String TelefoneUsuario { get; set; }
        public static String NomeUsuario { get; set; }
        public static String CaminhoFotoPessoal { get; set; }
        public static String CaminhoPastaWhatsApp { get; set; }

        public static string Procurar(string caminhoPastaWhatsApp)
        {
            CaminhoPastaWhatsApp = caminhoPastaWhatsApp;
            var resultado = "";

            CaminhoFotoPessoal = "";
            CaminhoAvatares = new List<String>();

            if (Banco.TipoDispositivo == TiposDispositivo.ANDROID)
            {
                var arquivoMeJpg = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => p.Name == "me.jpg");
                if (arquivoMeJpg.Any()) CaminhoFotoPessoal = arquivoMeJpg.First().Replace(caminhoPastaWhatsApp, "..");
            } else if (Banco.TipoDispositivo == TiposDispositivo.IOS)
            {
                var arquivoMeJpg = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => p.Name == "Photo.jpg");
                if (arquivoMeJpg.Any()) CaminhoFotoPessoal = arquivoMeJpg.First().Replace(caminhoPastaWhatsApp, "..");
            }


            if (Banco.TipoDispositivo == TiposDispositivo.ANDROID)
            {                
                var arquivoMe = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => p.Name == "me");
                if(arquivoMe.Any())
                {
                    TelefoneUsuario = File.ReadAllText(arquivoMe.First()).Split('\0')[13].Replace("t", "").Replace("\f", "");
                    var contatoUsuario = Banco.WaContacts.FirstOrDefault(p => p.Jid.Contains(TelefoneUsuario) && !p.Jid.Contains("-"));
                    if(contatoUsuario != null)
                        NomeUsuario =  contatoUsuario.NomeContato;
                }
            }
            else if (Banco.TipoDispositivo == TiposDispositivo.IOS)
            {

                var arquivoMe = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => p.Name == "net.whatsapp.WhatsApp.plist");
                if (arquivoMe.Any())
                {
                    var conteudoArquivoMe = File.ReadAllText(arquivoMe.First());

                    TelefoneUsuario = conteudoArquivoMe.Substring(conteudoArquivoMe.IndexOf("OwnJabberID")).Substring(27);
                    TelefoneUsuario = TelefoneUsuario.Substring(0, TelefoneUsuario.IndexOf("</string>"));

                    NomeUsuario = conteudoArquivoMe.Substring(conteudoArquivoMe.IndexOf("FullUserName")).Substring(28);
                    NomeUsuario = NomeUsuario.Substring(0, NomeUsuario.IndexOf("</string>"));

                    StatusUsuario = conteudoArquivoMe.Substring(conteudoArquivoMe.IndexOf("CurrentStatusText")).Substring(33);
                    StatusUsuario = StatusUsuario.Substring(0, StatusUsuario.IndexOf("</string>"));
                }
            }


            var audiosAmr = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".amr");
            foreach (var audioAmr in audiosAmr)
            {
                var startInfo = new ProcessStartInfo();
                startInfo.Arguments = "-i \"" + audioAmr + "\" -ar 22050 \"" + audioAmr.Replace(".amr", ".mp3") + "\" -y";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                var process = new Process();
                process.StartInfo = startInfo;
                startInfo.FileName = "ffmpeg.exe";
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
                startInfo.FileName = "ffmpeg.exe";
                process.Start();
            }


            var arquivosVideo = UtilitariosArquivo.ObterArquivos(Path.Combine(caminhoPastaWhatsApp, "*.*"), (p) => Path.GetExtension(p.Name) == ".mp4").ToList();
            foreach (var arquivoVideo in arquivosVideo)
            {
                if (arquivoVideo.EndsWith("_.mp4") || File.Exists(arquivoVideo.Replace(".mp4", "_.mp4"))) continue;

                var arq = UtilitariosVideo.GetVideoMetadata("ffmpeg.exe", arquivoVideo);
                var i = arq.IndexOf("Stream #0:0") + 24;
                var j = arq.IndexOf("Stream #0:1");

                if (!arq.Substring(i).Contains("h264"))
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.Arguments = "-i \"" + arquivoVideo + "\" -an -vcodec libx264 -crf 23 \"" + arquivoVideo.Replace(".mp4", "_.mp4") + "\"";
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    var process = new Process();
                    process.StartInfo = startInfo;
                    startInfo.FileName = "ffmpeg.exe";
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

            if(Banco.TipoDispositivo == TiposDispositivo.ANDROID)
            {
                if (!String.IsNullOrWhiteSpace(caminhoPastaFiles))
                {
                    var pastaAvatars = Path.Combine(caminhoPastaFiles, "Avatars");
                    if (Directory.Exists(pastaAvatars))
                        foreach (var arquivoAvatar in Directory.GetFiles(pastaAvatars).Where(p => p.EndsWith(".j")))
                            if (!File.Exists(arquivoAvatar + "pg"))
                                File.Copy(arquivoAvatar, arquivoAvatar + "pg");
                    CaminhoAvatares = UtilitariosArquivo.ObterArquivos(Path.Combine(pastaAvatars, "*.*"), (p) => Path.GetExtension(p.Name) == ".jpg").ToList();
                }
            } else if (Banco.TipoDispositivo == TiposDispositivo.IOS)
            {
                var pastaProfiles = Path.Combine(CaminhoPastaWhatsApp, "Profile");

                if (!String.IsNullOrWhiteSpace(pastaProfiles))
                {

                    foreach (var arquivoAvatar in Directory.GetFiles(pastaProfiles).Where(p => p.EndsWith(".thumb")))
                        if(!File.Exists(arquivoAvatar.Replace(".thumb", ".jpg")))
                            File.Copy(arquivoAvatar, arquivoAvatar.Replace(".thumb", ".jpg"));
                    CaminhoAvatares = UtilitariosArquivo.ObterArquivos(Path.Combine(pastaProfiles, "*.*"), (p) => Path.GetExtension(p.Name) == ".jpg").ToList();
                }
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
            switch(Banco.TipoDispositivo)
            {
                case TiposDispositivo.ANDROID:
                    var propriedadesImagemMensagem = mensagem.ThumbImage;
                    if (propriedadesImagemMensagem.Contains("IMG"))
                    {
                        var nomeArquivoImagem = propriedadesImagemMensagem.Substring(propriedadesImagemMensagem.IndexOf("IMG"), 23);
                        var caminho = CaminhoImagens.FirstOrDefault(p => p.Contains(nomeArquivoImagem));
                        return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"" + caminho + "\">";
                    }
                    else
                        return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
                case TiposDispositivo.IOS:
                    var caminhoRelativoImagem = mensagem.MediaLocalPath;
                    if(!String.IsNullOrWhiteSpace(caminhoRelativoImagem))
                    {
                        var nomeArquivo = Path.GetFileNameWithoutExtension(caminhoRelativoImagem);
                        var caminho = CaminhoImagens.FirstOrDefault(p => p.Contains(nomeArquivo));
                        if(caminho != null)
                            return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"" + caminho + "\">";
                    }

                    var caminhoRemotoImagem = mensagem.MediaUrl;
                    if (caminhoRemotoImagem != null)
                    {
                        if (!Directory.Exists(Path.Combine(CaminhoPastaWhatsApp, "DownloadedMedia")))
                            Directory.CreateDirectory(Path.Combine(CaminhoPastaWhatsApp, "DownloadedMedia"));
                        var nomeArquivoRemotoImagem = Path.Combine(CaminhoPastaWhatsApp, "DownloadedMedia", Path.GetFileName(caminhoRemotoImagem));

                        if (!File.Exists(Path.Combine(CaminhoPastaWhatsApp, "DownloadedMedia", nomeArquivoRemotoImagem)))
                            try
                            {
                                new WebClient().DownloadFile(mensagem.MediaUrl, nomeArquivoRemotoImagem);
                            } catch (Exception ex) { }

                        if (File.Exists(nomeArquivoRemotoImagem))
                        {
                            var caminhoRelativo = nomeArquivoRemotoImagem.Replace(CaminhoPastaWhatsApp, "..");
                            return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"" + caminhoRelativo + "\">";
                        }
                    }

                    var caminhoThumb = mensagem.ThumbImage;
                    if (caminhoThumb != null && File.Exists(Path.Combine(CaminhoPastaWhatsApp, caminhoThumb)))
                    {
                        var caminhoImagemThumb = caminhoThumb.Replace(".thumb", ".jpg");
                        if(!File.Exists(caminhoImagemThumb))
                            File.Copy(caminhoThumb, caminhoThumb.Replace(".thumb", ".jpg"));

                        if(File.Exists(caminhoImagemThumb))
                        {
                            var caminhoRelativoImagemThumb = caminhoImagemThumb.Replace(CaminhoPastaWhatsApp, "..");
                            return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"" + caminhoRelativoImagemThumb + "\">";
                        }
                    }

                    else
                        return "<span style=\"color: red\" font-weight:bold>ARQUIVO DE IMAGEM AUSENTE</span>";
                        //return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
                    break;
            }
            return "";
        }

        public static String ObterAvatar(String Id)
        {
            if (Banco.TipoDispositivo == TiposDispositivo.ANDROID)
                return CaminhoAvatares.LastOrDefault(p => p.Contains(Id)) ?? "";
            else if (Banco.TipoDispositivo == TiposDispositivo.IOS)
                return CaminhoAvatares.LastOrDefault(p => p.Contains(Id.Split('@')[0])) ?? "";
            else return "";
        }

        public static string ObterAudioDaMensagem(Message mensagem)
        {
            switch (Banco.TipoDispositivo)
            {
                case TiposDispositivo.ANDROID:
                    var dataPesquisa = mensagem.Timestamp.Date < mensagem.ReceivedTimestamp ? mensagem.Timestamp.Date : mensagem.ReceivedTimestamp;
                    while (dataPesquisa < DateTime.Today)
                    {
                        foreach (var audio in CaminhoAudios.Where(p => p.Contains(dataPesquisa.ToString("yyyyMMdd"))))
                        {
                            var fi = new FileInfo(Path.Combine(CaminhoPastaWhatsApp, audio.Replace("..\\", "")));
                            if (fi.Length == mensagem.MediaSize)
                                return "<audio width=\"480\" controls><source src=\"" + audio.Replace(".amr", ".mp3").Replace(".3ga", ".mp3") + "\">Seu navegador não suporta áudio HTML5.</audio>";
                        }
                        dataPesquisa = dataPesquisa.AddDays(1);
                    }

                    return "<span style=\"color: red\" font-weight:bold>ARQUIVO DE ÁUDIO AUSENTE</span>";
                case TiposDispositivo.IOS:
                    //var dataPesquisaIOS = mensagem.Timestamp.Date < mensagem.ReceivedTimestamp ? mensagem.Timestamp.Date : mensagem.ReceivedTimestamp;
                    //while (dataPesquisaIOS < DateTime.Today)
                    //{
                    //    foreach (var audio in CaminhoAudios.Where(p => p.Contains(dataPesquisaIOS.ToString("yyyyMMdd"))))
                    //    {
                    //        var fi = new FileInfo(Path.Combine(CaminhoPastaWhatsApp, audio.Replace("..\\", "")));
                    //        if (fi.Length == mensagem.MediaSize)
                    //            return "<audio width=\"480\" controls><source src=\"" + audio.Replace(".amr", ".mp3").Replace(".3ga", ".mp3") + "\">Seu navegador não suporta áudio HTML5.</audio>";
                    //    }
                    //    dataPesquisaIOS = dataPesquisaIOS.AddDays(1);
                    //}

                    //return "<span style=\"color: red\" font-weight:bold>ARQUIVO DE ÁUDIO AUSENTE</span>";

                    var caminhoRelativoAudio = mensagem.MediaLocalPath;
                    if (caminhoRelativoAudio != null)
                    {
                        var nomeArquivo = Path.GetFileName(caminhoRelativoAudio);
                        var caminho = CaminhoAudios.FirstOrDefault(p => p.Contains(nomeArquivo));
                        if (caminho != null)
                            return "<audio width=\"480\" controls><source src=\"" + caminho.Replace(".amr", ".mp3").Replace(".3ga", ".mp3") + "\">Seu navegador não suporta áudio HTML5.</audio>";
                    }
                    return "<span style=\"color: red\" font-weight:bold>ARQUIVO DE ÁUDIO AUSENTE</span>";
                    //return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
                    break;
            }
            return "";
        }
        

        public static string ObterVideoDaMensagem(Message mensagem)
        {
            switch(Banco.TipoDispositivo)
            {
                case TiposDispositivo.ANDROID:
                    var conteudoHtml = "";
                    if (mensagem.ThumbImage.Contains("VID-"))
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

                    var dataPesquisa = mensagem.Timestamp.Date < mensagem.ReceivedTimestamp ? mensagem.Timestamp.Date : mensagem.ReceivedTimestamp;

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



                    if (conteudoHtml == "")
                    {
                        conteudoHtml = "<span style=\"color: red\" font-weight:bold>ARQUIVO DE VÍDEO AUSENTE</span>";
                        //conteudoHtml = "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
                    }
                    if (!String.IsNullOrWhiteSpace(mensagem.MediaCaption))
                        conteudoHtml += "<span style=\"color: green; margin-left:5px; margin-right:5px;\" font-weight=bold>LEGENDA DO VÍDEO: " + mensagem.MediaCaption + "</span>";

                    return conteudoHtml;
                case TiposDispositivo.IOS:
                    var caminhoRelativoVideo = mensagem.MediaLocalPath;
                    if (caminhoRelativoVideo != null)
                    {
                        var nomeArquivo = Path.GetFileName(caminhoRelativoVideo);
                        var caminho = CaminhoVideos.FirstOrDefault(p => p.Contains(nomeArquivo));
                        if (caminho != null)
                            return "<video width=\"480\" controls><source src=\"" + caminho + "\" type=\"video/mp4\">Seu navegador não suporta vídeo HTML5.</video>";

                    }
                    return "<span style=\"color: red\" font-weight:bold>ARQUIVO DE VÍDEO AUSENTE</span>";
                    //return "<img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") + "\" src=\"data:image/jpg;base64," + Convert.ToBase64String(mensagem.RawData) + "\">";
                    break;

            }
            return "";            
        }



        public static String AdicionaEmoji(String texto)
        {
           // return texto;
            //if (!Directory.Exists(Path.Combine(CaminhoPastaWhatsApp, "emoji")))
            //    Directory.


            texto = texto.Replace("\U0001F0CF", "<img src=\"data/emoji_new/1F0CF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F191", "<img src=\"data/emoji_new/1F191.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F193", "<img src=\"data/emoji_new/1F193.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F196", "<img src=\"data/emoji_new/1F196.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F198", "<img src=\"data/emoji_new/1F198.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F232", "<img src=\"data/emoji_new/1F232.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F234", "<img src=\"data/emoji_new/1F234.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F251", "<img src=\"data/emoji_new/1F251.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F301", "<img src=\"data/emoji_new/1F301.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F309", "<img src=\"data/emoji_new/1F309.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F30B", "<img src=\"data/emoji_new/1F30B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F30C", "<img src=\"data/emoji_new/1F30C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F30D", "<img src=\"data/emoji_new/1F30D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F30E", "<img src=\"data/emoji_new/1F30E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F30F", "<img src=\"data/emoji_new/1F30F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F310", "<img src=\"data/emoji_new/1F310.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F311", "<img src=\"data/emoji_new/1F311.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F312", "<img src=\"data/emoji_new/1F312.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F313", "<img src=\"data/emoji_new/1F313.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F314", "<img src=\"data/emoji_new/1F314.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F315", "<img src=\"data/emoji_new/1F315.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F316", "<img src=\"data/emoji_new/1F316.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F317", "<img src=\"data/emoji_new/1F317.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F318", "<img src=\"data/emoji_new/1F318.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F31A", "<img src=\"data/emoji_new/1F31A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F31B", "<img src=\"data/emoji_new/1F31B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F31C", "<img src=\"data/emoji_new/1F31C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F31D", "<img src=\"data/emoji_new/1F31D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F31E", "<img src=\"data/emoji_new/1F31E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F320", "<img src=\"data/emoji_new/1F320.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F330", "<img src=\"data/emoji_new/1F330.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F331", "<img src=\"data/emoji_new/1F331.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F332", "<img src=\"data/emoji_new/1F332.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F333", "<img src=\"data/emoji_new/1F333.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F33C", "<img src=\"data/emoji_new/1F33C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F33D", "<img src=\"data/emoji_new/1F33D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F33F", "<img src=\"data/emoji_new/1F33F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F344", "<img src=\"data/emoji_new/1F344.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F347", "<img src=\"data/emoji_new/1F347.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F348", "<img src=\"data/emoji_new/1F348.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F34B", "<img src=\"data/emoji_new/1F34B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F34C", "<img src=\"data/emoji_new/1F34C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F34D", "<img src=\"data/emoji_new/1F34D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F34F", "<img src=\"data/emoji_new/1F34F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F350", "<img src=\"data/emoji_new/1F350.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F351", "<img src=\"data/emoji_new/1F351.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F352", "<img src=\"data/emoji_new/1F352.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F355", "<img src=\"data/emoji_new/1F355.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F356", "<img src=\"data/emoji_new/1F356.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F357", "<img src=\"data/emoji_new/1F357.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F360", "<img src=\"data/emoji_new/1F360.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F364", "<img src=\"data/emoji_new/1F364.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F365", "<img src=\"data/emoji_new/1F365.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F368", "<img src=\"data/emoji_new/1F368.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F369", "<img src=\"data/emoji_new/1F369.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F36A", "<img src=\"data/emoji_new/1F36A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F36B", "<img src=\"data/emoji_new/1F36B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F36C", "<img src=\"data/emoji_new/1F36C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F36D", "<img src=\"data/emoji_new/1F36D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F36E", "<img src=\"data/emoji_new/1F36E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F36F", "<img src=\"data/emoji_new/1F36F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F377", "<img src=\"data/emoji_new/1F377.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F379", "<img src=\"data/emoji_new/1F379.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F37C", "<img src=\"data/emoji_new/1F37C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F38A", "<img src=\"data/emoji_new/1F38A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F38B", "<img src=\"data/emoji_new/1F38B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3A0", "<img src=\"data/emoji_new/1F3A0.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3A3", "<img src=\"data/emoji_new/1F3A3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3AA", "<img src=\"data/emoji_new/1F3AA.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3AD", "<img src=\"data/emoji_new/1F3AD.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3AE", "<img src=\"data/emoji_new/1F3AE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3B2", "<img src=\"data/emoji_new/1F3B2.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3B3", "<img src=\"data/emoji_new/1F3B3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3B4", "<img src=\"data/emoji_new/1F3B4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3B9", "<img src=\"data/emoji_new/1F3B9.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3BB", "<img src=\"data/emoji_new/1F3BB.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3BC", "<img src=\"data/emoji_new/1F3BC.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3BD", "<img src=\"data/emoji_new/1F3BD.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3C2", "<img src=\"data/emoji_new/1F3C2.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3C7", "<img src=\"data/emoji_new/1F3C7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3C9", "<img src=\"data/emoji_new/1F3C9.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3E1", "<img src=\"data/emoji_new/1F3E1.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3E4", "<img src=\"data/emoji_new/1F3E4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F3EE", "<img src=\"data/emoji_new/1F3EE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F400", "<img src=\"data/emoji_new/1F400.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F401", "<img src=\"data/emoji_new/1F401.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F402", "<img src=\"data/emoji_new/1F402.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F403", "<img src=\"data/emoji_new/1F403.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F404", "<img src=\"data/emoji_new/1F404.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F405", "<img src=\"data/emoji_new/1F405.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F406", "<img src=\"data/emoji_new/1F406.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F407", "<img src=\"data/emoji_new/1F407.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F408", "<img src=\"data/emoji_new/1F408.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F409", "<img src=\"data/emoji_new/1F409.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F40A", "<img src=\"data/emoji_new/1F40A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F40B", "<img src=\"data/emoji_new/1F40B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F40C", "<img src=\"data/emoji_new/1F40C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F40F", "<img src=\"data/emoji_new/1F40F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F410", "<img src=\"data/emoji_new/1F410.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F413", "<img src=\"data/emoji_new/1F413.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F415", "<img src=\"data/emoji_new/1F415.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F416", "<img src=\"data/emoji_new/1F416.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F41C", "<img src=\"data/emoji_new/1F41C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F41D", "<img src=\"data/emoji_new/1F41D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F41E", "<img src=\"data/emoji_new/1F41E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F421", "<img src=\"data/emoji_new/1F421.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F422", "<img src=\"data/emoji_new/1F422.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F423", "<img src=\"data/emoji_new/1F423.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F425", "<img src=\"data/emoji_new/1F425.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F429", "<img src=\"data/emoji_new/1F429.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F42A", "<img src=\"data/emoji_new/1F42A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F432", "<img src=\"data/emoji_new/1F432.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F43C", "<img src=\"data/emoji_new/1F43C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F43D", "<img src=\"data/emoji_new/1F43D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F43E", "<img src=\"data/emoji_new/1F43E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F445", "<img src=\"data/emoji_new/1F445.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F453", "<img src=\"data/emoji_new/1F453.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F456", "<img src=\"data/emoji_new/1F456.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F45A", "<img src=\"data/emoji_new/1F45A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F45B", "<img src=\"data/emoji_new/1F45B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F45D", "<img src=\"data/emoji_new/1F45D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F45E", "<img src=\"data/emoji_new/1F45E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F464", "<img src=\"data/emoji_new/1F464.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F465", "<img src=\"data/emoji_new/1F465.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F46A", "<img src=\"data/emoji_new/1F46A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F46C", "<img src=\"data/emoji_new/1F46C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F46D", "<img src=\"data/emoji_new/1F46D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F470", "<img src=\"data/emoji_new/1F470.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F479", "<img src=\"data/emoji_new/1F479.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F47A", "<img src=\"data/emoji_new/1F47A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F48C", "<img src=\"data/emoji_new/1F48C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F495", "<img src=\"data/emoji_new/1F495.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F496", "<img src=\"data/emoji_new/1F496.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F49E", "<img src=\"data/emoji_new/1F49E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4A0", "<img src=\"data/emoji_new/1F4A0.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4A5", "<img src=\"data/emoji_new/1F4A5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4A7", "<img src=\"data/emoji_new/1F4A7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4AB", "<img src=\"data/emoji_new/1F4AB.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4AC", "<img src=\"data/emoji_new/1F4AC.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4AD", "<img src=\"data/emoji_new/1F4AD.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4AE", "<img src=\"data/emoji_new/1F4AE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4AF", "<img src=\"data/emoji_new/1F4AF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B2", "<img src=\"data/emoji_new/1F4B2.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B3", "<img src=\"data/emoji_new/1F4B3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B4", "<img src=\"data/emoji_new/1F4B4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B5", "<img src=\"data/emoji_new/1F4B5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B6", "<img src=\"data/emoji_new/1F4B6.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B7", "<img src=\"data/emoji_new/1F4B7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4B8", "<img src=\"data/emoji_new/1F4B8.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4BE", "<img src=\"data/emoji_new/1F4BE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C1", "<img src=\"data/emoji_new/1F4C1.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C2", "<img src=\"data/emoji_new/1F4C2.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C3", "<img src=\"data/emoji_new/1F4C3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C4", "<img src=\"data/emoji_new/1F4C4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C5", "<img src=\"data/emoji_new/1F4C5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C6", "<img src=\"data/emoji_new/1F4C6.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C7", "<img src=\"data/emoji_new/1F4C7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C8", "<img src=\"data/emoji_new/1F4C8.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4C9", "<img src=\"data/emoji_new/1F4C9.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4CA", "<img src=\"data/emoji_new/1F4CA.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4CB", "<img src=\"data/emoji_new/1F4CB.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4CC", "<img src=\"data/emoji_new/1F4CC.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4CD", "<img src=\"data/emoji_new/1F4CD.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4CE", "<img src=\"data/emoji_new/1F4CE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4CF", "<img src=\"data/emoji_new/1F4CF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D0", "<img src=\"data/emoji_new/1F4D0.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D1", "<img src=\"data/emoji_new/1F4D1.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D2", "<img src=\"data/emoji_new/1F4D2.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D3", "<img src=\"data/emoji_new/1F4D3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D4", "<img src=\"data/emoji_new/1F4D4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D5", "<img src=\"data/emoji_new/1F4D5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D7", "<img src=\"data/emoji_new/1F4D7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D8", "<img src=\"data/emoji_new/1F4D8.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4D9", "<img src=\"data/emoji_new/1F4D9.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4DA", "<img src=\"data/emoji_new/1F4DA.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4DB", "<img src=\"data/emoji_new/1F4DB.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4DC", "<img src=\"data/emoji_new/1F4DC.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4DE", "<img src=\"data/emoji_new/1F4DE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4DF", "<img src=\"data/emoji_new/1F4DF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4E4", "<img src=\"data/emoji_new/1F4E4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4E5", "<img src=\"data/emoji_new/1F4E5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4E6", "<img src=\"data/emoji_new/1F4E6.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4E7", "<img src=\"data/emoji_new/1F4E7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4E8", "<img src=\"data/emoji_new/1F4E8.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4EA", "<img src=\"data/emoji_new/1F4EA.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4EC", "<img src=\"data/emoji_new/1F4EC.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4ED", "<img src=\"data/emoji_new/1F4ED.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4EF", "<img src=\"data/emoji_new/1F4EF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4F0", "<img src=\"data/emoji_new/1F4F0.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4F5", "<img src=\"data/emoji_new/1F4F5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F4F9", "<img src=\"data/emoji_new/1F4F9.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F500", "<img src=\"data/emoji_new/1F500.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F501", "<img src=\"data/emoji_new/1F501.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F502", "<img src=\"data/emoji_new/1F502.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F503", "<img src=\"data/emoji_new/1F503.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F504", "<img src=\"data/emoji_new/1F504.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F505", "<img src=\"data/emoji_new/1F505.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F506", "<img src=\"data/emoji_new/1F506.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F507", "<img src=\"data/emoji_new/1F507.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F508", "<img src=\"data/emoji_new/1F508.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F509", "<img src=\"data/emoji_new/1F509.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F50B", "<img src=\"data/emoji_new/1F50B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F50C", "<img src=\"data/emoji_new/1F50C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F50E", "<img src=\"data/emoji_new/1F50E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F50F", "<img src=\"data/emoji_new/1F50F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F510", "<img src=\"data/emoji_new/1F510.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F515", "<img src=\"data/emoji_new/1F515.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F516", "<img src=\"data/emoji_new/1F516.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F517", "<img src=\"data/emoji_new/1F517.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F518", "<img src=\"data/emoji_new/1F518.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F519", "<img src=\"data/emoji_new/1F519.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F51A", "<img src=\"data/emoji_new/1F51A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F51B", "<img src=\"data/emoji_new/1F51B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F51C", "<img src=\"data/emoji_new/1F51C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F51F", "<img src=\"data/emoji_new/1F51F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F520", "<img src=\"data/emoji_new/1F520.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F521", "<img src=\"data/emoji_new/1F521.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F522", "<img src=\"data/emoji_new/1F522.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F523", "<img src=\"data/emoji_new/1F523.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F524", "<img src=\"data/emoji_new/1F524.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F526", "<img src=\"data/emoji_new/1F526.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F527", "<img src=\"data/emoji_new/1F527.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F529", "<img src=\"data/emoji_new/1F529.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F52A", "<img src=\"data/emoji_new/1F52A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F52C", "<img src=\"data/emoji_new/1F52C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F52D", "<img src=\"data/emoji_new/1F52D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F52E", "<img src=\"data/emoji_new/1F52E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F535", "<img src=\"data/emoji_new/1F535.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F536", "<img src=\"data/emoji_new/1F536.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F537", "<img src=\"data/emoji_new/1F537.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F538", "<img src=\"data/emoji_new/1F538.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F539", "<img src=\"data/emoji_new/1F539.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F53A", "<img src=\"data/emoji_new/1F53A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F53B", "<img src=\"data/emoji_new/1F53B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F53C", "<img src=\"data/emoji_new/1F53C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F53D", "<img src=\"data/emoji_new/1F53D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F55C", "<img src=\"data/emoji_new/1F55C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F55D", "<img src=\"data/emoji_new/1F55D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F55E", "<img src=\"data/emoji_new/1F55E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F55F", "<img src=\"data/emoji_new/1F55F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F560", "<img src=\"data/emoji_new/1F560.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F561", "<img src=\"data/emoji_new/1F561.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F562", "<img src=\"data/emoji_new/1F562.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F563", "<img src=\"data/emoji_new/1F563.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F564", "<img src=\"data/emoji_new/1F564.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F565", "<img src=\"data/emoji_new/1F565.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F566", "<img src=\"data/emoji_new/1F566.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F567", "<img src=\"data/emoji_new/1F567.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F5FE", "<img src=\"data/emoji_new/1F5FE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F5FF", "<img src=\"data/emoji_new/1F5FF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F600", "<img src=\"data/emoji_new/1F600.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F605", "<img src=\"data/emoji_new/1F605.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F606", "<img src=\"data/emoji_new/1F606.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F607", "<img src=\"data/emoji_new/1F607.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F608", "<img src=\"data/emoji_new/1F608.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F60B", "<img src=\"data/emoji_new/1F60B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F60E", "<img src=\"data/emoji_new/1F60E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F610", "<img src=\"data/emoji_new/1F610.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F611", "<img src=\"data/emoji_new/1F611.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F615", "<img src=\"data/emoji_new/1F615.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F617", "<img src=\"data/emoji_new/1F617.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F619", "<img src=\"data/emoji_new/1F619.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F61B", "<img src=\"data/emoji_new/1F61B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F61F", "<img src=\"data/emoji_new/1F61F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F624", "<img src=\"data/emoji_new/1F624.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F626", "<img src=\"data/emoji_new/1F626.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F627", "<img src=\"data/emoji_new/1F627.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F629", "<img src=\"data/emoji_new/1F629.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F62B", "<img src=\"data/emoji_new/1F62B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F62C", "<img src=\"data/emoji_new/1F62C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F62E", "<img src=\"data/emoji_new/1F62E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F62F", "<img src=\"data/emoji_new/1F62F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F634", "<img src=\"data/emoji_new/1F634.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F635", "<img src=\"data/emoji_new/1F635.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F636", "<img src=\"data/emoji_new/1F636.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F638", "<img src=\"data/emoji_new/1F638.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F639", "<img src=\"data/emoji_new/1F639.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F63A", "<img src=\"data/emoji_new/1F63A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F63B", "<img src=\"data/emoji_new/1F63B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F63C", "<img src=\"data/emoji_new/1F63C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F63D", "<img src=\"data/emoji_new/1F63D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F63E", "<img src=\"data/emoji_new/1F63E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F63F", "<img src=\"data/emoji_new/1F63F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F640", "<img src=\"data/emoji_new/1F640.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F648", "<img src=\"data/emoji_new/1F648.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F649", "<img src=\"data/emoji_new/1F649.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F64A", "<img src=\"data/emoji_new/1F64A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F64B", "<img src=\"data/emoji_new/1F64B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F64D", "<img src=\"data/emoji_new/1F64D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F64E", "<img src=\"data/emoji_new/1F64E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F681", "<img src=\"data/emoji_new/1F681.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F682", "<img src=\"data/emoji_new/1F682.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F686", "<img src=\"data/emoji_new/1F686.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F688", "<img src=\"data/emoji_new/1F688.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F68A", "<img src=\"data/emoji_new/1F68A.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F68B", "<img src=\"data/emoji_new/1F68B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F68D", "<img src=\"data/emoji_new/1F68D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F68E", "<img src=\"data/emoji_new/1F68E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F690", "<img src=\"data/emoji_new/1F690.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F694", "<img src=\"data/emoji_new/1F694.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F696", "<img src=\"data/emoji_new/1F696.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F698", "<img src=\"data/emoji_new/1F698.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F69B", "<img src=\"data/emoji_new/1F69B.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F69C", "<img src=\"data/emoji_new/1F69C.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F69D", "<img src=\"data/emoji_new/1F69D.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F69E", "<img src=\"data/emoji_new/1F69E.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F69F", "<img src=\"data/emoji_new/1F69F.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6A0", "<img src=\"data/emoji_new/1F6A0.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6A1", "<img src=\"data/emoji_new/1F6A1.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6A3", "<img src=\"data/emoji_new/1F6A3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6A6", "<img src=\"data/emoji_new/1F6A6.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6A8", "<img src=\"data/emoji_new/1F6A8.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6A9", "<img src=\"data/emoji_new/1F6A9.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6AA", "<img src=\"data/emoji_new/1F6AA.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6AB", "<img src=\"data/emoji_new/1F6AB.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6AE", "<img src=\"data/emoji_new/1F6AE.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6AF", "<img src=\"data/emoji_new/1F6AF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B0", "<img src=\"data/emoji_new/1F6B0.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B1", "<img src=\"data/emoji_new/1F6B1.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B3", "<img src=\"data/emoji_new/1F6B3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B4", "<img src=\"data/emoji_new/1F6B4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B5", "<img src=\"data/emoji_new/1F6B5.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B7", "<img src=\"data/emoji_new/1F6B7.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6B8", "<img src=\"data/emoji_new/1F6B8.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6BF", "<img src=\"data/emoji_new/1F6BF.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6C1", "<img src=\"data/emoji_new/1F6C1.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6C2", "<img src=\"data/emoji_new/1F6C2.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6C3", "<img src=\"data/emoji_new/1F6C3.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6C4", "<img src=\"data/emoji_new/1F6C4.png\" alt=\"\"/>");
            texto = texto.Replace("\U0001F6C5", "<img src=\"data/emoji_new/1F6C5.png\" alt=\"\"/>");
            texto = texto.Replace("\u203C", "<img src=\"data/emoji_new/203C.png\" alt=\"\"/>");
            texto = texto.Replace("\u2049", "<img src=\"data/emoji_new/2049.png\" alt=\"\"/>");
            texto = texto.Replace("\u2139", "<img src=\"data/emoji_new/2139.png\" alt=\"\"/>");
            texto = texto.Replace("\u2194", "<img src=\"data/emoji_new/2194.png\" alt=\"\"/>");
            texto = texto.Replace("\u2195", "<img src=\"data/emoji_new/2195.png\" alt=\"\"/>");
            texto = texto.Replace("\u21A9", "<img src=\"data/emoji_new/21A9.png\" alt=\"\"/>");
            texto = texto.Replace("\u21AA", "<img src=\"data/emoji_new/21AA.png\" alt=\"\"/>");
            texto = texto.Replace("\u231A", "<img src=\"data/emoji_new/231A.png\" alt=\"\"/>");
            texto = texto.Replace("\u231B", "<img src=\"data/emoji_new/231B.png\" alt=\"\"/>");
            texto = texto.Replace("\u23EB", "<img src=\"data/emoji_new/23EB.png\" alt=\"\"/>");
            texto = texto.Replace("\u23EC", "<img src=\"data/emoji_new/23EC.png\" alt=\"\"/>");
            texto = texto.Replace("\u23F0", "<img src=\"data/emoji_new/23F0.png\" alt=\"\"/>");
            texto = texto.Replace("\u23F3", "<img src=\"data/emoji_new/23F3.png\" alt=\"\"/>");
            texto = texto.Replace("\u24C2", "<img src=\"data/emoji_new/24C2.png\" alt=\"\"/>");
            texto = texto.Replace("\u25AA", "<img src=\"data/emoji_new/25AA.png\" alt=\"\"/>");
            texto = texto.Replace("\u25AB", "<img src=\"data/emoji_new/25AB.png\" alt=\"\"/>");
            texto = texto.Replace("\u25FB", "<img src=\"data/emoji_new/25FB.png\" alt=\"\"/>");
            texto = texto.Replace("\u25FC", "<img src=\"data/emoji_new/25FC.png\" alt=\"\"/>");
            texto = texto.Replace("\u25FD", "<img src=\"data/emoji_new/25FD.png\" alt=\"\"/>");
            texto = texto.Replace("\u25FE", "<img src=\"data/emoji_new/25FE.png\" alt=\"\"/>");
            texto = texto.Replace("\u2611", "<img src=\"data/emoji_new/2611.png\" alt=\"\"/>");
            texto = texto.Replace("\u267B", "<img src=\"data/emoji_new/267B.png\" alt=\"\"/>");
            texto = texto.Replace("\u2693", "<img src=\"data/emoji_new/2693.png\" alt=\"\"/>");
            texto = texto.Replace("\u26AA", "<img src=\"data/emoji_new/26AA.png\" alt=\"\"/>");
            texto = texto.Replace("\u26AB", "<img src=\"data/emoji_new/26AB.png\" alt=\"\"/>");
            texto = texto.Replace("\u26C5", "<img src=\"data/emoji_new/26C5.png\" alt=\"\"/>");
            texto = texto.Replace("\u26D4", "<img src=\"data/emoji_new/26D4.png\" alt=\"\"/>");
            texto = texto.Replace("\u2705", "<img src=\"data/emoji_new/2705.png\" alt=\"\"/>");
            texto = texto.Replace("\u2709", "<img src=\"data/emoji_new/2709.png\" alt=\"\"/>");
            texto = texto.Replace("\u270F", "<img src=\"data/emoji_new/270F.png\" alt=\"\"/>");
            texto = texto.Replace("\u2712", "<img src=\"data/emoji_new/2712.png\" alt=\"\"/>");
            texto = texto.Replace("\u2714", "<img src=\"data/emoji_new/2714.png\" alt=\"\"/>");
            texto = texto.Replace("\u2716", "<img src=\"data/emoji_new/2716.png\" alt=\"\"/>");
            texto = texto.Replace("\u2744", "<img src=\"data/emoji_new/2744.png\" alt=\"\"/>");
            texto = texto.Replace("\u2747", "<img src=\"data/emoji_new/2747.png\" alt=\"\"/>");
            texto = texto.Replace("\u274E", "<img src=\"data/emoji_new/274E.png\" alt=\"\"/>");
            texto = texto.Replace("\u2795", "<img src=\"data/emoji_new/2795.png\" alt=\"\"/>");
            texto = texto.Replace("\u2796", "<img src=\"data/emoji_new/2796.png\" alt=\"\"/>");
            texto = texto.Replace("\u2797", "<img src=\"data/emoji_new/2797.png\" alt=\"\"/>");
            texto = texto.Replace("\u27B0", "<img src=\"data/emoji_new/27B0.png\" alt=\"\"/>");
            texto = texto.Replace("\u2934", "<img src=\"data/emoji_new/2934.png\" alt=\"\"/>");
            texto = texto.Replace("\u2935", "<img src=\"data/emoji_new/2935.png\" alt=\"\"/>");
            texto = texto.Replace("\u2B1B", "<img src=\"data/emoji_new/2B1B.png\" alt=\"\"/>");
            texto = texto.Replace("\u2B1C", "<img src=\"data/emoji_new/2B1C.png\" alt=\"\"/>");
            texto = texto.Replace("\u3030", "<img src=\"data/emoji_new/3030.png\" alt=\"\"/>");

            //old emojis
            texto = texto.Replace("\ue415", "<img src=\"data/emoji/e415.png\" alt=\"\"/>");
            texto = texto.Replace("\ue056", "<img src=\"data/emoji/e056.png\" alt=\"\"/>");
            texto = texto.Replace("\ue057", "<img src=\"data/emoji/e057.png\" alt=\"\"/>");
            texto = texto.Replace("\ue414", "<img src=\"data/emoji/e414.png\" alt=\"\"/>");
            texto = texto.Replace("\ue405", "<img src=\"data/emoji/e405.png\" alt=\"\"/>");
            texto = texto.Replace("\ue106", "<img src=\"data/emoji/e106.png\" alt=\"\"/>");
            texto = texto.Replace("\ue418", "<img src=\"data/emoji/e418.png\" alt=\"\"/>");

            texto = texto.Replace("\ue417", "<img src=\"data/emoji/e417.png\" alt=\"\"/>");
            texto = texto.Replace("\ue40d", "<img src=\"data/emoji/e40d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue40a", "<img src=\"data/emoji/e40a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue404", "<img src=\"data/emoji/e404.png\" alt=\"\"/>");
            texto = texto.Replace("\ue105", "<img src=\"data/emoji/e105.png\" alt=\"\"/>");
            texto = texto.Replace("\ue409", "<img src=\"data/emoji/e409.png\" alt=\"\"/>");
            texto = texto.Replace("\ue40e", "<img src=\"data/emoji/e40e.png\" alt=\"\"/>");

            texto = texto.Replace("\ue402", "<img src=\"data/emoji/e402.png\" alt=\"\"/>");
            texto = texto.Replace("\ue108", "<img src=\"data/emoji/e108.png\" alt=\"\"/>");
            texto = texto.Replace("\ue403", "<img src=\"data/emoji/e403.png\" alt=\"\"/>");
            texto = texto.Replace("\ue058", "<img src=\"data/emoji/e058.png\" alt=\"\"/>");
            texto = texto.Replace("\ue407", "<img src=\"data/emoji/e407.png\" alt=\"\"/>");
            texto = texto.Replace("\ue401", "<img src=\"data/emoji/e401.png\" alt=\"\"/>");
            texto = texto.Replace("\ue40f", "<img src=\"data/emoji/e40f.png\" alt=\"\"/>");

            texto = texto.Replace("\ue40b", "<img src=\"data/emoji/e40b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue406", "<img src=\"data/emoji/e406.png\" alt=\"\"/>");
            texto = texto.Replace("\ue413", "<img src=\"data/emoji/e413.png\" alt=\"\"/>");
            texto = texto.Replace("\ue411", "<img src=\"data/emoji/e411.png\" alt=\"\"/>");
            texto = texto.Replace("\ue412", "<img src=\"data/emoji/e412.png\" alt=\"\"/>");
            texto = texto.Replace("\ue410", "<img src=\"data/emoji/e410.png\" alt=\"\"/>");
            texto = texto.Replace("\ue107", "<img src=\"data/emoji/e107.png\" alt=\"\"/>");

            texto = texto.Replace("\ue059", "<img src=\"data/emoji/e059.png\" alt=\"\"/>");
            texto = texto.Replace("\ue416", "<img src=\"data/emoji/e416.png\" alt=\"\"/>");
            texto = texto.Replace("\ue408", "<img src=\"data/emoji/e408.png\" alt=\"\"/>");
            texto = texto.Replace("\ue40c", "<img src=\"data/emoji/e40c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue11a", "<img src=\"data/emoji/e11a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue10c", "<img src=\"data/emoji/e10c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue32c", "<img src=\"data/emoji/e32c.png\" alt=\"\"/>");

            texto = texto.Replace("\ue32a", "<img src=\"data/emoji/e32a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue32d", "<img src=\"data/emoji/e32d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue328", "<img src=\"data/emoji/e328.png\" alt=\"\"/>");
            texto = texto.Replace("\ue32b", "<img src=\"data/emoji/e32b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue022", "<img src=\"data/emoji/e022.png\" alt=\"\"/>");
            texto = texto.Replace("\ue023", "<img src=\"data/emoji/e023.png\" alt=\"\"/>");
            texto = texto.Replace("\ue327", "<img src=\"data/emoji/e327.png\" alt=\"\"/>");

            texto = texto.Replace("\ue329", "<img src=\"data/emoji/e329.png\" alt=\"\"/>");
            texto = texto.Replace("\ue32e", "<img src=\"data/emoji/e32e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue32f", "<img src=\"data/emoji/e32f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue335", "<img src=\"data/emoji/e335.png\" alt=\"\"/>");
            texto = texto.Replace("\ue334", "<img src=\"data/emoji/e334.png\" alt=\"\"/>");
            texto = texto.Replace("\ue021", "<img src=\"data/emoji/e021.png\" alt=\"\"/>");
            texto = texto.Replace("\ue337", "<img src=\"data/emoji/e337.png\" alt=\"\"/>");

            texto = texto.Replace("\ue020", "<img src=\"data/emoji/e020.png\" alt=\"\"/>");
            texto = texto.Replace("\ue336", "<img src=\"data/emoji/e336.png\" alt=\"\"/>");
            texto = texto.Replace("\ue13c", "<img src=\"data/emoji/e13c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue330", "<img src=\"data/emoji/e330.png\" alt=\"\"/>");
            texto = texto.Replace("\ue331", "<img src=\"data/emoji/e331.png\" alt=\"\"/>");
            texto = texto.Replace("\ue326", "<img src=\"data/emoji/e326.png\" alt=\"\"/>");
            texto = texto.Replace("\ue03e", "<img src=\"data/emoji/e03e.png\" alt=\"\"/>");

            texto = texto.Replace("\ue11d", "<img src=\"data/emoji/e11d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue05a", "<img src=\"data/emoji/e05a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue00e", "<img src=\"data/emoji/e00e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue421", "<img src=\"data/emoji/e421.png\" alt=\"\"/>");
            texto = texto.Replace("\ue420", "<img src=\"data/emoji/e420.png\" alt=\"\"/>");
            texto = texto.Replace("\ue00d", "<img src=\"data/emoji/e00d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue010", "<img src=\"data/emoji/e010.png\" alt=\"\"/>");

            texto = texto.Replace("\ue011", "<img src=\"data/emoji/e011.png\" alt=\"\"/>");
            texto = texto.Replace("\ue41e", "<img src=\"data/emoji/e41e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue012", "<img src=\"data/emoji/e012.png\" alt=\"\"/>");
            texto = texto.Replace("\ue422", "<img src=\"data/emoji/e422.png\" alt=\"\"/>");
            texto = texto.Replace("\ue22e", "<img src=\"data/emoji/e22e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue22f", "<img src=\"data/emoji/e22f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue231", "<img src=\"data/emoji/e231.png\" alt=\"\"/>");

            texto = texto.Replace("\ue230", "<img src=\"data/emoji/e230.png\" alt=\"\"/>");
            texto = texto.Replace("\ue427", "<img src=\"data/emoji/e427.png\" alt=\"\"/>");
            texto = texto.Replace("\ue41d", "<img src=\"data/emoji/e41d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue00f", "<img src=\"data/emoji/e00f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue41f", "<img src=\"data/emoji/e41f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue14c", "<img src=\"data/emoji/e14c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue201", "<img src=\"data/emoji/e201.png\" alt=\"\"/>");

            texto = texto.Replace("\ue115", "<img src=\"data/emoji/e115.png\" alt=\"\"/>");
            texto = texto.Replace("\ue428", "<img src=\"data/emoji/e428.png\" alt=\"\"/>");
            texto = texto.Replace("\ue51f", "<img src=\"data/emoji/e51f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue429", "<img src=\"data/emoji/e429.png\" alt=\"\"/>");
            texto = texto.Replace("\ue424", "<img src=\"data/emoji/e424.png\" alt=\"\"/>");
            texto = texto.Replace("\ue423", "<img src=\"data/emoji/e423.png\" alt=\"\"/>");
            texto = texto.Replace("\ue253", "<img src=\"data/emoji/e253.png\" alt=\"\"/>");

            texto = texto.Replace("\ue426", "<img src=\"data/emoji/e426.png\" alt=\"\"/>");
            texto = texto.Replace("\ue111", "<img src=\"data/emoji/e111.png\" alt=\"\"/>");
            texto = texto.Replace("\ue425", "<img src=\"data/emoji/e425.png\" alt=\"\"/>");
            texto = texto.Replace("\ue31e", "<img src=\"data/emoji/e31e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue31f", "<img src=\"data/emoji/e31f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue31d", "<img src=\"data/emoji/e31d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue001", "<img src=\"data/emoji/e001.png\" alt=\"\"/>");

            texto = texto.Replace("\ue002", "<img src=\"data/emoji/e002.png\" alt=\"\"/>");
            texto = texto.Replace("\ue005", "<img src=\"data/emoji/e005.png\" alt=\"\"/>");
            texto = texto.Replace("\ue004", "<img src=\"data/emoji/e004.png\" alt=\"\"/>");
            texto = texto.Replace("\ue51a", "<img src=\"data/emoji/e51a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue519", "<img src=\"data/emoji/e519.png\" alt=\"\"/>");
            texto = texto.Replace("\ue518", "<img src=\"data/emoji/e518.png\" alt=\"\"/>");
            texto = texto.Replace("\ue515", "<img src=\"data/emoji/e515.png\" alt=\"\"/>");

            texto = texto.Replace("\ue516", "<img src=\"data/emoji/e516.png\" alt=\"\"/>");
            texto = texto.Replace("\ue517", "<img src=\"data/emoji/e517.png\" alt=\"\"/>");
            texto = texto.Replace("\ue51b", "<img src=\"data/emoji/e51b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue152", "<img src=\"data/emoji/e152.png\" alt=\"\"/>");
            texto = texto.Replace("\ue04e", "<img src=\"data/emoji/e04e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue51c", "<img src=\"data/emoji/e51c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue51e", "<img src=\"data/emoji/e51e.png\" alt=\"\"/>");

            texto = texto.Replace("\ue11c", "<img src=\"data/emoji/e11c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue536", "<img src=\"data/emoji/e536.png\" alt=\"\"/>");
            texto = texto.Replace("\ue003", "<img src=\"data/emoji/e003.png\" alt=\"\"/>");
            texto = texto.Replace("\ue41c", "<img src=\"data/emoji/e41c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue41b", "<img src=\"data/emoji/e41b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue419", "<img src=\"data/emoji/e419.png\" alt=\"\"/>");
            texto = texto.Replace("\ue41a", "<img src=\"data/emoji/e41a.png\" alt=\"\"/>");

            texto = texto.Replace("\ue04a", "<img src=\"data/emoji/e04a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue04b", "<img src=\"data/emoji/e04b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue049", "<img src=\"data/emoji/e049.png\" alt=\"\"/>");
            texto = texto.Replace("\ue048", "<img src=\"data/emoji/e048.png\" alt=\"\"/>");
            texto = texto.Replace("\ue04c", "<img src=\"data/emoji/e04c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue13d", "<img src=\"data/emoji/e13d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue443", "<img src=\"data/emoji/e443.png\" alt=\"\"/>");

            texto = texto.Replace("\ue43e", "<img src=\"data/emoji/e43e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue04f", "<img src=\"data/emoji/e04f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue052", "<img src=\"data/emoji/e052.png\" alt=\"\"/>");
            texto = texto.Replace("\ue053", "<img src=\"data/emoji/e053.png\" alt=\"\"/>");
            texto = texto.Replace("\ue524", "<img src=\"data/emoji/e524.png\" alt=\"\"/>");
            texto = texto.Replace("\ue52c", "<img src=\"data/emoji/e52c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue52a", "<img src=\"data/emoji/e52a.png\" alt=\"\"/>");

            texto = texto.Replace("\ue531", "<img src=\"data/emoji/e531.png\" alt=\"\"/>");
            texto = texto.Replace("\ue050", "<img src=\"data/emoji/e050.png\" alt=\"\"/>");
            texto = texto.Replace("\ue527", "<img src=\"data/emoji/e527.png\" alt=\"\"/>");
            texto = texto.Replace("\ue051", "<img src=\"data/emoji/e051.png\" alt=\"\"/>");
            texto = texto.Replace("\ue10b", "<img src=\"data/emoji/e10b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue52b", "<img src=\"data/emoji/e52b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue52f", "<img src=\"data/emoji/e52f.png\" alt=\"\"/>");

            texto = texto.Replace("\ue528", "<img src=\"data/emoji/e528.png\" alt=\"\"/>");
            texto = texto.Replace("\ue01a", "<img src=\"data/emoji/e01a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue134", "<img src=\"data/emoji/e134.png\" alt=\"\"/>");
            texto = texto.Replace("\ue530", "<img src=\"data/emoji/e530.png\" alt=\"\"/>");
            texto = texto.Replace("\ue529", "<img src=\"data/emoji/e529.png\" alt=\"\"/>");
            texto = texto.Replace("\ue526", "<img src=\"data/emoji/e526.png\" alt=\"\"/>");
            texto = texto.Replace("\ue52d", "<img src=\"data/emoji/e52d.png\" alt=\"\"/>");

            texto = texto.Replace("\ue521", "<img src=\"data/emoji/e521.png\" alt=\"\"/>");
            texto = texto.Replace("\ue523", "<img src=\"data/emoji/e523.png\" alt=\"\"/>");
            texto = texto.Replace("\ue52e", "<img src=\"data/emoji/e52e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue055", "<img src=\"data/emoji/e055.png\" alt=\"\"/>");
            texto = texto.Replace("\ue525", "<img src=\"data/emoji/e525.png\" alt=\"\"/>");
            texto = texto.Replace("\ue10a", "<img src=\"data/emoji/e10a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue109", "<img src=\"data/emoji/e109.png\" alt=\"\"/>");

            texto = texto.Replace("\ue522", "<img src=\"data/emoji/e522.png\" alt=\"\"/>");
            texto = texto.Replace("\ue019", "<img src=\"data/emoji/e019.png\" alt=\"\"/>");
            texto = texto.Replace("\ue054", "<img src=\"data/emoji/e054.png\" alt=\"\"/>");
            texto = texto.Replace("\ue520", "<img src=\"data/emoji/e520.png\" alt=\"\"/>");
            texto = texto.Replace("\ue306", "<img src=\"data/emoji/e306.png\" alt=\"\"/>");
            texto = texto.Replace("\ue030", "<img src=\"data/emoji/e030.png\" alt=\"\"/>");
            texto = texto.Replace("\ue304", "<img src=\"data/emoji/e304.png\" alt=\"\"/>");

            texto = texto.Replace("\ue110", "<img src=\"data/emoji/e110.png\" alt=\"\"/>");
            texto = texto.Replace("\ue032", "<img src=\"data/emoji/e032.png\" alt=\"\"/>");
            texto = texto.Replace("\ue305", "<img src=\"data/emoji/e305.png\" alt=\"\"/>");
            texto = texto.Replace("\ue303", "<img src=\"data/emoji/e303.png\" alt=\"\"/>");
            texto = texto.Replace("\ue118", "<img src=\"data/emoji/e118.png\" alt=\"\"/>");
            texto = texto.Replace("\ue447", "<img src=\"data/emoji/e447.png\" alt=\"\"/>");
            texto = texto.Replace("\ue119", "<img src=\"data/emoji/e119.png\" alt=\"\"/>");

            texto = texto.Replace("\ue307", "<img src=\"data/emoji/e307.png\" alt=\"\"/>");
            texto = texto.Replace("\ue308", "<img src=\"data/emoji/e308.png\" alt=\"\"/>");
            texto = texto.Replace("\ue444", "<img src=\"data/emoji/e444.png\" alt=\"\"/>");
            texto = texto.Replace("\ue441", "<img src=\"data/emoji/e441.png\" alt=\"\"/>");

            texto = texto.Replace("\ue436", "<img src=\"data/emoji/e436.png\" alt=\"\"/>");
            texto = texto.Replace("\ue437", "<img src=\"data/emoji/e437.png\" alt=\"\"/>");
            texto = texto.Replace("\ue438", "<img src=\"data/emoji/e438.png\" alt=\"\"/>");
            texto = texto.Replace("\ue43a", "<img src=\"data/emoji/e43a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue439", "<img src=\"data/emoji/e439.png\" alt=\"\"/>");
            texto = texto.Replace("\ue43b", "<img src=\"data/emoji/e43b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue117", "<img src=\"data/emoji/e117.png\" alt=\"\"/>");

            texto = texto.Replace("\ue440", "<img src=\"data/emoji/e440.png\" alt=\"\"/>");
            texto = texto.Replace("\ue442", "<img src=\"data/emoji/e442.png\" alt=\"\"/>");
            texto = texto.Replace("\ue446", "<img src=\"data/emoji/e446.png\" alt=\"\"/>");
            texto = texto.Replace("\ue445", "<img src=\"data/emoji/e445.png\" alt=\"\"/>");
            texto = texto.Replace("\ue11b", "<img src=\"data/emoji/e11b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue448", "<img src=\"data/emoji/e448.png\" alt=\"\"/>");
            texto = texto.Replace("\ue033", "<img src=\"data/emoji/e033.png\" alt=\"\"/>");

            texto = texto.Replace("\ue112", "<img src=\"data/emoji/e112.png\" alt=\"\"/>");
            texto = texto.Replace("\ue325", "<img src=\"data/emoji/e325.png\" alt=\"\"/>");
            texto = texto.Replace("\ue312", "<img src=\"data/emoji/e312.png\" alt=\"\"/>");
            texto = texto.Replace("\ue310", "<img src=\"data/emoji/e310.png\" alt=\"\"/>");
            texto = texto.Replace("\ue126", "<img src=\"data/emoji/e126.png\" alt=\"\"/>");
            texto = texto.Replace("\ue127", "<img src=\"data/emoji/e127.png\" alt=\"\"/>");
            texto = texto.Replace("\ue008", "<img src=\"data/emoji/e008.png\" alt=\"\"/>");

            texto = texto.Replace("\ue03d", "<img src=\"data/emoji/e03d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue00c", "<img src=\"data/emoji/e00c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue12a", "<img src=\"data/emoji/e12a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue00a", "<img src=\"data/emoji/e00a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue00b", "<img src=\"data/emoji/e00b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue009", "<img src=\"data/emoji/e009.png\" alt=\"\"/>");
            texto = texto.Replace("\ue316", "<img src=\"data/emoji/e316.png\" alt=\"\"/>");

            texto = texto.Replace("\ue129", "<img src=\"data/emoji/e129.png\" alt=\"\"/>");
            texto = texto.Replace("\ue141", "<img src=\"data/emoji/e141.png\" alt=\"\"/>");
            texto = texto.Replace("\ue142", "<img src=\"data/emoji/e142.png\" alt=\"\"/>");
            texto = texto.Replace("\ue317", "<img src=\"data/emoji/e317.png\" alt=\"\"/>");
            texto = texto.Replace("\ue128", "<img src=\"data/emoji/e128.png\" alt=\"\"/>");
            texto = texto.Replace("\ue14b", "<img src=\"data/emoji/e14b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue211", "<img src=\"data/emoji/e211.png\" alt=\"\"/>");

            texto = texto.Replace("\ue114", "<img src=\"data/emoji/e114.png\" alt=\"\"/>");
            texto = texto.Replace("\ue145", "<img src=\"data/emoji/e145.png\" alt=\"\"/>");
            texto = texto.Replace("\ue144", "<img src=\"data/emoji/e144.png\" alt=\"\"/>");
            texto = texto.Replace("\ue03f", "<img src=\"data/emoji/e03f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue313", "<img src=\"data/emoji/e313.png\" alt=\"\"/>");
            texto = texto.Replace("\ue116", "<img src=\"data/emoji/e116.png\" alt=\"\"/>");
            texto = texto.Replace("\ue10f", "<img src=\"data/emoji/e10f.png\" alt=\"\"/>");

            texto = texto.Replace("\ue104", "<img src=\"data/emoji/e104.png\" alt=\"\"/>");
            texto = texto.Replace("\ue103", "<img src=\"data/emoji/e103.png\" alt=\"\"/>");
            texto = texto.Replace("\ue101", "<img src=\"data/emoji/e101.png\" alt=\"\"/>");
            texto = texto.Replace("\ue102", "<img src=\"data/emoji/e102.png\" alt=\"\"/>");
            texto = texto.Replace("\ue13f", "<img src=\"data/emoji/e13f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue140", "<img src=\"data/emoji/e140.png\" alt=\"\"/>");
            texto = texto.Replace("\ue11f", "<img src=\"data/emoji/e11f.png\" alt=\"\"/>");

            texto = texto.Replace("\ue12f", "<img src=\"data/emoji/e12f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue031", "<img src=\"data/emoji/e031.png\" alt=\"\"/>");
            texto = texto.Replace("\ue30e", "<img src=\"data/emoji/e30e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue311", "<img src=\"data/emoji/e311.png\" alt=\"\"/>");
            texto = texto.Replace("\ue113", "<img src=\"data/emoji/e113.png\" alt=\"\"/>");
            texto = texto.Replace("\ue30f", "<img src=\"data/emoji/e30f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue13b", "<img src=\"data/emoji/e13b.png\" alt=\"\"/>");

            texto = texto.Replace("\ue42b", "<img src=\"data/emoji/e42b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue42a", "<img src=\"data/emoji/e42a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue018", "<img src=\"data/emoji/e018.png\" alt=\"\"/>");
            texto = texto.Replace("\ue016", "<img src=\"data/emoji/e016.png\" alt=\"\"/>");
            texto = texto.Replace("\ue015", "<img src=\"data/emoji/e015.png\" alt=\"\"/>");
            texto = texto.Replace("\ue014", "<img src=\"data/emoji/e014.png\" alt=\"\"/>");
            texto = texto.Replace("\ue42c", "<img src=\"data/emoji/e42c.png\" alt=\"\"/>");

            texto = texto.Replace("\ue42d", "<img src=\"data/emoji/e42d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue017", "<img src=\"data/emoji/e017.png\" alt=\"\"/>");
            texto = texto.Replace("\ue013", "<img src=\"data/emoji/e013.png\" alt=\"\"/>");
            texto = texto.Replace("\ue20e", "<img src=\"data/emoji/e20e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue20c", "<img src=\"data/emoji/e20c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue20f", "<img src=\"data/emoji/e20f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue20d", "<img src=\"data/emoji/e20d.png\" alt=\"\"/>");

            texto = texto.Replace("\ue131", "<img src=\"data/emoji/e131.png\" alt=\"\"/>");
            texto = texto.Replace("\ue12b", "<img src=\"data/emoji/e12b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue130", "<img src=\"data/emoji/e130.png\" alt=\"\"/>");
            texto = texto.Replace("\ue12d", "<img src=\"data/emoji/e12d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue324", "<img src=\"data/emoji/e324.png\" alt=\"\"/>");
            texto = texto.Replace("\ue301", "<img src=\"data/emoji/e301.png\" alt=\"\"/>");
            texto = texto.Replace("\ue148", "<img src=\"data/emoji/e148.png\" alt=\"\"/>");

            texto = texto.Replace("\ue502", "<img src=\"data/emoji/e502.png\" alt=\"\"/>");
            texto = texto.Replace("\ue03c", "<img src=\"data/emoji/e03c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue30a", "<img src=\"data/emoji/e30a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue042", "<img src=\"data/emoji/e042.png\" alt=\"\"/>");
            texto = texto.Replace("\ue040", "<img src=\"data/emoji/e040.png\" alt=\"\"/>");
            texto = texto.Replace("\ue041", "<img src=\"data/emoji/e041.png\" alt=\"\"/>");
            texto = texto.Replace("\ue12c", "<img src=\"data/emoji/e12c.png\" alt=\"\"/>");

            texto = texto.Replace("\ue007", "<img src=\"data/emoji/e007.png\" alt=\"\"/>");
            texto = texto.Replace("\ue31a", "<img src=\"data/emoji/e31a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue13e", "<img src=\"data/emoji/e13e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue31b", "<img src=\"data/emoji/e31b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue006", "<img src=\"data/emoji/e006.png\" alt=\"\"/>");
            texto = texto.Replace("\ue302", "<img src=\"data/emoji/e302.png\" alt=\"\"/>");
            texto = texto.Replace("\ue319", "<img src=\"data/emoji/e319.png\" alt=\"\"/>");

            texto = texto.Replace("\ue321", "<img src=\"data/emoji/e321.png\" alt=\"\"/>");
            texto = texto.Replace("\ue322", "<img src=\"data/emoji/e322.png\" alt=\"\"/>");
            texto = texto.Replace("\ue314", "<img src=\"data/emoji/e314.png\" alt=\"\"/>");
            texto = texto.Replace("\ue503", "<img src=\"data/emoji/e503.png\" alt=\"\"/>");
            texto = texto.Replace("\ue10e", "<img src=\"data/emoji/e10e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue318", "<img src=\"data/emoji/e318.png\" alt=\"\"/>");
            texto = texto.Replace("\ue43c", "<img src=\"data/emoji/e43c.png\" alt=\"\"/>");

            texto = texto.Replace("\ue11e", "<img src=\"data/emoji/e11e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue323", "<img src=\"data/emoji/e323.png\" alt=\"\"/>");
            texto = texto.Replace("\ue31c", "<img src=\"data/emoji/e31c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue034", "<img src=\"data/emoji/e034.png\" alt=\"\"/>");
            texto = texto.Replace("\ue035", "<img src=\"data/emoji/e035.png\" alt=\"\"/>");
            texto = texto.Replace("\ue045", "<img src=\"data/emoji/e045.png\" alt=\"\"/>");
            texto = texto.Replace("\ue338", "<img src=\"data/emoji/e338.png\" alt=\"\"/>");

            texto = texto.Replace("\ue047", "<img src=\"data/emoji/e047.png\" alt=\"\"/>");
            texto = texto.Replace("\ue30c", "<img src=\"data/emoji/e30c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue044", "<img src=\"data/emoji/e044.png\" alt=\"\"/>");
            texto = texto.Replace("\ue30b", "<img src=\"data/emoji/e30b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue043", "<img src=\"data/emoji/e043.png\" alt=\"\"/>");
            texto = texto.Replace("\ue120", "<img src=\"data/emoji/e120.png\" alt=\"\"/>");
            texto = texto.Replace("\ue33b", "<img src=\"data/emoji/e33b.png\" alt=\"\"/>");

            texto = texto.Replace("\ue33f", "<img src=\"data/emoji/e33f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue341", "<img src=\"data/emoji/e341.png\" alt=\"\"/>");
            texto = texto.Replace("\ue34c", "<img src=\"data/emoji/e34c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue344", "<img src=\"data/emoji/e344.png\" alt=\"\"/>");
            texto = texto.Replace("\ue342", "<img src=\"data/emoji/e342.png\" alt=\"\"/>");
            texto = texto.Replace("\ue33d", "<img src=\"data/emoji/e33d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue33e", "<img src=\"data/emoji/e33e.png\" alt=\"\"/>");

            texto = texto.Replace("\ue340", "<img src=\"data/emoji/e340.png\" alt=\"\"/>");
            texto = texto.Replace("\ue34d", "<img src=\"data/emoji/e34d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue339", "<img src=\"data/emoji/e339.png\" alt=\"\"/>");
            texto = texto.Replace("\ue147", "<img src=\"data/emoji/e147.png\" alt=\"\"/>");
            texto = texto.Replace("\ue343", "<img src=\"data/emoji/e343.png\" alt=\"\"/>");
            texto = texto.Replace("\ue33c", "<img src=\"data/emoji/e33c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue33a", "<img src=\"data/emoji/e33a.png\" alt=\"\"/>");

            texto = texto.Replace("\ue43f", "<img src=\"data/emoji/e43f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue34b", "<img src=\"data/emoji/e34b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue046", "<img src=\"data/emoji/e046.png\" alt=\"\"/>");
            texto = texto.Replace("\ue345", "<img src=\"data/emoji/e345.png\" alt=\"\"/>");
            texto = texto.Replace("\ue346", "<img src=\"data/emoji/e346.png\" alt=\"\"/>");
            texto = texto.Replace("\ue348", "<img src=\"data/emoji/e348.png\" alt=\"\"/>");
            texto = texto.Replace("\ue347", "<img src=\"data/emoji/e347.png\" alt=\"\"/>");

            texto = texto.Replace("\ue34a", "<img src=\"data/emoji/e34a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue349", "<img src=\"data/emoji/e349.png\" alt=\"\"/>");

            texto = texto.Replace("\ue036", "<img src=\"data/emoji/e036.png\" alt=\"\"/>");
            texto = texto.Replace("\ue157", "<img src=\"data/emoji/e157.png\" alt=\"\"/>");
            texto = texto.Replace("\ue038", "<img src=\"data/emoji/e038.png\" alt=\"\"/>");
            texto = texto.Replace("\ue153", "<img src=\"data/emoji/e153.png\" alt=\"\"/>");
            texto = texto.Replace("\ue155", "<img src=\"data/emoji/e155.png\" alt=\"\"/>");
            texto = texto.Replace("\ue14d", "<img src=\"data/emoji/e14d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue156", "<img src=\"data/emoji/e156.png\" alt=\"\"/>");

            texto = texto.Replace("\ue501", "<img src=\"data/emoji/e501.png\" alt=\"\"/>");
            texto = texto.Replace("\ue158", "<img src=\"data/emoji/e158.png\" alt=\"\"/>");
            texto = texto.Replace("\ue43d", "<img src=\"data/emoji/e43d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue037", "<img src=\"data/emoji/e037.png\" alt=\"\"/>");
            texto = texto.Replace("\ue504", "<img src=\"data/emoji/e504.png\" alt=\"\"/>");
            texto = texto.Replace("\ue44a", "<img src=\"data/emoji/e44a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue146", "<img src=\"data/emoji/e146.png\" alt=\"\"/>");

            texto = texto.Replace("\ue50a", "<img src=\"data/emoji/e50a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue505", "<img src=\"data/emoji/e505.png\" alt=\"\"/>");
            texto = texto.Replace("\ue506", "<img src=\"data/emoji/e506.png\" alt=\"\"/>");
            texto = texto.Replace("\ue122", "<img src=\"data/emoji/e122.png\" alt=\"\"/>");
            texto = texto.Replace("\ue508", "<img src=\"data/emoji/e508.png\" alt=\"\"/>");
            texto = texto.Replace("\ue509", "<img src=\"data/emoji/e509.png\" alt=\"\"/>");
            texto = texto.Replace("\ue03b", "<img src=\"data/emoji/e03b.png\" alt=\"\"/>");

            texto = texto.Replace("\ue04d", "<img src=\"data/emoji/e04d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue449", "<img src=\"data/emoji/e449.png\" alt=\"\"/>");
            texto = texto.Replace("\ue44b", "<img src=\"data/emoji/e44b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue51d", "<img src=\"data/emoji/e51d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue44c", "<img src=\"data/emoji/e44c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue124", "<img src=\"data/emoji/e124.png\" alt=\"\"/>");
            texto = texto.Replace("\ue121", "<img src=\"data/emoji/e121.png\" alt=\"\"/>");

            texto = texto.Replace("\ue433", "<img src=\"data/emoji/e433.png\" alt=\"\"/>");
            texto = texto.Replace("\ue202", "<img src=\"data/emoji/e202.png\" alt=\"\"/>");
            texto = texto.Replace("\ue135", "<img src=\"data/emoji/e135.png\" alt=\"\"/>");
            texto = texto.Replace("\ue01c", "<img src=\"data/emoji/e01c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue01d", "<img src=\"data/emoji/e01d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue10d", "<img src=\"data/emoji/e10d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue136", "<img src=\"data/emoji/e136.png\" alt=\"\"/>");

            texto = texto.Replace("\ue42e", "<img src=\"data/emoji/e42e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue01b", "<img src=\"data/emoji/e01b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue15a", "<img src=\"data/emoji/e15a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue159", "<img src=\"data/emoji/e159.png\" alt=\"\"/>");
            texto = texto.Replace("\ue432", "<img src=\"data/emoji/e432.png\" alt=\"\"/>");
            texto = texto.Replace("\ue430", "<img src=\"data/emoji/e430.png\" alt=\"\"/>");
            texto = texto.Replace("\ue431", "<img src=\"data/emoji/e431.png\" alt=\"\"/>");

            texto = texto.Replace("\ue42f", "<img src=\"data/emoji/e42f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue01e", "<img src=\"data/emoji/e01e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue039", "<img src=\"data/emoji/e039.png\" alt=\"\"/>");
            texto = texto.Replace("\ue435", "<img src=\"data/emoji/e435.png\" alt=\"\"/>");
            texto = texto.Replace("\ue01f", "<img src=\"data/emoji/e01f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue125", "<img src=\"data/emoji/e125.png\" alt=\"\"/>");
            texto = texto.Replace("\ue03a", "<img src=\"data/emoji/e03a.png\" alt=\"\"/>");

            texto = texto.Replace("\ue14e", "<img src=\"data/emoji/e14e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue252", "<img src=\"data/emoji/e252.png\" alt=\"\"/>");
            texto = texto.Replace("\ue137", "<img src=\"data/emoji/e137.png\" alt=\"\"/>");
            texto = texto.Replace("\ue209", "<img src=\"data/emoji/e209.png\" alt=\"\"/>");
            texto = texto.Replace("\ue154", "<img src=\"data/emoji/e154.png\" alt=\"\"/>");
            texto = texto.Replace("\ue133", "<img src=\"data/emoji/e133.png\" alt=\"\"/>");
            texto = texto.Replace("\ue150", "<img src=\"data/emoji/e150.png\" alt=\"\"/>");

            texto = texto.Replace("\ue320", "<img src=\"data/emoji/e320.png\" alt=\"\"/>");
            texto = texto.Replace("\ue123", "<img src=\"data/emoji/e123.png\" alt=\"\"/>");
            texto = texto.Replace("\ue132", "<img src=\"data/emoji/e132.png\" alt=\"\"/>");
            texto = texto.Replace("\ue143", "<img src=\"data/emoji/e143.png\" alt=\"\"/>");
            texto = texto.Replace("\ue50b", "<img src=\"data/emoji/e50b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue514", "<img src=\"data/emoji/e514.png\" alt=\"\"/>");
            texto = texto.Replace("\ue513", "<img src=\"data/emoji/e513.png\" alt=\"\"/>");

            texto = texto.Replace("\ue50c", "<img src=\"data/emoji/e50c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue50d", "<img src=\"data/emoji/e50d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue511", "<img src=\"data/emoji/e511.png\" alt=\"\"/>");
            texto = texto.Replace("\ue50f", "<img src=\"data/emoji/e50f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue512", "<img src=\"data/emoji/e512.png\" alt=\"\"/>");
            texto = texto.Replace("\ue510", "<img src=\"data/emoji/e510.png\" alt=\"\"/>");
            texto = texto.Replace("\ue50e", "<img src=\"data/emoji/e50e.png\" alt=\"\"/>");

            texto = texto.Replace("\ue21c", "<img src=\"data/emoji/e21c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue21d", "<img src=\"data/emoji/e21d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue21e", "<img src=\"data/emoji/e21e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue21f", "<img src=\"data/emoji/e21f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue220", "<img src=\"data/emoji/e220.png\" alt=\"\"/>");
            texto = texto.Replace("\ue221", "<img src=\"data/emoji/e221.png\" alt=\"\"/>");
            texto = texto.Replace("\ue222", "<img src=\"data/emoji/e222.png\" alt=\"\"/>");

            texto = texto.Replace("\ue223", "<img src=\"data/emoji/e223.png\" alt=\"\"/>");
            texto = texto.Replace("\ue224", "<img src=\"data/emoji/e224.png\" alt=\"\"/>");
            texto = texto.Replace("\ue225", "<img src=\"data/emoji/e225.png\" alt=\"\"/>");
            texto = texto.Replace("\ue210", "<img src=\"data/emoji/e210.png\" alt=\"\"/>");
            texto = texto.Replace("\ue232", "<img src=\"data/emoji/e232.png\" alt=\"\"/>");
            texto = texto.Replace("\ue233", "<img src=\"data/emoji/e233.png\" alt=\"\"/>");
            texto = texto.Replace("\ue235", "<img src=\"data/emoji/e235.png\" alt=\"\"/>");

            texto = texto.Replace("\ue234", "<img src=\"data/emoji/e234.png\" alt=\"\"/>");
            texto = texto.Replace("\ue236", "<img src=\"data/emoji/e236.png\" alt=\"\"/>");
            texto = texto.Replace("\ue237", "<img src=\"data/emoji/e237.png\" alt=\"\"/>");
            texto = texto.Replace("\ue238", "<img src=\"data/emoji/e238.png\" alt=\"\"/>");
            texto = texto.Replace("\ue239", "<img src=\"data/emoji/e239.png\" alt=\"\"/>");
            texto = texto.Replace("\ue23b", "<img src=\"data/emoji/e23b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue23a", "<img src=\"data/emoji/e23a.png\" alt=\"\"/>");

            texto = texto.Replace("\ue23d", "<img src=\"data/emoji/e23d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue23c", "<img src=\"data/emoji/e23c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue24d", "<img src=\"data/emoji/e24d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue212", "<img src=\"data/emoji/e212.png\" alt=\"\"/>");
            texto = texto.Replace("\ue24c", "<img src=\"data/emoji/e24c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue213", "<img src=\"data/emoji/e213.png\" alt=\"\"/>");
            texto = texto.Replace("\ue214", "<img src=\"data/emoji/e214.png\" alt=\"\"/>");

            texto = texto.Replace("\ue507", "<img src=\"data/emoji/e507.png\" alt=\"\"/>");
            texto = texto.Replace("\ue203", "<img src=\"data/emoji/e203.png\" alt=\"\"/>");
            texto = texto.Replace("\ue20b", "<img src=\"data/emoji/e20b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue22a", "<img src=\"data/emoji/e22a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue22b", "<img src=\"data/emoji/e22b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue226", "<img src=\"data/emoji/e226.png\" alt=\"\"/>");
            texto = texto.Replace("\ue227", "<img src=\"data/emoji/e227.png\" alt=\"\"/>");

            texto = texto.Replace("\ue22c", "<img src=\"data/emoji/e22c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue22d", "<img src=\"data/emoji/e22d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue215", "<img src=\"data/emoji/e215.png\" alt=\"\"/>");
            texto = texto.Replace("\ue216", "<img src=\"data/emoji/e216.png\" alt=\"\"/>");
            texto = texto.Replace("\ue217", "<img src=\"data/emoji/e217.png\" alt=\"\"/>");
            texto = texto.Replace("\ue218", "<img src=\"data/emoji/e218.png\" alt=\"\"/>");
            texto = texto.Replace("\ue228", "<img src=\"data/emoji/e228.png\" alt=\"\"/>");

            texto = texto.Replace("\ue151", "<img src=\"data/emoji/e151.png\" alt=\"\"/>");
            texto = texto.Replace("\ue138", "<img src=\"data/emoji/e138.png\" alt=\"\"/>");
            texto = texto.Replace("\ue139", "<img src=\"data/emoji/e139.png\" alt=\"\"/>");
            texto = texto.Replace("\ue13a", "<img src=\"data/emoji/e13a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue208", "<img src=\"data/emoji/e208.png\" alt=\"\"/>");
            texto = texto.Replace("\ue14f", "<img src=\"data/emoji/e14f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue20a", "<img src=\"data/emoji/e20a.png\" alt=\"\"/>");

            texto = texto.Replace("\ue434", "<img src=\"data/emoji/e434.png\" alt=\"\"/>");
            texto = texto.Replace("\ue309", "<img src=\"data/emoji/e309.png\" alt=\"\"/>");
            texto = texto.Replace("\ue315", "<img src=\"data/emoji/e315.png\" alt=\"\"/>");
            texto = texto.Replace("\ue30d", "<img src=\"data/emoji/e30d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue207", "<img src=\"data/emoji/e207.png\" alt=\"\"/>");
            texto = texto.Replace("\ue229", "<img src=\"data/emoji/e229.png\" alt=\"\"/>");
            texto = texto.Replace("\ue206", "<img src=\"data/emoji/e206.png\" alt=\"\"/>");

            texto = texto.Replace("\ue205", "<img src=\"data/emoji/e205.png\" alt=\"\"/>");
            texto = texto.Replace("\ue204", "<img src=\"data/emoji/e204.png\" alt=\"\"/>");
            texto = texto.Replace("\ue12e", "<img src=\"data/emoji/e12e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue250", "<img src=\"data/emoji/e250.png\" alt=\"\"/>");
            texto = texto.Replace("\ue251", "<img src=\"data/emoji/e251.png\" alt=\"\"/>");
            texto = texto.Replace("\ue14a", "<img src=\"data/emoji/e14a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue149", "<img src=\"data/emoji/e149.png\" alt=\"\"/>");

            texto = texto.Replace("\ue23f", "<img src=\"data/emoji/e23f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue240", "<img src=\"data/emoji/e240.png\" alt=\"\"/>");
            texto = texto.Replace("\ue241", "<img src=\"data/emoji/e241.png\" alt=\"\"/>");
            texto = texto.Replace("\ue242", "<img src=\"data/emoji/e242.png\" alt=\"\"/>");
            texto = texto.Replace("\ue243", "<img src=\"data/emoji/e243.png\" alt=\"\"/>");
            texto = texto.Replace("\ue244", "<img src=\"data/emoji/e244.png\" alt=\"\"/>");
            texto = texto.Replace("\ue245", "<img src=\"data/emoji/e245.png\" alt=\"\"/>");

            texto = texto.Replace("\ue246", "<img src=\"data/emoji/e246.png\" alt=\"\"/>");
            texto = texto.Replace("\ue247", "<img src=\"data/emoji/e247.png\" alt=\"\"/>");
            texto = texto.Replace("\ue248", "<img src=\"data/emoji/e248.png\" alt=\"\"/>");
            texto = texto.Replace("\ue249", "<img src=\"data/emoji/e249.png\" alt=\"\"/>");
            texto = texto.Replace("\ue24a", "<img src=\"data/emoji/e24a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue24b", "<img src=\"data/emoji/e24b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue23e", "<img src=\"data/emoji/e23e.png\" alt=\"\"/>");

            texto = texto.Replace("\ue532", "<img src=\"data/emoji/e532.png\" alt=\"\"/>");
            texto = texto.Replace("\ue533", "<img src=\"data/emoji/e533.png\" alt=\"\"/>");
            texto = texto.Replace("\ue534", "<img src=\"data/emoji/e534.png\" alt=\"\"/>");
            texto = texto.Replace("\ue535", "<img src=\"data/emoji/e535.png\" alt=\"\"/>");
            texto = texto.Replace("\ue21a", "<img src=\"data/emoji/e21a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue219", "<img src=\"data/emoji/e219.png\" alt=\"\"/>");
            texto = texto.Replace("\ue21b", "<img src=\"data/emoji/e21b.png\" alt=\"\"/>");

            texto = texto.Replace("\ue02f", "<img src=\"data/emoji/e02f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue024", "<img src=\"data/emoji/e024.png\" alt=\"\"/>");
            texto = texto.Replace("\ue025", "<img src=\"data/emoji/e025.png\" alt=\"\"/>");
            texto = texto.Replace("\ue026", "<img src=\"data/emoji/e026.png\" alt=\"\"/>");
            texto = texto.Replace("\ue027", "<img src=\"data/emoji/e027.png\" alt=\"\"/>");
            texto = texto.Replace("\ue028", "<img src=\"data/emoji/e028.png\" alt=\"\"/>");
            texto = texto.Replace("\ue029", "<img src=\"data/emoji/e029.png\" alt=\"\"/>");

            texto = texto.Replace("\ue02a", "<img src=\"data/emoji/e02a.png\" alt=\"\"/>");
            texto = texto.Replace("\ue02b", "<img src=\"data/emoji/e02b.png\" alt=\"\"/>");
            texto = texto.Replace("\ue02c", "<img src=\"data/emoji/e02c.png\" alt=\"\"/>");
            texto = texto.Replace("\ue02d", "<img src=\"data/emoji/e02d.png\" alt=\"\"/>");
            texto = texto.Replace("\ue02e", "<img src=\"data/emoji/e02e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue332", "<img src=\"data/emoji/e332.png\" alt=\"\"/>");
            texto = texto.Replace("\ue333", "<img src=\"data/emoji/e333.png\" alt=\"\"/>");

            texto = texto.Replace("\ue24e", "<img src=\"data/emoji/e24e.png\" alt=\"\"/>");
            texto = texto.Replace("\ue24f", "<img src=\"data/emoji/e24f.png\" alt=\"\"/>");
            texto = texto.Replace("\ue537", "<img src=\"data/emoji/e537.png\" alt=\"\"/>");
            
            return texto;
        }
    }
}
