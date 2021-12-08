using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskExceptionHandleSample
{
    public partial class Form1 : Form
    {
        bool Running,error;
        
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Running = true;
            var task = new Task(() => TestMetod("Message"));
            // Task içerisinde bir hata olursa hangi metod çağırılacak ise onu burada belirtiyoruz. OnlyOnFaulted olarak taskta hata alınırsa diye ikinci parametre olarak belirtiyorız.
            task.ContinueWith(t => { ErrorHandleAction(); },
                TaskContinuationOptions.OnlyOnFaulted);
            //İşlem başarılı bir şekilde sonlandığında herhangi bir işlem yapılması isteniliyorsa bu metodda belirtiyoruz. OnlyOnRanToCompletion olarak takstan sonra bunu çalıştır.
            task.ContinueWith(t => { MessageBox.Show("İşlem Başarılı bir şekilde sonlandırıldı."); },
                TaskContinuationOptions.OnlyOnRanToCompletion);

            task.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            error = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Running = false;
        }

        private void TestMetod(string message)
        {
            while (Running)
            {
                if (!error)
                {
                    Thread.Sleep(500);
                    listBox1.Items.Add(message);
                }
                else
                {
                    error = false;
                    throw new Exception();
                }
            }
        }
        public void ErrorHandleAction()
        {
            //PLC durdur yada başka ne işlem yapılmak istenilirse burada yap!
            MessageBox.Show("Hata Meydana Geldi");
        }
    }
}
