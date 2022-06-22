using Common;
using DataLayer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace LogicLayer
{
    public class DirectoryBL : BaseBL
    {
        private MyContext context;
        private MyConfig config;
        public DirectoryBL(MyContext context, MyConfig config)
        {
            this.context = context;
            this.config = config;
        }
        public async Task<ResponseBE> List(DirectoryBE model)
        {
            return await GetResponse(model, MyRole.Client, async (response) =>
            {
                var hotelCode =
                await (from r in context.Reservation
                       where r.Id == model.TokenBE.Id
                       select r.HotelCode).FirstOrDefaultAsync();               

                var head =
                await (from h in context.DirectoryHead
                       where h.HotelCode == hotelCode
                       select new
                       {
                           BannerUrl = h.BannerUrl,
                           Introduction = h.Introduction,
                       }).FirstOrDefaultAsync();

                var detail =
                await (from d in context.DirectoryDetail
                       where d.HotelCode == hotelCode
                       && d.Active
                       orderby d.OrderNo
                       select new
                       {
                           Name = d.Name,
                           BannerUrl = d.BannerUrl,
                           IconUrl = d.IconUrl,
                           Content = d.Content,
                       }).ToListAsync();

                response.data = new { head, detail };
            });
        }
        public async Task<ResponseBE> ListAdmin(DirectoryBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                var head =
                await (from h in context.DirectoryHead
                       where h.HotelCode == model.HotelCode
                       select new
                       {
                           BannerUrl = h.BannerUrl,
                           Introduction = h.Introduction,
                       }).FirstOrDefaultAsync();

                var detail =
                await (from d in context.DirectoryDetail
                       where d.HotelCode == model.HotelCode
                       && d.Active
                       orderby d.OrderNo
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           OrderNo = d.OrderNo,
                           BannerUrl = d.BannerUrl,
                           IconUrl = d.IconUrl,
                           Content = d.Content,
                           Active = d.Active,
                       }).ToListAsync();

                response.data = new { head, detail };
            });
        }
        public async Task<ResponseBE> UpsertHead(DirectoryBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");                

                var upsert =
                await (from h in context.DirectoryHead
                       where h.HotelCode == model.HotelCode                      
                       select h).FirstOrDefaultAsync();

                bool insert = upsert == null;
                if (insert)
                {
                    upsert = new DirectoryHead();
                    upsert.HotelCode = model.HotelCode;                    
                }

                if (!string.IsNullOrWhiteSpace(model.Image64))
                    upsert.BannerUrl = await SaveImage(model.HotelCode, model.Filename, model.Image64);
                upsert.Introduction = model.Introduction;
                
                if (insert)              
                    context.DirectoryHead.Add(upsert);              
                
                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<ResponseBE> Create(DirectoryBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");
                if (string.IsNullOrWhiteSpace(model.Name))
                    throw new Exception("Debe ingresar el nombre del servicio");
                if (string.IsNullOrWhiteSpace(model.Content))
                    throw new Exception("Debe ingresar una descripción");
                if (string.IsNullOrWhiteSpace(model.Filename))
                    throw new Exception("Debe adjuntar una imagen para el ícono");
                if (string.IsNullOrWhiteSpace(model.Image64))
                    throw new Exception("Debe adjuntar una imagen para el ícono(*)");
                if (string.IsNullOrWhiteSpace(model.Filename))
                    throw new Exception("Debe adjuntar una imagen para el banner");
                if (string.IsNullOrWhiteSpace(model.Image64))
                    throw new Exception("Debe adjuntar una imagen para el banner(*)");

                var insert = new DirectoryDetail();
                insert.HotelCode = model.HotelCode;
                insert.Name = model.Name;
                insert.Active = model.Active;
                insert.Content = model.Content;
                insert.IconUrl = await SaveImage(model.HotelCode, model.Iconname, model.Icon64);
                insert.BannerUrl = await SaveImage(model.HotelCode, model.Filename, model.Image64);
                insert.OrderNo = model.OrderNo;

                context.DirectoryDetail.Add(insert);
                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<ResponseBE> Edit(DirectoryBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (model.Id <= 0)
                    throw new Exception($"No se encontró el registro con id {model.Id}");
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");
                if (string.IsNullOrWhiteSpace(model.Name))
                    throw new Exception("Debe ingresar el nombre del servicio");
                if (string.IsNullOrWhiteSpace(model.Content))
                    throw new Exception("Debe ingresar una descripción");              

                var update =
                await (from h in context.DirectoryDetail
                       where h.Id == model.Id
                       select h).FirstOrDefaultAsync();

                if (update == null)
                    throw new Exception($"No se encontró el registro con id {model.Id}");

                if (!string.IsNullOrWhiteSpace(model.Image64))
                    update.BannerUrl = await SaveImage(model.HotelCode, model.Filename, model.Image64);
                if (!string.IsNullOrWhiteSpace(model.Icon64))
                    update.IconUrl = await SaveImage(model.HotelCode, model.Iconname, model.Icon64);

                update.Active = model.Active;
                update.Content = model.Content;
                update.Name = model.Name;
                update.OrderNo = model.OrderNo;

                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<string> SaveImage(string hotelCode, string filename, string image64)
        {
            try
            {
                DateTime now = DateTime.Now;
                string directory = string.Join("\\", config.SavePath, hotelCode, "DirectoryImages");
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);

                byte[] array = Convert.FromBase64String(image64);
                await System.IO.File.WriteAllBytesAsync(string.Join("\\", directory, filename), array);

                return string.Join("/", config.EndPoint, hotelCode, "DirectoryImages", filename);
            }
            catch (Exception)
            {
                throw new MyException("Error al guardar la imagen.");
            }
        }
    }
}
