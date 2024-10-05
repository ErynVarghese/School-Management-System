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
    public class EmpCRUDController : Controller
    {

        EmployerRepo emprepo = new EmployerRepo();
        DepartmentRepo deptrepo = new DepartmentRepo();


        List<Department> deptlist = new List<Department>();

        public EmpCRUDController()
        {
            deptlist = deptrepo.GetAll();
            ViewBag.DeptList = deptlist;

            ViewBag.MaxDeptId = deptrepo.GetNextId();

        }


        public ActionResult EmpList()
        {
            List<Employer> eList = new List<Employer>();

            eList = emprepo.GetAll();

            return View(eList);

        }


        [HttpGet]
        public ActionResult AddEmp(int id = 0)
        {
            Employer emp = new Employer();

            if (id > 0)
            {
                emp = emprepo.GetById(id);

            }

            return View(emp);
        }


        [HttpPost]
        public ActionResult AddEmp(Employer emp)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (emp.EmpId > 0)
                    {
                        string result = emprepo.Update(emp);
                        if (result == "Success")
                        {
                            ModelState.Clear();
                            TempData["Success"] = "Updated successfully...";
                        }
                        else
                        {
                            TempData["Error"] = "Failed to update...";
                        }
                    }
                    else
                    {
                        string result = emprepo.Create(emp);
                        if (result == "Success")
                        {
                            ModelState.Clear();
                            TempData["Success"] = "Created successfully...";
                        }
                        else
                        {
                            TempData["Error"] = "Failed to create...";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to update/create...";
                    throw ex.InnerException;
                }
            }
            else
            {
                TempData["Error"] = "Fill out all the details!";
            }

            return View(emp);
        }

/**

        [HttpGet]
        public JsonResult GetSectionsByClassId(int classId)
        {
            List<Section> sList = sectionRepo.GetSectionsByClassId(classId);
            return Json(sList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetClassFeeById(int classId)
        {

            decimal classFee = feestructrepo.GetClassFeeById(classId);
            return Json(classFee, JsonRequestBehavior.AllowGet);
        }

        **/


        public ActionResult DeleteEmp(int id)
        {
            try
            {
                string result = emprepo.Delete(id);
                if (result == "Success")
                    TempData["Success"] = "Record Deleted successfully...";
                else
                    TempData["Error"] = "Unable to delete...";

                // return RedirectToAction("SubCategoryList");
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            //modify below 
            return RedirectToAction("GetList");
        }


    }
}

