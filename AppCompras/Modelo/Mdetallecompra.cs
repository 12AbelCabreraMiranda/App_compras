using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompras.Modelo
{
    public class Mdetallecompra
    {
        public string Cantidad { get; set; }
        public string Preciocompra { get; set; }
        public string Idproducto { get; set; }
        public string Total { get; set; }
        public string Iddetallecompra { get; set; }

        //
        public string Imagen { get; set; }
        public string Descripcion { get; set; }
    }
}
