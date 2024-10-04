﻿using System;
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
    public class FeeStructureRepo : ICommon<FeeStructure>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<FeeStructure> GetAll()
        {
            List<FeeStructure> feestructlist = new List<FeeStructure>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    FeeStructure f = new FeeStructure();
                    f.FeeId = sdr.GetInt32(0);
                    f.ClassId = sdr.GetInt32(1);
                    f.TotalFee = sdr.GetDecimal(2);
                    f.Installment1 = sdr.GetDecimal(3);
                    f.Installment2 = sdr.GetDecimal(4);
                    f.Installment3 = sdr.GetDecimal(5);
                    feestructlist.Add(f);
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

            return feestructlist;
        }

        public string Create(FeeStructure obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@TotalFee", obj.TotalFee);
                cmd.Parameters.AddWithValue("@Installment1", obj.Installment1);
                cmd.Parameters.AddWithValue("@Installment2", obj.Installment2);
                cmd.Parameters.AddWithValue("@Installment3", obj.Installment3);
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

        public string Update(FeeStructure obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeId", obj.FeeId);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@TotalFee", obj.TotalFee);
                cmd.Parameters.AddWithValue("@Installment1", obj.Installment1);
                cmd.Parameters.AddWithValue("@Installment2", obj.Installment2);
                cmd.Parameters.AddWithValue("@Installment3", obj.Installment3);
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

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeId", Id);
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


        public FeeStructure GetById(int Id = 0)
        {
            FeeStructure obj = new FeeStructure();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeId", Id);
                cmd.Parameters.AddWithValue("@Mode", 2);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    obj.FeeId = sdr.GetInt32(0);
                    obj.ClassId = sdr.GetInt32(1);
                    obj.TotalFee = sdr.GetDecimal(2);
                    obj.Installment1 = sdr.GetDecimal(3);
                    obj.Installment2 = sdr.GetDecimal(4);
                    obj.Installment3 = sdr.GetDecimal(5);
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
                cmd = new SqlCommand("proc_fee", con);
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


        internal decimal GetClassFeeById(int Id)
        {
            decimal fee = 0;

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", Id);
                cmd.Parameters.AddWithValue("@Mode", 6);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                   fee = sdr.GetDecimal(0);
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

            return fee;
        }
    }
}