using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeModel.Data;
using OfficeModel.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Office.Controllers
{
    [Authorize(Policy = "SalesManager")]

    public class ClientsController : Controller
    {
        private readonly OfficeContext _context;
        private string _baseUrl = "http://localhost:7905/api/Clients";


        public ClientsController(OfficeContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var clients = JsonConvert.DeserializeObject<List<Client>>(await response.Content.ReadAsStringAsync());
                return View(clients);
            }
            return NotFound();
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var client = JsonConvert.DeserializeObject<Client>(
                await response.Content.ReadAsStringAsync());
                return View(client);
            }
            return NotFound();
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientID,Name,Address")] Client client)
        {
            if (!ModelState.IsValid) return View(client);
            try
            {
                var httpclient = new HttpClient();
                string json = JsonConvert.SerializeObject(client);
                var response = await httpclient.PostAsync(_baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }
            return View(client);

        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var client = JsonConvert.DeserializeObject<Client>(
                await response.Content.ReadAsStringAsync());
                return View(client);
            }
            return new NotFoundResult();
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientID,Name,Address")] Client client)
        {
            if (!ModelState.IsValid) return View(client);
            var httpclient = new HttpClient();
            string json = JsonConvert.SerializeObject(client);
            var response = await httpclient.PutAsync($"{_baseUrl}/{client.ClientID}",
            new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var client = JsonConvert.DeserializeObject<Client>(await response.Content.ReadAsStringAsync());
                return View(client);
            }
            return new NotFoundResult();
        }

        // POST: Clients/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind("ClientID")] Client client)
        {
            try
            {
                var httpclient = new HttpClient();
                HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{client.ClientID}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json")
                };
                var response = await httpclient.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
            }
            return View(client);
        }
    }


}

