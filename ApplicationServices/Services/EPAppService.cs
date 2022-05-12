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
    public class EPAppService : AppServiceBase<vwExecutandoPositivo>, IEPAppService
    {
        private readonly IEPService _baseService;

        public EPAppService(IEPService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public List<vwExecutandoPositivo> GetAllItens()
        {
            return _baseService.GetAllItens();
        }
    }
}
