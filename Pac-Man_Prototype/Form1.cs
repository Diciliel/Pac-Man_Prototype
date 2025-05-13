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
        public List <Food> foods { get; set; } = new List<Food>();
        public Walls walls { get; set; } = new Walls();

        int[,] gameMap = new int[,]
    {
        { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
        { 1,3,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,3,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1 },
        { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
        { 1,0,1,1,1,0,1,0,0,1,1,1,1,1,0,0,1,0,1,1,1,0,1 },
        { 1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1 },
        { 1,1,1,1,1,0,1,1,1,0,0,1,0,0,1,1,1,0,1,1,1,1,1 },
        { 4,4,4,4,1,0,1,3,0,0,0,0,0,0,0,3,1,0,1,4,4,4,4 },
        { 1,1,1,1,1,0,1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,1,1 },
        { 0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
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
            int ghostIndex = 0;

            for (int i = 0; i < gameMap.GetLength(0); i++)
            {
                for (int j = 0; j < gameMap.GetLength(1); j++)
                {
                    if (gameMap[i, j] == 2)
                    {
                        pacman.X = j * cellSize + (cellSize - pacman.width) / 2;
                        pacman.Y = i * cellSize + (cellSize - pacman.height) / 2;
                    }

                    if (gameMap[i, j] == 3 && ghostIndex < ghosts.Count)
                    {
                        ghosts[ghostIndex].X = j * cellSize + (cellSize - ghosts[ghostIndex].width) / 2;
                        ghosts[ghostIndex].Y = i * cellSize - (cellSize - ghosts[ghostIndex].height) / 2;
                        ghostIndex++;
                    }

                    if (gameMap[i, j] == 0)
                    {
                        Food newFood = new Food();
                        newFood.X = j * cellSize + (cellSize -newFood.width) / 2;
                        newFood.Y = i * cellSize + (cellSize -newFood.height) / 2;
                        foods.Add(newFood);
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
                    
                    foreach (var food in foods)
                    { 
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

        public void AddScore() 
        {
             score += 10;
             lbl_score.Text = "SCORE: 0 " + score;
        }

        public bool IsEating(Food food, PacMan pacman)
        {
            int pacmanCenterX = pacman.X + pacman.width / 2;
            int pacmanCenterY = pacman.Y + pacman.height / 2;

            int foodCenterX = food.X + food.width / 2;
            int foodCenterY = food.Y + food.height / 2;

            int distance = (int)Math.Sqrt(Math.Pow(pacmanCenterX - foodCenterX,2) +  Math.Pow(pacmanCenterY - foodCenterY,2));
            return distance < cellSize / 2;
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            DrawMap();
            pacman.Move(gameMap);

            foreach(var food in foods)
            {
                if (!food.isEaten && IsEating(food,pacman))
                {
                    food.isEaten = true;
                    AddScore();
                    break;
                }
            }

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
