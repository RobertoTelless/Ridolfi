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

namespace DataServices.Repositories
{
    public class ContatoRepository : RepositoryBase<CONTATO>, IContatoRepository
    {
        public List<CONTATO> GetAllItens()
        {
            return Db.CONTATO.ToList();
        }

        public CONTATO GetItemById(Int32 id)
        {
            IQueryable<CONTATO> query = Db.CONTATO.Where(p => p.CONT_CD_ID == id);
            return query.FirstOrDefault();
        }
    }
}
 