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
    
    public partial class CompletTask
    {
        public int ID { get; set; }
        public int User { get; set; }
        public System.DateTime CompletationDate { get; set; }
        public int Task { get; set; }
    
        public virtual Task Task1 { get; set; }
        public virtual User User1 { get; set; }
    }
}
