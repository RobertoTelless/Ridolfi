//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntitiesServices.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EMAIL
    {
        public int EMAI_CD_ID { get; set; }
        public int BENE_CD_ID { get; set; }
        public string EMAI_NM_EMAIL { get; set; }
        public int EMAI_IN_ATIVO { get; set; }
    
        public virtual BENEFICIARIO BENEFICIARIO { get; set; }
    }
}
