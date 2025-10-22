using ShootMeUp;
using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ShootMeUp
{
    // La classe AirSpace représente le territoire au dessus duquel les vaisseau peuvent voler
    // Il s'agit d'un formulaire (une fenêtre) qui montre une vue 2D depuis le coté

    public partial class AirSpace : Form
    {
        public static readonly int WIDTH = 600;        // Dimensions of the airspace
        public static readonly int HEIGHT = 1000;

        // La flotte est l'ensemble des ships qui évoluent dans notre espace aérien
        private List<Player> fleet;
        private List<Enemy> enemy;
        private List<Missile> missile;
        private int counter = 0;

        BufferedGraphicsContext currentContext;
        BufferedGraphics airspace;
        int[] ground = new int[WIDTH / 10+1];
        Brush groundBrush = new SolidBrush(Color.Blue);
        int scrollSmoother = 0;

        // Initialisation de l'espace aérien avec un certain nombre de ships
        public AirSpace(List<Player> fleet, List<Enemy> enemy, List<Missile> missile)
        {
            InitializeComponent();
            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;
            // Creates a BufferedGraphics instance associated with this form, and with
            // dimensions the same size as the drawing surface of the form.
            airspace = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            ground[0] = HEIGHT / 5;

            //for (int i = 1; i < ground.Length; i++)
            //{
            //    ground[i] = ground[i-1] + GlobalHelpers.alea.Next(0, 7)-3;
            //}
            ClientSize = new Size(WIDTH, HEIGHT);
            InitializeComponent();

            this.KeyPreview = true; // Ensures the form captures key events before child controls
            this.KeyDown += Form1_KeyDown;
            this.fleet = fleet;
            this.missile = missile;
            this.enemy = enemy;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Player ship in fleet)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                    case Keys.Left:
                        ship.Move = -4;
                        break;
                    case Keys.D:
                    case Keys.Right:
                        ship.Move = 4;
                        break;
                    case Keys.W:
                    case Keys.Up:
                        ship.Move = 0;
                        break;
                    case Keys.Space:
                        if (ship.Chargesnow >= 1)
                        {
                             missile.Add(new Missile(ship.X + 24, ship.Y));
                            ship.Chargesnow--;
                        }
                        else
                        {

                        }
                        break;
                    case Keys.Escape:
                        this.Close();
                        break;
                }
            }
        }
        // Affichage de la situation actuelle
        private void Render()
        {
            //fond du jeu
            //airspace.Graphics.Clear(Color.AliceBlue);
            airspace.Graphics.DrawImage(Resources.fond, 0, 0, WIDTH, HEIGHT);

            // draw ships
            foreach (Player ship in fleet)
            {
                ship.Render(airspace);
            }
            //foreach (Enemy enemy_ship in enemy)
            //{
            //    enemy_ship.Render(airspace);
            //}
            for (int i = enemy.Count - 1; i >= 0; i--)// Boucle pour appeler le methode Update dans la classe Enemy, et supprimer si hors écran
            {
                if (enemy[i].Update(32))
                {
                    enemy.RemoveAt(i);
                }
                else
                {
                    enemy[i].Render(airspace);
                }
            }
            for (int i = missile.Count - 1; i >= 0; i--) // Boucle pour appeler le methode Update dans la classe Missile, et supprimer si hors écran
            {
                if (missile[i].Update(32)) 
                {
                    missile.RemoveAt(i);
                }
                else
                {
                    missile[i].Render(airspace);
                }
            }

            Interface(airspace);


            //for (int i = 0; i < ground.Length; i++)
            //{
            //    airspace.Graphics.FillRectangle(groundBrush, new Rectangle(i * 10-scrollSmoother, HEIGHT - ground[i], 10, ground[i]));
            //}
            //scrollSmoother = (scrollSmoother + 5) % 10;
            //if (scrollSmoother == 0)
            //{
            //    for (int i = 1;i < ground.Length; i++) ground[i-1] = ground[i];
            //    ground[ground.Length - 1] = ground[ground.Length - 2] + GlobalHelpers.alea.Next(0, 7) - 3;
            //}
            airspace.Render();
        }

        // Calcul du nouvel état après que 'interval' millisecondes se sont écoulées
        private void Update(int interval)
        {
            foreach (Player ship in fleet)
            {
                ship.Update(interval);
                if (counter % 50 == 0)
                {
                    if (ship.Chargesnow >= 5)
                    { 
                    
                    }
                    else
                    {
                        ship.Chargesnow++;
                    }
                }
            }
            foreach (Enemy enemy_ship in enemy)
            {
                enemy_ship.Update(interval);
            }
            foreach (Missile missile in missile)
            {
                missile.Update(interval);
            }

        }
        private void Interface(BufferedGraphics drawingSpace)
        {
            foreach (Player ship in fleet)
            {
                drawingSpace.Graphics.DrawString("Charge des missiles : " + ship.Chargesnow, TextHelpers.drawFont, TextHelpers.writingBrush, 25, 35);
            }
        }
        // Méthode appelée à chaque frame
        private void NewFrame(object sender, EventArgs e)
        {
            counter++;
            this.Update(ticker.Interval);
            this.Render();

        }
    }
}