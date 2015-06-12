using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAReporter.Modelo;
using WAReporter.Utilitarios;

namespace WAReporter
{
    public class Banco
    {
        static SQLiteConnection ConexaoMsgStoreDb;
        static bool PossuiWaDb = false;
        static SQLiteConnection ConexaoWaDb;

        public static List<Chat> Chats { get; set; }
        public static List<GroupParticipant> GroupParticipants { get; set; }
        public static List<GroupParticipantHistory> GroupParticipantHistories { get; set; }
        public static List<MediaRef> MediaRefs { get; set; }
        public static List<Message> Messages { get; set; }
        public static List<Receipt> Receipts { get; set; }
        public static List<WaContact> WaContacts { get; set; }

        /// <summary>
        /// Inicializa a conexão e efetua o carregamento do banco sqlite de endereço definido por parâmetro
        /// </summary>
        /// <returns>Status do processamento</returns>
        /// <param name="enderecoMsgStore">String contendo o endereço do arquivo msgstore.db a ser carregado</param>
        /// <param name="WaDb">String contendo o endereço do arquivo msgstore.db a ser carregado</param>
        public static String CarregarBanco(String enderecoMsgStore, String enderecoWaDb)
        {
            String resultado = "";
            try {
                ConexaoMsgStoreDb = new SQLiteConnection("Data Source=" + enderecoMsgStore + ";Version=3;");

                PossuiWaDb = !String.IsNullOrWhiteSpace(enderecoWaDb);
                if (PossuiWaDb) ConexaoWaDb = new SQLiteConnection("Data Source=" + enderecoWaDb + ";Version=3;");

                ConexaoMsgStoreDb.Open();

                #region importa chat_list
                Chats = new List<Chat>();
                var chatListReader = new SQLiteCommand("select * from chat_list;", ConexaoMsgStoreDb).ExecuteReader();
                while (chatListReader.Read())
                {
                    var chat = new Chat();
                    chat.Id = chatListReader.GetInt32(0);
                    chat.KeyRemoteJid = chatListReader.IsDBNull(1) ? null : chatListReader.GetString(1);
                    chat.MessageTableId = chatListReader.IsDBNull(2) ? 0 : chatListReader.GetInt32(2);
                    chat.Subject = chatListReader.IsDBNull(3) ? null : chatListReader.GetString(3);
                    chat.Creation = chatListReader.IsDBNull(4) ? DateTime.MinValue : chatListReader.GetInt64(4).TimeStampParaDateTime();
                    chat.LastReadMessageTableId = chatListReader.IsDBNull(5) ? -1 : chatListReader.GetInt32(5);
                    chat.LastReadReceiptSentMessageTableId = chatListReader.IsDBNull(6) ? -1 : chatListReader.GetInt32(6);
                    chat.Archived = chatListReader.IsDBNull(7) ? -1 : chatListReader.GetInt32(7);
                    chat.SortTimestamp = chatListReader.IsDBNull(8) ? -1 : chatListReader.GetInt32(8);
                    chat.ModTag = chatListReader.IsDBNull(9) ? -1 : chatListReader.GetInt32(9);

                    if (!String.IsNullOrWhiteSpace(chat.Subject)) {
                        var s = chatListReader.GetValue(3);


                    }

                    Chats.Add(chat);
                }
                #endregion

                #region importa group_participants
                GroupParticipants = new List<GroupParticipant>();
                var groupParticipantReader = new SQLiteCommand("select * from group_participants;", ConexaoMsgStoreDb).ExecuteReader();
                while (groupParticipantReader.Read())
                {
                    var groupParticipant = new GroupParticipant();
                    groupParticipant.Id = groupParticipantReader.GetInt32(0);
                    groupParticipant.Gjid = groupParticipantReader.GetString(1);
                    groupParticipant.Jid = groupParticipantReader.GetString(2);
                    groupParticipant.Admin = groupParticipantReader.IsDBNull(3) ? -1 : groupParticipantReader.GetInt32(3);
                    groupParticipant.Pending = groupParticipantReader.IsDBNull(4) ? -1 : groupParticipantReader.GetInt32(4);

                    GroupParticipants.Add(groupParticipant);

                }
                                
                foreach (var chat in Chats.Where(p => p.KeyRemoteJid.Contains("-")))
                {
                    chat.ParticipantesGrupo = new List<GroupParticipant>();
                    chat.ParticipantesGrupo.AddRange(GroupParticipants.Where(p => p.Gjid == chat.KeyRemoteJid));
                }
                #endregion

                #region importa group_participants_history
                GroupParticipantHistories = new List<GroupParticipantHistory>();
                var groupParticipantHistoriesReader = new SQLiteCommand("select * from group_participants_history;", ConexaoMsgStoreDb).ExecuteReader();
                while (groupParticipantHistoriesReader.Read())
                {
                    var groupParticipantHistory = new GroupParticipantHistory();
                    groupParticipantHistory.Id = groupParticipantHistoriesReader.GetInt32(0);
                    groupParticipantHistory.Timestamp = groupParticipantHistoriesReader.IsDBNull(1) ? DateTime.MinValue : groupParticipantHistoriesReader.GetInt64(1).TimeStampParaDateTime();
                    groupParticipantHistory.GJid = groupParticipantHistoriesReader.GetString(2);
                    groupParticipantHistory.Jid = groupParticipantHistoriesReader.GetString(3);
                    groupParticipantHistory.Action = groupParticipantHistoriesReader.IsDBNull(4) ? -1 : groupParticipantHistoriesReader.GetInt32(4);
                    groupParticipantHistory.OldPhash = groupParticipantHistoriesReader.GetString(5);
                    groupParticipantHistory.NewPhash = groupParticipantHistoriesReader.GetString(6);

                    GroupParticipantHistories.Add(groupParticipantHistory);
                }
                #endregion

                #region importa media_refs
                MediaRefs = new List<MediaRef>();
                var mediaRefsReader = new SQLiteCommand("select * from media_refs;", ConexaoMsgStoreDb).ExecuteReader();
                while (mediaRefsReader.Read())
                {
                    var mediaRef = new MediaRef();
                    mediaRef.Id = mediaRefsReader.GetInt32(0);
                    mediaRef.Path = mediaRefsReader.GetString(1);
                    mediaRef.RefCount = mediaRefsReader.IsDBNull(2) ? -1 : mediaRefsReader.GetInt32(2);

                    MediaRefs.Add(mediaRef);
                }
                #endregion

                #region importa messages
                Messages = new List<Message>();
                var messagesReader = new SQLiteCommand("select * from messages;", ConexaoMsgStoreDb).ExecuteReader();
                while (messagesReader.Read())
                {
                    var message = new Message();
                    message.Id = messagesReader.GetInt32(0);
                    message.KeyRemoteJid = messagesReader.GetString(1);
                    message.KeyFromMe = messagesReader.IsDBNull(2) ? -1 : messagesReader.GetInt32(2);
                    message.KeyId = messagesReader.GetString(3);
                    message.Status = messagesReader.IsDBNull(4) ? -1 : messagesReader.GetInt32(4);
                    message.NeedsPush = messagesReader.IsDBNull(5) ? -1 : messagesReader.GetInt32(5);
                    message.Data = messagesReader.IsDBNull(6) ? "" : messagesReader.GetString(6);
                    message.Timestamp = messagesReader.IsDBNull(7) ? DateTime.MinValue : messagesReader.GetInt64(7).TimeStampParaDateTime();
                    message.MediaUrl = messagesReader.IsDBNull(8) ? "" : messagesReader.GetString(8);
                    message.MediaMimeType = messagesReader.IsDBNull(9) ? "" : messagesReader.GetString(9);
                    message.MediaWaType = messagesReader.IsDBNull(10) ? MediaWhatsappType.MEDIA_WHATSAPP_TEXT :
                        (MediaWhatsappType) Enum.Parse(typeof(MediaWhatsappType), messagesReader.GetString(10));
                    message.MediaSize = messagesReader.IsDBNull(11) ? -1 : messagesReader.GetInt32(11);
                    message.MediaName = messagesReader.IsDBNull(12) ? "" : messagesReader.GetString(12);
                    message.MediaCaption = messagesReader.IsDBNull(13) ? "" : messagesReader.GetString(13);
                    message.MediaHash = messagesReader.IsDBNull(14) ? "" : messagesReader.GetString(14);
                    message.MediaDuration = messagesReader.IsDBNull(15) ? -1 : messagesReader.GetInt32(15);
                    message.Origin = messagesReader.IsDBNull(16) ? -1 : messagesReader.GetInt32(16);
                    message.Latitude = messagesReader.IsDBNull(17) ? -1 : messagesReader.GetDouble(17);
                    message.Longitude = messagesReader.IsDBNull(18) ? -1 : messagesReader.GetDouble(18);
                    message.ThumbImage = messagesReader.IsDBNull(19) ? "" : messagesReader.GetString(19);
                    message.RemoteResource = messagesReader.IsDBNull(20) ? "" : messagesReader.GetString(20);
                    message.ReceivedTimestamp = messagesReader.IsDBNull(21) ? DateTime.MinValue : messagesReader.GetInt64(21).TimeStampParaDateTime();
                    message.SendTimestamp = messagesReader.IsDBNull(22) ? DateTime.MinValue : messagesReader.GetInt64(22).TimeStampParaDateTime();
                    message.ReceiptServerTimestamp = messagesReader.IsDBNull(23) ? DateTime.MinValue : messagesReader.GetInt64(23).TimeStampParaDateTime();
                    message.ReceiptDeviceTimestamp = messagesReader.IsDBNull(24) ? DateTime.MinValue : messagesReader.GetInt64(24).TimeStampParaDateTime();
                    message.ReadDeviceTimestamp = messagesReader.IsDBNull(25) ? DateTime.MinValue : messagesReader.GetInt64(25).TimeStampParaDateTime();
                    message.PlayedDeviceTimestamp = messagesReader.IsDBNull(26) ? DateTime.MinValue : messagesReader.GetInt64(26).TimeStampParaDateTime();
                    message.RecipientCount = messagesReader.IsDBNull(28) ? -1 : messagesReader.GetInt32(28);
                    message.ParticipantHash = messagesReader.IsDBNull(29) ? "" : messagesReader.GetString(29);

                    if(!messagesReader.IsDBNull(27))
                    {
                        const int CHUNK_SIZE = 2 * 1024;
                        byte[] buffer = new byte[CHUNK_SIZE];
                        long bytesRead;
                        long fieldOffset = 0;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            while ((bytesRead = messagesReader.GetBytes(27, fieldOffset, buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, (int)bytesRead);
                                fieldOffset += bytesRead;
                            }
                            message.RawData = stream.ToArray();
                        }
                    }

                    Messages.Add(message);
                }

                foreach(var chat in Chats)
                {
                    chat.Mensagens = new List<Message>();
                    chat.Mensagens.AddRange(Messages.Where(p => p.KeyRemoteJid == chat.KeyRemoteJid).OrderBy(p => p.Timestamp));
                }

                    
                #endregion

                #region importa receipt
                Receipts = new List<Receipt>();
                var receiptsReader = new SQLiteCommand("select * from receipts;", ConexaoMsgStoreDb).ExecuteReader();
                while (receiptsReader.Read())
                {
                    var receipt = new Receipt();
                    receipt.Id = receiptsReader.GetInt32(0);
                    receipt.KeyRemoteJid = receiptsReader.GetString(1);
                    receipt.KeyId = receiptsReader.GetString(2);
                    receipt.RemoteResource = receiptsReader.GetString(3);
                    receipt.ReceiptDeviceTimestamp = receiptsReader.IsDBNull(4) ? DateTime.MinValue : receiptsReader.GetInt64(4).TimeStampParaDateTime();
                    receipt.ReadDeviceTimestamp = receiptsReader.IsDBNull(5) ? DateTime.MinValue : receiptsReader.GetInt64(5).TimeStampParaDateTime();
                    receipt.PlayedDeviceTimestamp = receiptsReader.IsDBNull(6) ? DateTime.MinValue : receiptsReader.GetInt64(6).TimeStampParaDateTime();

                    Receipts.Add(receipt);
                }
                #endregion

                ConexaoMsgStoreDb.Close();

                if (PossuiWaDb)
                {
                    ConexaoWaDb.Open();

                    #region importa wa_contacts
                    WaContacts = new List<WaContact>();
                    var waContactsReader = new SQLiteCommand("select * from wa_contacts;", ConexaoWaDb).ExecuteReader();
                    while (waContactsReader.Read())
                    {
                        var waContact = new WaContact();
                        waContact.Id = waContactsReader.GetInt32(0);
                        waContact.Jid = waContactsReader.GetString(1);
                        waContact.IsWhatsappUser = waContactsReader.GetBoolean(2);
                        waContact.Status = waContactsReader.IsDBNull(3) ? "" : waContactsReader.GetString(3);
                        waContact.StatusTimestamp = waContactsReader.IsDBNull(4) ? DateTime.MinValue : waContactsReader.GetInt64(4).TimeStampParaDateTime();
                        waContact.Number = waContactsReader.IsDBNull(5) ? "" : waContactsReader.GetString(5);
                        waContact.RawContactId = waContactsReader.IsDBNull(6) ? -1 : waContactsReader.GetInt32(6);
                        waContact.DisplayName = waContactsReader.IsDBNull(7) ? "" : waContactsReader.GetString(7);
                        waContact.PhoneType = waContactsReader.IsDBNull(8) ? -1 : waContactsReader.GetInt32(8);
                        waContact.PhoneLabel = waContactsReader.IsDBNull(9) ? "" : waContactsReader.GetString(9);
                        waContact.UnseenMsgCount = waContactsReader.IsDBNull(10) ? -1 : waContactsReader.GetInt32(10);
                        waContact.PhotoTs = waContactsReader.IsDBNull(11) ? -1 : waContactsReader.GetInt64(11);
                        waContact.ThumbTs = waContactsReader.IsDBNull(12) ? -1 : waContactsReader.GetInt64(12);
                        waContact.PhotoIdTimestamp = waContactsReader.IsDBNull(13) ? DateTime.MinValue : waContactsReader.GetInt64(13).TimeStampParaDateTime();
                        waContact.GivenName = waContactsReader.IsDBNull(14) ? "" : waContactsReader.GetString(14);
                        waContact.FamilyName = waContactsReader.IsDBNull(15) ? "" : waContactsReader.GetString(15);
                        waContact.WaName = waContactsReader.IsDBNull(16) ? "" : waContactsReader.GetString(16);
                        waContact.SortName = waContactsReader.IsDBNull(17) ? "" : waContactsReader.GetString(17);
                        waContact.Callability = waContactsReader.IsDBNull(18) ? "" : waContactsReader.GetString(18);

                        bool isGrupo = waContact.Jid.Contains("-");
                        if (!isGrupo)
                        {
                            var nomeExibido = waContact.DisplayName;
                            var nomeWa = waContact.WaName;
                            var telefone = waContact.WaName;

                            waContact.NomeContato += !String.IsNullOrWhiteSpace(nomeExibido) && !String.IsNullOrWhiteSpace(nomeWa) ?
                                                nomeExibido + ", " + nomeWa : nomeExibido + nomeWa;
                            if (String.IsNullOrWhiteSpace(waContact.NomeContato)) waContact.NomeContato += waContact;
                        }
                        else waContact.NomeContato = waContact.DisplayName;

                        WaContacts.Add(waContact);
                    }

                    foreach (var chat in Chats) chat.Contato = WaContacts.FirstOrDefault(p => p.Jid == chat.KeyRemoteJid);

                    foreach (var groupParticipant in GroupParticipants) groupParticipant.Contato = WaContacts.FirstOrDefault(p => p.Jid == groupParticipant.Jid);
                    #endregion

                    ConexaoWaDb.Close();

                    resultado = "SUCESSO: Bancos de Dados Carregados.";
                }
            } catch (Exception ex)
            {
                resultado = "ERRO: "+ex.Message;
            }

            return resultado;
        }       


    }
}
