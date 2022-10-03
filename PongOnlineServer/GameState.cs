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

        public Ball ball;
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
                ball = new Ball();
                return "2";
            }
            return "Places are busy";
        }

        public string UpdateUser(string userName, string message)
        {
            var coords = message.Split(' ');

            if (userName == Player1.Name)
            {
                Player1.CanvasTop = coords[0];
                Player1.CanvasLeft = coords[1];
                ball.CanvasTop = coords[2];
                ball.CanvasLeft = coords[3];
                ball.dy = coords[4];
                ball.dx = coords[5];
                return Player1.Name;
            }
            else if(userName == Player2.Name)
            {
                Player2.CanvasTop = coords[0];
                Player2.CanvasLeft = coords[1];
                return Player2.Name;
            }
            return null;
        }

        public string DeleteUser(string userName)
        {
            if (userName == Player1.Name)
            {
                Player1 = null;
            }
            else if (userName == Player2.Name)
            {
                Player2 = null;
            }
            return $"{userName} удален";
        }

        public string GetOpponent(string userName)
        {
            string result = "Opponent not found";
            if (userName == Player1.Name)
            {
                result = Player2.CanvasTop + " " + Player2.CanvasLeft + " " + ball.CanvasTop + " " + ball.CanvasLeft + " " + ball.dy + " " + ball.dx;
            }
            else if (userName == Player2.Name)
            {
                result = Player1.CanvasTop + " " + Player1.CanvasLeft + " " + ball.CanvasTop + " " + ball.CanvasLeft + " " + ball.dy + " " + ball.dx;
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
