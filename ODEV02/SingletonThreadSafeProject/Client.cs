using System.Diagnostics;
using System.Threading.Tasks;

namespace SingletonThreadSafeProject
{
    public abstract class Client
    {
        public string IpAddress { get; set; }
        public abstract Task Connect();
    }
}
