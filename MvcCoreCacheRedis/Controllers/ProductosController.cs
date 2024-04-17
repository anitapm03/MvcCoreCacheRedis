using Microsoft.AspNetCore.Mvc;
using MvcCoreCacheRedis.Models;
using MvcCoreCacheRedis.Repositories;
using MvcCoreCacheRedis.Services;

namespace MvcCoreCacheRedis.Controllers
{
    public class ProductosController : Controller
    {
        private RepositoryProductos repo;
        private ServiceCacheRedis service;

        public ProductosController
            (RepositoryProductos repo, ServiceCacheRedis service)
        {
            this.repo = repo;
            this.service = service;
        }

        public IActionResult Index()
        {
            List<Producto> productos = this.repo.GetProductos();
            return View(productos);
        }

        public IActionResult Details(int id)
        {
            Producto prod = this.repo.FindProducto(id);
            return View(prod);
        }

        public async Task<IActionResult> Favoritos(int id)
        {
            List<Producto> productsFavs = await
                this.service.GetProductosFavortos();
            return View(productsFavs);
        }

        public async Task<IActionResult> SeleccionarFavorito(int id)
        {
            //recuperamos el producto del repo
            Producto prod = this.repo.FindProducto(id);
            await this.service.AddProductoFavorito(prod);

            ViewData["MSG"] = "Almacenado correctamente";

            return RedirectToAction("Details", new { id = id });
        }

        public async Task<IActionResult> DeleteFavorito(int id)
        {
            await this.service.DeleteProductoFav(id);

            return RedirectToAction("Favoritos");
        }
    }
}
