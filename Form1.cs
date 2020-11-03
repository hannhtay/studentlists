using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        Bitmap default_image;

        //Page size for pagination
        private int pageSize = 10;
        private int currentPageIndex = 1;
        private int totalPage = 0;
        

        public Form1()
        {
            InitializeComponent();
            showdata();
            default_image = new Bitmap(pictureBox1.Image);
            CalculateTotalPages();
            //Disable if no need nex and previous button
            if (currentPageIndex == 1)
            {
                prevButton.Enabled = false;
            }
            if (totalPage == 1)
            {
                nextButton.Enabled = false;
            }
            pageCount.Text = totalPage.ToString();
        }

        public void showdata()
        {
            DataTable dt = st.Students(pageSize, currentPageIndex);
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
            if (isValidate())
            {
                st.StudentName = studentname.Text;
                st.StudentFather = studentfname.Text;
                st.StudentBOD = studentdob.Value.ToString();
                st.StudentNRC = studentnrc.Text;
                st.StudentClass = studentClass.Text;
                st.StudentRemark = studentremark.Text;
                st.StudentImage = imglocation;
                Console.WriteLine(imglocation);
                if (male.Checked == true)
                {
                    st.StudentGender = "ကျား";
                }
                else
                {
                    st.StudentGender = "မ";
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
            else
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            studentid.Text = "";
            male.Checked = true;
            imglabel.Text = "?";
            pictureBox1.Image = default_image;
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
            if (gender == "ကျား")
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
            if (isValidate())
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
                    st.StudentGender = "ကျား";
                }
                else
                {
                    st.StudentGender = "မ";
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
            else
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(studentid.Text))
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
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
            else
            {
                MessageBox.Show("Student no found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
            string keyword =textBox6.Text;
            if (String.IsNullOrEmpty(keyword))
            {
                showdata();
            }
            else
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Students WHERE name LIKE N'%" + keyword + "%' OR fathername LIKE N'%" + keyword + "' OR class LIKE N'%" + keyword + "%' ", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }

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

        //User input validation
        public bool isValidate()
        {
            bool isPassed = false;
            if ( (!String.IsNullOrEmpty(studentname.Text)) && (!String.IsNullOrEmpty(studentfname.Text)) && (!String.IsNullOrEmpty(studentnrc.Text)) && (!String.IsNullOrEmpty(studentfname.Text)) && (!String.IsNullOrEmpty(studentClass.Text)) && (!String.IsNullOrEmpty(studentremark.Text)))
            {

                isPassed = true;
            }

            return isPassed;
          
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (currentPageIndex < totalPage)
            {
                currentPageIndex++;
                dataGridView1.ClearSelection();
                DataTable dt = st.Students(pageSize, currentPageIndex);
                dataGridView1.Font = new Font("Pyidaungsu", 11);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dt;
                if (currentPageIndex > 1)
                {
                    prevButton.Enabled = true;
                }
                else
                {
                    prevButton.Enabled = false;
                }
            }
            currentPage.Text = currentPageIndex.ToString();
            if (currentPageIndex == totalPage)
            {
                nextButton.Enabled = false;
            }

        }

        //Calculate Total Pages
        private void CalculateTotalPages()
        {
            SqlConnection conn = new SqlConnection(myconnstring);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM Students", conn);
                Int32 count = (Int32)comm.ExecuteScalar();
                totalPage = count / pageSize;
                if(count%pageSize > 0)
                {
                    totalPage += 1;
                }
                
            }
            finally
            {
                conn.Close();
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            if (currentPageIndex > 1)
            {
                currentPageIndex--;
                dataGridView1.ClearSelection();
                DataTable dt = st.Students(pageSize, currentPageIndex);
                dataGridView1.Font = new Font("Pyidaungsu", 11);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dt;
                if (currentPageIndex <= 1)
                {
                    prevButton.Enabled = false;
                }
                if (currentPageIndex < totalPage)
                {
                    nextButton.Enabled = true;
                }
                currentPage.Text = currentPageIndex.ToString();

            }
        }
        Bitmap bm;

        private void printButton_Click(object sender, EventArgs e)
        {
            int height = dataGridView1.Height;
            dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 2;
             bm = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
            dataGridView1.Height = height;
            printPreviewDialog1.ShowDialog();
            

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bm, Brushes.Black, new Point(10, 10));
        }
    }
}
