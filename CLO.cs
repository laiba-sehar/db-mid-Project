using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace MidProject
{
    public partial class CLO : Form
    {
        bool isUpdate = false;
        string cloName;
        int cloId;
        public CLO()
        {
            InitializeComponent();
        }

        private void CLO_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);
            dateTimePicker2.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker2.MaxDate = new DateTime(2024, 12, 31);
            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Update";
            Update.Text = "Update";
            Update.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(Update);

            view();
            isUpdate = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
        }
        private void view()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Clo", con2);
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

                PdfPTable table = new PdfPTable(dgv.Columns.Count);

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
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DateTime createDate = dateTimePicker1.Value;
            string createdDate = createDate.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime updateDate = dateTimePicker2.Value;
            string updatedDate = updateDate.ToString("yyyy-MM-dd HH:mm:ss");
            bool isValidDate = false;
            string dateFormat = "yyyy-MM-dd";
            bool validCreateDate = DateTime.TryParseExact(createdDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out createDate);
            bool validUpdateDate = DateTime.TryParseExact(updatedDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out updateDate);

            if (validCreateDate && validUpdateDate)
            {
                if (updateDate > createDate || updateDate == createDate)
                {
                    isValidDate = true;
                }
            }

            if (!isUpdate)
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into Clo values (@title,@dateC,@dateUI)", con);
                cmd.Parameters.AddWithValue("@title", (textBox1.Text));
                cmd.Parameters.AddWithValue("@dateC", createdDate);
                cmd.Parameters.AddWithValue("@dateUI", createdDate);
                cmd.ExecuteNonQuery();
                MessageBox.Show(" Added  SuccessFully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                textBox1.Text = String.Empty;
            }
            else
            {
                if (isValidDate)
                {
                    var con2 = Configuration.getInstance().getConnection();
                    con2.Open();
                    SqlCommand cmd2 = new SqlCommand("Update Clo Set Name=@title,DateCreated=@dateC,DateUpdated=@dateU where Id=@ID", con2);
                    cmd2.Parameters.AddWithValue("@title", (textBox1.Text));
                    cmd2.Parameters.AddWithValue("@dateC", createdDate);
                    cmd2.Parameters.AddWithValue("@dateU", updatedDate);
                    cmd2.Parameters.AddWithValue("@ID", cloId);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con2.Close();
                    isUpdate = false;
                    textBox1.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("Updated date must be greater than or equal to created date");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportToPDF(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.ColumnIndex;
            {
                if (index == 0)
                {
                    textBox1.Text = cloName;
                    isUpdate = true;
                }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {
                cloId = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                cloName = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch (Exception exp) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            view();
        }
    }
}
