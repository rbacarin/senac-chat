using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace portal.Controllers
{
    public class ChatRoomController : AsyncController
    {
        //
        // GET: /ChatRoom/

        [AsyncTimeout(60000)]
        public void BindAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            Chat.Server.ProcurarMensagensAsync(novasMensagens =>
            {
                AsyncManager.Parameters["response"] = new Chat.Models.ChatResponse()
                {
                    mensagens = novasMensagens
                };
                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult BindCompleted(Chat.Models.ChatResponse response)
        {
            return Json(response);
        }

        [HttpPost]
        public ActionResult post(string usuario, string mensagem)
        {
            Chat.Server.Post(usuario, mensagem);
            return Json(true);
        }
    }
}
