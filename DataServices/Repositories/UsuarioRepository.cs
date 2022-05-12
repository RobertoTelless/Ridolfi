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
    public class UsuarioRepository : RepositoryBase<USUARIO_SUGESTAO>, IUsuarioRepository
    {
        public List<USUARIO_SUGESTAO> GetAllItens()
        {
            IQueryable<USUARIO_SUGESTAO> query = Db.USUARIO_SUGESTAO;
            query = query.Where(p => p.USUA_IN_ATIVO == 1);
            return query.ToList();
        }
    }
}
 