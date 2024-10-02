using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace SMS.Controllers
{
   
    public class FeeCollectionController : Controller
    {
        // GET: FeeCollection
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public ActionResult FeeCollectionList(string searchedname)
        {
            List<FeeCollection> feelist = new List<FeeCollection>();
            List<ById> classlist = new List<ById>();
            List<ById> studentlist = new List<ById>();

            Dictionary<int, string> classDict = new Dictionary<int, string>();
            Dictionary<int, string> studentDict = new Dictionary<int, string>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_feecol", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", 1);

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
                    f.StudentName = sdr.GetString(7);

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
                cmd.Parameters.AddWithValue("@Mode", 10);

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    ById en = new ById();
                    en.Id = sdr.GetInt32(0);
                    en.Name = sdr.GetString(1);
                    classlist.Add(en);
                    classDict[sdr.GetInt32(0)] = sdr.GetString(1);
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching class names. " + ex.Message;
                con.Close();
            }
            finally
            {
                con.Close();
            }


            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 11);

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    ById en = new ById();
                    en.Id = sdr.GetInt32(0);
                    en.Name = sdr.GetString(1);
                    studentlist.Add(en);
                    studentDict[sdr.GetInt32(0)] = sdr.GetString(1);
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching student names. " + ex.Message;
                Debug.WriteLine(ex.Message);
                con.Close();
            }
            finally
            {
                con.Close();
            }

            ViewBag.ClassDict = classDict;
            ViewBag.StudentDict = studentDict;

            // Filter based on selected employee name
            ViewBag.ClassList = new SelectList(classlist, "Id", "Name");
            ViewBag.StudentList = new SelectList(studentlist, "Id", "Name");

            if (searchedname != null)
            {
                feelist = feelist.Where(cl => cl.ClassId.ToString().Contains(searchedname.ToString())).ToList();

            }


            return View(feelist);
        }

        [HttpGet]
        public ActionResult AddFeeCol(int id = 0, int? classId = null , int? studentId = null)
        {
            FeeCollection f = new FeeCollection();
            List<GetIdfromName> classList = new List<GetIdfromName>();
            List<GetIdfromName> studentList = new List<GetIdfromName>();

            ViewBag.ToPay1 = "false";
            ViewBag.ToPay2 = "false";
            ViewBag.ToPay3 = "false";

            try
            {

                if (id > 0)
                {
                    con.Open();
                    cmd = new SqlCommand("proc_feecol", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeColId", id);
                    cmd.Parameters.AddWithValue("@Mode", 2);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.Read())
                    {
                        f.FeeColId = sdr.GetInt32(0);
                        f.StudentId = sdr.GetInt32(1);
                        f.ClassId = sdr.GetInt32(2);
                        f.Installment1 = sdr.GetString(3);
                        f.Installment2 = sdr.GetString(4);
                        f.Installment3 = sdr.GetString(5);
                        f.FeeStatus = sdr.GetString(6);

                    }

                    con.Close();

                    

                    cmd = new SqlCommand("proc_student", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", f.ClassId);
                    cmd.Parameters.AddWithValue("@Mode", 9);

                    SqlDataReader sdrSection = cmd.ExecuteReader();
                    while (sdrSection.Read())
                    {
                        studentList.Add(new GetIdfromName
                        {
                            Id = sdrSection.GetInt32(0),
                            Name = sdrSection.GetString(1)
                        });
                    }

                    con.Close();
                    ViewBag.StudentList = new SelectList(studentList, "Id", "Name");

                }
                else
                {
                    cmd = new SqlCommand("proc_feecol", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 3);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        ViewBag.MaxFeeColId = sdr.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request: " + ex.Message;
            }
            finally
            {
                con.Close();
            }


            try
            {
                cmd = new SqlCommand("proc_class", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdrClass = cmd.ExecuteReader();
                while (sdrClass.Read())
                {
                    classList.Add(new GetIdfromName
                    {
                        Id = sdrClass.GetInt32(0),
                        Name = sdrClass.GetString(1)
                    });
                }
                ViewBag.ClassList = new SelectList(classList, "Id", "Name");
               // ViewBag.StudentList = new SelectList(studentList, "Id", "Name");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching classes: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

           // ViewBag.StudentList = new SelectList(studentList, "Id", "Name");

            //ViewBag.StudentList = new SelectList(studentList, "Id", "Name" ,studentId);


            if (classId.HasValue)
            {
                try
                {
                    cmd = new SqlCommand("proc_student", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", classId.Value);
                    cmd.Parameters.AddWithValue("@Mode", 9);

                    SqlDataReader sdrSection = cmd.ExecuteReader();
                    while (sdrSection.Read())
                    {
                        studentList.Add(new GetIdfromName
                        {
                            Id = sdrSection.GetInt32(0),
                            Name = sdrSection.GetString(1)
                        });
                    }
                    ViewBag.StudentList = new SelectList(studentList, "Id", "Name", studentId);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error fetching sections: " + ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }

            return View(f);
        }

        [HttpPost]
        public ActionResult AddFeeCol(FeeCollection f, int? classId, bool? isClassSelection , bool? isStudentSelection )
        {
            List<GetIdfromName> classList = new List<GetIdfromName>();

           // decimal totfee = 0;


            try
            {
                cmd = new SqlCommand("proc_class", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdrClass = cmd.ExecuteReader();
                while (sdrClass.Read())
                {
                    classList.Add(new GetIdfromName
                    {
                        Id = sdrClass.GetInt32(0),
                        Name = sdrClass.GetString(1)
                    });
                }
                ViewBag.ClassList = new SelectList(classList, "Id", "Name", classId);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching classes: " + ex.Message;
            }
            finally
            {
                con.Close();
            }


            if (classId.HasValue)
            {
                List<GetIdfromName> studentList = new List<GetIdfromName>();
                try
                {
                    cmd = new SqlCommand("proc_student", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", classId.Value);
                    cmd.Parameters.AddWithValue("@Mode", 9);

                    SqlDataReader sdrSection = cmd.ExecuteReader();
                    while (sdrSection.Read())
                    {
                        studentList.Add(new GetIdfromName
                        {
                            Id = sdrSection.GetInt32(0),
                            Name = sdrSection.GetString(1)
                        });
                    }
                    ViewBag.StudentList = new SelectList(studentList, "Id", "Name", f.StudentId);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error fetching sections: " + ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }

            if (isClassSelection.HasValue && isClassSelection.Value)
            {
                // adding to viewbag 

                decimal actualInstall1 = 0;
                decimal actualInstall2 = 0;
                decimal actualInstall3 = 0;


                con.Open();
                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@classid", classId.Value);
                cmd.Parameters.AddWithValue("@mode", 9);

                SqlDataReader sdrin = cmd.ExecuteReader();
                while (sdrin.Read())
                {
                    actualInstall1 = sdrin.GetDecimal(0);
                    actualInstall2 = sdrin.GetDecimal(1);
                    actualInstall3 = sdrin.GetDecimal(2);
                }
                con.Close();


                // Set values to ViewBag
                ViewBag.ActualInstall1 = actualInstall1;
                ViewBag.ActualInstall2 = actualInstall2;
                ViewBag.ActualInstall3 = actualInstall3;

                

                ModelState.Clear();

                return View(f);
            }

            if (isStudentSelection.HasValue && isStudentSelection.Value)
            {
                // adding to viewbag 

                decimal actualInstall1 = 0;
                decimal actualInstall2 = 0;
                decimal actualInstall3 = 0;


                con.Open();
                cmd = new SqlCommand("proc_fee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@classid", classId.Value);
                cmd.Parameters.AddWithValue("@mode", 9);

                SqlDataReader sdrin = cmd.ExecuteReader();
                while (sdrin.Read())
                {
                    actualInstall1 = sdrin.GetDecimal(0);
                    actualInstall2 = sdrin.GetDecimal(1);
                    actualInstall3 = sdrin.GetDecimal(2);
                }
                con.Close();


                // Set values to ViewBag
                ViewBag.ActualInstall1 = actualInstall1;
                ViewBag.ActualInstall2 = actualInstall2;
                ViewBag.ActualInstall3 = actualInstall3;

                //get the total fee from class table

                con.Open();
                cmd = new SqlCommand("proc_feecol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentid", f.StudentId);
                cmd.Parameters.AddWithValue("@mode", 10);

                SqlDataReader sdrin1 = cmd.ExecuteReader();
                if (sdrin1.Read()) // Read the first row returned
                {
                    // Store the Installment values in ViewBag, defaulting to false if NULL
                    ViewBag.ToPay1 = sdrin1.GetString(0);
                    ViewBag.ToPay2 =  sdrin1.GetString(1);
                    ViewBag.ToPay3 = sdrin1.GetString(2);

                    // Default to false if any value is NULL
                    ViewBag.ToPay1 = sdrin1.IsDBNull(0) ? "false" : ViewBag.ToPay1;
                    ViewBag.ToPay2 = sdrin1.IsDBNull(1) ? "false" : ViewBag.ToPay2;
                    ViewBag.ToPay3 = sdrin1.IsDBNull(2) ? "false" : ViewBag.ToPay3;
                }

                con.Close();



                ModelState.Clear();

                return View(f);
            }





            if ((!isClassSelection.HasValue || !isClassSelection.Value) && (!isStudentSelection.HasValue || !isStudentSelection.Value))
            {

                if (ModelState.IsValid && classId.HasValue)
                {
                    try
                    {
                        //get the total fee from class table

                        con.Open();
                        cmd = new SqlCommand("proc_feecol", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@studentid", f.StudentId);
                        cmd.Parameters.AddWithValue("@mode", 10);

                        SqlDataReader sdrin1 = cmd.ExecuteReader();
                        if (sdrin1.Read()) // Read the first row returned
                        {
                            // Store the Installment values in ViewBag, defaulting to false if NULL
                            ViewBag.ToPay1 =  sdrin1.GetString(0);
                            ViewBag.ToPay2 =  sdrin1.GetString(1);
                            ViewBag.ToPay3 =  sdrin1.GetString(2);

                            // Default to false if any value is NULL
                            ViewBag.ToPay1 = sdrin1.IsDBNull(0) ? false : ViewBag.ToPay1;
                            ViewBag.ToPay2 = sdrin1.IsDBNull(1) ? false : ViewBag.ToPay2;
                            ViewBag.ToPay3 = sdrin1.IsDBNull(2) ? false : ViewBag.ToPay3;
                        }

                        con.Close();
                    


                        decimal actualInstall1 = 0;
                        decimal actualInstall2 = 0;
                        decimal actualInstall3 = 0;


                        con.Open();
                        cmd = new SqlCommand("proc_fee", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@classid", classId.Value);
                        cmd.Parameters.AddWithValue("@mode", 9);

                        SqlDataReader sdrin = cmd.ExecuteReader();
                        while (sdrin.Read())
                        {
                            actualInstall1 = sdrin.GetDecimal(0);
                            actualInstall2 = sdrin.GetDecimal(1);
                            actualInstall3 = sdrin.GetDecimal(2);
                        }
                        con.Close();

                        int mode = 0;
                        // Set values to ViewBag
                        ViewBag.ActualInstall1 = actualInstall1;
                        ViewBag.ActualInstall2 = actualInstall2;
                        ViewBag.ActualInstall3 = actualInstall3;


                        con.Open();
                        cmd = new SqlCommand("proc_feecol", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (f.FeeColId > 0)
                        {
                            cmd.Parameters.AddWithValue("@Mode", 6);
                            mode = 6;
                            cmd.Parameters.AddWithValue("@FeeColId", f.FeeColId);

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Mode", 11);
                            mode = 11;
                            cmd.Parameters.AddWithValue("@FeeColId", f.FeeColId);
                        }

                        cmd.Parameters.AddWithValue("@StudentId", f.StudentId);
                        cmd.Parameters.AddWithValue("@ClassId", classId.Value);

                       //string conf1 = "false";
                       // string conf2 = "false";
                       // string conf3 = "false";



                        if (((f.Installment1 == "true") || (ViewBag.ToPay1 == "true")) && (mode == 11) )
                        {
                           // conf1 = "true";
                            f.Installment1 = "true";

                        } if (((f.Installment2 == "true") || (ViewBag.ToPay2 == "true")) && (mode == 11))
                        {
                           // conf2 = "true";
                            f.Installment2 = "true";
                        }
                       if (((f.Installment3 == "true") || (ViewBag.ToPay3 == "true"))&& (mode == 11))
                        {
                            //conf3 = "true";
                            f.Installment3 = "true";
                        }


                        cmd.Parameters.AddWithValue("@Installment1", f.Installment1);
                        cmd.Parameters.AddWithValue("@Installment2", f.Installment2);
                        cmd.Parameters.AddWithValue("@Installment3", f.Installment3);



                        if (f.Installment1 == "true" && f.Installment2 == "true" && f.Installment3 == "true")
                        {
                          //  f.FeeStatus = "Paid Installment 3";
                            cmd.Parameters.AddWithValue("@FeeStatus", "Paid Installment 1,2,3");
                        }
                        else if (f.Installment1 == "true" && f.Installment2 == "true")
                        {
                          //  f.FeeStatus = "Paid Installment 2";
                            cmd.Parameters.AddWithValue("@FeeStatus", "Paid Installment 1,2 ");
                        }
                        else if (f.Installment1 == "true")
                        {
                        //    f.FeeStatus = "Paid Installment 1";
                            cmd.Parameters.AddWithValue("@FeeStatus", "Paid Installment 1");
                        }
                        else
                        {
                           // f.FeeStatus = "Paid Nothing!";
                            cmd.Parameters.AddWithValue("@FeeStatus", "Paid Nothing");
                        }


                        int status = cmd.ExecuteNonQuery();

                        if (status <= 0)
                        {
                            TempData["Success"] = f.FeeColId > 0 ? "Fee Collection details updated successfully!" : "Fee details added successfully!";
                            return RedirectToAction("FeeCollectionList", "FeeCollection");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to add/update the records.";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "Error in processing the request: " + ex.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    TempData["Error"] = "Fill out all the details!";
                }
            }
            else
            {
                ModelState.Clear();
            }

            return View(f);
        }




        public ActionResult DeleteFeeCol(int id)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("proc_feecol", con);


                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@FeeColId", id);
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


            return RedirectToAction("FeeCollectionList", "FeeCollection");

        }
    }
}