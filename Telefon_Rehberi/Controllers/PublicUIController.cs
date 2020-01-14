using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi.Database;
using Telefon_Rehberi.Models;

namespace Telefon_Rehberi.Controllers
{
    public class PublicUIController : BaseController
    {
        // GET: PublicUI
        public ActionResult Index()
        {
            List<Personels> calisanlar = context.Personels.Where(x => x.Id != 0).ToList();
            return View(calisanlar);
        }

        [HttpGet]
        public ActionResult DepartmentControl()
        {
            List<Departman> departmanListModel = new List<Departman>();
            List<Database.Departmen> departmanList = context.Departmen.Where(x => x.Id != 0).ToList();
            System.Diagnostics.Debug.WriteLine("aa--" + departmanList.Count + "--");
            List<int> personelSayisi = new List<int>();
            for (int i = 0; i < departmanList.Count; i++)
            {
                Departman model = new Departman();
                model.departman = departmanList[i];
                System.Diagnostics.Debug.WriteLine("aa--" + model.departman + "--");
                System.Diagnostics.Debug.WriteLine("aa--" + departmanList[i] + "--");
                System.Diagnostics.Debug.WriteLine("aa--" + i + "--");
                model.PersonelSayisi = Convert.ToInt32(context.Personels.Where(x => x.DepartmanID == model.departman.Id).ToList().Count());
                departmanListModel.Add(model);
            }
            return View(departmanListModel);
        }

        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                TempData["resultInfo"] = "Departman içinde mevcut personel olduğu için silinemedi.";
                return RedirectToAction("DepartmentControl", "PublicUI");
            }
            var department = context.Departmen.FirstOrDefault(x => x.Id == id);
            string name = department.DepartmanAdi;
            context.Entry<Database.Departmen>(department).State = System.Data.Entity.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = name + " departmanı silindi.";
                return RedirectToAction("DepartmentControl", "PublicUI");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = "Silme İşlemi Gerçekleştirilemedi.";
                return RedirectToAction("DepartmentControl", "PublicUI");
            }

        }


        [HttpGet]
        public ActionResult DepartmanCreate()
        {
            if (Session["LogonAdmin"] != null)
            {
                Departmen model = new Departmen();

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "PublicUI");
            }
        }

        [HttpPost]
        public ActionResult DepartmanCreate(Departmen model)
        {

            System.Diagnostics.Debug.WriteLine("aa--" + model.DepartmanAdi + "--");
            if (Session["LogonAdmin"] == null)
            {
                TempData["resultInfo"] = "Oturum süreniz dolmuştur. Lütfen Oturum Açıp Tekrar Deneyiniz!";
                return RedirectToAction("Login", "Action");
            }

            if (model.DepartmanAdi != "")
            {
                if (context.Departmen.Any(x => x.DepartmanAdi == model.DepartmanAdi))
                {
                    TempData["resultInfo"] = "Aynı isimde bölüm var!";
                    return View();
                }
                try
                {

                    context.Entry<Database.Departmen>(model).State = System.Data.Entity.EntityState.Added;
                    TempData["resultInfo"] = model.DepartmanAdi + " departmanı eklendi.";
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData["resultInfo"] = ex.Message;
                }
                return RedirectToAction("DepartmentControl", "PublicUI");
            }
            else
            {
                TempData["resultInfo"] = "Departman Adı Giriniz!";
                return RedirectToAction("DepartmentControl", "PublicUI");
            }

        }

    }
}
