﻿using System;
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
    public partial class mainMenu : Form
    {
        public mainMenu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Insert insert = new Insert();
            this.Hide();
            insert.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Update update = new Update();
            this.Hide();
            update.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Retrieve retrieve = new Retrieve();
            this.Hide();
            retrieve.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Delete delete = new Delete();
            this.Hide();
            delete.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.Show();
        }
    }
}
