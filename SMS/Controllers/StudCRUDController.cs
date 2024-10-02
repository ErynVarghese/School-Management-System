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
    public class StudCRUDController : Controller
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public ActionResult StudList(string searchedname)
        {
            List<Student> studlist = new List<Student>();
            List<ById> enlist = new List<ById>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {

                    Student stud = new Student();

                    stud.StudentId = sdr.GetInt32(0);
                    stud.StudentName = sdr.GetString(1);
                    stud.DOB = sdr.GetDateTime(2);
                    stud.ClassId = sdr.GetInt32(3);
                    stud.SectionId = sdr.GetInt32(4);
                    stud.FatherName = sdr.GetString(5);
                    stud.ContactNo = sdr.GetInt32(6);
                    stud.StudentAddress = sdr.GetString(7);
                    stud.StudentUsername = sdr.GetString(8);
                    stud.StudentPassword = sdr.GetInt32(9);
                    stud.StudentFee = sdr.GetDecimal(10);

                    studlist.Add(stud);
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
                cmd = new SqlCommand("proc_student", con);
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
                TempData["Error"] = "Error fetching student names. " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            ViewBag.StudList = new SelectList(enlist, "Id", "Name");

           
            if (searchedname != null)
            {
                studlist = studlist.Where(stud => stud.StudentId.ToString().Contains(searchedname.ToString())).ToList();

            }

            return View(studlist);
        }


        [HttpGet]
        public ActionResult AddStud(int id = 0, int? classId = null)
        {
            Student stud = new Student();
            List<GetIdfromName> classList = new List<GetIdfromName>();
            List<GetIdfromName> sectionList = new List<GetIdfromName>();

            try
            {

                if (id > 0)
                {
                    con.Open();
                    cmd = new SqlCommand("proc_student", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", id);
                    cmd.Parameters.AddWithValue("@Mode", 1);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.Read())
                    {
                        stud.StudentId = sdr.GetInt32(0);
                        stud.StudentName = sdr.GetString(1);
                        stud.DOB = sdr.GetDateTime(2);
                        stud.ClassId = sdr.GetInt32(3);
                        stud.SectionId = sdr.GetInt32(4);
                        stud.FatherName = sdr.GetString(5);
                        stud.ContactNo = sdr.GetInt32(6);
                        stud.StudentAddress = sdr.GetString(7);
                        stud.StudentUsername = sdr.GetString(8);
                        stud.StudentPassword = sdr.GetInt32(9);
                        stud.StudentFee = sdr.GetDecimal(10);
                    }

                } else
                {
                    cmd = new SqlCommand("proc_student", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 7);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        ViewBag.MaxStudId = sdr.GetInt32(0);
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

            ViewBag.SectionList = new SelectList(sectionList, "Id", "Name");

           
            if (classId.HasValue)
            {
                try
                {
                    cmd = new SqlCommand("proc_section", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", classId.Value); 
                    cmd.Parameters.AddWithValue("@Mode", 6); 

                    SqlDataReader sdrSection = cmd.ExecuteReader();
                    while (sdrSection.Read())
                    {
                        sectionList.Add(new GetIdfromName
                        {
                            Id = sdrSection.GetInt32(0),
                            Name = sdrSection.GetString(1)
                        });
                    }
                    ViewBag.SectionList = new SelectList(sectionList, "Id", "Name", stud.SectionId); 
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

            return View(stud);
        }

        [HttpPost]
        public ActionResult AddStud(Student stud, int? classId, bool? isClassSelection)
        {
            List<GetIdfromName> classList = new List<GetIdfromName>();

            decimal totfee = 0;


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
                List<GetIdfromName> sectionList = new List<GetIdfromName>();
                try
                {
                    cmd = new SqlCommand("proc_section", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", classId.Value);
                    cmd.Parameters.AddWithValue("@Mode", 6);

                    SqlDataReader sdrSection = cmd.ExecuteReader();
                    while (sdrSection.Read())
                    {
                        sectionList.Add(new GetIdfromName
                        {
                            Id = sdrSection.GetInt32(0),
                            Name = sdrSection.GetString(1)
                        });
                    }
                    ViewBag.SectionList = new SelectList(sectionList, "Id", "Name", stud.SectionId);
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
                con.Open();
                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ClassId", classId.Value);
                cmd.Parameters.AddWithValue("Mode", 9); 

                SqlDataReader sdf = cmd.ExecuteReader();
                if (sdf.Read())
                {
                    stud.StudentFee = sdf.GetDecimal(0); 
                }

                con.Close();

                ModelState.Clear();

                return View(stud);
            }

           
            if (!isClassSelection.HasValue || !isClassSelection.Value)
            {
                
                if (ModelState.IsValid && classId.HasValue)
                {
                    try
                    {
                        //get the total fee from class table

                        con.Open();
                        cmd = new SqlCommand("proc_class", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("ClassId", classId.Value);
                        cmd.Parameters.AddWithValue("Mode", 9);

                        SqlDataReader sdf = cmd.ExecuteReader();

                        while (sdf.Read())
                        {
                             totfee = sdf.GetDecimal(0);
                        }

                        con.Close();


                        con.Open();
                        cmd = new SqlCommand("proc_student", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (stud.StudentId > 0) 
                        {
                            cmd.Parameters.AddWithValue("@Mode", 2); 
                            cmd.Parameters.AddWithValue("@StudentId", stud.StudentId); 
                        }
                        else 
                        {
                            cmd.Parameters.AddWithValue("@Mode", 3); 
                        }

                        cmd.Parameters.AddWithValue("@StudentName", stud.StudentName);
                        cmd.Parameters.AddWithValue("@DOB", stud.DOB);
                        cmd.Parameters.AddWithValue("@ClassId", classId.Value);
                        cmd.Parameters.AddWithValue("@SectionId", stud.SectionId);
                        cmd.Parameters.AddWithValue("@FatherName", stud.FatherName);
                        cmd.Parameters.AddWithValue("@ContactNo", stud.ContactNo);
                        cmd.Parameters.AddWithValue("@StudentAddress", stud.StudentAddress);
                        cmd.Parameters.AddWithValue("@StudentUsername", stud.StudentUsername);
                        cmd.Parameters.AddWithValue("@StudentPassword", stud.StudentPassword);
                        cmd.Parameters.AddWithValue("@StudentFee", totfee);

                        int status = cmd.ExecuteNonQuery();

                        if (status <= 0)
                        {
                            TempData["Success"] = stud.StudentId > 0 ? "Student details updated successfully!" : "Student details added successfully!";
                            return RedirectToAction("StudList", "StudCRUD");
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

            return View(stud);
        }



        public ActionResult DeleteStud(int id)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("proc_student", con);


                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@StudentId", id);
                cmd.Parameters.AddWithValue("@Mode", 4);

                int status = cmd.ExecuteNonQuery();

                if (status < 0)
                {
                    TempData["Success"] = "Student Record deleted successfully";

                }
                else
                {
                    TempData["Error"] = "Failed to delete student record!";
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

            return RedirectToAction("StudList", "StudCRUD");

        }


    }
}