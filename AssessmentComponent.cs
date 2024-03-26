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
using System.Globalization;

namespace MidProject
{
    public partial class AssessmentComponent : Form
    {
        int RubricIdentifier;
        int AssessmentIdentifier;
        bool isDateValid = false;
        string creationDate, updateDate;
        bool isMarkValid = false;
        bool isNameValid = false;
        bool isUpdate = false;
        int componentId;
        string componentName;
        int componentMarks;
        public AssessmentComponent()
        {
            InitializeComponent();
        }

        private void AssessmentComponent_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);
            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Update";
            Update.Text = "Update";
            Update.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(Update);

            view();
            bool isUpdate = false;
            view();

            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();
            LoadAssessmentData();
            LoadRubricData();
            SetDate();

        }
        private void view()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand($"select AssessmentComponent.Id,AssessmentComponent.Name,Rubric.Details,AssessmentComponent.TotalMarks,AssessmentComponent.DateCreated from AssessmentComponent JOIN Rubric ON Rubric.Id = AssessmentComponent.RubricId where AssessmentId = {AssessmentIdentifier}", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;



        }

        private string CheckComponentName(string x)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($"        IF(select max(1) from AssessmentComponent where AssessmentId = {AssessmentIdentifier} and AssessmentComponent.Name='{x}' )>0 BEGIN SELECT '1' END ELSE BEGIN SELECT '2' END", con);
            string z = "";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                z = (reader.GetString(0));
            }
            reader.Close();

            // X=cmd.ExecuteReader().GetString(0);
            cmd.ExecuteNonQuery();
            return z;


        }
        private string CheckMarks(int z)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($"\tdeclare @x as int=(select  sum(AssessmentComponent.TotalMarks) FROM Assessment join AssessmentComponent on AssessmentId=Assessment.Id where AssessmentId={AssessmentIdentifier})\r\n\tdeclare @y as int=(select distinct (Assessment.TotalMarks) FROM Assessment join AssessmentComponent on AssessmentId=Assessment.Id where AssessmentId={AssessmentIdentifier})\r\n\t   IF @x+{z}>@y   BEGIN   SELECT '1' END ELSE BEGIN   SELECT '2' END", con);
            string X = "";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                X = (reader.GetString(0));
            }
            reader.Close();

            // X=cmd.ExecuteReader().GetString(0);
            cmd.ExecuteNonQuery();
            return X;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (textBox1.Text == string.Empty)
            {// check is empty
                label3.Text = "Enter the name";
                isNameValid = false;
            }
            if (textBox1.Text.Any(ch => !char.IsLetter(ch)))
            {//check isSpecialCharactor
                label3.Text = "Allowed characters: a-Z";
                isNameValid = false;
            }
            else { isNameValid = true; }
        }
        private void SetDate()
        {
            dateTimePicker1.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);
            dateTimePicker2.MinDate = new DateTime(2024, 1, 1);
            dateTimePicker2.MaxDate = new DateTime(2024, 12, 31);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetDateOnClick();
            if (textBox3.Text != String.Empty && textBox1.Text != String.Empty)
            {
                string y = CheckMarks(Convert.ToInt32(textBox3.Text));
                string z = CheckComponentName(textBox1.Text);
                if (isDateValid == false && y != "1")
                {

                    if (isUpdate == false && z != "1")
                    {

                        if (textBox3.Text != String.Empty && textBox1.Text != String.Empty)
                        {



                            Random r = new Random();
                            int x = r.Next(0, 20);
                            var con = Configuration.getInstance().getConnection();

                            SqlCommand cmd = new SqlCommand("Insert into AssessmentComponent values (@name,@RId,@marks,@dateC,@dateU,@AssID)", con);
                            cmd.Parameters.AddWithValue("@marks", textBox3.Text);
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@AssID", AssessmentIdentifier.ToString());
                            cmd.Parameters.AddWithValue("@RId", RubricIdentifier.ToString());
                            cmd.Parameters.AddWithValue("@dateC", creationDate);
                            cmd.Parameters.AddWithValue("@dateU", updateDate);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show(" Added  SuccessFully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            con.Close();
                            con.Open();
                            textBox3.Text = String.Empty;
                            textBox1.Text = String.Empty;
                        }
                        else { MessageBox.Show("Fill the data First"); }

                    }
                    else
                    {
                        label1.Visible = true;
                        dateTimePicker2.Visible = true;
                        var con2 = Configuration.getInstance().getConnection();

                        SqlCommand cmd2 = new SqlCommand($"Update AssessmentComponent Set Name= '{textBox1.Text}',RubricId={RubricIdentifier},TotalMarks={textBox3.Text},DateUpdated='{updateDate}',AssessmentId={AssessmentIdentifier} where Id={componentId}", con2);
                        cmd2.Parameters.AddWithValue("@marks", textBox3.Text);
                        cmd2.Parameters.AddWithValue("@name", textBox1.Text);
                        cmd2.Parameters.AddWithValue("@AssID", AssessmentIdentifier.ToString());
                        cmd2.Parameters.AddWithValue("@RId", RubricIdentifier.ToString());
                        cmd2.Parameters.AddWithValue("@dateC", creationDate);
                        cmd2.Parameters.AddWithValue("@dateU", updateDate);
                        cmd2.Parameters.AddWithValue("@ID", componentId);
                        cmd2.ExecuteNonQuery();
                        MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        textBox3.Text = String.Empty;
                        textBox1.Text = String.Empty;
                        isUpdate = false;

                    }
                    if (z == "1") { MessageBox.Show("Already Exist"); }
                }
                else { if (y == "1") MessageBox.Show("Componnent Marks greater than total marks of assessment can nit be added"); }

            }
            else { MessageBox.Show("Fill the data first"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = String.Empty;
            textBox1.Text = String.Empty;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (textBox3.Text == string.Empty)
            {// check is empty
                label4.Text = "Enter the name";
                isMarkValid = false;
            }
            if (textBox3.Text.Any(ch => !char.IsDigit(ch)))
            {//check isSpecialCharactor
                label4.Text = "Allowed characters: 1-9";
                isMarkValid = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssessmentIdentifier = Convert.ToInt32(comboBox3.Items[comboBox1.SelectedIndex].ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            RubricIdentifier = Convert.ToInt32(comboBox4.Items[comboBox2.SelectedIndex].ToString());
        }
        private void LoadRubricDetails()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Details FROM Rubric", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();

        }
        private void LoadRubricIds()
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("Select  id FROM Rubric", con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox4.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd2.ExecuteNonQuery();

      
        }
        private void LoadRubricData()
        {


            comboBox2.Items.Clear();
            comboBox4.Items.Clear();
            LoadRubricDetails();
            //LoadRubricDetails2();
            LoadRubricIds();


        }
        private void LoadAssessmentData()
        {
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            LoadAssessmentDetails();
            LoadAssessmentIds();




        }
        private void LoadAssessmentIds()
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("Select  id FROM Assessment", con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox3.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd2.ExecuteNonQuery();

        }
        private void LoadAssessmentDetails()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Title FROM Assessment", con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.ColumnIndex;
            {

                if (index == 0)
                {
                    textBox1.Text = componentName;
                    textBox3.Text = componentMarks.ToString();


                    isUpdate = true;


                }

            }






        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                componentId = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                componentName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                componentMarks = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

            }
            catch (Exception exp) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            view();
        }

        private void SetDateOnClick()
        {
            DateTime selectedDateTime1 = dateTimePicker1.Value;
            creationDate = selectedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime selectedDateTime2 = dateTimePicker2.Value;
            updateDate = selectedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");

            string dateFormat = "yyyy-MM-dd";
            bool validDate1 = DateTime.TryParseExact(creationDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime1);
            bool validDate2 = DateTime.TryParseExact(updateDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime2);

            if (validDate1 && validDate2)
            {
                if (selectedDateTime2 > selectedDateTime1 || selectedDateTime2 == selectedDateTime1)
                {
                    isDateValid = true;
                }
            }

        }

    }
}
