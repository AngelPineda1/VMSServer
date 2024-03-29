using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VMSServer.Views
{
    /// <summary>
    /// Lógica de interacción para MensajesView.xaml
    /// </summary>
    public partial class MensajesView : UserControl
    {
        public MensajesView()
        {
            InitializeComponent();
            // Cargar la fuente desde el archivo
            var fontUri = new Uri("DS-DIGI.ttf", UriKind.Relative);
            FontFamily font = new FontFamily(fontUri.ToString());

            // Establecer la fuente para la ventana o para controles específicos
            this.FontFamily = font;
        }
    }
}
