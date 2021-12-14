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
            int cnt = 0;
            while(exit == false) {
                Test("test message " + (cnt++).ToString());
                Thread.Sleep(10);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thd = new Thread(new ThreadStart(TestThread));
            thd.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            exit = true;
        }

        public void Test(string text) {
            if(this.InvokeRequired)  {
                var d = new SafeCallDelegate(Test);
                this.Invoke(d, new object[] { text });
            }
            else {

            }

            this.Text = text;
        }

        bool exit = false;
        private delegate void SafeCallDelegate(string text);
    }
}
