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


        static int port = 5010; // ���� �������
        static string address = "172.16.6.2"; // ����� �������
        static ServerObject server; // ������
        static Thread listenThread; // ����� ��� �������������

        public void ConnectXamarin()
        {
            Console.WriteLine("� �������� �����������");
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(address), port);//����� ����������� � ������� 
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // �������� ������
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10); // ��������� ��������
                                  //������������ ����� �����, ����� ������ �� �������
            while (true) // ������ ��������������
            {

                var listener = tcpSocket.Accept(); // ����� ����� ��� ����������� ������ ��������� (�������)
                                                   //����� ��������� �������� ��� ������� �������, � ����� ������������ � ����� 
                var buffer = new byte[256]; // ����� ������ ��� ��� ������ �������� � �������� �������
                var size = 0;//����������� ������ ������ (������� ����������)
                var data = new StringBuilder(); // �������� ������

                do
                { // �������� �� �� ������ (��������)


                    size = listener.Receive(buffer); // ����� ��������� ������, ��� ��������� 256 ���� �� �� ����� ���� ������ ����������
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size)); //���������� ���������� ������


                } while (listener.Available > 0); // ���� ���� ������ ������
                Console.WriteLine(data);
                //��� ���� ����� ���� �������� ����� ����� ������������� ������� ������ � �����
                listener.Send(Encoding.UTF8.GetBytes("�����"));
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();


            }
        }
    }
}
