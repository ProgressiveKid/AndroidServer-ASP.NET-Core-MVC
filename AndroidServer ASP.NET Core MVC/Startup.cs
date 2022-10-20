using ConsoleServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidServer_ASP.NET_Core_MVC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
           // ConnectXamarin();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
            });
        }


        static int port = 5010; // порт сервера
        static string address = "172.16.6.2"; // адрес сервера
        static ServerObject server; // сервер
        static Thread listenThread; // поток дл€ прослушивани€

        public void ConnectXamarin()
        {
            Console.WriteLine("¬ ожидании подключени€");
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(address), port);//“очка подключени€ к серверу 
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // —оздание сокета
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10); // ѕрослушка запросов
                                  //јссинхронное нужно здесь, чтобы сервер не зависал
            while (true) // ƒелаем прослушиватель
            {

                var listener = tcpSocket.Accept(); // новый сокет дл€ подключени€ нового слушател€ (клиента)
                                                   //сокет создаетс€ отдельно под каждого клиента, а потом уничтожаетс€ в конце 
                var buffer = new byte[256]; // буфер данных так как данные приход€т в двоичном формате
                var size = 0;//фактический размер данных (реально полученных)
                var data = new StringBuilder(); // собирает данные

                do
                { // ѕолучили ли мы запрос (проверка)


                    size = listener.Receive(buffer); // ћетод получени€ байтов, где получвими 256 байт но их может быть другое количество
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size)); //раскодиуем полученные данные


                } while (listener.Available > 0); // ѕока есть данные читаем
                Console.WriteLine(data);
                //дл€ того чтобы дать обратный ответ нужно закодироавать обратно строку в байты
                listener.Send(Encoding.UTF8.GetBytes("”спех"));
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();


            }
        }
    }
}
