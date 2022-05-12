using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.DTO;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IOSEspService : IServiceBase<vwOrdemServicoEspecialidade>
    {
        List<vwOrdemServicoEspecialidade> GetAllItens();
        List<DTO_OS_UF> GetItensOSUF();
        List<DTO_OS_UF> GetItensOSTipo();
        List<DTO_OS_UF> GetItensOSCidade();
        List<DTO_OS_UF> GetOSAtrasoCidade();

    }
}
