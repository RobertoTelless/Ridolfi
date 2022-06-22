using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using ModelServices.Interfaces.Repositories;
using EntitiesServices.Work_Classes;
using System.Data.Entity;
using CrossCutting;

namespace DataServices.Repositories
{
    public class ClienteRepository : RepositoryBase<CLIENTE>, IClienteRepository
    {
        public CLIENTE CheckExist(CLIENTE conta)
        {
            IQueryable<CLIENTE> query = Db.CLIENTE;
            query = query.Where(p => p.CLIE_NM_NOME == conta.CLIE_NM_NOME);
            return query.FirstOrDefault();
        }

        public CLIENTE GetItemById(Int32 id)
        {
            IQueryable<CLIENTE> query = Db.CLIENTE;
            query = query.Where(p => p.CLIE_CD_ID == id);
            query = query.Include(p => p.CLIENTE_ANEXO);
            query = query.Include(p => p.CLIENTE_CONTATO);
            return query.FirstOrDefault();
        }

        public List<CLIENTE> GetAllItens()
        {
            IQueryable<CLIENTE> query = Db.CLIENTE.Where(p => p.CLIE_IN_ATIVO == 1);
            query = query.OrderBy(a => a.CLIE_NM_NOME);
            return query.ToList();
        }

        public List<CLIENTE> GetAllItensAdm()
        {
            IQueryable<CLIENTE> query = Db.CLIENTE;
            query = query.OrderBy(a => a.CLIE_NM_NOME);
            return query.ToList();
        }

        public List<CLIENTE> ExecuteFilter(Int32? catId, Int32? precatorio, Int32? trf, Int32? vara, Int32? titularidade, String nome, String oficio, String processo, DateTime? data)
        {
            List<CLIENTE> lista = new List<CLIENTE>();
            IQueryable<CLIENTE> query = Db.CLIENTE;
            if (catId > 0)
            {
                query = query.Where(p => p.CATEGORIA_CLIENTE.CACL_CD_ID == catId);
            }
            if (precatorio > 0)
            {
                query = query.Where(p => p.PREC_CD_ID == precatorio);
            }
            if (trf > 0)
            {
                query = query.Where(p => p.TRF1_CD_ID == trf);
            }
            if (vara > 0)
            {
                query = query.Where(p => p.VARA_CD_ID == vara);
            }
            if (titularidade > 0)
            {
                query = query.Where(p => p.TITU_CD_ID == titularidade);
            }
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.CLIE_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(oficio))
            {
                query = query.Where(p => p.CLIE_NR_OFICIO == oficio);
            }
            if (!String.IsNullOrEmpty(processo))
            {
                query = query.Where(p => p.CLIE_NR_PROCESSO == processo);
            }
            if (data != null)
            {
                query = query.Where(p => DbFunctions.TruncateTime(p.CLIE_DT_CADASTRO) == DbFunctions.TruncateTime(data));
            }
            if (query != null)
            {
                query = query.OrderBy(a => a.CLIE_NM_NOME);
                lista = query.ToList<CLIENTE>();
            }
            return lista;
        }
    }
}
 