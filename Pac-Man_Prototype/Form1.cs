using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public int score = 0;
        public int cellSize = 32;

        Bitmap canvasBitmap;
        Graphics graphics;
        public PacManGame()
        {
            InitializeComponent();

            ghosts = Ghosts.Create();

            for (int i = 0; i < gameMap.GetLength(0); i++)
            {
                for (int j = 0; j < gameMap.GetLength(1); j++)
                {
                    if (gameMap[i, j] == 2)
                    {
                        pacman.X = j * cellSize + (cellSize - pacman.width) / 2;
                        pacman.Y = i * cellSize + (cellSize - pacman.height) / 2;
                    }

                    if (gameMap[i, j] == 3)
                    {
                        for (int a = 0; a < ghosts.Count; a++)
                        {
                            ghosts[a].X = j * cellSize + (cellSize - ghosts[a].width) / 2;
                            ghosts[a].Y = i * cellSize - (25 + cellSize - ghosts[a].height) / 2;                           
                        }
                    }
                }
            }

            GameTimer.Start();
        }

        private void DrawMap() 
        {
            canvasBitmap = new Bitmap(canvas.Width, canvas.Height);
            graphics = Graphics.FromImage(canvasBitmap);

            uint bgcolor = 0xFF281C65;
           
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

                    pacman.Draw(graphics);

                    for(int a = 0; a < ghosts.Count; a++)
                    {
                        ghosts[a].Draw(graphics);
                    }
                    
                    if (gameMap[i,j] == 0)
                    {
                        food.X = j * cellSize + (cellSize - food.width) / 2;
                        food.Y = i * cellSize + (cellSize - food.height) / 2;

                        if (!food.isEaten) 
                        {
                            food.Draw(graphics);
                        }                       
                    }                   
                }
            } 

            BackColor = Color.FromArgb((int)bgcolor);

            canvas.Image = canvasBitmap;
        }

        public void EatFood() 
        {
            if (pacman.X == food.X && pacman.Y == food.Y)
            {
                food.isEaten = true;
                score++;
                lbl_score.Text = "SCORE: 0 " + score;
            }
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            DrawMap();
            EatFood();
            pacman.Move(gameMap);
          
            foreach (Ghosts ghost in ghosts)
            {
                ghost.Move(gameMap);
            }
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
