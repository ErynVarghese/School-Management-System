using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace SMS.Controllers
{
    public class SectionController : Controller
    {
        // GET: Section
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sms"].ConnectionString);
        SqlCommand cmd = null;

        public ActionResult SectionList(string searchedname)
        {
            List<Section> sectionlist = new List<Section>();
            List<ById> enlist = new List<ById>();

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_section", con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", 0);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {

                    Section s = new Section();

                    s.SectionId = sdr.GetInt32(0);
                    s.SectionName = sdr.GetString(1);
                    s.ClassId = sdr.GetInt32(2);
                    s.TotalSpace = sdr.GetInt32(3);


                    sectionlist.Add(s);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
                con.Close();
            }
            finally
            {
                con.Close();
            }

            try
            {
                con.Open();
                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 5);

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    ById en = new ById();
                    en.Id = sdr.GetInt32(0);
                    en.Name = sdr.GetString(1);
                    enlist.Add(en);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching section names. " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            ViewBag.SectionList = new SelectList(enlist, "Id", "Name");

           


            if (searchedname != null)
            {
                sectionlist = sectionlist.Where(s => s.ClassId.ToString().Contains(searchedname.ToString())).ToList();

            }

            return View(sectionlist);
        }

        [HttpGet]
        public ActionResult AddSection(int id = 0)
        {
            Section s = new Section();
            try
            {
                con.Open();

                
                List<Class> classList = new List<Class>();

                cmd = new SqlCommand("proc_class", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", 5);

                SqlDataReader classReader = cmd.ExecuteReader();

                while (classReader.Read())
                {
                    classList.Add(new Class
                    {
                        ClassId = classReader.GetInt32(0),
                        ClassName = classReader.GetString(1)
                    });
                }

                con.Close();
                ViewBag.ClassList = new SelectList(classList, "ClassId", "ClassName");

                if (id > 0)
                {
                    con.Open();
                    cmd = new SqlCommand("proc_section", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", id);
                    cmd.Parameters.AddWithValue("@Mode", 1);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        s.SectionId = sdr.GetInt32(0);
                        s.SectionName = sdr.GetString(1);
                        s.ClassId = sdr.GetInt32(2);
                        s.TotalSpace = sdr.GetInt32(3);
                    }

                    con.Close();
                    
                } else
                {
                    con.Open();
                   
                    cmd = new SqlCommand("proc_section", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 8);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        ViewBag.MaxSectionId = sdr.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return View(s);
        }

        [HttpPost]
        public ActionResult AddSection(Section s)
        {
            try
            {
             
                if (ModelState.IsValid)
                {
                    con.Open();
                    // Repopulate Class List for DropDownList
                    List<Class> classList = new List<Class>();
                    cmd = new SqlCommand("proc_class", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", 8);

                    SqlDataReader classReader = cmd.ExecuteReader();
                    while (classReader.Read())
                    {
                        classList.Add(new Class
                        {
                            ClassId = classReader.GetInt32(0),
                            ClassName = classReader.GetString(1)
                        });
                    }
                    
                    con.Close();
                    ViewBag.ClassList = new SelectList(classList, "ClassId", "ClassName");


                    // Proceed to Insert or Update
                    if (s.SectionId > 0)
                    {
                        con.Open();
                        // Check if section name already exists for the selected class
                        SqlCommand checkCmd = new SqlCommand("proc_section", con);
                        checkCmd.CommandType = CommandType.StoredProcedure;
                        checkCmd.Parameters.AddWithValue("@SectionName", s.SectionName);
                        checkCmd.Parameters.AddWithValue("@ClassId", s.ClassId);
                        checkCmd.Parameters.AddWithValue("@SectionId", s.SectionId);
                        checkCmd.Parameters.AddWithValue("@mode", 9);


                        SqlDataReader reader = checkCmd.ExecuteReader();
                        bool sectionExists = false;
                        if (reader.Read())
                        {
                            sectionExists = reader.GetInt32(0) > 0;
                        }

                        
                        con.Close();

                        if (sectionExists)
                        {
                            TempData["Error"] = "This section already exists for the selected class!";
                            return View(s);
                        }

                        con.Open();

                        cmd = new SqlCommand("proc_section", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SectionId", s.SectionId);
                        cmd.Parameters.AddWithValue("@SectionName", s.SectionName);
                        cmd.Parameters.AddWithValue("@ClassId", s.ClassId);
                        cmd.Parameters.AddWithValue("@TotalSpace", s.TotalSpace);
                        cmd.Parameters.AddWithValue("@Mode", 2);

                        int status = cmd.ExecuteNonQuery();


                        if (status < 0)
                        {
                            TempData["Success"] = "Section details edited successfully";
                            return RedirectToAction("SectionList", "Section");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to edit!";
                        }
                    }
                    else
                    {
                        con.Open();

                     
                        SqlCommand checkCmd = new SqlCommand("proc_section", con);
                        checkCmd.CommandType = CommandType.StoredProcedure;
                        checkCmd.Parameters.AddWithValue("@SectionName", s.SectionName);
                        checkCmd.Parameters.AddWithValue("@ClassId", s.ClassId);
                        checkCmd.Parameters.AddWithValue("@mode", 7);


                        SqlDataReader reader = checkCmd.ExecuteReader();
                        bool sectionExists = false;
                        if (reader.Read())
                        {
                            sectionExists = reader.GetInt32(0) > 0;
                        }

                        con.Close();

                        if (sectionExists)
                        {
                            TempData["Error"] = "This section already exists for the selected class!";
                            return View(s);
                        }

                        con.Open();
                        cmd = new SqlCommand("proc_section", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SectionName", s.SectionName);
                        cmd.Parameters.AddWithValue("@ClassId", s.ClassId);
                        cmd.Parameters.AddWithValue("@TotalSpace", s.TotalSpace);
                        cmd.Parameters.AddWithValue("@Mode", 3);

                        int status = cmd.ExecuteNonQuery();
                        if (status < 0)
                        {
                            TempData["Success"] = "Section details created successfully";
                            return RedirectToAction("SectionList", "Section");
                        }
                        else
                        {
                            TempData["Error"] = "Failed to create!";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Fill out all the details.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in processing the request. " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return View(s);
        }




        public ActionResult DeleteSection(int id)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("proc_section", con);


                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@SectionId", id);
                cmd.Parameters.AddWithValue("@Mode", 4);

                int status = cmd.ExecuteNonQuery();

                if (status < 0)
                {
                    TempData["Success"] = "Record deleted successfully";

                }
                else
                {
                    TempData["Error"] = "Failed to delete!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error in Processing the request. " + ex.Message;
                con.Close();
            }
            finally
            {
                con.Close();
            }

            return RedirectToAction("SectionList", "Section");

        }



    }
}