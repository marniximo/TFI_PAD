using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TFI_PAD.Models;
using System.IO;
using System.Configuration;

namespace TFI_PAD.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private BibliotecaEntities db = new BibliotecaEntities();

        public ActionResult Edit()
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.Split(':')[1];
            if (string.IsNullOrEmpty(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfils.FirstOrDefault(p=>p.Username == user);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            var path = Path.Combine(ConfigurationManager.AppSettings["Imagenes"], perfil.ID.ToString());
            if (System.IO.File.Exists(path))
            {
                string foto = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                ViewBag.FotoPerfil = foto;
            }
            else
            {
                string foto = Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["Imagenes"], "DEFAULT.png")));
                ViewBag.FotoPerfil = foto;
            }
            return View(perfil);
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
        public async Task<ActionResult> Edit(Perfil perfil, HttpPostedFileBase imagen)
        {
            if (imagen != null) {
                var path = Path.Combine(ConfigurationManager.AppSettings["Imagenes"], perfil.ID.ToString());
                await SaveAsAsync(imagen, path);
            }
            if (ModelState.IsValid)
            {
                db.Entry(perfil).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Biblioteca");
        }
    }
}
