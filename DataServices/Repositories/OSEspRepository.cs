using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.DTO;
using ModelServices.Interfaces.Repositories;
using EntitiesServices.Work_Classes;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class OSEspRepository : RepositoryBase<vwOrdemServicoEspecialidade>, IOSEspRepository
    {
        private readonly PessoaRepository _pesRepository;
        private readonly OrdemServicoRepository _osRepository;

        public OSEspRepository(PessoaRepository pesRepository, OrdemServicoRepository osRepository)
        {
            _pesRepository = pesRepository;
            _osRepository = osRepository;
        }

        public List<vwOrdemServicoEspecialidade> GetAllItens()
        {
            IQueryable<vwOrdemServicoEspecialidade> query = Db.vwOrdemServicoEspecialidade;
            return query.ToList();
        }

        public List<DTO_OS_UF> GetItensOSUF()
        {
            using (var ctx = new OnSuiteBIEntities())
            {
                var ss = from ri in ctx.OrdemServico
                         join rr in ctx.Pessoa
                            on ri.Cliente equals rr.Id
                         group ri by ri.Pessoa1.UF into g
                         select new DTO_OS_UF
                         {
                             UF = g.Key,
                             Quantidade = g.Count()
                         };
                var x = ss.First();
                var y = ss.Count();
                return ss.ToList<DTO_OS_UF>();
            }
        }

        public List<DTO_OS_UF> GetItensOSTipo()
        {
            using (var ctx = new OnSuiteBIEntities())
            {
                var ss = from ri in ctx.OrdemServico
                         group ri by ri.Tipo into g
                         select new DTO_OS_UF
                         {
                             UF = g.Key.ToString(),
                             Quantidade = g.Count()
                         };
                var x = ss.First();
                var y = ss.Count();
                List<DTO_OS_UF> final = ss.ToList();
                foreach (DTO_OS_UF item in final)
                {
                    if (item.UF == "0")
                    {
                        item.UF = "Corretiva";
                    }
                    else if (item.UF == "1")
                    {
                        item.UF = "Preventiva";
                    }
                    else if (item.UF == "2")
                    {
                        item.UF = "Emergencial";
                    }
                    else if (item.UF == "3")
                    {
                        item.UF = "Extra Contratual";
                    }
                    else if (item.UF == "4")
                    {
                        item.UF = "Garantia";
                    }
                    else if (item.UF == "5")
                    {
                        item.UF = "Obra";
                    }
                    else if (item.UF == "6")
                    {
                        item.UF = "Acompanhamento";
                    }
                    else if (item.UF == "7")
                    {
                        item.UF = "Emergencial SLA 03H";
                    }
                    else if (item.UF == "8")
                    {
                        item.UF = "Emergencial SLA 24H";
                    }
                    else if (item.UF == "9")
                    {
                        item.UF = "Essencial SLA 48H";
                    }
                    else if (item.UF == "10")
                    {
                        item.UF = "Corretiva de origem Preventiva";
                    }
                }
                return final;
            }
        }

        public List<DTO_OS_UF> GetItensOSCidade()
        {
            using (var ctx = new OnSuiteBIEntities())
            {
                var ss = from ri in ctx.OrdemServico
                         join rr in ctx.Pessoa
                            on ri.Cliente equals rr.Id
                         group ri by ri.Pessoa1.Cidade into g
                         select new DTO_OS_UF
                         {
                             UF = g.Key,
                             Quantidade = g.Count()
                         };
                var x = ss.First();
                var y = ss.Count();
                return ss.ToList<DTO_OS_UF>();
            }
        }

        public List<DTO_OS_UF> GetOSAtrasoCidade()
        {
            using (var ctx = new OnSuiteBIEntities())
            {
                var ss = from ri in ctx.OrdemServico
                         join rr in ctx.Pessoa
                            on ri.Cliente equals rr.Id
                         where ri.DataExecucaoFimPrevisao < DateTime.Today.Date
                         group ri by ri.Pessoa1.Cidade into g
                         select new DTO_OS_UF
                         {
                             UF = g.Key,
                             Quantidade = g.Count()
                         };
                var x = ss.First();
                var y = ss.Count();
                return ss.ToList<DTO_OS_UF>();
            }
        }

    }
}
 