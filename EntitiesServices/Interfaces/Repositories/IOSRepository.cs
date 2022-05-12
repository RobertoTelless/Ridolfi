using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.DTO;

namespace ModelServices.Interfaces.Repositories
{
    public interface IOSRepository : IRepositoryBase<OrdemServico>
    {
        List<OrdemServico> GetAllItens();
        List<OrdemServico> GetOSAtraso();
    }
}
