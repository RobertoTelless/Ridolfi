using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ICRAppService : IAppServiceBase<vwContasAReceber>
    {
        List<vwContasAReceber> GetAllItens();
        List<vwContasAReceber> GetByData(DateTime data);
        Int32 ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, DateTime? recInicio, DateTime? recFinal, String centroLucro, String sacado, Int32? prob, out List<vwContasAReceber> objeto);
    }
}
