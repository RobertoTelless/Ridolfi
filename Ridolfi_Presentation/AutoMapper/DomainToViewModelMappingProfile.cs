using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EntitiesServices.Model;
using ERP_CRM_Solution.ViewModels;

namespace MvcMapping.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<USUARIO, UsuarioViewModel>();
            CreateMap<USUARIO, UsuarioLoginViewModel>();
            CreateMap<LOG, LogViewModel>();
            CreateMap<CONFIGURACAO, ConfiguracaoViewModel>();
            CreateMap<NOTIFICACAO, NotificacaoViewModel>();
            CreateMap<MENSAGENS, MensagemViewModel>();
            CreateMap<GRUPO, GrupoViewModel>();
            CreateMap<GRUPO_CLIENTE, GrupoContatoViewModel>();
            CreateMap<TEMPLATE, TemplateViewModel>();
            CreateMap<AGENDA, AgendaViewModel>();
            CreateMap<NOTICIA, NoticiaViewModel>();
            CreateMap<NOTICIA_COMENTARIO, NoticiaComentarioViewModel>();
            CreateMap<TELEFONE, TelefoneViewModel>();
            CreateMap<TAREFA, TarefaViewModel>();
            CreateMap<TAREFA_ACOMPANHAMENTO, TarefaAcompanhamentoViewModel>();
            CreateMap<CLIENTE, ClienteViewModel>();
            CreateMap<CLIENTE_CONTATO, ClienteContatoViewModel>();
            CreateMap<TEMPLATE_SMS, TemplateSMSViewModel>();
            CreateMap<TEMPLATE_EMAIL, TemplateEMailViewModel>();
            CreateMap<CATEGORIA_CLIENTE, CategoriaClienteViewModel>();
            CreateMap<CARGO, CargoViewModel>();
            CreateMap<BANCO, BancoViewModel>();
            CreateMap<DEPARTAMENTO, DepartamentoViewModel>();
            CreateMap<ESCOLARIDADE, EscolaridadeViewModel>();
            CreateMap<PARENTESCO, ParentescoViewModel>();
            CreateMap<BENEFICIARIO, BeneficiarioViewModel>();
            CreateMap<BENEFICIARIO_ANOTACOES, BeneficiarioComentarioViewModel>();
            CreateMap<TABELA_IPCA, TabelaIPCAViewModel>();
            CreateMap<TABELA_IRRF, TabelaIRRFViewModel>();
            CreateMap<HONORARIO_ANOTACOES, HonorarioComentarioViewModel>();
            CreateMap<HONORARIO, HonorarioViewModel>();
            CreateMap<CONTATO, ContatoViewModel>();
            CreateMap<EMAIL, EMailViewModel>();
            CreateMap<ENDERECO, EnderecoViewModel>();
            CreateMap<TELEFONE, TelefoneBenefViewModel>();
            CreateMap<TRF, TRFViewModel>();
            CreateMap<VARA, VaraViewModel>();
            CreateMap<CLIENTE, ClienteViewModel>();
            CreateMap<CLIENTE_CONTATO, ClienteContatoViewModel>();
            CreateMap<CLIENTE_ANOTACAO, ClienteAnotacaoViewModel>();

        }
    }
}
