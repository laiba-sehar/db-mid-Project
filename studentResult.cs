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
    public partial class studentResult : Form
    {
        String name;
        int ID;
        public studentResult()
        {
            InitializeComponent();
        }

        private void studentResult_Load(object sender, EventArgs e)
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select *  from Assessment", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;

            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Evaluate";
            Update.Text = "Evaluate";
            Update.UseColumnTextForButtonValue = true;
            DataGridViewButtonColumn Delete = new DataGridViewButtonColumn();
            Delete.HeaderText = "Result";
            Delete.Text = "Result";
            Delete.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(Update);
            dataGridView1.Columns.Add(Delete);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.ColumnIndex;
            {

                if (index == 5)
                {
                    Evaluation newUserControl = new Evaluation(name, ID);
                    newUserControl.Dock = DockStyle.Fill;
                    newUserControl.ShowDialog();
                    this.Close();


                }
                else if (index == 6)
                {
                    Result newUserControl = new Result(ID, name);
                    newUserControl.Dock = DockStyle.Fill;
                    newUserControl.ShowDialog();
                    this.Close();
                }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {
                name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                ID = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());



            }
            catch (Exception exp) { }
        }
    }
}
