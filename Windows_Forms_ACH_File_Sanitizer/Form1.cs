using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Forms_ACH_File_Sanitizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void browseButton_Click(object sender, EventArgs e)
        {
            DialogResult fileChoice = openFileDialog1.ShowDialog(); // Open file browser.
            if (fileChoice == DialogResult.OK)
            {
                // Sanitize file and display path to user.
                outputLabel.Text = "File output to " + ACHFileFunctions.sanitize_file(openFileDialog1.FileName);
            }
            else
            {
                outputLabel.Text = "An error occured.";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
