using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pac_Man_Prototype
{
    public partial class PacManGame : Form
    {
        public PacMan pacman { get; set; } = new PacMan();
        public List<Ghosts> ghosts { get; set; } = new List<Ghosts>();
        public List<Food> foods { get; set; } = new List<Food>();
        public GameMap gameMap { get; set; } = new GameMap(30, 20);

        Bitmap canvasBitmap;
        Graphics graphics;
        public PacManGame()
        {
            InitializeComponent();

            ghosts = Ghosts.Create();
            
            GameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            canvasBitmap = new Bitmap(canvas.Width, canvas.Height);
            graphics = Graphics.FromImage(canvasBitmap);

            BackColor = Color.FromArgb((int)gameMap.color);

            gameMap.Draw(graphics);

            pacman.Move(canvas.Width, canvas.Height);
            pacman.Draw(graphics);

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
