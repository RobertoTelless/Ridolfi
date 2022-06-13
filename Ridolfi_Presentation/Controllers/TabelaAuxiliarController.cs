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
using Correios;
using Canducci.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using EntitiesServices.Attributes;
using OfficeOpenXml.Table;
using EntitiesServices.WorkClasses;
using System.Threading.Tasks;
using CrossCutting;
using System.Net.Mail;
using System.Net.Http;


namespace ERP_CRM_Solution.Controllers
{
    public class TabelasAuxiliaresController : Controller
    {
        private readonly ICategoriaClienteAppService ccApp;
        private readonly ICargoAppService caApp;
        private readonly IDepartamentoAppService depApp;
        private readonly IEscolaridadeAppService escApp;
        private readonly IParentescoAppService parApp;

        private String msg;
        private Exception exception;
        CATEGORIA_CLIENTE objetoCC = new CATEGORIA_CLIENTE();
        CATEGORIA_CLIENTE objetoCCAntes = new CATEGORIA_CLIENTE();
        List<CATEGORIA_CLIENTE> listaMasterCC = new List<CATEGORIA_CLIENTE>();
        CARGO objetoCG = new CARGO();
        CARGO objetoCGAntes = new CARGO();
        List<CARGO> listaMasterCG = new List<CARGO>();
        DEPARTAMENTO objetoDEP = new DEPARTAMENTO();
        DEPARTAMENTO objetoDEPAntes = new DEPARTAMENTO();
        List<DEPARTAMENTO> listaMasterDEP = new List<DEPARTAMENTO>();
        ESCOLARIDADE objetoESC = new ESCOLARIDADE();
        ESCOLARIDADE objetoESCAntes = new ESCOLARIDADE();
        List<ESCOLARIDADE> listaMasterESC = new List<ESCOLARIDADE>();
        PARENTESCO objetoPAR = new PARENTESCO();
        PARENTESCO objetoPARAntes = new PARENTESCO();
        List<PARENTESCO> listaMasterPAR = new List<PARENTESCO>();

        public TabelasAuxiliaresController(ICategoriaClienteAppService ccApps, ICargoAppService caApps, IDepartamentoAppService depApps, IEscolaridadeAppService escApps, IParentescoAppService parApps)
        {
            ccApp = ccApps;
            caApp = caApps;
            depApp = depApps;
            escApp = escApps;
            parApp = parApps;
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

            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        [HttpGet]
        public ActionResult MontarTelaCatCliente()
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
                    Session["MensPermissao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];


            // Carrega listas
            if ((List<CATEGORIA_CLIENTE>)Session["ListaCatCliente"] == null)
            {
                listaMasterCC = ccApp.GetAllItens();
                Session["ListaCatCliente"] = listaMasterCC;
            }
            ViewBag.Listas = (List<CATEGORIA_CLIENTE>)Session["ListaCatCliente"];
            Session["CatCliente"] = null;

            // Indicadores
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensCatCliente"] != null)
            {
                if ((Int32)Session["MensCatCliente"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensCatCliente"] == 3)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0066", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensCatCliente"] == 4)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0069", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            Session["VoltaCatCliente"] = 1;
            objetoCC = new CATEGORIA_CLIENTE();
            return View(objetoCC);
        }

        public ActionResult RetirarFiltroCatCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaCatCliente"] = null;
            return RedirectToAction("MontarTelaCatCliente");
        }

        public ActionResult MostrarTudoCatCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterCC = ccApp.GetAllItensAdm();
            Session["ListaCatCliente"] = listaMasterCC;
            return RedirectToAction("MontarTelaCatCliente");
        }

        public ActionResult VoltarBaseCatCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            return RedirectToAction("MontarTelaCatCliente");
        }

        [HttpGet]
        public ActionResult IncluirCatCliente()
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCatCliente"] = 2;
                    return RedirectToAction("MontarTelaCatCliente");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara listas

            // Prepara view
            CATEGORIA_CLIENTE item = new CATEGORIA_CLIENTE();
            CategoriaClienteViewModel vm = Mapper.Map<CATEGORIA_CLIENTE, CategoriaClienteViewModel>(item);
            vm.CACL_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirCatCliente(CategoriaClienteViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CATEGORIA_CLIENTE item = Mapper.Map<CategoriaClienteViewModel, CATEGORIA_CLIENTE>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = ccApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensCatCliente"] = 3;
                        return RedirectToAction("MontarTelaCatCliente");
                    }

                    // Sucesso
                    listaMasterCC = new List<CATEGORIA_CLIENTE>();
                    Session["ListaCatCliente"] = null;
                    Session["IdCatCliente"] = item.CACL_CD_ID;
                    return RedirectToAction("MontarTelaCatCliente");
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
        public ActionResult EditarCatCliente(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCatCliente"] = 2;
                    return RedirectToAction("MontarTelaCatCliente");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            CATEGORIA_CLIENTE item = ccApp.GetItemById(id);
            Session["CatCliente"] = item;

            // Indicadores

            // Mensagens
            if (Session["MensCatCliente"] != null)
            {


            }

            objetoCCAntes = item;
            Session["IdCatCliente"] = id;
            CategoriaClienteViewModel vm = Mapper.Map<CATEGORIA_CLIENTE, CategoriaClienteViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarCatCliente(CategoriaClienteViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    CATEGORIA_CLIENTE item = Mapper.Map<CategoriaClienteViewModel, CATEGORIA_CLIENTE>(vm);
                    Int32 volta = ccApp.ValidateEdit(item, objetoCCAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterCC = new List<CATEGORIA_CLIENTE>();
                    Session["ListaCatCliente"] = null;
                    return RedirectToAction("MontarTelaCatCliente");
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

        public ActionResult VoltarAnexoCatCliente()
        {

            return RedirectToAction("EditarCatCliente", new { id = (Int32)Session["IdCatCliente"] });
        }

        [HttpGet]
        public ActionResult ExcluirCatCliente(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCatCliente"] = 2;
                    return RedirectToAction("MontarTelaCatCliente");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            CATEGORIA_CLIENTE item = ccApp.GetItemById(id);
            objetoCCAntes = (CATEGORIA_CLIENTE)Session["CatCliente"];
            item.CACL_IN_ATIVO = 0;
            Int32 volta = ccApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensCatCliente"] = 4;
                return RedirectToAction("MontarTelaCatCliente");
            }
            Session["ListaCatCliente"] = null;
            return RedirectToAction("MontarTelaCatCliente");
        }

        [HttpGet]
        public ActionResult ReativarCatCliente(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCatCliente"] = 2;
                    return RedirectToAction("MontarTelaCatCliente");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            CATEGORIA_CLIENTE item = ccApp.GetItemById(id);
            objetoCCAntes = (CATEGORIA_CLIENTE)Session["CatCliente"];
            item.CACL_IN_ATIVO = 1;
            Int32 volta = ccApp.ValidateReativar(item, usuario);
            Session["ListaCatCliente"] = null;
            return RedirectToAction("MontarTelaCatCliente");
        }

        [HttpGet]
        public ActionResult MontarTelaCargo()
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
                    Session["MensPermissao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];


            // Carrega listas
            if ((List<CARGO>)Session["ListaCargo"] == null)
            {
                listaMasterCG = caApp.GetAllItens(idAss);
                Session["ListaCargo"] = listaMasterCG;
            }
            ViewBag.Listas = (List<CARGO>)Session["ListaCargo"];
            Session["Cargo"] = null;

            // Indicadores
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensCargo"] != null)
            {
                if ((Int32)Session["MensCargo"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensCargo"] == 3)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0067", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensCargo"] == 4)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0068", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            Session["VoltaCargo"] = 1;
            objetoCG = new CARGO();
            return View(objetoCG);
        }

        public ActionResult RetirarFiltroCargo()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaCargo"] = null;
            return RedirectToAction("MontarTelaCargo");
        }

        public ActionResult MostrarTudoCargo()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterCG = caApp.GetAllItensAdm(idAss);
            Session["ListaCargo"] = listaMasterCG;
            return RedirectToAction("MontarTelaCargo");
        }

        public ActionResult VoltarBaseCargo()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            return RedirectToAction("MontarTelaCargo");
        }

        [HttpGet]
        public ActionResult IncluirCargo()
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCargo"] = 2;
                    return RedirectToAction("MontarTelaCargo");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara listas

            // Prepara view
            CARGO item = new CARGO();
            CargoViewModel vm = Mapper.Map<CARGO, CargoViewModel>(item);
            vm.CARG_IN_ATIVO = 1;
            vm.CARG_IN_TIPO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirCargo(CargoViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CARGO item = Mapper.Map<CargoViewModel, CARGO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = caApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensCargo"] = 3;
                        return RedirectToAction("MontarTelaCargo");
                    }

                    // Sucesso
                    listaMasterCC = new List<CATEGORIA_CLIENTE>();
                    Session["ListaCargo"] = null;
                    Session["IdCargo"] = item.CARG_CD_ID;
                    return RedirectToAction("MontarTelaCargo");
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
        public ActionResult EditarCargo(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCargo"] = 2;
                    return RedirectToAction("MontarTelaCargo");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            CARGO item = caApp.GetItemById(id);
            Session["Cargo"] = item;

            // Indicadores

            // Mensagens
            if (Session["MensCargo"] != null)
            {


            }

            objetoCGAntes = item;
            Session["IdCargo"] = id;
            CargoViewModel vm = Mapper.Map<CARGO, CargoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarCargo(CargoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    CARGO item = Mapper.Map<CargoViewModel, CARGO>(vm);
                    Int32 volta = caApp.ValidateEdit(item, objetoCGAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterCG = new List<CARGO>();
                    Session["ListaCargo"] = null;
                    return RedirectToAction("MontarTelaCargo");
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

        public ActionResult VoltarAnexoCargo()
        {

            return RedirectToAction("EditarCargo", new { id = (Int32)Session["IdCargo"] });
        }

        [HttpGet]
        public ActionResult ExcluirCargo(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCargo"] = 2;
                    return RedirectToAction("MontarTelaCargo");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            CARGO item = caApp.GetItemById(id);
            objetoCGAntes = (CARGO)Session["Cargo"];
            item.CARG_IN_ATIVO = 0;
            Int32 volta = caApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensCargo"] = 4;
                return RedirectToAction("MontarTelaCargo");
            }
            Session["ListaCargo"] = null;
            return RedirectToAction("MontarTelaCargo");
        }

        [HttpGet]
        public ActionResult ReativarCargo(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensCargo"] = 2;
                    return RedirectToAction("MontarTelaCargo");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            CARGO item = caApp.GetItemById(id);
            objetoCGAntes = (CARGO)Session["Cargo"];
            item.CARG_IN_ATIVO = 1;
            Int32 volta = caApp.ValidateReativar(item, usuario);
            Session["ListaCargo"] = null;
            return RedirectToAction("MontarTelaCargo");
        }

        [HttpGet]
        public ActionResult MontarTelaDepartamento()
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
                    Session["MensPermissao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];


            // Carrega listas
            if ((List<DEPARTAMENTO>)Session["ListaDepartamento"] == null)
            {
                listaMasterDEP = depApp.GetAllItens(idAss);
                Session["ListaDepartamento"] = listaMasterDEP;
            }
            ViewBag.Listas = (List<DEPARTAMENTO>)Session["ListaDepartamento"];
            Session["Departamento"] = null;

            // Indicadores
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensDepartamento"] != null)
            {
                if ((Int32)Session["MensDepartamento"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensDepartamento"] == 3)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0120", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensDepartamento"] == 4)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0121", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            Session["VoltaDepartamento"] = 1;
            objetoDEP = new DEPARTAMENTO();
            return View(objetoDEP);
        }

        public ActionResult RetirarFiltroDepartamento()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaDepartamento"] = null;
            return RedirectToAction("MontarTelaDepartamento");
        }

        public ActionResult MostrarTudoDepartamento()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterDEP = depApp.GetAllItensAdm(idAss);
            Session["ListaDepartamento"] = listaMasterDEP;
            return RedirectToAction("MontarTelaDepartamento");
        }

        public ActionResult VoltarBaseDepartamento()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            return RedirectToAction("MontarTelaDepartamento");
        }

        [HttpGet]
        public ActionResult IncluirDepartamento()
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensDepartamento"] = 2;
                    return RedirectToAction("MontarTelaDepartamento");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara listas

            // Prepara view
            DEPARTAMENTO item = new DEPARTAMENTO();
            DepartamentoViewModel vm = Mapper.Map<DEPARTAMENTO, DepartamentoViewModel>(item);
            vm.DEPT_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirDepartamento(DepartamentoViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    DEPARTAMENTO item = Mapper.Map<DepartamentoViewModel, DEPARTAMENTO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = depApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensDepartamento"] = 3;
                        return RedirectToAction("MontarTelaDepartamento");
                    }

                    // Sucesso
                    listaMasterDEP = new List<DEPARTAMENTO>();
                    Session["ListaDepartamento"] = null;
                    Session["IdDepartamento"] = item.DEPT_CD_ID;
                    return RedirectToAction("MontarTelaDepartamento");
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
        public ActionResult EditarDepartamento(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensDepartamento"] = 2;
                    return RedirectToAction("MontarTelaDepartamento");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            DEPARTAMENTO item = depApp.GetItemById(id);
            Session["Departamento"] = item;

            // Indicadores

            // Mensagens
            if (Session["MensDepartamento"] != null)
            {


            }

            objetoDEPAntes = item;
            Session["IdDepartamento"] = id;
            DepartamentoViewModel vm = Mapper.Map<DEPARTAMENTO, DepartamentoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarDepartamento(DepartamentoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    DEPARTAMENTO item = Mapper.Map<DepartamentoViewModel, DEPARTAMENTO>(vm);
                    Int32 volta = depApp.ValidateEdit(item, objetoDEPAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterDEP = new List<DEPARTAMENTO>();
                    Session["ListaDepartamento"] = null;
                    return RedirectToAction("MontarTelaDepartamento");
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

        public ActionResult VoltarAnexoDepartamento()
        {

            return RedirectToAction("EditarDepartamento", new { id = (Int32)Session["IdDepartamento"] });
        }

        [HttpGet]
        public ActionResult ExcluirDepartamento(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensDepartamento"] = 2;
                    return RedirectToAction("MontarTelaDepartamento");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            DEPARTAMENTO item = depApp.GetItemById(id);
            objetoDEPAntes = (DEPARTAMENTO)Session["Departamento"];
            item.DEPT_IN_ATIVO = 0;
            Int32 volta = depApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensDepartamento"] = 4;
                return RedirectToAction("MontarTelaDepartamento");
            }
            Session["ListaDepartamento"] = null;
            return RedirectToAction("MontarTelaDepartamento");
        }

        [HttpGet]
        public ActionResult ReativarDepartamento(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensDepartamento"] = 2;
                    return RedirectToAction("MontarTelaDepartamento");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            DEPARTAMENTO item = depApp.GetItemById(id);
            objetoDEPAntes = (DEPARTAMENTO)Session["Departamento"];
            item.DEPT_IN_ATIVO = 1;
            Int32 volta = depApp.ValidateReativar(item, usuario);
            Session["ListaDepartamento"] = null;
            return RedirectToAction("MontarTelaDepartamento");
        }

        [HttpGet]
        public ActionResult MontarTelaEscolaridade()
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
                    Session["MensPermissao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];


            // Carrega listas
            if ((List<ESCOLARIDADE>)Session["ListaEscolaridade"] == null)
            {
                listaMasterESC = escApp.GetAllItens();
                Session["ListaEscolaridade"] = listaMasterESC;
            }
            ViewBag.Listas = (List<ESCOLARIDADE>)Session["ListaEscolaridade"];
            Session["Escolaridade"] = null;

            // Indicadores
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensEscolaridade"] != null)
            {
                if ((Int32)Session["MensEscolaridade"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensEscolaridade"] == 3)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0141", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensEscolaridade"] == 4)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0142", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            Session["VoltaEscolaridade"] = 1;
            objetoESC = new ESCOLARIDADE();
            return View(objetoESC);
        }

        public ActionResult RetirarFiltroEscolaridade()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaEscolaridade"] = null;
            return RedirectToAction("MontarTelaEscolaridade");
        }

        public ActionResult MostrarTudoEscolaridade()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterESC = escApp.GetAllItensAdm();
            Session["ListaEscolaridade"] = listaMasterESC;
            return RedirectToAction("MontarTelaEscolaridade");
        }

        public ActionResult VoltarBaseEscolaridade()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            return RedirectToAction("MontarTelaEscolaridade");
        }

        [HttpGet]
        public ActionResult IncluirEscolaridade()
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensEscolaridade"] = 2;
                    return RedirectToAction("MontarTelaEscolaridade");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara listas

            // Prepara view
            ESCOLARIDADE item = new ESCOLARIDADE();
            EscolaridadeViewModel vm = Mapper.Map<ESCOLARIDADE, EscolaridadeViewModel>(item);
            vm.ESCO_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirEscolaridade(EscolaridadeViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    ESCOLARIDADE item = Mapper.Map<EscolaridadeViewModel, ESCOLARIDADE>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = escApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensEscolaridade"] = 3;
                        return RedirectToAction("MontarTelaEscolaridade");
                    }

                    // Sucesso
                    listaMasterESC = new List<ESCOLARIDADE>();
                    Session["ListaEscolaridade"] = null;
                    Session["IdEscolaridade"] = item.ESCO_CD_ID;
                    return RedirectToAction("MontarTelaEscolaridade");
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
        public ActionResult EditarEscolaridade(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensEscolaridade"] = 2;
                    return RedirectToAction("MontarTelaEscolaridade");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            ESCOLARIDADE item = escApp.GetItemById(id);
            Session["Escolaridade"] = item;

            // Indicadores

            // Mensagens
            if (Session["MensEscolaridade"] != null)
            {


            }

            objetoESCAntes = item;
            Session["IdEscolaridade"] = id;
            EscolaridadeViewModel vm = Mapper.Map<ESCOLARIDADE, EscolaridadeViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarEscolaridade(EscolaridadeViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    ESCOLARIDADE item = Mapper.Map<EscolaridadeViewModel, ESCOLARIDADE>(vm);
                    Int32 volta = escApp.ValidateEdit(item, objetoESCAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterESC = new List<ESCOLARIDADE>();
                    Session["ListaEscolaridade"] = null;
                    return RedirectToAction("MontarTelaEscolaridade");
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

        public ActionResult VoltarAnexoEscolaridade()
        {

            return RedirectToAction("EditarEscolaridade", new { id = (Int32)Session["IdEscolaridade"] });
        }

        [HttpGet]
        public ActionResult ExcluirEscolaridade(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensEscolaridade"] = 2;
                    return RedirectToAction("MontarTelaEscolaridade");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            ESCOLARIDADE item = escApp.GetItemById(id);
            objetoESCAntes = (ESCOLARIDADE)Session["Escolaridade"];
            item.ESCO_IN_ATIVO = 0;
            Int32 volta = escApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensEscolaridade"] = 4;
                return RedirectToAction("MontarTelaEscolaridade");
            }
            Session["ListaEscolaridade"] = null;
            return RedirectToAction("MontarTelaEscolaridade");
        }

        [HttpGet]
        public ActionResult ReativarEscolaridade(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensEscolaridade"] = 2;
                    return RedirectToAction("MontarTelaEscolaridade");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            ESCOLARIDADE item = escApp.GetItemById(id);
            objetoESCAntes = (ESCOLARIDADE)Session["Escolaridade"];
            item.ESCO_IN_ATIVO = 1;
            Int32 volta = escApp.ValidateReativar(item, usuario);
            Session["ListaEscolaridade"] = null;
            return RedirectToAction("MontarTelaEscolaridade");
        }

        [HttpGet]
        public ActionResult MontarTelaParentesco()
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
                    Session["MensPermissao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];


            // Carrega listas
            if ((List<PARENTESCO>)Session["ListaParentesco"] == null)
            {
                listaMasterPAR = parApp.GetAllItens();
                Session["ListaParentesco"] = listaMasterPAR;
            }
            ViewBag.Listas = (List<PARENTESCO>)Session["ListaParentesco"];
            Session["Parentesco"] = null;

            // Indicadores
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensParentesco"] != null)
            {
                if ((Int32)Session["MensParentesco"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensParentesco"] == 3)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0143", CultureInfo.CurrentCulture));
                }
                if ((Int32)Session["MensParentesco"] == 4)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0144", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            Session["VoltaParentesco"] = 1;
            objetoPAR = new PARENTESCO();
            return View(objetoPAR);
        }

        public ActionResult RetirarFiltroParentesco()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaParentesco"] = null;
            return RedirectToAction("MontarTelaParentesco");
        }

        public ActionResult MostrarTudoParentesco()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterPAR = parApp.GetAllItensAdm();
            Session["ListaParentesco"] = listaMasterPAR;
            return RedirectToAction("MontarTelaParentesco");
        }

        public ActionResult VoltarBaseParentesco()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            return RedirectToAction("MontarTelaParentesco");
        }

        [HttpGet]
        public ActionResult IncluirParentesco()
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensParentesco"] = 2;
                    return RedirectToAction("MontarTelaParentesco");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara listas

            // Prepara view
            PARENTESCO item = new PARENTESCO();
            ParentescoViewModel vm = Mapper.Map<PARENTESCO, ParentescoViewModel>(item);
            vm.PARE_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirParentesco(ParentescoViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    PARENTESCO item = Mapper.Map<ParentescoViewModel, PARENTESCO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = parApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensParentesco"] = 3;
                        return RedirectToAction("MontarTelaParentesco");
                    }

                    // Sucesso
                    listaMasterPAR = new List<PARENTESCO>();
                    Session["ListaParentesco"] = null;
                    Session["IdParentesco"] = item.PARE_CD_ID;
                    return RedirectToAction("MontarTelaParentesco");
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
        public ActionResult EditarParentesco(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensParentesco"] = 2;
                    return RedirectToAction("MontarTelaParentesco");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            PARENTESCO item = parApp.GetItemById(id);
            Session["Parentesco"] = item;

            // Indicadores

            // Mensagens
            if (Session["MensParentesco"] != null)
            {


            }

            objetoPARAntes = item;
            Session["IdParentesco"] = id;
            ParentescoViewModel vm = Mapper.Map<PARENTESCO, ParentescoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarParentesco(ParentescoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    PARENTESCO item = Mapper.Map<ParentescoViewModel, PARENTESCO>(vm);
                    Int32 volta = parApp.ValidateEdit(item, objetoPARAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterPAR = new List<PARENTESCO>();
                    Session["ListaParentesco"] = null;
                    return RedirectToAction("MontarTelaParentesco");
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

        public ActionResult VoltarAnexoParentesco()
        {

            return RedirectToAction("EditarParentesco", new { id = (Int32)Session["IdParentesco"] });
        }

        [HttpGet]
        public ActionResult ExcluirParentesco(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensParentesco"] = 2;
                    return RedirectToAction("MontarTelaParentesco");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            PARENTESCO item = parApp.GetItemById(id);
            objetoPARAntes = (PARENTESCO)Session["Parentesco"];
            item.PARE_IN_ATIVO = 0;
            Int32 volta = parApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensParentesco"] = 4;
                return RedirectToAction("MontarTelaParentesco");
            }
            Session["ListaParentesco"] = null;
            return RedirectToAction("MontarTelaParentesco");
        }

        [HttpGet]
        public ActionResult ReativarParentesco(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM" & usuario.PERFIL.PERF_SG_SIGLA != "GER")
                {
                    Session["MensEscolaridade"] = 2;
                    return RedirectToAction("MontarTelaEscolaridade");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            PARENTESCO item = parApp.GetItemById(id);
            objetoPARAntes = (PARENTESCO)Session["Parentesco"];
            item.PARE_IN_ATIVO = 1;
            Int32 volta = parApp.ValidateReativar(item, usuario);
            Session["ListaParentesco"] = null;
            return RedirectToAction("MontarTelaParentesco");
        }


    }
}