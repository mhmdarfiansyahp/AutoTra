﻿using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{

    public class DosenController : Controller
    {
        private readonly Dosen dosenrepositori;
        public DosenController(IConfiguration configuration)
        {
            dosenrepositori = new Dosen(configuration);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(dosenrepositori.getAllData());

        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                return View(dosenrepositori.getSearch(search));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DosenModel adm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (dosenrepositori.IsNpkExists(adm.npk) || dosenrepositori.IsUsernameExists(adm.username, adm.npk))
                    {
                        if (dosenrepositori.IsNpkExists(adm.npk))
                        {
                            TempData["ErrorMessage"] = "NPK already exists. Please choose a different one.";
                        }

                        // Check if the username already exists
                        if (dosenrepositori.IsUsernameExists(adm.username, adm.npk))
                        {
                            TempData["ErrorMessage"] = "Username already exists. Please choose a different one.";
                        }

                        TempData["ShowErrorMessage"] = true;  // Set to true only if validation fails
                        return RedirectToAction("Create");  // Redirect to the same action to render the view
                    }

                    dosenrepositori.insertdata(adm);
                    TempData["Success"] = true;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            TempData["ShowErrorMessage"] = false;  // Set to false if validation succeeds
            return View(adm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            DosenModel filmmodel = dosenrepositori.getdata(id);
            if (filmmodel == null)
            {
                return NotFound();
            }

            return View(filmmodel);
        }

        [HttpPost]
        public IActionResult Edit(DosenModel dsnmodel)
        {
            if (ModelState.IsValid)
            {
                // Pemeriksaan apakah username sudah ada
                if (dosenrepositori.IsUsernameExists(dsnmodel.username, dsnmodel.npk))
                {
                    ModelState.AddModelError("username", "Username already exists. Please choose a different one.");
                    return View(dsnmodel);
                }

                if (int.TryParse(dsnmodel.npk, out int Npk))
                {
                    DosenModel newadm = dosenrepositori.getdata(Npk);
                    if (newadm == null)
                    {
                        return NotFound();
                    }

                    newadm.nama = dsnmodel.nama;
                    newadm.username = dsnmodel.username;
                    newadm.password = dsnmodel.password;
                    newadm.peran = dsnmodel.peran;
                    newadm.status = dsnmodel.status;
                    dosenrepositori.updatedata(newadm);
                    TempData["IsUpdateSuccess"] = true;
                    return RedirectToAction("Index");
                }
            }
            return View(dsnmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var response = new { success = false, message = "Gagal menghapus admin." };

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    dosenrepositori.deletedata(id);
                    response = new { success = true, message = "Dosen successfully changed his status." };
                }
                else
                {
                    response = new { success = false, message = "Dosen tidak ditemukan." };
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = ex.Message };
            }
            return Json(response);
        }
    }
}
