using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class ChatResponse
    {
        public List<Mensagem> mensagens { get; set; }
    }
}