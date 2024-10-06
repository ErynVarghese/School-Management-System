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


        public FeeStructure GetById(int Id)
        {
            FeeStructure obj = new FeeStructure();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FeeId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

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


        internal string UpdateById(Class cl)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", cl.ClassId);
                cmd.Parameters.AddWithValue("@TotalFee", cl.ClassFee);

                if (cl.InstallmentNo == 1)
                {
                    cmd.Parameters.AddWithValue("@Installment1", cl.ClassFee);
                    cmd.Parameters.AddWithValue("@Installment2", 0);
                    cmd.Parameters.AddWithValue("@Installment3", 0);
                }
                else if (cl.InstallmentNo == 2)
                {
                    cmd.Parameters.AddWithValue("@Installment1", (cl.ClassFee) / 2);
                    cmd.Parameters.AddWithValue("@Installment2", (cl.ClassFee) / 2);
                    cmd.Parameters.AddWithValue("@Installment3", 0);
                }
                else if (cl.InstallmentNo == 3)
                {
                    cmd.Parameters.AddWithValue("@Installment1", (cl.ClassFee) / 3);
                    cmd.Parameters.AddWithValue("@Installment2", (cl.ClassFee) / 3);
                    cmd.Parameters.AddWithValue("@Installment3", (cl.ClassFee) / 3);
                }
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

        internal string CreateById(Class cl)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", cl.ClassId);
                cmd.Parameters.AddWithValue("@TotalFee", cl.ClassFee);

                if (cl.InstallmentNo == 1)
                {
                    cmd.Parameters.AddWithValue("@Installment1", cl.ClassFee);
                    cmd.Parameters.AddWithValue("@Installment2", 0);
                    cmd.Parameters.AddWithValue("@Installment3", 0);
                }
                else if (cl.InstallmentNo == 2)
                {
                    cmd.Parameters.AddWithValue("@Installment1", (cl.ClassFee) / 2);
                    cmd.Parameters.AddWithValue("@Installment2", (cl.ClassFee) / 2);
                    cmd.Parameters.AddWithValue("@Installment3", 0);
                }
                else if (cl.InstallmentNo == 3)
                {
                    cmd.Parameters.AddWithValue("@Installment1", (cl.ClassFee) / 3);
                    cmd.Parameters.AddWithValue("@Installment2", (cl.ClassFee) / 3);
                    cmd.Parameters.AddWithValue("@Installment3", (cl.ClassFee) / 3);
                }
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

        internal decimal GetInstallation1ByClassId(int classid)
        {
            decimal result = 0;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", classid);
                cmd.Parameters.AddWithValue("@Mode", 8);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    if (!sdr.IsDBNull(0))
                    {
                        result = sdr.GetDecimal(0); // Read the decimal value
                    }
                    else
                    {
                        result = 0;
                    }


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


        internal decimal GetInstallation2ByClassId(int classid)
        {
            decimal result = 0;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", classid);
                cmd.Parameters.AddWithValue("@Mode", 9);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    if (!sdr.IsDBNull(0))
                    {
                        result = sdr.GetDecimal(0); // Read the decimal value
                    }
                    else
                    {
                        result = 0;
                    }


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

        internal decimal GetInstallation3ByClassId(int classid)
        {
            decimal result = 0;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", classid);
                cmd.Parameters.AddWithValue("@Mode", 10);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    if (!sdr.IsDBNull(0))
                    {
                        result = sdr.GetDecimal(0); // Read the decimal value
                    }
                    else
                    {
                        result = 0;
                    }


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