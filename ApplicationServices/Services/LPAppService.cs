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
    public class LPAppService : AppServiceBase<vwLancamentosAPagar>, ILPAppService
    {
        private readonly ILPService _baseService;

        public LPAppService(ILPService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public List<vwLancamentosAPagar> GetAllItens()
        {
            return _baseService.GetAllItens();
        }

        public Int32 ExecuteFilter(DateTime? emissaoInicio, DateTime? emissaoFinal, DateTime? vencInicio, DateTime? vencFinal, String centroLucro, String centroCusto, String beneficiario, out List<vwLancamentosAPagar> objeto)
        {
            try
            {
                objeto = new List<vwLancamentosAPagar>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(emissaoInicio, emissaoFinal, vencInicio, vencFinal, centroLucro, centroCusto, beneficiario);
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
