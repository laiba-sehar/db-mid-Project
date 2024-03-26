using System;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MidProject
{
    public partial class AddRubric : Form
    {
        bool check_update = false;
        string detail;
        int id, rubericID, Measurment;
        List<string> NamesClo = new List<string>();
        List<int> IDS = new List<int>();
        int RubericSID;

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = String.Empty;
            textBox1.Text = String.Empty;
            comboBox1.Text = String.Empty;
        }

        public AddRubric()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            MessageBox.Show(RubericSID.ToString());

            if (check_update == false)
            {
                if (textBox1.Text != String.Empty && textBox2.Text != String.Empty)
                {
                    //Random r = new Random();
                    //int x = r.Next(0, 20);
                    var con = Configuration.getInstance().getConnection();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into RubricLevel values (@Id,@Detail,@measure)", con);
                    cmd.Parameters.AddWithValue("@Detail", textBox1.Text);
                    cmd.Parameters.AddWithValue("@measure", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Id", RubericSID.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(" Added  SuccessFully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                    textBox1.Text = String.Empty;
                    textBox2.Text = String.Empty;

                }
                else { MessageBox.Show("Fill the data First"); }




            }
            else
            if (check_update == true)
            {
                //  if (check_date == true)
                //{
                var con2 = Configuration.getInstance().getConnection();
                con2.Open();
                SqlCommand cmd2 = new SqlCommand("Update RubricLevel Set Details=@Detail,RubricId=@CloID , MeasurementLevel=@measure where Id=@ID", con2);
                cmd2.Parameters.AddWithValue("@Detail", textBox1.Text);
                cmd2.Parameters.AddWithValue("@CloID", RubericSID);
                cmd2.Parameters.AddWithValue("@ID", id);
                cmd2.Parameters.AddWithValue("@measure", textBox2.Text);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con2.Close();
                check_update = false;
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;

                //  }

                // else
                //{
                MessageBox.Show("Updated date must be greater than or equal to created date");
                //}
            }

        }

        private void AddRubric_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Update";
            Update.Text = "Update";
            Update.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(Update);
            view();
            bool check_update = false;

            var con = Configuration.getInstance().getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select  Details  FROM Rubric", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                NamesClo.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
            var con2 = Configuration.getInstance().getConnection();
            con.Open();
            SqlCommand cmd2 = new SqlCommand("Select  id FROM Rubric", con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                IDS.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd2.ExecuteNonQuery();
            con2.Close();
            comboBox1.DataSource = NamesClo;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            ExportToPDF(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.ColumnIndex;
            {

                if (index == 0)
                {
                    textBox1.Text = detail;
                    textBox2.Text = Measurment.ToString();

                    check_update = true;


                }

            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                id = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                rubericID = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                Measurment = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                detail = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

            }
            catch (Exception exp) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RubericSID = IDS[comboBox1.SelectedIndex];
        }

        private void view()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from RubricLevel", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();



        }
        private void ExportToPDF(DataGridView dgv)
        {
            try
            {
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(document, new FileStream("TotaL CLO's.pdf", FileMode.Create));
                document.AddHeader("Header", "Report of  CLo's list");
                document.AddHeader("Date", DateTime.Now.ToString());
                document.Open();
                // Create a table with the same number of columns as the DataGridView
                PdfPTable table = new PdfPTable(dgv.Columns.Count);
                // Add the column headers from the DataGridView to the table
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    table.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Index == dataGridView1.Rows.Count - 1)
                    {
                        continue;

                    }
                    else
                    {
                        try
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {

                                if (cell.Value == null)
                                {
                                    MessageBox.Show("Fill all the columns of table (status) it can not be null");
                                }
                                else
                                {
                                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString()));
                                    table.AddCell(pdfCell);
                                }
                            }
                        }
                        catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }

                    }


                }
                document.Add(table);
                document.Close();
            }
            catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }
            // Close the document
        }

    }
}
