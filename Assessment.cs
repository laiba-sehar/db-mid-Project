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
    public partial class Assessment : Form
    {
        bool isUpdateMode;
        int assessmentId, totalMarks, totalWeightage;
        string assessmentTitle;
        bool isValidTitle = false; bool isValidMarks = false; bool isValidWeightage = false;

        public Assessment()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1_TextChanged(textBox1, EventArgs.Empty); // Validate Title
            textBox2_TextChanged(textBox2, EventArgs.Empty); // Validate Weightage
            textBox3_TextChanged(textBox3, EventArgs.Empty); // Validate Marks

            string validationResponse = ValidateTitleExistence();
            DateTime creationDate = dateTimePicker1.Value;
            string formattedDate = creationDate.ToString("yyyy-MM-dd HH:mm:ss");

            if (isValidTitle && isValidMarks && isValidWeightage)
            {
                var connection = Configuration.getInstance().getConnection();
                if (!isUpdateMode && validationResponse != "1")
                {
                    SqlCommand insertCommand = new SqlCommand("Insert into Assessment values (@title, @date, @marks, @weightage)", connection);
                    insertCommand.Parameters.AddWithValue("@title", textBox1.Text);
                    insertCommand.Parameters.AddWithValue("@date", formattedDate);
                    insertCommand.Parameters.AddWithValue("@marks", textBox3.Text);
                    insertCommand.Parameters.AddWithValue("@weightage", textBox2.Text);
                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (isUpdateMode)
                {
                    SqlCommand updateCommand = new SqlCommand("Update Assessment Set Title = @title, DateCreated = @date, TotalMarks = @marks, TotalWeightage = @weightage WHERE Id = @ID", connection);
                    updateCommand.Parameters.AddWithValue("@title", textBox1.Text);
                    updateCommand.Parameters.AddWithValue("@date", formattedDate);
                    updateCommand.Parameters.AddWithValue("@marks", textBox3.Text);
                    updateCommand.Parameters.AddWithValue("@weightage", textBox2.Text);
                    updateCommand.Parameters.AddWithValue("@ID", assessmentId);
                    updateCommand.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isUpdateMode = false;
                }
                else
                {
                    MessageBox.Show("Title already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                ResetFormFields();
            }
            else
            {
                MessageBox.Show("Please correct the form errors before proceeding.", "Form Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetFormFields();
        }
        private void ResetFormFields()
        {
            textBox1.Text = String.Empty;
            textBox3.Text = String.Empty;
            textBox2.Text = String.Empty;
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            isValidTitle = false;
            isValidMarks = false;
            isValidWeightage = false;
        }
        private void DisplayAssessmentData()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand fetchCommand = new SqlCommand("Select * from Assessment", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(fetchCommand);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataTable;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private String ValidateTitleExistence()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT CASE WHEN EXISTS (SELECT 1 FROM Assessment WHERE Title = @title) THEN '1' ELSE '2' END", connection);
            command.Parameters.AddWithValue("@title", textBox1.Text);
            string result = command.ExecuteScalar()?.ToString();
            return result ?? "2";
        }

        private void Assessment_Load(object sender, EventArgs e)
        {

        }


        // Event handler for text changes in the title textbox
  /*      private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            isValidTitle = !string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text.All(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch));
            label4.Text = isValidTitle ? "" : "Invalid title. Use only letters, digits, and spaces.";
        }*/

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            isValidTitle = !string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text.All(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch));
            label4.Text = isValidTitle ? "" : "Invalid title. Use only letters, digits, and spaces.";

        }
        private void view()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Assessment", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;



        }

        private void button4_Click(object sender, EventArgs e)
        {
            view();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AssessmentComponent assesscom = new AssessmentComponent();
            this.Hide();
            assesscom.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            isValidWeightage = int.TryParse(textBox2.Text, out int weightage) && weightage > 0;
            label5.Text = isValidWeightage ? "" : "Enter a valid number for weightage.";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            isValidMarks = int.TryParse(textBox3.Text, out int marks) && marks > 0;
            label6.Text = isValidMarks ? "" : "Enter a valid number for marks.";
        }

 
    }
}
