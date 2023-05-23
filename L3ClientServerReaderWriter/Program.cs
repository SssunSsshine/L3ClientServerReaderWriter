using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L3ClientServerReaderWriter
{

    public class Program
    {
        static public Mutex mutex = new Mutex(false, "GlobalMutex");

        static private int id = 0;
        static public int GetId() { return id; }
        static public void SetId() { id++; }
        static void Main(string[] args)
        {
            Queue<String> records = new Queue<string>();
            TcpListener server = null;
            try
            {
                int portNumber = 7;
                server = new TcpListener(portNumber);
                server.Start();
                Console.WriteLine("Echo server running on port 7");
                Server es = null;
                for (; ; )
                {
                    es = new Server(server.AcceptTcpClient(), records);
                    Thread serverThread = new Thread(
                        new ThreadStart(es.Conversation));
                    serverThread.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + " " + e.StackTrace);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
