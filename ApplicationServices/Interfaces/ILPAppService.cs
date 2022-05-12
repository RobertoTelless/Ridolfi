using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ILPAppService : IAppServiceBase<vwLancamentosAPagar>
    {
        List<vwLancamentosAPagar> GetAllItens();
        Int32 ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String centroCusto, String beneficiario, out List<vwLancamentosAPagar> objeto);
    }
}
