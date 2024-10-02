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
    public class LoginController : Controller
    {
        // GET: Login

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        [HttpGet]
        public ActionResult GetUserType()
        {
            return View();
        }

        [HttpPost]

        public ActionResult GetUserType(string type)
        {
            if (type == "Employer")
            {
                return RedirectToAction("EmpAuthentication", "Login");
            } else if (type == "Student")
            {
                return RedirectToAction("StudAuthentication", "Login");
            } else if (type == "Admin")
            {
                return RedirectToAction("AdminAuthentication", "Login");
            }
            else
            {
                TempData["Error"] = "Please select a valid user type.";
                return RedirectToAction("GetUserType");
            }
        }

        [HttpGet]
        public ActionResult EmpAuthentication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmpAuthentication(string username, int password)
        {
            Employer emp = new Employer();

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_emp", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmpUsername", username);
                cmd.Parameters.AddWithValue("@Mode", 6);

                bool checkusername = false;
                bool checkpassword = false;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                sda.Fill(ds);


                if (ds.Tables[0].Rows.Count > 0)
                {

                    checkusername = true;

                    int enteredpassword = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpPassword"].ToString());

                    if (enteredpassword == password)
                    {
                        checkpassword = true;

                        emp.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                        emp.EmpPassword = enteredpassword;
                    }

                }

                con.Close();



                if (checkusername && checkpassword)
                {
                    Session["EmpId"] = emp.EmpId;

                    con.Open();

                    SqlCommand cmdemp = new SqlCommand("proc_emp", con);

                    cmdemp.CommandType = CommandType.StoredProcedure;

                    cmdemp.Parameters.AddWithValue("@EmpUsername", username);
                    cmdemp.Parameters.AddWithValue("@Mode", 6);

                    SqlDataReader sdr = cmdemp.ExecuteReader();

                    while (sdr.Read())
                    {
                        Session["UserType"] = "Employer";
                        Session["EmpId"] = sdr.GetInt32(0);
                        Session["EmpName"] = sdr.GetString(1);
                        Session["ContactNo"] = sdr.GetInt32(2);
                        Session["Email"] = sdr.GetString(3);
                        Session["DOB"] = sdr.GetDateTime(4);
                        Session["DeptId"] = sdr.GetInt32(5);
                        Session["DOJ"] = sdr.GetDateTime(6);
                        Session["EmpUsername"] = sdr.GetString(7);
                        Session["EmpPassword"] = sdr.GetInt32(8);
                    }

                    TempData["Success"] = "Login Successfull";
                    return RedirectToAction("EmpList", "EmpCRUD");
                   
                }


                else if (checkusername && !checkpassword)
                {
                    TempData["Error"] = "Username exists, but password is incorrect!";
                }

                else
                {
                    TempData["Error"] = "Username does not exist, Click on Register to create a new one";

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

            return View();
        }



        [HttpGet]
        public ActionResult StudAuthentication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudAuthentication(string username, int password)
        {
            Student stud = new Student();

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_student", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentUsername", username);
                cmd.Parameters.AddWithValue("@Mode", 6);

                bool checkusername = false;
                bool checkpassword = false;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                sda.Fill(ds);


                if (ds.Tables[0].Rows.Count > 0)
                {

                    checkusername = true;

                    int enteredpassword = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentPassword"].ToString());

                    if (enteredpassword == password)
                    {
                        checkpassword = true;

                        stud.StudentId = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentId"].ToString());
                        stud.StudentPassword = enteredpassword;
                    }

                }

                con.Close();



                if (checkusername && checkpassword)
                {
                    Session["StudentId"] = stud.StudentId;

                    con.Open();

                    SqlCommand cmdstud = new SqlCommand("proc_student", con);

                    cmdstud.CommandType = CommandType.StoredProcedure;

                    cmdstud.Parameters.AddWithValue("@StudentUsername", username);
                    cmdstud.Parameters.AddWithValue("@Mode", 6);

                    SqlDataReader sdr = cmdstud.ExecuteReader();

                    while (sdr.Read())
                    {
                        Session["UserType"] = "Student";
                        Session["StudentId"] = sdr.GetInt32(0);
                        Session["StudentName"] = sdr.GetString(1);
                        Session["DOB"] = sdr.GetDateTime(2);
                        Session["ClassId"] = sdr.GetInt32(3);
                        Session["SectionId"] = sdr.GetInt32(4);
                        Session["FatherName"] = sdr.GetString(5);
                        Session["ContactNo"] = sdr.GetInt32(6);
                        Session["StudentAddress"] = sdr.GetString(7);
                        Session["StudentUsername"] = sdr.GetString(8);
                        Session["StudentPassword"] = sdr.GetInt32(9);
                    }

                    TempData["Success"] = "Login Successfull";
                    return RedirectToAction("StudList", "StudCRUD");
                   
                }


                else if (checkusername && !checkpassword)
                {
                    TempData["Error"] = "Username exists, but password is incorrect!";
                }

                else
                {
                    TempData["Error"] = "Username does not exist, Click on Register to create a new one";

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

            return View();
        }


        [HttpGet]
        public ActionResult AdminAuthentication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminAuthentication(string username, int password)
        {
            Admin adm = new Admin();

            try
            {
                con.Open();

                cmd = new SqlCommand("proc_admin", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AdminUsername", username);
                cmd.Parameters.AddWithValue("@Mode", 6);

                bool checkusername = false;
                bool checkpassword = false;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                sda.Fill(ds);


                if (ds.Tables[0].Rows.Count > 0)
                {

                    checkusername = true;

                    int enteredpassword = Convert.ToInt32(ds.Tables[0].Rows[0]["AdminPassword"].ToString());

                    if (enteredpassword == password)
                    {
                        checkpassword = true;

                        adm.AdminId = Convert.ToInt32(ds.Tables[0].Rows[0]["AdminId"].ToString());
                        adm.AdminPassword = enteredpassword;
                    }

                }

                con.Close();



                if (checkusername && checkpassword)
                {
                    Session["AdminId"] = adm.AdminId;

                    con.Open();

                    SqlCommand cmdemp = new SqlCommand("proc_admin", con);

                    cmdemp.CommandType = CommandType.StoredProcedure;

                    cmdemp.Parameters.AddWithValue("@AdminUsername", username);
                    cmdemp.Parameters.AddWithValue("@Mode", 6);

                    SqlDataReader sdr = cmdemp.ExecuteReader();

                    while (sdr.Read())
                    {
                        Session["UserType"] = "Admin";
                        Session["AdminId"] = sdr.GetInt32(0);
                        Session["AdminUsername"] = sdr.GetString(1);
                        Session["AdminName"] = sdr.GetString(2);
                        Session["AdminPassword"] = sdr.GetInt32(3);

                    }

                    TempData["Success"] = "Login Successfull";
                    return RedirectToAction("MainPage", "Login");
                }


                else if (checkusername && !checkpassword)
                {
                    TempData["Error"] = "Username exists, but password is incorrect!";
                }

                else
                {
                    TempData["Error"] = "Username does not exist, Click on Register to create a new one";

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

            return View();
        }



        public ActionResult MainPage()
        {
            Admin adm = new Admin();


            adm.AdminId = Convert.ToInt32(Session["AdminId"]);
            adm.AdminUsername = Session["AdminUserName"].ToString();
            adm.AdminName = Session["AdminName"].ToString();
            adm.AdminPassword = Convert.ToInt32(Session["AdminPassword"]);


            return View(adm);
        }

        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();
                Session.Clear();

            }

            catch (Exception ex)
            {
                TempData["Error"] = "Error in Processing the request. " + ex.Message;

            }

            finally
            {

            }

            return RedirectToAction("GetUserType");
        }




    }
}