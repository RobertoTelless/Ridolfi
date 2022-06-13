using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IClienteAppService : IAppServiceBase<CLIENTE>
    {
        Int32 ValidateCreate(CLIENTE perfil, USUARIO usuario);
        Int32 ValidateEdit(CLIENTE perfil, CLIENTE perfilAntes, USUARIO usuario);
        Int32 ValidateEdit(CLIENTE item, CLIENTE itemAntes);
        Int32 ValidateDelete(CLIENTE perfil, USUARIO usuario);
        Int32 ValidateReativar(CLIENTE perfil, USUARIO usuario);

        List<CLIENTE> GetAllItens();
        List<CLIENTE> GetAllItensAdm();
        CLIENTE GetItemById(Int32 id);
        CLIENTE CheckExist(CLIENTE conta);
        Int32 ExecuteFilter(Int32? catId, Int32? precatorio, Int32? trf, Int32? vara, Int32? titularidade, String npme, String oficio, String processo, out List<CLIENTE> objeto);
        CLIENTE_ANEXO GetAnexoById(Int32 id);
        CLIENTE_ANOTACAO GetAnotacaoById(Int32 id);

        List<CATEGORIA_CLIENTE> GetAllTipos();
        List<PRECATORIO> GetAllPrecatorios();
        List<TRF> GetAllTRF();
        List<VARA> GetAllVara();
        List<TITULARIDADE> GetAllTitularidade();

        CLIENTE_CONTATO GetContatoById(Int32 id);
        Int32 ValidateEditContato(CLIENTE_CONTATO item);
        Int32 ValidateCreateContato(CLIENTE_CONTATO item);
    }
}
