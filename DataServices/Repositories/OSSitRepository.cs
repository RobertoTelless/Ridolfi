using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using ModelServices.Interfaces.Repositories;
using EntitiesServices.Work_Classes;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class OSSitRepository : RepositoryBase<vwOrdemServicoSituacao>, IOSSitRepository
    {
        public List<vwOrdemServicoSituacao> GetAllItens()
        {
            IQueryable<vwOrdemServicoSituacao> query = Db.vwOrdemServicoSituacao;
            return query.ToList();
        }
    }
}
 