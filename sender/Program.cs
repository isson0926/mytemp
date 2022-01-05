using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;  

namespace sender
{
    class Program
    {
        static string ReadSerialPort(SerialPort sp, int len) {
            int count = 0;
            var read  = new byte[len];
            while(count < read.Length) {
                try {
                    count += sp.Read(read, count, read.Length - count);
                }
                catch(TimeoutException) {
                    Console.WriteLine("ReadSerialPort(), timeout exception");
                }
            }
            return Encoding.UTF8.GetString(read);
        }                                       

        static void Main(string[] args)
        {
            SerialPort sp = new SerialPort();
            try {
                sp.PortName = "COM1";
                sp.BaudRate = 9600;  //보레이트 변경이 필요하면 숫자 변경하기
                sp.DataBits = 8;
                sp.StopBits = StopBits.One;
                sp.Parity   = Parity.None;

                sp.Open();
                     
                sp.DiscardOutBuffer();
                for(;;) {
                    string readmsg = ReadSerialPort(sp, 3);
                    Console.WriteLine("read : " + readmsg);
                    if(readmsg != "~HS") {
                        Console.WriteLine("invalid command : " + readmsg);
                    }
                    else {
                        sp.Write(new byte[] {0x2}, 0, 1);
                        sp.Write("test message 1");
                        sp.Write(new byte[] {0x3}, 0, 1);
                        sp.Write(new byte[] {0xd}, 0, 1);
                        sp.Write(new byte[] {0xa}, 0, 1);

                        sp.Write(new byte[] {0x2}, 0, 1);
                        sp.Write("test message 2");
                        sp.Write(new byte[] {0x3}, 0, 1);
                        sp.Write(new byte[] {0xd}, 0, 1);
                        sp.Write(new byte[] {0xa}, 0, 1);

                        sp.Write(new byte[] {0x2}, 0, 1);
                        sp.Write("test message 3");
                        sp.Write(new byte[] {0x3}, 0, 1);
                        sp.Write(new byte[] {0xd}, 0, 1);
                        sp.Write(new byte[] {0xa}, 0, 1);

                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine("[error]" + ex.Message);
            }
            finally {
                sp.Close();
            }
        }
    }
}
