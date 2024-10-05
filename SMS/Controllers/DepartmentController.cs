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
    public class DepartmentController : Controller
    {
        // GET: Department

        DepartmentRepo deptrepo = new DepartmentRepo();

        public DepartmentController()
        {

            ViewBag.MaxDeptId = deptrepo.GetNextId();

        }


        public ActionResult DeptList()
        {
            List<Department> dList = new List<Department>();

            dList = deptrepo.GetAll();

            return View(dList);
        }


        [HttpGet]
        public ActionResult AddDept(int id = 0)
        {
            Department dept = new Department();

            if (id > 0)
            {
                dept = deptrepo.GetById(id);

            }

            return View(dept);
        }

        [HttpPost]
        public ActionResult AddDept(Department dept)
        {           

            if (ModelState.IsValid)
            {

                try
                {
                    if (dept.DeptId > 0)
                    {
                        string result = deptrepo.Update(dept);
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

                        string checkDeptNameExists = deptrepo.CheckDeptName(dept.DeptName);

                        if (checkDeptNameExists == "Error")
                        {
                            TempData["Error"] = "This department name already exists";
                        }
                        else
                        {

                            string result = deptrepo.Create(dept);
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

            return View(dept);
        }

/***

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

***/

        public ActionResult DeleteDept(int id)
        {
            try
            {
                string result = deptrepo.Delete(id);
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