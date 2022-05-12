using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Interfaces;
using EntitiesServices.Model;
using System.Globalization;
using EntitiesServices.WorkClasses;
using AutoMapper;
using SB_Dashboard_Presentation.ViewModels;
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
using EntitiesServices.DTO;


namespace SB_Dashboard_Presentation.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICRAppService crApp;
        private readonly IClienteAppService cpApp;
        private readonly ILPAppService lpApp;
        private readonly IPCAppService pcApp;
        private readonly IEPAppService epApp;
        private readonly IENAppService enApp;
        private readonly IOSEspAppService oseApp;
        private readonly IOSSitAppService ossApp;
        private readonly IOrdemServicoAppService OSApp;

        private String msg;
        private Exception exception;
        //USUARIO objeto = new USUARIO();
        //USUARIO objetoAntes = new USUARIO();
        List<vwContasAReceber> listaCR = new List<vwContasAReceber>();
        List<vwContasAPagar> listaCP = new List<vwContasAPagar>();
        List<vwLancamentosAPagar> listaLP = new List<vwLancamentosAPagar>();
        List<vwParcelamento> listaPC = new List<vwParcelamento>();
        List<vwExecutandoPositivo> listaEP = new List<vwExecutandoPositivo>();
        List<vwExecutandoNegativo> listaEN = new List<vwExecutandoNegativo>();
        List<vwOrdemServicoEspecialidade> listaOSE = new List<vwOrdemServicoEspecialidade>();
        List<vwOrdemServicoSituacao> listaOSS = new List<vwOrdemServicoSituacao>();
        List<OrdemServico> listaOS = new List<OrdemServico>();
        String extensao;

        public DashboardController(ICRAppService crApps, IClienteAppService cpApps, ILPAppService lpApps, IPCAppService pcApps, IEPAppService epApps, IENAppService enApps, IOSEspAppService oseApps, IOSSitAppService ossApps, IOrdemServicoAppService OSApps)
        {
            crApp = crApps;
            cpApp = cpApps;
            lpApp = lpApps;
            pcApp = pcApps;
            epApp = epApps;
            enApp = enApps;
            oseApp = oseApps;
            ossApp = ossApps;
            OSApp = OSApps;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Voltar()
        {
            //if ((String)Session["Ativa"] == null)
            //{
            //    return RedirectToAction("Login", "ControleAcesso");
            //}
            return RedirectToAction("MontarTelaFluxoCaixa", "Dashboard");
        }

        public ActionResult VoltarBase()
        {
            //if ((String)Session["Ativa"] == null)
            //{
            //    return RedirectToAction("Login", "ControleAcesso");
            //}
            return RedirectToAction("MontarTelaFluxoCaixa", "Dashboard");
        }

        [HttpGet]
        public ActionResult MontarTelaDashboard()
        {
            List<SelectListItem> filtro = new List<SelectListItem>();
            filtro.Add(new SelectListItem() { Text = "Filtro 1", Value = "1" });
            filtro.Add(new SelectListItem() { Text = "Filtro 2", Value = "0" });
            ViewBag.Filtro = new SelectList(filtro, "Value", "Text");

            ModeloViewModel mod = new ModeloViewModel();
            mod.FiltroData = DateTime.Today.Date;    
            return View(mod);
        }

        [HttpPost]
        public ActionResult FiltrarTudoDashboard(ModeloViewModel item)
        {
            
            
            
            
            
            
            
            
            
            
            
            
            
            return RedirectToAction("MontarTelaDashboard");
        }

        public ActionResult VerResultados()
        {
            // Prepara view
            List<ModeloViewModel> lista = new List<ModeloViewModel>();
            for (int i = 8; i < 13; i++)
            {
                ModeloViewModel mod = new ModeloViewModel();
                mod.MesAno = i.ToString() + "/" + "2020";
                mod.Valor = 5426 * i;
                lista.Add(mod);
            }
            ViewBag.Lista = lista;

            List<ComentarioViewModel> listaCom = new List<ComentarioViewModel>();
            for (int i = 1; i < 4; i++)
            {
                ComentarioViewModel mod = new ComentarioViewModel();
                mod.Data = DateTime.Now;
                mod.Texto = "Texto do Comentário...";
                mod.Foto = "~/Imagens/Foto_base.jpg";
                mod.Nome = "Roberto Telles";
                listaCom.Add(mod);
            }
            ViewBag.Lista = lista;
            ViewBag.ListaComent = listaCom;

            List<AnexosViewModel> listaAnexo = new List<AnexosViewModel>();
            AnexosViewModel ane = new AnexosViewModel();
            ane.Arquivo = "~/Imagens/Imagem1.jpg";
            ane.Data = DateTime.Today.Date;
            ane.Tipo = 1;
            ane.Titulo = "Imagem Qualquer";
            listaAnexo.Add(ane);
            ane.Arquivo = "~/Imagens/PDF1.pdf";
            ane.Data = DateTime.Today.Date;
            ane.Tipo = 2;
            ane.Titulo = "PDF Qualquer";
            listaAnexo.Add(ane);
            ViewBag.ListaAnexo = listaAnexo;

            ModeloViewModel modelo = new ModeloViewModel();
            modelo.Observacao = "Observações quaisquer";
            return View(modelo);
        }

        public ActionResult VerResultadosSemGrafico()
        {
            // Prepara view
            List<ModeloViewModel> lista = new List<ModeloViewModel>();
            for (int i = 8; i < 13; i++)
            {
                ModeloViewModel mod = new ModeloViewModel();
                mod.MesAno = i.ToString() + "/" + "2020";
                mod.Valor = 5426 * i;
                lista.Add(mod);
            }
            ViewBag.Lista = lista;

            List<ComentarioViewModel> listaCom = new List<ComentarioViewModel>();
            for (int i = 1; i < 4; i++)
            {
                ComentarioViewModel mod = new ComentarioViewModel();
                mod.Data = DateTime.Now;
                mod.Texto = "Texto do Comentário...";
                mod.Foto = "~/Imagens/Foto_base.jpg";
                mod.Nome = "Roberto Telles";
                listaCom.Add(mod);
            }
            ViewBag.Lista = lista;
            ViewBag.ListaComent = listaCom;

            List<AnexosViewModel> listaAnexo = new List<AnexosViewModel>();
            AnexosViewModel ane = new AnexosViewModel();
            ane.Arquivo = "~/Imagens/Imagem1.jpg";
            ane.Data = DateTime.Today.Date;
            ane.Tipo = 1;
            ane.Titulo = "Imagem Qualquer";
            listaAnexo.Add(ane);
            ane.Arquivo = "~/Imagens/PDF1.pdf";
            ane.Data = DateTime.Today.Date;
            ane.Tipo = 2;
            ane.Titulo = "PDF Qualquer";
            listaAnexo.Add(ane);
            ViewBag.ListaAnexo = listaAnexo;

            ModeloViewModel modelo = new ModeloViewModel();
            modelo.Observacao = "Observações quaisquer";
            return View(modelo);
        }


        [HttpPost]
        public ActionResult IncluirComentario()
        {
            return RedirectToAction("VerResultados");
        }

        [HttpGet]
        public ActionResult MontarTelaDashboardReal()
        {
            // Variaveis
            Decimal rec = 0;
            Decimal pag = 0;
            Decimal lp = 0;
            Decimal pc = 0;
            Decimal ep = 0;
            Decimal en = 0;
            DateTime inicio = Convert.ToDateTime("01/" + DateTime.Today.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Today.Date.Year.ToString());
            DateTime hoje = DateTime.Today.Date;
            hoje = Convert.ToDateTime("10/09/2019");
            Session["Hoje"] = hoje;

            // Carrega campos de filtro Aba Fluxo de Caixa
            ModeloViewModel mod = new ModeloViewModel();
            if (Session["Filtro"] == null)
            {
                mod.RecebimentoInicio = null;
                mod.RecebimentoFinal = null;
                mod.CentroLucro = null;

                Session["Filtro"] = null;
                Session["RecIni"] = null;
                Session["RecFim"] = null;
                Session["DataBase"] = Convert.ToDateTime("01/09/2019");
            }
            else
            {
                mod = (ModeloViewModel)Session["Filtro"];
                Session["RecIni"] = mod.RecebimentoInicio;
                Session["RecFim"] = mod.RecebimentoFinal;
                Session["DataBase"] = mod.EmissaoInicio;
            }

            // Carrega Indicadores Diretos -- A SUBSTITUR
            decimal saldoBancario = Convert.ToDecimal(164604.20);
            ViewBag.SaldoBancario = saldoBancario;

            // Carrega listas principais
            if ((Int32)Session["CarregaListas"] == 0)
            {
                List<OrdemServico> listaOSCheia = OSApp.GetAllItens();
                Session["ListaOSCheia"] = listaOSCheia;
                List<OrdemServico> listaOSIniciadas = OSApp.GetAllItensIniciadas();
                Session["ListaOSIniciadas"] = listaOSIniciadas;

                // Carrega listas dos filtros -- Aba Fluxo de Caixa
                List<vwContasAReceber> listaCR1 = crApp.GetAllItens();
                var listaCentroLucro = listaCR1.Select(p => p.Centro_de_Lucro).Distinct().ToList();
                List<SelectListItem> listaCL = new List<SelectListItem>();
                foreach (var item in listaCentroLucro)
                {
                    if (item != null)
                    {
                        listaCL.Add(new SelectListItem() { Text = item.ToString(), Value = item.ToString() });
                    }
                }
                SelectList cl = new SelectList(listaCL, "Value", "Text");
                ViewBag.CentroLucro = cl;
                Session["CL"] = cl;

                List<vwContasAPagar> listaCP1 = cpApp.GetAllItens();
                var listaCentroCusto = listaCP1.Select(p => p.Centro_de_Custos).Distinct().ToList();
                List<SelectListItem> listaCC = new List<SelectListItem>();
                foreach (var item in listaCentroCusto)
                {
                    if (item != null)
                    {
                        listaCC.Add(new SelectListItem() { Text = item.ToString(), Value = item.ToString() });
                    }
                }
                SelectList cc = new SelectList(listaCC, "Value", "Text");
                ViewBag.CentroCusto = cc;
                Session["CC"] = cc;
                Session["ListaCP"] = listaCP1;

                List<SelectListItem> libPag = new List<SelectListItem>();
                libPag.Add(new SelectListItem() { Text = "Sim", Value = "Sim" });
                libPag.Add(new SelectListItem() { Text = "Não", Value = "Não" });
                ViewBag.LiberadoPag = new SelectList(libPag, "Value", "Text");

                List<SelectListItem> execPos = new List<SelectListItem>();
                execPos.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execPos.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecPos = new SelectList(execPos, "Value", "Text");

                List<SelectListItem> execNeg = new List<SelectListItem>();
                execNeg.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execNeg.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecNeg= new SelectList(execNeg, "Value", "Text");

                List<SelectListItem> parc = new List<SelectListItem>();
                parc.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                parc.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.Parcelamento = new SelectList(parc, "Value", "Text");

                List<SelectListItem> lancPagar = new List<SelectListItem>();
                lancPagar.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                lancPagar.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.LancPagar = new SelectList(lancPagar, "Value", "Text");

                List<SelectListItem> crit = new List<SelectListItem>();
                crit.Add(new SelectListItem() { Text = "1", Value = "1" });
                crit.Add(new SelectListItem() { Text = "2", Value = "2" });
                crit.Add(new SelectListItem() { Text = "3", Value = "3" });
                crit.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Criticidade = new SelectList(crit, "Value", "Text");

                List<SelectListItem> prob = new List<SelectListItem>();
                prob.Add(new SelectListItem() { Text = "1", Value = "1" });
                prob.Add(new SelectListItem() { Text = "2", Value = "2" });
                prob.Add(new SelectListItem() { Text = "3", Value = "3" });
                prob.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Probabilidade = new SelectList(prob, "Value", "Text");

                Session["ExibeExecPos"] = 1;
                Session["ExibeExecNeg"] = 1;
                Session["ExibeParc"] = 1;
                Session["ExibeLancPag"] = 1;

                // Carrega listas - Aba Fluxo de Caixa
                hoje = Convert.ToDateTime("10/09/2019");
                //listaCR = listaCR1.Where(p => p.Data_de_Vencimento.Month == hoje.Month & p.Data_de_Vencimento.Year == hoje.Year).ToList();
                //listaCP = listaCP1.Where(p => p.Data_de_Vencimento.Month == hoje.Month & p.Data_de_Vencimento.Year == hoje.Year).ToList();
                //listaLP = lpApp.GetAllItens().Where(p => p.Data_de_Vencimento.Value.Month == 9 & p.Data_de_Vencimento.Value.Year == 2019).ToList();
                listaCR = listaCR1;
                listaCP = listaCP1;
                listaLP = lpApp.GetAllItens();
                listaPC = pcApp.GetAllItens();
                listaEP = epApp.GetAllItens();
                listaEN = enApp.GetAllItens();
                Session["ListaCR"] = listaCR;
                Session["ListaCP"] = listaCP;
                Session["ListaLP"] = listaLP;
                Session["ListaPC"] = listaPC;
                Session["ListaEP"] = listaEP;
                Session["ListaEN"] = listaEN;
                Session["CarregaListas"] = 1;

                rec = listaCR.Sum(p => p.Valor_Liquido);
                pag = listaCP.Sum(p => p.Valor);
                lp = listaLP.Sum(p => p.Valor);
                pc = listaPC.Sum(p => p.Valor_Bruto);
                ep = listaEP.Sum(p => p.ExecutandoPositivo.Value);
                en = listaEN.Sum(p => p.ExecutandoNegativo.Value);

                ViewBag.FalhaCR = listaCR.Count > 0 ? 0 : 1;
                ViewBag.FalhaCP = listaCP.Count > 0 ? 0 : 1;
                ViewBag.FalhaPC = listaPC.Count > 0 ? 0 : 1;
                ViewBag.FalhaLP = listaLP.Count > 0 ? 0 : 1;

            }
            else
            {
                // Aba Fluxo de Caixa
                listaCR = (List<vwContasAReceber>)Session["ListaCR"];
                listaCP = (List<vwContasAPagar>)Session["ListaCP"];
                listaPC = (List<vwParcelamento>)Session["ListaPC"];
                listaLP = (List<vwLancamentosAPagar>)Session["ListaLP"];
                listaEN = (List<vwExecutandoNegativo>)Session["ListaEN"];
                listaEP = (List<vwExecutandoPositivo>)Session["ListaEP"];
                Session["CarregaListas"] = 1;

                rec = listaCR.Sum(p => p.Valor_Liquido);
                pag = listaCP.Sum(p => p.Valor);
                lp = listaLP.Sum(p => p.Valor);
                pc = listaPC.Sum(p => p.Valor_Bruto);
                ep = listaEP.Sum(p => p.ExecutandoPositivo.Value);
                en = listaEN.Sum(p => p.ExecutandoNegativo.Value);

                ViewBag.FalhaCR = (Int32)Session["FalhaCR"];
                ViewBag.FalhaCP = (Int32)Session["FalhaCP"];
                ViewBag.FalhaPC = (Int32)Session["FalhaPC"];
                ViewBag.FalhaLP = (Int32)Session["FalhaLP"];

                ViewBag.Beneficiarios = (SelectList)Session["BE"];
                ViewBag.Sacado = (SelectList)Session["SA"];
                ViewBag.CentroCusto = (SelectList)Session["CC"];
                ViewBag.CentroLucro = (SelectList)Session["CL"];

                List<SelectListItem> libPag = new List<SelectListItem>();
                libPag.Add(new SelectListItem() { Text = "Sim", Value = "Sim" });
                libPag.Add(new SelectListItem() { Text = "Não", Value = "Não" });
                ViewBag.LiberadoPag = new SelectList(libPag, "Value", "Text");

                List<SelectListItem> execPos = new List<SelectListItem>();
                execPos.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execPos.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecPos = new SelectList(execPos, "Value", "Text");

                List<SelectListItem> execNeg = new List<SelectListItem>();
                execNeg.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execNeg.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecNeg = new SelectList(execNeg, "Value", "Text");

                List<SelectListItem> parc = new List<SelectListItem>();
                parc.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                parc.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.Parcelamento = new SelectList(parc, "Value", "Text");

                List<SelectListItem> lancPagar = new List<SelectListItem>();
                lancPagar.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                lancPagar.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.LancPagar = new SelectList(lancPagar, "Value", "Text");

                List<SelectListItem> crit = new List<SelectListItem>();
                crit.Add(new SelectListItem() { Text = "1", Value = "1" });
                crit.Add(new SelectListItem() { Text = "2", Value = "2" });
                crit.Add(new SelectListItem() { Text = "3", Value = "3" });
                crit.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Criticidade = new SelectList(crit, "Value", "Text");

                List<SelectListItem> prob = new List<SelectListItem>();
                prob.Add(new SelectListItem() { Text = "1", Value = "1" });
                prob.Add(new SelectListItem() { Text = "2", Value = "2" });
                prob.Add(new SelectListItem() { Text = "3", Value = "3" });
                prob.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Probabilidade = new SelectList(prob, "Value", "Text");

                mod = (ModeloViewModel)Session["Filtro"];
            }

            // Configura view - Fluxo de Caixa
            ViewBag.Rec = rec;
            ViewBag.Pag = pag;
            ViewBag.Lp = lp;
            ViewBag.Pc = pc;
            ViewBag.ExecPositivo = ep;
            ViewBag.ExecNegativo = en;

            Session["Rec"] = rec;
            Session["Pag"] = pag;
            Session["LP"] = lp;
            Session["PC"] = pc;

            // Campos calculados
            Decimal result = (rec + ep + pc) - (pag + en + lp);
            Decimal saldo = result + saldoBancario;
            ViewBag.Resultado = result;
            ViewBag.SaldoTotal = saldo;

            // Monta linha do filtro -- Aba Fluxo de caixa
            String linhaFiltro = String.Empty;
            if (mod.VencimentoInicio != null)
            {
                if (linhaFiltro == String.Empty)
                {
                    linhaFiltro = "Vencimento-Início: " + mod.VencimentoInicio.Value.ToShortDateString();
                }
                else
                {
                    linhaFiltro += " | Vencimento-Início: " + mod.VencimentoInicio.Value.ToShortDateString();
                }
            }
            if (mod.VencimentoFinal != null)
            {
                if (linhaFiltro == String.Empty)
                {
                    linhaFiltro = "Vencimento-Final: " + mod.VencimentoFinal.Value.ToShortDateString();
                }
                else
                {
                    linhaFiltro += " | Vencimento-Final: " + mod.VencimentoFinal.Value.ToShortDateString();
                }
            }
            if (mod.CentroLucro != null)
            {
                if (linhaFiltro == String.Empty)
                {
                    linhaFiltro = "Centro de Lucro: " + mod.CentroLucro;
                }
                else
                {
                    linhaFiltro += " | Centro de Lucro: " + mod.CentroLucro;
                }
            }
            if (mod.LiberadoPag != null)
            {
                if (linhaFiltro == String.Empty)
                {
                    linhaFiltro = "Liberado Pagto: " + mod.LiberadoPag;
                }
                else
                {
                    linhaFiltro += " | Liberado Pagto: " + mod.LiberadoPag;
                }
            }
            if (mod.Criticidade != null & mod.Criticidade > 0)
            {
                if (linhaFiltro == String.Empty)
                {
                    linhaFiltro = "Criticidade: " + mod.Criticidade.ToString();
                }
                else
                {
                    linhaFiltro += " | Criticidade: " + mod.Criticidade.ToString();
                }
            }
            if (mod.Probabilidade != null & mod.Probabilidade > 0)
            {
                if (linhaFiltro == String.Empty)
                {
                    linhaFiltro = "Probabilidade: " + mod.Probabilidade.ToString();
                }
                else
                {
                    linhaFiltro += " | Probabilidade: " + mod.Probabilidade.ToString();
                }
            }
            ViewBag.LinhaFiltro = linhaFiltro;

            // Retorna
            return View(mod);
        }

        public ActionResult MontarTelaOperacional()
        {
            DateTime inicio = Convert.ToDateTime("01/" + DateTime.Today.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Today.Date.Year.ToString());
            DateTime hoje = DateTime.Today.Date;
            hoje = Convert.ToDateTime("10/09/2019");
            Session["Hoje"] = hoje;
            if ( Session["ListaOSCheia"] == null)
            {
                List<OrdemServico> listaOSCheia = OSApp.GetAllItens();
                Session["ListaOSCheia"] = listaOSCheia;
            }
            if (Session["ListaOSIniciadas"] == null)
            {
                List<OrdemServico> listaOSIniciadas = OSApp.GetAllItensIniciadas();
                Session["ListaOSIniciadas"] = listaOSIniciadas;
            }
            // Retorna
            Session["Tela"] = 2;
            return View();
        }

        public ActionResult MontarTelaBI()
        {
            DateTime inicio = Convert.ToDateTime("01/" + DateTime.Today.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Today.Date.Year.ToString());
            DateTime hoje = DateTime.Today.Date;
            hoje = Convert.ToDateTime("10/09/2019");
            Session["Hoje"] = hoje;
            if (Session["ListaOSCheia"] == null)
            {
                List<OrdemServico> listaOSCheia = OSApp.GetAllItens();
                Session["ListaOSCheia"] = listaOSCheia;
            }
            if (Session["ListaOSIniciadas"] == null)
            {
                List<OrdemServico> listaOSIniciadas = OSApp.GetAllItensIniciadas();
                Session["ListaOSIniciadas"] = listaOSIniciadas;
            }
            Session["Tela"] = 3;
            return View();
        }

        public JsonResult GetDadosGraficoDespesas()
        {
            List<String> meses = new List<String>();
            List<Decimal> valor = new List<Decimal>();
            if (Session["GraficoDespesa"] == null)
            {
                DateTime hoje = Convert.ToDateTime("10/09/2019");
                List<vwContasAPagar> listaCP1 = (List<vwContasAPagar>)Session["ListaCP"];
                listaCP = listaCP1.Where(p => p.Data_de_Vencimento.Year == hoje.Year).ToList();
                Session["GraficoDespesa"] = listaCP1;
                List<vwContasAPagar> listaMes = new List<vwContasAPagar>();
                for (int i = 1; i < 13; i++)
                {
                    Int32 mes = i;
                    listaMes = listaCP.Where(p => p.Data_de_Vencimento.Month == mes).ToList();
                    var somaMes = listaMes.Sum(p => p.Valor) / 1000;
                    meses.Add(mes.ToString() + "/" + hoje.Year.ToString());
                    valor.Add(somaMes);
                }
                Session["Meses"] = meses;
                Session["Valor"] = valor;
            }
            else
            {
                meses = (List<String>)Session["Meses"];
                valor = (List<Decimal>)Session["valor"];
            }
            Hashtable result = new Hashtable();
            result.Add("meses", meses);
            result.Add("valores", valor);
            return Json(result);

        }

        public JsonResult GetDadosGraficoReceitas()
        {
            List<String> meses = new List<String>();
            List<Decimal> valor = new List<Decimal>();
            if (Session["GraficoReceita"] == null)
            {
                DateTime hoje = Convert.ToDateTime("10/09/2019");
                List<vwContasAReceber> listaCR1 = (List<vwContasAReceber>)Session["ListaCR"];
                Session["GraficoReceita"] = listaCR1;
                listaCR = listaCR1.Where(p => p.Data_de_Vencimento.Year == hoje.Year).ToList();
                List<vwContasAReceber> listaMes = new List<vwContasAReceber>();
                for (int i = 1; i < 13; i++)
                {
                    Int32 mes = i;
                    listaMes = listaCR.Where(p => p.Data_de_Vencimento.Month == mes).ToList();
                    var somaMes = listaMes.Sum(p => p.Valor_Liquido) / 1000;
                    meses.Add(mes.ToString() + "/" + hoje.Year.ToString());
                    valor.Add(somaMes);
                }
                Session["Meses"] = meses;
                Session["Valor"] = valor;
            }
            else
            {
                meses = (List<String>)Session["Meses"];
                valor = (List<Decimal>)Session["valor"];
            }
            Hashtable result = new Hashtable();
            result.Add("meses", meses);
            result.Add("valores", valor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoPagar()
        {
            List<String> meses = new List<String>();
            List<Decimal> valor = new List<Decimal>();
            if (Session["GraficoPagar"] == null)
            {
                DateTime hoje = Convert.ToDateTime("10/09/2019");
                List<vwLancamentosAPagar> listaLP1 = (List<vwLancamentosAPagar>)Session["ListaLP"];
                Session["GraficoPagar"] = listaLP1;
                listaLP = listaLP1.Where(p => p.Data_de_Vencimento.Value.Year == hoje.Year).ToList();
                List<vwLancamentosAPagar> listaMes = new List<vwLancamentosAPagar>();
                for (int i = 1; i < 13; i++)
                {
                    Int32 mes = i;
                    listaMes = listaLP.Where(p => p.Data_de_Vencimento.Value.Month == mes).ToList();
                    var somaMes = listaMes.Sum(p => p.Valor) / 1000;
                    meses.Add(mes.ToString() + "/" + hoje.Year.ToString());
                    valor.Add(somaMes);
                }
                Session["Meses"] = meses;
                Session["Valor"] = valor;
            }
            else
            {
                meses = (List<String>)Session["Meses"];
                valor = (List<Decimal>)Session["valor"];
            }

            Hashtable result = new Hashtable();
            result.Add("meses", meses);
            result.Add("valores", valor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoSituacao()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();
            if (Session["OSSituacao"] == null)
            {
                List<vwOrdemServicoSituacao> tbl = ossApp.GetAllItens();
                foreach (var item in tbl)
                {
                    desc.Add(item.Descricao);
                    quant.Add(item.Quantidade.Value);
                }
                Session["Desc"] = desc;
                Session["Quant"] = quant;
            }
            else
            {
                desc = (List<String>)Session["Desc"];
                quant = (List<Int32>)Session["Quant"];
            }

            cor.Add("#359E18");
            cor.Add("#FFAE00");
            cor.Add("#FF7F00");
            cor.Add("#D63131");
            cor.Add("#27A1C6");
            cor.Add("#501954 ");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoAtraso()
        {
            Int32 numAtraso = 0;
            if (Session["OSAtrasadas"] == null)
            {
                DateTime hoje = (DateTime)Session["Hoje"];
                List<OrdemServico> tbl = OSApp.GetOSAtraso(hoje);
                Session["OSAtrasadas"] = tbl;
                numAtraso = tbl.Count;
            }
            else
            {
                List<OrdemServico> tbl = (List<OrdemServico>)Session["OSAtrasadas"];
                Session["OSAtrasadas"] = tbl;
                numAtraso = tbl.Count;
            }

            List<OrdemServico> total = (List<OrdemServico>)Session["ListaOSCheia"];
            Int32 numTotal = total.Count;
            Int32 ativas = numTotal - numAtraso;
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();

            desc.Add("Em Atraso");
            quant.Add(numAtraso);
            cor.Add("#359E18");
            desc.Add("Ativas");
            quant.Add(ativas);
            cor.Add("#FFAE00");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoAvaliacao()
        {
            Int32 baixo = 0;
            Int32 alto = 0;
            if (Session["OSAvaliacao"] == null)
            {
                List<OrdemServico> tbl = OSApp.GetOSAvaliacao();
                Session["OSAvaliacao"] = tbl;
                List<OrdemServico> total = (List<OrdemServico>)Session["ListaOSCheia"];
                foreach (OrdemServico item in tbl)
                {
                    baixo += item.OrdemServicoAvaliacao.Where(p => p.Avaliacao < 5).ToList().Count;
                    alto += item.OrdemServicoAvaliacao.Where(p => p.Avaliacao > 5).ToList().Count;
                }
                Session["Alto"] = alto;
                Session["Baixo"] = baixo;
            }
            else
            {
                List<OrdemServico> tbl = (List<OrdemServico>)Session["OSAvaliacao"];
                alto = (Int32)Session["Alto"];
                baixo = (Int32)Session["Baixo"];
            }

            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();

            desc.Add("Avaliação < 5");
            quant.Add(baixo);
            cor.Add("#359E18");
            desc.Add("Avaliação >= 5");
            quant.Add(alto);
            cor.Add("#FFAE00");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoPendencia()
        {
            Int32 numPendencia = 0;
            if (Session["OSPendencia"] == null)
            {
                List<OrdemServico> tbl = OSApp.GetOSPendencias();
                Session["OSPendencia"] = tbl;
                numPendencia = tbl.Count;
            }
            else
            {
                List<OrdemServico> tbl = (List<OrdemServico>)Session["OSPendencia"];
                numPendencia = tbl.Count;
            }
            List<OrdemServico> total = (List<OrdemServico>)Session["ListaOSIniciadas"];
            
            Int32 numTotal = total.Count;
            Int32 ativas = numTotal - numPendencia;
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();

            desc.Add("Com Pendência");
            quant.Add(numPendencia);
            cor.Add("#FF7F00");
            desc.Add("Sem Pendência");
            quant.Add(ativas);
            cor.Add("#D63131");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public ActionResult AtualizaDados()
        {
            Session["Filtro"] = null;
            Session["CarregaListas"] = 0;
            Session["GraficoDespesa"] = null;
            Session["OSAtrasadas"] = null;
            Session["OSAvaliacao"] = null;
            Session["OSCidade"] = null;
            Session["OSEspecialidade"] = null;
            Session["OSPendencia"] = null;
            Session["OSPesquisa"] = null;
            Session["OSSituacao"] = null;
            Session["OSTipo"] = null;
            Session["OSUF"] = null;
            Session["GraficoPagar"] = null;
            Session["GraficoReceita"] = null;
            return RedirectToAction("MontarTelaFluxoCaixa", "Dashboard");
        }

        public ActionResult ExibirTudo()
        {
            if ((Int32)Session["Tela"] == 1)
            {
                return RedirectToAction("MontarTelaFluxoCaixa", "Dashboard");
            }
            if ((Int32)Session["Tela"] == 2)
            {
                return RedirectToAction("MontarTelaOperacional", "Dashboard");
            }
            if ((Int32)Session["Tela"] == 3)
            {
                return RedirectToAction("MontarTelaBI", "Dashboard");
            }
            return RedirectToAction("MontarTelaFluxoCaixa", "Dashboard");
        }

        public JsonResult GetDadosGraficoOrdemServicoPesquisa()
        {
            Int32 numPendencia = 0;
            if (Session["OSPesquisa"] == null)
            {
                List<OrdemServico> tbl = OSApp.GetOSPesquisa();
                Session["OSPesquisa"] = tbl;
                numPendencia = tbl.Count;
            }
            else
            {
                List<OrdemServico> tbl = (List<OrdemServico>)Session["OSPesquisa"];
                numPendencia = tbl.Count;
            }

            List<OrdemServico> total = (List<OrdemServico>)Session["ListaOSIniciadas"];
            Int32 numTotal = total.Count;
            Int32 ativas = numTotal - numPendencia;
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<String> cor = new List<String>();

            desc.Add("Com Pesquisa");
            quant.Add(numPendencia);
            cor.Add("#27A1C6");
            desc.Add("Sem Pesquisa");
            quant.Add(ativas);
            cor.Add("#501954 ");

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("cores", cor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoEspecialidade()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<Decimal> valor = new List<Decimal>();
            if (Session["OSEspecialidade"] == null)
            {
                List<vwOrdemServicoEspecialidade> tbl = oseApp.GetAllItens();
                Session["OSEspecialidade"] = tbl;
                foreach (var item in tbl)
                {
                    desc.Add(item.Descricao);
                    quant.Add(item.Quantidade.Value);
                    if (item.Total != null)
                    {
                        valor.Add(item.Total.Value);
                    }
                    else
                    {
                        valor.Add(0);
                    }
                }
                Session["Desc"] = desc;
                Session["Quant"] = quant;
                Session["Valor"] = valor;
            }
            else
            {
                desc = (List<String>)Session["Desc"];
                quant = (List<Int32>)Session["Quant"];
                valor = (List<Decimal>)Session["Valor"];
            }

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            result.Add("total", valor);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoUF()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<Decimal> valor = new List<Decimal>();
            if (Session["OSUF"] == null)
            {
                List<DTO_OS_UF> tbl = oseApp.GetItensOSUF();
                Session["OSUF"] = tbl;
                foreach (var item in tbl)
                {
                    if (!String.IsNullOrEmpty(item.UF))
                    {
                        desc.Add(item.UF);
                        quant.Add(item.Quantidade);
                    }
                }
                Session["Desc"] = desc;
                Session["Quant"] = quant;
            }
            else
            {
                desc = (List<String>)Session["Desc"];
                quant = (List<Int32>)Session["Quant"];
            }

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoTipo()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            if (Session["OSTipo"] == null)
            {
                List<DTO_OS_UF> tbl = oseApp.GetItensOSTipo();
                Session["OSTipo"] = tbl;
                foreach (var item in tbl)
                {
                    if (!String.IsNullOrEmpty(item.UF))
                    {
                        desc.Add(item.UF);
                        quant.Add(item.Quantidade);
                    }
                }
                Session["Desc"] = desc;
                Session["Quant"] = quant;
            }
            else
            {
                desc = (List<String>)Session["Desc"];
                quant = (List<Int32>)Session["Quant"];
            }

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            return Json(result);
        }

        public JsonResult GetDadosGraficoOrdemServicoCidade()
        {
            List<String> desc = new List<String>();
            List<Int32> quant = new List<Int32>();
            List<Decimal> valor = new List<Decimal>();
            if (Session["OSCidade"] == null)
            {
                List<DTO_OS_UF> tbl = oseApp.GetItensOSCidade();
                Session["OSCidade"] = tbl;
                foreach (var item in tbl)
                {
                    if (!String.IsNullOrEmpty(item.UF))
                    {
                        desc.Add(item.UF);
                        quant.Add(item.Quantidade);
                    }
                }
                Session["Desc"] = desc;
                Session["Quant"] = quant;
            }
            else
            {
                desc = (List<String>)Session["Desc"];
                quant = (List<Int32>)Session["Quant"];
            }

            Hashtable result = new Hashtable();
            result.Add("labels", desc);
            result.Add("valores", quant);
            return Json(result);
        }

        public ActionResult VerReceitasExpansao()
        {
            // Prepara grafico
            String ano = ((DateTime)Session["DataBase"]).Year.ToString();
            ViewBag.Ano = ano;            
            
            // Prepara view
            List<vwContasAReceber> listaCR = (List<vwContasAReceber>)Session["ListaCR"];
            ViewBag.ListaCR = listaCR;
            ViewBag.ListaCRNum = listaCR.Count;
            ViewBag.CRValor = (Decimal)Session["Rec"];
            return View();
        }

        public ActionResult VerOSSituacaoExpansao()
        {
            // Prepara view
            List<vwOrdemServicoSituacao> listaOSS = ossApp.GetAllItens();
            ViewBag.ListaOSS = listaOSS;
            ViewBag.ListaOSSNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSUFExpansao()
        {
            // Prepara view
            List<DTO_OS_UF> listaOSS = (List<DTO_OS_UF>)Session["OSUF"];
            ViewBag.ListaOSS = listaOSS;
            ViewBag.ListaOSSNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSAtrasoExpansao()
        {
            // Prepara view
            List<OrdemServico> listaOSS = (List<OrdemServico>)Session["OSAtrasadas"];
            ViewBag.ListaOSAtraso = listaOSS;
            ViewBag.ListaOSAtrasoNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSPendenciaExpansao()
        {
            // Prepara view
            List<OrdemServico> listaOSS = (List<OrdemServico>)Session["OSPendencia"];
            ViewBag.ListaOSAtraso = listaOSS;
            ViewBag.ListaOSAtrasoNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSPesquisaExpansao()
        {
            // Prepara view
            List<OrdemServico> listaOSS = (List<OrdemServico>)Session["OSPesquisa"];
            ViewBag.ListaOSAtraso = listaOSS;
            ViewBag.ListaOSAtrasoNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSAvaliacaoExpansao()
        {
            // Prepara view
            List<OrdemServico> listaOSS = (List<OrdemServico>)Session["OSAvaliacao"];
            ViewBag.ListaOSAtraso = listaOSS;
            ViewBag.ListaOSAtrasoNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSTipoExpansao()
        {
            // Prepara view
            List<DTO_OS_UF> listaOSS = (List<DTO_OS_UF>)Session["OSTipo"];
            ViewBag.ListaOSS = listaOSS;
            ViewBag.ListaOSSNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSCidadeExpansao()
        {
            // Prepara view
            return View();
        }

        public ActionResult VerOSCidadeTable()
        {
            // Prepara view
            List<DTO_OS_UF> listaOSS = (List<DTO_OS_UF>)Session["OSCidade"];
            ViewBag.ListaOSS = listaOSS;
            ViewBag.ListaOSSNum = listaOSS.Count;
            return View();
        }

        public ActionResult VerOSSEspecialidadeExpansao()
        {
            // Prepara view
            List<vwOrdemServicoEspecialidade> listaOSE = oseApp.GetAllItens();
            ViewBag.ListaOSE = listaOSE;
            ViewBag.ListaOSENum = listaOSE.Count;
            return View();
        }

        public ActionResult VerDespesasExpansao()
        {
            // Prepara grafico
            Session["DataBase"] = Convert.ToDateTime("01/03/2017");
            String ano = ((DateTime)Session["DataBase"]).Year.ToString();
            ViewBag.Ano = ano;

            // Prepara view
            List<vwContasAPagar> listaCP = (List<vwContasAPagar>)Session["ListaCP"];
            ViewBag.ListaCP = listaCP;
            ViewBag.ListaCPNum = listaCP.Count;
            ViewBag.CPValor = (Decimal)Session["Pag"];
            return View();
        }

        public ActionResult VerParcelamentosExpansao()
        {
            // Prepara grafico
            String ano = ((DateTime)Session["DataBase"]).Year.ToString();
            ViewBag.Ano = ano;

            // Prepara view
            List<vwParcelamento> listaPC = (List<vwParcelamento>)Session["ListaPC"];
            ViewBag.listaPC = listaPC;
            ViewBag.listaPCNum = listaPC.Count;
            ViewBag.PCValor = (Decimal)Session["PC"];
            return View();
        }

        public ActionResult VerLancamentosExpansao()
        {
            // Prepara grafico
            String ano = ((DateTime)Session["DataBase"]).Year.ToString();
            ViewBag.Ano = ano;

            // Prepara view
            List<vwLancamentosAPagar> listaLP = (List<vwLancamentosAPagar>)Session["ListaLP"];
            ViewBag.listaLP = listaLP;
            ViewBag.listaLPNum = listaLP.Count;
            ViewBag.LPValor = (Decimal)Session["LP"];
            return View();
        }

        [HttpPost]
        public JsonResult GetValorGraficoCR(Int32 id, Int32? meses)
        {
            if (meses == null)
            {
                meses = 3;
            }
            List<vwContasAReceber> lista = (List<vwContasAReceber>)Session["ListaCR"];
            DateTime data1 = (DateTime)Session["RecIni"];
            DateTime data2 = (DateTime)Session["RecFim"];
            Int32 mes = data1.Month;
            Int32 ano = data1.Year;
            Int32 numMes = ((data1.Year - data2.Year) * 12) + data1.Month - data2.Month;
            var hash = new Hashtable();
            for (int i = 1; i <= numMes; i++)
            {
                String label = mes.ToString().PadLeft(2, '0') + "/" + ano.ToString();
                Decimal valor = lista.Where(p => p.Data_de_Vencimento.Month == mes & p.Data_de_Vencimento.Year == ano).Sum(x => x.Valor_Liquido);
                hash.Add(label, valor);

                mes++;
                if (mes > 12)
                {
                    mes = 1;
                    ano++;
                }
            }
            return Json(hash);
        }

        public ActionResult VerTodosReceita()
        {
            // Prepara view
            List<vwContasAReceber> listaCR = (List<vwContasAReceber>)Session["ListaCR"];
            ViewBag.ListaCR = listaCR;
            ViewBag.ListaCRNum = listaCR.Count;
            ViewBag.CRValor = (Decimal)Session["Rec"];
            return View();
        }

        public ActionResult VerTodosDespesas()
        {
            // Prepara view
            List<vwContasAPagar> listaCP = (List<vwContasAPagar>)Session["listaCP"];
            ViewBag.listaCP = listaCP;
            ViewBag.listaCPNUm = listaCP.Count;
            ViewBag.CPValor = (Decimal)Session["Pag"];
            return View();
        }

        public ActionResult VerTodosParcelamentos()
        {
            // Prepara view
            List<vwParcelamento> listaPC = (List<vwParcelamento>)Session["listaPC"];
            ViewBag.listaPC = listaPC;
            ViewBag.listaPCNum = listaPC.Count;
            ViewBag.PCValor = (Decimal)Session["PC"];
            return View();
        }

        public ActionResult VerTodosLancamentos()
        {
            // Prepara view
            List<vwLancamentosAPagar> listaLP = (List<vwLancamentosAPagar>)Session["listaLP"];
            ViewBag.listaLP = listaLP;
            ViewBag.listaLPNum = listaLP.Count;
            ViewBag.LPValor = (Decimal)Session["LP"];
            return View();
        }

        public ActionResult RetirarFiltro()
        {
            Session["CarregaLista"] = 1;
            Session["ListaCR"] = Session["ListaCROrig"];
            Session["ListaCP"] = Session["ListaCPOrig"];
            Session["ListaLP"] = Session["ListaLPOrig"];
            Session["ListaPC"] = Session["ListaPCOrig"];
            Session["ListaEP"] = Session["ListaEPOrig"];
            Session["ListaEN"] = Session["ListaENOrig"];
            Session["Filtro"] = null;
            return RedirectToAction("MontarTelaFluxoCaixa");
        }

        [HttpPost]
        public ActionResult FiltrarGeralDashboard(ModeloViewModel item)
        {
            try
            {
                // Executa a operação
                Session["Filtro"] = item;
                Int32 volta = 0;

                // Receitas
                if (item.RecebimentoInicio != null || item.RecebimentoFinal != null || item.CentroLucro != null || (item.Probabilidade != null & item.Probabilidade > 0) )
                {
                    List<vwContasAReceber> listaCR = new List<vwContasAReceber>();
                    volta = crApp.ExecuteFilter(item.EmissaoInicio, item.EmissaoFinal, item.VencimentoInicio, item.VencimentoFinal, item.RecebimentoInicio, item.RecebimentoFinal, item.CentroLucro, item.Sacado, item.Probabilidade, out listaCR);
                    if (volta == 1)
                    {
                        Session["FalhaCR"] = 1;
                    }
                    Session["ListaCR"] = listaCR;
                }

                if (item.LiberadoPag != null || (item.Criticidade != null & item.Criticidade > 0))
                {
                    List<vwContasAPagar> listaCP = new List<vwContasAPagar>();
                    volta = cpApp.ExecuteFilter(item.EmissaoInicio, item.EmissaoFinal, item.VencimentoInicio, item.VencimentoFinal, item.PagamentoInicio, item.PagamentoFinal, item.CentroCusto, item.Beneficiario, item.LiberadoPag, item.Criticidade, out listaCP);
                    if (volta == 1)
                    {
                        Session["FalhaCP"] = 1;
                    }
                    Session["ListaCP"] = listaCP;
                }

                // Parcelamentos
                if (item.CentroLucro != null || (item.Probabilidade != null & item.Probabilidade > 0))
                {
                    List<vwParcelamento> listaPC = new List<vwParcelamento>();
                    volta = pcApp.ExecuteFilter(item.VencimentoInicio, item.VencimentoFinal, item.CentroLucro, item.Sacado, item.Probabilidade, out listaPC);
                    if (volta == 1)
                    {
                        Session["FalhaPC"] = 1;
                    }
                    Session["ListaPC"] = listaPC;
                }

                // Lancamentos
                if (item.CentroLucro != null)
                {
                    List<vwLancamentosAPagar> listaLP = new List<vwLancamentosAPagar>();
                    volta = lpApp.ExecuteFilter(item.EmissaoInicio, item.EmissaoFinal, item.VencimentoInicio, item.VencimentoFinal, item.CentroCusto, item.CentroLucro, item.Beneficiario, out listaLP);
                    if (volta == 1)
                    {
                        Session["FalhaLP"] = 1;
                    }
                    Session["ListaLP"] = listaLP;
                }

                // Sucesso
                Session["CarregaLista"] = 1;
                if (item.ExecPositivo != null)
                {
                    Session["ExibeExecPos"] = item.ExecPositivo;
                }
                if (item.ExecNegativo != null)
                {
                    Session["ExibeExecNeg"] = item.ExecNegativo;
                }
                if (item.Parcelamento != null)
                {
                    Session["ExibeParc"] = item.Parcelamento;
                }
                if (item.LancPagar != null)
                {
                    Session["ExibeLancPag"] = item.LancPagar;
                }

                return RedirectToAction("MontarTelaDashboardReal");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaDashboardReal");
            }
        }

        [HttpGet]
        public ActionResult MontarTelaFluxoCaixa()
        {
            // Variaveis
            Decimal rec = 0;
            Decimal pag = 0;
            Decimal lp = 0;
            Decimal pc = 0;
            Decimal ep = 0;
            Decimal en = 0;
            DateTime inicio = Convert.ToDateTime("01/" + DateTime.Today.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Today.Date.Year.ToString());
            DateTime hoje = DateTime.Today.Date;
            hoje = Convert.ToDateTime("10/09/2019");
            Session["Hoje"] = hoje;

            // Carrega campos de filtro Aba Fluxo de Caixa
            ModeloViewModel mod = new ModeloViewModel();
            if (Session["Filtro"] == null)
            {
                mod.RecebimentoInicio = null;
                mod.RecebimentoFinal = null;
                mod.CentroLucro = null;

                Session["Filtro"] = mod;
                Session["RecIni"] = null;
                Session["RecFim"] = null;
                Session["DataBase"] = Convert.ToDateTime("01/09/2019");
            }
            else
            {
                mod = (ModeloViewModel)Session["Filtro"];
                Session["RecIni"] = mod.RecebimentoInicio;
                Session["RecFim"] = mod.RecebimentoFinal;
                Session["DataBase"] = Convert.ToDateTime("01/09/2019");
            }

            // Carrega Indicadores Diretos -- A SUBSTITUR
            decimal saldoBancario = Convert.ToDecimal(164604.20);
            ViewBag.SaldoBancario = saldoBancario;

            // Carrega listas principais
            if ((Int32)Session["CarregaListas"] == 0)
            {
                // Carrega listas dos filtros -- Aba Fluxo de Caixa
                List<vwContasAReceber> listaCR1 = crApp.GetAllItens();
                var listaCentroLucro = listaCR1.Select(p => p.Centro_de_Lucro).Distinct().ToList();
                List<SelectListItem> listaCL = new List<SelectListItem>();
                foreach (var item in listaCentroLucro)
                {
                    if (item != null)
                    {
                        listaCL.Add(new SelectListItem() { Text = item.ToString(), Value = item.ToString() });
                    }
                }
                SelectList cl = new SelectList(listaCL, "Value", "Text");
                ViewBag.CentroLucro = cl;
                Session["CL"] = cl;

                List<vwContasAPagar> listaCP1 = cpApp.GetAllItens();
                var listaCentroCusto = listaCP1.Select(p => p.Centro_de_Custos).Distinct().ToList();
                List<SelectListItem> listaCC = new List<SelectListItem>();
                foreach (var item in listaCentroCusto)
                {
                    if (item != null)
                    {
                        listaCC.Add(new SelectListItem() { Text = item.ToString(), Value = item.ToString() });
                    }
                }
                SelectList cc = new SelectList(listaCC, "Value", "Text");
                ViewBag.CentroCusto = cc;
                Session["CC"] = cc;
                Session["ListaCP"] = listaCP1;

                List<SelectListItem> libPag = new List<SelectListItem>();
                libPag.Add(new SelectListItem() { Text = "Sim", Value = "Sim" });
                libPag.Add(new SelectListItem() { Text = "Não", Value = "Não" });
                ViewBag.LiberadoPag = new SelectList(libPag, "Value", "Text");

                List<SelectListItem> execPos = new List<SelectListItem>();
                execPos.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execPos.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecPos = new SelectList(execPos, "Value", "Text");

                List<SelectListItem> execNeg = new List<SelectListItem>();
                execNeg.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execNeg.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecNeg= new SelectList(execNeg, "Value", "Text");

                List<SelectListItem> parc = new List<SelectListItem>();
                parc.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                parc.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.Parcelamento = new SelectList(parc, "Value", "Text");

                List<SelectListItem> lancPagar = new List<SelectListItem>();
                lancPagar.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                lancPagar.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.LancPagar = new SelectList(lancPagar, "Value", "Text");

                List<SelectListItem> crit = new List<SelectListItem>();
                crit.Add(new SelectListItem() { Text = "1", Value = "1" });
                crit.Add(new SelectListItem() { Text = "2", Value = "2" });
                crit.Add(new SelectListItem() { Text = "3", Value = "3" });
                crit.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Criticidade = new SelectList(crit, "Value", "Text");

                List<SelectListItem> prob = new List<SelectListItem>();
                prob.Add(new SelectListItem() { Text = "1", Value = "1" });
                prob.Add(new SelectListItem() { Text = "2", Value = "2" });
                prob.Add(new SelectListItem() { Text = "3", Value = "3" });
                prob.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Probabilidade = new SelectList(prob, "Value", "Text");

                Session["ExibeExecPos"] = 1;
                Session["ExibeExecNeg"] = 1;
                Session["ExibeParc"] = 1;
                Session["ExibeLancPag"] = 1;

                // Carrega listas - Aba Fluxo de Caixa
                hoje = Convert.ToDateTime("10/09/2019");
                //listaCR = listaCR1.Where(p => p.Data_de_Vencimento.Month == hoje.Month & p.Data_de_Vencimento.Year == hoje.Year).ToList();
                //listaCP = listaCP1.Where(p => p.Data_de_Vencimento.Month == hoje.Month & p.Data_de_Vencimento.Year == hoje.Year).ToList();
                //listaLP = lpApp.GetAllItens().Where(p => p.Data_de_Vencimento.Value.Month == 9 & p.Data_de_Vencimento.Value.Year == 2019).ToList();
                listaCR = listaCR1;
                listaCP = listaCP1;
                listaLP = lpApp.GetAllItens();
                listaPC = pcApp.GetAllItens();
                listaEP = epApp.GetAllItens();
                listaEN = enApp.GetAllItens();
                Session["ListaCR"] = listaCR;
                Session["ListaCP"] = listaCP;
                Session["ListaLP"] = listaLP;
                Session["ListaPC"] = listaPC;
                Session["ListaEP"] = listaEP;
                Session["ListaEN"] = listaEN;
                Session["ListaCROrig"] = listaCR;
                Session["ListaCPOrig"] = listaCP;
                Session["ListaLPOrig"] = listaLP;
                Session["ListaPCOrig"] = listaPC;
                Session["ListaEPOrig"] = listaEP;
                Session["ListaENOrig"] = listaEN;
                Session["CarregaListas"] = 1;

                rec = listaCR.Sum(p => p.Valor_Liquido);
                pag = listaCP.Sum(p => p.Valor);
                lp = listaLP.Sum(p => p.Valor);
                pc = listaPC.Sum(p => p.Valor_Bruto);
                ep = listaEP.Sum(p => p.ExecutandoPositivo.Value);
                en = listaEN.Sum(p => p.ExecutandoNegativo.Value);

                ViewBag.FalhaCR = listaCR.Count > 0 ? 0 : 1;
                ViewBag.FalhaCP = listaCP.Count > 0 ? 0 : 1;
                ViewBag.FalhaPC = listaPC.Count > 0 ? 0 : 1;
                ViewBag.FalhaLP = listaLP.Count > 0 ? 0 : 1;
                Session["CarregaListas"] = 1;
            }
            else
            {
                // Aba Fluxo de Caixa
                listaCR = (List<vwContasAReceber>)Session["ListaCR"];
                listaCP = (List<vwContasAPagar>)Session["ListaCP"];
                listaPC = (List<vwParcelamento>)Session["ListaPC"];
                listaLP = (List<vwLancamentosAPagar>)Session["ListaLP"];
                listaEN = (List<vwExecutandoNegativo>)Session["ListaEN"];
                listaEP = (List<vwExecutandoPositivo>)Session["ListaEP"];
                Session["CarregaListas"] = 1;

                rec = listaCR.Sum(p => p.Valor_Liquido);
                pag = listaCP.Sum(p => p.Valor);
                lp = listaLP.Sum(p => p.Valor);
                pc = listaPC.Sum(p => p.Valor_Bruto);
                ep = listaEP.Sum(p => p.ExecutandoPositivo.Value);
                en = listaEN.Sum(p => p.ExecutandoNegativo.Value);

                ViewBag.FalhaCR = (Int32)Session["FalhaCR"];
                ViewBag.FalhaCP = (Int32)Session["FalhaCP"];
                ViewBag.FalhaPC = (Int32)Session["FalhaPC"];
                ViewBag.FalhaLP = (Int32)Session["FalhaLP"];

                ViewBag.CentroCusto = (SelectList)Session["CC"];
                ViewBag.CentroLucro = (SelectList)Session["CL"];

                List<SelectListItem> libPag = new List<SelectListItem>();
                libPag.Add(new SelectListItem() { Text = "Sim", Value = "Sim" });
                libPag.Add(new SelectListItem() { Text = "Não", Value = "Não" });
                ViewBag.LiberadoPag = new SelectList(libPag, "Value", "Text");

                List<SelectListItem> execPos = new List<SelectListItem>();
                execPos.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execPos.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecPos = new SelectList(execPos, "Value", "Text");

                List<SelectListItem> execNeg = new List<SelectListItem>();
                execNeg.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                execNeg.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.ExecNeg = new SelectList(execNeg, "Value", "Text");

                List<SelectListItem> parc = new List<SelectListItem>();
                parc.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                parc.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.Parcelamento = new SelectList(parc, "Value", "Text");

                List<SelectListItem> lancPagar = new List<SelectListItem>();
                lancPagar.Add(new SelectListItem() { Text = "Sim", Value = "1" });
                lancPagar.Add(new SelectListItem() { Text = "Não", Value = "0" });
                ViewBag.LancPagar = new SelectList(lancPagar, "Value", "Text");

                List<SelectListItem> crit = new List<SelectListItem>();
                crit.Add(new SelectListItem() { Text = "1", Value = "1" });
                crit.Add(new SelectListItem() { Text = "2", Value = "2" });
                crit.Add(new SelectListItem() { Text = "3", Value = "3" });
                crit.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Criticidade = new SelectList(crit, "Value", "Text");

                List<SelectListItem> prob = new List<SelectListItem>();
                prob.Add(new SelectListItem() { Text = "1", Value = "1" });
                prob.Add(new SelectListItem() { Text = "2", Value = "2" });
                prob.Add(new SelectListItem() { Text = "3", Value = "3" });
                prob.Add(new SelectListItem() { Text = "4", Value = "4" });
                ViewBag.Probabilidade = new SelectList(prob, "Value", "Text");
            }

            // Configura view - Fluxo de Caixa
            ViewBag.Rec = rec;
            ViewBag.Pag = pag;
            ViewBag.Lp = lp;
            ViewBag.Pc = pc;
            ViewBag.ExecPositivo = ep;
            ViewBag.ExecNegativo = en;

            Session["Rec"] = rec;
            Session["Pag"] = pag;
            Session["LP"] = lp;
            Session["PC"] = pc;

            // Campos calculados
            Decimal result = (rec + ep + pc) - (pag + en + lp);
            Decimal saldo = result + saldoBancario;
            ViewBag.Resultado = result;
            ViewBag.SaldoTotal = saldo;

            // Monta linha do filtro -- Aba Fluxo de caixa
            String linhaFiltro = String.Empty;
            ViewBag.LinhaFiltro = linhaFiltro;

            // Seta defaults
            mod.ExecPositivo = 1;
            mod.ExecNegativo = 1;
            mod.Parcelamento = 1;
            mod.LancPagar = 1;

            // Retorna
            Session["Tela"] = 1;
            return View(mod);
        }

        [HttpPost]
        public ActionResult FiltrarGeralDashboardNovo(ModeloViewModel item)
        {
            // Recupera listas
            Session["CarregaListas"] = 1;
            Int32 volta = 0;
            listaCR = (List<vwContasAReceber>)Session["ListaCROrig"];
            listaCP = (List<vwContasAPagar>)Session["ListaCPOrig"];
            listaPC = (List<vwParcelamento>)Session["ListaPCOrig"];
            listaLP = (List<vwLancamentosAPagar>)Session["ListaLPOrig"];
            listaEN = (List<vwExecutandoNegativo>)Session["ListaENOrig"];
            listaEP = (List<vwExecutandoPositivo>)Session["ListaEPOrig"];

            Decimal rec = listaCR.Sum(p => p.Valor_Liquido);
            Decimal pag = listaCP.Sum(p => p.Valor);
            Decimal lp = listaLP.Sum(p => p.Valor);
            Decimal pc = listaPC.Sum(p => p.Valor_Bruto);
            Decimal ep = listaEP.Sum(p => p.ExecutandoPositivo.Value);
            Decimal en = listaEN.Sum(p => p.ExecutandoNegativo.Value);

            // Trata Receitas
            if (item.RecebimentoInicio != null || item.RecebimentoFinal != null || item.CentroLucro != null || (item.Probabilidade != null & item.Probabilidade > 0))
            {
                List<vwContasAReceber> listaCR = new List<vwContasAReceber>();
                volta = crApp.ExecuteFilter(null, null, null, null, item.RecebimentoInicio, item.RecebimentoFinal, item.CentroLucro, null, item.Probabilidade, out listaCR);
                if (volta == 1)
                {
                    Session["FalhaCR"] = 1;
                }
                Session["ListaCR"] = listaCR;
            }

            // Trata Despesas
            if (item.RecebimentoInicio != null || item.RecebimentoFinal != null || item.LiberadoPag != null || (item.Criticidade != null & item.Criticidade > 0))
            {
                List<vwContasAPagar> listaCP = new List<vwContasAPagar>();
                volta = cpApp.ExecuteFilter(null, null, item.RecebimentoInicio, item.RecebimentoFinal, null, null, null, null, item.LiberadoPag, item.Criticidade, out listaCP);
                if (volta == 1)
                {
                    Session["FalhaCP"] = 1;
                }
                Session["ListaCP"] = listaCP;
            }

            // Trata Parcelamento
            if (item.Parcelamento == 1 || item.Parcelamento == null)
            {
                if (item.CentroLucro != null || (item.Probabilidade != null & item.Probabilidade > 0))
                {
                    List<vwParcelamento> listaPC = new List<vwParcelamento>();
                    volta = pcApp.ExecuteFilter(null, null, item.CentroLucro, null, item.Probabilidade, out listaPC);
                    if (volta == 1)
                    {
                        Session["FalhaPC"] = 1;
                    }
                    Session["ListaPC"] = listaPC;
                }
            }
            else
            {
                Session["ListaPC"] = new List<vwParcelamento>();
            }

            // Trata Lanc.Pagar
            if (item.LancPagar == 1 || item.LancPagar == null)
            {
                if (item.RecebimentoInicio != null || item.RecebimentoFinal != null || item.CentroLucro != null)
                {
                    List<vwLancamentosAPagar> listaLP = new List<vwLancamentosAPagar>();
                    volta = lpApp.ExecuteFilter(null, null, item.RecebimentoInicio, item.RecebimentoFinal, null, item.CentroLucro, null, out listaLP);
                    if (volta == 1)
                    {
                        Session["FalhaLP"] = 1;
                    }
                    Session["ListaLP"] = listaLP;
                }
            }
            else
            {
                Session["ListaLP"] = new List<vwLancamentosAPagar>();
            }

            // TRata executando positivo
            if (item.ExecPositivo == 1 || item.ExecPositivo == null)
            {
                listaEP = (List<vwExecutandoPositivo>)Session["ListaEP"];
            }
            else
            {
                Session["ListaEP"] = new List<vwExecutandoPositivo>();
            }

            // TRata executando negativo
            if (item.ExecNegativo == 1 || item.ExecNegativo == null)
            {
                listaEN = (List<vwExecutandoNegativo>)Session["ListaEN"];
            }
            else
            {
                Session["ListaEN"] = new List<vwExecutandoNegativo>();
            }

            rec = listaCR.Sum(p => p.Valor_Liquido);
            pag = listaCP.Sum(p => p.Valor);
            lp = listaLP.Sum(p => p.Valor);
            pc = listaPC.Sum(p => p.Valor_Bruto);
            ep = listaEP.Sum(p => p.ExecutandoPositivo.Value);
            en = listaEN.Sum(p => p.ExecutandoNegativo.Value);

            return RedirectToAction("MontarTelaFluxoCaixa");
        }
    }
}