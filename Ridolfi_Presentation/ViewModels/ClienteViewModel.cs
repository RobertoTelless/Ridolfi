using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_CRM_Solution.ViewModels
{
    public class ClienteViewModel
    {
        [Key]
        public int CLIE_CD_ID { get; set; }
        public int CACL_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo PRECATÓRIO obrigatorio")]
        public int PREC_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo TRF obrigatorio")]
        public int TRF1_CD_ID { get; set; }
        public int VARA_CD_ID { get; set; }
        public Nullable<int> TITU_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O NOME deve conter no minimo 1 caracteres e no máximo 50.")]
        public string CLIE_NM_NOME { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> CLIE_VL_VALOR { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> CLIE_VL_VALOR_PAGO { get; set; }
        [StringLength(50, ErrorMessage = "O OFÍCIO deve conter no máximo 50 caracteres.")]
        public string CLIE_NR_OFICIO { get; set; }
        [StringLength(50, ErrorMessage = "O PROCESSO deve conter no máximo 50 caracteres.")]
        public string CLIE_NR_PROCESSO { get; set; }
        [StringLength(50, ErrorMessage = "O NOME DO RÉU deve conter no máximo 50 caracteres.")]
        public string CLIE_NM_REU { get; set; }
        public string CLIE_NR_VENCIMENTO { get; set; }
        public Nullable<int> CLIE_IN_PART { get; set; }
        public Nullable<int> CLIE_IN_ATIVO { get; set; }

        public virtual CATEGORIA_CLIENTE CATEGORIA_CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_ANEXO> CLIENTE_ANEXO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_CONTATO> CLIENTE_CONTATO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_QUADRO_SOCIETARIO> CLIENTE_QUADRO_SOCIETARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_CLIENTE> GRUPO_CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MENSAGENS_DESTINOS> MENSAGENS_DESTINOS { get; set; }
        public virtual TRF TRF { get; set; }
        public virtual PRECATORIO PRECATORIO { get; set; }
        public virtual VARA VARA { get; set; }
        public virtual TITULARIDADE TITULARIDADE { get; set; }

    }
}