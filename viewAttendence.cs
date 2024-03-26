using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using System.IO;

namespace MidProject
{
    public partial class viewAttendence : Form
    {
        string selectedDate;
        public viewAttendence()
        {
            InitializeComponent();
        }
        private void LoadDatesComboBox()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("\r\nselect Distinct(AttendanceDate) from ClassAttendance join StudentAttendance on StudentAttendance.AttendanceId=ClassAttendance.Id\r\n", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add((reader.GetSqlDateTime(0)).ToString());
            }
            reader.Close();
            command.ExecuteNonQuery();
        }
        private void LoadStudentAttendance()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand($"  select CONCAT(FirstName,LastName)as NAME,RegistrationNumber,Lookup.Name as STATUS,AttendanceDate\r\nfrom ClassAttendance\r\njoin StudentAttendance\r\non StudentAttendance.AttendanceId=ClassAttendance.Id\r\njoin Student \r\non StudentAttendance.StudentId=Student.Id\r\njoin Lookup\r\non LookupId=StudentAttendance.AttendanceStatus\r\nwhere ClassAttendance.AttendanceDate='{selectedDate}'", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataTable;
            dataGridView1.DefaultCellStyle.ForeColor = Color.DarkBlue;
        }

        private void viewAttendence_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            LoadStudentAttendance();
            LoadDatesComboBox();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDate = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            LoadStudentAttendance();
        }
        private void ExportToPDF(DataGridView dgv, string name, string l, string marks)
        {
            try
            {
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(document, new FileStream(name + ".pdf", FileMode.CreateNew));
                document.Open();
                iTextSharp.text.Font headingFont = FontFactory.GetFont("Times New Roman", 18, iTextSharp.text.Font.BOLD);
                Paragraph heading = new Paragraph(name, headingFont);
                heading.Alignment = Element.ALIGN_CENTER;
                heading.SpacingBefore = 10f;
                heading.SpacingAfter = 10f;

                document.Add(heading);

                LineSeparator line = new LineSeparator();
                document.Add(line);

                iTextSharp.text.Font headingFont2 = FontFactory.GetFont("Times New Roman", 14, iTextSharp.text.Font.BOLD);
                Paragraph heading2 = new Paragraph(marks, headingFont2);
                heading2.Alignment = Element.ALIGN_LEFT;
                heading2.SpacingBefore = 10f;
                heading2.SpacingAfter = 10f;

                document.Add(heading2);



                iTextSharp.text.Font courseFont = FontFactory.GetFont("Times New Roman", 12);
                Paragraph course = new Paragraph(l, courseFont);

                course.Alignment = Element.ALIGN_CENTER;
                course.IndentationLeft = 55f;
                course.SpacingAfter = 20f;
                document.Add(course);

                LineSeparator line2 = new LineSeparator();
                document.Add(line2);



                PdfPTable table = new PdfPTable(dgv.Columns.Count);
                table.WidthPercentage = 100;
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    table.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Index == dataGridView1.Rows.Count)
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
                                    continue;
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
            catch (Exception exp) { }//MessageBox.Show("Fill all the columns of table (status) it can not be null"); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null && comboBox2.Text != String.Empty)
            {
                string dateString = comboBox2.Text;
                DateTime dateonly;
                DateTime.TryParse(dateString, out dateonly);

                string datestring2 = dateonly.ToString("yyyy-MM-dd");
                // DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture); string namex = "Total Result of a " + comboBox2.Text + " in " + LBLX.Text;
                string namx = " Student Attendance Report (" + datestring2 + ")";
                string linex = "Attendance Report of Students on" + comboBox2.Text;
                string date2 = "Attendance Date " + datestring2;

                ExportToPDF(dataGridView1, namx, linex, date2);
                MessageBox.Show("Report Generated");
            }
            else { MessageBox.Show("Select the record first to generate report"); }
        }
    }
}
