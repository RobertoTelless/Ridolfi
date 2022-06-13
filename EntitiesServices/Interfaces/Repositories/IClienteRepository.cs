using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.Repositories
{
    public interface IClienteRepository : IRepositoryBase<CLIENTE>
    {
        CLIENTE CheckExist(CLIENTE item);
        CLIENTE GetItemById(Int32 id);
        List<CLIENTE> GetAllItens();
        List<CLIENTE> GetAllItensAdm();
        List<CLIENTE> ExecuteFilter(Int32? catId, Int32? precatorio, Int32? trf, Int32? vara, Int32? titularidade, String npme, String oficio, String processo);
    }
}
