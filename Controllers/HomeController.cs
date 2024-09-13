using certificate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace certificate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender; 
       
        studentContext _studentContext;
        IWebHostEnvironment hostingenvironment;

        public HomeController(ILogger<HomeController> logger, studentContext studentContext, IWebHostEnvironment hc, IEmailSender emailSender)
        {

            _logger = logger;
            this._emailSender = emailSender;
            _studentContext = studentContext;
            hostingenvironment = hc;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Career()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(login login)
        {
            // Hardcoded credentials for demonstration purposes
            string Username = "admin";
            string Password = "password";

            if (login.Username == Username && login.Password == Password)
            {
                // Successful login
                // You can set a cookie or session here to remember the user
                return View("addstudents");
            }
            else
            {
                // Invalid login
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }
        }

        [HttpGet]
        public IActionResult UpdateCert()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateCert(updatecertModel uc)
        {
            try
            {
                var matchingStudents = _studentContext.students.Where(s => s.SId == uc.SId && s.SName == uc.SName);

                if (matchingStudents.Any())
                {
                    foreach (var existingStudent in matchingStudents)
                    {
                        if (uc.Scertificate != null)
                        {
                            string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "certificates");

                            if (!Directory.Exists(uploadFolder))
                            {
                                Directory.CreateDirectory(uploadFolder);
                            }

                            string fileName = Guid.NewGuid().ToString()+"_"+uc.Scertificate.FileName;
                            string filePath = Path.Combine(uploadFolder, fileName);



                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                uc.Scertificate.CopyTo(fileStream);
                            }

                            existingStudent.Scertificate = filePath;
                        }

                        _studentContext.Update(existingStudent);
                    }

                    _studentContext.SaveChanges();

                    ViewBag.Success = "Record(s) Updated";
                }
                else
                {
                    ViewBag.Error = "No student found with the provided ID and name.";
                }
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Error = "An error occurred while updating the record(s). Please try again later.";
                
                _logger.LogError(ex, "Error occurred during database update.");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred. Please try again later.";
              
                _logger.LogError(ex, "Unexpected error occurred during file upload.");
            }

            
            return View();
        }

        public IActionResult studentInfo()
        {
            var model = _studentContext.students.ToList(); // Assuming Students is your DbSet in DbContext
            return View(model);
        }
        [HttpGet]
        public IActionResult addstudents()
        {
            return View();
        }

        [HttpPost]
        public IActionResult addstudents(studentmodel sm)
        {
            try
            {
                student st = new student()
                {
                    SId = sm.SId,
                    SName = sm.SName,
                    Sdob = sm.Sdob,
                    mobileNo = sm.mobileNo,
                    SEmail = sm.SEmail,
                    SAddress = sm.SAddress,
                    Scertificate = sm.SId,
                    Scourse = sm.Scourse,
                    Sgender = sm.Sgender,
                };
                _studentContext.students.Add(st);
                _studentContext.SaveChanges();
                ViewBag.success = "Record Added";

                return View();
            }
            catch (Exception ex) 
            {
                ViewBag.error = "Invalid input. Please check the form for errors.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult certificateImg()
        {
            return View();
        }

        [HttpPost]
        public IActionResult certificateImg(studentmodel model)
        {
            try
            {
                // Retrieve the student from the database based on ID and Name
                var matchingStudent = _studentContext.students.FirstOrDefault(s => s.SId == model.SId && s.SName == model.SName);

                if (matchingStudent != null)
                {
                    // Retrieve the certificate image base64 string
                    string certificateImageBase64 = GetCertificateImageBase64(matchingStudent.Scertificate);

                    // Create a view model to pass student information and certificate image to the view
                    studentmodel studentmodel = new studentmodel

                    {
                        SId = matchingStudent.SId,
                        SName = matchingStudent.SName,
                        Sdob = matchingStudent.Sdob,
                        Sgender=matchingStudent.Sgender,
                        mobileNo = matchingStudent.mobileNo,
                        SEmail = matchingStudent.SEmail,
                        SAddress = matchingStudent.SAddress,
                        Scourse=matchingStudent.SId,
                        Scertificate = certificateImageBase64
                    };

                    // Pass the view model to the view
                    return View("Details", studentmodel);
                    return RedirectToAction("Details");
                }
                else
                {
                    ViewBag.Error = "No student found with the provided ID and name.";
                    return View(); // Return the same view if no student found
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while retrieving student information. Please try again later.";
                _logger.LogError(ex, "Error occurred during student retrieval.");
                return View(); // Return the same view in case of an error
            }
        }

                private string GetCertificateImageBase64(string certificateFileName)
                {
                if (string.IsNullOrEmpty(certificateFileName))
                 {
                   return null;
                 }

                 try
                 {
                   string certificateFilePath = Path.Combine("wwwroot", "certificates", certificateFileName);

                  if (System.IO.File.Exists(certificateFilePath))
                 {
                  byte[] certificateBytes = System.IO.File.ReadAllBytes(certificateFilePath);
                   return Convert.ToBase64String(certificateBytes);
                 }
                  else
                 {
                  // Certificate file not found
                  return null;
                   }
                   }
                    catch (Exception ex)
                  {
                    // Handle any exceptions, e.g., file not found, access denied, etc.
                   // Log the exception
                   _logger.LogError(ex, "Error occurred while retrieving certificate image.");
                     return null;
                  }
                 }


        [HttpGet]
        public IActionResult Certificate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Certificate(studentmodel model)
        {
            try
            {
                // Retrieve the student from the database based on ID and Name
                var matchingStudent = _studentContext.students.FirstOrDefault(s => s.SId == model.SId && s.SName == model.SName);

                if (matchingStudent != null)
                {
                  
                    studentmodel studentmodel = new studentmodel 

                    {
                        SId = matchingStudent.SId,
                        SName = matchingStudent.SName,
                        Sdob = matchingStudent.Sdob,
                        mobileNo = matchingStudent.mobileNo,
                        SEmail = matchingStudent.SEmail,
                        SAddress = matchingStudent.SAddress,
                        Scourse = matchingStudent.Scourse,
                    };

                    // Pass the view model to the view
                    return View("StudentDetails", studentmodel);
                    return RedirectToAction("StudentDetails");
                }
                else
                {
                    ViewBag.Error = "No student found with the provided ID and name.";
                    return View(); // Return the same view if no student found
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while retrieving student information. Please try again later.";
                _logger.LogError(ex, "Error occurred during student retrieval.");
                return View(); // Return the same view in case of an error
            }
        }


        [HttpGet]
        public ActionResult SendMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendMessage(string contact_form_name, string contact_form_email, string contact_form_phone, string contact_form_message, string contact_form_sender_email)
        {
            var receiver = "tritcontact@gmail.com";
            var subject = "A mail from the contact page";
            string message = $"Name: {contact_form_name}\nEmail: {contact_form_email}\nPhone: {contact_form_phone}\nMessage: {contact_form_message}";

            try
            {
                // Send email using the injected IEmailSender
                await _emailSender.SendEmailAsync(receiver, subject, message);

                // Redirect to a thank you page
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email");

                // Set error message
                ViewBag.ErrorMessage = "Failed to send message. Please try again later.";

                // Return error view
                return View("Error");
            }
        }


      


        public IActionResult Details()
        {
            return View();
        }
       

        public IActionResult StudentDetails() { 
            return View(); 
        }

        [HttpPost]
        public IActionResult Logout()
        {
            
            return RedirectToAction("login", "home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}