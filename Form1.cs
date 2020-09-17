//////////////////////////////////////////////////////
/// 
/// Datei: SnakeGame
/// Datum: 25.04.2019
/// Ersteller: Nicole Schmidlin
/// Version: 1.7
/// Änderungen: Layout Anpassung
/// Beschreibung: Snake Game
/// 
//////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Cirlce> Snake = new List<Cirlce>(); // creating list array for the snake
        private Cirlce food = new Cirlce(); // creating a single circle class called food

        public Form1()
        {
            InitializeComponent();


            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += updateScreen;
            gameTimer.Start(); // starting the timer

            startGame();
        }

        private void updateScreen(object sender, EventArgs e)
        {
            // each tick will run this function

            if(Settings.GameOver == true)
            {

                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }

            }

            else
            {
                if (Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }

                movePlayer(); //run move player function

            }

            pbCanvas.Invalidate();


        }

        private void movePlayer()
        {
            // main loop for the snake head and parts
            for(int i = Snake.Count -1; i >= 0; i--)
            {
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    // restric the snake from leaving the canvas
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if(
                        Snake[i].X < 0 || Snake[i].Y < 0 || 
                        Snake[i].X > maxXpos || Snake[i].Y > maxYpos
                        )
                    {
                        die();
                    }

                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }

                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                    }

                }
                else
                {
                    // if there are no collisions, we continue moving
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }


            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (Settings.GameOver == false)
            {
                Brush snakeColor;

                for(int i = 0; i < Snake.Count; i++)
                {
                    if(i == 0)
                    {
                        snakeColor = Brushes.Black;
                    }
                    else
                    {
                        snakeColor = Brushes.Green;
                    }

                    canvas.FillEllipse(snakeColor,
                                        new Rectangle(
                                            Snake[i].X * Settings.Width,
                                            Snake[i].Y * Settings.Height,
                                            Settings.Width, Settings.Height
                                            ));

                    canvas.FillEllipse(Brushes.Red,
                                        new Rectangle(
                                        food.X * Settings.Width,
                                        food.Y * Settings.Height,
                                        Settings.Width, Settings.Height
                                        ));


                }

            }
            else
            {
                string gameOver = "Game Over \n" + "Final Score is: " + Settings.Score + "\nPress enter to Restart \n";
                label3.Text = gameOver;
                label3.Visible = true;
            }

        }

        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Cirlce head = new Cirlce { X = 10, Y = 5 };
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();

            generateFood();
        }

        private void generateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;

            Random rnd = new Random();
            food = new Cirlce { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
        }

        private void eat()
        {
            Cirlce body = new Cirlce
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y,
            };

            Snake.Add(body);
            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            generateFood();
        }

        private void die()
        {
            Settings.GameOver = true;
        }

    }
}
