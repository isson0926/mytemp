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
                sp.PortName   = "COM3";
                sp.BaudRate   = 9600;  //보레이트 변경이 필요하면 숫자 변경하기
                sp.DataBits   = 8;
                sp.StopBits   = StopBits.One;
                sp.Parity     = Parity.None;
                sp.Handshake  = Handshake.XOnXOff;
                sp.DtrEnable  = true;

                sp.Open();
                sp.DiscardInBuffer();

                int cnt = 0;
                for(;;) {
                    sp.Write("~HS");
                    string recvmsg1       = extractMessage(read(sp));
                    string recvmsg2       = extractMessage(read(sp));
                    string recvmsg3       = extractMessage(read(sp));
                    string printStatusMsg = decodeMessage(recvmsg1, recvmsg2, recvmsg3);

                    Console.WriteLine("[V2] " + "[" + cnt.ToString("D5") + "]" + " " + printStatusMsg  );
                    cnt++;
                }
                     
            }
            catch(Exception ex) {
                Console.WriteLine("[error]" + ex.Message);
            }
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
                }
                catch(Exception) {
                    Console.WriteLine("error!");
                }
            }
        }

        static string extractMessage(string readmsg) {
            string emsg = String.Empty;
            for(int i = 0; i < readmsg.Length; i++) {
                if(!((int)readmsg[i] == 0x2
                  || (int)readmsg[i] == 0x3 
                  || (int)readmsg[i] == 0xd 
                  || (int)readmsg[i] == 0xa)) { 
                    emsg += readmsg[i];
                }
            }
            return emsg;
        }

        static string decodeMessage(string msg1, string msg2, string msg3) {
            string[] msgList1 = msg1.Split(',');
            string[] msgList2 = msg2.Split(',');
            string[] msgList3 = msg3.Split(',');

            bool paperOut  = msgList1[1].Trim() == "1";
            bool ribbonOut = msgList2[3].Trim() == "1";
            bool headUp    = msgList2[2].Trim() == "1";
            bool pause     = msgList1[2].Trim() == "1";

            return paperOut   ? "용지없음"
                 : ribbonOut  ? "리본없음"
                 : headUp     ? "헤더열림"
                 : pause      ? "프린터중지"
                 :              "정상" ;
        }
    }
}
