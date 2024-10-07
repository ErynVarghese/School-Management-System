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
using OfficeOpenXml; // Make sure to include this namespace
using System.IO;


namespace SMS.Controllers
{
    public class EmpCRUDController : Controller
    {

        EmployerRepo emprepo = new EmployerRepo();
        DepartmentRepo deptrepo = new DepartmentRepo();


        List<Department> deptlist = new List<Department>();

        List<Employer> emplist = new List<Employer>();

        public EmpCRUDController()
        {
            deptlist = deptrepo.GetAll();
            ViewBag.DeptList = deptlist;

            emplist = emprepo.GetAll();
            ViewBag.EmpList = emplist;

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


        public ActionResult ExportToExcel()
        {

            if (emplist == null || !emplist.Any())
            {
                TempData["Error"] = "No employee data available to export.";
                return RedirectToAction("EmpList");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            try
            {


                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Employees");
                    workSheet.Cells[1, 1].Value = "Employee ID";
                    workSheet.Cells[1, 2].Value = "Employee Name";
                    workSheet.Cells[1, 3].Value = "Contact Number";
                    workSheet.Cells[1, 4].Value = "Email";
                    workSheet.Cells[1, 5].Value = "Date of Birth";
                    workSheet.Cells[1, 6].Value = "Department ID";
                    workSheet.Cells[1, 7].Value = "Date of Joining";

                    int row = 2;

                    foreach (var emp in emplist)
                    {
                        workSheet.Cells[row, 1].Value = emp.EmpId;
                        workSheet.Cells[row, 2].Value = emp.EmpName;
                        workSheet.Cells[row, 3].Value = emp.ContactNo;
                        workSheet.Cells[row, 4].Value = emp.Email;
                        workSheet.Cells[row, 5].Value = emp.DOB?.ToString("dd-MM-yyyy");
                        workSheet.Cells[row, 6].Value = emp.DeptId;
                        workSheet.Cells[row, 7].Value = emp.DOJ?.ToString("dd-MM-yyyy");
                        row++;
                    }

                    workSheet.Cells.AutoFitColumns();


                    using (var stream = new MemoryStream())
                    {
                        excel.SaveAs(stream);
                        var fileName = "EmployeeList.xlsx";
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occured" + ex.Message;
                return RedirectToAction("EmpList");
            }
        }


    }
}

