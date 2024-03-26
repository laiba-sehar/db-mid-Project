using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            classAttendence attendence = new classAttendence();
            this.Hide();
            attendence.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainMenu Main = new mainMenu();
            this.Hide();
            Main.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Assessment assess = new Assessment();
            this.Hide();
            assess.Show();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Assessment assess = new Assessment();
            this.Hide();
            assess.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Assessment assess = new Assessment();
            this.Hide();
            assess.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CLO clo = new CLO();
            this.Hide();
            clo.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rubric rubric = new Rubric();
            this.Hide();
            rubric.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            studentResult stres = new studentResult();
            this.Hide();
            stres.Show();
        }
    }
}
