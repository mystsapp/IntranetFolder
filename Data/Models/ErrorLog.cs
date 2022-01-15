using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class ErrorLog
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public string MaCn { get; set; }
        public DateTime NgayTao { get; set; }
        public string LogFile { get; set; }
    }
}
