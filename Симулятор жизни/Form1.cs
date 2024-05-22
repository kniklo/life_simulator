using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Симулятор_жизни
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const int Width = 100;
        private const int Height = 50;
        private const int CellSize = 10;
        private int[,] currentField = new int[Width+2, Height+2];
        private int[,] lastField = new int[Width+2, Height+2];
        Graphics g;
        Pen pen;
        Brush brushGreen, brushWhite;

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Height = CellSize * Height + Height;
            pictureBox1.Width = CellSize * Width + Width;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            pen = new Pen(Color.Black);
            brushGreen = new SolidBrush(Color.Green);
            brushWhite = new SolidBrush(Color.White);
            StartGame();
        }
        void StartGame()
        {
            FillField();
            DrawField();
            timer1.Enabled = true;
            timer1.Interval = 10; 
            timer1.Stop();
        }

        private void FillField()
        {
            var rand = new Random();
            for (int col = 1; col <= Width; col++)
            {
                for (int row = 1; row <= Height; row++)
                {
                    currentField[col, row] = rand.Next(2);
                }
            }
            for (int col = 0; col <= Width + 1; col++)
            {
                currentField[col, 0] = 0;
                currentField[col, Height + 1] = 0;
                lastField[col, 0] = 0;
                lastField[col, Height + 1] = 0;
            }

            for (int row = 0; row <= Height + 1; row++)
            {
                currentField[0, row] = 0;
                currentField[Width + 1, row] = 0;
                lastField[0, row] = 0;
                lastField[Width + 1, row] = 0;
            }
        }
        private void DrawField()
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    if (currentField[col, row] == 0)
                        DrawEmptyCell(col, row);
                    else
                        DrawFillCell(col, row);
                }

            }
            this.Refresh();
        }
        private void DrawEmptyCell(int col, int row)
        {
            var x = (col -1) * CellSize + 1; 
            var y = (row -1) * CellSize + 1;     
            g.DrawRectangle(pen, x, y, CellSize, CellSize);
            g.FillRectangle(brushWhite, x + 2, y + 2, CellSize - 3, CellSize - 3);
        }
        private void DrawFillCell(int col, int row)
        {
            var x = (col -1) * CellSize + 1;
            var y = (row -1) * CellSize + 1;
            g.DrawRectangle(pen, x, y, CellSize, CellSize);
            g.FillRectangle(brushGreen, x + 2, y + 2, CellSize - 3, CellSize - 3);
        }
        //
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshArrays();
            DrawField();
        }
        private void RefreshArrays()
        {
            for (int col = 1; col <= Width; col++)
            {
                for (int row = 1; row <= Height; row++)
                {
                    lastField[col, row] = currentField[col, row];
                }
            }
            RefillArray();
        }

        private void RefillArray()
        {
            for (int col = 1; col <= Width; col++)
            {
                for (int row = 1; row <= Height; row++)
                {
                    if (lastField[col, row] == 0)
                    {
                        if (GetEmptyCellStatus(col, row))
                            currentField[col, row] = 1;
                    }
                    else
                    {
                        if(!GetAliveCellStatus(col,row))
                            currentField[col, row] = 0;
                    }
                }
            }
        }

        private bool GetAliveCellStatus(int col, int row)
        {
            var neighbours = 0;
            for (int i = col - 1; i <= col + 1 ; i++)
            {
                for (int j = row-1; j <= row+1; j++)
                {
                    if (lastField[i, j] != 0)
                        neighbours++;
                }
            }
            neighbours--; //так как саму клетку считаем тоже
            if (neighbours < 2) return false;
            if (neighbours > 3) return false;
            return true;
        }

        private bool GetEmptyCellStatus(int col, int row)
        {
            var neighbours = 0;
            for (int i = col - 1; i <= col + 1; i++)
            {
                for (int j = row - 1; j <= row + 1; j++)
                {
                    if (lastField[i, j] != 0)
                        neighbours++;
                }
            }
            if (neighbours == 3) return true;
            return false;
        }


    }
}
