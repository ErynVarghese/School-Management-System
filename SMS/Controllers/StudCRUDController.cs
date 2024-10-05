using SMS.Models;
using SMS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class StudCRUDController : Controller
    {

        StudentRepo studrepo = new StudentRepo();
        ClassRepo classRepo = new ClassRepo();
        SectionRepo sectionRepo = new SectionRepo();
        FeeStructureRepo feestructrepo = new FeeStructureRepo();


        List<Class> classlist = new List<Class>();

        List<Section> sectionlist = new List<Section>();

        public StudCRUDController()
        {
            classlist = classRepo.GetAll();
            ViewBag.ClassList = classlist;

            sectionlist = sectionRepo.GetAll();
            ViewBag.SectionList = sectionlist;

            ViewBag.MaxStudId = studrepo.GetNextId();

        }


        public ActionResult StudList(string searchedname)
        {
            List<Student> sList = new List<Student>();

            sList = studrepo.GetAll();

            return View(sList);
        }


        [HttpGet]
        public ActionResult AddStud(int id = 0)
        {
            Student stud = new Student();


            if (id > 0)
            {
              stud = studrepo.GetById(id);

            } 


            return View(stud);
        }

        [HttpPost]
        public ActionResult AddStud(Student stud)
        {
            List<Student> sList = new List<Student>();

            if (ModelState.IsValid)
            {


                try
                {
                    if (stud.StudentId > 0)
                    {
                        string result = studrepo.Update(stud);
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
                        string result = studrepo.Create(stud);
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

            return View(stud);
    }



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



        public ActionResult DeleteStud(int id)
        {
            try
            {
                string result = studrepo.Delete(id);
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