using System;

namespace WAReporter.Modelo
{
    public class WaContact
    {
        //Esses campos correspondem aos existentes na tabela wa_contacts dos bancos wa.db

        public int Id { get; set; }
        public String Jid { get; set; }
        public bool IsWhatsappUser { get; set; }
        public String Status { get; set; }
        public DateTime StatusTimestamp { get; set; }
        public String Number { get; set; }
        public int RawContactId { get; set; }
        public String DisplayName { get; set; }
        public int PhoneType { get; set; }
        public String PhoneLabel { get; set; }
        public int UnseenMsgCount { get; set; }
        public long PhotoTs { get; set; }
        public long ThumbTs { get; set; }
        public DateTime PhotoIdTimestamp { get; set; }
        public String GivenName { get; set; }
        public String FamilyName { get; set; }
        public String WaName { get; set; }
        public String SortName { get; set; }
        public String Callability { get; set; }
        public String PicturePath { get; set; }

        public String NomeContato { get; set; }
    }
}
