using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ILPService : IServiceBase<vwLancamentosAPagar>
    {
        List<vwLancamentosAPagar> GetAllItens();
        List<vwLancamentosAPagar> ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String centroCusto, String beneficiario);
    }
}
