﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ISimpleHttpServer.Model;

using HttpListener = SimpleHttpServer.Service.HttpListener;

namespace Console.Net.Test
{
    class Program
    {
        private static HttpListener _httpListener;
        static void Main(string[] args)
        {
            _httpListener = new HttpListener(timeout: TimeSpan.FromSeconds(30));

            StartTcpListener();
            System.Console.ReadKey();
        }

        private static async void StartTcpListener()
        {
            await _httpListener.StartTcpRequestListener(port: 8000);
            
            // Rx Subscribe
            _httpListener.HttpRequestObservable.Subscribe(async 
               request =>
               {

                //Enter your code handling each incoming Http request here.

                //Example response
                System.Console.WriteLine($"Remote Address: {request.RemoteAddress}");
                System.Console.WriteLine($"Remote Port: {request.RemotePort}");
                System.Console.WriteLine("--------------***-------------");
                if (request.RequestType == RequestType.TCP)
                {
                    var response = new Console.Net.Test.Model.HttpReponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        ResponseReason = HttpStatusCode.OK.ToString(),
                        Headers = new Dictionary<string, string>
                            {
                                {"Date", DateTime.UtcNow.ToString("r")},
                                {"Content-Type", "text/html; charset=UTF-8" },
                            },
                        Body = new MemoryStream(Encoding.UTF8.GetBytes($"<html>\r\n<body>\r\n<h1>Hello, World! {DateTime.Now}</h1>\r\n</body>\r\n</html>"))
                    };

                    await _httpListener.HttpReponse(request, response).ConfigureAwait(false);
                }

               });
        }
    }
}
