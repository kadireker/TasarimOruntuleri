using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonThreadSafeProject
{
    internal class RequestClient :  Client 
    {
        public override async Task Connect()
        {
            await Task.Factory.StartNew(() => IpAddress = GetRandomIpAddress());    
        }
        public string  GetRandomIpAddress()
        {
            string ip = string.Empty;
            var random = new Random();
            Console.WriteLine("Plase wait... Loading Ip Address...");
            Thread.Sleep(1000);
            ip = $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
            Console.WriteLine("Giving Ip Address : {0}", ip);
            return ip;
        }
    }
}
