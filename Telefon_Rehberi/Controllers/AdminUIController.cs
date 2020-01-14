using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Telefon_Rehberi.Database;
using Telefon_Rehberi.Models.Account;

namespace Telefon_Rehberi.Controllers
{
    public class AdminUIController : BaseController
    {
        // GET: AdminUI
        [HttpGet]
        public ActionResult Profil(int id)
        {
            Personel personel = new Personel();
            personel.calisan = context.Personels.FirstOrDefault(x => x.Id == id);
            personel.departmanAdi = context.Departmen.FirstOrDefault(x => x.Id == personel.calisan.DepartmanID).DepartmanAdi;
            Database.Personels yonetici = context.Personels.FirstOrDefault(y => y.Id == personel.calisan.YoneticiID);

            if (yonetici == null)
                personel.yoneticiAdSoyad = "Yönetici bilgisi yok!";
            else
                personel.yoneticiAdSoyad = yonetici.PersonelAdi + " " + yonetici.PersonelSoyadi;
            return View(personel);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["LogonAdmin"] != null)
            {
                Models.Account.ChangePassword model = new Models.Account.ChangePassword();

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(Models.Account.ChangePassword model)
        {

            if (Session["LogonAdmin"] == null)
            {
                TempData["resultInfo"] = "Oturum süreniz dolmuştur. Lütfen Oturum Açıp Tekrar Deneyiniz!";
                return RedirectToAction("Login", "Action");
            }
            if (model.admin.Password == model.password)
            {
                var admin = context.Logins.FirstOrDefault(x => x.Username == logonUserName);
                if (admin != null)
                {
                    if (admin.Password == model.currentPassword)//girilen mevcut şifre doğrumu
                    {
                        admin.Password = model.admin.Password;
                        try
                        {

                            context.Entry<Database.Logins>(admin).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            TempData["resultInfo"] = "Şifreniz Başarıyla Değiştirildi";

                        }
                        catch (Exception ex)
                        {
                            TempData["resultInfo"] = "Üzgünüz şifreniz değiştirilemedi!";

                        }
                        return View(new Models.Account.ChangePassword());
                    }
                    else
                    {
                        TempData["resultInfo"] = "Mevcut Şifreniz Doğru Değil";
                        return View(new Models.Account.ChangePassword());
                    }
                }
                else
                {
                    TempData["resultInfo"] = "Şifreniz Değiştirilemedi!";
                    return View(new Models.Account.ChangePassword());
                }
            }
            else
            {
                TempData["resultInfo"] = "Şifreler Uyuşmamaktadır!";
                return View(new Models.Account.ChangePassword());
            }

        }

        [HttpGet]
        public ActionResult ProfileCreate()
        {
            Database.Personels model = new Database.Personels();
            var departmanlar = context.Departmen.Select(x => new SelectListItem()
            {
                Text = x.DepartmanAdi,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.departmanList = departmanlar;

            var yoneticiler = context.Personels.Select(x => new SelectListItem()
            {
                Text = x.PersonelAdi + " " + x.PersonelSoyadi,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.yoneticiList = yoneticiler;
            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileCreate(Database.Personels personel)
        {

            System.Diagnostics.Debug.WriteLine("SomeText");
            System.Diagnostics.Debug.WriteLine("ad" + personel.PersonelAdi);
            System.Diagnostics.Debug.WriteLine("soyad" + personel.PersonelSoyadi);
            System.Diagnostics.Debug.WriteLine("tel" + personel.PersonelTelNo);
            System.Diagnostics.Debug.WriteLine("yonetici" + personel.YoneticiID);
            System.Diagnostics.Debug.WriteLine("departman" + personel.DepartmanID);



            context.Entry<Database.Personels>(personel).State = System.Data.Entity.EntityState.Added;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Yeni Profil Kaydı Oluşturuldu.";
                return RedirectToAction("Index", "PublicUI");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = ex.Message;
                var departmanlar = context.Departmen.Select(x => new SelectListItem()
                {
                    Text = x.DepartmanAdi,
                    Value = x.Id.ToString()
                }).ToList();
                ViewBag.departmanList = departmanlar;

                var yoneticiler = context.Personels.Select(x => new SelectListItem()
                {
                    Text = x.PersonelAdi + " " + x.PersonelSoyadi,
                    Value = x.Id.ToString()
                }).ToList();
                ViewBag.yoneticiList = yoneticiler;
                return RedirectToAction("Index", "PublicUI");
            }
        }


        [HttpGet]
        public ActionResult ProfileEdit(int id = 0)
        {
            var personel = new Database.Personels();
            personel.Id = 0;
            Database.Personels model = new Database.Personels();
            if (id != 0)
            {
                personel = context.Personels.FirstOrDefault(x => x.Id == id);
                model = personel;
            }
            System.Diagnostics.Debug.WriteLine("aaa--" + model.Id + "--");
            var departmanlar = context.Departmen.Select(x => new SelectListItem()
            {
                Text = x.DepartmanAdi,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.departmanList = departmanlar;

            var yoneticiler = context.Personels.Select(x => new SelectListItem()
            {
                Text = x.PersonelAdi + " " + x.PersonelSoyadi,
                Value = x.Id.ToString()
            }).Where(x => x.Value != model.Id.ToString()).ToList();
            ViewBag.yoneticiList = yoneticiler;

            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileEdit(Database.Personels model)
        {
            context.Entry<Database.Personels>(model).State = System.Data.Entity.EntityState.Modified;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Bilgiler Güncellendi!";
                return RedirectToAction("Profil", "AdminUI", new { id = model.Id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("aaa--" + "" + ex.Message);
                TempData["resultInfo"] = "Bilgiler Güncellenemedi!";
                var user = context.Personels.FirstOrDefault(x => x.Id == model.Id);
                Personel personel = new Personel();
                personel.calisan = user;
                context.Entry<Database.Personels>(model).State = System.Data.Entity.EntityState.Added;
                return RedirectToAction("ProfileEdit", "AdminUI", new { id = model.Id });
            }
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.Account.Login model)
        {
            try
            {
                var admin = context.Logins.FirstOrDefault(x => x.Username == model.admin.Username && x.Password == model.admin.Password);
                if (admin != null)
                {
                    Session["LogonAdmin"] = admin;
                    logonUserName = admin.Username;
                    return RedirectToAction("Index", "PublicUI");
                }
                else
                {
                    ViewBag.error = "Admin Bilgileri Yanlış!";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return View();


            }
        }

        public ActionResult Logout()
        {
            Session["LogonAdmin"] = null;
            return RedirectToAction("Login", "AdminUI");
        }

        public ActionResult Delete(int id)
        {
            var personel = context.Personels.FirstOrDefault(x => x.Id == id);
            if (context.Personels.Any(x => x.YoneticiID == personel.Id))
            {
                TempData["resultInfo"] = "Bu personel yönetici olduğu için silinemez!";
                return RedirectToAction("Index", "PublicUI");
            }
            context.Entry<Database.Personels>(personel).State = System.Data.Entity.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Personel silindi!";
                return RedirectToAction("Index", "PublicUI");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = "Kullanıcı bir sebepten dolayı silinemedi!";
                return RedirectToAction("Index", "PublicUI");
            }

        }
    }
}
