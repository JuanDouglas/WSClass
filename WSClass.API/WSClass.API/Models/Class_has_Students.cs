//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSClass.API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Class_has_Students
    {
        public int ID { get; set; }
        public int Student { get; set; }
        public int Class { get; set; }
        public System.DateTime AddDate { get; set; }
    
        public virtual Class Class1 { get; set; }
        public virtual User User { get; set; }
    }
}
