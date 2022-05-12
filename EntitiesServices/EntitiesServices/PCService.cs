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
    public class PCService : ServiceBase<vwParcelamento>, IPCService
    {
        private readonly IPCRepository _baseRepository;
        protected OnSuiteBIEntities Db = new OnSuiteBIEntities();

        public PCService(IPCRepository baseRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public List<vwParcelamento> GetAllItens()
        {
            return _baseRepository.GetAllItens();
        }

        public Int32 Create(vwParcelamento item)
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


        public Int32 Edit(vwParcelamento item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    vwParcelamento obj = _baseRepository.GetById(item.Ordem_de_Servico);
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

        public Int32 Delete(vwParcelamento item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
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

        public List<vwParcelamento> ExecuteFilter(DateTime? vencInicio, DateTime? vencFinal, String centroLucro, string sacado, Int32? prob)
        {
            return _baseRepository.ExecuteFilter(vencInicio, vencFinal, centroLucro, sacado, prob);
        }

    }
}
