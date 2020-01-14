using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi.Database;

namespace Telefon_Rehberi.Models.Account
{
    public class Personel
    {
        public Personels calisan { get; set; }
        public string departmanAdi;
        public string yoneticiAdSoyad;
        public List<SelectListItem> departmanList { get; set; }
        public List<SelectListItem> yoneticiList { get; set; }
        public int yoneticiID;
        public int departmanID;
    }
}