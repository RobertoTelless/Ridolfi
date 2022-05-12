using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IPCAppService : IAppServiceBase<vwParcelamento>
    {
        List<vwParcelamento> GetAllItens();
        Int32 ExecuteFilter(DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String sacado, Int32? prob, out List<vwParcelamento> objeto);
    }
}
