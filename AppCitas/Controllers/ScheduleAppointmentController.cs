using AppCitas.Data;
using AppCitas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppCitas.Controllers
{
    public class ScheduleAppointmentController : Controller
    {
        //URL donde se hubica el API 
        string BaseUrl = "https://localhost:44315/";

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public ScheduleAppointmentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)//ApplicationDbContext context
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/ScheduleAppointment
        public async Task<ActionResult> Index()
        {
            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var administrador = user.Administrador;
                TempData["RUN"] = user.RUN;

                if (administrador == true)
                {
                    TempData["hablitarUser"] = "1";
                    
                }
            }
            //-------------------------------------------------------------------------------------------------------------

            List<ScheduleAppointment> scheduleAppointmentList = new List<ScheduleAppointment>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los ScheduleAppointment uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/ScheduleAppointments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    scheduleAppointmentList = JsonConvert.DeserializeObject<List<ScheduleAppointment>>(appResponse);
                }

                //Muestra la lista de todos los scheduleAppointmentList               
                return View(scheduleAppointmentList);
            }
        }

        //Crear nuevo ScheduleAppointment       
        public async Task<ActionResult> Create()
        {
            List<string> listEstado = new List<string>()
            {
                "proposed",
                "pending",
                "booked",
                "arrived",
                "fulfilled",
                "cancelled",
                "cancelled",
                "entered-in-error",
                "checked-in",
                "waitlist"
            };

            IEnumerable<SelectListItem> item1 = listEstado.Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() });

            ViewBag.listaEstado = item1;

            //-------------------------------------------------------------------------------------------------------------

            List<Appointments> appointmentsList = new List<Appointments>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Appointments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    appointmentsList = JsonConvert.DeserializeObject<List<Appointments>>(appResponse);
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = appointmentsList.Select(c => new SelectListItem { Value = c.IdAppointments.ToString(), Text = c.DescriptionApp });

            ViewBag.cursoLista = item;
            //--------------------------------------------------------------------------------------------------------------------------------

            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var administrador = user.Administrador;

                TempData["nombres"] = user.Nombres + " " + user.Apellidos;
                TempData["RUN"] = user.RUN;

                if (administrador == true)
                {
                    TempData["hablitarUser"] = "1";
                }
            }
            //-------------------------------------------------------------------------------------------------------------

            return View();
        }

        // POST: api/ScheduleAppointment
        [HttpPost]
        public async Task<ActionResult> Create(ScheduleAppointment scheduleAppointmentList)
        {           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl + "api/ScheduleAppointments");
                var postTask = client.PostAsJsonAsync<ScheduleAppointment>("ScheduleAppointments", scheduleAppointmentList);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Error, contacta al administrador.");


            //-------------------------------------------------------------------------------------------------------------

            List<Appointments> appointmentsList = new List<Appointments>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Appointments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    appointmentsList = JsonConvert.DeserializeObject<List<Appointments>>(appResponse);
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = appointmentsList.Select(c => new SelectListItem { Value = c.IdAppointments.ToString(), Text = c.DescriptionApp });

            ViewBag.cursoLista = item;
            //--------------------------------------------------------------------------------------------------------------------------------


            TempData["RUN"] = "";
            TempData["nombres"] = "";
            return View(scheduleAppointmentList);
        }

        //Modificar usuario        
        public async Task<ActionResult> Edit(int id)
        {
            //-------------------------------------------------------------------------------------------------------------

            List<Appointments> appointmentsList = new List<Appointments>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Appointments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    appointmentsList = JsonConvert.DeserializeObject<List<Appointments>>(appResponse);
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = appointmentsList.Select(c => new SelectListItem { Value = c.IdAppointments.ToString(), Text = c.DescriptionApp });

            ViewBag.cursoLista = item;
            //--------------------------------------------------------------------------------------------------------------------------------


            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var administrador = user.Administrador;

                if (administrador == true)
                {
                    TempData["hablitarUser"] = "1";
                }
            }
            //-------------------------------------------------------------------------------------------------------------

            //Models
            ScheduleAppointment scheduleAppointmentList = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/ScheduleAppointments/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ScheduleAppointment>();
                    readTask.Wait();
                    scheduleAppointmentList = readTask.Result;
                }
            }

            return View(scheduleAppointmentList);
        }

        // PUT: api/ScheduleAppointment/5
        [HttpPost]
        public async Task<ActionResult> Edit(ScheduleAppointment scheduleAppointmentList)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //HTTP POST
                var putTask = client.PutAsJsonAsync($"api/ScheduleAppointments/{scheduleAppointmentList.IdScheduleAppointment}", scheduleAppointmentList);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            //-------------------------------------------------------------------------------------------------------------

            List<Appointments> appointmentsList = new List<Appointments>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Appointments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    appointmentsList = JsonConvert.DeserializeObject<List<Appointments>>(appResponse);
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = appointmentsList.Select(c => new SelectListItem { Value = c.IdAppointments.ToString(), Text = c.DescriptionApp });

            ViewBag.cursoLista = item;
            //--------------------------------------------------------------------------------------------------------------------------------


            return View(scheduleAppointmentList);
        }

        //Borrar Citas
        public async Task<ActionResult> Delete(int Id)
        {
            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var administrador = user.Administrador;

                if (administrador == true)
                {
                    TempData["hablitarUser"] = "1";
                }
            }
            //-------------------------------------------------------------------------------------------------------------

            //Models
            ScheduleAppointment scheduleAppointmentList = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Get
                var responseTask = client.GetAsync("api/ScheduleAppointments/" + Id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ScheduleAppointment>();
                    readTask.Wait();
                    scheduleAppointmentList = readTask.Result;
                }
            }

            return View(scheduleAppointmentList);
        }

        // DELETE: api/ScheduleAppointment/5
        [HttpPost]
        public ActionResult Delete(ScheduleAppointment scheduleAppointmentList, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Delete
                var deleteTask = client.DeleteAsync($"api/ScheduleAppointments/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(scheduleAppointmentList);
        }

        //Detalles Citas
        public async Task<ActionResult> Details(int Id)
        {
            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var administrador = user.Administrador;

                if (administrador == true)
                {
                    TempData["hablitarUser"] = "1";
                }
            }
            //-------------------------------------------------------------------------------------------------------------

            //Models
            ScheduleAppointment scheduleAppointmentList = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/ScheduleAppointments/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ScheduleAppointment>();
                    readTask.Wait();
                    scheduleAppointmentList = readTask.Result;
                }
            }

            return View(scheduleAppointmentList);
        }
    }
}
