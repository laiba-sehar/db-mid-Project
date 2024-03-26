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
    public partial class Rubric : Form
    {
        bool isUpdateChecked = false;
        int rubricId;
        string rubricDetail;
        int cloId;
        public Rubric()
        {
            InitializeComponent();

        }
        private void ViewRubrics()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM Rubric", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataTable;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            connection.Close();
        }
        private string CheckIfExists(string detail)
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand($"IF(SELECT MAX(1) FROM Rubric WHERE Details='{detail}') > 0 BEGIN SELECT '1' END ELSE BEGIN SELECT '2' END", connection);
            string result = "";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result = (reader.GetString(0));
            }
            reader.Close();
            command.ExecuteNonQuery();
            return result;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.Show();
        }

        private void Rubric_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
            updateColumn.HeaderText = "Update";
            updateColumn.Text = "Update";
            updateColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(updateColumn);
            ViewRubrics();
            isUpdateChecked = false;
            LoadComboBoxData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string result = CheckIfExists(detailstxt.Text);

            if (!isUpdateChecked && result != "1")
            {
                if (detailstxt.Text != String.Empty)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 200);
                    var connection = Configuration.getInstance().getConnection();
                    SqlCommand command = new SqlCommand($"INSERT INTO Rubric VALUES ({randomNumber}, '{detailstxt.Text}', {cloId})", connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    detailstxt.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("Fill the data First");
                }
            }
            else if (isUpdateChecked)
            {
                var connection = Configuration.getInstance().getConnection();
                SqlCommand command = new SqlCommand("UPDATE Rubric SET Details=@Detail, CloId=@CloId WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Detail", detailstxt.Text);
                command.Parameters.AddWithValue("@CloId", cloId);
                command.Parameters.AddWithValue("@Id", rubricId);
                command.ExecuteNonQuery();
                MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isUpdateChecked = false;
                detailstxt.Text = String.Empty;
            }
            else if (result == "1")
            {
                MessageBox.Show("Already Exist");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            detailstxt.Text = String.Empty;
            comboBox1.Text = String.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ViewRubrics();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.ColumnIndex;

            if (index == 0)
            {
                detailstxt.Text = rubricDetail;
                isUpdateChecked = true;
            }
        }
        private void LoadComboBoxData()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            LoadComboName();
            LoadComboId();
        }
        private void LoadComboId()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT Id FROM Clo", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(Convert.ToInt16(reader.GetInt32(0)));
            }
            reader.Close();
            command.ExecuteNonQuery();
        }
        private void LoadComboName()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT Name FROM Clo", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            reader.Close();
            command.ExecuteNonQuery();
            connection.Close();
            connection.Open();
        }
        private void RubricControl_Load(object sender, EventArgs e)
        {
         
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                rubricId = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                rubricDetail = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            catch (Exception exp) { }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rubriclevel rub = new Rubriclevel();
            this.Hide();
            rub.Show();
            /*
            RubricLevelControl newControl = new RubricLevelControl();
            newControl.Dock = DockStyle.Fill;
            this.Parent.Controls.Add(newControl);
            newControl.BringToFront();
            this.Hide();*/
        }

        private void comboBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            cloId = Convert.ToInt32(comboBox2.Items[comboBox1.SelectedIndex].ToString());

        }
    }
}
