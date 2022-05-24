using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.Repositories
{
    public interface IHonorarioRepository : IRepositoryBase<HONORARIO>
    {
        HONORARIO CheckExist(HONORARIO item, Int32 idAss);
        HONORARIO GetItemById(Int32 id);
        List<HONORARIO> GetAllItens(Int32 idAss);
        List<HONORARIO> GetAllItensAdm(Int32 idAss);
        List<HONORARIO> ExecuteFilter(Int32? tipo, String cpf, String cnpj, String razao, String nome);
    }
}
