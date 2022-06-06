using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_CRM_Solution.ViewModels
{
    public class EMailViewModel
    {
        [Key]
        public int EMAI_CD_ID { get; set; }
        public int BENE_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo E-MAIL obrigatorio")]
        [StringLength(150, ErrorMessage = "O E-MAIL deve ter no máximo 150 caracteres.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Deve ser um e-mail válido")]
        public string EMAI_NM_EMAIL { get; set; }
        public int EMAI_IN_ATIVO { get; set; }

        public virtual BENEFICIARIO BENEFICIARIO { get; set; }
    }
}