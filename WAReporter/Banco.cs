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
        static SQLiteConnection ConexaoChatStorage;
        static bool PossuiBancoContatos = false;
        static SQLiteConnection ConexaoContatos;

        public static List<Chat> Chats { get; set; }
        public static List<GroupParticipant> GroupParticipants { get; set; }
        public static List<GroupParticipantHistory> GroupParticipantHistories { get; set; }
        public static List<MediaRef> MediaRefs { get; set; }
        public static List<Message> Messages { get; set; }
        public static List<Receipt> Receipts { get; set; }
        public static List<WaContact> WaContacts { get; set; }

        public static TiposDispositivo TipoDispositivo { get; set; }

        /// <summary>
        /// Inicializa a conexão e efetua o carregamento do banco sqlite Android de endereço definido por parâmetro
        /// </summary>
        /// <returns>Status do processamento</returns>
        /// <param name="enderecoMsgStore">String contendo o endereço do arquivo msgstore.db a ser carregado</param>
        /// <param name="enderecoWaDb">String contendo o endereço do arquivo wa.db a ser carregado</param>
        public static String CarregarBancoAndroid(String enderecoMsgStore, String enderecoWaDb)
        {
            TipoDispositivo = TiposDispositivo.ANDROID;

            String resultado = "";
            try
            {
                ConexaoChatStorage = new SQLiteConnection("Data Source=" + enderecoMsgStore + ";Version=3;");

                PossuiBancoContatos = !String.IsNullOrWhiteSpace(enderecoWaDb);
                if (PossuiBancoContatos) ConexaoContatos = new SQLiteConnection("Data Source=" + enderecoWaDb + ";Version=3;");

                ConexaoChatStorage.Open();

                #region importa chat_list
                Chats = new List<Chat>();
                var chatListReader = new SQLiteCommand("select * from chat_list;", ConexaoChatStorage).ExecuteReader();
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

                    if (!String.IsNullOrWhiteSpace(chat.Subject))
                    {
                        var s = chatListReader.GetValue(3);


                    }

                    Chats.Add(chat);
                }
                #endregion

                #region importa group_participants
                GroupParticipants = new List<GroupParticipant>();
                try
                {
                    var groupParticipantReader = new SQLiteCommand("select * from group_participants;", ConexaoChatStorage).ExecuteReader();
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
                  
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("no such table")) throw ex;
                }

                foreach (var chat in Chats.Where(p => p.KeyRemoteJid.Contains("-")))
                {
                    chat.ParticipantesGrupo = new List<GroupParticipant>();
                    chat.ParticipantesGrupo.AddRange(GroupParticipants.Where(p => p.Gjid == chat.KeyRemoteJid));
                }
                #endregion

                #region importa group_participants_history
                try
                {
                    GroupParticipantHistories = new List<GroupParticipantHistory>();
                    var groupParticipantHistoriesReader = new SQLiteCommand("select * from group_participants_history;", ConexaoChatStorage).ExecuteReader();
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

                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("no such table")) throw ex;
                }
                #endregion

                #region importa media_refs
                try
                {
                    MediaRefs = new List<MediaRef>();
                    var mediaRefsReader = new SQLiteCommand("select * from media_refs;", ConexaoChatStorage).ExecuteReader();
                    while (mediaRefsReader.Read())
                    {
                        var mediaRef = new MediaRef();
                        mediaRef.Id = mediaRefsReader.GetInt32(0);
                        mediaRef.Path = mediaRefsReader.GetString(1);
                        mediaRef.RefCount = mediaRefsReader.IsDBNull(2) ? -1 : mediaRefsReader.GetInt32(2);

                        MediaRefs.Add(mediaRef);
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("no such table")) throw ex;
                }                
                #endregion

                #region importa messages
                Messages = new List<Message>();
                var messagesReader = new SQLiteCommand("select * from messages;", ConexaoChatStorage).ExecuteReader();
                while (messagesReader.Read())
                {
                    var message = new Message();
                    message.Id = messagesReader.GetInt32(messagesReader.GetOrdinal("_id"));
                    if (message.Id == 1) continue;
                    message.KeyRemoteJid = messagesReader.GetString(messagesReader.GetOrdinal("key_remote_jid"));
                    message.KeyFromMe = messagesReader.IsDBNull(messagesReader.GetOrdinal("key_from_me")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("key_from_me"));
                    message.KeyId = messagesReader.GetString(messagesReader.GetOrdinal("key_id"));
                    message.Status = messagesReader.IsDBNull(messagesReader.GetOrdinal("status")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("status"));
                    message.NeedsPush = messagesReader.IsDBNull(messagesReader.GetOrdinal("needs_push")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("needs_push"));
                    message.Data = messagesReader.IsDBNull(messagesReader.GetOrdinal("data")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("data"));
                    message.Timestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("timestamp")).TimeStampParaDateTime();
                    message.MediaUrl = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_url")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_url"));
                    message.MediaMimeType = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_mime_type")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_mime_type"));
                    message.MediaWaType = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_wa_type")) ? MediaWhatsappType.MEDIA_WHATSAPP_TEXT :
                        (MediaWhatsappType)Enum.Parse(typeof(MediaWhatsappType), messagesReader.GetString(messagesReader.GetOrdinal("media_wa_type")));
                    message.MediaSize = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_size")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("media_size"));
                    message.MediaName = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_name")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_name"));
                    message.MediaHash = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_hash")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_hash"));
                    message.MediaCaption = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_caption")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_caption"));

                    message.MediaDuration = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_duration")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("media_duration"));

                    message.Origin = messagesReader.IsDBNull(messagesReader.GetOrdinal("origin")) ? -1 : messagesReader.GetDouble(messagesReader.GetOrdinal("origin"));
                    message.Latitude = messagesReader.IsDBNull(messagesReader.GetOrdinal("latitude")) ? "" : messagesReader.GetValue(messagesReader.GetOrdinal("latitude")).ToString();
                    message.Longitude = messagesReader.IsDBNull(messagesReader.GetOrdinal("longitude")) ? "" : messagesReader.GetValue(messagesReader.GetOrdinal("longitude")).ToString();
                    message.ThumbImage = messagesReader.IsDBNull(messagesReader.GetOrdinal("thumb_image")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("thumb_image"));


                    message.RemoteResource = messagesReader.IsDBNull(messagesReader.GetOrdinal("remote_resource")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("remote_resource"));
                    message.ReceivedTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("received_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("received_timestamp")).TimeStampParaDateTime();
                    message.SendTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("send_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("send_timestamp")).TimeStampParaDateTime();
                    message.ReceiptServerTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("receipt_server_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("receipt_server_timestamp")).TimeStampParaDateTime();
                    //   var val = messagesReader.GetValue(24);
                    //  var v2 = val.GetType();

                    message.ReceiptDeviceTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("receipt_device_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("receipt_device_timestamp")).TimeStampParaDateTime();
                    message.ReadDeviceTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("read_device_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("read_device_timestamp")).TimeStampParaDateTime();
                    message.PlayedDeviceTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("played_device_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("played_device_timestamp")).TimeStampParaDateTime();
                    message.RecipientCount = messagesReader.IsDBNull(messagesReader.GetOrdinal("recipient_count")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("recipient_count"));
                    message.ParticipantHash = messagesReader.IsDBNull(messagesReader.GetOrdinal("participant_hash")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("participant_hash"));


                    if (!messagesReader.IsDBNull(messagesReader.GetOrdinal("raw_data")))
                    {
                        const int CHUNK_SIZE = 2 * 1024;
                        byte[] buffer = new byte[CHUNK_SIZE];
                        long bytesRead;
                        long fieldOffset = 0;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            while ((bytesRead = messagesReader.GetBytes(messagesReader.GetOrdinal("raw_data"), fieldOffset, buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, (int)bytesRead);
                                fieldOffset += bytesRead;
                            }
                            message.RawData = stream.ToArray();
                        }
                    }

                    Messages.Add(message);
                }

                foreach (var chat in Chats)
                {
                    chat.Mensagens = new List<Message>();
                    chat.Mensagens.AddRange(Messages.Where(p => p.KeyRemoteJid == chat.KeyRemoteJid).OrderBy(p => p.Timestamp));
                }


                #endregion

                #region importa receipt
                try
                {
                    Receipts = new List<Receipt>();
                    var receiptsReader = new SQLiteCommand("select * from receipts;", ConexaoChatStorage).ExecuteReader();
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
                } catch (Exception ex)
                {
                    if (!ex.Message.Contains("no such table")) throw ex;
                }
                #endregion

                ConexaoChatStorage.Close();


                WaContacts = new List<WaContact>();
                if (PossuiBancoContatos)
                {
                    ConexaoContatos.Open();

                    #region importa wa_contacts
                    var waContactsReader = new SQLiteCommand("select * from wa_contacts;", ConexaoContatos).ExecuteReader();
                    while (waContactsReader.Read())
                    {
                        var waContact = new WaContact();
                        waContact.Id = waContactsReader.GetInt32(waContactsReader.GetOrdinal("_id"));
                        waContact.Jid = waContactsReader.GetString(waContactsReader.GetOrdinal("jid"));
                        waContact.IsWhatsappUser = waContactsReader.GetBoolean(waContactsReader.GetOrdinal("is_whatsapp_user"));
                        waContact.Status = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("status")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("status"));
                        waContact.StatusTimestamp = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("status_timestamp")) ? DateTime.MinValue : waContactsReader.GetInt64(waContactsReader.GetOrdinal("status_timestamp")).TimeStampParaDateTime();
                        waContact.Number = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("number")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("number"));
                        waContact.RawContactId = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("raw_contact_id")) ? -1 : waContactsReader.GetInt32(waContactsReader.GetOrdinal("raw_contact_id"));
                            waContact.DisplayName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("display_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("display_name"));
                        waContact.PhoneType = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("phone_type")) ? -1 : waContactsReader.GetInt32(waContactsReader.GetOrdinal("phone_type"));
                        waContact.PhoneLabel = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("phone_label")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("phone_label"));
                        waContact.UnseenMsgCount = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("unseen_msg_count")) ? -1 : waContactsReader.GetInt32(waContactsReader.GetOrdinal("unseen_msg_count"));
                        waContact.PhotoTs = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("photo_ts")) ? -1 : waContactsReader.GetInt64(waContactsReader.GetOrdinal("photo_ts"));
                        waContact.ThumbTs = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("thumb_ts")) ? -1 : waContactsReader.GetInt64(waContactsReader.GetOrdinal("thumb_ts"));
                        waContact.PhotoIdTimestamp = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("photo_id_timestamp")) ? DateTime.MinValue : waContactsReader.GetInt64(waContactsReader.GetOrdinal("photo_id_timestamp")).TimeStampParaDateTime();
                        waContact.GivenName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("given_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("given_name"));
                        waContact.FamilyName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("family_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("family_name"));
                        waContact.WaName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("wa_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("wa_name"));
                        waContact.SortName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("sort_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("sort_name"));
                        waContact.Callability = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("callability")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("callability"));

                        bool isGrupo = waContact.Jid.Contains("-");
                        if (!isGrupo)
                        {
                            var nomeExibido = waContact.DisplayName;
                            var nomeWa = waContact.WaName;
                            var telefone = waContact.Jid;

                            waContact.NomeContato += !String.IsNullOrWhiteSpace(nomeExibido) && !String.IsNullOrWhiteSpace(nomeWa) ?
                                                nomeExibido + ", " + nomeWa : nomeExibido + nomeWa;
                            if (String.IsNullOrWhiteSpace(waContact.NomeContato)) waContact.NomeContato += waContact.Jid;
                        }
                        else waContact.NomeContato = waContact.DisplayName;

                        WaContacts.Add(waContact);
                    }

                   
                    #endregion

                    ConexaoContatos.Close();
                }

                foreach (var chat in Chats) chat.Contato = WaContacts.FirstOrDefault(p => p.Jid == chat.KeyRemoteJid) ?? new WaContact { Jid = chat.KeyRemoteJid, NomeContato = chat.KeyRemoteJid.Contains("-") ? chat.Subject : chat.KeyRemoteJid };

                foreach (var groupParticipant in GroupParticipants) groupParticipant.Contato = WaContacts.FirstOrDefault(p => p.Jid == groupParticipant.Jid) ?? new WaContact { Jid = groupParticipant.Jid, NomeContato = groupParticipant.Jid };

                foreach (var message in Messages) message.Contato = WaContacts.FirstOrDefault(p => message.KeyRemoteJid.Contains("-") ? p.Jid == message.RemoteResource : p.Jid == message.KeyRemoteJid ) ?? new WaContact { Jid = message.RemoteResource, NomeContato = message.RemoteResource };
                






                resultado = "SUCESSO: Bancos de Dados Carregados.";

            }
            catch (ArgumentNullException ex)
            {
                resultado = "ERRO: " + ex.Message;
            }

            return resultado;
        }

        /// <summary>
        /// Inicializa a conexão e efetua o carregamento do banco sqlite iPhone de endereço definido por parâmetro
        /// </summary>
        /// <returns>Status do processamento</returns>
        /// <param name="enderecoChatStorage">String contendo o endereço do arquivo ChatStorage.sqlite a ser carregado</param>
        /// <param name="enderecoContacts">String contendo o endereço do arquivo Contacts.sqlite a ser carregado</param>
        public static String CarregarBancoIPhone(String enderecoChatStorage, String enderecoContacts)
        {
            TipoDispositivo = TiposDispositivo.IOS;

            String resultado = "";
            try
            {
                ConexaoChatStorage = new SQLiteConnection("Data Source=" + enderecoChatStorage + ";Version=3;");

                PossuiBancoContatos = !String.IsNullOrWhiteSpace(enderecoContacts);
                if (PossuiBancoContatos) ConexaoContatos = new SQLiteConnection("Data Source=" + enderecoContacts + ";Version=3;");

                ConexaoChatStorage.Open();

                #region importa ZWACHATSESSION
                Chats = new List<Chat>();
                var chatSessionReader = new SQLiteCommand("select * from ZWACHATSESSION;", ConexaoChatStorage).ExecuteReader();
                while (chatSessionReader.Read())
                {
                    var chat = new Chat();
                    chat.Id = chatSessionReader.GetInt32(chatSessionReader.GetOrdinal("Z_PK"));
                    chat.KeyRemoteJid = chatSessionReader.IsDBNull(1) ? null : chatSessionReader.GetString(chatSessionReader.GetOrdinal("ZCONTACTJID"));
                    //chat.MessageTableId = chatSessionReader.IsDBNull(2) ? 0 : chatSessionReader.GetInt32(2);
                    chat.Subject = chatSessionReader.IsDBNull(3) ? null : chatSessionReader.GetString(chatSessionReader.GetOrdinal("ZPARTNERNAME"));
                    //chat.Creation = chatSessionReader.IsDBNull(4) ? DateTime.MinValue : chatSessionReader.GetInt64(4).TimeStampParaDateTime();
                    chat.LastReadMessageTableId = chatSessionReader.GetInt32(chatSessionReader.GetOrdinal("ZLASTMESSAGE"));
                    //chat.LastReadReceiptSentMessageTableId = chatSessionReader.IsDBNull(6) ? -1 : chatSessionReader.GetInt32(6);
                    //chat.Archived = chatSessionReader.IsDBNull(7) ? -1 : chatSessionReader.GetInt32(7);
                    //chat.SortTimestamp = chatSessionReader.IsDBNull(8) ? -1 : chatSessionReader.GetInt32(8);
                    //chat.ModTag = chatSessionReader.IsDBNull(9) ? -1 : chatSessionReader.GetInt32(9);
                    
                    Chats.Add(chat);
                }
                #endregion

                #region importa group_participants
                GroupParticipants = new List<GroupParticipant>();
                //try
                //{
                    var groupParticipantReader = new SQLiteCommand("select * from (select * from ZWAGROUPMEMBER LEFT JOIN ZWAGROUPINFO on "+
                        "ZWAGROUPMEMBER.ZCHATSESSION = ZWAGROUPINFO.Z_PK) LEFT JOIN ZWACHATSESSION on ZCHATSESSION = ZWACHATSESSION.Z_PK;", ConexaoChatStorage).ExecuteReader();
                    while (groupParticipantReader.Read())
                    {
                        var groupParticipant = new GroupParticipant();
                        groupParticipant.Id = groupParticipantReader.GetInt32(groupParticipantReader.GetOrdinal("Z_PK"));
                        groupParticipant.Gjid = groupParticipantReader.GetString(groupParticipantReader.GetOrdinal("ZCONTACTJID"));
                        groupParticipant.Jid = groupParticipantReader.GetString(groupParticipantReader.GetOrdinal("ZMEMBERJID"));
                        groupParticipant.Admin = groupParticipantReader.GetInt32(groupParticipantReader.GetOrdinal("ZISADMIN"));
                        groupParticipant.ContactName = groupParticipantReader.GetString(groupParticipantReader.GetOrdinal("ZCONTACTNAME"));
                        //groupParticipant.Pending = groupParticipantReader.IsDBNull(4) ? -1 : groupParticipantReader.GetInt32(4);

                        GroupParticipants.Add(groupParticipant);
                    }
                //}
                //catch (Exception ex)
                //{
                //    if (!ex.Message.Contains("no such table")) throw ex;
                //}

                foreach (var chat in Chats.Where(p => p.KeyRemoteJid.Contains("-")))
                {
                    chat.ParticipantesGrupo = new List<GroupParticipant>();
                    chat.ParticipantesGrupo.AddRange(GroupParticipants.Where(p => p.Gjid == chat.KeyRemoteJid));
                }
                #endregion

                #region importa group_participants_history
                //try
                //{
                //    GroupParticipantHistories = new List<GroupParticipantHistory>();
                //    var groupParticipantHistoriesReader = new SQLiteCommand("select * from group_participants_history;", ConexaoChatStorage).ExecuteReader();
                //    while (groupParticipantHistoriesReader.Read())
                //    {
                //        var groupParticipantHistory = new GroupParticipantHistory();
                //        groupParticipantHistory.Id = groupParticipantHistoriesReader.GetInt32(0);
                //        groupParticipantHistory.Timestamp = groupParticipantHistoriesReader.IsDBNull(1) ? DateTime.MinValue : groupParticipantHistoriesReader.GetInt64(1).TimeStampParaDateTime();
                //        groupParticipantHistory.GJid = groupParticipantHistoriesReader.GetString(2);
                //        groupParticipantHistory.Jid = groupParticipantHistoriesReader.GetString(3);
                //        groupParticipantHistory.Action = groupParticipantHistoriesReader.IsDBNull(4) ? -1 : groupParticipantHistoriesReader.GetInt32(4);
                //        groupParticipantHistory.OldPhash = groupParticipantHistoriesReader.GetString(5);
                //        groupParticipantHistory.NewPhash = groupParticipantHistoriesReader.GetString(6);

                //        GroupParticipantHistories.Add(groupParticipantHistory);
                //    }

                //}
                //catch (Exception ex)
                //{
                //    if (!ex.Message.Contains("no such table")) throw ex;
                //}
                #endregion

                #region importa media_refs
                //try
                //{
                //    MediaRefs = new List<MediaRef>();
                //    var mediaRefsReader = new SQLiteCommand("select * from media_refs;", ConexaoChatStorage).ExecuteReader();
                //    while (mediaRefsReader.Read())
                //    {
                //        var mediaRef = new MediaRef();
                //        mediaRef.Id = mediaRefsReader.GetInt32(0);
                //        mediaRef.Path = mediaRefsReader.GetString(1);
                //        mediaRef.RefCount = mediaRefsReader.IsDBNull(2) ? -1 : mediaRefsReader.GetInt32(2);

                //        MediaRefs.Add(mediaRef);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    if (!ex.Message.Contains("no such table")) throw ex;
                //}
                #endregion

                #region importa messages
                Messages = new List<Message>();
                var messagesReader = new SQLiteCommand("select * from (select * from ZWAMESSAGE left join ZWAMEDIAITEM on ZWAMESSAGE.Z_PK = ZWAMEDIAITEM.ZMESSAGE) left join ZWAGROUPMEMBER on ZGROUPMEMBER = ZWAGROUPMEMBER.Z_PK;", ConexaoChatStorage).ExecuteReader();
                while (messagesReader.Read())
                {
                    var message = new Message();
                    message.Id = messagesReader.GetInt32(messagesReader.GetOrdinal("Z_PK"));
                    //if (message.Id == 1) continue;
                    message.KeyFromMe = messagesReader.GetInt32(messagesReader.GetOrdinal("ZISFROMME"));
                    message.KeyRemoteJid = message.KeyFromMe == 1 ?
                        messagesReader.GetString(messagesReader.GetOrdinal("ZTOJID")) :
                        messagesReader.GetString(messagesReader.GetOrdinal("ZFROMJID")) ;
                    
                    message.Status = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZMESSAGESTATUS")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("ZMESSAGESTATUS"));
                    message.Data = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZTEXT")) ? "" :  messagesReader.GetString(messagesReader.GetOrdinal("ZTEXT"));
                    message.MediaUrl = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZMEDIAURL")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("ZMEDIAURL"));
                    message.MediaLocalPath = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZMEDIALOCALPATH")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("ZMEDIALOCALPATH"));
                   

                    message.Latitude = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZLATITUDE")) ? "" : messagesReader.GetValue(messagesReader.GetOrdinal("ZLATITUDE")).ToString();
                    message.Longitude = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZLONGITUDE")) ? "" : messagesReader.GetValue(messagesReader.GetOrdinal("ZLONGITUDE")).ToString();
                    message.ThumbImage = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZXMPPTHUMBPATH")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("ZXMPPTHUMBPATH"));

                    if(message.KeyFromMe == 0 && message.KeyRemoteJid.Contains("."))
                        message.RemoteResource = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZMEMBERJID")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("ZMEMBERJID"));

                    message.MediaWaType = messagesReader.IsDBNull(messagesReader.GetOrdinal("ZMESSAGETYPE")) ? MediaWhatsappType.MEDIA_WHATSAPP_TEXT :
                     (MediaWhatsappType)Enum.Parse(typeof(MediaWhatsappType), messagesReader.GetInt32(messagesReader.GetOrdinal("ZMESSAGETYPE")).ToString());

                    if (message.MediaWaType == MediaWhatsappType.MEDIA_WHATSAPP_VIDEO)
                        message.MediaWaType = MediaWhatsappType.MEDIA_WHATSAPP_AUDIO;
                    else if (message.MediaWaType == MediaWhatsappType.MEDIA_WHATSAPP_AUDIO)
                        message.MediaWaType = MediaWhatsappType.MEDIA_WHATSAPP_VIDEO;   

                    //message.MediaDuration = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_duration")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("media_duration"));
                    //message.Origin = messagesReader.IsDBNull(messagesReader.GetOrdinal("origin")) ? -1 : messagesReader.GetDouble(messagesReader.GetOrdinal("origin"));
                    //message.NeedsPush = messagesReader.IsDBNull(messagesReader.GetOrdinal("needs_push")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("needs_push"));
                    //message.KeyId = messagesReader.GetString(messagesReader.GetOrdinal("key_id"));
                    //message.MediaMimeType = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_mime_type")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_mime_type"));
                    //message.MediaWaType = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_wa_type")) ? MediaWhatsappType.MEDIA_WHATSAPP_TEXT :
                    //    (MediaWhatsappType)Enum.Parse(typeof(MediaWhatsappType), messagesReader.GetString(messagesReader.GetOrdinal("media_wa_type")));
                    //message.MediaSize = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_size")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("media_size"));
                    //message.MediaName = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_name")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_name"));
                    //message.MediaHash = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_hash")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_hash"));
                    //message.MediaCaption = messagesReader.IsDBNull(messagesReader.GetOrdinal("media_caption")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("media_caption"));

                    //message.ReceivedTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("received_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("received_timestamp")).TimeStampParaDateTime();
                    //message.SendTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("send_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("send_timestamp")).TimeStampParaDateTime();
                    //message.ReceiptServerTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("receipt_server_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("receipt_server_timestamp")).TimeStampParaDateTime();
                    //   var val = messagesReader.GetValue(24);
                    //  var v2 = val.GetType();

                    //message.ReceiptDeviceTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("receipt_device_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("receipt_device_timestamp")).TimeStampParaDateTime();
                    //message.ReadDeviceTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("read_device_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("read_device_timestamp")).TimeStampParaDateTime();
                    //message.PlayedDeviceTimestamp = messagesReader.IsDBNull(messagesReader.GetOrdinal("played_device_timestamp")) ? DateTime.MinValue : messagesReader.GetInt64(messagesReader.GetOrdinal("played_device_timestamp")).TimeStampParaDateTime();
                    //message.RecipientCount = messagesReader.IsDBNull(messagesReader.GetOrdinal("recipient_count")) ? -1 : messagesReader.GetInt32(messagesReader.GetOrdinal("recipient_count"));
                    //message.ParticipantHash = messagesReader.IsDBNull(messagesReader.GetOrdinal("participant_hash")) ? "" : messagesReader.GetString(messagesReader.GetOrdinal("participant_hash"));


                    var dataBanco = messagesReader.GetDouble(messagesReader.GetOrdinal("ZMESSAGEDATE"));
                    var d2 = (Convert.ToInt64(dataBanco));

                    var d3 = (978307200 + d2) * 1000;

                    message.Timestamp = (d3).TimeStampParaDateTime();
                    
                    Messages.Add(message);
                }

                foreach (var chat in Chats)
                {
                    chat.Mensagens = new List<Message>();
                    chat.Mensagens.AddRange(Messages.Where(p => p.KeyRemoteJid == chat.KeyRemoteJid).OrderBy(p => p.Timestamp));
                }


                #endregion

                #region importa receipt
                //try
                //{
                //    Receipts = new List<Receipt>();
                //    var receiptsReader = new SQLiteCommand("select * from receipts;", ConexaoChatStorage).ExecuteReader();
                //    while (receiptsReader.Read())
                //    {
                //        var receipt = new Receipt();
                //        receipt.Id = receiptsReader.GetInt32(0);
                //        receipt.KeyRemoteJid = receiptsReader.GetString(1);
                //        receipt.KeyId = receiptsReader.GetString(2);
                //        receipt.RemoteResource = receiptsReader.GetString(3);
                //        receipt.ReceiptDeviceTimestamp = receiptsReader.IsDBNull(4) ? DateTime.MinValue : receiptsReader.GetInt64(4).TimeStampParaDateTime();
                //        receipt.ReadDeviceTimestamp = receiptsReader.IsDBNull(5) ? DateTime.MinValue : receiptsReader.GetInt64(5).TimeStampParaDateTime();
                //        receipt.PlayedDeviceTimestamp = receiptsReader.IsDBNull(6) ? DateTime.MinValue : receiptsReader.GetInt64(6).TimeStampParaDateTime();

                //        Receipts.Add(receipt);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    if (!ex.Message.Contains("no such table")) throw ex;
                //}
                #endregion

                ConexaoChatStorage.Close();


                WaContacts = new List<WaContact>();
                if (PossuiBancoContatos)
                {
                    ConexaoContatos.Open();

                    #region importa wa_contacts
                    var waContactsReader = new SQLiteCommand("select * from (select ZWACONTACT.Z_PK as ID, ZWACONTACT.ZFIRSTNAME,  ZWACONTACT.ZFULLNAME, "+
                        "ZWAPHONE.Z_PK, ZWAPHONE.ZPHONE as NUMERO from ZWACONTACT LEFT JOIN ZWAPHONE on ZWACONTACT.Z_PK = ZWAPHONE.ZCONTACT) Z1 "+
                        "LEFT JOIN ZWASTATUS ON  Z1.Z_PK = ZWASTATUS.ZPHONE;", ConexaoContatos).ExecuteReader();
                    while (waContactsReader.Read())
                    {
                        var waContact = new WaContact();
                        waContact.Id = waContactsReader.GetInt32(waContactsReader.GetOrdinal("ID"));
                        waContact.Jid = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("ZWHATSAPPID")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("ZWHATSAPPID"));
                        //waContact.IsWhatsappUser = waContactsReader.GetBoolean(waContactsReader.GetOrdinal("is_whatsapp_user"));
                        waContact.Status = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("ZTEXT")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("ZTEXT"));
                        //waContact.StatusTimestamp = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("status_timestamp")) ? DateTime.MinValue : waContactsReader.GetInt64(waContactsReader.GetOrdinal("status_timestamp")).TimeStampParaDateTime();
                        waContact.Number = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("NUMERO")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("NUMERO"));
                        //waContact.RawContactId = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("raw_contact_id")) ? -1 : waContactsReader.GetInt32(waContactsReader.GetOrdinal("raw_contact_id"));
                        waContact.DisplayName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("ZFIRSTNAME")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("ZFIRSTNAME"));
                        //waContact.PhoneType = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("phone_type")) ? -1 : waContactsReader.GetInt32(waContactsReader.GetOrdinal("phone_type"));
                        //waContact.PhoneLabel = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("phone_label")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("phone_label"));
                        //waContact.UnseenMsgCount = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("unseen_msg_count")) ? -1 : waContactsReader.GetInt32(waContactsReader.GetOrdinal("unseen_msg_count"));
                        //waContact.PhotoTs = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("photo_ts")) ? -1 : waContactsReader.GetInt64(waContactsReader.GetOrdinal("photo_ts"));
                        //waContact.ThumbTs = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("thumb_ts")) ? -1 : waContactsReader.GetInt64(waContactsReader.GetOrdinal("thumb_ts"));
                        //waContact.PhotoIdTimestamp = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("photo_id_timestamp")) ? DateTime.MinValue : waContactsReader.GetInt64(waContactsReader.GetOrdinal("photo_id_timestamp")).TimeStampParaDateTime();
                        //waContact.GivenName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("given_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("given_name"));
                        //waContact.FamilyName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("family_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("family_name"));
                        waContact.WaName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("ZFULLNAME")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("ZFULLNAME"));
                        //waContact.SortName = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("sort_name")) ? "" : waContactsReader.GetString(waContactsReader.GetOrdinal("sort_name"));
                        waContact.Callability = waContactsReader.IsDBNull(waContactsReader.GetOrdinal("ZCALLABILITY")) ? "" : waContactsReader.GetInt32(waContactsReader.GetOrdinal("ZCALLABILITY")).ToString();
                                                
                        var nomeExibido = waContact.DisplayName;
                        var nomeWa = waContact.WaName;

                        waContact.NomeContato += !String.IsNullOrWhiteSpace(nomeExibido) && !String.IsNullOrWhiteSpace(nomeWa) ?
                                            nomeExibido + ", " + nomeWa : nomeExibido + nomeWa;
                        if (String.IsNullOrWhiteSpace(waContact.NomeContato)) waContact.NomeContato += waContact.Jid;
                        
                        WaContacts.Add(waContact);
                    }


                    #endregion

                    ConexaoContatos.Close();
                }

                foreach (var chat in Chats)
                {
                    chat.Contato = WaContacts.FirstOrDefault(p => p.Jid == chat.KeyRemoteJid.Split('@')[0]) ??
                        new WaContact { Jid = chat.KeyRemoteJid, NomeContato = chat.KeyRemoteJid.Contains("-") ? chat.Subject : chat.KeyRemoteJid };
                }

                foreach (var groupParticipant in GroupParticipants)
                {
                        if (Banco.TipoDispositivo == TiposDispositivo.ANDROID)
                            groupParticipant.Contato = new WaContact { Jid = groupParticipant.Jid, NomeContato = groupParticipant.Jid };
                        else if (Banco.TipoDispositivo == TiposDispositivo.IOS)
                            groupParticipant.Contato = new WaContact { Jid = groupParticipant.Jid, NomeContato = groupParticipant.ContactName };
                        WaContacts.Add(groupParticipant.Contato);
                }

                foreach (var message in Messages)
                {
                    if (message.KeyRemoteJid.Contains("-"))
                    {
                        message.Contato = WaContacts.LastOrDefault(p => p.Jid == message.RemoteResource) ??
                            new WaContact { Jid = message.KeyRemoteJid, NomeContato = message.KeyRemoteJid+"_)" };
                    }
                    else
                        message.Contato = WaContacts.FirstOrDefault(p => p.Jid == message.KeyRemoteJid.Split('@')[0]) ??
                            new WaContact { Jid = message.KeyRemoteJid, NomeContato = message.KeyRemoteJid };
                }
                
                resultado = "SUCESSO: Bancos de Dados Carregados.";

            }
            catch (ArgumentNullException ex)
            {
                resultado = "ERRO: " + ex.Message;
            }


            return resultado;
        }
    }

    public enum TiposDispositivo
    {
        ANDROID,
        IOS
    }
}
