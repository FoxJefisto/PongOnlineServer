using PongOnlineServer;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleServer
{
    public class ClientObject
    {
        public TcpClient client;
        public GameState gameState { get; set; }
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
            gameState = GameState.GetInstance(); 
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    string response = "Default";
                    Console.WriteLine(message);
                    if ( message.Contains("start",StringComparison.OrdinalIgnoreCase))
                    {
                        string userName = message.Substring(0,message.IndexOf(":"));
                        response = gameState.RegisterUser(userName);
                    }
                    else if ( message.Contains("update", StringComparison.OrdinalIgnoreCase))
                    {
                        var userName = gameState.UpdateUser(message);
                        if(userName != null)
                        {
                            response = gameState.GetOpponent(userName);
                        }
                        else
                        {
                            response = "User not found";
                        }
                    }
                    else if (message.Contains("status", StringComparison.OrdinalIgnoreCase))
                    {
                        response = gameState.GetStatus();
                    }
                    // отправляем обратно сообщение в верхнем регистре
                    data = Encoding.Unicode.GetBytes(response);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}