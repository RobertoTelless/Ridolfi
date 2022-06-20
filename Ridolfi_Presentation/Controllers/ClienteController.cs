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

namespace ERP_CRM_Solution.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteAppService tranApp;
        private readonly IConfiguracaoAppService confApp;

        private String msg;
        private Exception exception;
        private String extensao;
        CLIENTE objetoTran = new CLIENTE();
        CLIENTE objetoTranAntes = new CLIENTE();
        List<CLIENTE> listaMasterTran = new List<CLIENTE>();

        public ClienteController(IClienteAppService tranApps, IConfiguracaoAppService confApps)
        {
            tranApp = tranApps;
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
            return RedirectToAction("MontarTelaDashboardCadastros", "BaseAdmin");
        }

        [HttpGet]
        public ActionResult MontarTelaCliente()
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
                    Session["MensCliente"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            if ((List<CLIENTE>)Session["ListaCliente"] == null || ((List<CLIENTE>)Session["ListaCliente"]).Count == 0)
            {
                listaMasterTran = tranApp.GetAllItens();
                Session["ListaCliente"] = listaMasterTran;
            }

            ViewBag.Listas = (List<CLIENTE>)Session["ListaCliente"];
            ViewBag.Title = "Cliente";
            ViewBag.Cats = new SelectList(tranApp.GetAllTipos().OrderBy(p => p.CACL_NM_NOME), "CACL_CD_ID", "CACL_NM_NOME");
            ViewBag.Precs = new SelectList(tranApp.GetAllPrecatorios().OrderBy(p => p.PREC_NM_PRECATORIO), "PREC_CD_ID", "PREC_NM_PRECATORIO");
            ViewBag.TRF = new SelectList(tranApp.GetAllTRF().OrderBy(p => p.TRF1_NM_NOME), "TRF1_CD_ID", "TRF1_NM_NOME");
            ViewBag.Vara = new SelectList(tranApp.GetAllVara().OrderBy(p => p.VARA_NM_NOME), "VARA_CD_ID", "VARA_NM_NOME");
            ViewBag.Titu = new SelectList(tranApp.GetAllTitularidade().OrderBy(p => p.TITU_NM_NOME), "TITU_CD_ID", "TITU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensCliente"] != null)
            {
                // Mensagem
                if ((Int32)Session["MensCliente"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0145", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            objetoTran = new CLIENTE();
            Session["MensCliente"] = 0;
            if (Session["FiltroCliente"] != null)
            {
                objetoTran = (CLIENTE)Session["FiltroCliente"];
            }
            return View(objetoTran);
        }

        public ActionResult RetirarFiltroCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaCliente"] = null;
            Session["FiltroCliente"] = null;
            return RedirectToAction("MontarTelaCliente");
        }

        public ActionResult MostrarTudoCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            listaMasterTran = tranApp.GetAllItensAdm();
            Session["FiltroCliente"] = null;
            Session["ListaCliente"] = listaMasterTran;
            return RedirectToAction("MontarTelaCliente");
        }

        [HttpPost]
        public ActionResult FiltrarCliente(CLIENTE item)
        {            
            try
            {
                if ((String)Session["Ativa"] == null)
                {
                    return RedirectToAction("Login", "ControleAcesso");
                }
                // Executa a operação
                List<CLIENTE> listaObj = new List<CLIENTE>();
                Session["FiltroCliente"] = item;
                Int32 volta = tranApp.ExecuteFilter(item.CACL_CD_ID, item.PREC_CD_ID, item.TRF1_CD_ID, item.VARA_CD_ID, item.TITU_CD_ID, item.CLIE_NM_NOME, item.CLIE_NR_OFICIO, item.CLIE_NR_PROCESSO, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensCliente"] = 1;
                }

                // Sucesso
                listaMasterTran = listaObj;
                Session["ListaCliente"] = listaObj;
                return RedirectToAction("MontarTelaCliente");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaCliente");
            }
        }

        public ActionResult VoltarBaseCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaCliente");
        }

        [HttpGet]
        public ActionResult IncluirCliente()
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
                if (usuario.PERFIL.PERF_SG_SIGLA == "FUN" || usuario.PERFIL.PERF_SG_SIGLA == "VIS")
                {
                    Session["MensCliente"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            ViewBag.Cats = new SelectList(tranApp.GetAllTipos().OrderBy(p => p.CACL_NM_NOME), "CACL_CD_ID", "CACL_NM_NOME");
            ViewBag.Precs = new SelectList(tranApp.GetAllPrecatorios().OrderBy(p => p.PREC_NM_PRECATORIO), "PREC_CD_ID", "PREC_NM_PRECATORIO");
            ViewBag.TRF = new SelectList(tranApp.GetAllTRF().OrderBy(p => p.TRF1_NM_NOME), "TRF1_CD_ID", "TRF1_NM_NOME");
            ViewBag.Vara = new SelectList(tranApp.GetAllVara().OrderBy(p => p.VARA_NM_NOME), "VARA_CD_ID", "VARA_NM_NOME");
            ViewBag.Titu = new SelectList(tranApp.GetAllTitularidade().OrderBy(p => p.TITU_NM_NOME), "TITU_CD_ID", "TITU_NM_NOME");

            // Prepara view
            CLIENTE item = new CLIENTE();
            ClienteViewModel vm = Mapper.Map<CLIENTE, ClienteViewModel>(item);
            vm.CLIE_IN_ATIVO = 1;
            vm.CLIE_VL_VALOR = 0;
            vm.CLIE_VL_VALOR_PAGO = 0;
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult IncluirCliente(ClienteViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            ViewBag.Cats = new SelectList(tranApp.GetAllTipos().OrderBy(p => p.CACL_NM_NOME), "CACL_CD_ID", "CACL_NM_NOME");
            ViewBag.Precs = new SelectList(tranApp.GetAllPrecatorios().OrderBy(p => p.PREC_NM_PRECATORIO), "PREC_CD_ID", "PREC_NM_PRECATORIO");
            ViewBag.TRF = new SelectList(tranApp.GetAllTRF().OrderBy(p => p.TRF1_NM_NOME), "TRF1_CD_ID", "TRF1_NM_NOME");
            ViewBag.Vara = new SelectList(tranApp.GetAllVara().OrderBy(p => p.VARA_NM_NOME), "VARA_CD_ID", "VARA_NM_NOME");
            ViewBag.Titu = new SelectList(tranApp.GetAllTitularidade().OrderBy(p => p.TITU_NM_NOME), "TITU_CD_ID", "TITU_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CLIENTE item = Mapper.Map<ClienteViewModel, CLIENTE>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = tranApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        Session["MensCliente"] = 3;
                    }

                    // Cria pastas
                    String caminho = "/Imagens/1" + "/Cliente/" + item.CLIE_CD_ID.ToString() + "/Anexos/";
                    Directory.CreateDirectory(Server.MapPath(caminho));

                    // Sucesso
                    listaMasterTran = new List<CLIENTE>();
                    Session["ListaCliente"] = null;
                    Session["IdCliente"] = item.CLIE_CD_ID;

                    if (Session["FileQueueCliente"] != null)
                    {
                        List<FileQueue> fq = (List<FileQueue>)Session["FileQueueCliente"];
                        foreach (var file in fq)
                        {
                            if (file.Profile == null)
                            {
                                UploadFileQueueCliente(file);
                            }
                        }
                        Session["FileQueueCliente"] = null;
                    }
                    return RedirectToAction("VoltarBaseCliente");
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
        public ActionResult VerCliente(Int32 id)
        {

            // Prepara view
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            CLIENTE item = tranApp.GetItemById(id);
            objetoTranAntes = item;
            Session["Cliente"] = item;
            Session["IdCliente"] = id;
            Session["VoltaComent"] = 2;
            ClienteViewModel vm = Mapper.Map<CLIENTE, ClienteViewModel>(item);
            return View(vm);
        }

        [HttpGet]
        public ActionResult EditarCliente(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA == "FUN" || usuario.PERFIL.PERF_SG_SIGLA == "VIS")
                {
                    Session["MensCliente"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            ViewBag.Cats = new SelectList(tranApp.GetAllTipos().OrderBy(p => p.CACL_NM_NOME), "CACL_CD_ID", "CACL_NM_NOME");
            ViewBag.Precs = new SelectList(tranApp.GetAllPrecatorios().OrderBy(p => p.PREC_NM_PRECATORIO), "PREC_CD_ID", "PREC_NM_PRECATORIO");
            ViewBag.TRF = new SelectList(tranApp.GetAllTRF().OrderBy(p => p.TRF1_NM_NOME), "TRF1_CD_ID", "TRF1_NM_NOME");
            ViewBag.Vara = new SelectList(tranApp.GetAllVara().OrderBy(p => p.VARA_NM_NOME), "VARA_CD_ID", "VARA_NM_NOME");
            ViewBag.Titu = new SelectList(tranApp.GetAllTitularidade().OrderBy(p => p.TITU_NM_NOME), "TITU_CD_ID", "TITU_NM_NOME");

            CLIENTE item = tranApp.GetItemById(id);
            objetoTranAntes = item;
            Session["Cliente"] = item;
            Session["IdCliente"] = id;
            Session["VoltaComent"] = 1;
            ClienteViewModel vm = Mapper.Map<CLIENTE, ClienteViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarCliente(ClienteViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Cats = new SelectList(tranApp.GetAllTipos().OrderBy(p => p.CACL_NM_NOME), "CACL_CD_ID", "CACL_NM_NOME");
            ViewBag.Precs = new SelectList(tranApp.GetAllPrecatorios().OrderBy(p => p.PREC_NM_PRECATORIO), "PREC_CD_ID", "PREC_NM_PRECATORIO");
            ViewBag.TRF = new SelectList(tranApp.GetAllTRF().OrderBy(p => p.TRF1_NM_NOME), "TRF1_CD_ID", "TRF1_NM_NOME");
            ViewBag.Vara = new SelectList(tranApp.GetAllVara().OrderBy(p => p.VARA_NM_NOME), "VARA_CD_ID", "VARA_NM_NOME");
            ViewBag.Titu = new SelectList(tranApp.GetAllTitularidade().OrderBy(p => p.TITU_NM_NOME), "TITU_CD_ID", "TITU_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    CLIENTE item = Mapper.Map<ClienteViewModel, CLIENTE>(vm);
                    Int32 volta = tranApp.ValidateEdit(item, objetoTranAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterTran = new List<CLIENTE>();
                    Session["ListaCliente"] = null;
                    return RedirectToAction("MontarTelaCliente");
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
        public ActionResult ExcluirCliente(Int32 id)
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
                if (usuario.PERFIL.PERF_SG_SIGLA == "FUN" || usuario.PERFIL.PERF_SG_SIGLA == "VIS")
                {
                    Session["MensCliente"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            CLIENTE item = tranApp.GetItemById(id);
            Session["Cliente"] = item;
            item.CLIE_IN_ATIVO = 0;
            Int32 volta = tranApp.ValidateDelete(item, usuario);
            listaMasterTran = new List<CLIENTE>();
            Session["ListaCliente"] = null;
            return RedirectToAction("MontarTelaCliente");
        }

        [HttpGet]
        public ActionResult ReativarCliente(Int32 id)
        {
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA == "FUN" || usuario.PERFIL.PERF_SG_SIGLA == "VIS")
                {
                    Session["MensCliente"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            CLIENTE item = tranApp.GetItemById(id);
            Session["Cliente"] = item;
            item.CLIE_IN_ATIVO = 1;
            Int32 volta = tranApp.ValidateReativar(item, usuario);
            listaMasterTran = new List<CLIENTE>();
            Session["ListaCliente"] = null;
            return RedirectToAction("MontarTelaCliente");
        }

        [HttpGet]
        public ActionResult VerAnexoCliente(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            CLIENTE_ANEXO item = tranApp.GetAnexoById(id);
            return View(item);
        }

        public ActionResult VoltarAnexoCliente()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            return RedirectToAction("EditarCliente", new { id = (Int32)Session["IdCliente"] });
        }

        public FileResult DownloadCliente(Int32 id)
        {
            CLIENTE_ANEXO item = tranApp.GetAnexoById(id);
            String arquivo = item.CLAN_AQ_ARQUIVO;
            Int32 pos = arquivo.LastIndexOf("/") + 1;
            String nomeDownload = arquivo.Substring(pos);
            String contentType = string.Empty;
            if (arquivo.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }
            else if (arquivo.Contains(".jpg"))
            {
                contentType = "image/jpg";
            }
            else if (arquivo.Contains(".png"))
            {
                contentType = "image/png";
            }
            return File(arquivo, contentType, nomeDownload);
        }

        [HttpPost]
        public void UploadFileToSession(IEnumerable<HttpPostedFileBase> files, String profile)
        {
            List<FileQueue> queue = new List<FileQueue>();

            foreach (var file in files)
            {
                FileQueue f = new FileQueue();
                f.Name = Path.GetFileName(file.FileName);
                f.ContentType = Path.GetExtension(file.FileName);

                MemoryStream ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                f.Contents = ms.ToArray();

                if (profile != null)
                {
                    if (file.FileName.Equals(profile))
                    {
                        f.Profile = 1;
                    }
                }

                queue.Add(f);
            }

            Session["FileQueueCliente"] = queue;
        }

        [HttpPost]
        public ActionResult UploadFileQueueCliente(FileQueue file)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdCliente"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (file == null)
            {
                Session["MensCliente"] = 5;
                return RedirectToAction("VoltarAnexoCliente");
            }

            CLIENTE item = tranApp.GetById(idNot);
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            var fileName = file.Name;
            if (fileName.Length > 250)
            {
                Session["MensCliente"] = 6;
                return RedirectToAction("VoltarAnexoCliente");
            }
            String caminho = "/Imagens/1" + "/Cliente/" + item.CLIE_CD_ID.ToString() + "/Anexos/";
            String path = Path.Combine(Server.MapPath(caminho), fileName);
            System.IO.File.WriteAllBytes(path, file.Contents);

            //Recupera tipo de arquivo
            extensao = Path.GetExtension(fileName);
            String a = extensao;

            // Gravar registro
            CLIENTE_ANEXO foto = new CLIENTE_ANEXO();
            foto.CLAN_AQ_ARQUIVO = "~" + caminho + fileName;
            foto.CLAN_DT_ANEXO = DateTime.Today;
            foto.CLAN_IN_ATIVO = 1;
            Int32 tipo = 3;
            if (extensao.ToUpper() == ".JPG" || extensao.ToUpper() == ".GIF" || extensao.ToUpper() == ".PNG" || extensao.ToUpper() == ".JPEG")
            {
                tipo = 1;
            }
            if (extensao.ToUpper() == ".MP4" || extensao.ToUpper() == ".AVI" || extensao.ToUpper() == ".MPEG")
            {
                tipo = 2;
            }
            if (extensao.ToUpper() == ".PDF")
            {
                tipo = 3;
            }
            foto.CLAN_IN_TIPO = tipo;
            foto.CLAN_NM_TITULO = fileName;
            foto.CLIE_CD_ID = item.CLIE_CD_ID;
            item.CLIENTE_ANEXO.Add(foto);
            objetoTranAntes = item;
            
            Int32 volta = tranApp.ValidateEdit(item, objetoTranAntes);
            return RedirectToAction("VoltarAnexoCliente");
        }

        [HttpPost]
        public ActionResult UploadFileCliente(HttpPostedFileBase file)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdCliente"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (file == null)
            {
                Session["MensCliente"] = 5;
                return RedirectToAction("VoltarAnexoCliente");
            }

            CLIENTE item = tranApp.GetById(idNot);
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            var fileName = Path.GetFileName(file.FileName);
            if (fileName.Length > 250)
            {
                Session["MensCliente"] = 6;
                return RedirectToAction("VoltarAnexoCliente");
            }
            String caminho = "/Imagens/1" + "/Cliente/" + item.CLIE_CD_ID.ToString() + "/Anexos/";
            String path = Path.Combine(Server.MapPath(caminho), fileName);
            file.SaveAs(path);

            //Recupera tipo de arquivo
            extensao = Path.GetExtension(fileName);
            String a = extensao;

            // Gravar registro
            CLIENTE_ANEXO foto = new CLIENTE_ANEXO();
            foto.CLAN_AQ_ARQUIVO = "~" + caminho + fileName;
            foto.CLAN_DT_ANEXO = DateTime.Today;
            foto.CLAN_IN_ATIVO = 1;
            Int32 tipo = 3;
            if (extensao.ToUpper() == ".JPG" || extensao.ToUpper() == ".GIF" || extensao.ToUpper() == ".PNG" || extensao.ToUpper() == ".JPEG")
            {
                tipo = 1;
            }
            if (extensao.ToUpper() == ".MP4" || extensao.ToUpper() == ".AVI" || extensao.ToUpper() == ".MPEG")
            {
                tipo = 2;
            }
            if (extensao.ToUpper() == ".PDF")
            {
                tipo = 3;
            }
            foto.CLAN_IN_TIPO = tipo;
            foto.CLAN_NM_TITULO = fileName;
            foto.CLIE_CD_ID = item.CLIE_CD_ID;

            item.CLIENTE_ANEXO.Add(foto);
            objetoTranAntes = item;
            Int32 volta = tranApp.ValidateEdit(item, objetoTranAntes);
            return RedirectToAction("VoltarAnexoCliente");
        }

        [HttpGet]
        public ActionResult IncluirAnotacao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 id = (Int32)Session["IdCliente"];
            CLIENTE item = tranApp.GetItemById(id);
            USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
            CLIENTE_ANOTACAO coment = new CLIENTE_ANOTACAO();
            ClienteAnotacaoViewModel vm = Mapper.Map<CLIENTE_ANOTACAO, ClienteAnotacaoViewModel>(coment);
            vm.CLAN_DT_ANOTACAO = DateTime.Now;
            vm.CLAN_IN_ATIVO = 1;
            vm.CLIE_CD_ID = item.CLIE_CD_ID;
            vm.USUARIO = usuarioLogado;
            vm.USUA_CD_ID = usuarioLogado.USUA_CD_ID;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirAnotacao(ClienteAnotacaoViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdCliente"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CLIENTE_ANOTACAO item = Mapper.Map<ClienteAnotacaoViewModel, CLIENTE_ANOTACAO>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    CLIENTE not = tranApp.GetItemById(idNot);

                    item.USUARIO = null;
                    not.CLIENTE_ANOTACAO.Add(item);
                    objetoTranAntes = not;
                    Int32 volta = tranApp.ValidateEdit(not, objetoTranAntes);

                    // Verifica retorno

                    // Sucesso
                    if ((Int32)Session["VoltaComent"] == 1)
                    {
                        return RedirectToAction("EditarCliente", new { id = idNot });
                    }
                    Session["VoltaComent"] = 0;
                    return RedirectToAction("VerCliente", new { id = idNot });
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

        //public ActionResult GerarRelatorioLista()
        //{            
        //    // Prepara geração
        //    String data = DateTime.Today.Date.ToShortDateString();
        //    data = data.Substring(0, 2) + data.Substring(3, 2) + data.Substring(6, 4);
        //    String nomeRel = "BeneficiarioLista" + "_" + data + ".pdf";
        //    List<BENEFICIARIO> lista = (List<BENEFICIARIO>)Session["ListaBeneficiario"];
        //    BENEFICIARIO filtro = (BENEFICIARIO)Session["FiltroBeneficiario"];
        //    Font meuFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        //    Font meuFont1 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        //    Font meuFont2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        //    // Cria documento
        //    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();

        //    // Linha horizontal
        //    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line);

        //    // Cabeçalho
        //    PdfPTable table = new PdfPTable(5);
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //    table.SpacingBefore = 1f;
        //    table.SpacingAfter = 1f;

        //    PdfPCell cell = new PdfPCell();
        //    cell.Border = 0;
        //    Image image = Image.GetInstance(Server.MapPath("~/Imagens/Base/LogoRidolfi.jpg"));
        //    image.ScaleAbsolute(50, 50);
        //    cell.AddElement(image);
        //    table.AddCell(cell);

        //    cell = new PdfPCell(new Paragraph("Baneficiários - Listagem", meuFont2))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_CENTER
        //    };
        //    cell.Border = 0;
        //    cell.Colspan = 4;
        //    table.AddCell(cell);
        //    pdfDoc.Add(table);

        //    // Linha Horizontal
        //    Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.GREEN, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line1);
        //    line1 = new Paragraph("  ");
        //    pdfDoc.Add(line1);

        //    // Grid
        //    table = new PdfPTable(new float[] { 70f, 120f, 120f, 70f, 80f});
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = 0;
        //    table.SpacingBefore = 1f;
        //    table.SpacingAfter = 1f;

        //    cell = new PdfPCell(new Paragraph("Beneficiários selecionados pelos parametros de filtro abaixo", meuFont1))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
        //    };
        //    cell.Colspan = 5;
        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    table.AddCell(cell);

        //    cell = new PdfPCell(new Paragraph("Tipo", meuFont))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_LEFT
        //    };
        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Nome", meuFont))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_LEFT
        //    };
        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Razão Social", meuFont))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_LEFT
        //    };
        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Data Nasc.", meuFont))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_LEFT
        //    };
        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Renda", meuFont))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_LEFT
        //    };
        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    table.AddCell(cell);

        //    foreach (BENEFICIARIO item in lista)
        //    {
        //        cell = new PdfPCell(new Paragraph(item.TIPO_PESSOA.TIPE_NM_NOME, meuFont))
        //        {
        //            VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
        //        };
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Paragraph(item.BENE_NM_NOME, meuFont))
        //        {
        //            VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
        //        };
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Paragraph(item.MOME_NM_RAZAO_SOCIAL, meuFont))
        //        {
        //            VerticalAlignment = Element.ALIGN_MIDDLE,
        //            HorizontalAlignment = Element.ALIGN_LEFT
        //        };
        //        table.AddCell(cell);
        //        if (item.BENE_DT_NASCIMENTO != null)
        //        {
        //            cell = new PdfPCell(new Paragraph(item.BENE_DT_NASCIMENTO.Value.ToShortDateString(), meuFont))
        //            {
        //                VerticalAlignment = Element.ALIGN_MIDDLE,
        //                HorizontalAlignment = Element.ALIGN_LEFT
        //            };
        //        }
        //        else
        //        {
        //            cell = new PdfPCell(new Paragraph("-", meuFont))
        //            {
        //                VerticalAlignment = Element.ALIGN_MIDDLE,
        //                HorizontalAlignment = Element.ALIGN_LEFT
        //            };
        //        }
        //        table.AddCell(cell);
        //        if (item.BENE_VL_RENDA != null)
        //        {
        //            cell = new PdfPCell(new Paragraph(CrossCutting.Formatters.DecimalFormatter(item.BENE_VL_RENDA.Value), meuFont))
        //            {
        //                VerticalAlignment = Element.ALIGN_MIDDLE,
        //                HorizontalAlignment = Element.ALIGN_LEFT
        //            };
        //        }
        //        else
        //        {
        //            cell = new PdfPCell(new Paragraph("-", meuFont))
        //            {
        //                VerticalAlignment = Element.ALIGN_MIDDLE,
        //                HorizontalAlignment = Element.ALIGN_LEFT
        //            };
        //        }
        //        table.AddCell(cell);
        //    }
        //    pdfDoc.Add(table);

        //    // Linha Horizontal
        //    Paragraph line2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line2);

        //    // Rodapé
        //    Chunk chunk1 = new Chunk("Parâmetros de filtro: ", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK));
        //    pdfDoc.Add(chunk1);

        //    String parametros = String.Empty;
        //    Int32 ja = 0;
        //    if (filtro != null)
        //    {
        //        if (filtro.TIPE_CD_ID != null)
        //        {
        //            parametros += "Tipo de Pessoa: " + filtro.TIPE_CD_ID;
        //            ja = 1;
        //        }
        //        if (filtro.SEXO_CD_ID != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Sexo: " + filtro.SEXO_CD_ID;
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros +=  " e Sexo: " + filtro.SEXO_CD_ID;
        //            }
        //        }
        //        if (filtro.ESCI_CD_ID != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Estado Civil: " + filtro.ESCI_CD_ID;
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros += " e Estado Civil: " + filtro.ESCI_CD_ID;
        //            }
        //        }
        //        if (filtro.ESCO_CD_ID != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Escolaridade: " + filtro.ESCO_CD_ID;
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros += " e Escolaridade: " + filtro.ESCO_CD_ID;
        //            }
        //        }
        //        if (filtro.PARE_CD_ID != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Parentesco: " + filtro.PARE_CD_ID;
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros += " e Parentesco: " + filtro.PARE_CD_ID;
        //            }
        //        }
        //        if (filtro.BENE_NM_NOME != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Nome: " + filtro.BENE_NM_NOME;
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros += " e Nome: " + filtro.BENE_NM_NOME;
        //            }
        //        }
        //        if (filtro.MOME_NM_RAZAO_SOCIAL != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Razão Social: " + filtro.MOME_NM_RAZAO_SOCIAL;
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros += " e Razão Social: " + filtro.MOME_NM_RAZAO_SOCIAL;
        //            }
        //        }
        //        if (filtro.BENE_DT_NASCIMENTO != null)
        //        {
        //            if (ja == 0)
        //            {
        //                parametros += "Data Nasc: " + filtro.BENE_DT_NASCIMENTO.Value.ToShortDateString();
        //                ja = 1;
        //            }
        //            else
        //            {
        //                parametros += " e Data Nasc: " + filtro.BENE_DT_NASCIMENTO.Value.ToShortDateString();
        //            }
        //        }
        //        if (ja == 0)
        //        {
        //            parametros = "Nenhum filtro definido.";
        //        }
        //    }
        //    else
        //    {
        //        parametros = "Nenhum filtro definido.";
        //    }
        //    Chunk chunk = new Chunk(parametros, FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK));
        //    pdfDoc.Add(chunk);

        //    // Linha Horizontal
        //    Paragraph line3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line3);

        //    // Finaliza
        //    pdfWriter.CloseStream = false;
        //    pdfDoc.Close();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + nomeRel);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Write(pdfDoc);
        //    Response.End();

        //    return RedirectToAction("MontarTelaBeneficiario");
        //}

        //public ActionResult GerarRelatorioDetalhe()
        //{

        //    // Prepara geração
        //    BENEFICIARIO tran = tranApp.GetById((Int32)Session["IdBeneficiario"]);
        //    String data = DateTime.Today.Date.ToShortDateString();
        //    data = data.Substring(0, 2) + data.Substring(3, 2) + data.Substring(6, 4);
        //    String nomeRel = "Beneficiario_" + tran.BENE_CD_ID.ToString() + "_" + data + ".pdf";
        //    Font meuFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        //    Font meuFont1 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        //    Font meuFont2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        //    Font meuFontBold = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        //    // Cria documento
        //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();

        //    // Linha horizontal
        //    Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line1);

        //    // Cabeçalho
        //    PdfPTable table = new PdfPTable(5);
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //    table.SpacingBefore = 1f;
        //    table.SpacingAfter = 1f;

        //    PdfPCell cell = new PdfPCell();
        //    cell.Border = 0;
        //    Image image = Image.GetInstance(Server.MapPath("~/Imagens/Base/LogoRidolfi.jpg"));
        //    image.ScaleAbsolute(50, 50);
        //    cell.AddElement(image);
        //    table.AddCell(cell);

        //    cell = new PdfPCell(new Paragraph("Beneficiario - Detalhes", meuFont2))
        //    {
        //        VerticalAlignment = Element.ALIGN_MIDDLE,
        //        HorizontalAlignment = Element.ALIGN_CENTER
        //    };
        //    cell.Border = 0;
        //    cell.Colspan = 4;
        //    table.AddCell(cell);

        //    pdfDoc.Add(table);

        //    // Linha Horizontal
        //    line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line1);
        //    line1 = new Paragraph("  ");
        //    pdfDoc.Add(line1);

        //    // Dados Gerais
        //    table = new PdfPTable(new float[] { 120f, 120f, 120f, 120f, 120f });
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = 0;
        //    table.SpacingBefore = 1f;
        //    table.SpacingAfter = 1f;

        //    cell = new PdfPCell(new Paragraph("Dados Gerais", meuFontBold));
        //    cell.Border = 0;
        //    cell.Colspan = 5;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);

        //    cell = new PdfPCell(new Paragraph("Nome: " + tran.BENE_NM_NOME, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 2;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Razão Social: " + tran.MOME_NM_RAZAO_SOCIAL, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 3;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);

        //    cell = new PdfPCell(new Paragraph("Tipo Pessoa: " + tran.TIPO_PESSOA.TIPE_NM_NOME, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 1;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Sexo: " + tran.SEXO.SEXO_NM_NOME, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 1;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Estado Civil: " + tran.ESTADO_CIVIL.ESCI_NM_NOME, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 1;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Escolaridade: " + tran.ESCOLARIDADE.ESCO_NM_NOME, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 1;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);
        //    cell = new PdfPCell(new Paragraph("Parentesco: " + tran.PARENTESCO.PARE_NM_NOME, meuFont));
        //    cell.Border = 0;
        //    cell.Colspan = 1;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    table.AddCell(cell);

        //    if (tran.BENE_DT_NASCIMENTO != null)
        //    {
        //        cell = new PdfPCell(new Paragraph("Data Nasc.: " + tran.BENE_DT_NASCIMENTO.Value.ToShortDateString(), meuFont));
        //        cell.Border = 0;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    }
        //    else
        //    {
        //        cell = new PdfPCell(new Paragraph("Data Nasc.: - ", meuFont));
        //        cell.Border = 0;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    }
        //    table.AddCell(cell);
        //    if (tran.BENE_VL_RENDA != null)
        //    {
        //        cell = new PdfPCell(new Paragraph("Renda: R$ " + CrossCutting.Formatters.DecimalFormatter(tran.BENE_VL_RENDA.Value), meuFont));
        //        cell.Border = 0;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    }
        //    else
        //    {
        //        cell = new PdfPCell(new Paragraph("Renda: - ", meuFont));
        //        cell.Border = 0;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    }
        //    table.AddCell(cell);
        //    if (tran.BENE_VL_RENDA_ESTIMADA != null)
        //    {
        //        cell = new PdfPCell(new Paragraph("Renda Estimada: R$ " + CrossCutting.Formatters.DecimalFormatter(tran.BENE_VL_RENDA_ESTIMADA.Value), meuFont));
        //        cell.Border = 0;
        //        cell.Colspan = 3;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    }
        //    else
        //    {
        //        cell = new PdfPCell(new Paragraph("Renda Estimada: - ", meuFont));
        //        cell.Border = 0;
        //        cell.Colspan = 3;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    }
        //    table.AddCell(cell);
        //    pdfDoc.Add(table);

        //    // Linha Horizontal
        //    line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line1);

        //    // Finaliza
        //    pdfWriter.CloseStream = false;
        //    pdfDoc.Close();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + nomeRel);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Write(pdfDoc);
        //    Response.End();

        //    return RedirectToAction("VoltarAnexoBeneficiario");
        //}

        [HttpGet]
        public ActionResult EditarContato(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            // Prepara view

            CLIENTE_CONTATO item = tranApp.GetContatoById(id);
            ClienteContatoViewModel vm = Mapper.Map<CLIENTE_CONTATO, ClienteContatoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarContato(ClienteContatoViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    CLIENTE_CONTATO item = Mapper.Map<ClienteContatoViewModel, CLIENTE_CONTATO>(vm);
                    Int32 volta = tranApp.ValidateEditContato(item);

                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoCliente");
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
        public ActionResult ExcluirContato(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            CLIENTE_CONTATO item = tranApp.GetContatoById(id);
            item.CLCO_IN_ATIVO = 0;
            Int32 volta = tranApp.ValidateEditContato(item);
            return RedirectToAction("VoltarAnexoCliente");
        }

        [HttpGet]
        public ActionResult ReativarContato(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            CLIENTE_CONTATO item = tranApp.GetContatoById(id);
            item.CLCO_IN_ATIVO = 1;
            Int32 volta = tranApp.ValidateEditContato(item);
            return RedirectToAction("VoltarAnexoCliente");
        }

        [HttpGet]
        public ActionResult IncluirContato()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            CLIENTE_CONTATO item = new CLIENTE_CONTATO();
            ClienteContatoViewModel vm = Mapper.Map<CLIENTE_CONTATO, ClienteContatoViewModel>(item);
            vm.CLIE_CD_ID = (Int32)Session["IdCliente"];
            vm.CLCO_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirContato(ClienteContatoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CLIENTE_CONTATO item = Mapper.Map<ClienteContatoViewModel, CLIENTE_CONTATO>(vm);
                    Int32 volta = tranApp.ValidateCreateContato(item);
                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoCliente");
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


    }
}