using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database.Query;
using System.Linq;
using Firebase.Database;
using System.Threading.Tasks;
using AppCompras.Modelo;
using AppCompras.Conexiones;

namespace AppCompras.Datos
{
    public class Ddetallecompras
    {
        public async Task InsertarDc(Mdetallecompra parametros)
        {
            await Cconexion.firebase
                .Child("Detallecompra")
                .PostAsync(new Mdetallecompra()
                {
                    Cantidad =parametros.Cantidad,
                    Idproducto=parametros.Idproducto,
                    Preciocompra=parametros.Preciocompra,
                    Total=parametros.Total
                });
        }

        public async Task<List<Mdetallecompra>> MostrarVistapreviaDc()
        {
            var ListaDC = new List<Mdetallecompra>();
            var funcionproductos = new Dproductos();
            var parametrosProductos = new Mproductos();

            var data = (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>())
                .Where(a => a.Key != "Modelo")
                .Select(item => new Mdetallecompra
                {
                    Idproducto=item.Object.Idproducto,
                    Iddetallecompra=item.Key
                });
           // data.Where(a => a.Key != "Modelo");

            foreach(var hobit in data)
            {
                var parametros = new Mdetallecompra();
                parametros.Idproducto = hobit.Idproducto;
                parametrosProductos.Idproducto = hobit.Idproducto;
                
                var listaproductos = await funcionproductos.MostrarproductosXid(parametrosProductos);
                parametros.Imagen = listaproductos[0].Icono;
                ListaDC.Add(parametros);
            }
            return ListaDC;
        }

        public async Task<List<Mdetallecompra>> MostrarDc()
        {
            var ListaDC = new List<Mdetallecompra>();
            var funcionproductos = new Dproductos();
            var parametrosProductos = new Mproductos();

            var data = (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>())
                .Where(a => a.Key != "Modelo")
                .Select(item => new Mdetallecompra
                {
                    Idproducto = item.Object.Idproducto,
                    Iddetallecompra = item.Key,
                    Cantidad=item.Object.Cantidad,
                    Total= item.Object.Total,
                });
            // data.Where(a => a.Key != "Modelo");

            foreach (var hobit in data)
            {
                var parametros = new Mdetallecompra();
                parametros.Idproducto = hobit.Idproducto;
                parametrosProductos.Idproducto = hobit.Idproducto;

                var listaproductos = await funcionproductos.MostrarproductosXid(parametrosProductos);
                parametros.Descripcion = listaproductos[0].Descripcion;
                parametros.Imagen = listaproductos[0].Icono;
                parametros.Cantidad = hobit.Cantidad;
                parametros.Total = hobit.Total;
                ListaDC.Add(parametros);
            }
            return ListaDC;
        }

        public async Task<List<Mdetallecompra>> MostrarDcXidproducto(string idproducto)
        {
            return (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>()).Where(a => a.Object.Idproducto == idproducto).Select(item => new Mdetallecompra
                {
                    Total= item.Object.Total,
                }).ToList();
        }

        public async Task Editardetalle(Mdetallecompra parametros)
        {
            var data = (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>())
                .Where(a => a.Object.Idproducto == parametros.Idproducto)
                .FirstOrDefault();

            double cantidadInicial = Convert.ToDouble(data.Object.Cantidad);
            data.Object.Cantidad = (cantidadInicial + Convert.ToDouble(parametros.Cantidad)).ToString();

            double cantidad = Convert.ToDouble(data.Object.Cantidad);
            double preciocompra = Convert.ToDouble(parametros.Preciocompra);
            double total = 0;
            
            total = cantidad * preciocompra;
            data.Object.Total = total.ToString();

            //update en la bd
            await Cconexion.firebase
                .Child("Detallecompra")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task<List<Mdetallecompra>> Mostrarcantidades()
        {
            return (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>()
                ).Where(a=>a.Key!="Modelo")
                .Select(item => new Mdetallecompra
                {
                    Cantidad = item.Object.Cantidad,
                }).ToList();
        }


        public async Task<string> Sumarcantidad()
        {
            var funcion = new Ddetallecompras();
            var lista = await funcion.Mostrarcantidades();

            double cantidad = 0;

            foreach(var item in lista)
            {
                cantidad += Convert.ToDouble(item.Cantidad);
            }

            return cantidad.ToString();
        }

    }
}
