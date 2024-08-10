using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputForm
{
    public partial class Form1 : Form
    {
        public string ServerIP { get; private set; }
        public string ServerPort { get; private set; }
        public string YourIP { get; private set; }
        public string YourPort { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        

       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve values from text boxes
            ServerIP = serverIPTextBox.Text;
            ServerPort = serverPortTextBox.Text;
            YourIP = yourIPTextBox.Text;
            YourPort = yourPortTextBox.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
