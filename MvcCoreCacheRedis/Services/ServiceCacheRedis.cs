using MvcCoreCacheRedis.Helpers;
using MvcCoreCacheRedis.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreCacheRedis.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase database;

        public ServiceCacheRedis()
        {
            this.database = 
                HelperCacheMultiplexer.Connection.GetDatabase();
        }

        //metodo para almacenar productos
        public async Task AddProductoFavorito(Producto producto)
        {
            //cache redis funciona con key/value 
            //dichas claves son comunes para todos los users
            //deberiamos tener una clave unica para cada user
            //vamos a almacenar el producto en formato JSON
            //tendremos en chache redis una coleccion de
            //todos los productos
            string jsonProductos = await
                this.database.StringGetAsync("favoritos");
            List<Producto> productosList;
            if (jsonProductos == null)
            {
                //aun no hay prods, creamos la lista
                productosList = new List<Producto>();

            }
            else
            {
                //ya tenamos favs en cache, los recuperamos
                productosList = 
                    JsonConvert.DeserializeObject<List<Producto>>
                    (jsonProductos);
            }

            //incluimos el nuevo producto
            productosList.Add(producto);
            //serializamos la coleccion con los nuevos datos de nuevo
            jsonProductos = JsonConvert.SerializeObject(productosList);
            //almacenamos los datos en cache redis
            await this.database.StringSetAsync("favoritos", jsonProductos);

        }

        //metodo para recuperar todos los productos
        public async Task<List<Producto>> GetProductosFavortos() 
        {
            string jsonProductos = await
                this.database.StringGetAsync("favoritos");
            if (jsonProductos == null)
            {
                return null;

            }
            else
            {
                List<Producto> favs = JsonConvert.DeserializeObject<List<Producto>>
                    (jsonProductos);
                return favs;
            }

        }

        //metodo para eliminar los favoritos
        public async Task DeleteProductoFav(int id)
        {
            //esto noe s una bbdd, no podemos filtrar
            List<Producto> favs = await
                this.GetProductosFavortos();

            if (favs != null)
            {
                //buscamos el producto
                Producto delete = favs.FirstOrDefault
                    (x => x.IdProducto == id);
                //ELIMINAMOS EL PRODUCTO DE LA COLECCION
                favs.Remove(delete);

                //comprobamos si quedan favs
                if(favs.Count == 0)
                {
                    //si no tenemos favs, eliminamos la key
                    await this.database.KeyDeleteAsync("favoritos");
                }
                else
                {
                    //ALMACENAMOS LOS PRODUCTOS FAVORITOS DE NUEVO
                    string jsonProductos =
                        JsonConvert.SerializeObject(favs);
                    //PODEMOS INDICAR POR CODIGO EL TIEMPO DE 
                    //DURACION DE LA KEY DENTRO DE CACHE REDIS.
                    //SI NO LE DIGO NADA, EL TIEMPO PREDETERMINADO
                    //ES DE 24 HORAS
                    await this.database.StringSetAsync
                        ("favoritos", jsonProductos
                        , TimeSpan.FromMinutes(30));

                }
            }

        }

    }
}
