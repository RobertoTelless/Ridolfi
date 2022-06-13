using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IClienteService : IServiceBase<CLIENTE>
    {
        Int32 Create(CLIENTE perfil, LOG log);
        Int32 Create(CLIENTE perfil);
        Int32 Edit(CLIENTE perfil, LOG log);
        Int32 Edit(CLIENTE perfil);
        Int32 Delete(CLIENTE perfil, LOG log);

        CLIENTE CheckExist(CLIENTE conta);
        CLIENTE GetItemById(Int32 id);
        List<CLIENTE> GetAllItens();
        List<CLIENTE> GetAllItensAdm();
        List<CLIENTE> ExecuteFilter(Int32? catId, Int32? precatorio, Int32? trf, Int32? vara, Int32? titularidade, String npme, String oficio, String processo);
        CLIENTE_ANEXO GetAnexoById(Int32 id);
        CLIENTE_ANOTACAO GetAnotacaoById(Int32 id);

        List<CATEGORIA_CLIENTE> GetAllTipos();
        List<PRECATORIO> GetAllPrecatorios();
        List<TRF> GetAllTRF();
        List<VARA> GetAllVara();
        List<TITULARIDADE> GetAllTitularidade();

        CLIENTE_CONTATO GetContatoById(Int32 id);
        Int32 EditContato(CLIENTE_CONTATO item);
        Int32 CreateContato(CLIENTE_CONTATO item);
    }
}
