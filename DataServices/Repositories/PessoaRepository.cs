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
    public class PessoaRepository : RepositoryBase<Pessoa>
    {
        public List<Pessoa> GetAllItens()
        {
            IQueryable<Pessoa> query = Db.Pessoa;
            return query.ToList();
        }
    }
}
 