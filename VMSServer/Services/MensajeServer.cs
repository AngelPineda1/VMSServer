using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media;
using VMSServer.Models;
using Newtonsoft.Json;

namespace VMSServer.Services
{
    public class MensajeServer
    {
        HttpListener server = new HttpListener();
        string Archivo = "Mensajes.json";
        public MensajeServer()
        {
            server.Prefixes.Add("http://*:8001/mensaje/");
        }
        public void Iniciar()
        {
            if (!server.IsListening)
            {
                server.Start();
                new Thread(Escuchar) { IsBackground = true }.Start();
            }
        }
        public event EventHandler<Mensaje>? MensajeRecicibido;
        private void Escuchar()
        {
            while (true)
            {
                var context = server.GetContext();
                var pagina = File.ReadAllText("C:\\Users\\BROCOTECH\\Documents\\Cliente Servidor\\Unidad 2\\VMSServer\\VMSServer\\Assets//index.html");
                var bufferpagina = Encoding.UTF8.GetBytes(pagina);
                if (context.Request.Url != null)
                {
                    if (context.Request.Url.LocalPath == "/mensaje/")
                    {
                        context.Response.ContentLength64 = bufferpagina.Length;
                        context.Response.OutputStream.Write(bufferpagina, 0, bufferpagina.Length);
                        context.Response.StatusCode = 200;
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod == "POST" && context.Request.Url.LocalPath == "/mensaje/enviar")
                    {
                        byte[] bufferdata = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(bufferdata, 0, bufferdata.Length);
                        string datos = Encoding.UTF8.GetString(bufferdata);
                        var diccionario = HttpUtility.ParseQueryString(datos);
                        Mensaje mensaje = new Mensaje()
                        {
                            MensajeVMS = diccionario["mensaje"] ?? ""
                        };
                        GuardarMensaje(mensaje);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            MensajeRecicibido?.Invoke(this, mensaje);
                        }));
                        context.Response.StatusCode = 200;
                        context.Response.Close();
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }

                }

            }
        }

        private void GuardarMensaje(Mensaje mensaje)
        {
            List<Mensaje> mensajes;

            // Verificar si el archivo existe y cargar los mensajes existentes
            if (File.Exists(Archivo))
            {
                var json = File.ReadAllText(Archivo);

                
                    mensajes = JsonConvert.DeserializeObject<List<Mensaje>>(json);
                
            }
            else
            {
                // Si el archivo no existe, crear una nueva lista de mensajes
                mensajes = new List<Mensaje>();
            }

            // Agregar el nuevo mensaje a la lista
            mensajes.Add(mensaje);

            // Serializar la lista completa de mensajes a JSON
            string mensajesJson = JsonConvert.SerializeObject(mensajes);

            // Guardar el JSON en el archivo
            File.WriteAllText(Archivo, mensajesJson);
        }
        public ObservableCollection<Mensaje>? CargarArchivo()
        {
            if (File.Exists(Archivo))
            {
                var json = File.ReadAllText(Archivo);

                
                    var mensajes = JsonConvert.DeserializeObject<List<Mensaje>>(json);
                    return new ObservableCollection<Mensaje>(mensajes);
                


            }
            return null;
        }
        public void Detener()
        {
            server.Stop();
        }
    }
}
