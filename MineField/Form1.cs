using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineField
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }

        // this menu redirects to relevant pages
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            GameBoard form3 = new GameBoard();
            form3.Show();
            
        }

        private void BtnHowToPlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
            this.Show();
        }
    }
}

