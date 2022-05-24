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
    
    public partial class BENEFICIARIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BENEFICIARIO()
        {
            this.PRECATORIO = new HashSet<PRECATORIO>();
            this.BENEFICIARIO_ANEXO = new HashSet<BENEFICIARIO_ANEXO>();
            this.BENEFICIARIO_ANOTACOES = new HashSet<BENEFICIARIO_ANOTACOES>();
        }
    
        public int BENE_CD_ID { get; set; }
        public int TIPE_CD_ID { get; set; }
        public Nullable<int> SEXO_CD_ID { get; set; }
        public Nullable<int> ESCI_CD_ID { get; set; }
        public Nullable<int> ESCO_CD_ID { get; set; }
        public Nullable<int> PARE_CD_ID { get; set; }
        public string BENE_NM_NOME { get; set; }
        public string MOME_NM_RAZAO_SOCIAL { get; set; }
        public Nullable<System.DateTime> BENE_DT_NASCIMENTO { get; set; }
        public Nullable<decimal> BENE_VL_RENDA { get; set; }
        public Nullable<decimal> BENE_VL_RENDA_ESTIMADA { get; set; }
        public int BENE_IN_ATIVO { get; set; }
    
        public virtual ESTADO_CIVIL ESTADO_CIVIL { get; set; }
        public virtual PARENTESCO PARENTESCO { get; set; }
        public virtual SEXO SEXO { get; set; }
        public virtual ESCOLARIDADE ESCOLARIDADE { get; set; }
        public virtual TIPO_PESSOA TIPO_PESSOA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRECATORIO> PRECATORIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BENEFICIARIO_ANEXO> BENEFICIARIO_ANEXO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BENEFICIARIO_ANOTACOES> BENEFICIARIO_ANOTACOES { get; set; }
    }
}
