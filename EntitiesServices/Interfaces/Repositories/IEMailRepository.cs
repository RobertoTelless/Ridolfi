using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IEMailRepository : IRepositoryBase<EMAIL>
    {
        List<EMAIL> GetAllItens();
        EMAIL GetItemById(Int32 id);
    }
}
