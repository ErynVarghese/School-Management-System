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
    public class RegistrationController : Controller
    {
        // GET: Registration

        StudentRepo studrepo = new StudentRepo();
        EmployerRepo emprepo = new EmployerRepo();
        AdminRepo admrepo = new AdminRepo();

        ClassRepo classRepo = new ClassRepo();
        SectionRepo sectionRepo = new SectionRepo();
        DepartmentRepo deptrepo = new DepartmentRepo();


        List<Class> classlist = new List<Class>();

        List<Section> sectionlist = new List<Section>();

        List<Department> deptlist = new List<Department>();

        public RegistrationController()
        {
            classlist = classRepo.GetAll();
            ViewBag.ClassList = classlist;

            sectionlist = sectionRepo.GetAll();
            ViewBag.SectionList = sectionlist;

            deptlist = deptrepo.GetAll();
            ViewBag.DeptList = deptlist;

            ViewBag.MaxStudId = studrepo.GetNextId();
            ViewBag.MaxEmpId = deptrepo.GetNextId();
            ViewBag.MaxAdminId = admrepo.GetNextId();
        }


        public ActionResult EmpRegister()
        {
            return View();
        }


        [HttpPost]
        public ActionResult EmpRegister(Employer emp)
        {   
            
                if (ModelState.IsValid)
                {
                try
                {
                    string result = emprepo.Create(emp);
                     if (result == "Success")
                     {
                         ModelState.Clear();
                         TempData["Success"] = "Registered successfully...";
                     }
                     else
                        {
                           TempData["Error"] = "Failed to register...";
                        }
                    }                
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to register...";
                    throw ex.InnerException;
                }
            }
            else
            {
                TempData["Error"] = "Fill out all the details!";
            }

            return View(emp);
        }

        public ActionResult StudRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudRegister(Student stud)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string result = studrepo.Create(stud);
                    if (result == "Success")
                    {
                        ModelState.Clear();
                        TempData["Success"] = "Registered successfully...";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to register...";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to register...";
                    throw ex.InnerException;
                }
            }
            else
            {
                TempData["Error"] = "Fill out all the details!";
            }

            return View(stud);
        }




        public ActionResult AdminRegister()
        {
             return View();
        }

        [HttpPost]

        public ActionResult AdminRegister(Admin adm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string result = admrepo.Create(adm);
                    if (result == "Success")
                    {
                        ModelState.Clear();
                        TempData["Success"] = "Registered successfully...";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to register...";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to register...";
                    throw ex.InnerException;
                }
            }
            else
            {
                TempData["Error"] = "Fill out all the details!";
            }

            return View(adm);
        }

    }

}