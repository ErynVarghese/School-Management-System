using SMS.Models;
using SMS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;

using System.IO;

namespace SMS.Controllers
{
    public class StudCRUDController : Controller
    {

        StudentRepo studrepo = new StudentRepo();
        ClassRepo classRepo = new ClassRepo();
        SectionRepo sectionRepo = new SectionRepo();
        FeeStructureRepo feestructrepo = new FeeStructureRepo();


        List<Class> classlist = new List<Class>();

        List<Student> sList = new List<Student>();

        List<Section> sectionlist = new List<Section>();

        public StudCRUDController()
        {
            classlist = classRepo.GetAll();
            ViewBag.ClassList = classlist;

            sectionlist = sectionRepo.GetAll();
            ViewBag.SectionList = sectionlist;

            ViewBag.MaxStudId = studrepo.GetNextId();

        }


        public ActionResult StudList()
        {


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


        [HttpGet]
        public ActionResult UploadStudent()
        {
            return View();
        }

        [HttpPost]

        public ActionResult UploadStudent(System.Web.HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null || fileUpload.ContentLength == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return RedirectToAction("UploadStudent");
            }

            using (var stream = new MemoryStream())
            {
                fileUpload.InputStream.CopyTo(stream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var excel = new ExcelPackage(stream))
                {
                    var workSheet = excel.Workbook.Worksheets.FirstOrDefault();

                    if (workSheet == null)
                    {
                        TempData["Error"] = "The uploaded file does not contain any worksheet.";
                        return RedirectToAction("UploadStudent");
                    }

                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;

                    //Iterate through the worksheet cells and read data

                    //for (var r = start.Row + 1; r <= end.Row; r++)
                    //{
                    //    string CorrectType;
                    //    string Exists;



                    //    Student student = new Student
                    //    {
                    //        StudentId = Convert.ToInt32(workSheet.Cells[r, 1].Value?.ToString()),
                    //        StudentName = workSheet.Cells[r, 2].Value?.ToString(),
                    //        DOB = Convert.ToDateTime(workSheet.Cells[r, 3].Value?.ToString()),
                    //        ClassId = Convert.ToInt32(workSheet.Cells[r, 4].Value?.ToString()),
                    //        SectionId = Convert.ToInt32(workSheet.Cells[r, 5].Value?.ToString()),
                    //        FatherName = workSheet.Cells[r, 6].Value?.ToString(),
                    //        ContactNo = Convert.ToInt32(workSheet.Cells[r, 7].Value?.ToString()),
                    //        StudentAddress = workSheet.Cells[r, 8].Value?.ToString(),
                    //        StudentUsername = workSheet.Cells[r, 9].Value?.ToString(),
                    //        StudentPassword = Convert.ToInt32(workSheet.Cells[r, 10].Value?.ToString()),
                    //        StudentFee = Convert.ToInt32(workSheet.Cells[r, 11].Value?.ToString())
                    //    };

                    //    sList.Add(student);
                    //}

                    List<string> validationErrors = new List<string>();

                    //Iterate through the worksheet cells and read data
                    for (var r = start.Row + 1; r <= end.Row; r++)
                    {
                        string errorMessage = "";

                        // Validate StudentId
                        string cellValue = workSheet.Cells[r, 1].Value?.ToString(); 

                        
                        if (string.IsNullOrEmpty(cellValue))
                        {
                            errorMessage += $"Row {r}: StudentId is required. ";
                        }
                        else if (!int.TryParse(cellValue, out int studentId))
                        {
                            errorMessage += $"Row {r}: Invalid StudentId type. ";
                        }

                        // Validate StudentName
                        string studentName = workSheet.Cells[r, 2].Value?.ToString();
                        if (string.IsNullOrEmpty(studentName))
                        {
                            errorMessage += $"Row {r}: StudentName cannot be empty. ";
                        }

                        // Validate DOB
                        if (!DateTime.TryParse(workSheet.Cells[r, 3].Value?.ToString(), out DateTime dob))
                        {
                            errorMessage += $"Row {r}: Invalid Date of Birth. ";
                        }

                        // Validate ClassId
                        if (!int.TryParse(workSheet.Cells[r, 4].Value?.ToString(), out int classId))
                        {
                            errorMessage += $"Row {r}: Invalid ClassId. ";
                        }
                        else
                        {
                            // Check if ClassId exists in your database (pseudo code)
                            // if (!classRepo.Exists(classId)) {
                            //     errorMessage += $"Row {r}: ClassId {classId} does not exist. ";
                            // }
                        }

                        // Validate SectionId
                        if (!int.TryParse(workSheet.Cells[r, 5].Value?.ToString(), out int sectionId))
                        {
                            errorMessage += $"Row {r}: Invalid SectionId. ";
                        }
                        else
                        {
                            // Check if SectionId exists in your database (pseudo code)
                            // if (!sectionRepo.Exists(sectionId)) {
                            //     errorMessage += $"Row {r}: SectionId {sectionId} does not exist. ";
                            // }
                        }

                        // Validate FatherName
                        string fatherName = workSheet.Cells[r, 6].Value?.ToString();
                        if (string.IsNullOrEmpty(fatherName))
                        {
                            errorMessage += $"Row {r}: FatherName cannot be empty. ";
                        }

                        // Validate ContactNo
                        if (!int.TryParse(workSheet.Cells[r, 7].Value?.ToString(), out int contactNo))
                        {
                            errorMessage += $"Row {r}: Invalid Contact Number. ";
                        }

                        // Validate StudentAddress
                        string studentAddress = workSheet.Cells[r, 8].Value?.ToString();
                        if (string.IsNullOrEmpty(studentAddress))
                        {
                            errorMessage += $"Row {r}: StudentAddress cannot be empty. ";
                        }

                        // Validate StudentUsername
                        string studentUsername = workSheet.Cells[r, 9].Value?.ToString();
                        if (string.IsNullOrEmpty(studentUsername))
                        {
                            errorMessage += $"Row {r}: StudentUsername cannot be empty. ";
                        }

                        // Validate StudentPassword
                        if (!int.TryParse(workSheet.Cells[r, 10].Value?.ToString(), out int studentPassword))
                        {
                            errorMessage += $"Row {r}: Invalid Student Password. ";
                        }

                        // Validate StudentFee
                        if (!int.TryParse(workSheet.Cells[r, 11].Value?.ToString(), out int studentFee))
                        {
                            errorMessage += $"Row {r}: Invalid Student Fee. ";
                        }

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            // If there are no validation errors, create the student
                            Student student = new Student
                            {
                                StudentId = studentId,
                                StudentName = studentName,
                                DOB = dob,
                                ClassId = classId,
                                SectionId = sectionId,
                                FatherName = fatherName,
                                ContactNo = contactNo,
                                StudentAddress = studentAddress,
                                StudentUsername = studentUsername,
                                StudentPassword = studentPassword,
                                StudentFee = studentFee
                            };

                            sList.Add(student);
                        }
                        else
                        {
                            validationErrors.Add(errorMessage);
                        }
                    }

                    // After the loop, you can handle validation errors as needed
                    if (validationErrors.Any())
                    {
                        TempData["Error"] = string.Join("<br>", validationErrors);
                    }


                }
            }
           
            try
            {
                int failedCount = 0;

                foreach (Student stud in sList)
                {
                    // Attempt to create the student record
                    string result = studrepo.Create(stud);

                    // Skip if the creation failed
                    if (result != "Success")
                    {
                        failedCount++;
                        continue; // Skip this record and continue with the next
                    }
                }

                // Provide feedback based on the number of failed records
                if (failedCount > 0)
                {
                    TempData["Error"] = $"{failedCount} student record(s) failed to create.";
                }
                else
                {
                    TempData["Success"] = $"{sList.Count} student records uploaded successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during processing." + ex.Message;
            }

            return RedirectToAction("UploadStudent");
        }
    
    }

}
