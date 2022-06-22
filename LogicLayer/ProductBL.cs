using Common;
using SoapClient;
using DataLayer;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LogicLayer
{
	public class ProductBL : BaseBL
	{
		private MyContext context { get; set; }
		private string FileServerPath { get; set; }
		private string FileEndpoint { get; set; }
		public ProductBL(MyContext context, string FileServerPath, string FileEndpoint)
		{
			this.context = context;
			this.FileServerPath = FileServerPath;
			this.FileEndpoint = FileEndpoint;
		}
		public async Task<ResponseBE> List(ProductBE productRequestBE)
		{
			return await GetResponse(productRequestBE, MyRole.Client, async (response) =>
			{
				var reservation = await (from r in context.Reservation
										 where r.Active && r.Id == productRequestBE.TokenBE.Id
										 select r).FirstOrDefaultAsync();

				var list = (from c in context.Category
							join t in context.ProductType on c.Id equals t.IdCategory
							join p in context.Product on t.Id equals p.IdProductType
							where p.IdProvider == productRequestBE.IdProvider 
							&& p.Active
							&& p.HotelCode == reservation.HotelCode
							orderby c.OrderNo, t.OrderNo
							select new { c, p, t }).ToList().GroupBy(x => x.c).Select(x => new
							{
								Name = x.Key.Name,
								Image = x.Key.Image,
								Id = x.Key.Id,
								Products = x.Select(y => new
								{
									Name = y.t.Name,
									Image = y.t.Image,
									Id = y.p.Id,
									Price = y.t.Price ?? 0,
									Description = y.t.Description,
									Modifiers = y.t.Modifiers
								}).ToList()
							});

				response.data = list;
			});
		}

		async public Task<ResponseBE> Manage(DummyBE dummyBE)
		{
			return await GetResponse(dummyBE, MyRole.Admin, async (response) =>
			{
				var source = await (from c in context.Category
									join t in context.ProductType on c.Id equals t.IdCategory
									join p in context.Product on t.Id equals p.IdProductType
									join v in context.Provider on p.IdProvider equals v.Id
									join m in context.Permission on p.HotelCode equals m.HotelCode
									where m.IdUser == dummyBE.TokenBE.Id
									orderby c.OrderNo, t.OrderNo
									select new
									{
										Provider = v.Name,
										Category = c.Name,
										Product = t.Name,
										Id = p.Id,
										Active = p.Active
									}).ToListAsync();

				var list = source.GroupBy(x => x.Provider).Select(x => new
				{
					Provider = x.Key,
					Categories = x.GroupBy(y => y.Category).Select(y => new
					{
						Category = y.Key,
						Products = y.Select(z => new
						{
							Product = z.Product,
							Id = z.Id,
							Active = z.Active
						}).ToList()
					}).ToList()
				}).ToList();

				response.data = list;
			});
		}
		async public Task<ResponseBE> Categories(ProductBE productBE)
		{
			return await GetResponse(productBE, MyRole.Admin, async (response) =>
			{
				var source = await (from c in context.Category
									join t in context.ProductType on c.Id equals t.IdCategory
									join p in context.Product on t.Id equals p.IdProductType
									orderby c.OrderNo
									where p.HotelCode == productBE.HotelCode
									&& p.IdProvider == productBE.IdProvider
									select new
									{
										Description = c.Name,
										Id = c.Id
									}).Distinct().ToListAsync();
				response.data = source;
			});
		}
		async public Task<ResponseBE> CategoriesAll(ProductBE productBE)
		{
			return await GetResponse(productBE, MyRole.Admin, async (response) =>
			{
				var source = await (from c in context.Category
									orderby c.OrderNo
									select new
									{
										Description = c.Name,
										Id = c.Id
									}).ToListAsync();
				response.data = source;
			});
		}
		async public Task<ResponseBE> Search(ProductBE filterBE)
		{
			return await GetResponse(filterBE, MyRole.Admin, async (response) =>
			{
				var source = await (from t in context.ProductType
									join p in context.Product on t.Id equals p.IdProductType
									where
									p.HotelCode == filterBE.HotelCode
									&& t.IdCategory == filterBE.IdCategory
									&& p.IdProvider == filterBE.IdProvider
									orderby t.Name
									select new
									{
										OrderNo = t.OrderNo ?? 0,
										Id = p.Id,
										Active = p.Active,
										Name = t.Name,
										Price = t.Price ?? 0
									}).ToListAsync();

				response.data = source;
			});
		}
		async public Task<ResponseBE> FindById(ProductBE filterBE)
		{
			return await GetResponse(filterBE, MyRole.Admin, async (response) =>
			{
				var source = await (from t in context.ProductType
									join p in context.Product on t.Id equals p.IdProductType
									where p.Id == filterBE.Id
									orderby t.OrderNo
									select new
									{
										Id = p.Id,
										IdCategory = t.IdCategory,
										Name = t.Name,
										Description = t.Description,
										Active = p.Active,
										Price = t.Price ?? 0,
										Image = "",
										ImageLink = t.Image,
										Modifiers = t.Modifiers,
										OrderNo = t.OrderNo ?? 0
									}).FirstOrDefaultAsync();

				response.data = source;
			});
		}
		async public Task<ResponseBE> Update(ProductBE productBE)
		{
			return await GetResponse(productBE, MyRole.Admin, async (response) =>
			{
				var productUpdate = await context.Product.FirstOrDefaultAsync(x => x.Id == productBE.Id);
				var typeUpdate = await context.ProductType.FirstOrDefaultAsync(x => x.Id == productUpdate.IdProductType);

				if (productBE.newCategory)
				{
					int? maxOrder = await context.Category.MaxAsync(x => x.OrderNo);
					var addCategory = new Category();
					addCategory.Name = productBE.categoryName;
					addCategory.OrderNo = (maxOrder ?? 0) + 1;
					addCategory.Created = DateTime.Now;
					await context.Category.AddAsync(addCategory);
					await context.SaveChangesAsync();

					productBE.IdCategory = addCategory.Id;
				}

				productUpdate.Active = productBE.Active;
				typeUpdate.Name = productBE.Name;
				typeUpdate.Description = productBE.Description;
				if (productBE.Image?.Length > 0 && productBE.FileName?.Length > 0)
				{
					typeUpdate.Image = await SavePhoto(productBE);
				}				
				typeUpdate.Modifiers = productBE.Modifiers;
				typeUpdate.OrderNo = productBE.OrderNo;
				typeUpdate.Price = productBE.Price;
				typeUpdate.IdCategory = productBE.IdCategory;

				await context.SaveChangesAsync();
			});
		}

		async public Task<ResponseBE> Create(ProductBE productBE)
		{
			return await GetResponseTransaction(productBE, MyRole.Admin, context, async (response) =>
			{
				if (productBE.newCategory)
				{
					int? maxOrder = await context.Category.MaxAsync(x => x.OrderNo);
					var addCategory = new Category();
					addCategory.Name = productBE.categoryName;
					addCategory.OrderNo = (maxOrder ?? 0) + 1;
					addCategory.Created = DateTime.Now;
					await context.Category.AddAsync(addCategory);
					await context.SaveChangesAsync();

					productBE.IdCategory = addCategory.Id;
				}
				var addType = new ProductType();
				addType.Active = true;
				addType.Created = DateTime.Now;
				addType.Description = productBE.Description;
				addType.IdCategory = productBE.IdCategory;
				if (productBE.Image?.Length > 0 && productBE.FileName?.Length > 0)
				{
					addType.Image = await SavePhoto(productBE);
				}
				addType.Name = productBE.Name;
				addType.OrderNo = productBE.OrderNo;
				addType.Modifiers = productBE.Modifiers;
				addType.Price = productBE.Price;

				await context.ProductType.AddAsync(addType);
				await context.SaveChangesAsync();

				var addProduct = new Product();
				addProduct.Active = productBE.Active;
				addProduct.Created = DateTime.Now;
				addProduct.HotelCode = productBE.HotelCode;
				addProduct.IdProductType = addType.Id;
				addProduct.IdProvider = productBE.IdProvider;

				await context.Product.AddAsync(addProduct);
				await context.SaveChangesAsync();
			});
		}

		async public Task<ResponseBE> Delete(ProductBE productBE)
		{
			return await GetResponseTransaction(productBE, MyRole.Admin, context, async (response) =>
			{
				var productRemove = await context.Product.FirstOrDefaultAsync(x => x.Id == productBE.Id);
				var typeRemove = await context.ProductType.FirstOrDefaultAsync(x => x.Id == productRemove.IdProductType);

				context.Product.Remove(productRemove);
				context.ProductType.Remove(typeRemove);
				await context.SaveChangesAsync();
			});
		}
		async public Task<ResponseBE> Toggle(ProductBE productRequestBE)
		{
			return await GetResponse(productRequestBE, MyRole.Admin, async (response) =>
			{
				var product = await (from p in context.Product
									 where p.Id == productRequestBE.Id
									 select p).FirstOrDefaultAsync();

				if (product == null)
					throw new MyException($"No se encontró el producto con id = {productRequestBE.Id}");

				product.Active = !product.Active;

				await context.SaveChangesAsync();
			});
		}

		public async Task<string> SavePhoto(ProductBE productBE)
		{
			try
			{
				string providerName = await (from p in context.Provider
									   where p.Id == productBE.IdProvider
									   select p.Name).FirstOrDefaultAsync();

				Regex rgx = new Regex("[^a-zA-Z0-9]");
				providerName = rgx.Replace(providerName, "");

				DateTime now = DateTime.Now;
				string directory = $"{FileServerPath}\\{productBE.HotelCode}\\{providerName}";
				if (!System.IO.Directory.Exists(directory))
					System.IO.Directory.CreateDirectory(directory);

				Regex rgx2 = new Regex("[^a-zA-Z0-9\\.]");
				productBE.FileName = $"{productBE.Id}-{rgx2.Replace(productBE.FileName, "")}";

				byte[] array = Convert.FromBase64String(productBE.Image);
				System.IO.File.WriteAllBytes($"{directory}\\{productBE.FileName}", array);

				return $"{FileEndpoint}/{productBE.HotelCode}/{providerName}/{productBE.FileName}";
			}
			catch (Exception)
			{
				throw new MyException("Error al guardar la imagen.");
			}
		}

		public async Task<ResponseBE> CartEnabled(ProductBE productBE)
		{
			return await GetResponse(productBE, MyRole.Client, async (response) =>
			{
				var reservation = await (from r in context.Reservation
										 where r.Active && r.Id == productBE.TokenBE.Id
										 select r).FirstOrDefaultAsync();
				
				var config = await (from c in context.CartConfig
								  where c.HotelCode == reservation.HotelCode
								  && c.IdProvider == productBE.IdProvider
								  select c).FirstOrDefaultAsync();

				response.data = true;
				if (config != null)
				{
					if (!config.Active)
					{
						response.data = false;
						response.message = "Servicio Inactivo";
					}
					else
					{
						try
						{
							int start = int.Parse(Regex.Replace(config.HourStart, "[^0-9]", ""));
							int end = int.Parse(Regex.Replace(config.HourEnd, "[^0-9]", ""));
							int now = int.Parse(Regex.Replace(DateTime.Now.ToString("HH:mm"), "[^0-9]", ""));
							if (start > now || now > end)
							{
								response.data = false;
								response.message = $"La atención es desde {config.HourStart} hasta {config.HourEnd}";
							}
						}
						catch (Exception)
						{
							response.data = false;
							response.message = "Error en el horario de atención.";
						}
					}
				}
			});
		}		
	}
}
