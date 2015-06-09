using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter.Utilitarios
{
    public static class UtilitariosData
    {
        public static DateTime TimeStampParaDateTime(this Int64 timestampSqlite)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestampSqlite/100).ToLocalTime();
            return dtDateTime;
        }
    }
}
