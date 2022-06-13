using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;

namespace ERP_CRM_Solution.ViewModels
{
    public class ClienteAnotacaoViewModel
    {
        [Key]
        public int CLAN_CD_ID { get; set; }
        public int CLIE_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo DATA obrigatorio")]
        [DataType(DataType.Date, ErrorMessage = "Deve ser uma data válida")]
        public Nullable<System.DateTime> CLAN_DT_ANOTACAO { get; set; }
        public Nullable<int> USUA_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo TEXTO obrigatorio")]
        [StringLength(5000, MinimumLength = 1, ErrorMessage = "O COMENTÁRIO deve conter no minimo 1 caracteres e no máximo 5000.")]
        public string CLAN_TX_ANOTACAO { get; set; }
        public Nullable<int> CLAN_IN_ATIVO { get; set; }

        public virtual CLIENTE CLIENTE { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}