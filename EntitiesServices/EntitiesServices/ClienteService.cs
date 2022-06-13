using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ModelServices.Interfaces.Repositories;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Data.Entity;
using System.Data;

namespace ModelServices.EntitiesServices
{
    public class ClienteService : ServiceBase<CLIENTE>, IClienteService
    {
        private readonly IClienteRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly ICategoriaClienteRepository _tipoRepository;
        private readonly IClienteAnexoRepository _anexoRepository;
        private readonly IClienteContatoRepository _contRepository;
        private readonly IPrecatorioRepository _preRepository;
        private readonly ITRFRepository _trfRepository;
        private readonly IVaraRepository _varaRepository;
        private readonly ITitularidadeRepository _titRepository;
        private readonly IClienteAnotacaoRepository _anoRepository;
        protected DB_RidolfiEntities Db = new DB_RidolfiEntities();

        public ClienteService(IClienteRepository baseRepository, ILogRepository logRepository, ICategoriaClienteRepository tipoRepository, IClienteAnexoRepository anexoRepository, IPrecatorioRepository preRepository, IClienteContatoRepository contRepository, ITRFRepository trfRepository, IVaraRepository varaRepository, ITitularidadeRepository titRepository, IClienteAnotacaoRepository anoRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _tipoRepository = tipoRepository;
            _anexoRepository = anexoRepository;
            _contRepository = contRepository;
            _preRepository = preRepository;
            _trfRepository = trfRepository;
            _varaRepository = varaRepository;
            _titRepository = titRepository;
            _anoRepository = anoRepository;
        }

        public CLIENTE CheckExist(CLIENTE conta)
        {
            CLIENTE item = _baseRepository.CheckExist(conta);
            return item;
        }

        public CLIENTE GetItemById(Int32 id)
        {
            CLIENTE item = _baseRepository.GetItemById(id);
            return item;
        }

        public CLIENTE_ANOTACAO GetAnotacaoById(Int32 id)
        {
            return _anoRepository.GetItemById(id);
        }

        public List<CLIENTE> GetAllItens()
        {
            return _baseRepository.GetAllItens();
        }

        public List<CLIENTE> GetAllItensAdm()
        {
            return _baseRepository.GetAllItensAdm();
        }

        public List<PRECATORIO> GetAllPrecatorios()
        {
            return _preRepository.GetAllItens();
        }

        public List<CATEGORIA_CLIENTE> GetAllTipos()
        {
            return _tipoRepository.GetAllItens();
        }

        public List<TRF> GetAllTRF()
        {
            return _trfRepository.GetAllItens();
        }

        public List<VARA> GetAllVara()
        {
            return _varaRepository.GetAllItens();
        }

        public List<TITULARIDADE> GetAllTitularidade()
        {
            return _titRepository.GetAllItens();
        }

        public CLIENTE_ANEXO GetAnexoById(Int32 id)
        {
            return _anexoRepository.GetItemById(id);
        }

        public CLIENTE_CONTATO GetContatoById(Int32 id)
        {
            return _contRepository.GetItemById(id);
        }

        public List<CLIENTE> ExecuteFilter(Int32? catId, Int32? precatorio, Int32? trf, Int32? vara, Int32? titularidade, String nome, String oficio, String processo)
        {
            return _baseRepository.ExecuteFilter(catId, precatorio, trf, vara, titularidade, nome, oficio, processo);
        }

        public Int32 Create(CLIENTE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _logRepository.Add(log);
                    _baseRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Create(CLIENTE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _baseRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }


        public Int32 Edit(CLIENTE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CLIENTE obj = _baseRepository.GetById(item.CLIE_CD_ID);
                    _baseRepository.Detach(obj);
                    _logRepository.Add(log);
                    _baseRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Edit(CLIENTE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CLIENTE obj = _baseRepository.GetById(item.CLIE_CD_ID);
                    _baseRepository.Detach(obj);
                    _baseRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Delete(CLIENTE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _logRepository.Add(log);
                    _baseRepository.Remove(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 EditContato(CLIENTE_CONTATO item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CLIENTE_CONTATO obj = _contRepository.GetById(item.CLCO_CD_ID);
                    _contRepository.Detach(obj);
                    _contRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 CreateContato(CLIENTE_CONTATO item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _contRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
