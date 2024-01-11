using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man_Prototype
{
    public enum ShapeType { pacman, ghosts, food, wall, powerups }
    public abstract class Shapes
    {
        public int X { get; set; }
        public int Y { get; set; }
        protected enum Direction { left, right, up, down };        

        protected int width;
        protected int height;
        protected uint color;
        protected ShapeType type;
        protected Direction direct;

        public abstract void Draw(Graphics g);
    }

    public class PacMan : Shapes
    {
        public int speed;
        private int startAngle;

        public PacMan() 
        {
            this.type = ShapeType.pacman;
            this.X = 64;
            this.Y = 64;
            this.width = 32;
            this.height = 32;
            this.speed = 15;
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
        public void Move(int maxWidth, int maxHeight) 
        {
            switch (this.direct) 
            {
                case Direction.right:
                    this.X += this.speed;
                    this.startAngle = 45;
                    if (X > maxWidth) X = 0;
                    break;
                case Direction.left:
                    this.X -= this.speed;
                    this.startAngle = 225;
                    if (X < 0) X = maxWidth;
                    break;
                case Direction.up:
                    this.Y -= this.speed;
                    this.startAngle = 315;
                    if (Y + height < 0) Y = maxHeight;
                    break;
                case Direction.down:
                    this.Y += this.speed;
                    this.startAngle = 135;
                    if (Y > maxHeight) Y = 0;
                    break;
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
            this.type = ShapeType.ghosts;
            this.width = 25;
            this.height = 64;
            this.direct = (Direction)random.Next(0,4);
        }

        public static List<Ghosts> Create() 
        {
            List <Ghosts> ghosts = new List <Ghosts>();

            for (int i = 0; i < 4; i++) 
            {
                ghosts.Add(new Ghosts());

                ghosts[i].X = Ghosts.random.Next(448,544);;
                ghosts[i].Y = 320;

                ghosts[i].direct = (Direction)Ghosts.random.Next(0, 4);

                ghosts[i].color = colors[i];
                ghosts[i].speed = Ghosts.random.Next(13, 18);

            }
            return ghosts;
        }
        public void Move(int maxWidth, int maxHeight) 
        {
            switch (this.direct) 
            {
                case Direction.right:
                    this.X += this.speed;
                    if (X > maxWidth) X = 0;
                    break;
                case Direction.left:
                    this.X -= this.speed;
                    if (X < 0) X = maxWidth;
                    break;
                case Direction.up:
                    this.Y -= this.speed;
                    if (Y + height < 0) Y = maxHeight;
                    break;
                case Direction.down:
                    this.Y += this.speed;
                    if (Y > maxHeight) Y = 0;
                    break;
            }
        }
        public override void Draw(Graphics g)
        {
            g.FillPie(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height, 180, 180);
        }
    }

    public class Food : Shapes 
    {
        public Food()
        {
            this.type = ShapeType.food;
            //this.X = 64;
            //this.Y = 64;
            this.width = 10;
            this.height = 10;
            this.color = 0xFFFEDF2F;
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height);
        }
    }

    public class Walls : Shapes 
    {
        private static Walls[] WallArray;
        private int[,] Dots;
        public Walls()
        {
            this.type = ShapeType.wall;
            this.width= 32;
            this.height= 32;
            this.color= 0xFF4026d6;

            WallArray = new Walls[]
            {
                new Walls                
                {
                    Dots = new int[,]
                    {
                        {1, 1, 1, 1, 1,}
                    }                    
                },

                new Walls
                {
                    Dots = new int[,]
                    {
                        {1, 1, 1, 1,},
                        {1, 0, 0, 0,}
                    }
                },

                new Walls
                {
                    Dots = new int[,]
                    {
                        {1, 1, 1, 1, 1,},
                        {0, 0, 1, 0, 0,},
                        {0, 0, 1, 0, 0,}
                    }
                },
            };
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height);
        }
    }
}
