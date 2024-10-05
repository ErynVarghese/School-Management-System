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
    public class EmployerRepo : ICommon<Employer>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<Employer> GetAll()
        {
            List<Employer> emplist = new List<Employer>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_emp", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {

                    Employer emp = new Employer();

                    emp.EmpId = sdr.GetInt32(0);
                    emp.EmpName = sdr.GetString(1);
                    emp.ContactNo = sdr.GetInt32(2);
                    emp.Email = sdr.GetString(3);
                    emp.DOB = sdr.GetDateTime(4);
                    emp.DeptId = sdr.GetInt32(5);
                    emp.DOJ = sdr.GetDateTime(6);
                    emp.EmpUsername = sdr.GetString(7);
                    emp.EmpPassword = sdr.GetInt32(8);

                    emplist.Add(emp);
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

            return emplist;
        }

        public string Create(Employer obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpName", obj.EmpName);
                cmd.Parameters.AddWithValue("@ContactNo", obj.ContactNo);
                cmd.Parameters.AddWithValue("@Email", obj.Email);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                cmd.Parameters.AddWithValue("@DeptId", obj.DeptId);
                cmd.Parameters.AddWithValue("@DOJ", obj.DOJ);
                cmd.Parameters.AddWithValue("@EmpUsername", obj.EmpUsername);
                cmd.Parameters.AddWithValue("@EmpPassword", obj.EmpPassword);
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

        public string Update(Employer obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", obj.EmpId);
                cmd.Parameters.AddWithValue("@EmpName", obj.EmpName);
                cmd.Parameters.AddWithValue("@ContactNo", obj.ContactNo);
                cmd.Parameters.AddWithValue("@Email", obj.Email);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                cmd.Parameters.AddWithValue("@DeptId", obj.DeptId);
                cmd.Parameters.AddWithValue("@DOJ", obj.DOJ);
                cmd.Parameters.AddWithValue("@EmpUsername", obj.EmpUsername);
                cmd.Parameters.AddWithValue("@EmpPassword", obj.EmpPassword);
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

                cmd = new SqlCommand("proc_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", Id);
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


        public Employer GetById(int Id = 0)
        {
            Employer obj = new Employer();

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    obj.EmpId = sdr.GetInt32(0);
                    obj.EmpName = sdr.GetString(1);
                    obj.ContactNo = sdr.GetInt32(2);
                    obj.Email = sdr.GetString(3);
                    obj.DOB = sdr.GetDateTime(4);
                    obj.DeptId = sdr.GetInt32(5);
                    obj.DOJ = sdr.GetDateTime(6);
                    obj.EmpUsername = sdr.GetString(7);
                    obj.EmpPassword = sdr.GetInt32(8);
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

            return obj;
        }

        public int GetNextId()
        {
            int NextId = 0;

            try
            {
                cmd = new SqlCommand("proc_emp", con);
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
                SqlCommand cmd = new SqlCommand("proc_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpUsername", username);
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

    }

    
}