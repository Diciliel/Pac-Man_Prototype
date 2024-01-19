using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pac_Man_Prototype
{
    public class GameMap
    {
        //public int cellSize;
        public int[,] map;

        public GameMap(int[,] map)
        {
            //this.color = 0xFF281C65;
            //this.cellSize = 32;
            this.map = map;
        }

        /*public bool CanMove(int x, int y) 
        {
            if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1)) 
            {
                return map[x, y] != 1;
            }
            return false;
        }*/
    } 
}
