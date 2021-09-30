using Diziyorum.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace Diziyorum.Controllers
{
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private DiziYorumEntities db = new DiziYorumEntities();

        // GET: Admin
       
        public ActionResult Bloglar()
        {
            
           return View(db.TBLBLOG.ToList());
        }

        public ActionResult Yorumlar()
        {
            var yorum = from x in db.TBLYORUM
                        select new
                        {
                            x.YORUMID,
                            x.TBLBLOG.BLOGBASLIK
                        };


            return View(yorum.ToList());
        }

        public ActionResult BlogSil()
        {
            int a = Convert.ToInt32(Request.QueryString["BLOGID"]);
            var blog = db.TBLBLOG.Find(a);
            db.TBLBLOG.Remove(blog);
            var yorum = db.TBLYORUM.Where(x => x.YORUMBLOG == blog.BLOGID).ToList();
            foreach (var i in yorum)
            {
                db.TBLYORUM.Remove(i);

            }

            db.SaveChanges();
            return RedirectToAction("Bloglar","Admin");

        }
        public ActionResult YorumSil()
        {
            int x = Convert.ToInt32(Request.QueryString["YORUMID"]);
            var yorum = db.TBLYORUM.Find(x);
            db.TBLYORUM.Remove(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar","Admin");
        }

        public ActionResult BlogEkle()
        {
            var yeni = new KategorilerTurler();

            yeni.kategoriler = (from k in db.TBLKATEGORI
                                select new SelectListItem
                                {
                                    Text = k.KATEGORIAD,
                                    Value = k.KATEGORIID.ToString()
                                }).ToList();



            yeni.turler = (from k in db.TBLTUR
                           select new SelectListItem
                           {
                               Text = k.TURAD,
                               Value = k.TURID.ToString()
                           }).ToList();







            return View(yeni);
        }
        [HttpPost]
        public ActionResult BlogEkle(string baslik, string tarih, string gorsel, string icerik, KategorilerTurler kategoriTur)
        {
            var yeni = new KategorilerTurler();

            yeni.kategoriler = (from k in db.TBLKATEGORI
                                select new SelectListItem
                                {
                                    Text = k.KATEGORIAD,
                                    Value = k.KATEGORIID.ToString()
                                }).ToList();



            yeni.turler = (from k in db.TBLTUR
                           select new SelectListItem
                           {
                               Text = k.TURAD,
                               Value = k.TURID.ToString()
                           }).ToList();


            TBLBLOG t = new TBLBLOG();
            t.BLOGBASLIK = baslik;
            t.BLOGGORSEL = gorsel;
            t.BLOGTARIH = DateTime.Now;
            t.BLOGICERIK = icerik;
            t.BLOGKATEGORI = kategoriTur.selectedKategoriId;
            t.BLOGTUR = kategoriTur.selectedTurId;
            db.TBLBLOG.Add(t);
            db.SaveChanges();
            Response.Redirect("Bloglar");

            return View(yeni);
        }

        int y;
        public ActionResult BlogGuncelle()
        {

           y = int.Parse(Request.QueryString["BLOGID"]);

            var yeni = new KategorilerTurler();
            yeni.kategoriler = (from k in db.TBLKATEGORI
                                select new SelectListItem
                                {
                                    Text = k.KATEGORIAD,
                                    Value = k.KATEGORIID.ToString()
                                }).ToList();



            yeni.turler = (from k in db.TBLTUR
                           select new SelectListItem
                           {
                               Text = k.TURAD,
                               Value = k.TURID.ToString()
                           }).ToList();

            return View(yeni);
        }

        
        [HttpPost]
        public ActionResult BlogGuncelle(string baslik, string tarih, string gorsel, string icerik, KategorilerTurler kategoriTur,int id)
        {
            

            var t = db.TBLBLOG.Find(id);
            t.BLOGBASLIK = baslik;
            t.BLOGGORSEL = gorsel;
            t.BLOGTARIH = DateTime.Now;
            t.BLOGICERIK = icerik;
            t.BLOGKATEGORI = kategoriTur.selectedKategoriId;
            t.BLOGTUR = kategoriTur.selectedTurId;

            db.SaveChanges();












            return RedirectToAction("Bloglar", "Admin");
        }


        

        public ActionResult İstatistikler()
        {

            ViewBag.Blog = db.TBLBLOG.Count().ToString();
            ViewBag.Yorum = db.TBLYORUM.Count().ToString();
            ViewBag.Film = db.TBLBLOG.Where(x => x.BLOGTUR == 2).Count().ToString();
            ViewBag.Dizi = db.TBLBLOG.Where(x => x.TBLTUR.TURAD == "Dizi").Count().ToString();
            ViewBag.Animasyon = db.TBLBLOG.Where(x => x.TBLTUR.TURAD == "Animasyon").Count().ToString();
            ViewBag.Most = db.TBLBLOG.Where(y => y.BLOGID == (db.TBLYORUM.GroupBy(x => x.YORUMBLOG).OrderByDescending(x => x.Count()).Select(z => z.Key).FirstOrDefault())).Select(k => k.BLOGBASLIK).FirstOrDefault();







            return View();
        }










        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADMIN aDMIN = db.ADMIN.Find(id);
            if (aDMIN == null)
            {
                return HttpNotFound();
            }
            return View(aDMIN);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,KULLANICI,SIFRE")] ADMIN aDMIN)
        {
            if (ModelState.IsValid)
            {
                db.ADMIN.Add(aDMIN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aDMIN);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADMIN aDMIN = db.ADMIN.Find(id);
            if (aDMIN == null)
            {
                return HttpNotFound();
            }
            return View(aDMIN);
        }

        // POST: Admin/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,KULLANICI,SIFRE")] ADMIN aDMIN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aDMIN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aDMIN);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADMIN aDMIN = db.ADMIN.Find(id);
            if (aDMIN == null)
            {
                return HttpNotFound();
            }
            return View(aDMIN);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ADMIN aDMIN = db.ADMIN.Find(id);
            db.ADMIN.Remove(aDMIN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
