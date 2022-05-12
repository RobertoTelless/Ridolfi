using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IPCRepository : IRepositoryBase<vwParcelamento>
    {
        List<vwParcelamento> GetAllItens();
        List<vwParcelamento> ExecuteFilter(DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String sacado, Int32? prob);

    }
}
