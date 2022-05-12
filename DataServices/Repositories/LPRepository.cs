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
    public class LPRepository : RepositoryBase<vwLancamentosAPagar>, ILPRepository
    {
        public List<vwLancamentosAPagar> GetAllItens()
        {
            IQueryable<vwLancamentosAPagar> query = Db.vwLancamentosAPagar;
            return query.ToList();
        }

        public List<vwLancamentosAPagar> ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String centroCusto, String beneficiario)
        {
            List<vwLancamentosAPagar> lista = new List<vwLancamentosAPagar>();
            IQueryable<vwLancamentosAPagar> query = Db.vwLancamentosAPagar;
            if (!String.IsNullOrEmpty(centroLucro))
            {
                query = query.Where(p => p.Centro_de_Lucro == centroLucro);
            }
            if (!String.IsNullOrEmpty(beneficiario))
            {
                query = query.Where(p => p.Beneficiario == beneficiario);
            }
            if (!String.IsNullOrEmpty(centroCusto))
            {
                query = query.Where(p => p.Centro_de_Custos == centroCusto);
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
            if (query != null)
            {
                query = query.OrderByDescending(a => a.Data_de_Vencimento);
                lista = query.ToList<vwLancamentosAPagar>();
            }
            return lista;
        }

    }
}
 