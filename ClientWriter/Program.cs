using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientWriter
{
    internal class Program
    {
        static public Mutex mutex = new Mutex(false, "GlobalMutex");
        static void Main(string[] args)
        {
            
            Encoding win = Encoding.GetEncoding("windows-1251");
            byte[] writer = win.GetBytes("writer");
            int id;
            TcpClient conversant = null;
            try
            {
                String host = args.Length == 1 ? args[0] : "127.0.0.1";

                conversant = new TcpClient(host, 7);

                NetworkStream ns = conversant.GetStream();
                byte[] myReadBuffer = new byte[1024];
                int numberOfBytesRead;
                ns.Write(writer, 0, writer.Length);
                numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);
                id = Int32.Parse(win.GetString(myReadBuffer, 0, numberOfBytesRead));
                int i = 0;
                do
                {
                    byte[] bytes = win.GetBytes(id + " recNum " + i);
                    ns.Write(bytes, 0, bytes.Length);
                    i++;
                } while(ns.Read(myReadBuffer, 0, myReadBuffer.Length) > 0);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + " " + ex.StackTrace);
            }
            finally
            {
                if (conversant != null) conversant.Close();
            }
        }
    }
}
