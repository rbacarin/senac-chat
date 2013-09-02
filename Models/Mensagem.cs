using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Mensagem
    {
        public DateTime datahora { get; set; }
        public string mensagem { get; set; }
        public string usuario { get; set; }
    }
}