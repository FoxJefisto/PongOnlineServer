using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PongOnlineServer
{
    public class GameState
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public bool gameStatus { get; set; } = false;

        private static GameState instance;
        public static GameState GetInstance()
        {
            if (instance == null)
                instance = new GameState();
            return instance;
        }

        public string RegisterUser(string userName)
        {
            if (Player1 == null)
            {
                Player1 = new Player(userName);
                return "1";
            }
            else if (Player2 == null)
            {
                Player2 = new Player(userName);
                gameStatus = true;
                return "2";
            }
            return "Places are busy";
        }

        public string UpdateUser(string message)
        {
            string userName = message.Substring(0, message.IndexOf(":"));
            var matchCoords = Regex.Match(message, @"update (\d+) (\d+)");
            var canvasTop = matchCoords.Groups[1].Value;
            var canvasLeftRight = matchCoords.Groups[2].Value;
            if (userName == Player1.Name)
            {
                Player1.CanvasTop = canvasTop;
                Player1.CanvasLeft = canvasLeftRight;
                return Player1.Name;
            }
            else if(userName == Player2.Name)
            {
                Player2.CanvasTop = canvasTop;
                Player2.CanvasLeft = canvasLeftRight;
                return Player2.Name;
            }
            return null;
        }

        public string GetOpponent(string userName)
        {
            string result = "Opponent not found";
            if (userName == Player1.Name)
            {
                result = Player2.CanvasTop + " " + Player2.CanvasLeft;
            }
            else if (userName == Player2.Name)
            {
                result = Player1.CanvasTop + " " + Player1.CanvasLeft;
            }
            return result;
        }

        public string GetStatus()
        {
            return gameStatus.ToString();
        }

        private GameState() { }
    }
}
