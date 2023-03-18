using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace L3ClientServerReaderWriter
{
    public class Server
    {
        TcpClient client;
        int idWriter;
        Queue<String> records;
        Encoding win = Encoding.GetEncoding("windows-1251");
        public Server(TcpClient tcpc, Queue<String> q)
        {
            idWriter = Program.GetId();
            client = tcpc;
            records= q;
        }
        public void Conversation()
        {
            try
            {
                
                Console.WriteLine("Connection accepted");
                Program.SetId();
                byte[] myReadBuffer = new byte[1024];

                int numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);
                String str = win.GetString(myReadBuffer, 0, numberOfBytesRead).Trim();
                while (numberOfBytesRead > 0)
                {
                    if (str.Equals("writer"))
                    {   
                        AddRecord();
                    }
                    else if (str.Equals("reader"))
                    {
                        DeleteRecord();
                    }
                    numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);
                }

                Console.WriteLine("Closing connection.");
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e + " " + e.StackTrace);
            }
            finally
            {
                if (client != null) client.Close();
            }
        }
        public int GetId() {
            return idWriter;
        }
        public void DeleteRecord() {
            byte[] mySenderBuffer;
            if (records == null || records.Count == 0) {
                mySenderBuffer = win.GetBytes("Empty");
                client.GetStream().Write(mySenderBuffer, 0, mySenderBuffer.Length);
                return; 
            }
            mySenderBuffer = win.GetBytes(records.Dequeue());
            client.GetStream().Write(mySenderBuffer, 0, mySenderBuffer.Length);
        }
       
        public void AddRecord() {
            byte[] myReadBuffer = new byte[1024];
            byte[] idBytes = win.GetBytes(idWriter.ToString());
            client.GetStream().Write(idBytes, 0, idBytes.Length);
            int numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);
            while (numberOfBytesRead > 0)
            {
                records.Enqueue(win.GetString(myReadBuffer, 0, numberOfBytesRead).Trim());
                client.GetStream().Write(idBytes, 0, idBytes.Length);
                numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);
            }
        }
    }
}
