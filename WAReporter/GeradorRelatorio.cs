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
            arquivoHtml.WriteLine(".chat { width: 70%; background-color: #FFFFFF; font-family: \"Arial\", sans-serif;" +
                "font-size: 10pt; padding: 10px; margin-left: auto; margin-right: auto; }");
            //arquivoHtml.WriteLine(".chat_header { text-align: center; font-weight: bold; margin-bottom: 20px; padding-top: 10px;" +
            //    "padding-bottom: 10px; background-color: #FFFFFF; border: 1px solid #000000; }");
            //arquivoHtml.WriteLine(".heading { font-size: 140 %; }");
            arquivoHtml.WriteLine(".contact { }");
            arquivoHtml.WriteLine(".day {text-align: center; margin: 30px 10px 10px 10px;}");
            arquivoHtml.WriteLine(".day > span { background-color: #B0D0F0; }");
            arquivoHtml.WriteLine(".messages { background-color: #FFFFFF; border: 1px solid #000000; padding: 2px 4px; }");
            arquivoHtml.WriteLine(".message { margin: 2px 0px; max-width: 70%; }");
            arquivoHtml.WriteLine(".message > .timestamp { font-family: \"Arial\", monospace; font-size: 8pt; }");
            arquivoHtml.WriteLine(".message > .text > span { }");
            arquivoHtml.WriteLine(".incoming_message { text-align: left; }");
            arquivoHtml.WriteLine(".incoming_message > .text > span { background-color: #CCD9FF; }");
            arquivoHtml.WriteLine(".outgoing_message { text-align: right; margin-left: auto; }");
            arquivoHtml.WriteLine(".outgoing_message > .text > span { background-color: #BAE691; }");
            arquivoHtml.WriteLine("img {width: 300px; height: auto; }");
            arquivoHtml.WriteLine(".ui-accordion-header { background-color: white; }");
            arquivoHtml.WriteLine(".ui-accordion-header.ui-state-active { background-color: orange; }");
            arquivoHtml.WriteLine("</style>");
            arquivoHtml.WriteLine("<script> $(function() { $(\"#accordion\").accordion({collapsible: true, heightStyle: \"content\" })});;</script>");

            arquivoHtml.WriteLine("</head>");
            #endregion

            arquivoHtml.WriteLine("<body>");
            arquivoHtml.WriteLine("<button type=\"button\" onclick=\"alert('Hello world!')\">Mostrar Todos</button>");
            arquivoHtml.WriteLine("<button type=\"button\" onclick=\"alert('Hello world!')\">Recolher Todos</button>");
            arquivoHtml.WriteLine("<div id=\"accordion\">");

            foreach (var chat in chats)
            {
                arquivoHtml.WriteLine("<h3>");
                arquivoHtml.WriteLine(chat.NomeContato);
                if (chat.Mensagens.Any())
                    arquivoHtml.WriteLine(" - Última mensagem: " + chat.Mensagens.Last().Timestamp);

                arquivoHtml.WriteLine("</h3>");


                arquivoHtml.WriteLine("<div>");
                arquivoHtml.WriteLine("<div class=\"chat\">");
    
                arquivoHtml.WriteLine("</div><div class=\"messages\">");
                
                foreach (var mensagem in chat.Mensagens)
                {
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
                            arquivoHtml.WriteLine("<div><img src=\"" + Midia.ObterImagemDaMensagem(mensagem) + "\"></div>");
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
                }

                arquivoHtml.WriteLine("</div></div>");
            }

            arquivoHtml.WriteLine("</div>");

            arquivoHtml.WriteLine("</body>");
            arquivoHtml.WriteLine("</html>");

            arquivoHtml.Close();

            Process.Start("\"" + caminhoHtml + "\"");

            resultado = "SUCESSO: Relatório HTML gerado.";

            return resultado;
        }
    }
}
