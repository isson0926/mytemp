using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace csharp_WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.C)
            {
                #if DEBUG
                Application.Exit();
                #endif
            }
        }

        public void TestThread() {

            while(true) {
                Test();
                Thread.Sleep(10);
            }

            Console.WriteLine("Thread exit");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            thd = new Thread(new ThreadStart(TestThread));
            thd.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void Test() {

            if (this.InvokeRequired)  {
                var d = new SafeCallDelegate(Test);
                this.Invoke(d);
            }
            else {

            }

            this.Text = "abc";
        }


        private delegate void SafeCallDelegate();
        private Thread thd;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thd.Abort();
        }
    }
}
