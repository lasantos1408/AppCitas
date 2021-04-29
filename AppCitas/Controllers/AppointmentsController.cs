using AppCitas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppCitas.Controllers
{
    public class AppointmentsController : Controller
    {
        //URL donde se hubica el API 
        string BaseUrl = "https://localhost:44315/";

        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Appointments
        public async Task<ActionResult> Index()
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

                //Muestra la lista de todos los Appointments               
                return View(appointmentsList);
            }
        }

        //Crear nuevo Appointments       
        public async Task<ActionResult> Create()
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

            return View();
        }

        // POST: api/Appointments
        [HttpPost]
        public ActionResult Create(Appointments appointments)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl + "api/Appointments");
                var postTask = client.PostAsJsonAsync<Appointments>("appointments", appointments);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Error, contacta al administrador.");

            return View(appointments);
        }

        //Modificar usuario        
        public async Task<ActionResult> Edit(int id)
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
            Appointments appointments = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/Appointments/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Appointments>();
                    readTask.Wait();
                    appointments = readTask.Result;
                }
            }

            return View(appointments);
        }

        // PUT: api/Appointments/5
        [HttpPost]
        public ActionResult Edit(Appointments appointments)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //HTTP POST
                var putTask = client.PutAsJsonAsync($"api/Appointments/{appointments.IdAppointments}", appointments);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(appointments);
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
            Appointments appointments = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Get
                var responseTask = client.GetAsync("api/Appointments/" + Id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Appointments>();
                    readTask.Wait();
                    appointments = readTask.Result;
                }
            }

            return View(appointments);
        }

        // DELETE: api/Appointments/5
        [HttpPost]
        public ActionResult Delete(Appointments appointments, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Delete
                var deleteTask = client.DeleteAsync($"api/Appointments/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(appointments);
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
            Appointments appointments = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/Appointments/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Appointments>();
                    readTask.Wait();
                    appointments = readTask.Result;
                }
            }

            return View(appointments);
        }

    }
}
