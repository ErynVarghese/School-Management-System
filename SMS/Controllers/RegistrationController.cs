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
    public class RegistrationController : Controller
    {
        // GET: Registration


        public ActionResult EmpRegister()
        {
            List<GetIdfromName> departmentList = new List<GetIdfromName>();

            try
            {
                // Fetch the max employee ID
                cmd = new SqlCommand("proc_emp", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 7);

                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    ViewBag.MaxEmpId = sdr.GetInt32(0);
                }
                con.Close();

                // Fetch the list of departments
                cmd = new SqlCommand("proc_dept", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdrDept = cmd.ExecuteReader();
                while (sdrDept.Read())
                {
                    departmentList.Add(new GetIdfromName
                    {
                        Id = sdrDept.GetInt32(0), // Department ID
                        Name = sdrDept.GetString(1) // Department Name
                    });
                }
                ViewBag.DepartmentList = new SelectList(departmentList, "Id", "Name");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return View();
        }


        [HttpPost]
        public ActionResult EmpRegister(Employer emp)
        {
            List<GetIdfromName> departmentList = new List<GetIdfromName>();

            try
            {
                // Repopulate the list of departments for the dropdown
                cmd = new SqlCommand("proc_dept", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdrDept = cmd.ExecuteReader();
                while (sdrDept.Read())
                {
                    departmentList.Add(new GetIdfromName
                    {
                        Id = sdrDept.GetInt32(0),
                        Name = sdrDept.GetString(1) 
                    });
                }
                ViewBag.DepartmentList = new SelectList(departmentList, "Id", "Name");

                con.Close();

                if (ModelState.IsValid)
                {
                    con.Open();
                    cmd = new SqlCommand("proc_emp", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmpName", emp.EmpName);
                    cmd.Parameters.AddWithValue("@ContactNo", emp.ContactNo);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@DOB", emp.DOB);
                    cmd.Parameters.AddWithValue("@DeptId", emp.DeptId);
                    cmd.Parameters.AddWithValue("@DOJ", emp.DOJ);
                    cmd.Parameters.AddWithValue("@EmpUsername", emp.EmpUsername);
                    cmd.Parameters.AddWithValue("@EmpPassword", emp.EmpPassword);
                    cmd.Parameters.AddWithValue("@Mode", 3);

                    int status = cmd.ExecuteNonQuery();

                    if (status < 0)
                    {
                        TempData["Success"] = "Registration successful! :)";
                        if (Session["UserType"] == null)
                        {
                            return RedirectToAction("GetUserType", "Login");
                        } else
                        {
                            return RedirectToAction("MainPage", "Login");
                        } 
                    }
                    else
                    {
                        TempData["Error"] = "Registration failed.";
                        return View(emp);
                    }
                }
                else
                {
                    TempData["Error"] = "Fill out all the details!!";

                    return View(emp);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request: " + ex.Message;
                return View(emp);
            }
            finally
            {
                con.Close();

            }
            //return View(emp);
        }

        public ActionResult StudRegister(int? classId = null)
        {
            Student stud = new Student();
            List<GetIdfromName> classList = new List<GetIdfromName>();
            List<GetIdfromName> sectionList = new List<GetIdfromName>();

            try
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
                    ViewBag.SectionList = new SelectList(sectionList, "Id", "Name");
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
        public ActionResult StudRegister(Student stud, int? classId, bool? isClassSelection)
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
                    ViewBag.SectionList = new SelectList(sectionList, "Id", "Name", stud.SectionId); // Maintain selected SectionId
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

                ModelState.Clear(); // Ignore validation errors for class submission

                return View(stud);
            }

            // Check ModelState for registration submission
            if (!isClassSelection.HasValue || !isClassSelection.Value)
            {
                if (ModelState.IsValid && classId.HasValue)
                {
                    try
                    {
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

                        cmd.Parameters.AddWithValue("@StudentName", stud.StudentName);
                        cmd.Parameters.AddWithValue("@DOB", stud.DOB);
                        cmd.Parameters.AddWithValue("@ClassId", classId.Value);
                        cmd.Parameters.AddWithValue("@SectionId", stud.SectionId);
                        cmd.Parameters.AddWithValue("@FatherName", stud.FatherName);
                        cmd.Parameters.AddWithValue("@ContactNo", stud.ContactNo);
                        cmd.Parameters.AddWithValue("@StudentAddress", stud.StudentAddress);
                        cmd.Parameters.AddWithValue("@StudentUsername", stud.StudentUsername);
                        cmd.Parameters.AddWithValue("@StudentPassword", stud.StudentPassword);
                        cmd.Parameters.AddWithValue("StudentFee", totfee);
                        cmd.Parameters.AddWithValue("@Mode", 3);

                        int status = cmd.ExecuteNonQuery();

                        if (status < 0)
                        {
                            TempData["Success"] = "Registration successful! :)";
                            if (Session["UserType"] == null)
                            {
                                return RedirectToAction("GetUserType", "Login");
                            }
                            else if (Session["UserType"].ToString() == "Admin")
                            {
                                return RedirectToAction("MainPage", "Login");
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Registration failed.";
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






        public ActionResult AdminRegister()
        {
            try
            {
                cmd = new SqlCommand("proc_admin", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 7);

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    ViewBag.MaxAdminId = sdr.GetInt32(0);
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

            return View();
        }

        [HttpPost]

        public ActionResult AdminRegister(Admin adm)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    con.Open();


                    cmd = new SqlCommand("proc_admin", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AdminUsername", adm.AdminUsername);
                    cmd.Parameters.AddWithValue("@AdminName", adm.AdminName);
                    cmd.Parameters.AddWithValue("@AdminPassword", adm.AdminPassword);
                    cmd.Parameters.AddWithValue("@Mode", 3);


                    int status = cmd.ExecuteNonQuery();


                    if (status < 0)
                    {
                        TempData["Success"] = "Registration successful! :)";
                        return RedirectToAction("MainPage", "Login");
                    }
                    else
                    {

                        TempData["Error"] = "Registration failed.";
                        return View(adm);
                    }
                }
                else
                {
                    TempData["Error"] = "Fill out all the details!";
                    return View(adm);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
                return View(adm);
            }
            finally
            {
                con.Close();

            }
        }



    }

}