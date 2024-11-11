using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SMS.Repositories;

namespace SMS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        EmployerRepo emprepo = new EmployerRepo();
        StudentRepo studrepo = new StudentRepo();
        AdminRepo adminrepo = new AdminRepo();


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
                Session["UserType"] = "Employer";
                return RedirectToAction("EmpAuthentication", "Login");

            } else if (type == "Student")
            {
                Session["UserType"] = "Student";
                return RedirectToAction("StudAuthentication", "Login");
            } else if (type == "Admin")
            {
                Session["UserType"] = "Admin";
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
                DataSet ds = emprepo.GetByUsername(username);

                bool checkusername = false;
                bool checkpassword = false;


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



                if (checkusername && checkpassword)
                {
                    Session["UserType"] = "Employer";
                    Session["EmpId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"]);
                    Session["EmpName"] = ds.Tables[0].Rows[0]["EmpName"].ToString();
                    Session["ContactNo"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ContactNo"]);
                    Session["Email"] = ds.Tables[0].Rows[0]["Email"].ToString();
                    Session["DOB"] = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"]);
                    Session["DeptId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["DeptId"]);
                    Session["DOJ"] = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOJ"]);
                    Session["EmpUsername"] = ds.Tables[0].Rows[0]["EmpUsername"].ToString();
                    Session["EmpPassword"] = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpPassword"]);

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
               
            }
            finally
            {
                
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
                DataSet ds = studrepo.GetByUsername(username);

                bool checkusername = false;
                bool checkpassword = false;



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


                if (checkusername && checkpassword)
                { 

                    Session["UserType"] = "Student";
                    Session["StudentId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentId"]);
                    Session["StudentName"] = ds.Tables[0].Rows[0]["StudentName"].ToString();
                    Session["DOB"] = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"]);
                    Session["ClassId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ClassId"]);
                    Session["SectionId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["SectionId"]);
                    Session["FatherName"] = ds.Tables[0].Rows[0]["FatherName"].ToString();
                    Session["ContactNo"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ContactNo"]);
                    Session["StudentAddress"] = ds.Tables[0].Rows[0]["StudentAddress"].ToString();
                    Session["StudentUsername"] = ds.Tables[0].Rows[0]["StudentUsername"].ToString();
                    Session["StudentPassword"] = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentPassword"]);


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
                
            }
            finally
            {

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

                DataSet ds = adminrepo.GetByUsername(username);

                bool checkusername = false;
                bool checkpassword = false;


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

                if (checkusername && checkpassword)
                {
                    Session["UserType"] = "Admin";
                    Session["AdminId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["AdminId"]);
                    Session["AdminUsername"] = ds.Tables[0].Rows[0]["AdminUsername"].ToString();
                    Session["AdminName"] = ds.Tables[0].Rows[0]["AdminName"].ToString();
                    Session["AdminPassword"] = Convert.ToInt32(ds.Tables[0].Rows[0]["AdminPassword"]);

                    TempData["Success"] = "Login Successfull";
                    return RedirectToAction("AdminRegister", "Registration");
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
         
            }
            finally
            {
               
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