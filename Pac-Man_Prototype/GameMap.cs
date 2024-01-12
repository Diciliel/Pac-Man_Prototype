using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pac_Man_Prototype
{
    public enum State { Food, Wall, Ghosts }
    public class GameMap
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public State state { get; private set; }

        Random random = new Random();

        public int cellSize;
        public uint color;
        //public GameMap[,] Map { get; private set; }
        public Shapes[,] shapes { get; set; }

        public GameMap(int rows, int cols) 
        {
            Rows = rows;
            Cols = cols;
            //state = (State)random.Next(0,5);
            color = 0xFF281C65;
            cellSize = 32;

            shapes = new Shapes[Rows, Cols];
            

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    double rand = random.NextDouble();

                    if (rand < 0.2)
                    {

                        shapes[i, j] = new Walls { X = i * cellSize, Y = j * cellSize };
                    }
                    else 
                    {
                        shapes[i, j] = new Food { X = i * cellSize + 11, Y = j * cellSize + 11 };
                    }
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
                    shapes[i, j]?.Draw(g);
                    
                }
            }            
        }
    }
}
