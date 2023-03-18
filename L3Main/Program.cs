using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            Process server = new Process();
            server.StartInfo.FileName = "D:\\User\\unic\\КМ\\L3ClientServerReaderWriter\\L3ClientServerReaderWriter\\bin\\Debug\\L3ClientServerReaderWriter.exe";
            server.Start();
            int i = 0;
            while (i < 4)
            {
                Process writer = new Process();
                Process reader = new Process();
                reader.StartInfo.FileName = "D:\\User\\unic\\КМ\\L3ClientServerReaderWriter\\ClientReader\\bin\\Debug\\ClientReader.exe";
                writer.StartInfo.FileName = "D:\\User\\unic\\КМ\\L3ClientServerReaderWriter\\ClientWriter\\bin\\Debug\\ClientWriter.exe";
                i++;
                writer.Start();
                reader.Start();
            }

        }
    }
}
