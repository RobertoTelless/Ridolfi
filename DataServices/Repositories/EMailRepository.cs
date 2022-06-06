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
    public class EMailRepository : RepositoryBase<EMAIL>, IEMailRepository
    {
        public List<EMAIL> GetAllItens()
        {
            return Db.EMAIL.ToList();
        }

        public EMAIL GetItemById(Int32 id)
        {
            IQueryable<EMAIL> query = Db.EMAIL.Where(p => p.EMAI_CD_ID == id);
            return query.FirstOrDefault();
        }
    }
}
 