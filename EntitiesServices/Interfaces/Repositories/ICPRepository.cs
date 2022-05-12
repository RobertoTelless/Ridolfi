using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface ICPRepository : IRepositoryBase<vwContasAPagar>
    {
        List<vwContasAPagar> GetAllItens();
        List<vwContasAPagar> GetByData(DateTime data);
        List<vwContasAPagar> ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, DateTime? pagInicio, DateTime? pagFinal, String centroCusto, String beneficiario, String libPag, Int32? crit);
    }
}
