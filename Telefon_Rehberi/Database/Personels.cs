//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Telefon_Rehberi.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Personels
    {
        public int Id { get; set; }
        
        [RegularExpression("^[a-zA-Z]*$")]
        public string PersonelAdi { get; set; }
        
        [RegularExpression("^[a-zA-Z]*$")]
        public string PersonelSoyadi { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        public string PersonelTelNo { get; set; }
        public int YoneticiID { get; set; }
        public int DepartmanID { get; set; }
    
        public virtual Departmen Departmen { get; set; }
    }
}
