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
    public class FeeCollectionRepo : ICommon<FeeCollection>
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<FeeCollection> GetAll()
        {
            List<FeeCollection> feecollist = new List<FeeCollection>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_feecol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    FeeCollection f = new FeeCollection();
                    f.FeeColId = sdr.GetInt32(0);
                    f.StudentId = sdr.GetInt32(1);
                    f.ClassId = sdr.GetInt32(2);
                    f.Installment1 = sdr.GetString(3);
                    f.Installment2 = sdr.GetString(4);
                    f.Installment3 = sdr.GetString(5);
                    f.FeeStatus = sdr.GetString(6);

                    feecollist.Add(f);
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

            return feecollist;
        }

        public string Create(FeeCollection obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_feecol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", obj.StudentId);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@Installment1", obj.Installment1);
                cmd.Parameters.AddWithValue("@Installment2", obj.Installment2);
                cmd.Parameters.AddWithValue("@Intallment3", obj.Installment3);
                cmd.Parameters.AddWithValue("@FeeStatus", obj.FeeStatus);
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

        public string Update(FeeCollection obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_feecol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeColId", obj.FeeColId);
                cmd.Parameters.AddWithValue("@StudentId", obj.StudentId);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@Installment1", obj.Installment1);
                cmd.Parameters.AddWithValue("@Installment2", obj.Installment2);
                cmd.Parameters.AddWithValue("@Intallment3", obj.Installment3);
                cmd.Parameters.AddWithValue("@FeeStatus", obj.FeeStatus);
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

                cmd = new SqlCommand("proc_feecol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeColId", Id);
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


        public FeeCollection GetById(int Id = 0)
        {
            FeeCollection obj = new FeeCollection();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_feecol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeColId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    obj.FeeColId = sdr.GetInt32(0);
                    obj.StudentId = sdr.GetInt32(1);
                    obj.ClassId = sdr.GetInt32(2);
                    obj.Installment1 = sdr.GetString(3);
                    obj.Installment2 = sdr.GetString(4);
                    obj.Installment3 = sdr.GetString(5);
                    obj.FeeStatus = sdr.GetString(6);
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
    }
}