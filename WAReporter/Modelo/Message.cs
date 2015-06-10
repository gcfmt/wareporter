using System;

namespace WAReporter.Modelo
{
    public class Message
    {
        //Esses campos correspondem aos existentes na tabela messages dos bancos msgstore.db

        public int Id { get; set; }
        public String KeyRemoteJid { get; set; }
        public int KeyFromMe { get; set; }
        public String KeyId { get; set; }
        public int Status { get; set; }
        public int NeedsPush { get; set; }
        public String Data { get; set; }
        public DateTime Timestamp { get; set; }
        public String MediaUrl { get; set; }
        public String MediaMimeType { get; set; }
        public MediaWhatsappType MediaWaType { get; set; }
        public int MediaSize { get; set; }
        public String MediaName { get; set; }
        public String MediaCaption { get; set; }
        public String MediaHash { get; set; }
        public int MediaDuration { get; set; }
        public int Origin{ get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public String ThumbImage { get; set; }
        public String RemoteResource { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
        public DateTime SendTimestamp { get; set; }
        public DateTime ReceiptServerTimestamp { get; set; }
        public DateTime ReceiptDeviceTimestamp { get; set; }
        public DateTime ReadDeviceTimestamp { get; set; }
        public DateTime PlayedDeviceTimestamp { get; set; }
        public byte[] RawData { get; set; }
        public int RecipientCount { get; set; }
        public String ParticipantHash { get; set; }
    }

    public enum MediaWhatsappType
    {
        MEDIA_WHATSAPP_TEXT = 0,
        MEDIA_WHATSAPP_IMAGE = 1,
        MEDIA_WHATSAPP_AUDIO = 2,
        MEDIA_WHATSAPP_VIDEO = 3,
        MEDIA_WHATSAPP_CONTACT = 4,
        MEDIA_WHATSAPP_LOCATION = 5
    };
}
