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
    public class BeneficiarioController : Controller
    {
        private readonly IBeneficiarioAppService tranApp;
        private readonly IConfiguracaoAppService confApp;

        private String msg;
        private Exception exception;
        private String extensao;
        BENEFICIARIO objetoTran = new BENEFICIARIO();
        BENEFICIARIO objetoTranAntes = new BENEFICIARIO();
        List<BENEFICIARIO> listaMasterTran = new List<BENEFICIARIO>();

        public BeneficiarioController(IBeneficiarioAppService tranApps, IConfiguracaoAppService confApps)
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
        public ActionResult MontarTelaBeneficiario()
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
                    Session["MensBeneficiario"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            if ((List<BENEFICIARIO>)Session["ListaBeneficiario"] == null || ((List<BENEFICIARIO>)Session["ListaBeneficiario"]).Count == 0)
            {
                listaMasterTran = tranApp.GetAllItens();
                Session["ListaBeneficiario"] = listaMasterTran;
            }

            ViewBag.Listas = (List<BENEFICIARIO>)Session["ListaBeneficiario"];
            ViewBag.Title = "Beneficiario";
            ViewBag.TipoPessoa = new SelectList(tranApp.GetAllTiposPessoa().OrderBy(p => p.TIPE_NM_NOME), "TIPE_CD_ID", "TIPE_NM_NOME");
            ViewBag.Sexo = new SelectList(tranApp.GetAllSexo().OrderBy(p => p.SEXO_NM_NOME), "SEXO_CD_ID", "SEXO_NM_NOME");
            ViewBag.EstadoCivil = new SelectList(tranApp.GetAllEstadoCivil().OrderBy(p => p.ESCI_NM_NOME), "ESCI_CD_ID", "ESCI_NM_NOME");
            ViewBag.Escolaridade = new SelectList(tranApp.GetAllEscolaridade().OrderBy(p => p.ESCO_NM_NOME), "ESCO_CD_ID", "ESCO_NM_NOME");
            ViewBag.Parentesco = new SelectList(tranApp.GetAllParentesco().OrderBy(p => p.PARE_NM_NOME), "PARE_CD_ID", "PARE_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            if (Session["MensBeneficiario"] != null)
            {
                // Mensagem
                if ((Int32)Session["MensBeneficiario"] == 2)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0145", CultureInfo.CurrentCulture));
                }
            }

            // Abre view
            objetoTran = new BENEFICIARIO();
            Session["MensBeneficiario"] = 0;
            Session["VoltaBeneficiario"] = 1;
            if (Session["FiltroBeneficiario"] != null)
            {
                objetoTran = (BENEFICIARIO)Session["FiltroBeneficiario"];
            }
            return View(objetoTran);
        }

        public ActionResult RetirarFiltroBeneficiario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaBeneficiario"] = null;
            Session["FiltroBeneficiario"] = null;
            return RedirectToAction("MontarTelaBeneficiario");
        }

        public ActionResult MostrarTudoBeneficiario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            listaMasterTran = tranApp.GetAllItensAdm();
            Session["FiltroBeneficiario"] = null;
            Session["ListaBeneficiario"] = listaMasterTran;
            return RedirectToAction("MontarTelaBeneficiario");
        }

        [HttpPost]
        public ActionResult FiltrarBeneficiario(BENEFICIARIO item)
        {            
            try
            {
                if ((String)Session["Ativa"] == null)
                {
                    return RedirectToAction("Login", "ControleAcesso");
                }
                // Executa a operação
                List<BENEFICIARIO> listaObj = new List<BENEFICIARIO>();
                Session["FiltroBeneficiario"] = item;
                Int32 volta = tranApp.ExecuteFilter(item.TIPE_CD_ID, item.SEXO_CD_ID, item.ESCI_CD_ID, item.ESCO_CD_ID, item.PARE_CD_ID, item.MOME_NM_RAZAO_SOCIAL, item.BENE_NM_NOME, item.BENE_DT_NASCIMENTO, item.BENE_NR_CPF, item.BENE_NR_CNPJ, item.BENE_NM_PARENTESCO, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensBeneficiario"] = 1;
                }

                // Sucesso
                listaMasterTran = listaObj;
                Session["ListaBeneficiario"] = listaObj;
                return RedirectToAction("MontarTelaBeneficiario");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaBeneficiario");
            }
        }

        public ActionResult VoltarBaseBeneficiario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaBeneficiario");
        }

        [HttpGet]
        public ActionResult IncluirBeneficiario()
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
                    Session["MensBeneficiario"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            ViewBag.TipoPessoa = new SelectList(tranApp.GetAllTiposPessoa().OrderBy(p => p.TIPE_NM_NOME), "TIPE_CD_ID", "TIPE_NM_NOME");
            ViewBag.Sexo = new SelectList(tranApp.GetAllSexo().OrderBy(p => p.SEXO_NM_NOME), "SEXO_CD_ID", "SEXO_NM_NOME");
            ViewBag.EstadoCivil = new SelectList(tranApp.GetAllEstadoCivil().OrderBy(p => p.ESCI_NM_NOME), "ESCI_CD_ID", "ESCI_NM_NOME");
            ViewBag.Escolaridade = new SelectList(tranApp.GetAllEscolaridade().OrderBy(p => p.ESCO_NM_NOME), "ESCO_CD_ID", "ESCO_NM_NOME");
            ViewBag.Parentesco = new SelectList(tranApp.GetAllParentesco().OrderBy(p => p.PARE_NM_NOME), "PARE_CD_ID", "PARE_NM_NOME");

            // Prepara view
            BENEFICIARIO item = new BENEFICIARIO();
            BeneficiarioViewModel vm = Mapper.Map<BENEFICIARIO, BeneficiarioViewModel>(item);
            vm.BENE_DT_CADASTRO = DateTime.Today.Date;
            vm.BENE_IN_ATIVO = 1;
            vm.BENE_VL_RENDA = 0;
            vm.BENE_VL_RENDA_ESTIMADA = 0;
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult IncluirBeneficiario(BeneficiarioViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            ViewBag.TipoPessoa = new SelectList(tranApp.GetAllTiposPessoa().OrderBy(p => p.TIPE_NM_NOME), "TIPE_CD_ID", "TIPE_NM_NOME");
            ViewBag.Sexo = new SelectList(tranApp.GetAllSexo().OrderBy(p => p.SEXO_NM_NOME), "SEXO_CD_ID", "SEXO_NM_NOME");
            ViewBag.EstadoCivil = new SelectList(tranApp.GetAllEstadoCivil().OrderBy(p => p.ESCI_NM_NOME), "ESCI_CD_ID", "ESCI_NM_NOME");
            ViewBag.Escolaridade = new SelectList(tranApp.GetAllEscolaridade().OrderBy(p => p.ESCO_NM_NOME), "ESCO_CD_ID", "ESCO_NM_NOME");
            ViewBag.Parentesco = new SelectList(tranApp.GetAllParentesco().OrderBy(p => p.PARE_NM_NOME), "PARE_CD_ID", "PARE_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    BENEFICIARIO item = Mapper.Map<BeneficiarioViewModel, BENEFICIARIO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = tranApp.ValidateCreate(item, usuario);

                    // Verifica retorno

                    // Cria pastas
                    String caminho = "/Imagens/1" + "/Beneficiarios/" + item.BENE_CD_ID.ToString() + "/Anexos/";
                    Directory.CreateDirectory(Server.MapPath(caminho));

                    // Sucesso
                    listaMasterTran = new List<BENEFICIARIO>();
                    Session["ListaBeneficiario"] = null;
                    Session["IdBeneficiario"] = item.BENE_CD_ID;

                    if (Session["FileQueueTrans"] != null)
                    {
                        List<FileQueue> fq = (List<FileQueue>)Session["FileQueueTrans"];
                        foreach (var file in fq)
                        {
                            if (file.Profile == null)
                            {
                                UploadFileQueueBeneficiario(file);
                            }
                        }
                        Session["FileQueueTrans"] = null;
                    }
                    return RedirectToAction("VoltarAnexoBeneficiario");
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
        public ActionResult VerBeneficiario(Int32 id)
        {

            // Prepara view
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            BENEFICIARIO item = tranApp.GetItemById(id);
            objetoTranAntes = item;
            Session["Beneficiario"] = item;
            Session["IdBeneficiario"] = id;
            Session["VoltaComent"] = 2;
            BeneficiarioViewModel vm = Mapper.Map<BENEFICIARIO, BeneficiarioViewModel>(item);
            return View(vm);
        }

        [HttpGet]
        public ActionResult EditarBeneficiario(Int32 id)
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
                    Session["MensBeneficiario"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            ViewBag.TipoPessoa = new SelectList(tranApp.GetAllTiposPessoa().OrderBy(p => p.TIPE_NM_NOME), "TIPE_CD_ID", "TIPE_NM_NOME");
            ViewBag.Sexo = new SelectList(tranApp.GetAllSexo().OrderBy(p => p.SEXO_NM_NOME), "SEXO_CD_ID", "SEXO_NM_NOME");
            ViewBag.EstadoCivil = new SelectList(tranApp.GetAllEstadoCivil().OrderBy(p => p.ESCI_NM_NOME), "ESCI_CD_ID", "ESCI_NM_NOME");
            ViewBag.Escolaridade = new SelectList(tranApp.GetAllEscolaridade().OrderBy(p => p.ESCO_NM_NOME), "ESCO_CD_ID", "ESCO_NM_NOME");
            ViewBag.Parentesco = new SelectList(tranApp.GetAllParentesco().OrderBy(p => p.PARE_NM_NOME), "PARE_CD_ID", "PARE_NM_NOME");

            BENEFICIARIO item = tranApp.GetItemById(id);
            objetoTranAntes = item;
            Session["Beneficiario"] = item;
            Session["IdBeneficiario"] = id;
            Session["VoltaComent"] = 1;
            BeneficiarioViewModel vm = Mapper.Map<BENEFICIARIO, BeneficiarioViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarBeneficiario(BeneficiarioViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.TipoPessoa = new SelectList(tranApp.GetAllTiposPessoa().OrderBy(p => p.TIPE_NM_NOME), "TIPE_CD_ID", "TIPE_NM_NOME");
            ViewBag.Sexo = new SelectList(tranApp.GetAllSexo().OrderBy(p => p.SEXO_NM_NOME), "SEXO_CD_ID", "SEXO_NM_NOME");
            ViewBag.EstadoCivil = new SelectList(tranApp.GetAllEstadoCivil().OrderBy(p => p.ESCI_NM_NOME), "ESCI_CD_ID", "ESCI_NM_NOME");
            ViewBag.Escolaridade = new SelectList(tranApp.GetAllEscolaridade().OrderBy(p => p.ESCO_NM_NOME), "ESCO_CD_ID", "ESCO_NM_NOME");
            ViewBag.Parentesco = new SelectList(tranApp.GetAllParentesco().OrderBy(p => p.PARE_NM_NOME), "PARE_CD_ID", "PARE_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    BENEFICIARIO item = Mapper.Map<BeneficiarioViewModel, BENEFICIARIO>(vm);
                    Int32 volta = tranApp.ValidateEdit(item, objetoTranAntes, usuario);

                    // Verifica retorno

                    // Sucesso
                    listaMasterTran = new List<BENEFICIARIO>();
                    Session["ListaBeneficiario"] = null;
                    return RedirectToAction("MontarTelaBeneficiario");
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
        public ActionResult ExcluirBeneficiario(Int32 id)
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
                    Session["MensBeneficiario"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            BENEFICIARIO item = tranApp.GetItemById(id);
            Session["Beneficiario"] = item;
            item.BENE_IN_ATIVO = 0;
            Int32 volta = tranApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensBeneficiario"] = 2;
                return RedirectToAction("MontarTelaBeneficiario", "Beneficiario");
            }
            listaMasterTran = new List<BENEFICIARIO>();
            Session["ListaBeneficiario"] = null;
            return RedirectToAction("MontarTelaBeneficiario");
        }

        [HttpGet]
        public ActionResult ReativarBeneficiario(Int32 id)
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
                    Session["MensBeneficiario"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            BENEFICIARIO item = tranApp.GetItemById(id);
            Session["Beneficiario"] = item;
            item.BENE_IN_ATIVO = 1;
            Int32 volta = tranApp.ValidateReativar(item, usuario);
            listaMasterTran = new List<BENEFICIARIO>();
            Session["ListaBeneficiario"] = null;
            return RedirectToAction("MontarTelaBeneficiario");
        }

        public ActionResult VerCardsBeneficiario()
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
                    Session["MensBeneficiario"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            if ((List<BENEFICIARIO>)Session["ListaBeneficiario"] == null || ((List<BENEFICIARIO>)Session["ListaBeneficiario"]).Count == 0)
            {
                listaMasterTran = tranApp.GetAllItens();
                Session["ListaBeneficiario"] = listaMasterTran;
            }

            ViewBag.Listas = (List<BENEFICIARIO>)Session["ListaBeneficiario"];
            ViewBag.Title = "Beneficiario";

            // Abre view
            objetoTran = new BENEFICIARIO();
            Session["VoltaBeneficiario"] = 2;
            if (Session["FiltroBeneficiario"] != null)
            {
                objetoTran = (BENEFICIARIO)Session["FiltroBeneficiario"];
            }
            return View(objetoTran);
        }

        [HttpGet]
        public ActionResult VerAnexoBeneficiario(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            BENEFICIARIO_ANEXO item = tranApp.GetAnexoById(id);
            return View(item);
        }

        public ActionResult VoltarAnexoBeneficiario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            return RedirectToAction("EditarBeneficiario", new { id = (Int32)Session["IdBeneficiario"] });
        }

        public FileResult DownloadBeneficiario(Int32 id)
        {
            BENEFICIARIO_ANEXO item = tranApp.GetAnexoById(id);
            String arquivo = item.BEAN_AQ_ARQUIVO;
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

            Session["FileQueueTrans"] = queue;
        }

        [HttpPost]
        public ActionResult UploadFileQueueBeneficiario(FileQueue file)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdBeneficiario"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (file == null)
            {
                Session["MensBeneficiario"] = 5;
                return RedirectToAction("VoltarAnexoBeneficiario");
            }

            BENEFICIARIO item = tranApp.GetById(idNot);
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            var fileName = file.Name;
            if (fileName.Length > 250)
            {
                Session["MensBeneficiario"] = 6;
                return RedirectToAction("VoltarAnexoBeneficiario");
            }
            String caminho = "/Imagens/1" + "/Beneficiarios/" + item.BENE_CD_ID.ToString() + "/Anexos/";
            String path = Path.Combine(Server.MapPath(caminho), fileName);
            System.IO.File.WriteAllBytes(path, file.Contents);

            //Recupera tipo de arquivo
            extensao = Path.GetExtension(fileName);
            String a = extensao;

            // Gravar registro
            BENEFICIARIO_ANEXO foto = new BENEFICIARIO_ANEXO();
            foto.BEAN_AQ_ARQUIVO = "~" + caminho + fileName;
            foto.BEAN_DT_ANEXO = DateTime.Today;
            foto.BEAN_IN_ATIVO = 1;
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
            foto.BEAN_IN_TIPO = tipo;
            foto.BEAN_NM_TITULO = fileName;
            foto.BENE_CD_ID = item.BENE_CD_ID;
            item.BENEFICIARIO_ANEXO.Add(foto);
            objetoTranAntes = item;
            
            Int32 volta = tranApp.ValidateEdit(item, objetoTranAntes);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpPost]
        public ActionResult UploadFileBeneficiario(HttpPostedFileBase file)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdBeneficiario"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (file == null)
            {
                Session["MensBeneficiario"] = 5;
                return RedirectToAction("VoltarAnexoBeneficiario");
            }

            BENEFICIARIO item = tranApp.GetById(idNot);
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            var fileName = Path.GetFileName(file.FileName);
            if (fileName.Length > 250)
            {
                Session["MensBeneficiario"] = 6;
                return RedirectToAction("VoltarAnexoBeneficiario");
            }
            String caminho = "/Imagens/1" + "/Beneficiarios/" + item.BENE_CD_ID.ToString() + "/Anexos/";
            String path = Path.Combine(Server.MapPath(caminho), fileName);
            file.SaveAs(path);

            //Recupera tipo de arquivo
            extensao = Path.GetExtension(fileName);
            String a = extensao;

            // Gravar registro
            BENEFICIARIO_ANEXO foto = new BENEFICIARIO_ANEXO();
            foto.BEAN_AQ_ARQUIVO = "~" + caminho + fileName;
            foto.BEAN_DT_ANEXO = DateTime.Today;
            foto.BEAN_IN_ATIVO = 1;
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
            foto.BEAN_IN_TIPO = tipo;
            foto.BEAN_NM_TITULO = fileName;
            foto.BENE_CD_ID = item.BENE_CD_ID;

            item.BENEFICIARIO_ANEXO.Add(foto);
            objetoTranAntes = item;
            Int32 volta = tranApp.ValidateEdit(item, objetoTranAntes);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult IncluirComentario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 id = (Int32)Session["IdBeneficiario"];
            BENEFICIARIO item = tranApp.GetItemById(id);
            USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
            BENEFICIARIO_ANOTACOES coment = new BENEFICIARIO_ANOTACOES();
            BeneficiarioComentarioViewModel vm = Mapper.Map<BENEFICIARIO_ANOTACOES, BeneficiarioComentarioViewModel>(coment);
            vm.BECO_DT_COMENTARIO = DateTime.Now;
            vm.BECO_IN_ATIVO = 1;
            vm.BENE_CD_ID = item.BENE_CD_ID;
            vm.USUARIO = usuarioLogado;
            vm.USUA_CD_ID = usuarioLogado.USUA_CD_ID;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirComentario(BeneficiarioComentarioViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdBeneficiario"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    BENEFICIARIO_ANOTACOES item = Mapper.Map<BeneficiarioComentarioViewModel, BENEFICIARIO_ANOTACOES>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    BENEFICIARIO not = tranApp.GetItemById(idNot);

                    item.USUARIO = null;
                    not.BENEFICIARIO_ANOTACOES.Add(item);
                    objetoTranAntes = not;
                    Int32 volta = tranApp.ValidateEdit(not, objetoTranAntes);

                    // Verifica retorno

                    // Sucesso
                    if ((Int32)Session["VoltaComent"] == 1)
                    {
                        return RedirectToAction("EditarBeneficiario", new { id = idNot });
                    }
                    Session["VoltaComent"] = 0;
                    return RedirectToAction("VerBeneficiario", new { id = idNot });
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

        public ActionResult GerarRelatorioLista()
        {            
            // Prepara geração
            String data = DateTime.Today.Date.ToShortDateString();
            data = data.Substring(0, 2) + data.Substring(3, 2) + data.Substring(6, 4);
            String nomeRel = "BeneficiarioLista" + "_" + data + ".pdf";
            List<BENEFICIARIO> lista = (List<BENEFICIARIO>)Session["ListaBeneficiario"];
            BENEFICIARIO filtro = (BENEFICIARIO)Session["FiltroBeneficiario"];
            Font meuFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFont1 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFont2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Cria documento
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            // Linha horizontal
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            // Cabeçalho
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.SpacingBefore = 1f;
            table.SpacingAfter = 1f;

            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            Image image = Image.GetInstance(Server.MapPath("~/Imagens/Base/LogoRidolfi.jpg"));
            image.ScaleAbsolute(50, 50);
            cell.AddElement(image);
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Baneficiários - Listagem", meuFont2))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            cell.Border = 0;
            cell.Colspan = 4;
            table.AddCell(cell);
            pdfDoc.Add(table);

            // Linha Horizontal
            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.GREEN, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line1);
            line1 = new Paragraph("  ");
            pdfDoc.Add(line1);

            // Grid
            table = new PdfPTable(new float[] { 70f, 120f, 120f, 70f, 80f});
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 1f;
            table.SpacingAfter = 1f;

            cell = new PdfPCell(new Paragraph("Beneficiários selecionados pelos parametros de filtro abaixo", meuFont1))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.Colspan = 5;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Tipo", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Nome", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Razão Social", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Data Nasc.", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Renda", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);

            foreach (BENEFICIARIO item in lista)
            {
                cell = new PdfPCell(new Paragraph(item.TIPO_PESSOA.TIPE_NM_NOME, meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Paragraph(item.BENE_NM_NOME, meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Paragraph(item.MOME_NM_RAZAO_SOCIAL, meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                if (item.BENE_DT_NASCIMENTO != null)
                {
                    cell = new PdfPCell(new Paragraph(item.BENE_DT_NASCIMENTO.Value.ToShortDateString(), meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                }
                else
                {
                    cell = new PdfPCell(new Paragraph("-", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                }
                table.AddCell(cell);
                if (item.BENE_VL_RENDA != null)
                {
                    cell = new PdfPCell(new Paragraph(CrossCutting.Formatters.DecimalFormatter(item.BENE_VL_RENDA.Value), meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                }
                else
                {
                    cell = new PdfPCell(new Paragraph("-", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                }
                table.AddCell(cell);
            }
            pdfDoc.Add(table);

            // Linha Horizontal
            Paragraph line2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line2);

            // Rodapé
            Chunk chunk1 = new Chunk("Parâmetros de filtro: ", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK));
            pdfDoc.Add(chunk1);

            String parametros = String.Empty;
            Int32 ja = 0;
            if (filtro != null)
            {
                if (filtro.TIPE_CD_ID != null)
                {
                    parametros += "Tipo de Pessoa: " + filtro.TIPE_CD_ID;
                    ja = 1;
                }
                if (filtro.SEXO_CD_ID != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Sexo: " + filtro.SEXO_CD_ID;
                        ja = 1;
                    }
                    else
                    {
                        parametros +=  " e Sexo: " + filtro.SEXO_CD_ID;
                    }
                }
                if (filtro.ESCI_CD_ID != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Estado Civil: " + filtro.ESCI_CD_ID;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Estado Civil: " + filtro.ESCI_CD_ID;
                    }
                }
                if (filtro.ESCO_CD_ID != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Escolaridade: " + filtro.ESCO_CD_ID;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Escolaridade: " + filtro.ESCO_CD_ID;
                    }
                }
                if (filtro.PARE_CD_ID != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Parentesco: " + filtro.PARE_CD_ID;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Parentesco: " + filtro.PARE_CD_ID;
                    }
                }
                if (filtro.BENE_NM_NOME != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Nome: " + filtro.BENE_NM_NOME;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Nome: " + filtro.BENE_NM_NOME;
                    }
                }
                if (filtro.MOME_NM_RAZAO_SOCIAL != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Razão Social: " + filtro.MOME_NM_RAZAO_SOCIAL;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Razão Social: " + filtro.MOME_NM_RAZAO_SOCIAL;
                    }
                }
                if (filtro.BENE_DT_NASCIMENTO != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Data Nasc: " + filtro.BENE_DT_NASCIMENTO.Value.ToShortDateString();
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Data Nasc: " + filtro.BENE_DT_NASCIMENTO.Value.ToShortDateString();
                    }
                }
                if (ja == 0)
                {
                    parametros = "Nenhum filtro definido.";
                }
            }
            else
            {
                parametros = "Nenhum filtro definido.";
            }
            Chunk chunk = new Chunk(parametros, FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            // Linha Horizontal
            Paragraph line3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line3);

            // Finaliza
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + nomeRel);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return RedirectToAction("MontarTelaBeneficiario");
        }

        public ActionResult GerarRelatorioDetalhe()
        {
            
            // Prepara geração
            BENEFICIARIO tran = tranApp.GetById((Int32)Session["IdBeneficiario"]);
            String data = DateTime.Today.Date.ToShortDateString();
            data = data.Substring(0, 2) + data.Substring(3, 2) + data.Substring(6, 4);
            String nomeRel = "Beneficiario_" + tran.BENE_CD_ID.ToString() + "_" + data + ".pdf";
            Font meuFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFont1 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFont2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFontBold = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            // Cria documento
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            // Linha horizontal
            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line1);

            // Cabeçalho
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.SpacingBefore = 1f;
            table.SpacingAfter = 1f;

            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            Image image = Image.GetInstance(Server.MapPath("~/Imagens/Base/LogoRidolfi.jpg"));
            image.ScaleAbsolute(50, 50);
            cell.AddElement(image);
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Beneficiario - Detalhes", meuFont2))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            cell.Border = 0;
            cell.Colspan = 4;
            table.AddCell(cell);

            pdfDoc.Add(table);

            // Linha Horizontal
            line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line1);
            line1 = new Paragraph("  ");
            pdfDoc.Add(line1);

            // Dados Gerais
            table = new PdfPTable(new float[] { 120f, 120f, 120f, 120f, 120f });
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 1f;
            table.SpacingAfter = 1f;

            cell = new PdfPCell(new Paragraph("Dados Gerais", meuFontBold));
            cell.Border = 0;
            cell.Colspan = 5;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Nome: " + tran.BENE_NM_NOME, meuFont));
            cell.Border = 0;
            cell.Colspan = 2;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Razão Social: " + tran.MOME_NM_RAZAO_SOCIAL, meuFont));
            cell.Border = 0;
            cell.Colspan = 3;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Tipo Pessoa: " + tran.TIPO_PESSOA.TIPE_NM_NOME, meuFont));
            cell.Border = 0;
            cell.Colspan = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Sexo: " + tran.SEXO.SEXO_NM_NOME, meuFont));
            cell.Border = 0;
            cell.Colspan = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Estado Civil: " + tran.ESTADO_CIVIL.ESCI_NM_NOME, meuFont));
            cell.Border = 0;
            cell.Colspan = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Escolaridade: " + tran.ESCOLARIDADE.ESCO_NM_NOME, meuFont));
            cell.Border = 0;
            cell.Colspan = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Parentesco: " + tran.PARENTESCO.PARE_NM_NOME, meuFont));
            cell.Border = 0;
            cell.Colspan = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            if (tran.BENE_DT_NASCIMENTO != null)
            {
                cell = new PdfPCell(new Paragraph("Data Nasc.: " + tran.BENE_DT_NASCIMENTO.Value.ToShortDateString(), meuFont));
                cell.Border = 0;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                cell = new PdfPCell(new Paragraph("Data Nasc.: - ", meuFont));
                cell.Border = 0;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            table.AddCell(cell);
            if (tran.BENE_VL_RENDA != null)
            {
                cell = new PdfPCell(new Paragraph("Renda: R$ " + CrossCutting.Formatters.DecimalFormatter(tran.BENE_VL_RENDA.Value), meuFont));
                cell.Border = 0;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                cell = new PdfPCell(new Paragraph("Renda: - ", meuFont));
                cell.Border = 0;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            table.AddCell(cell);
            if (tran.BENE_VL_RENDA_ESTIMADA != null)
            {
                cell = new PdfPCell(new Paragraph("Renda Estimada: R$ " + CrossCutting.Formatters.DecimalFormatter(tran.BENE_VL_RENDA_ESTIMADA.Value), meuFont));
                cell.Border = 0;
                cell.Colspan = 3;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                cell = new PdfPCell(new Paragraph("Renda Estimada: - ", meuFont));
                cell.Border = 0;
                cell.Colspan = 3;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            table.AddCell(cell);
            pdfDoc.Add(table);

            // Linha Horizontal
            line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line1);

            // Finaliza
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + nomeRel);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult EnviarEMailBeneficiario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            USUARIO usuario = (USUARIO)Session["UserCredentials"];

            if (Session["MensMensagem"] != null)
            {
                if ((Int32)Session["MensMensagem"] == 66)
                {
                    ModelState.AddModelError("", PlatMensagens_Resources.ResourceManager.GetString("M0026", CultureInfo.CurrentCulture));
                }
            }

            // recupera beneficiario
            BENEFICIARIO cli = tranApp.GetItemById((Int32)Session["IdBeneficiario"]);
            Session["Beneficiario"] = cli;

            // Prepara mensagem
            String header = "Prezado <b>" + cli.BENE_NM_NOME + "</b>";
            String body = String.Empty;
            String footer = "<b>" + "Atenciosamente" + "</b>";

            // Monta vm
            MensagemViewModel vm = new MensagemViewModel();
            vm.MENS_DT_CRIACAO = DateTime.Now;
            vm.MENS_IN_ATIVO = 1;
            vm.NOME = cli.BENE_NM_NOME;
            vm.ID = (Int32)Session["IdBeneficiario"];
            vm.MODELO = cli.BENE_EM_EMAIL;
            vm.USUA_CD_ID = usuario.USUA_CD_ID;
            vm.MENS_NM_CABECALHO = header;
            vm.MENS_NM_RODAPE = footer;
            vm.MENS_IN_TIPO = 1;
            vm.ID = cli.BENE_CD_ID;
            return View(vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EnviarEMailBeneficiario(MensagemViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (ModelState.IsValid)
            {
                Int32 idNot = (Int32)Session["IdBeneficiario"];
                try
                {
                    // Checa corpo da mensagem
                    if (String.IsNullOrEmpty(vm.MENS_TX_TEXTO))
                    {
                        Session["MensMensagem"] = 66;
                        return RedirectToAction("EnviarEMailCliente");
                    }

                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 volta = ProcessaEnvioEMailBeneficiario(vm, usuarioLogado);

                    // Verifica retorno
                    if (volta == 1)
                    {

                    }

                    // Sucesso
                    return RedirectToAction("VoltarAnexoBeneficiario");
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

        [ValidateInput(false)]
        public Int32 ProcessaEnvioEMailBeneficiario(MensagemViewModel vm, USUARIO usuario)
        {
            // Recupera cliente
            Int32 idAss = (Int32)Session["IdAssinante"];
            BENEFICIARIO cont = (BENEFICIARIO)Session["Beneficiario"];

            // Processa e-mail
            CONFIGURACAO conf = confApp.GetItemById(1);

            // Prepara cabeçalho
            String cab = "Prezado Sr(a). <b>" + cont.BENE_NM_NOME + "</b>";

            // Prepara rodape
            String rod = "<b>" + "Atenciosamente"+ "</b>";

            // Prepara corpo do e-mail e trata link
            String corpo = vm.MENS_TX_TEXTO + "<br /><br />";
            StringBuilder str = new StringBuilder();
            str.AppendLine(corpo);
            if (!String.IsNullOrEmpty(vm.MENS_NM_LINK))
            {
                if (!vm.MENS_NM_LINK.Contains("www."))
                {
                    vm.MENS_NM_LINK = "www." + vm.MENS_NM_LINK;
                }
                if (!vm.MENS_NM_LINK.Contains("http://"))
                {
                    vm.MENS_NM_LINK = "http://" + vm.MENS_NM_LINK;
                }
                str.AppendLine("<a href='" + vm.MENS_NM_LINK + "'>Clique aqui para maiores informações</a>");
            }
            String body = str.ToString();
            String emailBody = cab + "<br /><br />" + body + "<br /><br />" + rod;

            // Monta e-mail
            NetworkCredential net = new NetworkCredential(conf.CONF_NM_EMAIL_EMISSOO, conf.CONF_NM_SENHA_EMISSOR);
            Email mensagem = new Email();
            mensagem.ASSUNTO = "Contato";
            mensagem.CORPO = emailBody;
            mensagem.DEFAULT_CREDENTIALS = false;
            mensagem.EMAIL_DESTINO = cont.BENE_EM_EMAIL;
            mensagem.EMAIL_EMISSOR = conf.CONF_NM_EMAIL_EMISSOO;
            mensagem.ENABLE_SSL = true;
            mensagem.NOME_EMISSOR = "Ridolfi";
            mensagem.PORTA = conf.CONF_NM_PORTA_SMTP;
            mensagem.PRIORIDADE = System.Net.Mail.MailPriority.High;
            mensagem.SENHA_EMISSOR = conf.CONF_NM_SENHA_EMISSOR;
            mensagem.SMTP = conf.CONF_NM_HOST_SMTP;
            mensagem.IS_HTML = true;
            mensagem.NETWORK_CREDENTIAL = net;

            // Envia mensagem
            try
            {
                Int32 voltaMail = CommunicationPackage.SendEmail(mensagem);
            }
            catch (Exception ex)
            {
                String erro = ex.Message;
            }
            return 0;
        }

        [HttpGet]
        public ActionResult EnviarSMSBeneficiario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            BENEFICIARIO cont = tranApp.GetItemById((Int32)Session["IdBeneficiario"]);
            Session["Beneficiario"] = cont;
            ViewBag.Beneficiario = cont;
            MensagemViewModel mens = new MensagemViewModel();
            mens.NOME = cont.BENE_NM_NOME;
            mens.ID = (Int32)Session["IdBeneficiario"];
            mens.MODELO = cont.BENE_NR_CELULAR;
            mens.MENS_DT_CRIACAO = DateTime.Today.Date;
            mens.MENS_IN_TIPO = 2;
            return View(mens);
        }

        [HttpPost]
        public ActionResult EnviarSMSBeneficiario(MensagemViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idNot = (Int32)Session["IdBeneficiario"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 volta = ProcessaEnvioSMSBeneficiario(vm, usuarioLogado);

                    // Verifica retorno
                    if (volta == 1)
                    {

                    }

                    // Sucesso
                    return RedirectToAction("VoltarAnexoBeneficiario");
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

        [ValidateInput(false)]
        public Int32 ProcessaEnvioSMSBeneficiario(MensagemViewModel vm, USUARIO usuario)
        {
            // Recupera contatos
            Int32 idAss = (Int32)Session["IdAssinante"];
            BENEFICIARIO cont = (BENEFICIARIO)Session["Beneficiario"];

            // Processa SMS
            CONFIGURACAO conf = confApp.GetItemById(1);

            // Monta token
            String text = conf.CONF_SG_LOGIN_SMS + ":" + conf.CONF_SG_SENHA_SMS;
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            String token = Convert.ToBase64String(textBytes);
            String auth = "Basic " + token;

            // Prepara texto
            String texto = vm.MENS_TX_SMS;

            // Prepara corpo do SMS e trata link
            StringBuilder str = new StringBuilder();
            str.AppendLine(vm.MENS_TX_SMS);
            if (!String.IsNullOrEmpty(vm.LINK))
            {
                if (!vm.LINK.Contains("www."))
                {
                    vm.LINK = "www." + vm.LINK;
                }
                if (!vm.LINK.Contains("http://"))
                {
                    vm.LINK = "http://" + vm.LINK;
                }
                str.AppendLine("<a href='" + vm.LINK + "'>Clique aqui para maiores informações</a>");
                texto += "  " + vm.LINK;
            }
            String body = str.ToString();
            String smsBody = body;
            String erro = null;

            // inicia processo
            String resposta = String.Empty;

            // Monta destinatarios
            try
            {
                String listaDest = "55" + Regex.Replace(cont.BENE_NR_CELULAR, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled).ToString();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api-v2.smsfire.com.br/sms/send/bulk");
                httpWebRequest.Headers["Authorization"] = auth;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                String customId = Cryptography.GenerateRandomPassword(8);
                String data = String.Empty;
                String json = String.Empty;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    json = String.Concat("{\"destinations\": [{\"to\": \"", listaDest, "\", \"text\": \"", texto, "\", \"customId\": \"" + customId + "\", \"from\": \"Ridolfi\"}]}");
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    resposta = result;
                }
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return 0;
        }


        [HttpGet]
        public ActionResult EditarEndereco(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            // Prepara view
            ViewBag.UF = new SelectList(tranApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");

            ENDERECO item = tranApp.GetEnderecoById(id);
            EnderecoViewModel vm = Mapper.Map<ENDERECO, EnderecoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEndereco(EnderecoViewModel vm)
        {
            ViewBag.UF = new SelectList(tranApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
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
                    ENDERECO item = Mapper.Map<EnderecoViewModel, ENDERECO>(vm);
                    Int32 volta = tranApp.ValidateEditEndereco(item);

                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoBeneficiario");
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
        public ActionResult ExcluirEndereco(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            ENDERECO item = tranApp.GetEnderecoById(id);
            item.ENDE_IN_ATIVO = 0;
            Int32 volta = tranApp.ValidateEditEndereco(item);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult ReativarEndereco(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            ENDERECO item = tranApp.GetEnderecoById(id);
            item.ENDE_IN_ATIVO = 1;
            Int32 volta = tranApp.ValidateEditEndereco(item);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult IncluirEndereco()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            ViewBag.UF = new SelectList(tranApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");

            ENDERECO item = new ENDERECO();
            EnderecoViewModel vm = Mapper.Map<ENDERECO, EnderecoViewModel>(item);
            vm.BENE_CD_ID = (Int32)Session["IdBeneficiario"];
            vm.ENDE_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirEndereco(EnderecoViewModel vm)
        {
            ViewBag.UF = new SelectList(tranApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    ENDERECO item = Mapper.Map<EnderecoViewModel, ENDERECO>(vm);
                    Int32 volta = tranApp.ValidateCreateEndereco(item);
                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoBeneficiario");
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
        public ActionResult EditarEMail(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            // Prepara view

            EMAIL item = tranApp.GetEMailById(id);
            EMailViewModel vm = Mapper.Map<EMAIL, EMailViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEMail(EMailViewModel vm)
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
                    EMAIL item = Mapper.Map<EMailViewModel, EMAIL>(vm);
                    Int32 volta = tranApp.ValidateEditEMail(item);

                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoBeneficiario");
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
        public ActionResult ExcluirEMail(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            EMAIL item = tranApp.GetEMailById(id);
            item.EMAI_IN_ATIVO = 0;
            Int32 volta = tranApp.ValidateEditEMail(item);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult ReativarEMail(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            EMAIL item = tranApp.GetEMailById(id);
            item.EMAI_IN_ATIVO = 1;
            Int32 volta = tranApp.ValidateEditEMail(item);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult IncluirEMail()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            EMAIL item = new EMAIL();
            EMailViewModel vm = Mapper.Map<EMAIL, EMailViewModel>(item);
            vm.BENE_CD_ID = (Int32)Session["IdBeneficiario"];
            vm.EMAI_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirEMail(EMailViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    EMAIL item = Mapper.Map<EMailViewModel, EMAIL>(vm);
                    Int32 volta = tranApp.ValidateCreateEMail(item);
                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoBeneficiario");
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

        [HttpPost]
        public JsonResult PesquisaCEP_Javascript(String cep, int tipoEnd)
        {
            // Chama servico ECT
            ZipCodeLoad zipLoad = new ZipCodeLoad();
            ZipCodeInfo end = new ZipCodeInfo();
            ZipCode zipCode = null;
            cep = CrossCutting.ValidarNumerosDocumentos.RemoveNaoNumericos(cep);
            if (ZipCode.TryParse(cep, out zipCode))
            {
                end = zipLoad.Find(zipCode);
            }

            // Atualiza
            var hash = new Hashtable();
            hash.Add("ENDE_NM_ENDERECO", end.Address);
            hash.Add("ENDE_NM_BAIRRO", end.District);
            hash.Add("ENDE_NM_CIDADE", end.City);
            hash.Add("UF_CD_ID", tranApp.GetUFbySigla(end.Uf).UF_CD_ID);
            hash.Add("ENDE_NR_CEP", cep);

            // Retorna
            Session["VoltaCEP"] = 2;
            return Json(hash);
        }

        [HttpGet]
        public ActionResult VerContato(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            // Prepara view
            CONTATO item = tranApp.GetContatoById(id);
            ContatoViewModel vm = Mapper.Map<CONTATO, ContatoViewModel>(item);
            return View(vm);
        }

        [HttpGet]
        public ActionResult EditarTelefone(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            // Prepara view
            ViewBag.Tipo = new SelectList(tranApp.GetAllTipoTelefone(), "TITE_CD_ID", "TITE_NM_NOME");

            TELEFONE item = tranApp.GetTelefoneById(id);
            TelefoneBenefViewModel vm = Mapper.Map<TELEFONE, TelefoneBenefViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTelefone(TelefoneBenefViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            ViewBag.Tipo = new SelectList(tranApp.GetAllTipoTelefone(), "TITE_CD_ID", "TITE_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    TELEFONE item = Mapper.Map<TelefoneBenefViewModel, TELEFONE>(vm);
                    Int32 volta = tranApp.ValidateEditTelefone(item);

                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoBeneficiario");
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
        public ActionResult ExcluirTelefone(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            TELEFONE item = tranApp.GetTelefoneById(id);
            item.TELE_IN_ATIVO = 0;
            Int32 volta = tranApp.ValidateEditTelefone(item);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult ReativarTelefone(Int32 id)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            TELEFONE item = tranApp.GetTelefoneById(id);
            item.TELE_IN_ATIVO = 1;
            Int32 volta = tranApp.ValidateEditTelefone(item);
            return RedirectToAction("VoltarAnexoBeneficiario");
        }

        [HttpGet]
        public ActionResult IncluirTelefone()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara view
            ViewBag.Tipo = new SelectList(tranApp.GetAllTipoTelefone(), "TITE_CD_ID", "TITE_NM_NOME");
            TELEFONE item = new TELEFONE();
            TelefoneBenefViewModel vm = Mapper.Map<TELEFONE, TelefoneBenefViewModel>(item);
            vm.BENE_CD_ID = (Int32)Session["IdBeneficiario"];
            vm.TELE_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirTelefone(TelefoneBenefViewModel vm)
        {
            ViewBag.Tipo = new SelectList(tranApp.GetAllTipoTelefone(), "TITE_CD_ID", "TITE_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    TELEFONE item = Mapper.Map<TelefoneBenefViewModel, TELEFONE>(vm);
                    Int32 volta = tranApp.ValidateCreateTelefone(item);
                    // Verifica retorno
                    return RedirectToAction("VoltarAnexoBeneficiario");
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