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


        public FeeCollectionController()
        {

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