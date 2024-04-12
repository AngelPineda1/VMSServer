using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Schema;
using VMSServer.Models;
using VMSServer.Services;

namespace VMSServer.ViewModels
{
    public class MensajeViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Mensaje> Mensajes { get; set; }=new ObservableCollection<Mensaje>();
        public string Mensaje {  get; set; }
        public bool Parpadeo { get; set; }
        public int Indice { get; set; } 
        public MensajeServer server = new();
        public ICommand SiguienteCommand { get; set; }
        public ICommand AnteriorCommand { get; set; }



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
            if (Mensajes != null && Mensajes.Any())
            {
                Mensaje = Mensajes.LastOrDefault()?.MensajeVMS;
                Parpadeo = Mensajes[Indice].Opcion;

            }
            else
            {
                Mensajes = new ObservableCollection<Mensaje>(); // Inicializa Mensajes con una nueva instancia vacía si es null
            }
            SiguienteCommand = new RelayCommand(Siguiente);
            AnteriorCommand = new RelayCommand(Anterior);
        }

        private void Anterior()
        {
            Indice--;
            if (Indice >= 0)
            {
                Mensaje = Mensajes[Indice].MensajeVMS;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));
                Parpadeo = Mensajes[Indice].Opcion;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Parpadeo)));



            }
            else
            {
                Indice = Mensajes.Count-1;
                Mensaje = Mensajes[Indice].MensajeVMS;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));
                Parpadeo = Mensajes[Indice].Opcion;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Parpadeo)));


            }
        }

        private void Siguiente()
        {
            Indice++;
            if(Indice <Mensajes.Count)
            {
                Mensaje = Mensajes[Indice].MensajeVMS;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));
                Parpadeo = Mensajes[Indice].Opcion;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Parpadeo)));

            }
            else
            {
                Indice = 0;
                Mensaje = Mensajes[Indice].MensajeVMS;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));

                Parpadeo = Mensajes[Indice].Opcion;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Parpadeo)));


            }
        }

        private void Server_MensajeRecicibido(object? sender, Mensaje e)
        {
            if (e!=null)
            {
                Mensajes.Add(e);
                Mensaje = e.MensajeVMS;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));
                Indice = Mensajes.Count - 1;
                Parpadeo = Mensajes[Indice].Opcion;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Parpadeo)));

            }

        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
