using System;

namespace WAReporter.Modelo
{
    public class Receipt
    {
        //Esses campos correspondem aos existentes na tabela receipts dos bancos msgstore.db

        public int Id { get; set; }
        public String KeyRemoteJid { get; set; }
        public String KeyId { get; set; }
        public String RemoteResource { get; set; }
        public DateTime ReceiptDeviceTimestamp { get; set; }
        public DateTime ReadDeviceTimestamp { get; set; }
        public DateTime PlayedDeviceTimestamp { get; set; }
    }
}
