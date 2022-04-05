using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonThreadSafeProject
{

    internal class Program
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static void Main(string[] args)
        {
            Console.WriteLine("Size of the pool : {0}", RouterManagerPool.Instance.Size);
            Task.WhenAll(
                 MainAsync(args)).Wait();

        }

        static async Task MainAsync(string[] args)
        {
            var clients = new List<Client>();
            int j = 0;
            int i = 0;
            while (j < 20)
            {
                i++;
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("State : " + i);
                Client cc = RouterManagerPool.Instance.RequestObject();
                if (cc != null)
                {
                    Task dd = cc.Connect();
                    dd.Wait();
                    clients.Add(cc);
                    j++;
                   
                }
                else
                {
                    Console.WriteLine("All avaliable clients are acquired. Please wait...");
                    int rndClientListIndexNumber = rnd.Next(clients.Count);
                    await RouterManagerPool.Instance.ReleaseObject(clients[rndClientListIndexNumber]);
                }
            }


            //Parallel.For(0, 10, (i, state) =>
            //{
            //    var obj = RouterManagerPool.Instance.RequestObject();
            //    Thread.Sleep(rnd.Next(0, 100));
            //    if (obj != null)
            //    {
            //        Task dd = obj.Connect();
            //        dd.Wait();
            //        clients.Add(obj);
            //    }
            //    else
            //    {
            //        int rndClientListIndexNumber = rnd.Next(clients.Count);
            //        Task dd = RouterManagerPool.Instance.ReleaseObject(clients[rndClientListIndexNumber]);
            //        clients.RemoveAt(rndClientListIndexNumber);
            //        dd.Wait();
            //    }
            //});


            await Task.CompletedTask;
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Finish");

        }
    }
}
