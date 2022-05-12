using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EntitiesServices.Model;
using EntitiesServices.DTO;
using EntitiesServices.Work_Classes;
using ModelServices.Interfaces.Repositories;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Data.Entity;
using System.Data;

namespace ModelServices.EntitiesServices
{
    public class OSEspService : ServiceBase<vwOrdemServicoEspecialidade>, IOSEspService
    {
        private readonly IOSEspRepository _baseRepository;
        protected OnSuiteBIEntities Db = new OnSuiteBIEntities();

        public OSEspService(IOSEspRepository baseRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public List<vwOrdemServicoEspecialidade> GetAllItens()
        {
            return _baseRepository.GetAllItens();
        }

        public List<DTO_OS_UF> GetItensOSUF()
        {
            return _baseRepository.GetItensOSUF();
        }

        public List<DTO_OS_UF> GetItensOSTipo()
        {
            return _baseRepository.GetItensOSTipo();
        }

        public List<DTO_OS_UF> GetItensOSCidade()
        {
            return _baseRepository.GetItensOSCidade();
        }

        public List<DTO_OS_UF> GetOSAtrasoCidade()
        {
            return _baseRepository.GetOSAtrasoCidade();
        }

        public Int32 Create(vwOrdemServicoEspecialidade item)
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



    }
}
