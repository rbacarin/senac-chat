using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reactive.Subjects;

namespace Chat
{
    public class Server
    {
        private static Subject<Models.Mensagem> _messages = new Subject<Models.Mensagem>();


        public static void CheckForMessagesAsync(Action<List<Models.Mensagem>> onMessages)
        {
            var queued = ThreadPool.QueueUserWorkItem(new WaitCallback(parm =>
            {
                var msgs = new List<Models.Mensagem>();
                var wait = new AutoResetEvent(false);
                using (var subscriber = _messages.Subscribe(msg =>
                {
                    msgs.Add(msg);
                    wait.Set();
                }))
                {
                    // Wait for the max seconds for a new msg
                    wait.WaitOne(TimeSpan.FromSeconds(60));
                }

                ((Action<List<Models.Mensagem>>)parm)(msgs);
            }), onMessages);

            if (!queued)
                onMessages(new List<Models.Mensagem>());
        }

        public static void Post(string usuario, string mensagem)
        {
            _messages
              .OnNext(new Models.Mensagem
               {
                   usuario = usuario,
                   mensagem = mensagem,
                   datahora = DateTime.Now
               });
        }
    }
}
