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
    
    public partial class PRECATORIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRECATORIO()
        {
            this.CLIENTE = new HashSet<CLIENTE>();
            this.LOAS = new HashSet<LOAS>();
        }
    
        public int PREC_CD_ID { get; set; }
        public int TRF1_CD_ID { get; set; }
        public Nullable<int> BENE_CD_ID { get; set; }
        public Nullable<int> HONO_CD_ID { get; set; }
        public Nullable<int> BANC_CD_ID { get; set; }
        public Nullable<int> NATU_CD_ID { get; set; }
        public Nullable<int> PRES_CD_ID { get; set; }
        public string PREC_NM_PRECATORIO { get; set; }
        public string PREC_NR_ANO { get; set; }
        public string PREC_NM_PROCESSO_ORIGEM { get; set; }
        public string PREC_NM_REQUERENTE { get; set; }
        public string PREC_NM_ADVOGADO { get; set; }
        public string PREC_NM_DEPRECANTE { get; set; }
        public string PREC_NM_ASSUNTO { get; set; }
        public string PREC_NM_REQUERIDO { get; set; }
        public string PREC_SG_PROCEDIMENTO { get; set; }
        public string PREC_NM_SITUACAO_REQUISICAO { get; set; }
        public Nullable<decimal> PREC_VL_VALOR_INSCRITO_PROPOSTA { get; set; }
        public string PREC_NM_PROC_EXECUCAO { get; set; }
        public Nullable<System.DateTime> PREC_DT_BEN_DATABASE { get; set; }
        public Nullable<decimal> PREC_VL_BEN_VALOR_PRINCIPAL { get; set; }
        public Nullable<decimal> PREC_VL_JUROS { get; set; }
        public Nullable<decimal> PREC_VL_VALOR_INICIAL_PSS { get; set; }
        public Nullable<decimal> PREC_VL_BEN_VALOR_REQUISITADO { get; set; }
        public string PREC_SG_BEN_IR_RRA { get; set; }
        public Nullable<int> PREC_BEN_MESES_EXE_ANTERIOR { get; set; }
        public Nullable<System.DateTime> PREC_DT_HON_DATABASE { get; set; }
        public Nullable<decimal> PREC_VL_HON_VALOR_PRINCIPAL { get; set; }
        public Nullable<decimal> PREC_VL_HON_JUROS { get; set; }
        public Nullable<decimal> PREC_VL_HON_VALOR_INICIAL_PSS { get; set; }
        public Nullable<decimal> PREC_VL_HON_VALOR_REQUISITADO { get; set; }
        public string PREC_SG_HON_IR_RRA { get; set; }
        public Nullable<int> PREC_IN_HON_MESES_EXE_ANTERIOR { get; set; }
        public Nullable<int> PREC_IN_FOI_PESQUISADO { get; set; }
        public Nullable<System.DateTime> PREC_DT_INSERT_BD { get; set; }
        public Nullable<int> PREC_IN_FOI_IMPORTADO_PIPE { get; set; }
        public Nullable<System.DateTime> PREC_DT_PROTOCOLO_TRF { get; set; }
        public string PREC_NM_OFICIO_REQUISITORIO { get; set; }
        public string PREC_NM_REQUISICAO_BLOQUEADA { get; set; }
        public Nullable<System.DateTime> PREC_DT_EXPEDICAO { get; set; }
        public string PREC_NM_PRC_LOA { get; set; }
        public string PREC_TX_OBSERVACAO { get; set; }
        public Nullable<int> PREC_IN_ATIVO { get; set; }
    
        public virtual BANCO BANCO { get; set; }
        public virtual BENEFICIARIO BENEFICIARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
        public virtual HONORARIO HONORARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOAS> LOAS { get; set; }
        public virtual NATUREZA NATUREZA { get; set; }
        public virtual PRECATORIO_ESTADO PRECATORIO_ESTADO { get; set; }
        public virtual TRF TRF { get; set; }
    }
}
