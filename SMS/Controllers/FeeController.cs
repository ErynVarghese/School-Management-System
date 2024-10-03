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
    public class FeeController : Controller
    {
        // GET: Fee

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
            SqlCommand cmd = null;

            public ActionResult FeeList(string searchedname)
            {
                List<FeeStructure> feelist = new List<FeeStructure>();
                List<ById> enlist = new List<ById>();

                try
                {
                    con.Open();
                    cmd = new SqlCommand("proc_fee", con);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 1);

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

                        feelist.Add(f);
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
                    feelist = feelist.Where(cl => cl.ClassId.ToString().Contains(searchedname.ToString())).ToList();

                }


                return View(feelist);
            }

            [HttpGet]
            public ActionResult AddFee(int id = 0)
            {
                FeeStructure f = new FeeStructure();

                try
                {

                con.Open();
                List<Class> classList = new List<Class>();

                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", 5);

                SqlDataReader classReader = cmd.ExecuteReader();

                while (classReader.Read())
                {
                    classList.Add(new Class
                    {
                        ClassId = classReader.GetInt32(0),
                        ClassName = classReader.GetString(1)
                    });
                }

                con.Close();
                ViewBag.ClassList = new SelectList(classList, "ClassId", "ClassName");


                if (id > 0)
                    {

                        con.Open();
                        cmd = new SqlCommand("proc_fee", con);


                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@FeeId", id);
                        cmd.Parameters.AddWithValue("@Mode", 2);

                        SqlDataReader sdr = cmd.ExecuteReader();

                        while (sdr.Read())
                        {
                       
                        f.FeeId = sdr.GetInt32(0);
                        f.ClassId = sdr.GetInt32(1);
                        f.TotalFee = sdr.GetDecimal(2);
                        f.Installment1 = sdr.GetDecimal(3);
                        f.Installment2 = sdr.GetDecimal(4);
                        f.Installment3 = sdr.GetDecimal(5);

                    }

                    }
                    else
                    {
                        con.Open();
                        // Fetch the max fee ID when creating a new department
                        cmd = new SqlCommand("proc_fee", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 3);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            ViewBag.MaxFeeId = sdr.GetInt32(0);
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

                return View(f);

            }

            [HttpPost]
            public ActionResult AddFee(FeeStructure f)
            {
                try
                {
                con.Open();
                // Repopulate Class List for DropDownList
                List<Class> classList = new List<Class>();
                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", 8);

                SqlDataReader classReader = cmd.ExecuteReader();
                while (classReader.Read())
                {
                    classList.Add(new Class
                    {
                        ClassId = classReader.GetInt32(0),
                        ClassName = classReader.GetString(1)
                    });
                }

                con.Close();
                ViewBag.ClassList = new SelectList(classList, "ClassId", "ClassName");



                con.Open();
                    if (ModelState.IsValid)
                    {

                        if (f.FeeId > 0)
                        {

                            cmd = new SqlCommand("proc_fee", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@FeeId", f.FeeId);
                            cmd.Parameters.AddWithValue("@ClassId", f.ClassId);
                            cmd.Parameters.AddWithValue("@TotalFee", f.TotalFee);
                            cmd.Parameters.AddWithValue("@Installment1", f.Installment1);
                            cmd.Parameters.AddWithValue("@Installment2", f.Installment2);
                            cmd.Parameters.AddWithValue("@Installment3", f.Installment3);
                            cmd.Parameters.AddWithValue("@Mode", 6);

                            int status = cmd.ExecuteNonQuery();
                            if (status < 0)
                            {
                                TempData["Success"] = "Fee details edited successfully";
                                return RedirectToAction("FeeList", "Fee");
                            }
                            else
                            {
                                TempData["Error"] = "Failed to edit!";
                            }
                        }
                        else
                        {
                            cmd = new SqlCommand("proc_fee", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ClassId", f.ClassId);
                            cmd.Parameters.AddWithValue("@TotalFee", f.TotalFee);
                            cmd.Parameters.AddWithValue("@Installment1", f.Installment1);
                            cmd.Parameters.AddWithValue("@Installment2", f.Installment2);
                            cmd.Parameters.AddWithValue("@Installment3", f.Installment3);

                            cmd.Parameters.AddWithValue("@Mode", 5);

                            int status = cmd.ExecuteNonQuery();
                            if (status < 0)
                            {
                                TempData["Success"] = "Fee details created successfully";
                                return RedirectToAction("FeeList", "Fee");
                            }
                            else
                            {
                                TempData["Error"] = "Failed to create!";
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
                    con.Close();
                }

                return View(f);
            }




            public ActionResult DeleteFee(int id)
            {
                try
                {
                    con.Open();
                    cmd = new SqlCommand("proc_fee", con);


                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@FeeId", id);
                    cmd.Parameters.AddWithValue("@Mode", 8);

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

                return RedirectToAction("FeeList", "Fee");

            }
        }
}