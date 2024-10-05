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
    public class SectionController : Controller
    {
        // GET: Section

        SectionRepo sectionrepo = new SectionRepo();
        ClassRepo classRepo = new ClassRepo();


        List<Class> classlist = new List<Class>();

        public SectionController()
        {
            classlist = classRepo.GetAll();
            ViewBag.ClassList = classlist;

            ViewBag.MaxSectionId = sectionrepo.GetNextId();

        }


        public ActionResult SectionList()
        {
            List<Section> sList = new List<Section>();

            sList = sectionrepo.GetAll();

            return View(sList);
        }


        [HttpGet]
        public ActionResult AddSection(int id = 0)
        {
            Section section = new Section();

            if (id > 0)
            {
                section = sectionrepo.GetById(id);

            }

            return View(section);
        }

        [HttpPost]
        public ActionResult AddSection(Section section)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    string checkSectionNameExists = sectionrepo.CheckSectionName(section);

                    if (checkSectionNameExists == "Error")
                    {
                        TempData["Error"] = "This section name already exists";
                    }
                    else
                    {
                        if (section.SectionId > 0)
                        {
                        string result = sectionrepo.Update(section);
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
                            string result = sectionrepo.Create(section);
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

            return View(section);
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

        public ActionResult DeleteSection(int id)
        {
            try
            {
                string result = sectionrepo.Delete(id);
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