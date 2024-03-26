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
    public partial class Evaluation : Form
    {
        String name;
        int id;
        int ACID;
        string selected_name_index;
        int name_id;
        int mid;


        int mlm;
        public Evaluation()
        {
            InitializeComponent();
            
        }
        public Evaluation(String name, int id)
        {
            InitializeComponent();
            this.name = name;
            this.id = id;
        }

        private void Evaluation_Load(object sender, EventArgs e)
        {
          //  lblass.Text = name;
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand($"select * from AssessmentComponent where AssessmentComponent.AssessmentId={id}", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();
            con2.Open();
            loadMeasurement();
            loadstudents();
            loadstudentID();
            loadMeasurementID();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand($"select * from AssessmentComponent where AssessmentComponent.AssessmentId={id}", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();
            con2.Open();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime1 = DateTime.Now;
            string dateC = selectedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");


            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand($"Insert into StudentResult values ({name_id},{ACID},{mid},@dateC)", con);

            cmd.Parameters.AddWithValue("@dateC", dateC);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Evaluated");
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            ACID = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            MessageBox.Show("Componnent Selected  :)");
            loadMeasurement();
            loadstudents();
            loadstudentID();
            loadMeasurementID();
        }
        private void loadMeasurementID()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand($"  Select  r.id from AssessmentComponent as A join\tRubricLevel as r on A.RubricId=r.RubricId where r.RubricId in (select RubricId from AssessmentComponent where AssessmentId={id}    and AssessmentComponent.Id={ACID}  )", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox4.Items.Add(reader.GetInt32(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
            con.Open();



        }
        private void loadMeasurement()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand($"  Select  MeasurementLevel from AssessmentComponent as A join\tRubricLevel as r on A.RubricId=r.RubricId where r.RubricId in (select RubricId from AssessmentComponent where AssessmentId={id}    and AssessmentComponent.Id={ACID}  )and A.Id={ACID}", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetInt32(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
            con.Open();



        }
        private void loadstudents()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand($" select RegistrationNumber from Student \r\nexcept \r\nselect RegistrationNumber from Student join StudentResult on Student.Id\t= StudentResult.StudentId\t\r\njoin AssessmentComponent on StudentResult.AssessmentComponentId= AssessmentComponent.Id where RubricId in  (select RubricId from AssessmentComponent where AssessmentId={id}   and AssessmentComponent.Id={ACID} )", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
            con.Open();

        }
        private void loadstudentID()
        {

            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand($" select ID from Student \r\nexcept \r\nselect student.Id from Student join StudentResult on Student.Id\t= StudentResult.StudentId\t\r\njoin AssessmentComponent on StudentResult.AssessmentComponentId= AssessmentComponent.Id where RubricId in  (select RubricId from AssessmentComponent where AssessmentId={id}   and AssessmentComponent.Id={ACID} )", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add(reader.GetInt32(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
            con.Open();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            studentResult student = new studentResult();
            this.Hide();
            student.Show();
        }
    }
}
