using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jarvis1.NewFolder1
{
    public partial class progressBar : UserControl
    {
        int progress;
        public progressBar()
        {
            progress =  0 ;
            InitializeComponent();
        }

        private void progressBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen objPen = new Pen(Color.Gold);
            Rectangle rect1 = new Rectangle(0 - this.Width / 2 + 25, 0 - this.Height / 2 + 25, this.Width - 50, this.Height - 50);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress*3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Gold), rect1, 0, (int)(this.progress*3.6));

            e.Graphics.RotateTransform(-90);
            objPen = new Pen(Color.Red);
            rect1 = new Rectangle(0 - this.Width / 2 + 30, 0 - this.Height / 2 + 30, this.Width - 60, this.Height - 60);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress * 3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Red), rect1, 0, (int)(this.progress * 3.6));

            e.Graphics.RotateTransform(-45);
            objPen = new Pen(Color.Gold);
            rect1 = new Rectangle(0 - this.Width / 2 + 35, 0 - this.Height / 2 + 35, this.Width - 70, this.Height - 70);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress * 3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Gold), rect1, 0, (int)(this.progress * 3.6));

            e.Graphics.RotateTransform(-55);
            objPen = new Pen(Color.Red);
            rect1 = new Rectangle(0 - this.Width / 2 + 40, 0 - this.Height / 2 + 40, this.Width - 80, this.Height - 80);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress * 3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Red), rect1, 0, (int)(this.progress * 3.6));

            e.Graphics.RotateTransform(-70);
            objPen = new Pen(Color.Gold);
            rect1 = new Rectangle(0 - this.Width / 2 + 45, 0 - this.Height / 2 + 45, this.Width - 90, this.Height - 90);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress * 3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Gold), rect1, 0, (int)(this.progress * 3.6));

            e.Graphics.RotateTransform(-45);
            objPen = new Pen(Color.Red);
            rect1 = new Rectangle(0 - this.Width / 2 + 50, 0 - this.Height / 2 + 50, this.Width - 100, this.Height - 100);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress * 3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Red), rect1, 0, (int)(this.progress * 3.6));

            e.Graphics.RotateTransform(-30);
            objPen = new Pen(Color.Gold);
            rect1 = new Rectangle(0 - this.Width / 2 + 55, 0 - this.Height / 2 + 55, this.Width - 110, this.Height - 110);
            e.Graphics.DrawPie(objPen, rect1, 0, (int)(this.progress * 3.6));
            e.Graphics.FillPie(new SolidBrush(Color.Gold), rect1, 0, (int)(this.progress * 3.6));

            e.Graphics.RotateTransform(110);
            objPen = new Pen(Color.Red);
            rect1 = new Rectangle(0 - this.Width / 2 + 60, 0 - this.Height / 2 + 60, this.Width - 120, this.Height - 120);
            e.Graphics.DrawPie(objPen, rect1, 0, 360);
            e.Graphics.FillPie(new SolidBrush(Color.Red), rect1, 0, 360);
            e.Graphics.RotateTransform(225);
            StringFormat ft = new StringFormat();
            ft.LineAlignment = StringAlignment.Center;
            ft.Alignment = StringAlignment.Center;
            //rect1 = new Rectangle(0-this.Width/2 + 42, 0- this.Height/2 + 160 , this.Width-80 , this.Height-80);
            e.Graphics.DrawString(" "+(this.progress + 20).ToString()+ "%", new Font("Arial", 18), new SolidBrush(Color.Gold), rect1, ft);

        }

        public void updateProgress(int progress)
        {
            this.progress = progress;
            this.Invalidate();
        }

        private void progressBar_Load(object sender, EventArgs e)
        {

        }
    }
}
