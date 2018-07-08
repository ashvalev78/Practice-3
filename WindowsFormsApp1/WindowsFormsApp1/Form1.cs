using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        Line mainLine;
        Graphics[] graph = new Graphics[5]; 
        Graphics graph2;
        Pen[] colors = new Pen[10];
        int treeListD = 5;
        Point coordinates;

        public Form1()
        {
            InitializeComponent();
        }

        private void drawFract(Line recLine, int currentDepth, int fractNum, int depth)
        {
            if (currentDepth < depth)
            {
                for (int i = 0; i < 9; i++)
                {
                    recLine.MakeChilds();
                    //MessageBox.Show(Convert.ToString(recLine.sons[i].start.X) + ", " + Convert.ToString(recLine.sons[i].start.Y) + 
                    //    "; " + Convert.ToString(recLine.sons[i].end.X) + ", " + Convert.ToString(recLine.sons[i].end.Y));
                    graph[fractNum].DrawLine(colors[currentDepth], recLine.sons[i].start, recLine.sons[i].end);
                    drawFract(recLine.sons[i], currentDepth + 1, fractNum, depth);
                }
            }
            else return;
        }

        private void control()
        {
            mainLine = new Line(pictureBox1.Width / 9, pictureBox1.Height / 2, 8 * pictureBox1.Width / 9, pictureBox1.Height / 2);
            graph[0] = pictureBox1.CreateGraphics();
            graph[1] = pictureBox2.CreateGraphics();
            graph[2] = pictureBox3.CreateGraphics();
            graph[3] = pictureBox4.CreateGraphics();
            graph[4] = pictureBox5.CreateGraphics();
            graph2 = pictureBox6.CreateGraphics();
            colors[0] = new Pen(Color.Black);
            colors[1] = new Pen(Color.Green);
            colors[2] = new Pen(Color.Red);
            colors[3] = new Pen(Color.Orange);
            colors[4] = new Pen(Color.Blue);
            colors[5] = new Pen(Color.Goldenrod);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            control();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawTree();
            mainLine.MakeChilds();
            /*for (int i = 0; i < 5; i++)
            {
                graph[i].Clear(Color.White);
                graph[i].DrawLine(new Pen(Color.Black), mainLine.start, mainLine.end);
                drawFract(mainLine, 1, i, (i + 1));
            }*/
        }

        private void DrawLayer(int width, int number, int layer, int height)
        {
            double coeff = width / (number + 1);
            double nextCoeffX = width / (Math.Pow(3, layer - 1) + 1);
            int coeffY = height / 5;
            int k = 1;
            if (layer > 0)
            {
                for (int i = 0; i < number; i++)
                {
                    graph2.DrawEllipse(colors[layer], (int)(coeff * (i + 1)), layer * coeffY, treeListD, treeListD);
                    graph2.DrawLine(colors[layer], (int)(coeff * (i + 1)), layer * coeffY, (int)(nextCoeffX * k), (layer - 1) * coeffY + treeListD);
                    if ((i + 1) % 3 == 0)
                    {
                        k++;
                    }
                }
                DrawLayer(width, (int)Math.Pow(3, layer - 1), layer - 1, height);
            }
            else return;
        }

        private void DrawTree()
        {
            int width = pictureBox6.Width;
            treeListD = 5;
            int center = width / 2;
            graph2.DrawEllipse(new Pen(Color.Black), center - treeListD, 0, treeListD, treeListD);
            DrawLayer(width, (int)Math.Pow(3, 4), 4, pictureBox6.Height);
        }

        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            coordinates = me.Location;
            int pBH = pictureBox6.Height;
            if (coordinates.Y >= 3 * pBH / 5)
            {
                graph[4].Clear(Color.White);
                graph[4].DrawLine(new Pen(Color.Black), mainLine.start, mainLine.end);
                drawFract(mainLine, 1, 4, 5);
            }
            if (coordinates.Y >= 2 * pBH / 5 && coordinates.Y < 3 * pBH / 5)
            {
                graph[3].Clear(Color.White);
                graph[3].DrawLine(new Pen(Color.Black), mainLine.start, mainLine.end);
                drawFract(mainLine, 1, 3, 4);
            }
            if (coordinates.Y >= pBH / 5 && coordinates.Y < 2 * pBH / 5)
            {
                graph[2].Clear(Color.White);
                graph[2].DrawLine(new Pen(Color.Black), mainLine.start, mainLine.end);
                drawFract(mainLine, 1, 2, 3);
            }
            if (coordinates.Y >= 5 && coordinates.Y < pBH / 5)
            {
                graph[1].Clear(Color.White);
                graph[1].DrawLine(new Pen(Color.Black), mainLine.start, mainLine.end);
                drawFract(mainLine, 1, 1, 2);
            }
            if (coordinates.Y >= 0 && coordinates.Y < 5)
            {
                graph[0].DrawLine(colors[0], mainLine.start, mainLine.end);
            }
        }
    }

    public class Line
    {
        public Point start, end;
        public Line[] sons = new Line[9];
        public int length;
        public bool isVertical;

        public Line(int x1, int y1, int x2, int y2)
        {
            this.start = new Point(x1, y1);
            this.end = new Point(x2, y2);
            if (x1 == x2)
            {
                this.isVertical = true;
                this.length = y2 - y1;
            }
            else
            {
                this.isVertical = false;
                this.length = x2 - x1;
            }
        }


        public void MakeChilds()
        {
            if (!isVertical)
            {
                // horizontal line divided into three lines
                sons[0] = new Line(start.X, start.Y, start.X + (int)(length * 0.335), start.Y);
                sons[1] = new Line(start.X + (int)(length * 0.335), start.Y, start.X + (int)(length * 0.667), start.Y);
                sons[2] = new Line(start.X + (int)(length * 0.667), start.Y, end.X, start.Y);
                // top three lines
                sons[3] = new Line(start.X + (int)(length * 0.335), start.Y, start.X + (int)(length * 0.335), start.Y - (int)(length * 0.335));
                sons[4] = new Line(start.X + (int)(length * 0.667), start.Y, start.X + (int)(length * 0.667), start.Y - (int)(length * 0.335));
                sons[5] = new Line(start.X + (int)(length * 0.335), start.Y - (int)(length * 0.335), start.X + (int)(length * 0.667), start.Y - (int)(length * 0.335));
                // bottom three lines
                sons[6] = new Line(start.X + (int)(length * 0.335), start.Y, start.X + (int)(length * 0.335), start.Y + (int)(length * 0.335));
                sons[7] = new Line(start.X + (int)(length * 0.667), start.Y, start.X + (int)(length * 0.667), start.Y + (int)(length * 0.335));
                sons[8] = new Line(start.X + (int)(length * 0.335), start.Y + (int)(length * 0.335), start.X + (int)(length * 0.667), start.Y + (int)(length * 0.335));
            }
            else
            {
                // vertical line divided into three lines
                sons[0] = new Line(start.X, start.Y, start.X, start.Y + (int)(length * 0.335));
                sons[1] = new Line(start.X, start.Y + (int)(length * 0.335), start.X, start.Y + (int)(length * 0.667));
                sons[2] = new Line(start.X, start.Y + (int)(length * 0.667), start.X, end.Y);
                // right three lines
                sons[3] = new Line(start.X, start.Y + (int)(length * 0.335), start.X + (int)(length * 0.335), start.Y + (int)(length * 0.335));
                sons[4] = new Line(start.X, start.Y + (int)(length * 0.667), start.X + (int)(length * 0.335), start.Y + (int)(length * 0.667));
                sons[5] = new Line(start.X + (int)(length * 0.335), start.Y + (int)(length * 0.335), start.X + (int)(length * 0.335), start.Y + (int)(length * 0.667));
                // left three lines
                sons[6] = new Line(start.X, start.Y + (int)(length * 0.335), start.X - (int)(length * 0.335), start.Y + (int)(length * 0.335));
                sons[7] = new Line(start.X, start.Y + (int)(length * 0.667), start.X - (int)(length * 0.335), start.Y + (int)(length * 0.667));
                sons[8] = new Line(start.X - (int)(length * 0.335), start.Y + (int)(length * 0.335), start.X - (int)(length * 0.335), start.Y + (int)(length * 0.667));
            }
        }
    }
}
