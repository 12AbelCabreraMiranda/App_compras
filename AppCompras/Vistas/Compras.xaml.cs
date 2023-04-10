using AppCompras.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Compras : ContentPage
    {
        VMcompras vm;
        public Compras()
        {
            InitializeComponent();
            vm= new VMcompras(Navigation, Carrilderecha, Carrilizquierda);
            BindingContext = vm;
            this.Appearing += Compras_Appearing;
        }

        private async void Compras_Appearing(object sender, EventArgs e)
        {
            //esto va se como un disparador, cada vez que yo regrese a esta pagina
            //haga un pop y tambien cuando se muestre por primera vez se va activar
            await vm.MostrarVistapreviaDc();
            await vm.MostrarDetalleC();
            await vm.Sumarcantidades();
        }

        private async void DeslizarPanelcontador(object sender, SwipedEventArgs e)
        {
            await vm.MostrarpanelDc(gridproductos,Paneldetallecompra,Panelcontador);//los parametros son los nombres que ya están agregado en el front xaml
        }

        private async void DeslizarPaneldetallecompra(object sender, SwipedEventArgs e)
        {
            await vm.MostrargridPdroductos(gridproductos, Paneldetallecompra, Panelcontador);//los parametros son los nombres que ya están agregado en el front xaml
        }
    }
}