using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Web.Common;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using ModelServices.Interfaces.Repositories;
using ApplicationServices.Services;
using ModelServices.EntitiesServices;
using DataServices.Repositories;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Presentation.Start.NinjectWebCommons), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Presentation.Start.NinjectWebCommons), "Stop")]

namespace Presentation.Start
{
    public class NinjectWebCommons
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(typeof(IAppServiceBase<>)).To(typeof(AppServiceBase<>));
            kernel.Bind<IUsuarioAppService>().To<UsuarioAppService>();
            kernel.Bind<ILogAppService>().To<LogAppService>();
            kernel.Bind<IConfiguracaoAppService>().To<ConfiguracaoAppService>();
            kernel.Bind<INotificacaoAppService>().To<NotificacaoAppService>();
            kernel.Bind<ITemplateAppService>().To<TemplateAppService>();
            kernel.Bind<IClienteAppService>().To<ClienteAppService>();
            kernel.Bind<IClienteCnpjAppService>().To<ClienteCnpjAppService>();
            kernel.Bind<IMensagemAppService>().To<MensagemAppService>();
            kernel.Bind<IGrupoAppService>().To<GrupoAppService>();
            kernel.Bind<ICategoriaClienteAppService>().To<CategoriaClienteAppService>();
            kernel.Bind<IAgendaAppService>().To<AgendaAppService>();
            kernel.Bind<ITemplateSMSAppService>().To<TemplateSMSAppService>();
            kernel.Bind<ITemplateEMailAppService>().To<TemplateEMailAppService>();
            kernel.Bind<INoticiaAppService>().To<NoticiaAppService>();
            kernel.Bind<ITarefaAppService>().To<TarefaAppService>();
            kernel.Bind<ITipoPessoaAppService>().To<TipoPessoaAppService>();
            kernel.Bind<ITelefoneAppService>().To<TelefoneAppService>();
            kernel.Bind<ICategoriaTelefoneAppService>().To<CategoriaTelefoneAppService>();
            kernel.Bind<ICargoAppService>().To<CargoAppService>();
            kernel.Bind<IBancoAppService>().To<BancoAppService>();
            kernel.Bind<IDepartamentoAppService>().To<DepartamentoAppService>();
            kernel.Bind<IPeriodicidadeAppService>().To<PeriodicidadeAppService>();
            kernel.Bind<IEscolaridadeAppService>().To<EscolaridadeAppService>();
            kernel.Bind<IParentescoAppService>().To<ParentescoAppService>();
            kernel.Bind<IBeneficiarioAppService>().To<BeneficiarioAppService>();

            kernel.Bind(typeof(IServiceBase<>)).To(typeof(ServiceBase<>));
            kernel.Bind<IUsuarioService>().To<UsuarioService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<IConfiguracaoService>().To<ConfiguracaoService>();
            kernel.Bind<INotificacaoService>().To<NotificacaoService>();
            kernel.Bind<ITemplateService>().To<TemplateService>();
            kernel.Bind<IClienteService>().To<ClienteService>();
            kernel.Bind<IClienteCnpjService>().To<ClienteCnpjService>();
            kernel.Bind<IMensagemService>().To<MensagemService>();
            kernel.Bind<IGrupoService>().To<GrupoService>();
            kernel.Bind<ICategoriaClienteService>().To<CategoriaClienteService>();
            kernel.Bind<IAgendaService>().To<AgendaService>();
            kernel.Bind<ITemplateSMSService>().To<TemplateSMSService>();
            kernel.Bind<ITemplateEMailService>().To<TemplateEMailService>();
            kernel.Bind<INoticiaService>().To<NoticiaService>();
            kernel.Bind<ITarefaService>().To<TarefaService>();
            kernel.Bind<ITipoPessoaService>().To<TipoPessoaService>();
            kernel.Bind<ITelefoneService>().To<TelefoneService>();
            kernel.Bind<ICategoriaTelefoneService>().To<CategoriaTelefoneService>();
            kernel.Bind<ICargoService>().To<CargoService>();
            kernel.Bind<IBancoService>().To<BancoService>();
            kernel.Bind<IDepartamentoService>().To<DepartamentoService>();
            kernel.Bind<IPeriodicidadeService>().To<PeriodicidadeService>();
            kernel.Bind<IEscolaridadeService>().To<EscolaridadeService>();
            kernel.Bind<IParentescoService>().To<ParentescoService>();
            kernel.Bind<IBeneficiarioService>().To<BeneficiarioService>();

            kernel.Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));
            kernel.Bind<IConfiguracaoRepository>().To<ConfiguracaoRepository>();
            kernel.Bind<IUsuarioRepository>().To<UsuarioRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<IPerfilRepository>().To<PerfilRepository>();
            kernel.Bind<ITemplateRepository>().To<TemplateRepository>();
            kernel.Bind<ITipoPessoaRepository>().To<TipoPessoaRepository>();
            kernel.Bind<ICategoriaNotificacaoRepository>().To<CategoriaNotificacaoRepository>();
            kernel.Bind<INotificacaoRepository>().To<NotificacaoRepository>();
            kernel.Bind<INotificacaoAnexoRepository>().To<NotificacaoAnexoRepository>();
            kernel.Bind<IUsuarioAnexoRepository>().To<UsuarioAnexoRepository>();
            kernel.Bind<IUFRepository>().To<UFRepository>();
            kernel.Bind<ICategoriaClienteRepository>().To<CategoriaClienteRepository>();
            //kernel.Bind<IClienteRepository>().To<ClienteRepository>();
            //kernel.Bind<IClienteAnexoRepository>().To<ClienteAnexoRepository>();
            //kernel.Bind<IClienteContatoRepository>().To<ClienteContatoRepository>();
            //kernel.Bind<IClienteCnpjRepository>().To<ClienteCnpjRepository>();
            kernel.Bind<ICargoRepository>().To<CargoRepository>();
            kernel.Bind<IMensagemRepository>().To<MensagemRepository>();
            kernel.Bind<IMensagemDestinoRepository>().To<MensagemDestinoRepository>();
            kernel.Bind<IMensagemAnexoRepository>().To<MensagemAnexoRepository>();
            kernel.Bind<IGrupoRepository>().To<GrupoRepository>();
            kernel.Bind<IGrupoContatoRepository>().To<GrupoContatoRepository>();
            kernel.Bind<ICategoriaAgendaRepository>().To<CategoriaAgendaRepository>();
            kernel.Bind<IAgendaAnexoRepository>().To<AgendaAnexoRepository>();
            kernel.Bind<IAgendaRepository>().To<AgendaRepository>();
            kernel.Bind<ISexoRepository>().To<SexoRepository>();
            kernel.Bind<ITemplateSMSRepository>().To<TemplateSMSRepository>();
            kernel.Bind<ITemplateEMailRepository>().To<TemplateEMailRepository>();
            kernel.Bind<INoticiaRepository>().To<NoticiaRepository>();
            kernel.Bind<INoticiaComentarioRepository>().To<NoticiaComentarioRepository>();
            kernel.Bind<ITarefaRepository>().To<TarefaRepository>();
            kernel.Bind<ITarefaAnexoRepository>().To<TarefaAnexoRepository>();
            kernel.Bind<ITarefaNotificacaoRepository>().To<TarefaNotificacaoRepository>();
            kernel.Bind<ITipoTarefaRepository>().To<TipoTarefaRepository>();
            kernel.Bind<ITelefoneRepository>().To<TelefoneRepository>();
            kernel.Bind<ICategoriaTelefoneRepository>().To<CategoriaTelefoneRepository>();
            kernel.Bind<IPeriodicidadeRepository>().To<PeriodicidadeRepository>();
            kernel.Bind<IBancoRepository>().To<BancoRepository>();
            kernel.Bind<IDepartamentoRepository>().To<DepartamentoRepository>();
            kernel.Bind<IEscolaridadeRepository>().To<EscolaridadeRepository>();
            kernel.Bind<IParentescoRepository>().To<ParentescoRepository>();
            kernel.Bind<IBeneficiarioRepository>().To<BeneficiarioRepository>();
            kernel.Bind<IBeneficiarioAnexoRepository>().To<BeneficiarioAnexoRepository>();
            kernel.Bind<IBeneficiarioComentarioRepository>().To<BeneficiarioComentarioRepository>();

        }
    }
}