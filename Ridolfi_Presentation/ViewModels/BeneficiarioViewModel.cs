using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_CRM_Solution.ViewModels
{
    public class BeneficiarioViewModel
    {
        [Key]
        public int BENE_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo TIPO DE PESSOA obrigatorio")]
        public int TIPE_CD_ID { get; set; }
        public Nullable<int> SEXO_CD_ID { get; set; }
        public Nullable<int> ESCI_CD_ID { get; set; }
        public Nullable<int> ESCO_CD_ID { get; set; }
        public Nullable<int> PARE_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "O NOME deve conter no minimo 1 caracteres e no máximo 250 caracteres.")]
        public string BENE_NM_NOME { get; set; }
        [StringLength(250, ErrorMessage = "A RAZÃO SOCIAL deve conter no máximo 250 caracteres.")]
        public string MOME_NM_RAZAO_SOCIAL { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE NASCIMENTO Deve ser uma data válida")]
        public Nullable<System.DateTime> BENE_DT_NASCIMENTO { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> BENE_VL_RENDA { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> BENE_VL_RENDA_ESTIMADA { get; set; }
        public int BENE_IN_ATIVO { get; set; }
        public Nullable<System.DateTime> BENE_DT_CADASTRO { get; set; }

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