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


        public static void ProcurarMensagensAsync(Action<List<Models.Mensagem>> callback)
        {
            var queued = ThreadPool.QueueUserWorkItem(new WaitCallback(_callback =>
            {
                List<Models.Mensagem> msgs = new List<Models.Mensagem>();
                AutoResetEvent wait = new AutoResetEvent(false);
                using (var subscriber = _messages.Subscribe(msg =>
                {
                    msgs.Add(msg);
                    wait.Set();
                }))
                {
                    // Esperar o tempo limite de bind por mensagens, se não, liberar longpool
                    // Ou entao, espera o wait.Set()
                    wait.WaitOne(TimeSpan.FromSeconds(60));
                } 

                ((Action<List<Models.Mensagem>>)_callback)(msgs);
            }), callback);

            // Se por algum motivo nao conseguiu montar a fila, retorna vazio.
            if (!queued)
                callback(new List<Models.Mensagem>());
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
