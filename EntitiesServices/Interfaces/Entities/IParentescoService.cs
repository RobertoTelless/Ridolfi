using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IParentescoService : IServiceBase<PARENTESCO>
    {
        Int32 Create(PARENTESCO perfil, LOG log);
        Int32 Create(PARENTESCO perfil);
        Int32 Edit(PARENTESCO perfil, LOG log);
        Int32 Edit(PARENTESCO perfil);
        Int32 Delete(PARENTESCO perfil, LOG log);

        PARENTESCO CheckExist(PARENTESCO item, Int32 idAss);
        PARENTESCO GetItemById(Int32 id);
        List<PARENTESCO> GetAllItens(Int32 idAss);
        List<PARENTESCO> GetAllItensAdm(Int32 idAss);
    }
}
