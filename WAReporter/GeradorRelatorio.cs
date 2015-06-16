﻿using System;
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
            arquivoHtml.WriteLine("<table  style=\"font-size:13px; margin-bottom:20px;\">");
            arquivoHtml.WriteLine("<tr>");
            arquivoHtml.WriteLine("<th>ID</th>");
            arquivoHtml.WriteLine("<th colspan=2>Conversação</th>");      
            arquivoHtml.WriteLine("<th>Tipo</th>");
            arquivoHtml.WriteLine("<th>Participantes</th>");
            arquivoHtml.WriteLine("<th>Mensagens</th>");
            arquivoHtml.WriteLine("<th>Última Mensagem</th>");
            arquivoHtml.WriteLine("</tr>");

            foreach (var chat in chats.OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                bool isGrupo = chat.KeyRemoteJid.Contains("-");

                arquivoHtml.WriteLine("<tr>");
                arquivoHtml.WriteLine("<td style=\"text-align: left\">" + chat.KeyRemoteJid + "</td>");
                arquivoHtml.WriteLine("<td><img style=\"width: 40px; height: 40px; \" src=\"" + Midia.ObterAvatar(chat.KeyRemoteJid) + "\"></td>");
                arquivoHtml.WriteLine("<td style=\"text-align: left\" width=\"350px\">"+chat.Contato.NomeContato + "</td>");
                arquivoHtml.WriteLine("<td>" + (isGrupo ? "Grupo" : "Indivíduo") + "</td>");
                arquivoHtml.WriteLine("<td style=\"text-align: right\">" + (isGrupo ? "<a href=\"#partic-" + chat.KeyRemoteJid + "\">"+(chat.ParticipantesGrupo.Count()-1).ToString()+"</a>" : "") + "</td>");
                arquivoHtml.WriteLine("<td style=\"text-align: right\"><a href=\"#msg-" + chat.KeyRemoteJid + "\">" + (isGrupo ? chat.Mensagens.Count - 1 : chat.Mensagens.Count) + "</a></td>");
                arquivoHtml.WriteLine("<td>" + chat.Mensagens.Last().Timestamp + "</td>");
                arquivoHtml.WriteLine("</tr>");
            }
            arquivoHtml.WriteLine("</table>");
            #endregion

            #region Participantes de Grupos
            arquivoHtml.WriteLine("<h3 style=\"text-align:center\">Participantes de Grupos<h3>");
            foreach (var chat in chats.Where(p => p.KeyRemoteJid.Contains("-")).OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                arquivoHtml.WriteLine("<a name=\"partic-"+chat.KeyRemoteJid+"\"></a>");
                arquivoHtml.WriteLine("<h4 style=\"text-align:center\">" + chat.Subject + "<h4>");
                arquivoHtml.WriteLine("<h5 style=\"text-align:center\"><a href=\"#\">Ir para o topo</a><h5>");

                arquivoHtml.WriteLine("<table style=\"margin-bottom:50px;\">");
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
            arquivoHtml.WriteLine("<h3 style=\"text-align:center\">Mensagens<h3>");
            foreach (var chat in chats.OrderByDescending(p => p.Mensagens.Last().Timestamp))
            {
                arquivoHtml.WriteLine("<div style=\"text-align:center\">");
                arquivoHtml.WriteLine("<a name=\"msg-" + chat.KeyRemoteJid + "\"></a>");
                arquivoHtml.WriteLine("<div font-size=14>" + chat.Contato.NomeContato + "</div>");
                arquivoHtml.WriteLine("<a href=\"#\">Ir para o topo</a>");
                arquivoHtml.WriteLine("</div>");

                arquivoHtml.WriteLine("<table style=\"margin-bottom:50px; width:70%; font-size:16px;\">");
                
                bool isGrupo = chat.KeyRemoteJid.Contains("-");

                foreach (var mensagem in chat.Mensagens)
                {
                    if (mensagem == chat.Mensagens.First() && isGrupo) continue;

                    arquivoHtml.WriteLine("<tr>");

                    //arquivoHtml.WriteLine(".incoming_message { text-align: left; }");
                    //arquivoHtml.WriteLine(".incoming_message > .text > span { background-color: #CCD9FF; }");
                    //arquivoHtml.WriteLine(".outgoing_message { text-align: right; margin-left: auto; }");
                    //arquivoHtml.WriteLine(".outgoing_message > .text > span { background-color: #BAE691; }");

                    arquivoHtml.WriteLine("<td style=\"text-align:" + (mensagem.KeyFromMe == 1 ? "right" : "left") + "; valign=bottom;  \">");

                    

                    switch (mensagem.MediaWaType)
                    {
                        case MediaWhatsappType.MEDIA_WHATSAPP_TEXT:
                        {
                            arquivoHtml.WriteLine("<div style=\"float: left, bottom; margin-top:7px; margin-bottom:7px;\">");
                            arquivoHtml.WriteLine("<span>"+mensagem.Data+ "</span>");
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_IMAGE:
                        {
                            arquivoHtml.WriteLine("<div style=\"float: left, bottom; margin-top:7px; margin-bottom:7px;\"><img style=\"width: 480px; height: auto; align: " + (mensagem.KeyFromMe == 1 ? "right" : "left") +"\" src=\"" + Midia.ObterImagemDaMensagem(mensagem) + "\">");
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_AUDIO:
                        {
                            arquivoHtml.WriteLine("<div><audio width=\"480\" controls><source src=\""+ Midia.ObterAudioDaMensagem(mensagem) + "\">Seu navegador não suporta áudio HTML5.</audio>");
                        }
                        break;
                        case MediaWhatsappType.MEDIA_WHATSAPP_VIDEO:
                        {
                            arquivoHtml.WriteLine("<div><video width=\"480\" controls><source src=\""+ Midia.ObterVideoDaMensagem(mensagem) + "\">Seu navegador não suporta vídeo HTML5.</video>");
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
