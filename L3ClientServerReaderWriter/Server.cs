using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L3ClientServerReaderWriter
{
    public class Server
    {
        static public Mutex mutex = new Mutex(false, "GlobalMutex");
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
                    str = win.GetString(myReadBuffer, 0, numberOfBytesRead).Trim();
                }

                Console.WriteLine("Closing connection.");
                client.Close();
            }
            catch (IOException se) {
                Console.WriteLine("Closing connection.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Closing connection.");
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
            mutex.WaitOne();
            byte[] mySenderBuffer;
            if (records == null || records.Count == 0) {
                mySenderBuffer = win.GetBytes("Empty");
                client.GetStream().Write(mySenderBuffer, 0, mySenderBuffer.Length);
                return; 
            }
            try
            {
                mySenderBuffer = win.GetBytes(records.Dequeue());
            }
            catch(Exception e)
            {
                mySenderBuffer = win.GetBytes("null");
                client.GetStream().Write(mySenderBuffer, 0, mySenderBuffer.Length);
                return;
            }
            client.GetStream().Write(mySenderBuffer, 0, mySenderBuffer.Length);
            mutex.ReleaseMutex();
        }

        public void AddRecord() {
            byte[] myReadBuffer = new byte[1024];
            byte[] idBytes = win.GetBytes(idWriter.ToString());
            client.GetStream().Write(idBytes, 0, idBytes.Length);
            int numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);
            while (numberOfBytesRead > 0)
            {
                String data = win.GetString(myReadBuffer, 0, numberOfBytesRead).Trim();
                if (data != null)
                {
                    try { records.Enqueue(data); }
                    catch (ArgumentException e) { continue; }
                }

                client.GetStream().Write(idBytes, 0, idBytes.Length);
                numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);
            }
        }
    }
}
