using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongOnlineServer
{
    public class Player
    {
        public string Name { get; set; }
        public string CanvasLeft { get; set; }
        public string CanvasTop { get; set; }

        public Player(string userName)
        {
            Name = userName;
        }
    }
}
