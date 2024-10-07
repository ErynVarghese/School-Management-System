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

            sList = studrepo.GetAll();
            ViewBag.StudentList = sList;

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
        public JsonResult GetStudentsByClassId(int classId)
        {
            List<Student> sList = studrepo.GetStudentsByClassId(classId);
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

                    List<string> validationErrors = new List<string>();


                    int studentId = 0; 
                    DateTime dob; 
                    int classId = 0;
                    int sectionId = 0;
                    int contactNo = 0;
                    int studentPassword = 0;
                    int studentFee = 0;


                 
                    for (var r = start.Row + 1; r <= end.Row; r++)
                    {
                        string errorMessage = "";

                        // Validate StudentId - no ext - update alt
                        string studentIdStr = workSheet.Cells[r, 1].Value?.ToString();
                        if (string.IsNullOrEmpty(studentIdStr))
                        {
                            errorMessage += $"Row {r}: StudentId cannot be null or empty. ";
                        }
                        else if (!int.TryParse(studentIdStr, out  studentId))
                        {
                            errorMessage += $"Row {r}: Invalid StudentId type. ";
                        }

                        // Validate StudentName - no ext
                        string studentName = workSheet.Cells[r, 2].Value?.ToString();
                        if (string.IsNullOrEmpty(studentName))
                        {
                            errorMessage += $"Row {r}: StudentName cannot be empty. ";
                        }
                        else if (!studentName.All(char.IsLetter)) 
                        {
                            errorMessage += $"Row {r}: StudentName must contain only letters. ";
                        }

                        // Validate DOB - no ext
                        if (!DateTime.TryParse(workSheet.Cells[r, 3].Value?.ToString(), out  dob))
                        {
                            errorMessage += $"Row {r}: Invalid Date of Birth. ";
                        }

                        // Validate ClassId - ext needed (in else)
                        string classIdStr = workSheet.Cells[r, 4].Value?.ToString();
                        if (string.IsNullOrEmpty(classIdStr))
                        {
                            errorMessage += $"Row {r}: ClassId cannot be null or empty. ";
                        }
                        else if (!int.TryParse(classIdStr, out  classId))
                        {
                            errorMessage += $"Row {r}: Invalid ClassId. ";
                        }
                        else
                        {
                            //check if the class id exists 
                            string result = classRepo.CheckClassId(classId);

                            if (result == "Error")
                            {
                                errorMessage += $"Row {r}: ClassId {classId} does not exist. ";
                            }

                        }

                        // Validate SectionId - ext needed (in else)
                        string sectionIdStr = workSheet.Cells[r, 5].Value?.ToString();
                        if (string.IsNullOrEmpty(sectionIdStr))
                        {
                            errorMessage += $"Row {r}: SectionId cannot be null or empty. ";
                        }
                        else if (!int.TryParse(sectionIdStr, out  sectionId))
                        {
                            errorMessage += $"Row {r}: Invalid SectionId. ";
                        }
                        else
                        {
                            string result = sectionRepo.CheckSectionId(sectionId);

                            if (result == "Error")
                            {
                                errorMessage += $"Row {r}: SectionId {sectionId} does not exist. ";
                            }

                        }

                        // Validate FatherName - no ext
                        string fatherName = workSheet.Cells[r, 6].Value?.ToString();
                        if (string.IsNullOrEmpty(fatherName))
                        {
                            errorMessage += $"Row {r}: FatherName cannot be empty. ";
                        }
                        else if (!fatherName.All(char.IsLetter))
                        {
                            errorMessage += $"Row {r}: FatherName must contain only letters. ";
                        }

                        // Validate ContactNo - no ext
                        string contactNoStr = workSheet.Cells[r, 7].Value?.ToString();
                        if (string.IsNullOrEmpty(contactNoStr))
                        {
                            errorMessage += $"Row {r}: Contact Number cannot be null or empty. ";
                        }
                        else if (!int.TryParse(contactNoStr, out  contactNo))
                        {
                            errorMessage += $"Row {r}: Invalid Contact Number. ";
                        }


                        // Validate StudentAddress - no ext
                        string studentAddress = workSheet.Cells[r, 8].Value?.ToString();
                        if (string.IsNullOrEmpty(studentAddress))
                        {
                            errorMessage += $"Row {r}: StudentAddress cannot be empty. ";
                        }
                        else if (!studentAddress.All(char.IsLetter))
                        {
                            errorMessage += $"Row {r}: Student Address must contain only letters. ";
                        }

                        // Validate StudentUsername - no ext
                        string studentUsername = workSheet.Cells[r, 9].Value?.ToString();
                        if (string.IsNullOrEmpty(studentUsername))
                        {
                            errorMessage += $"Row {r}: StudentUsername cannot be empty. ";
                        }
                        else if (!studentUsername.All(char.IsLetter))
                        {
                            errorMessage += $"Row {r}: StudentName must contain only letters. ";
                        }

                        // Validate StudentPassword - no ext

                        string studentPasswordStr = workSheet.Cells[r, 10].Value?.ToString();
                        if (string.IsNullOrEmpty(studentPasswordStr))
                        {
                            errorMessage += $"Row {r}: Student Password cannot be null or empty. ";
                        }
                        else if (!int.TryParse(studentPasswordStr, out  studentPassword))
                        {
                            errorMessage += $"Row {r}: Invalid Student Password. ";
                        }

                        // Validate StudentFee - no ext

                        string studentFeeStr = workSheet.Cells[r, 11].Value?.ToString();
                        if (string.IsNullOrEmpty(studentFeeStr))
                        {
                            errorMessage += $"Row {r}: Student Fee cannot be null or empty. ";
                        }
                        else if (!int.TryParse(studentFeeStr, out  studentFee))
                        {
                            errorMessage += $"Row {r}: Invalid Student Fee. ";
                        }

                    
                        if (string.IsNullOrEmpty(errorMessage))
                        {
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
                    string result = studrepo.StudentExists(stud.StudentId);

                    if (result == "Exists")
                    {
                        string updateresult = studrepo.Update(stud);

                      
                        if (updateresult != "Success")
                        {
                            failedCount++;
                            continue; 
                        }

                    }
                    else
                    {
                        string createresult = studrepo.Create(stud);

                      
                        if (createresult != "Success")
                        {
                            failedCount++;
                            continue; 
                        }
                    }
                 

                }

            
                if (failedCount > 0)
                {
                    TempData["Error"] = $"{failedCount} student record(s) failed to update/create.";
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


        [HttpGet]
        public ActionResult UploadImage()
        {

            return View(); 
        }



        [HttpPost]
       
        public ActionResult UploadImage(Student student, System.Web.HttpPostedFileBase fileUpload)
        {
           
             
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                   
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string uploadPath = Path.Combine(Server.MapPath("~/UploadedImages"));

             
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);

                // Check if the file already exists
                if (System.IO.File.Exists(filePath))
                {
                   
                    TempData["Error"] = "A file with the same name already exists. Please rename the file and try again.";
                    return RedirectToAction("UploadImage"); 
                }

                try
                    {
                        fileUpload.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Error saving the image: " + ex.Message;
                        return View(student);
                    }


                    
                    StudentRepo studentRepo = new StudentRepo(); 
                    string result = studentRepo.AddImage(student.StudentId, fileName);

                    // Check the result and handle accordingly
                    if (result == "Success")
                    {
                          TempData["Success"] = "uploaded successfully!";
                 
                        return RedirectToAction("UploadImage");
                    }
                    else
                    {
                           TempData["Error"] = "Failed to update the student's image.";
                    }
                }
                else
                {
                    TempData["Error"] = "Upload a valid image";
                }
            

            return View(student);
        }

  


            [HttpGet]
            public ActionResult ViewStudentImage()
            {
                return View(new Student()); 
            }


            [HttpPost]
            
            public ActionResult ViewStudentImage(int studentId)
            {
             
                string imageName = studrepo.GetStudentImage(studentId);

                if (!string.IsNullOrEmpty(imageName) && (imageName != "not"))
                {
                    ViewBag.ImagePath = Url.Content("~/UploadedImages/" + imageName); 
                } else if (!string.IsNullOrEmpty(imageName) && (imageName == "not"))
            {
                TempData["Error"] = "The student doesnt have an image uploaded yet!";
            }
                else
                {
                TempData["Error"] = "No image found for this Student ID.";
                }

                return View(new Student { StudentId = studentId }); 
            }
        



    }

}
