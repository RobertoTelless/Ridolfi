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
    public class ClienteAppService : AppServiceBase<CLIENTE>, IClienteAppService
    {
        private readonly IClienteService _baseService;
        private readonly IConfiguracaoService _confService;

        public ClienteAppService(IClienteService baseService, IConfiguracaoService confService) : base(baseService)
        {
            _baseService = baseService;
            _confService = confService;
        }

        public List<CLIENTE> GetAllItens()
        {
            List<CLIENTE> lista = _baseService.GetAllItens();
            return lista;
        }

        public List<CLIENTE> GetAllItensAdm()
        {
            List<CLIENTE> lista = _baseService.GetAllItensAdm();
            return lista;
        }

        public CLIENTE_ANOTACAO GetAnotacaoById(Int32 id)
        {
            return _baseService.GetAnotacaoById(id);
        }

        public CLIENTE GetItemById(Int32 id)
        {
            CLIENTE item = _baseService.GetItemById(id);
            return item;
        }

        public CLIENTE CheckExist(CLIENTE conta)
        {
            CLIENTE item = _baseService.CheckExist(conta);
            return item;
        }

        public List<CATEGORIA_CLIENTE> GetAllTipos()
        {
            List<CATEGORIA_CLIENTE> lista = _baseService.GetAllTipos();
            return lista;
        }

        public List<PRECATORIO> GetAllPrecatorios()
        {
            List<PRECATORIO> lista = _baseService.GetAllPrecatorios();
            return lista;
        }

        public List<TRF> GetAllTRF()
        {
            List<TRF> lista = _baseService.GetAllTRF();
            return lista;
        }

        public List<VARA> GetAllVara()
        {
            List<VARA> lista = _baseService.GetAllVara();
            return lista;
        }

        public List<TITULARIDADE> GetAllTitularidade()
        {
            List<TITULARIDADE> lista = _baseService.GetAllTitularidade();
            return lista;
        }

        public CLIENTE_ANEXO GetAnexoById(Int32 id)
        {
            CLIENTE_ANEXO lista = _baseService.GetAnexoById(id);
            return lista;
        }

        public CLIENTE_CONTATO GetContatoById(Int32 id)
        {
            CLIENTE_CONTATO lista = _baseService.GetContatoById(id);
            return lista;
        }

        public Int32 ExecuteFilter(Int32? catId, Int32? precatorio, Int32? trf, Int32? vara, Int32? titularidade, String nome, String oficio, String processo, out List<CLIENTE> objeto)
        {
            try
            {
                objeto = new List<CLIENTE>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(catId, precatorio, trf, vara, titularidade, nome, oficio, processo);
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

        public Int32 ValidateCreate(CLIENTE item, USUARIO usuario)
        {
            try
            {
                // Completa objeto
                item.CLIE_IN_ATIVO = 1;


                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_NM_OPERACAO = "AddCLIE",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CLIENTE>(item)
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

        public Int32 ValidateEdit(CLIENTE item, CLIENTE itemAntes, USUARIO usuario)
        {
            try
            {
                if (itemAntes.CATEGORIA_CLIENTE != null)
                {
                    itemAntes.CATEGORIA_CLIENTE = null;
                }

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_NM_OPERACAO = "EditCLIE",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CLIENTE>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<CLIENTE>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(CLIENTE item, CLIENTE itemAntes)
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

        public Int32 ValidateDelete(CLIENTE item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (true)
                {

                }

                // Acerta campos
                item.CLIE_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DelCLIE",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CLIENTE>(item)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(CLIENTE item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.CLIE_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatCLIE",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CLIENTE>(item)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEditContato(CLIENTE_CONTATO item)
        {
            try
            {
                // Persiste
                return _baseService.EditContato(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateCreateContato(CLIENTE_CONTATO item)
        {
            try
            {
                item.CLCO_IN_ATIVO = 1;

                // Persiste
                Int32 volta = _baseService.CreateContato(item);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
