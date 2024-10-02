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
    public class EmpCRUDController : Controller
    {
        // GET: SMSCRUD
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public ActionResult EmpList(string searchedname)
        {
            List<Employer> emplist = new List<Employer>();
            List<ById> enlist = new List<ById>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Employer emp = new Employer();
                    emp.EmpId = sdr.GetInt32(0);
                    emp.EmpName = sdr.GetString(1);
                    emp.ContactNo = sdr.GetInt32(2);
                    emp.Email = sdr.GetString(3);
                    emp.DOB = sdr.GetDateTime(4);
                    emp.DeptId = sdr.GetInt32(5);
                    emp.DOJ = sdr.GetDateTime(6);
                    emp.EmpUsername = sdr.GetString(7);
                    emp.EmpPassword = sdr.GetInt32(8);

                    emplist.Add(emp);
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

            
            try
            {
                con.Open();
                cmd = new SqlCommand("proc_emp", con);
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
                TempData["Error"] = "Error fetching employee names. " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            ViewBag.EmployeeList = new SelectList(enlist, "Id","Name");


            //if (!string.IsNullOrEmpty(searchedname))
            if (searchedname != null)
            {
                emplist = emplist.Where(emp => emp.EmpId.ToString().Contains(searchedname.ToString())).ToList();

            }


            return View(emplist);
        }



        [HttpGet]
        public ActionResult AddEmp(int id = 0)
        {
            Employer emp = new Employer();
            List<GetIdfromName> departmentList = new List<GetIdfromName>();

            try
            {
                if (id > 0)
                {


                    con.Open();
                    cmd = new SqlCommand("proc_emp", con);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmpId", id);
                    cmd.Parameters.AddWithValue("@Mode", 1);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        emp.EmpId = sdr.GetInt32(0);
                        emp.EmpName = sdr.GetString(1);
                        emp.ContactNo = sdr.GetInt32(2);
                        emp.Email = sdr.GetString(3);
                        emp.DOB = sdr.GetDateTime(4);
                        emp.DeptId = sdr.GetInt32(5);
                        emp.DOJ = sdr.GetDateTime(6);
                        emp.EmpUsername = sdr.GetString(7);
                        emp.EmpPassword = sdr.GetInt32(8);
                    }

                    con.Close();

                }  else
                {
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
                }



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

            return View(emp);

        }

        [HttpPost]

        public ActionResult AddEmp(Employer emp)
        {

            List<GetIdfromName> departmentList = new List<GetIdfromName>();


            try
            {
                
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


                con.Open();
                if (ModelState.IsValid)
                {

                    if (emp.EmpId > 0)
                    {

                        cmd = new SqlCommand("proc_emp", con);


                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EmpId", emp.EmpId);
                        cmd.Parameters.AddWithValue("@EmpName", emp.EmpName);
                        cmd.Parameters.AddWithValue("@ContactNo", emp.ContactNo);
                        cmd.Parameters.AddWithValue("@Email", emp.Email);
                        cmd.Parameters.AddWithValue("@DOB", emp.DOB);
                        cmd.Parameters.AddWithValue("@DeptId", emp.DeptId);
                        cmd.Parameters.AddWithValue("@DOJ", emp.DOJ);
                        cmd.Parameters.AddWithValue("@EmpUsername", emp.EmpUsername);
                        cmd.Parameters.AddWithValue("@EmpPassword", emp.EmpPassword);
                        cmd.Parameters.AddWithValue("@Mode", 2);

                        int status = cmd.ExecuteNonQuery();

                        if (status < 0)
                        {
                            TempData["Success"] = "Employee details edited successfully";
                            return RedirectToAction("EmpList", "EmpCRUD");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to edit!";
                            return View(emp);
                        }

                    }
                    else
                    {

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
                            TempData["Success"] = "Employee details created successfully";
                            return RedirectToAction("EmpList", "EmpCRUD");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to create!";
                            return View(emp);
                        }

                    }
                }
                else
                {
                    TempData["Error"] = "Fill out all the details.";
                    return View(emp);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
                return View(emp);
               
            }
            finally
            {
                con.Close();
            }

         
        }



        public ActionResult DeleteEmp(int id)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("proc_emp", con);


                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@EmpId", id);
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

            return RedirectToAction("EmpList", "EmpCRUD");

        }

    }
}





