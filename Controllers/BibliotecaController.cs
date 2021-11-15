using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TFI_PAD.Models;

namespace TFI_PAD.Controllers
{
    [Authorize]
    public class BibliotecaController : Controller
    {
        private BibliotecaEntities _context = new BibliotecaEntities();
        public ActionResult Index()
        {
            return View(_context.Libros.ToList());
        }

        public ActionResult Upload()
        {
            var asignaturas = _context.Asignaturas.ToList();
            var list = new List<SelectListItem>();
            foreach (var asignatura in asignaturas)
            {
                list.Add(new SelectListItem()
                {
                    Text = asignatura.Nombre,
                    Value = asignatura.Codigo
                });
            }
            ViewBag.Asignaturas = new SelectList(list, "Value", "Text");

            return View(new Libro());
        }

        [NonAction]
        public async Task SaveAsAsync(HttpPostedFileBase file, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.InputStream.CopyToAsync(stream);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(Libro libro, string Asignatura, string Username, HttpPostedFileBase file)
        {
            libro.ID = Guid.NewGuid();
            var asignatura = _context.Asignaturas.First(a => a.Codigo == Asignatura);
            var perfil = _context.Perfils.First(p => p.Username == Username);
            libro.Asignatura = asignatura;
            libro.Perfil = perfil;
            var fileName = file.FileName;
            libro.NombreArchivo = fileName;

            if (ModelState.IsValid)
            {
                await SaveAsAsync(file, Path.Combine(ConfigurationManager.AppSettings["PDFFolder"], fileName));   
            }
            _context.Libros.Add(libro);
            _context.SaveChanges();

            return RedirectToAction("Index", "Biblioteca");
        }

        public FileStreamResult Download(Guid ID) 
        {
            var libro = _context.Libros.First(l => l.ID == ID);
            var path = Path.Combine(ConfigurationManager.AppSettings["PDFFolder"], libro.NombreArchivo);
            var stream = new FileStream(path, FileMode.Open);
            return File(stream, "application/pdf");
        }
    }
}