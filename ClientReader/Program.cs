using System;
using System.Net.Sockets;
using System.Text;

namespace ClientReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            Encoding win = Encoding.GetEncoding("windows-1251");
            byte[] reader = win.GetBytes("reader");
            TcpClient conversant = null;
            try
            {
                String host = args.Length == 1 ? args[0] : "127.0.0.1";

                conversant = new TcpClient(host, 7);

                NetworkStream ns = conversant.GetStream();
                byte[] myReadBuffer = new byte[1024];
                int numberOfBytesRead;
                do
                {
                    
                    ns.Write(reader, 0, reader.Length);
                    numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);
                    String recievedData = win.GetString(myReadBuffer, 0, numberOfBytesRead);
                    if (recievedData.Equals("null")) { 
                         
                        continue; 
                    }
                    Console.WriteLine("Reply from " + host + ":" + recievedData);
                } while (numberOfBytesRead > 0);
                Console.ReadLine();
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
