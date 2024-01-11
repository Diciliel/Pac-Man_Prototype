using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pac_Man_Prototype
{
    public enum State { Food, Wall, PowerUp, Empty, Ghosts }
    public class GameMap
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public State state { get; private set; }

        public ShapeType[,] Shapes { get; set; }

        Random random = new Random();

        private int cellSize;
        private uint color;

        //public GameMap[,] Map { get; private set; }

        public GameMap(int rows, int cols) 
        {
            Rows = rows;
            Cols = cols;
            state = (State)random.Next(0,5);
            color = 0xFF281C65;
            cellSize = 32;

            Shapes = new ShapeType[rows, cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Shapes[i, j] = (ShapeType)random.Next(0,5);
                }
            }
        }

        public void Draw(Graphics g) 
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb((int)this.color)), i * cellSize, j * cellSize, cellSize, cellSize);
                }
            }            
        }
    }
}
