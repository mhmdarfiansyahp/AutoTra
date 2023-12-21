﻿using AutoTra.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoTra.Controllers
{

    public class MobilController : Controller
    {
        private readonly Mobil mobilrepositori;
        public MobilController(IConfiguration configuration)
        {
            mobilrepositori = new Mobil(configuration);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(mobilrepositori.getAllData());
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                return View(mobilrepositori.getSearch(search));
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
        public IActionResult Create(MobilModel mbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (mbl.jenis_mobil == "Non Asset")
                    {
                        // Jika jenis_mobil adalah "Non Asset", set vin ke string kosong
                        mbl.vin = string.Empty;
                    }

                    mobilrepositori.insertdata(mbl);
                    TempData["SuccessMessage"] = "Data berhasil ditambahkan";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Log ModelState errors
                    foreach (var modelStateKey in ModelState.Keys)
                    {
                        var modelStateVal = ModelState[modelStateKey];
                        if (modelStateVal.Errors.Any())
                        {
                            foreach (var error in modelStateVal.Errors)
                            {
                                Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                            }
                        }
                    }

                    // Return to the view with validation errors
                    return View(mbl);
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(ex.ToString());

                // You might want to redirect to an error page or handle the exception accordingly
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            MobilModel mblmodel = mobilrepositori.getdata(id);
            if (mblmodel == null)
            {
                return NotFound();
            }
            Console.WriteLine(mblmodel.id_mobil);

            if (!string.IsNullOrEmpty(mblmodel.vin) && mblmodel.vin.Length == 17)
            {
                ViewData["VinCharacters"] = mblmodel.vin.ToCharArray();
            }

            return View(mblmodel);
        }

        [HttpPost]
        public IActionResult Edit(MobilModel mblmodel)
        {
            if (true)
            {
                Console.WriteLine(mblmodel.jenis_mobil);

                MobilModel newmbl = mobilrepositori.getdata(mblmodel.id_mobil);
                if (newmbl == null)
                {
                    return NotFound();
                }

                newmbl.jenis_mobil = mblmodel.jenis_mobil;
                newmbl.nama = mblmodel.nama;
                newmbl.vin = mblmodel.vin;
                newmbl.no_engine = mblmodel.no_engine;
                newmbl.warna = mblmodel.warna;
                newmbl.kilometer = mblmodel.kilometer;
                newmbl.bahan_bakar = mblmodel.bahan_bakar;
                newmbl.status = mblmodel.status;
                mobilrepositori.updatedata(newmbl);
                TempData["SuccessMessage"] = "Mobil berhasil diupdate.";
                return RedirectToAction("Index");

            }
            return View(mblmodel);
        }

        [HttpPost]
        public IActionResult Delete(string id_mobil)
        {
            var response = new { success = false, message = "Gagal menghapus mobil." };
            Console.WriteLine("id " + id_mobil);
            Console.WriteLine("try");

            try
            {
                if (id_mobil != "")
                {
                    Console.WriteLine("if");
                    Console.WriteLine("if" + id_mobil);
                    mobilrepositori.deletedata(id_mobil);
                    TempData["SuccessMessage"] = "Mobil berhasil dihapus!.";
                    response = new { success = true, message = "Mobil berhasil dihapus." };
                }
                else
                {
                    response = new { success = false, message = "Mobil tidak ditemukan." };
                    Console.WriteLine("else");
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = ex.Message };
                Console.WriteLine("catch");
            }
            return Json(response);
        }
    }
}