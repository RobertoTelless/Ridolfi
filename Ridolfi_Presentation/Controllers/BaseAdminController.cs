using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Interfaces;
using EntitiesServices.Model;
using System.Globalization;
using SMS_Presentation.App_Start;
using EntitiesServices.Work_Classes;
using AutoMapper;
using ERP_CRM_Solution.ViewModels;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using System.Runtime.Caching;
using EntitiesServices.WorkClasses;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using CrossCutting;

namespace ERP_CRM_Solution.Controllers
{
    public class BaseAdminController : Controller
    {
        private readonly IUsuarioAppService baseApp;
        private readonly INoticiaAppService notiApp;
        private readonly ILogAppService logApp;
        private readonly ITarefaAppService tarApp;
        private readonly INotificacaoAppService notfApp;
        private readonly IUsuarioAppService usuApp;
        private readonly IAgendaAppService ageApp;
        private readonly IConfiguracaoAppService confApp;
        private readonly ITipoPessoaAppService tpApp;
        private readonly IClienteAppService cliApp;
        private readonly ITelefoneAppService telApp;
        private readonly IBeneficiarioAppService benApp;
        private readonly IHonorarioAppService honApp;
        private readonly ITRFAppService trfApp;

        private String msg;
        private Exception exception;
        USUARIO objeto = new USUARIO();
        USUARIO objetoAntes = new USUARIO();
        List<USUARIO> listaMaster = new List<USUARIO>();
        String extensao;

        public BaseAdminController(IUsuarioAppService baseApps, ILogAppService logApps, INoticiaAppService notApps, ITarefaAppService tarApps, INotificacaoAppService notfApps, IUsuarioAppService usuApps, IAgendaAppService ageApps, IConfiguracaoAppService confApps, ITipoPessoaAppService tpApps, ITelefoneAppService telApps, IBeneficiarioAppService benApps, IHonorarioAppService honApps, ITRFAppService trfApps, IClienteAppService cliApps)
        {
            baseApp = baseApps;
            logApp = logApps;
            notiApp = notApps;
            tarApp = tarApps;
            notfApp = notfApps;
            usuApp = usuApps;
            ageApp = ageApps;
            confApp = confApps;
            tpApp = tpApps;
            telApp = telApps;
            benApp = benApps;
            honApp = honApps;
            trfApp = trfApps;
            cliApp = cliApps;
        }

        public ActionResult CarregarAdmin()
        {
            Int32? idAss = (Int32)Session["IdAssinante"];
            ViewBag.Usuarios = baseApp.GetAllUsuarios(idAss.Value).Count;
            ViewBag.Logs = logApp.GetAllItens(idAss.Value).Count;
            ViewBag.UsuariosLista = baseApp.GetAllUsuarios(idAss.Value);
            ViewBag.LogsLista = logApp.GetAllItens(idAss.Value);
            return View();

        }

        public ActionResult CarregarLandingPage()
        {
            return View();
        }

        public JsonResult GetRefreshTime()
        {
            return Json(confApp.GetById(1).CONF_NR_REFRESH_DASH);
        }

        public JsonResult GetConfigNotificacoes()
        {
            Int32? idAss = (Int32)Session["IdAssinante"];
            bool hasNotf;
            var hash = new Hashtable();
            USUARIO usu = (USUARIO)Session["Usuario"];
            CONFIGURACAO conf = confApp.GetById(1);

            if (baseApp.GetAllItensUser(usu.USUA_CD_ID, idAss.Value).Count > 0)
            {
                hasNotf = true;
            }
            else
            {
                hasNotf = false;
            }

            hash.Add("CONF_NM_ARQUIVO_ALARME", conf.CONF_NM_ARQUIVO_ALARME);
            hash.Add("NOTIFICACAO", hasNotf);
            return Json(hash);
        }

        public ActionResult MontarTelaDashboardCadastros()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            USUARIO usuario = (USUARIO)Session["UserCredentials"];
            Int32 idAss = (Int32)Session["IdAssinante"];
            UsuarioViewModel vm = Mapper.Map<USUARIO, UsuarioViewModel>(usuario);

            // Carrega valores dos cadastros
            List<BENEFICIARIO> bene = benApp.GetAllItens();
            Int32 beneficiarios = bene.Count;
            List<HONORARIO> hono = honApp.GetAllItens();
            Int32 honorarios = hono.Count;
            Int32 precatorios = 0;
            List<CLIENTE> clie = cliApp.GetAllItens();
            Int32 clientes = clie.Count;
            List<TRF> trfs = trfApp.GetAllItens();
            Int32 trf = trfs.Count;

            Session["Beneficiarios"] = beneficiarios;
            Session["Honorarios"] = honorarios;
            Session["Precatorios"] = precatorios;
            Session["Clientes"] = clientes;
            Session["Trf"] = trf;

            ViewBag.Beneficiarios = beneficiarios;
            ViewBag.Honorarios = honorarios;
            ViewBag.Precatorios = precatorios;
            ViewBag.Clientes = clientes;
            ViewBag.Trf = trf;

            Session["BeneFisica"] = bene.Where(p => p.TIPE_CD_ID == 1).ToList().Count;
            Session["BeneJuridica"] = bene.Where(p => p.TIPE_CD_ID == 2).ToList().Count;
            Session["HonoFisica"] = hono.Where(p => p.TIPE_CD_ID == 1).ToList().Count;
            Session["HonoJuridica"] = hono.Where(p => p.TIPE_CD_ID == 2).ToList().Count;
            return View(vm);
        }

        public JsonResult GetDadosGraficoBeneficiarioTipo()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();

            Int32 q1 = (Int32)Session["BeneFisica"];
            Int32 q2 = (Int32)Session["BeneJuridica"];


            desc.Add("Pessoa Física");
            quant.Add(q1);
            cor.Add("#359E18");
            desc.Add("Pessoa Jurídica");
            quant.Add(q2);
            cor.Add("#FFAE00");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoHonorarioTipo()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();

            Int32 q1 = (Int32)Session["HonoFisica"];
            Int32 q2 = (Int32)Session["HonoJuridica"];


            desc.Add("Pessoa Física");
            quant.Add(q1);
            cor.Add("#359E18");
            desc.Add("Pessoa Jurídica");
            quant.Add(q2);
            cor.Add("#FFAE00");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public ActionResult CarregarBase()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            if ((Int32)Session["Login"] == 1)
            {
                Session["Perfis"] = baseApp.GetAllPerfis();
                Session["Usuarios"] = usuApp.GetAllUsuarios(idAss);
                Session["TiposPessoas"] = tpApp.GetAllItens();
                Session["UFs"] = telApp.GetAllUF();
            }

            Session["MensTarefa"] = 0;
            Session["MensNoticia"] = 0;
            Session["MensNotificacao"] = 0;
            Session["MensUsuario"] = 0;
            Session["MensLog"] = 0;
            Session["MensUsuarioAdm"] = 0;
            Session["MensAgenda"] = 0;
            Session["MensTemplate"] = 0;
            Session["MensConfiguracao"] = 0;
            Session["MensTelefone"] = 0;
            Session["MensBanco"] = 0;
            Session["MensGrupo"] = 0;
            Session["MensCliente"] = 0;
            Session["MensSMSCliente"] = 0;
            Session["ErroSoma"] = 0;
            Session["SMSEMailEnvio"] = 0;
            Session["MensTemplateSMS"] = 0;
            Session["MensPermissao"] = 0;
            Session["MensCatCliente"] = 0;
            Session["MensCargo"] = 0;
            Session["VoltaNotificacao"] = 3;
            Session["VoltaNoticia"] = 1;
            Session["AgendaCorp"] = 0;
            Session["VoltaMensagem"] = 0;
            Session["VoltaAgendaCRMCalend"] = 0;
            Session["MensBeneficiario"] = 0;
            Session["MensHonorario"] = 0;

            USUARIO usu = new USUARIO();
            UsuarioViewModel vm = new UsuarioViewModel();
            List<NOTIFICACAO> noti = new List<NOTIFICACAO>();

            ObjectCache cache = MemoryCache.Default;
            USUARIO usuContent = cache["usuario" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID] as USUARIO;

            if (usuContent == null)
            {
                usu = usuApp.GetItemById(((USUARIO)Session["UserCredentials"]).USUA_CD_ID);
                vm = Mapper.Map<USUARIO, UsuarioViewModel>(usu);
                noti = notfApp.GetAllItens(idAss);
                DateTime expiration = DateTime.Now.AddDays(15);
                cache.Set("usuario" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID, usu, expiration);
                cache.Set("vm" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID, vm, expiration);
                cache.Set("noti" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID, noti, expiration);
            }

            usu = cache.Get("usuario" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID) as USUARIO;
            vm = cache.Get("vm" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID) as UsuarioViewModel;
            noti = cache.Get("noti" + ((USUARIO)Session["UserCredentials"]).USUA_CD_ID) as List<NOTIFICACAO>;
            ViewBag.Perfil = usu.PERFIL.PERF_SG_SIGLA;

            noti = notfApp.GetAllItensUser(usu.USUA_CD_ID, usu.ASSI_CD_ID.Value);
            Session["Notificacoes"] = noti; 
            Session["ListaNovas"] = noti.Where(p => p.NOTI_IN_VISTA == 0).ToList().Take(5).OrderByDescending(p => p.NOTI_DT_EMISSAO).ToList();
            Session["NovasNotificacoes"] = noti.Where(p => p.NOTI_IN_VISTA == 0).Count();
            Session["Nome"] = usu.USUA_NM_NOME;

            Session["Noticias"] = notiApp.GetAllItensValidos(idAss);
            Session["NoticiasNumero"] = ((List<NOTICIA>)Session["Noticias"]).Count;

            Session["ListaPendentes"] = tarApp.GetTarefaStatus(usu.USUA_CD_ID, 1);
            Session["TarefasPendentes"] = ((List<TAREFA>)Session["ListaPendentes"]).Count;
            Session["TarefasLista"] = tarApp.GetByUser(usu.USUA_CD_ID);
            Session["Tarefas"] = ((List<TAREFA>)Session["TarefasLista"]).Count;

            Session["Agendas"] = ageApp.GetByUser(usu.USUA_CD_ID, usu.ASSI_CD_ID.Value);
            Session["NumAgendas"] = ((List<AGENDA>)Session["Agendas"]).Count;
            Session["AgendasHoje"] = ((List<AGENDA>)Session["Agendas"]).Where(p => p.AGEN_DT_DATA == DateTime.Today.Date).ToList();
            Session["NumAgendasHoje"] = ((List<AGENDA>)Session["AgendasHoje"]).Count;

            Session["Telefones"] = telApp.GetAllItens(usu.ASSI_CD_ID.Value);
            Session["NumTelefones"] = ((List<TELEFONES>)Session["Telefones"]).Count;
            Session["Logs"] = usu.LOG.Count;

            String frase = String.Empty;
            String nome = String.Empty;
            Int32 loc = usu.USUA_NM_NOME.IndexOf(" ");
            if (usu.USUA_NM_NOME.IndexOf(" ") != -1)
            {
                nome = usu.USUA_NM_NOME.Substring(0, usu.USUA_NM_NOME.IndexOf(" "));
            }
            else
            {
                nome = usu.USUA_NM_NOME;
            }
            Session["NomeGreeting"] = nome;
            if (DateTime.Now.Hour <= 12)
            {
                frase = "Bom dia, " + nome;
            }
            else if (DateTime.Now.Hour > 12 & DateTime.Now.Hour <= 18)
            {
                frase = "Boa tarde, " + nome;
            }
            else
            {
                frase = "Boa noite, " + nome;
            }
            Session["Greeting"] = frase;
            Session["Foto"] = usu.USUA_AQ_FOTO;

            // Mensagens
            if ((Int32)Session["MensPermissao"] == 2)
            {
                ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            Session["MensPermissao"] = 0;
            ViewBag.Especial = vm.USUA_IN_ESPECIAL;
            return View(vm);
        }

        public ActionResult CarregarDesenvolvimento()
        {
            return View();
        }

        public ActionResult VoltarDashboard()
        {
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult MontarFaleConosco()
        {
            return View();
        }

        public ActionResult MontarCentralMensagens()
        {
            // Verifica se tem usuario logado
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA == "VIS")
                {
                    Session["MensCRM"] = 2;
                    return RedirectToAction("VoltarAcompanhamentoCRM");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Cria Base de mensagens
            CONFIGURACAO conf = confApp.GetItemById(idAss);
            List<MensagemWidgetViewModel> listaMensagens = new List<MensagemWidgetViewModel>();
            if (Session["ListaMensagem"] == null)
            {
                // Carrega notificações
                List<NOTIFICACAO> notificacoes = (List<NOTIFICACAO>)Session["Notificacoes"];
                List<NOTIFICACAO> naoLidas = notificacoes.Where(p => p.NOTI_IN_VISTA == 0).ToList();
                foreach (NOTIFICACAO item in naoLidas)
                {
                    MensagemWidgetViewModel mens = new MensagemWidgetViewModel();
                    mens.DataMensagem = item.NOTI_DT_EMISSAO.Value;
                    mens.Descrição = item.NOTI_NM_TITULO;
                    mens.FlagUrgencia = 1;
                    mens.IdMensagem = item.NOTI_CD_ID;
                    mens.NomeUsuario = usuario.USUA_NM_NOME;
                    mens.TipoMensagem = 1;
                    mens.Categoria = item.CATEGORIA_NOTIFICACAO.CANO_NM_NOME;
                    mens.NomeCliente = item.NOTI_IN_VISTA == 1 ? "Lida" : "Não Lida";
                    listaMensagens.Add(mens);
                }

                // Carrega Agenda
                List<AGENDA> agendas = (List<AGENDA>)Session["Agendas"];
                List<AGENDA> hoje = agendas.Where(p => p.AGEN_DT_DATA == DateTime.Today.Date).ToList();
                foreach (AGENDA item in hoje)
                {
                    MensagemWidgetViewModel mens = new MensagemWidgetViewModel();
                    mens.DataMensagem = item.AGEN_DT_DATA;
                    mens.Descrição = item.AGEN_NM_TITULO;
                    mens.FlagUrgencia = 1;
                    mens.IdMensagem = item.AGEN_CD_ID;
                    mens.NomeUsuario = usuario.USUA_NM_NOME;
                    mens.TipoMensagem = 2;
                    mens.Categoria = item.CATEGORIA_AGENDA.CAAG_NM_NOME;
                    mens.NomeCliente = item.AGEN_IN_STATUS == 1 ? "Ativa" : (item.AGEN_IN_STATUS == 2 ? "Suspensa" : "Encerrada");
                    listaMensagens.Add(mens);
                }

                // Carrega Tarefas
                List<TAREFA> tarefas = (List<TAREFA>)Session["ListaPendentes"];
                foreach (TAREFA item in tarefas)
                {
                    MensagemWidgetViewModel mens = new MensagemWidgetViewModel();
                    mens.DataMensagem = item.TARE_DT_ESTIMADA.Value;
                    mens.Descrição = item.TARE_NM_TITULO;
                    mens.FlagUrgencia = 1;
                    mens.IdMensagem = item.TARE_CD_ID;
                    mens.NomeUsuario = usuario.USUA_NM_NOME;
                    mens.TipoMensagem = 3;
                    mens.Categoria = item.TIPO_TAREFA.TITR_NM_NOME;
                    if (item.TARE_IN_STATUS == 1)
                    {
                        mens.NomeCliente = "Ativa";

                    }
                    else if (item.TARE_IN_STATUS == 2)
                    {
                        mens.NomeCliente = "Em Andamento";

                    }
                    else if (item.TARE_IN_STATUS == 3)
                    {
                        mens.NomeCliente = "Suspensa";

                    }
                    else if (item.TARE_IN_STATUS == 4)
                    {
                        mens.NomeCliente = "Cancelada";

                    }
                    else if (item.TARE_IN_STATUS == 5)
                    {
                        mens.NomeCliente = "Encerrada";

                    }
                    listaMensagens.Add(mens);
                }
                Session["ListaMensagem"] = listaMensagens;
            }
            else
            {
                listaMensagens = (List<MensagemWidgetViewModel>)Session["ListaMensagem"];
            }

            // Prepara listas dos filtros
            List<SelectListItem> tipos = new List<SelectListItem>();
            tipos.Add(new SelectListItem() { Text = "Notificações", Value = "1" });
            tipos.Add(new SelectListItem() { Text = "Agenda", Value = "2" });
            tipos.Add(new SelectListItem() { Text = "Tarefas", Value = "3" });
            ViewBag.Tipos = new SelectList(tipos, "Value", "Text");
            List<SelectListItem> urg = new List<SelectListItem>();
            urg.Add(new SelectListItem() { Text = "Sim", Value = "1" });
            urg.Add(new SelectListItem() { Text = "Não", Value = "2" });
            ViewBag.Urgencia = new SelectList(urg, "Value", "Text");

            // Exibe
            ViewBag.ListaMensagem = listaMensagens;
            MensagemWidgetViewModel mod = new MensagemWidgetViewModel();
            return View(mod);
        }


        public ActionResult VerNotificacaoBase(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["VoltaNotificacao"] = 10;
            return RedirectToAction("VerNotificacao", "Notificacao", new { id = id });
        }

        public ActionResult VerAgendaBase(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["VoltaAgenda"] = 10;
            Session["VoltaAgendaCRM"] = 0;
            return RedirectToAction("EditarAgenda", "Agenda", new { id = id });
        }

        public ActionResult VerTarefaBase(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["VoltaTarefa"] = 10;
            Session["VoltaAgendaCRM"] = 0;
            return RedirectToAction("EditarTarefa", "Tarefa", new { id = id });
        }

        public ActionResult RetirarFiltroCentralMensagens()
        {

            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaMensagem"] = null;
            return RedirectToAction("MontarCentralMensagens");
        }

        [HttpPost]
        public ActionResult FiltrarCentralMensagens(MensagemWidgetViewModel item)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            try
            {
                // Executa a operação
                List<MensagemWidgetViewModel> listaObj = (List<MensagemWidgetViewModel>)Session["ListaMensagem"];
                if (item.TipoMensagem != null)
                {
                    listaObj = listaObj.Where(p => p.TipoMensagem == item.TipoMensagem).ToList();
                }
                if (item.Descrição != null)
                {
                    listaObj = listaObj.Where(p => p.Descrição.Contains(item.Descrição)).ToList();
                }
                if (item.DataMensagem != null)
                {
                    listaObj = listaObj.Where(p => p.DataMensagem == item.DataMensagem).ToList();
                }
                if (item.FlagUrgencia != null)
                {
                    listaObj = listaObj.Where(p => p.FlagUrgencia == item.FlagUrgencia).ToList();
                }
                Session["ListaMensagem"] = listaObj;

                // Sucesso
                return RedirectToAction("MontarCentralMensagens");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarCentralMensagens");
            }
        }
    }
}