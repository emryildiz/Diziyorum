using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Diziyorum;
using Diziyorum.Models;

namespace Diziyorum.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private DiziYorumEntities db = new DiziYorumEntities();

        // GET: Home


        
        public ActionResult Index()
        {

            return View(db.TBLBLOG.OrderByDescending(x => x.BLOGTARIH).ToList());
        }
        
        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Gonderi(string message)
        {
            var a = new BlogYorum();
            int id = Convert.ToInt32(Request.QueryString["BLOGID"]);
            var blog = db.TBLBLOG.Where(x => x.BLOGID == id).ToList();
            var yorum = db.TBLYORUM.Where(x => x.YORUMBLOG == id).ToList();
            a.blog = blog;
            a.yorum = yorum;

            TBLYORUM ax = new TBLYORUM();
            ax.YORUMICERIK = message;
            ax.KULLANICIAD = Session["Kullanici"].ToString();
            ax.YORUMBLOG = id;
            db.TBLYORUM.Add(ax);
            db.SaveChanges();
            Response.Redirect("Gonderi?BLOGID=" + id);
            return View(a);
        }
        [AllowAnonymous]
        public ActionResult Gonderi()
        {

            var a = new BlogYorum();
            int id = Convert.ToInt32(Request.QueryString["BLOGID"]);
            var blog = db.TBLBLOG.Where(x => x.BLOGID == id).ToList();
            var yorum = db.TBLYORUM.Where(x => x.YORUMBLOG == id).ToList();
            a.blog = blog;
            a.yorum = yorum;
            return View(a);
        }
       
        public ActionResult Kategori()
        {
            int id = Convert.ToInt32(Request.QueryString["KATEGORIID"]);
            var bloglar = db.TBLBLOG.Where(x => x.BLOGKATEGORI == id).OrderByDescending(x => x.BLOGTARIH).ToList();

            


            return View(bloglar);

        }
       
        [HttpPost]
        public ActionResult İletisim(string isim, string mail, string konu, string mesaj)
        {
            TBLILETISIM ilet = new TBLILETISIM();
            ilet.ADSOYAD = isim;
            ilet.KONU = konu;
            ilet.MESAJ = mesaj;
            ilet.MAIL = mail;
            db.TBLILETISIM.Add(ilet);
            db.SaveChanges();
            return RedirectToAction("İletisim","Home");
        }

        
        public ActionResult YorumSil()
        {
            int id = Convert.ToInt32(Request.QueryString["YorumID"]);
            var silinecekYorum = db.TBLYORUM.Find(id);
            var blogid = silinecekYorum.YORUMBLOG;
            db.TBLYORUM.Remove(silinecekYorum);
            db.SaveChanges();
            Response.Redirect("Gonderi?BLOGID=" + blogid);
            return View();

        }
        
        public ActionResult İletisim()
        {
            return View();
        }
        
        public ActionResult Tur()
        {
            int id = Convert.ToInt32(Request.QueryString["TURID"]);
            var bloglar = db.TBLBLOG.Where(x => x.BLOGTUR == id).OrderByDescending(x => x.BLOGTARIH).ToList();




            return View(bloglar);

        }



        
        public ActionResult Hakkimda()
        {
            return View();
        }


        
        public ActionResult Login()
        {

            return View();
        }

        
        [HttpPost]
        public ActionResult Login(TBLKULLANICI kullanici)
        {
            
            var kontrol = false;

            var kontrolKullanici = db.TBLKULLANICI.FirstOrDefault(x => x.KULLANICIADI == kullanici.KULLANICIADI && x.SIFRE == kullanici.SIFRE);
            
            if (kontrolKullanici!=null)
            {
                kontrol = true;
                Session.Add("Kullanici", kullanici.KULLANICIADI);

                FormsAuthentication.SetAuthCookie(kullanici.KULLANICIADI, false);

               
                return RedirectToAction("Index", "Home");
                

            }

            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya şifreniz yanlış";
                return View();
            }





            
        }


       
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(string kullanici, string sifre, string mail)
        {





            TBLKULLANICI yeni = new TBLKULLANICI();
            yeni.KULLANICIADI = kullanici;
            yeni.SIFRE = sifre;
            yeni.EMAIL = mail;
            var kontrolkullanici = db.TBLKULLANICI.FirstOrDefault(x => x.KULLANICIADI == kullanici);
            var kontrolmail = db.TBLKULLANICI.FirstOrDefault(x => x.EMAIL == mail);





            if (kontrolkullanici != null)
            {
                ViewBag.Mesaj = "Kullanıcı adi zaten alınmış";
                return View();

            }
            else if(kontrolmail !=null)
            {
                ViewBag.Mesaj = "E-Posta daha önce kullanılmış";
                return View();
            }
            else
            {
                db.TBLKULLANICI.Add(yeni);
                db.SaveChanges();
                return RedirectToAction("Login","Home");

            }









            
        }





        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLBLOG tBLBLOG = db.TBLBLOG.Find(id);
            if (tBLBLOG == null)
            {
                return HttpNotFound();
            }
            return View(tBLBLOG);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            ViewBag.BLOGKATEGORI = new SelectList(db.TBLKATEGORI, "KATEGORIID", "KATEGORIAD");
            ViewBag.BLOGTUR = new SelectList(db.TBLTUR, "TURID", "TURAD");
            return View();
        }

        // POST: Home/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BLOGID,BLOGBASLIK,BLOGICERIK,BLOGTARIH,BLOGTUR,BLOGGORSEL,BLOGKATEGORI")] TBLBLOG tBLBLOG)
        {
            if (ModelState.IsValid)
            {
                db.TBLBLOG.Add(tBLBLOG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BLOGKATEGORI = new SelectList(db.TBLKATEGORI, "KATEGORIID", "KATEGORIAD", tBLBLOG.BLOGKATEGORI);
            ViewBag.BLOGTUR = new SelectList(db.TBLTUR, "TURID", "TURAD", tBLBLOG.BLOGTUR);
            return View(tBLBLOG);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLBLOG tBLBLOG = db.TBLBLOG.Find(id);
            if (tBLBLOG == null)
            {
                return HttpNotFound();
            }
            ViewBag.BLOGKATEGORI = new SelectList(db.TBLKATEGORI, "KATEGORIID", "KATEGORIAD", tBLBLOG.BLOGKATEGORI);
            ViewBag.BLOGTUR = new SelectList(db.TBLTUR, "TURID", "TURAD", tBLBLOG.BLOGTUR);
            return View(tBLBLOG);
        }

        // POST: Home/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BLOGID,BLOGBASLIK,BLOGICERIK,BLOGTARIH,BLOGTUR,BLOGGORSEL,BLOGKATEGORI")] TBLBLOG tBLBLOG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBLBLOG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BLOGKATEGORI = new SelectList(db.TBLKATEGORI, "KATEGORIID", "KATEGORIAD", tBLBLOG.BLOGKATEGORI);
            ViewBag.BLOGTUR = new SelectList(db.TBLTUR, "TURID", "TURAD", tBLBLOG.BLOGTUR);
            return View(tBLBLOG);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLBLOG tBLBLOG = db.TBLBLOG.Find(id);
            if (tBLBLOG == null)
            {
                return HttpNotFound();
            }
            return View(tBLBLOG);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBLBLOG tBLBLOG = db.TBLBLOG.Find(id);
            db.TBLBLOG.Remove(tBLBLOG);
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
