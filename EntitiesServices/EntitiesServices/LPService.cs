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
    public class LPService : ServiceBase<vwLancamentosAPagar>, ILPService
    {
        private readonly ILPRepository _baseRepository;
        protected OnSuiteBIEntities Db = new OnSuiteBIEntities();

        public LPService(ILPRepository baseRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public List<vwLancamentosAPagar> GetAllItens()
        {
            return _baseRepository.GetAllItens();
        }

        public Int32 Create(vwLancamentosAPagar item)
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


        public Int32 Edit(vwLancamentosAPagar item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    vwLancamentosAPagar obj = _baseRepository.GetById(item.Ordem_de_Servico);
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

        public Int32 Delete(vwLancamentosAPagar item)
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

        public List<vwLancamentosAPagar> ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String centroCusto, String beneficiario)
        {
            return _baseRepository.ExecuteFilter(emissaoInicio, emissaoFinal, vencInicio, vencFinal, centroLucro, centroCusto, beneficiario);
        }

    }
}
