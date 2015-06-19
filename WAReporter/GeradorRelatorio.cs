using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAReporter.Modelo;

namespace WAReporter
{
    public static class GeradorRelatorio
    {

        public static string GerarRelatorioHtml(List<Chat> chats, String caminhoHtml)
        {
            var resultado = "";

            var arquivoHtml = new StreamWriter(caminhoHtml);
            arquivoHtml.WriteLine("<!DOCTYPE html>");
            arquivoHtml.WriteLine("<html>");

            #region tag head (estilos, scripts, etc.)
            arquivoHtml.WriteLine("<head>");
            arquivoHtml.WriteLine("<meta charset=\"utf-8\">");
            arquivoHtml.WriteLine("<title>WhatsApp - Relatório de Conversações</title>");
            arquivoHtml.WriteLine("<link rel=\"stylesheet\" href=\"http://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css\"/>");
            arquivoHtml.WriteLine("<script src=\"http://code.jquery.com/jquery-1.10.2.js\"></script>");
            arquivoHtml.WriteLine("<script src=\"http://code.jquery.com/ui/1.11.4/jquery-ui.js\"></script>");
            arquivoHtml.WriteLine("<style type=\"text/css\">");
            arquivoHtml.WriteLine("font-family: Helvetica, Arial, sans-serif;");
            arquivoHtml.WriteLine("h3 { font-size: 12pt; }");
            arquivoHtml.WriteLine(".messages { background-color: #FFFFFF; border: 1px solid #000000; padding: 2px 4px; }");
            arquivoHtml.WriteLine(".message {  }");
            arquivoHtml.WriteLine(".message > .timestamp { font-family: \"Arial\", monospace; font-size: 8pt; }");
            arquivoHtml.WriteLine(".message > .text > span { }");
            arquivoHtml.WriteLine("table { width: 70%;  margin: 0px auto; color: #333; font-size: 11; font-family: Helvetica, Arial, sans-serif; border-collapse: collapse;" +
                "border-spacing: 0; }");
            arquivoHtml.WriteLine("td, th { border: 1px solid transparent; }");
            arquivoHtml.WriteLine("th { background: #DFDFDF; font-weight: bold; }");
            arquivoHtml.WriteLine("td { background: #FAFAFA; text-align: center; }");
            arquivoHtml.WriteLine("tr: nth-child(even) td { background: #BFE7EA; }");
            arquivoHtml.WriteLine("tr: nth-child(odd) td { background: #F9FDFD; }");
            arquivoHtml.WriteLine("</style>");

            arquivoHtml.WriteLine("</head>");
            #endregion

            arquivoHtml.WriteLine("<body>");


            arquivoHtml.WriteLine("<h2 style=\"text-align:center\">Relatório de Conversações WhatsApp<h2>");
            arquivoHtml.WriteLine("<h4 style=\"text-align:center; margin-bottom:30px;\">Gerência de Computação Forense - POLITEC/MT<h4>");

            #region Chats
            arquivoHtml.WriteLine("<h3 style=\"text-align:center\">Chats (Conversações)<h3>");
            arquivoHtml.WriteLine("<table  style=\"font-size:13px; margin-bottom:100px;\">");
            arquivoHtml.WriteLine("<tr>");
            arquivoHtml.WriteLine("<th>ID</th>");
            arquivoHtml.WriteLine("<th colspan=2>Conversação</th>");      
            arquivoHtml.WriteLine("<th>Tipo</th>");
            arquivoHtml.WriteLine("<th>Participantes</th>");
            arquivoHtml.WriteLine("<th>Mensagens</th>");
            arquivoHtml.WriteLine("<th>Última Mensagem</th>");
            arquivoHtml.WriteLine("</tr>");

            foreach (var chat in chats.OrderByDescending(p => p.Mensagens.Any() ? p.Mensagens.Last().Timestamp : DateTime.MinValue))
            {
                bool isGrupo = chat.KeyRemoteJid.Contains("-");

                arquivoHtml.WriteLine("<tr>");
                arquivoHtml.WriteLine("<td style=\"text-align: left\">" + chat.KeyRemoteJid + "</td>");
                arquivoHtml.WriteLine("<td><img style=\"width: 40px; height: 40px; \" src=\"" + Midia.ObterAvatar(chat.KeyRemoteJid) + "\"></td>");
                arquivoHtml.WriteLine("<td style=\"text-align: left\" width=\"350px\">"+chat.Contato.NomeContato + "</td>");
                arquivoHtml.WriteLine("<td>" + (isGrupo ? "Grupo" : "Indivíduo") + "</td>");
                arquivoHtml.WriteLine("<td style=\"text-align: right\">" + (isGrupo ? "<a href=\"#partic-" + chat.KeyRemoteJid + "\">"+ (chat.ParticipantesGrupo.Any() ? (chat.ParticipantesGrupo.Count()-1).ToString() : "N/D") + "</a>" : "")+ "</td>");
                arquivoHtml.WriteLine("<td style=\"text-align: right\"><a href=\"#msg-" + chat.KeyRemoteJid + "\">" + (isGrupo ? chat.Mensagens.Count - 1 : chat.Mensagens.Count) + "</a></td>");
                arquivoHtml.WriteLine("<td>" + (chat.Mensagens.Any() ? chat.Mensagens.Last().Timestamp.ToString() : "Nenhuma") + "</td>");
                arquivoHtml.WriteLine("</tr>");
            }
            arquivoHtml.WriteLine("</table>");
            #endregion

            #region Participantes de Grupos
            foreach (var chat in chats.Where(p => p.KeyRemoteJid.Contains("-") && p.ParticipantesGrupo.Any()).OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                //arquivoHtml.WriteLine("<h3 style=\"text-align:center\">Participantes do Grupo<h3>");
                arquivoHtml.WriteLine("<a name=\"partic-" + chat.KeyRemoteJid + "\"></a>");

                arquivoHtml.WriteLine("<table style=\"margin-bottom:0px; width:70%; font-size:16px; background:white\">");
                arquivoHtml.WriteLine("<tr style=\"background:white\">");
                arquivoHtml.WriteLine("<td style=\"text-align: left, valign=center, background:white\"><img style=\"width: 100px; height: 100px; \" src=\"" + Midia.ObterAvatar(chat.KeyRemoteJid) + "\">");
                arquivoHtml.WriteLine("<span>" + chat.Subject + "</span></td>");
                arquivoHtml.WriteLine("<td style=\"font-weight:bold\"><b>Participantes do Grupo</b></td>");
                arquivoHtml.WriteLine("<td colspan=2 style=\"text-align: right\"><a href=\"#\">Ir para o topo</a></td>");
                arquivoHtml.WriteLine("</tr>");
                arquivoHtml.WriteLine("</table>");
                                
                arquivoHtml.WriteLine("<table style=\"margin-bottom:50px;font-size:13px;\">");
                
                arquivoHtml.WriteLine("<tr>");
                arquivoHtml.WriteLine("<th>ID</th>");
                arquivoHtml.WriteLine("<th>Contato</th>");
                arquivoHtml.WriteLine("<th></th>");
                arquivoHtml.WriteLine("<th>Administrador</th>");
                arquivoHtml.WriteLine("</tr>");

                foreach (var participant in chat.ParticipantesGrupo.Where(p => !String.IsNullOrWhiteSpace(p.Jid)).OrderBy(p => p.Contato == null ? "" : p.Contato.NomeContato))
                {
                    arquivoHtml.WriteLine("<tr>");

                    arquivoHtml.WriteLine("<td>" + participant.Jid + "</td>");
                    arquivoHtml.WriteLine("<td><img style=\"width: 40px; height: 40px; \" src=\"" + Midia.ObterAvatar(participant.Jid) + "\"></td>");

                    arquivoHtml.WriteLine("<td style=\"text-align: left\" width=\"350px\">" + (participant.Contato != null ?
                        participant.Contato.NomeContato : "") + "</td>");

                    arquivoHtml.WriteLine("<td>"+(participant.Admin == 1 ? "Administrador" : "" )+"</td>");
                    arquivoHtml.WriteLine("</tr>");                   
                }

                arquivoHtml.WriteLine("</table>");
            }
            #endregion


            #region
            foreach (var chat in chats.Where(p => p.Mensagens.Any()).OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
              
                arquivoHtml.WriteLine("<table style=\"margin-bottom:0px; width:70%; font-size:16px; background:#FFFFFF\">");
                arquivoHtml.WriteLine("<a name=\"msg-" + chat.KeyRemoteJid + "\"></a>");
                arquivoHtml.WriteLine("<tr style=\"background:white\">");
                arquivoHtml.WriteLine("<td style=\"text-align: left,  valign=center, background:#FFFFFF\"><img style=\"width: 100px; height: 100px; \" src=\"" + Midia.ObterAvatar(chat.KeyRemoteJid) + "\">");
                arquivoHtml.WriteLine("<span>" + chat.Contato.NomeContato + "</span></td>");
                arquivoHtml.WriteLine("<td style=\"font-weight:bold\"><b>Mensagens</b></td>");
                arquivoHtml.WriteLine("<td colspan=2 style=\"text-align: right\"><a href=\"#\">Ir para o topo</a></td>");
                arquivoHtml.WriteLine("</tr>");
                arquivoHtml.WriteLine("</table>");



                arquivoHtml.WriteLine("<table style=\"margin-bottom:50px; width:70%; font-size:16px;\">");

                

                bool isGrupo = chat.KeyRemoteJid.Contains("-");

                foreach (var mensagem in chat.Mensagens)
                {
                    if (mensagem == chat.Mensagens.First() && isGrupo) continue;

                    arquivoHtml.WriteLine("<tr>");

                    arquivoHtml.WriteLine("<td style=\"text-align:" + (mensagem.KeyFromMe == 1 ? "right" : "left") + "; valign=bottom;  \">");
                    arquivoHtml.WriteLine(mensagem.Id);

                    switch (mensagem.MediaWaType)
                    {
                        case MediaWhatsappType.MEDIA_WHATSAPP_TEXT:
                        {
                            arquivoHtml.WriteLine("<div style=\"float: left, bottom; margin-top:7px; margin-bottom:7px;\">");
                            if(mensagem.MediaSize == 1)
                                arquivoHtml.WriteLine("<span style=\"font-size:12px; margin-left:5px; font-weight: bold; color:green\"> NOME DO GRUPO ALTERADO PARA " + mensagem.Data + "</span>");
                            else if (!String.IsNullOrWhiteSpace(mensagem.Data))
                                    arquivoHtml.WriteLine("<span>"+ Midia.AdicionaEmoji(mensagem.Data)+ "</span>");
                            else
                                    arquivoHtml.WriteLine("<span style=\"font-size:12px; margin-left:5px; font-weight: bold; color:green\">" + mensagem.RemoteResource + " SAIU DO GRUPO</span>");

                            }
                            break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_IMAGE:
                        {
                            arquivoHtml.WriteLine("<div style=\"float: left, bottom; margin-top:7px; margin-bottom:7px;\">"+Midia.ObterImagemDaMensagem(mensagem));
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_AUDIO:
                        {
                            arquivoHtml.WriteLine("<div>"+Midia.ObterAudioDaMensagem(mensagem));
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_VIDEO:
                        {
                            arquivoHtml.WriteLine("<div>"+Midia.ObterVideoDaMensagem(mensagem));
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_CONTACT:
                        {
                            arquivoHtml.WriteLine("<span>[ Contact ]</span>");
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_LOCATION:
                        {
                            arquivoHtml.WriteLine("<span>[ Location: \"" + mensagem.Latitude + "; \""+ mensagem.Longitude + " ]</span>");
                        }
                        break;
                        default:
                            {
                                var id = mensagem.Id;
                            }
                            break;
                    }

                    if(mensagem.KeyFromMe == 0 && isGrupo)
                        arquivoHtml.WriteLine("<span style=\"font-size:12px; margin-left:5px; font-weight: bold; color: #663300\"><b>" + mensagem.Contato.NomeContato + "</b></span>");
                    arquivoHtml.WriteLine("<span style=\"font-size:12px; margin-left:5px; font-weight: bold; color: #001D72\"><b>" + mensagem.Timestamp + "</b></span>");

                    arquivoHtml.WriteLine("</div>");

                    arquivoHtml.WriteLine("</td>");

                    arquivoHtml.WriteLine("</tr>");
                }
                arquivoHtml.WriteLine("</table>");
            }
            #endregion
            
            arquivoHtml.WriteLine("</body>");
            arquivoHtml.WriteLine("</html>");

            arquivoHtml.Close();

            Process.Start("\"" + caminhoHtml + "\"");

            resultado = "SUCESSO: Relatório HTML gerado.";

            return resultado;
        }
    }
}
