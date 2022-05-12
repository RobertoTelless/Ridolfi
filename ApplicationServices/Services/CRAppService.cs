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
    public class CRAppService : AppServiceBase<vwContasAReceber>, ICRAppService
    {
        private readonly ICRService _baseService;

        public CRAppService(ICRService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public List<vwContasAReceber> GetAllItens()
        {
            return _baseService.GetAllItens();
        }

        public List<vwContasAReceber> GetByData(DateTime data)
        {
            return _baseService.GetByData(data);
        }

        public Int32 ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, DateTime? recInicio, DateTime? recFinal, String centroLucro, String sacado, Int32? prob, out List<vwContasAReceber> objeto)
        {
            try
            {
                objeto = new List<vwContasAReceber>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(emissaoInicio, emissaoFinal, vencInicio, vencFinal, recInicio, recFinal, centroLucro, sacado, prob);
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
