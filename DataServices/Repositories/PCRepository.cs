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
    public class PCRepository : RepositoryBase<vwParcelamento>, IPCRepository
    {
        public List<vwParcelamento> GetAllItens()
        {
            IQueryable<vwParcelamento> query = Db.vwParcelamento;
            return query.ToList();
        }

        public List<vwParcelamento> ExecuteFilter(DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String sacado, Int32? prob)
        {
            List<vwParcelamento> lista = new List<vwParcelamento>();
            IQueryable<vwParcelamento> query = Db.vwParcelamento;
            if (prob != null & prob > 0)
            {
                query = query.Where(p => p.Probabilidade == prob);
            }
            if (!String.IsNullOrEmpty(centroLucro))
            {
                query = query.Where(p => p.Centro_de_Lucro == centroLucro);
            }
            if (!String.IsNullOrEmpty(sacado))
            {
                query = query.Where(p => p.Cliente == sacado);
            }
            if (vencInicio != null & vencFinal != null)
            {
                query = query.Where(p => p.Data_de_Vencimento >= vencInicio & p.Data_de_Vencimento <= vencFinal);
            }
            else if (vencInicio != null & vencFinal == null)
            {
                query = query.Where(p => p.Data_de_Vencimento >= vencInicio);
            }
            else if (vencInicio == null & vencFinal != null)
            {
                query = query.Where(p => p.Data_de_Vencimento <= vencFinal);
            }
            if (query != null)
            {
                query = query.OrderByDescending(a => a.Data_de_Vencimento);
                lista = query.ToList<vwParcelamento>();
            }
            return lista;
        }
    }
}
 