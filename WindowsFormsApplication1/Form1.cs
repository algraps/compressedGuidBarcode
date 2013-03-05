using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //GENERATE A BARCODE
            pictureBox1.Image=  GenerateBarCode(Guid.Parse("0cd395e1-f09f-432d-8c64-5d246abdd144"));
        }

       

        public static Image GenerateBarCode(Guid guid)
        {
            return GenCode128.Code128Rendering.MakeBarcodeImage(BarCode.GUIDToCompressedString128(guid), 1, false);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            printDocument1.OriginAtMargins = true;
            printDocument1.DocumentName = "TEST IMAGE PRINTING";

            printDialog1.Document = printDocument1;
            printDialog1.ShowDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();

        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = BarCode.CompressedStringToGUID128(textBox1.Text).ToString();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    textBox1.Text = BarCode.CompressedStringToGUID128(textBox1.Text).ToString();
            //}
        }
        
    }
}
