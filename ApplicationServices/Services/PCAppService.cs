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
    public class PCAppService : AppServiceBase<vwParcelamento>, IPCAppService
    {
        private readonly IPCService _baseService;

        public PCAppService(IPCService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public List<vwParcelamento> GetAllItens()
        {
            return _baseService.GetAllItens();
        }

        public Int32 ExecuteFilter(DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String sacado,Int32? prob, out List<vwParcelamento> objeto)
        {
            try
            {
                objeto = new List<vwParcelamento>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(vencInicio, vencFinal, centroLucro, sacado, prob);
                if (objeto.Count == 0)
                {
                    volta = 1;
                }
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
