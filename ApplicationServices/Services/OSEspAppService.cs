using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Text.RegularExpressions;
using EntitiesServices.DTO;

namespace ApplicationServices.Services
{
    public class OSEspAppService : AppServiceBase<vwOrdemServicoEspecialidade>, IOSEspAppService
    {
        private readonly IOSEspService _baseService;

        public OSEspAppService(IOSEspService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public List<vwOrdemServicoEspecialidade> GetAllItens()
        {
            return _baseService.GetAllItens();
        }

        public List<DTO_OS_UF> GetItensOSUF()
        {
            return _baseService.GetItensOSUF();
        }
        public List<DTO_OS_UF> GetItensOSTipo()
        {
            return _baseService.GetItensOSTipo();
        }
        public List<DTO_OS_UF> GetItensOSCidade()
        {
            return _baseService.GetItensOSCidade();
        }
        public List<DTO_OS_UF> GetOSAtrasoCidade()
        {
            return _baseService.GetOSAtrasoCidade();
        }
    }
}
