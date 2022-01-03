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
        static void Main(string[] args)
        {
            try {
                SerialPort sp = new SerialPort();
                sp.PortName = "COM1";
                sp.BaudRate = 9600;  //보레이트 변경이 필요하면 숫자 변경하기
                sp.DataBits = 8;
                sp.StopBits = StopBits.One;
                sp.Parity   = Parity.None;

                sp.Open();
                     
                sp.DiscardOutBuffer();
                for(;;) {
                    sp.Write(new byte[] {0x2}, 0, 1);
                    sp.Write("abc");
                    Thread.Sleep(1000);
                    sp.Write(new byte[] {0x3}, 0, 1);
                    sp.Write(new byte[] {0xd}, 0, 1);
                    sp.Write(new byte[] {0xa}, 0, 1);
                }

                sp.Close();
            }
            catch(Exception ex) {
                Console.WriteLine("[error]" + ex.Message);
            }
        }
    }
}
