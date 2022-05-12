using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IPCService : IServiceBase<vwParcelamento>
    {
        List<vwParcelamento> GetAllItens();
        List<vwParcelamento> ExecuteFilter(DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String sacado, Int32? prob);
    }
}
