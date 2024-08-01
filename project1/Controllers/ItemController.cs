using project1.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace _2301B2TempEmbedding.Controllers
{
    public class ItemController : Controller
    {
        //FoodContext db = new FoodContext();
        private readonly FoodContext _db;
        public ItemController(FoodContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var ItemsData = _db.Items.Include(p => p.Cat);
            return View(ItemsData);
        }

        public IActionResult Create()
        {
            ViewBag.CatId = new SelectList(_db.Categories, "CatId", "CatName");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item item, IFormFile file)
        {
            var imageName = DateTime.Now.ToString("yymmddhhmmss");//24074455454454
            imageName += Path.GetFileName(file.FileName);//24074455454454apple.png

            string imagepath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Uploads");
            var imagevalue = Path.Combine(imagepath, imageName);

            using (var stream = new FileStream(imagevalue, FileMode.Create))
            {

                file.CopyTo(stream);

            }

            var dbimage = Path.Combine("/Uploads", imageName);//   /uploads/240715343434apple.png
            item.Pimage = dbimage;
            if (ModelState.IsValid)
            {
                _db.Items.Add(item);
                _db.SaveChanges();
            }
            else
            {
                return View();
            }


            ViewBag.CatId = new SelectList(_db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }

        //Edit
        public IActionResult Edit(int id)
        {
            ViewBag.CatId = new SelectList(_db.Categories, "CatId", "CatName");
            var item1 = _db.Items.Find(id);

            return View(item1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item, IFormFile file, string oldimage)
        {
            var dbimage = "";
            if (file != null && file.Length > 0)
            {
                var imageName = DateTime.Now.ToString("yymmddhhmmss");//24074455454454
                imageName += Path.GetFileName(file.FileName);//24074455454454apple.png

                string imagepath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Uploads");
                var imagevalue = Path.Combine(imagepath, imageName);
                using (var stream = new FileStream(imagevalue, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                dbimage = Path.Combine("/Uploads", imageName);//   /uploads/240715343434apple.png
                item.Pimage = dbimage;
                _db.Items.Update(item);
                _db.SaveChanges();

            }
            else
            {
                item.Pimage = oldimage;
                _db.Items.Update(item);
                _db.SaveChanges();

            }
            ViewBag.CatId = new SelectList(_db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            ViewBag.CatId = new SelectList(_db.Categories, "CatId", "CatName");
            var item1 = _db.Items.Find(id);
            if (item1 == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(item1);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Item item)
        {

            _db.Items.Remove(item);
            _db.SaveChanges();


            ViewBag.CatId = new SelectList(_db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }

    }
}

