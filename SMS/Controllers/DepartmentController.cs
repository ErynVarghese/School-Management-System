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
    public class DepartmentController : Controller
    {
        // GET: Department


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
            SqlCommand cmd = null;

            public ActionResult DeptList(string searchedname)
            {
                List<Department> deptlist = new List<Department>();
                List<ById> enlist = new List<ById>();

                try
                {
                    con.Open();
                    cmd = new SqlCommand("proc_dept", con);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 0);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {

                        Department dept = new Department();

                        dept.DeptId = sdr.GetInt32(0);
                        dept.DeptName = sdr.GetString(1);


                        deptlist.Add(dept);
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
                cmd = new SqlCommand("proc_dept", con);
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

            ViewBag.DeptList = new SelectList(enlist, "Id", "Name");

            // Filter based on selected employee name

            if (searchedname != null)
            {
                deptlist = deptlist.Where(dept => dept.DeptId.ToString().Contains(searchedname.ToString())).ToList();

            }

            return View(deptlist);
        }

        [HttpGet]
        public ActionResult AddDept(int id = 0)
        {
            Department dept = new Department();

            try
            {
                if (id > 0)
                {
                    con.Open();
                    cmd = new SqlCommand("proc_dept", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeptId", id);
                    cmd.Parameters.AddWithValue("@Mode", 1);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        dept.DeptId = sdr.GetInt32(0);
                        dept.DeptName = sdr.GetString(1);
                    }

                  
                }
                else
                {
                    con.Open();
                    // Fetch the max Department ID when creating a new department
                    cmd = new SqlCommand("proc_dept", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 7);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        ViewBag.MaxDeptId = sdr.GetInt32(0);
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

            return View(dept);
        }


        [HttpPost]
        public ActionResult AddDept(Department dept)
        {
            try
            {
                con.Open();

                if (ModelState.IsValid)
                {



                    // If no duplicate department name found, proceed with insert or update
                    if (dept.DeptId > 0)
                    {
                        // Edit the department
                        cmd = new SqlCommand("proc_dept", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DeptId", dept.DeptId);
                        cmd.Parameters.AddWithValue("@DeptName", dept.DeptName);
                        cmd.Parameters.AddWithValue("@Mode", 2);

                        int status = cmd.ExecuteNonQuery();
                        if (status < 0)
                        {
                            TempData["Success"] = "Department details edited successfully";
                            return RedirectToAction("DeptList", "Department");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to edit!";
                        }
                    }
                    else
                    {

                        // Check if the department name already exists (case-insensitive)
                        SqlCommand checkCmd = new SqlCommand("proc_dept", con);
                        checkCmd.CommandType = CommandType.StoredProcedure;
                        checkCmd.Parameters.AddWithValue("@DeptName", dept.DeptName);
                        checkCmd.Parameters.AddWithValue("@Mode", 6);

                        SqlDataReader reader = checkCmd.ExecuteReader();

                        bool deptExists = false;
                        if (reader.Read())
                        {
                            deptExists = reader.GetInt32(0) > 0; // Check if the count is greater than 0
                        }

                        reader.Close(); // Close the reader after use

                        if (deptExists)
                        {
                            TempData["Error"] = "This department name already exists!";
                            return View(dept);
                        }

                        // Add new department
                        cmd = new SqlCommand("proc_dept", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DeptName", dept.DeptName);
                        cmd.Parameters.AddWithValue("@Mode", 3);

                        int status = cmd.ExecuteNonQuery();
                        if (status < 0)
                        {
                            TempData["Success"] = "Department details created successfully";
                            return RedirectToAction("DeptList", "Department");
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

            return View(dept);
        }




        public ActionResult DeleteDept(int id)
            {
                try
                {
                    con.Open();
                    cmd = new SqlCommand("proc_dept", con);


                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@DeptId", id);
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

                return RedirectToAction("DeptList", "Department");

            }


            [HttpGet]


            public ActionResult GetByDeptName()
            {


                List<ById> enlist = new List<ById>();

                try
                {
                    con.Open();
                    cmd = new SqlCommand("proc_dept", con);
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
                    TempData["Error"] = "Error in processing the request. " + ex.Message;
                }
                finally
                {
                    con.Close();
                }

                ViewBag.DepartmentList = new SelectList(enlist, "Id", "Name");
                return View(new Department());
            }

            [HttpPost]
            public ActionResult GetByDeptName(string name)
            {
                Department dept = new Department();
                List<ById> enlist = new List<ById>();

                try
                {

                    con.Open();
                    cmd = new SqlCommand("proc_dept", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DeptId", Convert.ToInt32(name));
                    cmd.Parameters.AddWithValue("@Mode", 1);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.Read())
                    {
                        dept.DeptId = sdr.GetInt32(0);
                        dept.DeptName = sdr.GetString(1);
                    }

                    con.Close();
                }

                catch (Exception ex)
                {
                    TempData["Error"] = "Error in processing the request. " + ex.Message;
                    con.Close();
                }

                try
                {

                    con.Open();
                    cmd = new SqlCommand("proc_dept", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 5);
                    SqlDataReader sdr2 = cmd.ExecuteReader();

                    while (sdr2.Read())
                    {
                        ById en = new ById();
                        en.Id = sdr2.GetInt32(0);
                        en.Name = sdr2.GetString(1);
                        enlist.Add(en);
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error in processing the request while fetching department list: " + ex.Message;
                    con.Close();
                }


                ViewBag.DepartmentList = new SelectList(enlist, "Id", "Name");

                return View(dept);
            }

        }
    }


