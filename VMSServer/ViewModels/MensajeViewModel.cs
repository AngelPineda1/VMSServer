using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using VMSServer.Models;
using VMSServer.Services;

namespace VMSServer.ViewModels
{
    public class MensajeViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Mensaje> Mensajes { get; set; }=new ObservableCollection<Mensaje>();
        public MensajeServer server = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public string IP
        {
            get
            {
                return string.Join(' ', Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(x => x.ToString()));
            }
        }
        public MensajeViewModel()
        {
            server.MensajeRecicibido += Server_MensajeRecicibido;
            server.Iniciar();

            Mensajes = server.CargarArchivo();
        }

        private void Server_MensajeRecicibido(object? sender, Mensaje e)
        {
            if (e!=null)
            {
                Mensajes.Add(e);
            }
        }
    }
}
