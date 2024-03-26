using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MidProject
{
    public partial class Delete : Form
    {
        public Delete()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Delete Student where RegistrationNumber=@RegistrationNumber", con);
            cmd.Parameters.AddWithValue("@RegistrationNumber", textBox1.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Deleted");
        }
    }
}
