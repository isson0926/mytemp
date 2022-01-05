// cpp_serial_test.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//

#include <iostream>
#include <windows.h>
#include <string>
#include <vector>

using namespace std;

void write(HANDLE handle, string msg) {
    DWORD to_write = msg.size();
    DWORD written  = 0;
    if(WriteFile(handle, msg.c_str(), to_write, &written, NULL) == FALSE) 
        throw "write call failed";
    if(to_write != written)
        throw "write result failed, to_write != written.";
}

char read_char(HANDLE handle) { 
    DWORD read = 0;
    char  ch;
    if(ReadFile(handle, &ch, 1, &read, NULL) == FALSE) throw "read call failed";
    if(read != 1) throw "read failed, read count != 1";
    return ch;
}

string read_line(HANDLE handle) {
    string line;
    for(;;) {
        char ch = read_char(handle);
        cout << ch << flush ;
        line += ch;
        if(ch == 0xa) 
            return line;
    }
}

int main(int argc, char **argv) {
    try {
        if(argc != 2) 
            throw "usage : cpp_serial_test.exe COM4";

        string port_name = argv[1];
        string dev_name  = "\\\\.\\" + port_name;
        HANDLE sp        = CreateFile(dev_name.c_str(), GENERIC_READ | GENERIC_WRITE, 0 , NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
        if(sp == INVALID_HANDLE_VALUE) throw "port open failed";

        SetCommMask(sp, EV_RXCHAR);
        SetupComm  (sp, 4096, 4096); 
        PurgeComm  (sp, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);

        DCB dcb;

        memset(&dcb, 0, sizeof(dcb));

        dcb.BaudRate = CBR_9600;
        dcb.Parity   = NOPARITY;
        dcb.ByteSize = 8;
        dcb.StopBits = ONESTOPBIT;

        SetCommState (sp, &dcb);

        for(;;) {
            write(sp, "~HS");
            cout << "write done" << endl;
            string msg1 = read_line(sp);  cout << msg1 ;
            string msg2 = read_line(sp);  cout << msg2 ;
            string msg3 = read_line(sp);  cout << msg3 ;
        }

        CloseHandle(sp);
    }
    catch(const char *ex) {
        cout << ex << endl;
    }
}

