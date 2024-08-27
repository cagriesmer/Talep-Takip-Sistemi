using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalepTakip.Models
{
    internal class Request
    {
        public string UserName { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqDescription { get; set; }
        public string ReqTitle { get; set; }
        public string State { get; set; }
        public string ReqId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public string CompDate { get; set; }

        public Request() { }
    }
}
