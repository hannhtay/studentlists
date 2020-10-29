using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentList
{
    class Student
    {
        


        //Getter and Setter
        public string StudentName { get; set; }
        public string StudentFather { get; set; }
        public string StudentBOD { get; set; }
        public string StudentNRC { get; set; }
        public int StudentGender { get; set; }
        public string StudentClass { get; set; }
        public string StudentRemark { get; set; }
        public int StudentId { get; set; }
        public string StudentImage { get; set; }

        //Connect to database
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
       
        //show all students
        public DataTable Students()
        {
            SqlConnection conn = new SqlConnection(myconnstring);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM Students ORDER BY id DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;

        }
        

        //Create Students
        public bool Insert(Student st)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstring);


            try
            {
                //sql query
                string sql = "INSERT INTO Students (name, fathername, dob, nrc, gender, class, remark, created_at, photo) VALUES (@name, @fathername, @dob, @nrc, @gender, @class, @remark, @created_at, @image)";
                //Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Add with Values
                DateTime datetime = DateTime.Now;
                byte[] image = null;
                FileStream strem = new FileStream(StudentImage, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(strem);
                image = brs.ReadBytes((int)strem.Length);
           
                cmd.Parameters.AddWithValue("@name", st.StudentName);
                cmd.Parameters.AddWithValue("@fathername", st.StudentFather);
                cmd.Parameters.AddWithValue("@dob", st.StudentBOD);
                cmd.Parameters.AddWithValue("@nrc", st.StudentNRC);
                cmd.Parameters.AddWithValue("@gender", st.StudentGender);
                cmd.Parameters.AddWithValue("@class", st.StudentClass);
                cmd.Parameters.AddWithValue("@remark", st.StudentRemark);
                cmd.Parameters.AddWithValue("@created_at", datetime);//get current date time
                cmd.Parameters.AddWithValue("@image", image);

                //open connection
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection Success");
                }
                catch (SqlException)
                {
                    return false;
                    Console.WriteLine("Connection Error");
                }
            

                //execute query
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                    Console.WriteLine("Insert Success");
                }
                else
                {
                    isSuccess = false;
                    Console.WriteLine("Insert Data Fail");
                }

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Update Stuents
        public bool Update(Student st)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                string sql = "UPDATE Students SET name=@name, fathername=@fathername, dob=@dob, nrc=@nrc, gender=@gender, class=@class, remark=@remark, photo=@image WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                DateTime datetime = DateTime.Now;
                cmd.Parameters.AddWithValue("@name", st.StudentName);
                cmd.Parameters.AddWithValue("@fathername", st.StudentFather);
                cmd.Parameters.AddWithValue("@dob", st.StudentBOD);
                cmd.Parameters.AddWithValue("@nrc", st.StudentNRC);
                cmd.Parameters.AddWithValue("@gender", st.StudentGender);
                cmd.Parameters.AddWithValue("@class", st.StudentClass);
                cmd.Parameters.AddWithValue("@remark", st.StudentRemark);
                cmd.Parameters.AddWithValue("@created_at", datetime);
                cmd.Parameters.AddWithValue("@id", st.StudentId);

                byte[] image = null;
                FileStream strem = new FileStream(StudentImage, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(strem);
                image = brs.ReadBytes((int)strem.Length);
                cmd.Parameters.AddWithValue("@image", image);


                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }


            }
            catch(Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Delete students
        public bool destroy(Student st)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstring);
            try
            {
                string sql = "DELETE FROM Students WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", st.StudentId);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
               
            }catch(Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }


            return isSuccess;
        }

        
    }
}
