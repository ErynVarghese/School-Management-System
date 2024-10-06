using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SMS.Repositories
{

    public class StudentRepo : ICommon<Student>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<Student> GetAll()
        {
            List<Student> studlist = new List<Student>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {

                    Student stud = new Student();

                    stud.StudentId = sdr.GetInt32(0);
                    stud.StudentName = sdr.GetString(1);
                    stud.DOB = sdr.GetDateTime(2);
                    stud.ClassId = sdr.GetInt32(3);
                    stud.SectionId = sdr.GetInt32(4);
                    stud.FatherName = sdr.GetString(5);
                    stud.ContactNo = sdr.GetInt32(6);
                    stud.StudentAddress = sdr.GetString(7);
                    stud.StudentUsername = sdr.GetString(8);
                    stud.StudentPassword = sdr.GetInt32(9);
                    stud.StudentFee = sdr.GetDecimal(10);

                    studlist.Add(stud);
                }
            }
            catch (Exception ex)
            { 
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return studlist;
        }

        public string Create (Student obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentName", obj.StudentName);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@SectionId", obj.SectionId);
                cmd.Parameters.AddWithValue("@FatherName", obj.FatherName);
                cmd.Parameters.AddWithValue("@ContactNo", obj.ContactNo);
                cmd.Parameters.AddWithValue("@StudentAddress", obj.StudentAddress);
                cmd.Parameters.AddWithValue("@StudentUsername", obj.StudentUsername);
                cmd.Parameters.AddWithValue("@StudentPassword", obj.StudentPassword);
                cmd.Parameters.AddWithValue("@StudentFee", obj.StudentFee);
                cmd.Parameters.AddWithValue("@Mode", 2);
                
                int status = cmd.ExecuteNonQuery();
                
                if (status <= 0)
                {
                    result = "Success";
                }
                
                else
                {
                    result = "Fail";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string Update (Student obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", obj.StudentId);
                cmd.Parameters.AddWithValue("@StudentName", obj.StudentName);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@SectionId", obj.SectionId);
                cmd.Parameters.AddWithValue("@FatherName", obj.FatherName);
                cmd.Parameters.AddWithValue("@ContactNo", obj.ContactNo);
                cmd.Parameters.AddWithValue("@StudentAddress", obj.StudentAddress);
                cmd.Parameters.AddWithValue("@StudentUsername", obj.StudentUsername);
                cmd.Parameters.AddWithValue("@StudentPassword", obj.StudentPassword);
                cmd.Parameters.AddWithValue("@StudentFee", obj.StudentFee);
                cmd.Parameters.AddWithValue("@Mode", 3);

                int status = cmd.ExecuteNonQuery();

                if (status <= 0)
                {
                    result = "Success";
                }

                else
                {
                    result = "Fail";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return result;

        }


        public string Delete(int Id)
        {
            string result = string.Empty;

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);


                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@StudentId", Id);
                cmd.Parameters.AddWithValue("@Mode", 4);

                int status = cmd.ExecuteNonQuery();

                if (status <= 0)
                {
                    result = "Success";
                }

                else
                {
                    result = "Fail";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return result;

        }


        public Student GetById(int Id = 0)
        {
            Student obj = new Student();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    obj.StudentId = sdr.GetInt32(0);
                    obj.StudentName = sdr.GetString(1);
                    obj.DOB = sdr.GetDateTime(2);
                    obj.ClassId = sdr.GetInt32(3);
                    obj.SectionId = sdr.GetInt32(4);
                    obj.FatherName = sdr.GetString(5);
                    obj.ContactNo = sdr.GetInt32(6);
                    obj.StudentAddress = sdr.GetString(7);
                    obj.StudentUsername = sdr.GetString(8);
                    obj.StudentPassword = sdr.GetInt32(9);
                    obj.StudentFee = sdr.GetDecimal(10);
                }
            } 
            catch(Exception ex) 
            {
               con.Close();
            throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return obj;
        }


        public int GetNextId()
        {
            int NextId = 0;

            try
            {
                cmd = new SqlCommand("proc_student", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 5);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    NextId = sdr.GetInt32(0);
                }
            }
            catch (Exception ex)
            {

                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return NextId;
        }

        internal DataSet GetByUsername(string username)
        {
            DataSet ds = new DataSet();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentUsername", username);
                cmd.Parameters.AddWithValue("@Mode", 6);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(ds);

            }

            catch (Exception ex)
            {

                con.Close();
                throw ex.InnerException;
            }

            finally
            {
                con.Close();
            }

            return ds;
        }


        internal string StudentExists(int studid)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                SqlCommand checkCmd = new SqlCommand("proc_student", con);
                checkCmd.CommandType = CommandType.StoredProcedure;
                checkCmd.Parameters.AddWithValue("@StudentId", studid);
                checkCmd.Parameters.AddWithValue("@mode", 7); ;

                bool studExists = false;

                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    studExists = reader.GetInt32(0) > 0;
                }

                if (studExists)
                {
                    result = "Exists";
                }

                else
                {
                    result = "NotExists";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return result;

        }

        internal List<Student> GetStudentsByClassId(int Id)
        {
            List<Student> sList = new List<Student>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", Id);
                cmd.Parameters.AddWithValue("@Mode", 8);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Student obj = new Student();
                    obj.StudentId = sdr.GetInt32(0);
                    obj.StudentName = sdr.GetString(1);
                    sList.Add(obj);
                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }
            return sList;
        }


        public string AddImage(int studid, string filename)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", studid);
                cmd.Parameters.AddWithValue("@ImageName", filename);
                cmd.Parameters.AddWithValue("@Mode", 9);

                int status = cmd.ExecuteNonQuery();

                if (status <= 0)
                {
                    result = "Success";
                }

                else
                {
                    result = "Fail";
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return result;

        }

        internal string GetStudentImage(int studid)
        {
            string image = string.Empty;

            try
            {
                con.Open();

                SqlCommand checkCmd = new SqlCommand("proc_student", con);
                checkCmd.CommandType = CommandType.StoredProcedure;
                checkCmd.Parameters.AddWithValue("@StudentId", studid);
                checkCmd.Parameters.AddWithValue("@mode", 10); ;

        

                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    image = reader.GetString(0);

                }
               

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex.InnerException;
            }
            finally
            {
                con.Close();
            }

            return image;

        }

    }
}