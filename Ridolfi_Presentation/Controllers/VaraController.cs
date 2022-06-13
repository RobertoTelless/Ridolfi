using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Interfaces;
using EntitiesServices.Model;
using System.Globalization;
using SMS_Presentation.App_Start;
using EntitiesServices.WorkClasses;
using AutoMapper;
using ERP_CRM_Solution.ViewModels;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using System.Web.UI.WebControls;
using System.Runtime.Caching;
using Image = iTextSharp.text.Image;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Canducci.Zip;
using CrossCutting;

namespace SMS_Presentation.Controllers
{
    public class VaraController : Controller
    {
        private readonly IVaraAppService fornApp;
        private readonly ILogAppService logApp;
        private readonly IConfiguracaoAppService confApp;

        private String msg;
        private Exception exception;
        VARA objetoForn = new VARA();
        VARA objetoFornAntes = new VARA();
        List<VARA> listaMasterForn = new List<VARA>();
        String extensao;

        public VaraController(IVaraAppService fornApps, ILogAppService logApps, IConfiguracaoAppService confApps)
        {
            fornApp = fornApps;
            logApp = logApps;
            confApp = confApps;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return View();
        }

        public ActionResult Voltar()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult VoltarGeral()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        [HttpGet]
        public ActionResult MontarTelaVARA()
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
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Carrega listas
            if (Session["ListaVara"] == null)
            {
                listaMasterForn = fornApp.GetAllItens();
                Session["ListaVara"] = listaMasterForn;
            }
            ViewBag.Listas = (List<VARA>)Session["ListaVara"];
            ViewBag.Title = "Vara";
            ViewBag.TRF = new SelectList(fornApp.GetAllTRF(), "TRF_CD_ID", "TRF_NM_NOME");

            // Indicadores
            ViewBag.Vara = ((List<VARA>)Session["ListaVara"]).Count;
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensVara"] != null)
            {
                if ((Int32)Session["MensVara"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensVara"] == 3)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0150", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensVara"] == 5)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0149", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            objetoForn = new VARA();
            objetoForn.VARA_IN_ATIVO = 1;
            Session["MensVara"] = 0;
            return View(objetoForn);
        }

        public ActionResult RetirarFiltroVara()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaVara"] = null;
            Session["FiltroVara"] = null;
            return RedirectToAction("MontarTelaVara");
        }

        public ActionResult MostrarTudoVara()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterForn = fornApp.GetAllItensAdm();
            Session["FiltroVara"] = null;
            Session["ListaVara"] = listaMasterForn;
            return RedirectToAction("MontarTelaVara");
        }

        [HttpPost]
        public ActionResult FiltrarVara(VARA item)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            try
            {
                // Executa a operação
                Int32 idAss = (Int32)Session["IdAssinante"];
                List<VARA> listaObj = new List<VARA>();
                Session["FiltroVara"] = item;
                Int32 volta = fornApp.ExecuteFilter(item.VARA_NM_NOME, item.TRF1_CD_ID, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensVara"] = 1;
                }

                // Sucesso
                listaMasterForn = listaObj;
                Session["ListaVara"] = listaObj;
                return RedirectToAction("MontarTelaVara");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaVara");
            }
        }

        public ActionResult VoltarBaseVara()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaVara");
        }

        [HttpGet]
        public ActionResult IncluirVara()
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
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara listas
            ViewBag.TRF = new SelectList(fornApp.GetAllTRF(), "TRF_CD_ID", "TRF_NM_NOME");

            // Prepara view
            VARA item = new VARA();
            VaraViewModel vm = Mapper.Map<VARA, VaraViewModel>(item);
            vm.VARA_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirVara(VaraViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.TRF = new SelectList(fornApp.GetAllTRF(), "TRF_CD_ID", "TRF_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    VARA item = Mapper.Map<VaraViewModel, VARA>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 volta = fornApp.ValidateCreate(item, usuarioLogado);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensVara"] = 3;
                        return RedirectToAction("MontarTelaVara", "Vara");
                    }

                    // Sucesso
                    listaMasterForn = new List<VARA>();
                    Session["ListaVara"] = null;
                    Session["Vara"] = fornApp.GetAllItens();
                    Session["IdVolta"] = item.VARA_CD_ID;
                    return RedirectToAction("MontarTelaVara");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult EditarVara(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA == "FUN")
                {
                    Session["MensVara"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara view
            ViewBag.TRF = new SelectList(fornApp.GetAllTRF(), "TRF_CD_ID", "TRF_NM_NOME");

            VARA item = fornApp.GetItemById(id);
            objetoFornAntes = item;
            Session["Vara"] = item;
            Session["IdVolta"] = id;
            Session["IdVara"] = id;
            VaraViewModel vm = Mapper.Map<VARA, VaraViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarVara(VaraViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.TRF = new SelectList(fornApp.GetAllTRF(), "TRF_CD_ID", "TRF_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    VARA item = Mapper.Map<VaraViewModel, VARA>(vm);
                    Int32 volta = fornApp.ValidateEdit(item, objetoFornAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    listaMasterForn = new List<VARA>();
                    Session["ListaVara"] = null;
                    return RedirectToAction("MontarTelaVara");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult VerVara(Int32 id)
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
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara view
            ViewBag.Incluir = (Int32)Session["IncluirForn"];

            VARA item = fornApp.GetItemById(id);
            objetoFornAntes = item;
            Session["Vara"] = item;
            Session["IdVolta"] = id;
            Session["IdVara"] = id;
            VaraViewModel vm = Mapper.Map<VARA, VaraViewModel>(item);
            return View(vm);
        }

        [HttpGet]
        public ActionResult ExcluirVara(Int32 id)
        {
            // Valida acesso
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            VARA item = fornApp.GetItemById(id);
            objetoFornAntes = (VARA)Session["Vara"];
            item.VARA_IN_ATIVO = 0;
            Int32 volta = fornApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensVara"] = 5;
                return RedirectToAction("MontarTelaVara", "Vara");
            }
            listaMasterForn = new List<VARA>();
            Session["ListaVara"] = null;
            Session["FiltroVara"] = null;
            return RedirectToAction("MontarTelaVara");
        }

        [HttpGet]
        public ActionResult ReativarVara(Int32 id)
        {
            // Valida acesso
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            VARA item = fornApp.GetItemById(id);
            objetoFornAntes = (VARA)Session["Vara"];
            item.VARA_IN_ATIVO = 1;
            Int32 volta = fornApp.ValidateReativar(item, usuario);
            listaMasterForn = new List<VARA>();
            Session["ListaVara"] = null;
            Session["FiltroVara"] = null;
            return RedirectToAction("MontarTelaVara");
        }
    }
}