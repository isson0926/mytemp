using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;  

namespace cs_console
{

    class Program
    {
        static void Main(string[] args)
        {        
            try {
                SerialPort sp = new SerialPort();
                sp.PortName = "COM2";
                sp.BaudRate = 9600;  //보레이트 변경이 필요하면 숫자 변경하기
                sp.DataBits = 8;
                sp.StopBits = StopBits.One;
                sp.Parity   = Parity.None;
                //sp.ReadBufferSize = 1024 * 10;
                sp.ReadTimeout = 1000 * 60;
                sp.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);

                sp.Open();
                sp.DiscardInBuffer();

                int cnt = 0;
                for(;;) {
                    string _recvmsg = read(sp);
                    //string _recvmsg = read2(sp);
                    //string _recvmsg = sp.ReadLine();
                    string recvmsg = String.Empty;
                    for(int i = 0; i < _recvmsg.Length; i++) {
                        if(!((int)_recvmsg[i] == 0x2
                          || (int)_recvmsg[i] == 0x3 
                          || (int)_recvmsg[i] == 0xd 
                          || (int)_recvmsg[i] == 0xa)) { 
                            recvmsg += _recvmsg[i];
                        }
                    }

                    //byte [] buffer = new byte[6];
                    //sp.Read(buffer, 0, 6);
                    //string recvmsg = Encoding.Default.GetString(buffer);
                    Console.WriteLine("[" + cnt.ToString("D5") + "]" + " " + "'" + recvmsg + "'");
                    cnt++;
                }
                     
            }
            catch(Exception ex) {
                Console.WriteLine("[error]" + ex.Message);
            }
        }

        static string read2(SerialPort sp) {
            int count = 0;
            var read = new byte[7];
            while (count < read.Length)
            {
                try
                {
                    count += sp.Read(read, count, read.Length - count);
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("read2, timeout exception");
                }
            }
            return Encoding.Default.GetString(read);
        }

        static string read(SerialPort sp) {
            
            List<byte> byteList = new List<byte>();

            for(;;) {
                try {
                    int rb = (byte)sp.ReadByte();
                    if(rb == -1) {
                        Console.WriteLine("end of stream");
                        return String.Empty;
                    }

                    byte b = (byte)rb;
                    //Console.WriteLine("readByte , byte = " + b.ToString());

                    byteList.Add(b); 

                    if (b == 0x3) {
                       byte [] byteArray = new byte[byteList.Count]; 
                       for(int i = 0; i < byteList.Count; i++) {
                           byteArray[i] = byteList[i];
                       }
                       return Encoding.Default.GetString(byteArray);
                    }

                    Thread.Sleep(100);
                }
                catch(Exception) {
                    Console.WriteLine("error!");
                }
            }
        }

        static void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {
            SerialError err = e.EventType;
            string strErr = "";

            switch (err)
            {
                case SerialError.Frame:
                    strErr = "HardWare Framing Error";
                    break;
                case SerialError.Overrun:
                    strErr = "Charaters Buffer Over Run";
                    break;
                case SerialError.RXOver:
                    strErr = "Input Buffer OverFlow";
                    break;
                case SerialError.RXParity:
                    strErr = "Founded Parity Error";
                    break;
                case SerialError.TXFull:
                    strErr = "Write Buffer was Fulled";
                    break;
                default:
                    break;
            }
            Console.WriteLine("error : " + strErr);
        }
    }
}
