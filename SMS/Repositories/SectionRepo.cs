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
    public class SectionRepo : ICommon<Section>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public List<Section> GetAll()
        {
            List<Section> sectionlist = new List<Section>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_section", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Section s = new Section();
                    s.SectionId = sdr.GetInt32(0);
                    s.SectionName = sdr.GetString(1);
                    s.ClassId = sdr.GetInt32(2);
                    s.TotalSpace = sdr.GetInt32(3);

                    sectionlist.Add(s);
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

            return sectionlist;
        }

        public string Create(Section obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_section", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionName", obj.SectionName);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@TotalSpace", obj.TotalSpace);
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

        public string Update(Section obj)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_section", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionId", obj.SectionId);
                cmd.Parameters.AddWithValue("@SectionName", obj.SectionName);
                cmd.Parameters.AddWithValue("@ClassId", obj.ClassId);
                cmd.Parameters.AddWithValue("@TotalSpace", obj.TotalSpace);
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

                cmd = new SqlCommand("proc_section", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionId", Id);
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


        public Section GetById(int Id = 0)
        {
            Section obj = new Section();

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_section", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionId", Id);
                cmd.Parameters.AddWithValue("@Mode", 1);

                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    obj.SectionId = sdr.GetInt32(0);
                    obj.SectionName = sdr.GetString(1);
                    obj.ClassId = sdr.GetInt32(2);
                    obj.TotalSpace = sdr.GetInt32(3);
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
                cmd = new SqlCommand("proc_section", con);
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


        internal List<Section> GetSectionsByClassId(int Id)
        {
            List<Section> sList = new List<Section>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_section", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", Id);
                cmd.Parameters.AddWithValue("@Mode", 6);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Section obj = new Section();
                    obj.SectionId = sdr.GetInt32(0);
                    obj.SectionName = sdr.GetString(1);
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

        internal string CheckSectionName(Section section)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                SqlCommand checkCmd = new SqlCommand("proc_section", con);
                checkCmd.CommandType = CommandType.StoredProcedure;
                checkCmd.Parameters.AddWithValue("@SectionName", section.SectionName);
                checkCmd.Parameters.AddWithValue("@ClassId", section.ClassId);
                checkCmd.Parameters.AddWithValue("@SectionId", section.SectionId);
                checkCmd.Parameters.AddWithValue("@mode", 9); ;

                bool sectionExists = false;

                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    sectionExists = reader.GetInt32(0) > 0;
                }

                if (sectionExists)
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

        internal string CheckSectionId( int sectionid)
        {
            string result = string.Empty;

            try
            {
                con.Open();

                SqlCommand checkCmd = new SqlCommand("proc_section", con);
                checkCmd.CommandType = CommandType.StoredProcedure;
                checkCmd.Parameters.AddWithValue("@SectionId", sectionid);
                checkCmd.Parameters.AddWithValue("@mode", 8); ;

                bool sectionExists = false;

                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    sectionExists = reader.GetInt32(0) > 0;
                }

                if (!sectionExists)
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