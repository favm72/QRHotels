using Common;
using DataLayer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace LogicLayer
{
    public class HomeMenuBL : BaseBL
    {
        private MyContext context;
        private MyConfig config;
        public HomeMenuBL(MyContext context, MyConfig config)
        {
            this.context = context;
            this.config = config;
        }
        public async Task<ResponseBE> List(HomeMenuBE model)
        {
            return await GetResponse(model, MyRole.Client, async (response) =>
            {
                var hotelCode =
                await (from r in context.Reservation
                       where r.Id == model.TokenBE.Id
                       select r.HotelCode).FirstOrDefaultAsync();

                var list = await (from h in context.HomeMenu
                                  where h.HotelCode == hotelCode
                                  && h.Active
                                  orderby h.OrderNo
                                  select new
                                  {
                                      h.Id,
                                      h.Description,
                                      h.ImageUrl,
                                      h.IsHalf,
                                      h.LinkUrl,
                                      h.Link,
                                      h.IdInfoPage
                                  }).ToListAsync();

                response.data = list;
            });
        }
        public async Task<ResponseBE> ListAdmin(HomeMenuBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                var list = await (from h in context.HomeMenu
                                  where h.HotelCode == model.HotelCode
                                  orderby h.OrderNo
                                  select new
                                  {
                                      h.Id,
                                      h.Description,
                                      h.ImageUrl,
                                      h.IsHalf,
                                      h.LinkUrl,
                                      h.OrderNo,
                                      h.Active,
                                      h.Link,
                                      h.IdInfoPage
                                  }).ToListAsync();

                response.data = list;
            });
        }
        public async Task<ResponseBE> Create(HomeMenuBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");             
                if (string.IsNullOrWhiteSpace(model.LinkUrl))
                    throw new Exception("Debe seleccionar una funcionalidad");
                if (string.IsNullOrWhiteSpace(model.Description))
                    throw new Exception("Debe ingresar una descripción");
                if (model.LinkUrl != "slider")
                {
                    if (string.IsNullOrWhiteSpace(model.Filename))
                        throw new Exception("Debe adjuntar una imagen");
                    if (string.IsNullOrWhiteSpace(model.Image64))
                        throw new Exception("Debe adjuntar una imagen(*)");
                }

                if (model.LinkUrl == "link")
                {
                    if (string.IsNullOrWhiteSpace(model.Link))
                        throw new Exception("Debe ingresar un link");
                }

                if (model.LinkUrl == "informative")
                {
                    if (model.IdInfoPage == null || model.IdInfoPage <= 0)
                        throw new Exception("Debe ingresar seleccionar una página informativa");
                }

                var imageUrl = "*";
                if (model.LinkUrl != "slider")
                    imageUrl = await SaveImage(model);

                var menu = new HomeMenu();
                menu.HotelCode = model.HotelCode;
                menu.Active = model.Active;
                menu.Description = model.Description.Trim();
                if (!string.IsNullOrWhiteSpace(model.Title))
                    menu.Title = model.Title;
                menu.ImageUrl = imageUrl;
                menu.IsHalf = model.IsHalf;
                menu.LinkUrl = model.LinkUrl;
                menu.OrderNo = model.OrderNo;
                if (!string.IsNullOrWhiteSpace(model.Link))
                    menu.Link = model.Link.Trim();
                if (model.IdInfoPage != null && model.IdInfoPage > 0)
                    menu.IdInfoPage = model.IdInfoPage;

                context.HomeMenu.Add(menu);
                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<ResponseBE> Edit(HomeMenuBE model)
        {
            return await GetResponse(model, MyRole.Admin, async (response) =>
            {
                if (model.Id <= 0)
                    throw new Exception($"No se encontró el registro con id {model.Id}");
                if (string.IsNullOrWhiteSpace(model.HotelCode))
                    throw new Exception("Debe seleccionar un Hotel");                
                if (string.IsNullOrWhiteSpace(model.LinkUrl))
                    throw new Exception("Debe seleccionar una funcionalidad");
                if (string.IsNullOrWhiteSpace(model.Description))
                    throw new Exception("Debe ingresar una descripción");
                if (model.LinkUrl == "link")
                {
                    if (string.IsNullOrWhiteSpace(model.Link))
                        throw new Exception("Debe ingresar un link");
                }

                if (model.LinkUrl == "informative")
                {
                    if (model.IdInfoPage == null || model.IdInfoPage <= 0)
                        throw new Exception("Debe ingresar seleccionar una página informativa");
                }

                var menu =
                await (from h in context.HomeMenu
                       where h.Id == model.Id
                       select h).FirstOrDefaultAsync();

                if (menu == null)
                    throw new Exception($"No se encontró el registro con id {model.Id}");

                if (!string.IsNullOrWhiteSpace(model.Image64))
                    menu.ImageUrl = await SaveImage(model);

                menu.Active = model.Active;
                menu.Description = model.Description.Trim();
                if (!string.IsNullOrWhiteSpace(model.Title))
                    menu.Title = model.Title;
                menu.IsHalf = model.IsHalf;
                menu.OrderNo = model.OrderNo;
                if (!string.IsNullOrWhiteSpace(model.Link))
                    menu.Link = model.Link.Trim();
                if (model.IdInfoPage != null && model.IdInfoPage > 0)
                    menu.IdInfoPage = model.IdInfoPage;

                await context.SaveChangesAsync();

                response.data = true;
            });
        }
        public async Task<string> SaveImage(HomeMenuBE model)
        {
            try
            {
                DateTime now = DateTime.Now;
                string directory = string.Join("\\", config.SavePath, model.HotelCode, "HomeImages");
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);

                byte[] array = Convert.FromBase64String(model.Image64);
                await System.IO.File.WriteAllBytesAsync(string.Join("\\", directory, model.Filename), array);

                return string.Join("/", config.EndPoint, model.HotelCode, "HomeImages", model.Filename);
            }
            catch (Exception)
            {
                throw new MyException("Error al guardar la imagen.");
            }
        }
    }
}
