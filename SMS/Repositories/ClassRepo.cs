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
    public class ClassRepo : ICommon <Class>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<Class> GetAll()
        {
            List<Class> classlist = new List<Class>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Class cl = new Class();
                    cl.ClassId = sdr.GetInt32(0);
                    cl.ClassName = sdr.GetString(1);
                    cl.ClassSize = sdr.GetInt32(2);
                    cl.ClassFee = sdr.GetDecimal(3);
                    cl.InstallmentNo = sdr.GetInt32(4);
                    classlist.Add(cl);
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

            return classlist;
        }

        public string Create(Class obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassName", obj.ClassName);
                cmd.Parameters.AddWithValue("@ClassSize", obj.ClassSize);
                cmd.Parameters.AddWithValue("@ClassFee", obj.ClassFee);
                cmd.Parameters.AddWithValue("@InstallmentNo", obj.InstallmentNo);
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

        public string Update(Class obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@ClassName", obj.ClassName);
                cmd.Parameters.AddWithValue("@ClassSize", obj.ClassSize);
                cmd.Parameters.AddWithValue("@ClassFee", obj.ClassFee);
                cmd.Parameters.AddWithValue("@InstallmentNo", obj.InstallmentNo);
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

                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", Id);
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


        public Class GetById(int Id = 0)
        {
            Class obj = new Class();

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    obj.ClassId = sdr.GetInt32(0);
                    obj.ClassName = sdr.GetString(1);
                    obj.ClassSize = sdr.GetInt32(2);
                    obj.ClassFee = sdr.GetDecimal(3);
                    obj.InstallmentNo = sdr.GetInt32(4);
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
                cmd = new SqlCommand("proc_class", con);
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

        internal string CheckClassName(string classname)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                SqlCommand checkCmd = new SqlCommand("proc_class", con);
                checkCmd.CommandType = CommandType.StoredProcedure;
                checkCmd.Parameters.AddWithValue("@ClassName", classname);
                checkCmd.Parameters.AddWithValue("@mode", 6); ;

                bool classExists = false;

                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    classExists = reader.GetInt32(0) > 0;
                }

                if (classExists)
                {
                    result = "Error";
                }

                else
                {
                    result = "Success";
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

    }
}