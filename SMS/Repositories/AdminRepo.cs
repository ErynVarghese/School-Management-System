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
    public class AdminRepo : ICommon<Admin>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<Admin> GetAll()
        {
            List<Admin> adminlist = new List<Admin>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Admin a = new Admin();
                    a.AdminId = sdr.GetInt32(0);
                    a.AdminUsername = sdr.GetString(1);
                    a.AdminName = sdr.GetString(2);
                    a.AdminPassword = sdr.GetInt32(3);

                    adminlist.Add(a);
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

            return adminlist;
        }

        public string Create(Admin obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminUsername", obj.AdminUsername);
                cmd.Parameters.AddWithValue("@AdminName", obj.AdminName);
                cmd.Parameters.AddWithValue("@AdminPassword", obj.AdminPassword);
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

        public string Update(Admin obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminId", obj.AdminId);
                cmd.Parameters.AddWithValue("@AdminUsername", obj.AdminUsername);
                cmd.Parameters.AddWithValue("@AdminName", obj.AdminName);
                cmd.Parameters.AddWithValue("@AdminPassword", obj.AdminPassword);
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

                cmd = new SqlCommand("proc_admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminId", Id);
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


        public Admin GetById(int Id = 0)
        {
            Admin obj = new Admin();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    obj.AdminId = sdr.GetInt32(0);
                    obj.AdminUsername = sdr.GetString(1);
                    obj.AdminName = sdr.GetString(2);
                    obj.AdminPassword = sdr.GetInt32(3);
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
                cmd = new SqlCommand("proc_admin", con);
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
                SqlCommand cmd = new SqlCommand("proc_admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminUsername", username);
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