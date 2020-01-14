using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telefon_Rehberi.Database;

namespace Telefon_Rehberi.Models
{
    public class User
    {
        public Personels personel { get; set; }
        public bool yoneticilik { get; set; }
    }
}