using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SMS.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public ActionResult ClassList(string searchedname)
        {
            List<Class> classlist = new List<Class>();
            List<ById> enlist = new List<ById>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_class", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", 8);

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
                TempData["Error"] = "Error in processing the request. " + ex.Message;
                con.Close();
            }
            finally
            {
                con.Close();
            }

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 5);

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    ById en = new ById();
                    en.Id = sdr.GetInt32(0);
                    en.Name = sdr.GetString(1);
                    enlist.Add(en);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching class names. " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            ViewBag.ClassList = new SelectList(enlist, "Id", "Name");

            // Filter based on selected employee name


            if (searchedname != null)
            {
                classlist = classlist.Where(cl => cl.ClassId.ToString().Contains(searchedname.ToString())).ToList();

            }


            return View(classlist);
        }

        [HttpGet]
        public ActionResult AddClass(int id = 0)
        {
            Class cl = new Class();

            try
            {
                if (id > 0)
                {

                    con.Open();
                    cmd = new SqlCommand("proc_class", con);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClassId", id);
                    cmd.Parameters.AddWithValue("@Mode", 1);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        cl.ClassId = sdr.GetInt32(0);
                        cl.ClassName = sdr.GetString(1);
                        cl.ClassSize = sdr.GetInt32(2);
                        cl.ClassFee = sdr.GetDecimal(3);
                        cl.InstallmentNo = sdr.GetInt32(4);


                    }

                }
                else
                {
                    con.Open();
                    // Fetch the max Department ID when creating a new department
                    cmd = new SqlCommand("proc_class", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 7);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        ViewBag.MaxClassId = sdr.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
                con.Close();
            }
            finally
            {
                con.Close();
            }

            return View(cl);

        }

        [HttpPost]
        public ActionResult AddClass(Class cl)
        {
            int TheId = 0;

            try
            {
                
                if (ModelState.IsValid)
                {

                
                    if (cl.ClassId > 0)
                    {
                        con.Open();
                       

                        cmd = new SqlCommand("proc_class", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClassId", cl.ClassId);
                        cmd.Parameters.AddWithValue("@ClassName", cl.ClassName);
                        cmd.Parameters.AddWithValue("@ClassSize", cl.ClassSize);
                        cmd.Parameters.AddWithValue("@ClassFee", cl.ClassFee);
                        cmd.Parameters.AddWithValue("@InstallmentNo", cl.InstallmentNo);
                        cmd.Parameters.AddWithValue("@Mode", 2);

                        int status = cmd.ExecuteNonQuery();
                        con.Close();
                        if (status < 0)
                        {
                            TempData["Success"] = "Class details edited successfully";
                            //return RedirectToAction("ClassList", "Class");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to edit!";
                        }


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
                        } else if (cl.InstallmentNo == 2)
                        {
                            cmd.Parameters.AddWithValue("@Installment1", (cl.ClassFee)/2);
                            cmd.Parameters.AddWithValue("@Installment2", (cl.ClassFee)/2);
                            cmd.Parameters.AddWithValue("@Installment3", 0);
                        } else if (cl.InstallmentNo == 3)
                         {
                            cmd.Parameters.AddWithValue("@Installment1", (cl.ClassFee)/3);
                            cmd.Parameters.AddWithValue("@Installment2", (cl.ClassFee)/3);
                            cmd.Parameters.AddWithValue("@Installment3", (cl.ClassFee)/3);
                        }
                        cmd.Parameters.AddWithValue("@Mode", 4);

                        int status2 = cmd.ExecuteNonQuery();
                        con.Close();
                        if (status2 < 0)
                        {
                            TempData["Success"] = "Fee details edited successfully";
                            return RedirectToAction("ClassList", "Class");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to edit!";
                        }


                    }
                    else
                    {

                        con.Open();

                        SqlCommand checkCmd = new SqlCommand("proc_class", con);
                        checkCmd.CommandType = CommandType.StoredProcedure;
                        checkCmd.Parameters.AddWithValue("@ClassName", cl.ClassName);
                        checkCmd.Parameters.AddWithValue("@mode", 6);

                        SqlDataReader reader = checkCmd.ExecuteReader();

                        bool classExists = false;
                        if (reader.Read())
                        {
                            classExists = reader.GetInt32(0) > 0; 
                        }

                        con.Close(); 

                        if (classExists)
                        {
                            TempData["Error"] = "This class name already exists!";
                            return View(cl);
                        }

                        con.Open();
                     
                        cmd = new SqlCommand("proc_class", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClassName", cl.ClassName);
                        cmd.Parameters.AddWithValue("@ClassSize", cl.ClassSize);
                        cmd.Parameters.AddWithValue("@ClassFee", cl.ClassFee);
                        cmd.Parameters.AddWithValue("@InstallmentNo", cl.InstallmentNo);

                        cmd.Parameters.AddWithValue("@Mode", 3);

                        int status = cmd.ExecuteNonQuery();

                        con.Close();

                        if (status < 0)
                        {
                            TempData["Success"] = "Class details created successfully";
                            //return RedirectToAction("ClassList", "Class");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to create!";
                        }



                        con.Open();
                        // Fetch the max Department ID when creating a new department
                        cmd = new SqlCommand("proc_class", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 7);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            TheId = sdr.GetInt32(0);
                        }
                        con.Close();

                        //add to the fee table
                        con.Open();


                        cmd = new SqlCommand("proc_fee", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClassId", TheId - 1);
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
                        cmd.Parameters.AddWithValue("@Mode", 5);

                        int status2 = cmd.ExecuteNonQuery();
                        con.Close();
                        if (status2 < 0)
                        {
                            TempData["Success"] = "Fee details created successfully";
                            return RedirectToAction("ClassList", "Class");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to edit!";
                        }

                    }

                }
                else
                {
                    TempData["Error"] = "Fill out all the details.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
            }
            finally
            {
               
            }

            return View(cl);
        }




        public ActionResult DeleteClass(int id)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("proc_class", con);


                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ClassId", id);
                cmd.Parameters.AddWithValue("@Mode", 4);

                int status = cmd.ExecuteNonQuery();

                if (status < 0)
                {
                    TempData["Success"] = "Record deleted successfully";

                }
                else
                {
                    TempData["Error"] = "Failed to delete!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in Processing the request. " + ex.Message;
                con.Close();
            }
            finally
            {
                con.Close();
            }

            return RedirectToAction("ClassList", "Class");

        }

    }
}