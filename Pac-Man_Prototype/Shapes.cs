using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pac_Man_Prototype
{
    public enum ShapeType { pacman, ghosts, food, wall, powerups }
    public enum Direction { left, right, up, down };
    public abstract class Shapes
    {
        public int X { get; set; }
        public int Y { get; set; }
                

        public int width;
        public int height;
        public int cellSize = 32;
        protected uint color;
        protected ShapeType type;
        public Direction direct;

        public abstract void Draw(Graphics g);
    }

    public class PacMan : Shapes
    {
        public int speed;
        private int startAngle;

        public PacMan() 
        {
            this.type = ShapeType.pacman;
            this.X = 0;
            this.Y = 0;
            this.width = 30;
            this.height = 30;
            this.speed = cellSize / 2;
            this.direct = Direction.right;
            this.color = 0xFFF8C93C;
            this.startAngle = 45;           
        }

        public void GoRight() 
        {
            direct = Direction.right;
        }

        public void GoLeft()
        {
            direct = Direction.left;
        }

        public void GoUp()
        {
            direct = Direction.up;
        }

        public void GoDown()
        {
            direct = Direction.down;
        }
        public void Move(int[,] gameMap) 
        {
            int nextX = this.X;
            int nextY = this.Y;

            switch (this.direct) 
            {
                case Direction.right:
                    nextX += this.speed;
                    this.startAngle = 45;
                    break;
                case Direction.left:
                    nextX -= this.speed;
                    this.startAngle = 225;
                    break;
                case Direction.up:
                    nextY -= this.speed;
                    this.startAngle = 315;
                    break;
                case Direction.down:
                    nextY += this.speed;
                    this.startAngle = 135;
                    break;
            }
            if (nextX / cellSize >= 0 && nextX / cellSize < gameMap.GetLength(1) && nextY / cellSize >= 0 && nextY / cellSize < gameMap.GetLength(0))
            {
                if (gameMap[nextY / cellSize, nextX / cellSize] != 1 && // Top-left corner
                    gameMap[(nextY + this.height - 1) / cellSize, nextX / cellSize] != 1 && // Bottom-left corner
                    gameMap[nextY / cellSize, (nextX + this.width - 1) / cellSize] != 1 && // Top-right corner
                    gameMap[(nextY + this.height - 1) / cellSize, (nextX + this.width - 1) / cellSize] != 1) // Bottom-right corner
                {
                    this.X = nextX;
                    this.Y = nextY;
                }
            }
        }

        public override void Draw(Graphics g)
        {
            g.FillPie(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height, startAngle, 270);
        }
    }

    public class Ghosts : Shapes
    {
        public int speed;
        private static uint[] colors = { 0xFFEB2C5D, 0xFFFC6840, 0xFFA0178A, 0xFF622097 };
        private static Random random = new Random();    

        public Ghosts()
        {
            this.X= 0;
            this.Y= 0;
            this.type = ShapeType.ghosts;
            this.width = 30;
            this.height = 30;
            this.direct = (Direction)random.Next(0,4);
        }

        public static List<Ghosts> Create() 
        {
            List <Ghosts> ghosts = new List <Ghosts>();

            for (int i = 0; i < 4; i++) 
            {
                ghosts.Add(new Ghosts());

                ghosts[i].direct = (Direction)Ghosts.random.Next(0, 4);

                ghosts[i].color = colors[i];
                ghosts[i].speed = Ghosts.random.Next(12, 18);
            }
            return ghosts;
        }
        public void Move(int[,] gameMap) 
        {
            int nextX = this.X;
            int nextY = this.Y;

            switch (this.direct) 
            {
                case Direction.right:
                    nextX += this.speed;
                    break;
                case Direction.left:
                    nextX -= this.speed;
                    break;
                case Direction.up:
                    nextY -= this.speed;
                    break;
                case Direction.down:
                    nextY += this.speed;
                    break;
            }

            if (IsFree(nextX, nextY, gameMap))
            { 
                this.X = nextX;
                this.Y = nextY;
            }
            else
            {
                List<Direction> validDirections = new List<Direction>();

                if (IsFree (this.X + speed, this.Y, gameMap)) validDirections.Add (Direction.right);
                if (IsFree (this.X - speed, this.Y, gameMap)) validDirections.Add (Direction.left);
                if (IsFree (this.X, this.Y - speed, gameMap)) validDirections.Add (Direction.up);
                if (IsFree (this.X, this.Y + speed, gameMap)) validDirections.Add(Direction.down);

                if (validDirections.Count > 0)
                {
                    this.direct = validDirections[random.Next (validDirections.Count)];
                }                
            }
        }
        private bool IsFree(int x, int y, int[,] map)
        {
            int width = this.width;
            int height = this.height;

            int cellSize = this.cellSize;

            int gridCols = map.GetLength(1);
            int gridRows = map.GetLength(0);

            int tlX = x / cellSize;
            int tlY = y / cellSize;

            int trX = (x + width - 1) / cellSize;
            int trY = y / cellSize;

            int blX = x / cellSize;
            int blY = (y + height - 1) / cellSize;

            int brX = (x + width - 1) / cellSize;
            int brY = (y + height - 1) / cellSize;

            bool insideBounds = tlX >= 0 && trX < gridCols && tlY >= 0 && blY < gridRows;

            if (!insideBounds) return false;

            return
                map[tlY, tlX] != 1 && map[trY, trX] != 1 && map[blY, blX] != 1 && map[brY, brX] != 1;
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height);
            //g.DrawRectangle(Pens.Red, this.X, this.Y, this.width, this.height);
        }       
    }

    public class Food : Shapes 
    {
        public bool isEaten;
        public Food()
        {
            this.X = 0;
            this.Y = 0;
            this.type = ShapeType.food;
            this.width = 6;
            this.height = 6;
            this.color = 0xFFFEDF2F;
            this.isEaten = false;
        }

        public override void Draw(Graphics g)
        {
             g.FillEllipse(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height);                        
        }
    }

    public class Walls : Shapes 
    {
        public Walls()
        {
            this.X = 0;
            this.Y = 0;
            this.type = ShapeType.wall;
            this.width= 32;
            this.height= 32;
            this.color= 0xFF4026d6;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.FromArgb((int)this.color), 3), this.X, this.Y, this.width, this.height);
        }
    }
}
