using PongOnlineServer;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

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
                    var matchCommand = Regex.Match(message, @"([^:]+): (\w+)\s*(.*)");
                    var userName = matchCommand.Groups[1].Value;
                    var command = matchCommand.Groups[2].Value;
                    string response;
                    switch (command)
                    {
                        case "register":
                            response = gameState.RegisterUser(userName);
                            break;
                        case "update":
                            gameState.UpdateUser(userName, matchCommand.Groups[3].Value);
                            response = gameState.GetOpponent(userName);
                            break;
                        case "status":
                            response = gameState.GetStatus();
                            break;
                        case "cancel":
                            response = gameState.DeleteUser(userName);
                            break;
                        default:
                            response = "unknown command";
                            break;
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