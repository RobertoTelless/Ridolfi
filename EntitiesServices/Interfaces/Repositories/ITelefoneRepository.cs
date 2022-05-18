using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface ITelefoneRepository : IRepositoryBase<TELEFONES>
    {
        TELEFONES CheckExist(TELEFONES item, Int32 idAss);
        TELEFONES GetItemById(Int32 id);
        List<TELEFONES> GetAllItens(Int32 idAss);
        List<TELEFONES> GetAllItensAdm(Int32 idAss);
        List<TELEFONES> ExecuteFilter(Int32? catId, String nome, String telefone, String cidade, Int32? uf, String celular, String email, Int32 idAss);
    }
}
