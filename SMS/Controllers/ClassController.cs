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
    public class ClassController : Controller
    {
        StudentRepo studrepo = new StudentRepo();
        ClassRepo classRepo = new ClassRepo();
        SectionRepo sectionRepo = new SectionRepo();
        FeeStructureRepo feestructrepo = new FeeStructureRepo();


        List<Class> classlist = new List<Class>();

        List<Section> sectionlist = new List<Section>();

        public ClassController()
        {

            ViewBag.MaxClassId = classRepo.GetNextId();

        }


        public ActionResult ClassList()
        {
            List<Class> clist = new List<Class>();

            clist = classRepo.GetAll();

            return View(clist);
        }


        [HttpGet]
        public ActionResult AddClass(int id = 0)
        {
            Class cl = new Class();


            if (id > 0)
            {
                cl = classRepo.GetById(id);

            }


            return View(cl);
        }

        [HttpPost]
        public ActionResult AddClass(Class cl)
        {
            

            if (ModelState.IsValid)
            {


                try
                {
                    if (cl.ClassId > 0)
                    {

                        string result1 = classRepo.Update(cl);
                        if (result1 == "Success")
                        {
                            ModelState.Clear();
                            TempData["Success"] = "Updated successfully...";
                        }
                        else
                        {
                            TempData["Error"] = "Failed to update...";
                        }

                        string result2 = feestructrepo.UpdateById(cl);
                        if (result2 == "Success")
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
                        string checkClassNameExists = classRepo.CheckClassName(cl.ClassName);

                        if (checkClassNameExists == "Error")
                        {
                            TempData["Error"] = "This class name already exists";
                        }
                        else
                        {

                            string result1 = classRepo.Create(cl);
                            if (result1 == "Success")
                            {
                                ModelState.Clear();
                                TempData["Success"] = "Created successfully...";
                            }
                            else
                            {
                                TempData["Error"] = "Failed to create...";
                            }


                            string result2 = feestructrepo.CreateById(cl);
                            if (result2 == "Success")
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

            return View(cl);
        }

// use this later - if u need it 


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



        public ActionResult DeleteClass(int id)
        {
            try
            {
                string result = classRepo.Delete(id);
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