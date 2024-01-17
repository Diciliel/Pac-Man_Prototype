using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Pac_Man_Prototype
{
    public partial class PacManGame : Form
    {
        public PacMan pacman { get; set; } = new PacMan();
        public List<Ghosts> ghosts { get; set; } = new List<Ghosts>();
        public Food food { get; set; } = new Food();
        public Walls walls { get; set; } = new Walls();

        int[,] gameMap = new int[,]
    {
        { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
        { 1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
        { 1,0,1,1,1,0,1,0,0,1,1,1,1,1,0,0,1,0,1,1,1,0,1 },
        { 1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1 },
        { 1,1,1,1,1,0,1,1,1,0,0,1,0,0,1,1,1,0,1,1,1,1,1 },
        { 4,4,4,4,1,0,1,0,0,0,0,0,0,0,0,0,1,0,1,4,4,4,4 },
        { 1,1,1,1,1,0,1,0,1,1,1,3,1,1,1,0,1,0,1,1,1,1,1 },
        { 0,0,0,0,0,0,0,0,1,0,3,3,3,0,1,0,0,0,0,0,0,0,0 },
        { 1,1,1,1,1,0,1,0,1,1,1,1,1,1,1,0,1,0,1,1,1,1,1 },
        { 4,4,4,4,1,0,1,0,0,0,0,0,0,0,0,0,1,0,1,4,4,4,4 },
        { 1,1,1,1,1,0,1,0,0,1,1,1,1,1,0,0,1,0,1,1,1,1,1 },
        { 1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
        { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
    };

        Bitmap canvasBitmap;
        Graphics graphics;
        public PacManGame()
        {
            InitializeComponent();

            ghosts = Ghosts.Create();

            GameTimer.Start();
        }

        private void DrawMap() 
        {
            uint bgcolor = 0xFF281C65;
            int cellSize = 32;

            for (int i = 0; i < gameMap.GetLength(0); i++) 
            {
                for(int j = 0; j< gameMap.GetLength(1); j++)
                {
                    if (gameMap[i, j] == 1)
                    {
                        walls.X = j * walls.width;
                        walls.Y = i * walls.height;

                        walls.Draw(graphics);
                    }

                    /*if (gameMap[i, j] == 2) 
                    {
                        pacman.X = j * cellSize + (cellSize - pacman.width) / 2;
                        pacman.Y = i * cellSize + (cellSize - pacman.height) / 2;

                        pacman.Draw(graphics);
                    }*/

                    /*if (gameMap[i, j] == 3)
                    {
                        for (int a = 0; a < 4; a++) 
                        {
                           ghosts = Ghosts.Create(i, j);

                            ghosts[a].X = j * cellSize + (cellSize - ghosts[a].width) / 2;
                            ghosts[a].Y = i * cellSize - (25 + cellSize - ghosts[a].height) / 2;

                            ghosts[a].Draw(graphics);
                        }
                    }*/

                    if (gameMap[i,j] == 0)
                    {
                        food.X = j * cellSize + (cellSize - food.width) / 2;
                        food.Y = i * cellSize + (cellSize - food.height) / 2;

                        food.Draw(graphics);
                    }
                    pacman.Draw(graphics);
                }
            }
            BackColor = Color.FromArgb((int)bgcolor);
        }
        /*private void MovePacman()
        {
            int nextX = pacman.X;
            int nextY = pacman.Y;

            if (nextX >= 0 && nextX < gameMap.GetLength(1) && nextY >= 0 && nextY < gameMap.GetLength(0) && gameMap[nextX, nextY] != 1)
            {
                pacman.Move(canvas.Width, canvas.Height);
            }
        }*/
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            canvasBitmap = new Bitmap(canvas.Width, canvas.Height);
            graphics = Graphics.FromImage(canvasBitmap);

            DrawMap();
            //MovePacman();
            //pacman.Draw(graphics);
            pacman.Move(canvas.Width, canvas.Height);

            foreach (Ghosts ghost in ghosts)
            {
                ghost.Draw(graphics);
                ghost.Move(canvas.Width, canvas.Height);
            }

            canvas.Image = canvasBitmap;
        }

        private void PacManGame_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    pacman.GoLeft();
                    break;
                case Keys.Right:
                    pacman.GoRight();
                    break;
                case Keys.Up:
                    pacman.GoUp();
                    break;
                case Keys.Down:
                    pacman.GoDown();
                    break;
            }
        }
    }
}
