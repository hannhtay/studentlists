using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentList
{
    public partial class Form1 : Form
    {
        Student st = new Student();
        string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        string imglocation = "";

        public Form1()
        {
            InitializeComponent();
            showdata();
        }

        public void showdata()
        {
            DataTable dt = st.Students();
            dataGridView1.Font = new Font("Pyidaungsu", 11);
            dataGridView1.AutoGenerateColumns = false;
            
            dataGridView1.DataSource = dt;
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            Clear();

        }
        //Save to database
        private void save_Click(object sender, EventArgs e)
        {
            st.StudentName = studentname.Text;
            st.StudentFather= studentfname.Text;
            st.StudentBOD= studentdob.Value.ToString();
            st.StudentNRC= studentnrc.Text;
            st.StudentClass=studentClass.Text;
            st.StudentRemark= studentremark.Text;
            st.StudentImage = imglocation;
            Console.WriteLine(imglocation);
            if (male.Checked == true)
            {
                 st.StudentGender = 1;
            }
            else
            {
                 st.StudentGender = 0;
            }

            bool success = st.Insert(st);
            if (success == true)
            {
                MessageBox.Show("ကျောင်းသားထည့်ပြီးပါယာ", "အောင်မြင်ပါရေ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.ClearSelection();
                showdata();
                Clear();
            }
            else
            {
                MessageBox.Show("Student add Fail");
            }


        }
        //clear function
        public void Clear()
        {
            studentname.Text = "";
            studentfname.Text = "";
            studentdob.Text = "";
            studentnrc.Text = "";
            studentClass.Text = "";
            studentremark.Text = "";
            male.Checked = true;
            imglabel.Text = "?";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        Image ConvertBinaryToImage(byte[] imdata)
        {

            using (MemoryStream ms = new MemoryStream(imdata))
            {
                return Image.FromStream(ms);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            studentname.Text = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            studentfname.Text = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            studentnrc.Text = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            studentClass.Text = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();
            studentdob.Value = Convert.ToDateTime(dataGridView1.Rows[rowIndex].Cells[8].Value.ToString());
            string gender = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            if (Int32.Parse(gender)== 1)
            {
                male.Checked = true;
            }
            else
            {
                female.Checked = true;
            }

            studentremark.Text = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString();
            studentid.Text = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

            string imgstring = dataGridView1.Rows[rowIndex].Cells[7].Value.ToString();
            Console.WriteLine(imgstring);
            if (!(string.IsNullOrEmpty(imgstring)))
            {
                byte[] a = (byte[])dataGridView1.Rows[rowIndex].Cells[7].Value;

                pictureBox1.Image = ConvertBinaryToImage(a);
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            st.StudentName = studentname.Text;
            st.StudentFather = studentfname.Text;
            st.StudentBOD = studentdob.Value.ToString();
            Console.WriteLine(st.StudentBOD);
            st.StudentNRC = studentnrc.Text;
            st.StudentClass = studentClass.Text;
            st.StudentRemark = studentremark.Text;
            st.StudentId = Int32.Parse(studentid.Text);
            st.StudentImage = imglocation;
            if (male.Checked == true)
            {
                st.StudentGender = 1;
            }
            else
            {
                st.StudentGender = 0;
            }

            bool success = st.Update(st);
            if (success == true)
            {
                MessageBox.Show("ကျောင်းသားထည့်ပြီးပါယာ", "အောင်မြင်ပါရေ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.ClearSelection();
                showdata();
                Clear();
            }
            else
            {
                MessageBox.Show("Student add Fail");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(DialogResult.Yes==MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                st.StudentId = Int32.Parse(studentid.Text);
                bool success = st.destroy(st);
                if (success == true)
                {
                    dataGridView1.ClearSelection();
                    showdata();
                    Clear();
                }
                else
                {
                    MessageBox.Show("Student add Fail");
                }
            }

            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
            string keyword =textBox6.Text;
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Students WHERE name LIKE N'%"+keyword +"%' OR fathername LIKE N'%"+keyword+"' OR class LIKE N'%"+keyword+"%' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        

        private void image_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png file (*.png)|*.png|jpg file (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imglocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imglocation;
                imglabel.Text = imglocation;

            }

        }
    }
}
