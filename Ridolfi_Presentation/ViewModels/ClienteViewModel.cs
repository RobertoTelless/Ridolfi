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
        public int PREC_CD_ID { get; set; }
        public int TRF1_CD_ID { get; set; }
        public int VARA_CD_ID { get; set; }
        public Nullable<int> TITU_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O NOME deve conter no minimo 1 caracteres e no máximo 50.")]
        public string CLIE_NM_NOME { get; set; }
        public Nullable<decimal> CLIE_VL_VALOR { get; set; }
        public Nullable<decimal> CLIE_VL_VALOR_PAGO { get; set; }
        public string CLIE_NR_OFICIO { get; set; }
        public string CLIE_NR_PROCESSO { get; set; }
        [StringLength(50, ErrorMessage = "O NOME DO REU deve conter no minimo 1 caracteres e no máximo 50.")]
        public string CLIE_NM_REU { get; set; }
        public string CLIE_NR_VENCIMENTO { get; set; }
        public Nullable<int> CLIE_IN_PART { get; set; }
        public Nullable<int> CLIE_IN_ATIVO { get; set; }
        public int CACL_CD_ID { get; set; }

    }
}