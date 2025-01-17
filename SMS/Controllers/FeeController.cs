﻿using System;
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
    public class FeeController : Controller
    {
        // GET: Fee

        FeeStructureRepo feestructrepo = new FeeStructureRepo();
        ClassRepo classRepo = new ClassRepo();

        List<Class> classlist = new List<Class>();
        List<FeeStructure> feestruct = new List<FeeStructure>();

        public FeeController()
        {
            classlist = classRepo.GetAll();
            ViewBag.ClassList = classlist;


            ViewBag.MaxFeeStructId = feestructrepo.GetNextId();

        }

        [HttpGet]
        public ActionResult FeeStructList()
        {
            List<FeeStructure> feeList = new List<FeeStructure>();

            feeList = feestructrepo.GetAll();

            return View(feeList);
        }

        [HttpPost]

        public ActionResult FeeStructList(int drpClass = 0)
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Authentication", "Login");
            }
            else
            {
                Session["Searched"] = true;

                List<FeeStructure> feeList = new List<FeeStructure>();

                feeList = feestructrepo.GetAll();


                if (drpClass > 0)
                {
                    feeList = feeList.Where(c => c.ClassId == drpClass).ToList();
                }

                return View(feeList);
            }
        }

    

    [HttpGet]
        public ActionResult AddFeeStruct(int id = 0)
        {
            FeeStructure feestruct = new FeeStructure();


            if (id > 0)
            {
                feestruct = feestructrepo.GetById(id);

            }


            return View(feestruct);
        }

        [HttpPost]
        public ActionResult AddFeeStruct(FeeStructure feeStruct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (feeStruct.FeeId > 0)
                    {
                        string result = feestructrepo.Update(feeStruct);
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
                        string result = feestructrepo.Create(feeStruct);
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

            return View(feeStruct);
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

        public ActionResult DeleteFeeStruct(int id)
        {
            try
            {
                string result = feestructrepo.Delete(id);
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