using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telefon_Rehberi.Database;

namespace Telefon_Rehberi.Models
{
    public class Departman
    {
        public Departmen departman { get; set; }
        public int PersonelSayisi { get; set; }
    }
}