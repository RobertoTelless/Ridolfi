using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Text.RegularExpressions;

namespace ApplicationServices.Services
{
    public class PrecatorioAppService : AppServiceBase<PRECATORIO>, IPrecatorioAppService
    {
        private readonly IPrecatorioService _baseService;
        private readonly IConfiguracaoService _confService;

        public PrecatorioAppService(IPrecatorioService baseService, IConfiguracaoService confService) : base(baseService)
        {
            _baseService = baseService;
            _confService = confService;
        }

        public List<PRECATORIO> GetAllItens()
        {
            List<PRECATORIO> lista = _baseService.GetAllItens();
            return lista;
        }

        public List<PRECATORIO> GetAllItensAdm()
        {
            List<PRECATORIO> lista = _baseService.GetAllItensAdm();
            return lista;
        }

        public PRECATORIO GetItemById(Int32 id)
        {
            PRECATORIO item = _baseService.GetItemById(id);
            return item;
        }

        public PRECATORIO CheckExist(PRECATORIO conta)
        {
            PRECATORIO item = _baseService.CheckExist(conta);
            return item;
        }

        public List<BENEFICIARIO> GetAllBeneficiarios()
        {
            List<BENEFICIARIO> lista = _baseService.GetAllBeneficiarios();
            return lista;
        }

        public List<TRF> GetAllTRF()
        {
            List<TRF> lista = _baseService.GetAllTRF();
            return lista;
        }

        public List<HONORARIO> GetAllAdvogados()
        {
            List<HONORARIO> lista = _baseService.GetAllAdvogados();
            return lista;
        }

        public List<BANCO> GetAllBancos()
        {
            List<BANCO> lista = _baseService.GetAllBancos();
            return lista;
        }

        public List<NATUREZA> GetAllNaturezas()
        {
            List<NATUREZA> lista = _baseService.GetAllNaturezas();
            return lista;
        }

        public List<PRECATORIO_ESTADO> GetAllEstados()
        {
            List<PRECATORIO_ESTADO> lista = _baseService.GetAllEstados();
            return lista;
        }

        public PRECATORIO_ANEXO GetAnexoById(Int32 id)
        {
            PRECATORIO_ANEXO lista = _baseService.GetAnexoById(id);
            return lista;
        }

        public PRECATORIO_ANOTACAO GetComentarioById(Int32 id)
        {
            PRECATORIO_ANOTACAO lista = _baseService.GetComentarioById(id);
            return lista;
        }

        public Int32 ExecuteFilter(Int32? trf, Int32? beneficiario, Int32? advogado, Int32? natureza, Int32? estado, String nome, String ano, out List<PRECATORIO> objeto)
        {
            try
            {
                objeto = new List<PRECATORIO>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(trf, beneficiario, advogado, natureza, estado, nome, ano);
                if (objeto.Count == 0)
                {
                    volta = 1;
                }
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateCreate(PRECATORIO item, USUARIO usuario)
        {
            try
            {
                // Completa objeto
                item.PREC_IN_ATIVO = 1;


                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_NM_OPERACAO = "AddPREC",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<PRECATORIO>(item)
                };

                // Persiste
                Int32 volta = _baseService.Create(item, log);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(PRECATORIO item, PRECATORIO itemAntes, USUARIO usuario)
        {
            try
            {
                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_NM_OPERACAO = "EditPREC",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<PRECATORIO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<PRECATORIO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(PRECATORIO item, PRECATORIO itemAntes)
        {
            try
            {
                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateDelete(PRECATORIO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.CLIENTE.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.PREC_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DelPREC",
                    LOG_TX_REGISTRO = item.PREC_NM_PRECATORIO
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(PRECATORIO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.PREC_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatPREC",
                    LOG_TX_REGISTRO = item.PREC_NM_PRECATORIO
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
