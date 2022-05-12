using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using ModelServices.Interfaces.Repositories;
using EntitiesServices.Work_Classes;
using System.Data.Entity;
using EntitiesServices.DTO;

namespace DataServices.Repositories
{
    public class CRRepository : RepositoryBase<vwContasAReceber>, ICRRepository
    {
        public List<vwContasAReceber> GetAllItens()
        {
            IQueryable<vwContasAReceber> query = Db.vwContasAReceber;
            return query.ToList();
        }

        public List<vwContasAReceber> GetByData(DateTime data)
        {
            IQueryable<vwContasAReceber> query = Db.vwContasAReceber;
            query = query.Where(p => p.Data_de_Vencimento.Month == data.Month & p.Data_de_Vencimento.Year == data.Year);
            return query.ToList();
        }

        public List<vwContasAReceber> ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, DateTime? recInicio, DateTime? recFinal, String centroLucro, String sacado, Int32? prob)
        {
            List<vwContasAReceber> lista = new List<vwContasAReceber>();
            IQueryable<vwContasAReceber> query = Db.vwContasAReceber;
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
                query = query.Where(p => p.Sacado == sacado);
            }
            if (emissaoInicio != null & emissaoFinal != null)
            {
                query = query.Where(p => p.Data_de_Emissao >= emissaoInicio & p.Data_de_Emissao <= emissaoFinal);
            }
            else if (emissaoInicio != null & emissaoFinal == null)
            {
                query = query.Where(p => p.Data_de_Emissao >= emissaoInicio);
            }
            else if (emissaoInicio == null & emissaoFinal != null)
            {
                query = query.Where(p => p.Data_de_Emissao <= emissaoFinal);
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
            if (recInicio != null & recFinal != null)
            {
                query = query.Where(p => p.Data_de_Recebimento >= recInicio & p.Data_de_Recebimento <= recFinal);
            }
            else if (recInicio != null & recFinal == null)
            {
                query = query.Where(p => p.Data_de_Recebimento >= recInicio);
            }
            else if (recInicio == null & recFinal != null)
            {
                query = query.Where(p => p.Data_de_Recebimento <= recFinal);
            }
            if (query != null)
            {
                query = query.OrderByDescending(a => a.Data_de_Vencimento);
                lista = query.ToList<vwContasAReceber>();
            }
            return lista;
        }

    }
}
 