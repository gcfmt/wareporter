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
            arquivoHtml.WriteLine("h3 { font-size: 12pt; }");
            //arquivoHtml.WriteLine(".chat_header { text-align: center; font-weight: bold; margin-bottom: 20px; padding-top: 10px;" +
            //    "padding-bottom: 10px; background-color: #FFFFFF; border: 1px solid #000000; }");
            //arquivoHtml.WriteLine(".heading { font-size: 140 %; }");
            arquivoHtml.WriteLine(".contact { }");
            arquivoHtml.WriteLine(".messages { background-color: #FFFFFF; border: 1px solid #000000; padding: 2px 4px; }");
            arquivoHtml.WriteLine(".message { margin: 2px 0px; max-width: 70%; }");
            arquivoHtml.WriteLine(".message > .timestamp { font-family: \"Arial\", monospace; font-size: 8pt; }");
            arquivoHtml.WriteLine(".message > .text > span { }");
            arquivoHtml.WriteLine(".incoming_message { text-align: left; }");
            arquivoHtml.WriteLine(".incoming_message > .text > span { background-color: #CCD9FF; }");
            arquivoHtml.WriteLine(".outgoing_message { text-align: right; margin-left: auto; }");
            arquivoHtml.WriteLine(".outgoing_message > .text > span { background-color: #BAE691; }");
            arquivoHtml.WriteLine(".ui-accordion-header { background-color: white; }");
            arquivoHtml.WriteLine(".ui-accordion-header.ui-state-active { background-color: orange; }");
            arquivoHtml.WriteLine("table {  margin: 0px auto; color: #333; font-size: 11; font-family: Helvetica, Arial, sans-serif; border-collapse: collapse;" +
                "border-spacing: 0; }");
            arquivoHtml.WriteLine("td, th { border: 1px solid transparent; }");
            arquivoHtml.WriteLine("th { background: #DFDFDF; font-weight: bold; }");
            arquivoHtml.WriteLine("td { background: #FAFAFA; text-align: center; }");
            arquivoHtml.WriteLine("tr: nth-child(even) td { background: #F1F1F1; }");
            arquivoHtml.WriteLine("tr: tr: nth-child(odd) td { background: #FEFEFE; }");
            arquivoHtml.WriteLine("</style>");
            arquivoHtml.WriteLine("<script> $(function() { $(\".accordion\").accordion({collapsible: true, heightStyle: \"content\", active: false })});;</script>");

            arquivoHtml.WriteLine("</head>");
            #endregion

            arquivoHtml.WriteLine("<body>");


            arquivoHtml.WriteLine("<h2>Relatório de Conversações WhatsApp<h2>");
            arquivoHtml.WriteLine("<h4>Gerência de Computação Forense - POLITEC/MT<h4>");

            #region Chats
            arquivoHtml.WriteLine("<h3>Chats (Conversações)<h3>");
            arquivoHtml.WriteLine("<table style=\"margin-bottom:20px;\">");
            arquivoHtml.WriteLine("<tr>");
            arquivoHtml.WriteLine("<th>Código</th>");
            arquivoHtml.WriteLine("<th>Conversação</th>");
            arquivoHtml.WriteLine("<th></th>");
            arquivoHtml.WriteLine("<th>ID</th>");
            arquivoHtml.WriteLine("<th>Tipo</th>");
            arquivoHtml.WriteLine("<th>Mensagens</th>");
            arquivoHtml.WriteLine("<th>Última Mensagem</th>");
            arquivoHtml.WriteLine("</tr>");

            foreach (var chat in chats.OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                bool isGrupo = chat.KeyRemoteJid.Contains("-");

                arquivoHtml.WriteLine("<tr>");
                arquivoHtml.WriteLine("<td>" + chat.Id + "</td>");
                arquivoHtml.WriteLine("<td><img style=\"width: 40px; height: 40px; \" src=\"" + Midia.ObterAvatar(chat.KeyRemoteJid) + "\"></td>");
                arquivoHtml.WriteLine("<td style=\"text-align: left\" width=\"350px\"><a href=\"#"+chat.KeyRemoteJid+"\">"+chat.Contato.NomeContato + "</a></td>");
                arquivoHtml.WriteLine("<td>" + chat.KeyRemoteJid + "</td>");
                arquivoHtml.WriteLine("<td>" + (isGrupo ? "Grupo" : "Indivíduo") + "</td>");
                arquivoHtml.WriteLine("<td>" + (isGrupo ? chat.Mensagens.Count - 1 : chat.Mensagens.Count) + "</td>");
                arquivoHtml.WriteLine("<td>" + chat.Mensagens.Last().Timestamp + "</td>");
                arquivoHtml.WriteLine("</tr>");
            }
            arquivoHtml.WriteLine("</table>");
            #endregion

            #region Participantes de Grupos
            arquivoHtml.WriteLine("<h3>Participantes de Grupos<h3>");
            foreach (var chat in chats.OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                bool isGrupo = chat.KeyRemoteJid.Contains("-");
                if (!isGrupo) continue;
                arquivoHtml.WriteLine("<a name=\"part-"+chat.KeyRemoteJid+"\"></a>");
                arquivoHtml.WriteLine("<h4>" + chat.Subject + "<h4>");
                arquivoHtml.WriteLine("<h5><a href=\"#\">Ir para o topo</a><h5>");

                arquivoHtml.WriteLine("<table style=\"margin-bottom:20px;\">");
                arquivoHtml.WriteLine("<tr>");
                arquivoHtml.WriteLine("<th>Código</th>");
                arquivoHtml.WriteLine("<th>Contato</th>");
                arquivoHtml.WriteLine("<th></th>");
                arquivoHtml.WriteLine("<th>ID</th>");
                arquivoHtml.WriteLine("<th>Administrador</th>");
                arquivoHtml.WriteLine("</tr>");

                foreach (var participant in chat.ParticipantesGrupo)
                {
                    if (String.IsNullOrWhiteSpace(participant.Jid)) continue;
                    arquivoHtml.WriteLine("<tr>");

                    arquivoHtml.WriteLine("<td>" + participant.Id + "</td>");
                    arquivoHtml.WriteLine("<td><img style=\"width: 40px; height: 40px; \" src=\"" + Midia.ObterAvatar(participant.Contato.Jid) + "\"></td>");

                    arquivoHtml.WriteLine("<td style=\"text-align: left\" width=\"350px\">" + participant.Contato.NomeContato + "</td>");
                    arquivoHtml.WriteLine("<td>" + participant.Jid + "</td>");
                    arquivoHtml.WriteLine("<td>"+(participant.Admin == 1 ? "Administrador do Grupo" : "" )+"</td>");
                    arquivoHtml.WriteLine("<td>" + participant.Jid + "</td>");
                    arquivoHtml.WriteLine("</tr>");                   
                }

                arquivoHtml.WriteLine("</table>");
            }
            #endregion


            #region
            arquivoHtml.WriteLine("<h3>Mensagens<h3>");
            foreach (var chat in chats.OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                arquivoHtml.WriteLine("<a name=\"" + chat.KeyRemoteJid + "\"></a>");
                arquivoHtml.WriteLine("<h4>" + chat.Subject + "<h4>");
                arquivoHtml.WriteLine("<h5><a href=\"#\">Ir para o topo</a><h5>");

                arquivoHtml.WriteLine("<table style=\"margin-bottom:20px;\">");
                
                bool isGrupo = chat.KeyRemoteJid.Contains("-");


                foreach (var mensagem in chat.Mensagens)
                {
                    arquivoHtml.WriteLine("<tr>");

                    arquivoHtml.WriteLine("<div class=\"message " + (mensagem.KeyFromMe == 0 ? "incoming" : "outgoing") + "_message\"><div class=\"text\">");

                    switch (mensagem.MediaWaType)
                    {
                        case MediaWhatsappType.MEDIA_WHATSAPP_TEXT:
                            {
                                arquivoHtml.WriteLine("<span>" + mensagem.Data + "</span>");
                            }
                            break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_IMAGE:
                            {
                                //   if (mensagem.RawData.ToStringgetRawDataSize() > 0 && message.getRawData() != NULL)
                                // {
                                arquivoHtml.WriteLine("<div><img style=\"{width: 300px; height: auto; }\" src=\"" + Midia.ObterImagemDaMensagem(mensagem) + "\"></div>");
                                //}
                            }
                            break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_AUDIO:
                            {
                                arquivoHtml.WriteLine("<span>[ \" << formatAudio(message) << \" ]</span>");
                            }
                            break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_VIDEO:
                            {
                                //  if (message.getRawDataSize() > 0 && message.getRawData() != NULL)
                                // {
                                arquivoHtml.WriteLine("<div><img src=\"data:image/jpeg;base64," + mensagem.RawData + "\"></div>");
                                //  }
                                arquivoHtml.WriteLine("<span>[ Video ]</span>");
                            }
                            break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_CONTACT:
                            {
                                arquivoHtml.WriteLine("<span>[ Contact ]</span>");
                            }
                            break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_LOCATION:
                            {
                                arquivoHtml.WriteLine("<span>[ Location: \" << message.getLatitude() << \"; \" << message.getLongitude() << \" ]</span>");
                            }
                            break;
                    }

                    arquivoHtml.WriteLine("</div><div class=\"timestamp\"><span>" + mensagem.Timestamp + "</span></div></div>");
                    arquivoHtml.WriteLine("</div></div>");
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
