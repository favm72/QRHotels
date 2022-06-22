using Common;
using DataLayer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace LogicLayer
{
    public class InfoPageBL : BaseBL
    {
        private MyContext context;
        private MyConfig config;
        public InfoPageBL(MyContext context, MyConfig config)
        {
            this.context = context;
            this.config = config;
        }       
        public async Task<ResponseBE> List(InfoPageBE model)
        {
            return await GetResponse(model, MyRole.Both, async (response) =>
            {
                var list = await (from p in context.InfoPage
                                  where p.HotelCode == model.HotelCode
                                  orderby p.Name
                                  select new
                                  {
                                      p.Id,
                                      p.Name,                                      
                                  }).ToListAsync();

                response.data = list;
            });
        }
        public async Task<ResponseBE> Create(InfoPageBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");
                if (string.IsNullOrWhiteSpace(model.Name))
                    throw new Exception("Debe ingresar un nombre");
                model.Name = model.Name.Trim();

                var existe =
                await (from p in context.InfoPage
                       where p.HotelCode == model.HotelCode
                       && p.Name == model.Name
                       select 1).AnyAsync();

                if (existe)
                    throw new Exception($"Ya existe un registro con el nombre \"{model.Name}\"");

                var info = new InfoPage();
                info.HotelCode = model.HotelCode;
                info.Name = model.Name;

                context.InfoPage.Add(info);
                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<ResponseBE> Edit(InfoPageBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (model.Id <= 0)
                    throw new Exception($"No se encontró el registro con id {model.Id}");               
                if (string.IsNullOrWhiteSpace(model.Name))
                    throw new Exception("Debe ingresar un nombre");
                model.Name = model.Name.Trim();

                var info =
                await (from p in context.InfoPage
                       where p.Id == model.Id
                       select p).FirstOrDefaultAsync();

                if (info == null)
                    throw new Exception($"No se encontró el registro con id {model.Id}");

                info.Name = model.Name;

                await context.SaveChangesAsync();

                response.data = true;
            });
        }       
    }
}
