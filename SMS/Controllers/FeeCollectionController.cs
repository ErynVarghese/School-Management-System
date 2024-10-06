using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using SMS.Repositories;

namespace SMS.Controllers
{
   
    public class FeeCollectionController : Controller
    {
        // GET: FeeCollection

        FeeCollectionRepo feecolrepo = new FeeCollectionRepo();
        FeeStructureRepo feestructrepo = new FeeStructureRepo();

        ClassRepo classRepo = new ClassRepo();
        StudentRepo studRepo = new StudentRepo();

        List<Class> classlist = new List<Class>();

        List<Student> studlist = new List<Student>();



        public FeeCollectionController()
        {

            classlist = classRepo.GetAll();
            ViewBag.ClassList = classlist;

            studlist = studRepo.GetAll();
            ViewBag.StudentList = studlist;

            ViewBag.MaxFeeColId = feecolrepo.GetNextId();

        }


        public ActionResult FeeColList()
        {
            List<FeeCollection> fList = new List<FeeCollection>();

            fList = feecolrepo.GetAll();

            return View(fList);
        }


        [HttpGet]
        public ActionResult AddFeeCol(int id = 0)
        {
            FeeCollection feecol = new FeeCollection();


            if (id > 0)
            {
                feecol = feecolrepo.GetById(id);

            }


            return View(feecol);
        }

        [HttpPost]
        public ActionResult AddFeeCol(FeeCollection feecol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (feecol.FeeColId > 0)
                    {
                        string result = feecolrepo.Update(feecol);
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
                        string result = feecolrepo.Create(feecol);
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

            return View(feecol);
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


        public JsonResult GetInstallations(int classId)
        {
            // Replace this with your actual database fetching logic
            // Assuming you are fetching three decimal values related to classId from your DB
            List<decimal> installations = new List<decimal>();

            var installation1 = feestructrepo.GetInstallation1ByClassId(classId); // Fetch from DB
            var installation2 = feestructrepo.GetInstallation2ByClassId(classId); // Fetch from DB
            var installation3 = feestructrepo.GetInstallation3ByClassId(classId); // Fetch from DB

            installations.Add(installation1);
            installations.Add(installation2);
            installations.Add(installation3);

            // Return the list of installation values as JSON
            return Json(installations,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstallationsByStudId(int studid)
        {
            // Replace this with your actual database fetching logic
            // Assuming you are fetching three decimal values related to classId from your DB
            List<bool> installations = new List<bool>();

            bool installation1 = feecolrepo.GetInstallation1ByStudId(studid).ToLower() == "true"; // Fetch from DB
            bool installation2 = feecolrepo.GetInstallation2ByStudId(studid).ToLower() == "true"; // Fetch from DB
            bool installation3 = feecolrepo.GetInstallation3ByStudId(studid).ToLower() == "true"; // Fetch from DB


            installations.Add(installation1);
            installations.Add(installation2);
            installations.Add(installation3);

            // Return the list of installation values as JSON
            return Json(installations,JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteFeeCol(int id)
        {
            try
            {
                string result = feecolrepo.Delete(id);
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