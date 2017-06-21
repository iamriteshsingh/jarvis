using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jarvis1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
           InitializeComponent();
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            
            
            Task.Run(() =>
                {
                    for(int i=1;i<=80;i++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.progressUpgrade)).Start(i);
                        
                        System.Threading.Thread.Sleep(40);
                        
                    }
                });
           
        }
        public void progressUpgrade(object progress)
        {
            progressBar1.Invoke((MethodInvoker)delegate{progressBar1.updateProgress(Convert.ToInt32(progress)); });
        }

        
    }
}
