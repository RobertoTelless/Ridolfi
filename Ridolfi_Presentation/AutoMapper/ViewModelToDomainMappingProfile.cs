using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EntitiesServices.Model;
using ERP_CRM_Solution.ViewModels;

namespace MvcMapping.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UsuarioViewModel, USUARIO>();
            CreateMap<UsuarioLoginViewModel, USUARIO>();
            CreateMap<LogViewModel, LOG>();
            CreateMap<ConfiguracaoViewModel, CONFIGURACAO>();
            CreateMap<NotificacaoViewModel, NOTIFICACAO>();
            CreateMap<MensagemViewModel, MENSAGENS>();
            CreateMap<GrupoViewModel, GRUPO>();
            CreateMap<GrupoContatoViewModel, GRUPO_CLIENTE>();
            CreateMap<TemplateViewModel, TEMPLATE>();
            CreateMap<AgendaViewModel, AGENDA>();
            CreateMap<NoticiaViewModel, NOTICIA>();
            CreateMap<NoticiaComentarioViewModel, NOTICIA_COMENTARIO>();
            CreateMap<TelefoneViewModel, TELEFONE>();
            CreateMap<TarefaViewModel, TAREFA>();
            CreateMap<TarefaAcompanhamentoViewModel, TAREFA_ACOMPANHAMENTO>();
            CreateMap<ClienteViewModel, CLIENTE>();
            CreateMap<ClienteContatoViewModel, CLIENTE_CONTATO>();
            CreateMap<TemplateSMSViewModel, TEMPLATE_SMS>();
            CreateMap<TemplateEMailViewModel, TEMPLATE_EMAIL>();
            CreateMap<CategoriaClienteViewModel, CATEGORIA_CLIENTE>();
            CreateMap<CargoViewModel, CARGO>();
            CreateMap<BancoViewModel, BANCO>();
            CreateMap<DepartamentoViewModel, DEPARTAMENTO>();
            CreateMap<EscolaridadeViewModel, ESCOLARIDADE>();
            CreateMap<ParentescoViewModel, PARENTESCO>();
            CreateMap<BeneficiarioComentarioViewModel, BENEFICIARIO_ANOTACOES>();
            CreateMap<BeneficiarioViewModel, BENEFICIARIO>();
            CreateMap<TabelaIPCAViewModel, TABELA_IPCA>();
            CreateMap<TabelaIRRFViewModel, TABELA_IRRF>();
            CreateMap<HonorarioComentarioViewModel, HONORARIO_ANOTACOES>();
            CreateMap<HonorarioViewModel, HONORARIO>();

        }
    }
}