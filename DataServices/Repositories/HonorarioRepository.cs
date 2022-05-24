using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using EntitiesServices.Work_Classes;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class HonorarioRepository : RepositoryBase<HONORARIO>, IHonorarioRepository
    {
        public HONORARIO CheckExist(HONORARIO conta, Int32 idAss)
        {
            IQueryable<HONORARIO> query = Db.HONORARIO;
            query = query.Where(p => p.HONO_NM_NOME == conta.HONO_NM_NOME || p.HONO_NM_RAZAO_SOCIAL == conta.HONO_NM_RAZAO_SOCIAL);
            return query.FirstOrDefault();
        }

        public HONORARIO GetItemById(Int32 id)
        {
            IQueryable<HONORARIO> query = Db.HONORARIO;
            query = query.Where(p => p.HONO_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<HONORARIO> GetAllItens(Int32 idAss)
        {
            IQueryable<HONORARIO> query = Db.HONORARIO.Where(p => p.HONO_IN_ATIVO == 1);
            return query.ToList();
        }

        public List<HONORARIO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<HONORARIO> query = Db.HONORARIO;
            return query.ToList();
        }

        public List<HONORARIO> ExecuteFilter(Int32? tipo, String cpf, String cnpj, String razao, String nome)
        {
            List<HONORARIO> lista = new List<HONORARIO>();
            IQueryable<HONORARIO> query = Db.HONORARIO;
            if (tipo != null)
            {
                query = query.Where(p => p.TIPE_CD_ID == tipo);
            }
            if (cpf != null)
            {
                query = query.Where(p => p.HONO_NR_CPF.Contains(cpf));
            }
            if (cnpj != null)
            {
                query = query.Where(p => p.HONO_NR_CNPJ.Contains(cnpj));
            }
            if (!String.IsNullOrEmpty(razao))
            {
                query = query.Where(p => p.HONO_NM_RAZAO_SOCIAL.Contains(razao));
            }
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.HONO_NM_NOME.Contains(nome));
            }
            if (query != null)
            {
                lista = query.ToList<HONORARIO>();
            }
            return lista;
        }

    }
}
