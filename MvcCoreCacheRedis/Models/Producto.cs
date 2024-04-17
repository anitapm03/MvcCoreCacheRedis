namespace MvcCoreCacheRedis.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public string Imagen { get; set; }
    }
}
/*<producto>
    <idproducto>1</idproducto>
    <nombre>Nike Air Jordan</nombre>
    <descripcion>Las Air Jordan VI dejaron huella el año en que MJ llevó a los Bulls hacia su primer título de campeonato. En la actualidad, las Air Jordan 6 Retro para hombre rinden homenaje a las originales de 1991 que cautivaron a toda una generación.</descripcion>
    <precio>150</precio>
    <imagen>https://images-na.ssl-images-amazon.com/images/I/71ruzoe8%2BvL._AC_UY395_.jpg</imagen>
  </producto>*/