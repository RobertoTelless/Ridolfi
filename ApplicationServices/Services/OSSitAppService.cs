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

namespace ApplicationServices.Services
{
    public class OSSitAppService : AppServiceBase<vwOrdemServicoSituacao>, IOSSitAppService
    {
        private readonly IOSSitService _baseService;

        public OSSitAppService(IOSSitService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public List<vwOrdemServicoSituacao> GetAllItens()
        {
            return _baseService.GetAllItens();
        }
    }
}
