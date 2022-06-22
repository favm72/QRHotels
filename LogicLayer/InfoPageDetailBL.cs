using Common;
using DataLayer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace LogicLayer
{
    public class InfoPageDetailBL : BaseBL
    {
        private MyContext context;
        private MyConfig config;
        public InfoPageDetailBL(MyContext context, MyConfig config)
        {
            this.context = context;
            this.config = config;
        }
        public async Task<ResponseBE> List(InfoPageDetailBE model)
        {
            return await GetResponse(model, MyRole.Client, async (response) =>
            {
                var list =
                await (from d in context.InfoPageDetail
                       where d.IdInfoPage == model.IdInfoPage
                       && d.Active
                       orderby d.OrderNo
                       select new
                       {
                           d.Id,
                           d.Description,
                           d.ImageUrl,
                           d.MapUrl,
                           d.Title,
                           d.LinkUrl
                       }).ToListAsync();

                response.data = list;
            });
        }
        public async Task<ResponseBE> ListAdmin(InfoPageDetailBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                var list = await (from p in context.InfoPageDetail
                                  where p.IdInfoPage == model.IdInfoPage
                                  orderby p.OrderNo
                                  select new
                                  {
                                      p.Id,
                                      p.Title,
                                      p.Description,
                                      p.ImageUrl,
                                      p.MapUrl,
                                      p.LinkUrl,
                                      p.OrderNo,
                                      p.Active
                                  }).ToListAsync();

                response.data = list;
            });
        }
        public async Task<ResponseBE> Create(InfoPageDetailBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");
                if (string.IsNullOrWhiteSpace(model.Title))
                    throw new Exception("Debe seleccionar un Título");                
                if (string.IsNullOrWhiteSpace(model.Filename))
                    throw new Exception("Debe adjuntar una imagen");
                if (string.IsNullOrWhiteSpace(model.Image64))
                    throw new Exception("Debe adjuntar una imagen(*)");            

                var imageUrl = await SaveImage(model);
               
                var detail = new InfoPageDetail();
                detail.IdInfoPage = model.IdInfoPage;
                detail.Active = model.Active;
                if (!string.IsNullOrWhiteSpace(model.Description))
                    detail.Description = model.Description.Trim();                
                if (!string.IsNullOrWhiteSpace(model.MapUrl))
                    detail.MapUrl = model.MapUrl.Trim();
                if (!string.IsNullOrWhiteSpace(model.LinkUrl))
                    detail.LinkUrl = model.LinkUrl.Trim();
                detail.ImageUrl = imageUrl;
                detail.OrderNo = model.OrderNo;
                detail.Title = model.Title;
                context.InfoPageDetail.Add(detail);
                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<ResponseBE> Edit(InfoPageDetailBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (model.Id <= 0)
                    throw new Exception($"No se encontró el registro con id {model.Id}");
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");
                if (string.IsNullOrWhiteSpace(model.Title))
                    throw new Exception("Debe seleccionar un Título");

                var detail =
                await (from d in context.InfoPageDetail
                       where d.Id == model.Id
                       select d).FirstOrDefaultAsync();

                if (detail == null)
                    throw new Exception($"No se encontró el registro con id {model.Id}");

                if (!string.IsNullOrWhiteSpace(model.Image64))
                    detail.ImageUrl = await SaveImage(model);

                detail.Active = model.Active;
                detail.IdInfoPage = model.IdInfoPage;
                if (!string.IsNullOrWhiteSpace(model.Description))
                    detail.Description = model.Description.Trim();
                if (!string.IsNullOrWhiteSpace(model.MapUrl))
                    detail.MapUrl = model.MapUrl.Trim();
                if (!string.IsNullOrWhiteSpace(model.LinkUrl))
                    detail.LinkUrl = model.LinkUrl.Trim();
                detail.OrderNo = model.OrderNo;
                detail.Title = model.Title;

                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<string> SaveImage(InfoPageDetailBE model)
        {
            try
            {
                DateTime now = DateTime.Now;
                string directory = string.Join("\\", config.SavePath, model.HotelCode, "InfoImages");
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);

                byte[] array = Convert.FromBase64String(model.Image64);
                await System.IO.File.WriteAllBytesAsync(string.Join("\\", directory, model.Filename), array);

                return string.Join("/", config.EndPoint, model.HotelCode, "InfoImages", model.Filename);
            }
            catch (Exception)
            {
                throw new MyException("Error al guardar la imagen.");
            }
        }
    }
}
