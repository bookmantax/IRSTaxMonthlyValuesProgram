//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CityMonthDatabaseGenerator
{
    using System;
    using System.Collections.Generic;
    
    public partial class AprPerDiem
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Meals { get; set; }
        public int Lodging { get; set; }
        public int Total { get; set; }
        public int CityId { get; set; }
    
        public virtual CityPerDiem CityPerDiem { get; set; }
    }
}
