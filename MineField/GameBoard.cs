using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineField
{
    public partial class GameBoard : Form
    {
        public GameBoard()
        {
            InitializeComponent();
        }
        Button[,] buttons = new Button[8, 8];
        public int score = 0;
        List<int> mineIndices = new List<int>();
        //this project can have a dynamic structure, for this, all constant numbers can be assigned and recreated here.

        private void GameBoard_Load(object sender, EventArgs e)
        {                       // Place dynamic key generator and mine
            int MineRate = 20;          
            int MaxMineAmount = 10;
            int left = 0;
            int top = 0;
            this.Width = 550;
            this.Height = 570;

            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Height = 64;
                    buttons[i, j].Width = 64;
                    buttons[i, j].Left = left;
                    buttons[i, j].Top = top;

                    if (mineIndices.Count < MaxMineAmount && random.Next(100) < MineRate && !buttons[i, j].Name.StartsWith("X"))
                    {
                        buttons[i, j].Name = $"X{i}{j}";
                        mineIndices.Add(i * 8 + j);
                    }
                    left += 64;
                    buttons[i, j].Click += Button_Click;
                    buttons[i, j].MouseDown += Button_MouseClick;
                    this.Controls.Add(buttons[i, j]);

                }
                top += 64;
                left = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (buttons[i, j].Name.StartsWith("X"))
                        continue;

                    int minesAround = CountMinesAround(i, j, buttons);
                    if (minesAround > 0)
                    {
                        buttons[i, j].Name = minesAround.ToString();
                    }
                    else
                    {
                        buttons[i, j].Name = $"E{i}{j}";
                    }
                }
            }

        }


        private int CountMinesAround(int row, int col, Button[,] buttons)
        {                                                           // finds out how many mines are next to it
            int count = 0;

            for (int x = row - 1; x <= row + 1; x++)
            {
                for (int y = col - 1; y <= col + 1; y++)
                {

                    if (x == row && y == col)
                        continue;

                    if (x >= 0 && x < 8 && y >= 0 && y < 8)
                    {
                        if (buttons[x, y].Name.StartsWith("X"))
                            count++;
                    }
                }
            }

            return count;
        }

        private void GameOver()
        {           // check the whole board reveal all the mines
            foreach (var button in buttons)
            {
                if (button.Name.StartsWith("X"))
                {
                    button.BackColor = Color.Red;
                    button.Text = "💣";
                }
            }

        }
        private void Won()
        {       //checks the game's winning status, where the number of mines is removed from all keys
            int counter = 0;
            foreach (var button in buttons)
            {
                if (button.Name == ".")
                {
                    counter++;
                    if (counter == 64 - mineIndices.Count)
                    {
                        GameOver();
                        MessageBox.Show($"You Win \nCongratulations\nYour Score : {score}");
                        Application.Restart();
                        break;
                    }
                }
            }
        }
        private void LookButtons(int row, int col)
        {       // It is used to open the buttons around all buttons where the number of mines is 0.
            for (int x = row - 1; x <= row + 1; x++)
            {
                for (int y = col - 1; y <= col + 1; y++)
                {
                    if (x >= 0 && x < 8 && y >= 0 && y < 8)
                    {
                        if (!buttons[x, y].Name.StartsWith("X") && !buttons[x, y].Name.StartsWith("."))
                        {
                            if (!buttons[x, y].Name.StartsWith("E"))
                            {
                                buttons[x, y].Text = buttons[x, y].Name;
                                buttons[x, y].Name = ".";
                                buttons[x, y].BackColor = Color.Salmon;
                                score += 1;

                            }
                            else
                            {
                                score += 1;
                                buttons[x, y].Name = ".";
                                buttons[x, y].BackColor = Color.White;
                                LookButtons(x, y);
                            }
                        }
                    }
                }
            }
        }

        private void FindButtonName(Button button, out int row, out int col)
        {       //reveal the name of the clicked button
            row = -1;
            col = -1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (buttons[i, j] == button)
                    {
                        row = i;
                        col = j;
                        return;
                    }
                }
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {               //controls the resulting buttons as follows X(field) or E(emply) or mine area

            if (sender is Button button)
            {               //There are 4 states  = 1. may button 2. Empty buttons 3.previously checked button 4.button showing the number of mines
                if (button.Name.StartsWith("X"))
                {
                    button.Text = "💣";
                    button.Font = new Font("Segoe UI Emoji", 12);
                    button.BackColor = Color.Red;
                    GameOver();
                    MessageBox.Show($"GAMEOVER\nYour Score : {score}");
                    Application.Restart();
                }
                else if (button.Name.StartsWith("E"))
                {
                    button.BackColor = Color.White;
                    button.Name = ".";
                    //button.Text = "";
                    FindButtonName(button, out int row, out int col);
                    LookButtons(row, col);
                    score += 1;
                }
                else if (button.Name == ".")
                {
                    //pass
                }
                else
                {
                    score += 1;
                    button.Text = button.Name;
                    button.BackColor = Color.Salmon;
                    button.Name = ".";
                }
            }
            Won();                          //Function that checks if the game is finished if this is true you win
        }

        private void Button_MouseClick(object sender, MouseEventArgs e)
        {                                            // right click flag       
            if (e.Button == MouseButtons.Right)
            {
                if (sender is Button button && button.Name != ".")
                {
                    button.Text = button.Text == "🚩" ? "" : "🚩";
                    button.BackColor = button.Text == "🚩" ? Color.DarkGray : Color.LightGray;
                }
            }
        }
    }
}

