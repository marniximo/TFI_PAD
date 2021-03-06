using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
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

        public ActionResult Estadisticas(Guid ID) {
            var libro = _context.Libros.Find(ID);
            if (HttpContext.User.Identity.Name.Split(':')[1] != libro.Perfil.Username)
            {
                return RedirectToAction("Index");
            }
            var visitas = _context.Visitas.Where(v => v.Libro.ID == ID);
            var usuariosNoVisita = _context.Perfils.Where(p => !visitas.Select(v => v.Perfil).Contains(p)).ToList();
            ViewBag.UsuariosNoVisita = usuariosNoVisita;
            return View(libro);
        }

        public ActionResult Chart(int visitas, int noVisitas) {
            return PartialView(new int[] { noVisitas, visitas});
        }

        public ActionResult Filtrar(string filtro)
        {
            var libros = _context.Libros.Where(l =>
                l.Autor.ToLower().Contains(filtro) ||
                l.Perfil.Nombre.ToLower().Contains(filtro) ||
                l.Asignatura.Nombre.ToLower().Contains(filtro) ||
                l.Titulo.ToLower().Contains(filtro)
            ).ToList();
            return View("Index", libros);
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

        public ActionResult Eliminar(Guid ID) {
            var libro = _context.Libros.First(l => l.ID == ID);

            var user = System.Web.HttpContext.Current.User.Identity.Name.Split(':')[1];
            Perfil perfil = _context.Perfils.First(p => p.Username == user);

            if (libro.Perfil == perfil) {
                _context.Libros.Remove(libro);
                _context.SaveChanges();
                var path = Path.Combine(ConfigurationManager.AppSettings["PDFFolder"], libro.NombreArchivo);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            return RedirectToAction("Index");
        }

        [NonAction]
        private async Task SaveAsAsync(HttpPostedFileBase file, string filePath)
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
            await SaveAsAsync(file, Path.Combine(ConfigurationManager.AppSettings["PDFFolder"], fileName));
            _context.Libros.Add(libro);
            _context.SaveChanges();
            return RedirectToAction("Index", "Biblioteca");
        }

        [NonAction]
        public void RegistrarVisita(Perfil perfil, Libro libro) {
            var visita = _context.Visitas.FirstOrDefault(v => v.Perfil.ID == perfil.ID && v.Libro.ID == libro.ID);
            if (visita == null)
            {
                _context.Visitas.Add(new Visita { ID = Guid.NewGuid(), CantidadVisitas = 1, Libro = libro, Perfil = perfil });
            }
            else {
                visita.CantidadVisitas ++;
            }
            _context.SaveChanges();
        }

        public FileStreamResult Download(Guid ID) 
        {
            var username = System.Web.HttpContext.Current.User.Identity.Name.Split(':')[1];
            var perfil = _context.Perfils.First(p => p.Username == username);
            var libro = _context.Libros.First(l => l.ID == ID);
            RegistrarVisita(perfil, libro);
            var path = Path.Combine(ConfigurationManager.AppSettings["PDFFolder"], libro.NombreArchivo);
            var stream = new FileStream(path, FileMode.Open);
            return File(stream, "application/pdf");
        }
    }
}