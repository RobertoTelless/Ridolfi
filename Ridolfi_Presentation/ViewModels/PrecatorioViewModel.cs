using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_CRM_Solution.ViewModels
{
    public class PrecatorioViewModel
    {
        [Key]
        public int PREC_CD_ID { get; set; }
        public int TRF1_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo BENEFICIÁRIO obrigatorio")]
        public Nullable<int> BENE_CD_ID { get; set; }
        public Nullable<int> HONO_CD_ID { get; set; }
        public Nullable<int> BANC_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NATUREZA obrigatorio")]
        public Nullable<int> NATU_CD_ID { get; set; }
        public Nullable<int> PRES_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo PRECATÓRIO obrigatorio")]
        [StringLength(50, ErrorMessage = "O PRECATÓRIO deve conter no máximo 50 caracteres.")]
        public string PREC_NM_PRECATORIO { get; set; }
        [Required(ErrorMessage = "Campo ANO obrigatorio")]
        [StringLength(4, ErrorMessage = "O ANO deve conter no máximo 4 caracteres.")]
        public string PREC_NR_ANO { get; set; }
        [Required(ErrorMessage = "Campo PROCESSO DE ORIGEM obrigatorio")]
        [StringLength(20, ErrorMessage = "O PROCESSO DE ORIGEM deve conter no máximo 20 caracteres.")]
        public string PREC_NM_PROCESSO_ORIGEM { get; set; }
        [Required(ErrorMessage = "Campo REQUERENTE obrigatorio")]
        [StringLength(50, ErrorMessage = "O REQUERENTE deve conter no máximo 50 caracteres.")]
        public string PREC_NM_REQUERENTE { get; set; }
        public string PREC_NM_ADVOGADO { get; set; }
        [Required(ErrorMessage = "Campo DEPRECANTE obrigatorio")]
        [StringLength(50, ErrorMessage = "O DEPRECANTE deve conter no máximo 50 caracteres.")]
        public string PREC_NM_DEPRECANTE { get; set; }
        [StringLength(500, ErrorMessage = "O ASSUNTO deve conter no máximo 500 caracteres.")]
        public string PREC_NM_ASSUNTO { get; set; }
        [Required(ErrorMessage = "Campo REQUERIDO obrigatorio")]
        [StringLength(50, ErrorMessage = "O REQUERIDO deve conter no máximo 50 caracteres.")]
        public string PREC_NM_REQUERIDO { get; set; }
        public string PREC_SG_PROCEDIMENTO { get; set; }
        [StringLength(50, ErrorMessage = "A SITUAÇÂO DA REQUISIÇÂO deve conter no máximo 50 caracteres.")]
        public string PREC_NM_SITUACAO_REQUISICAO { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_VALOR_INSCRITO_PROPOSTA { get; set; }
        [StringLength(50, ErrorMessage = "O PROCESSO EXECUÇÃO deve conter no máximo 50 caracteres.")]
        public string PREC_NM_PROC_EXECUCAO { get; set; }
        [Required(ErrorMessage = "Campo DATA DE CÁLCULO obrigatorio")]
        [DataType(DataType.Date, ErrorMessage = "Deve ser uma data válida")]
        public Nullable<System.DateTime> PREC_DT_BEN_DATABASE { get; set; }
        [Required(ErrorMessage = "Campo VALOR RRINCIPAL obrigatorio")]
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_BEN_VALOR_PRINCIPAL { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_JUROS { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_VALOR_INICIAL_PSS { get; set; }
        [Required(ErrorMessage = "Campo VALOR REQUISITADO obrigatorio")]
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_BEN_VALOR_REQUISITADO { get; set; }
        public string PREC_SG_BEN_IR_RRA { get; set; }
        public Nullable<int> PREC_BEN_MESES_EXE_ANTERIOR { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Deve ser uma data válida")]
        public Nullable<System.DateTime> PREC_DT_HON_DATABASE { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_HON_VALOR_PRINCIPAL { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_HON_JUROS { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_HON_VALOR_INICIAL_PSS { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<decimal> PREC_VL_HON_VALOR_REQUISITADO { get; set; }
        public string PREC_SG_HON_IR_RRA { get; set; }
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<int> PREC_IN_HON_MESES_EXE_ANTERIOR { get; set; }
        [Required(ErrorMessage = "Campo FOI PESQUISADO obrigatorio")]
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<int> PREC_IN_FOI_PESQUISADO { get; set; }
        [Required(ErrorMessage = "Campo DATA DE INSERÇÃO obrigatorio")]
        [DataType(DataType.Date, ErrorMessage = "Deve ser uma data válida")]
        public Nullable<System.DateTime> PREC_DT_INSERT_BD { get; set; }
        [Required(ErrorMessage = "Campo IMPORTADO CRM obrigatorio")]
        [RegularExpression(@"^[0-9]+([,.][0-9]+)?$", ErrorMessage = "Deve ser um valor numérico positivo")]
        public Nullable<int> PREC_IN_FOI_IMPORTADO_PIPE { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Deve ser uma data válida")]
        public Nullable<System.DateTime> PREC_DT_PROTOCOLO_TRF { get; set; }
        [StringLength(50, ErrorMessage = "O OFÍCIO REQUISITÓRIO deve conter no máximo 50 caracteres.")]
        public string PREC_NM_OFICIO_REQUISITORIO { get; set; }
        [StringLength(50, ErrorMessage = "A REQUISIÇÂO BLOQUEADA deve conter no máximo 50 caracteres.")]
        public string PREC_NM_REQUISICAO_BLOQUEADA { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Deve ser uma data válida")]
        public Nullable<System.DateTime> PREC_DT_EXPEDICAO { get; set; }
        [Required(ErrorMessage = "Campo PRC LOA obrigatorio")]
        [StringLength(50, ErrorMessage = "O PRC LOA deve conter no máximo 50 caracteres.")]
        public string PREC_NM_PRC_LOA { get; set; }
        [StringLength(5000, ErrorMessage = "A OBSERVAÇÃO deve conter no máximo 5000 caracteres.")]
        public string PREC_TX_OBSERVACAO { get; set; }
        public Nullable<int> PREC_IN_ATIVO { get; set; }
        public Nullable<int> PREC_IN_SITUACAO_ATUAL { get; set; }
        public Nullable<System.DateTime> PREC_DT_CADASTRO { get; set; }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRECATORIO_ANEXO> PRECATORIO_ANEXO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRECATORIO_ANOTACAO> PRECATORIO_ANOTACAO { get; set; }




















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
        [StringLength(20, ErrorMessage = "O CPF deve conter no máximo 20 caracteres.")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string BENE_NR_CPF { get; set; }
        [StringLength(20, ErrorMessage = "O RG deve conter no máximo 20 caracteres.")]
        public string BENE_NR_RG { get; set; }
        [StringLength(20, ErrorMessage = "O TELEFONE deve conter no máximo 20 caracteres.")]
        public string BENE_NR_TELEFONE { get; set; }
        [StringLength(20, ErrorMessage = "O TELEFONE deve conter no máximo 20 caracteres.")]
        public string BENE_NR_TELEFONE2 { get; set; }
        [StringLength(20, ErrorMessage = "O TELEFONE deve conter no máximo 20 caracteres.")]
        public string BENE_NR_TELEFONE3 { get; set; }
        [StringLength(20, ErrorMessage = "O TELEFONE deve conter no máximo 20 caracteres.")]
        public string BENE_NR_TELEFONE4 { get; set; }
        [StringLength(20, ErrorMessage = "O CELULAR deve conter no máximo 20 caracteres.")]
        public string BENE_NR_CELULAR { get; set; }
        [StringLength(20, ErrorMessage = "O CELULAR deve conter no máximo 20 caracteres.")]
        public string BENE_NR_CELULAR2 { get; set; }
        [StringLength(20, ErrorMessage = "O CELULAR deve conter no máximo 20 caracteres.")]
        public string BENE_NR_CELULAR3 { get; set; }
        [StringLength(20, ErrorMessage = "O CELULAR deve conter no máximo 20 caracteres.")]
        public string BENE_NR_CELULAR4 { get; set; }
        [StringLength(150, ErrorMessage = "O E-MAIL deve conter no máximo 150 caracteres.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Deve ser um e-mail válido")]
        public string BENE_EM_EMAIL { get; set; }
        [StringLength(20, ErrorMessage = "O CNPJ deve conter no máximo 20 caracteres.")]
        [CustomValidationCNPJ(ErrorMessage = "CNPJ inválido")]
        public string BENE_NR_CNPJ { get; set; }
        [StringLength(100, ErrorMessage = "O NOME DO PARENTE deve conter no máximo 100 caracteres.")]
        public string BENE_NM_PARENTESCO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BENEFICIARIO_ANEXO> BENEFICIARIO_ANEXO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BENEFICIARIO_ANOTACOES> BENEFICIARIO_ANOTACOES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTATO> CONTATO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMAIL> EMAIL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENDERECO> ENDERECO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELEFONE> TELEFONE { get; set; }
        public virtual ESTADO_CIVIL ESTADO_CIVIL { get; set; }
        public virtual PARENTESCO PARENTESCO { get; set; }
        public virtual SEXO SEXO { get; set; }
        public virtual ESCOLARIDADE ESCOLARIDADE { get; set; }
        public virtual TIPO_PESSOA TIPO_PESSOA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRECATORIO> PRECATORIO { get; set; }
    }
}