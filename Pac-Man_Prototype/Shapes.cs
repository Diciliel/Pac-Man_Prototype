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
    public enum Direction { left, right, up, down };
    public abstract class Shapes
    {
        public int X { get; set; }
        public int Y { get; set; }
                

        public int width;
        public int height;
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
            this.X = 32;
            this.Y = 32;
            this.width = 28;
            this.height = 28;
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
            this.X= 0;
            this.Y= 0;
            this.type = ShapeType.ghosts;
            this.width = 25;
            this.height = 60;
            this.direct = (Direction)random.Next(0,4);
        }

        public static List<Ghosts> Create() 
        {
            List <Ghosts> ghosts = new List <Ghosts>();

            for (int i = 0; i < 4; i++) 
            {
                ghosts.Add(new Ghosts());

                ghosts[i].X = 352;
                ghosts[i].Y = Ghosts.random.Next(320, 480);

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
        private bool isEaten;
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
            if (!this.isEaten)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb((int)this.color)), this.X, this.Y, this.width, this.height);
            }
            
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
