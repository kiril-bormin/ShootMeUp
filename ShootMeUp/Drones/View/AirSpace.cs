using ShootMeUp;
using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ShootMeUp
{
    // La classe AirSpace reprйsente le territoire au dessus duquel les vaisseau peuvent voler
    // Il s'agit d'un formulaire (une fenкtre) qui montre une vue 2D depuis le cotй

    public partial class AirSpace : Form
    {
        public static readonly int WIDTH = 600;        // Dimensions of the airspace
        public static readonly int HEIGHT = 1000;

        // La flotte est l'ensemble des ships qui йvoluent dans notre espace aйrien
        private List<Player> fleet; 
        private List<Enemy> enemy; 
        private List<Missile> missile;
        private List<Obstacle> obstacle;   

        private List<Image> backgroundImages;   // Liste pour les 4 images
        private List<int> backgroundYPositions; // Liste pour les positions des images 
        private int scrollSpeed = 1; // Vitesse de mouvement 
        private int counter = 0;

        BufferedGraphicsContext currentContext;
        BufferedGraphics airspace;
        int[] ground = new int[WIDTH / 10+1];
        Brush groundBrush = new SolidBrush(Color.Blue);
        int scrollSmoother = 0;

        // Initialisation de l'espace aйrien avec un certain nombre de ships
        public AirSpace(List<Player> fleet, List<Enemy> enemy, List<Missile> missile, List<Obstacle> obstacles)
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

            backgroundImages = new List<Image>();
            backgroundYPositions = new List<int>();

            //Ajouter les fonds dans la liste
            backgroundImages.Add(Properties.Resources.zone1); 
            backgroundImages.Add(Properties.Resources.zone2); 
            backgroundImages.Add(Properties.Resources.zone3); 
            backgroundImages.Add(Properties.Resources.zone4);

            // Placer les l'un sur l'autre
            for (int i = 0; i < backgroundImages.Count; i++)
            {
                backgroundYPositions.Add(i * -HEIGHT);
            }

            this.KeyPreview = true; // Ensures the form captures key events before child controls
            this.KeyDown += Form1_KeyDown;
            this.fleet = fleet;
            this.missile = missile;
            this.enemy = enemy;
            this.obstacle = obstacles;
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
            for (int i = 0; i < backgroundImages.Count; i++)
            {
                airspace.Graphics.DrawImage(backgroundImages[i], 0, backgroundYPositions[i], WIDTH, HEIGHT);
            }
            // draw ships
            foreach (Player ship in fleet)
            {
                ship.Render(airspace);

            }
            //foreach (Enemy enemy_ship in enemy)
            //{
            //    enemy_ship.Render(airspace);
            //}
            foreach (Enemy enemy_ship in enemy)
            {
                enemy_ship.Render(airspace);
                Console.WriteLine(enemy_ship.Hp);
            }

            foreach (Missile missile in missile)
            {
                missile.Render(airspace);
            }
            
            foreach (Obstacle obstacle in obstacle)
            {
                obstacle.Render(airspace);
            }



            //for (int i = enemy.Count - 1; i >= 0; i--)// Boucle pour appeler le methode Update dans la classe Enemy, et supprimer si hors йcran
            //{
            //    if (enemy[i].Update(32))
            //    {
            //        enemy.RemoveAt(i);
            //    }
            //    else
            //    {
            //        enemy[i].Render(airspace);
            //    }
            //}
            //for (int i = missile.Count - 1; i >= 0; i--) // Boucle pour appeler le methode Update dans la classe Missile, et supprimer si hors йcran
            //{
            //    if (missile[i].Update(32)) 
            //    {
            //        missile.RemoveAt(i);
            //    }
            //    else
            //    {
            //        missile[i].Render(airspace);
            //    }
            //}

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

        // Calcul du nouvel йtat aprиs que 'interval' millisecondes se sont йcoulйes
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
            //foreach (Enemy enemy_ship in enemy)
            //{
            //    enemy_ship.Update(interval);
            //}
            //foreach (Missile missile in missile)
            //{
            //    missile.Update(interval);
            //}
            for (int i = enemy.Count - 1; i >= 0; i--)
            {
                if (enemy[i].Update(interval)) 
                {
                    enemy.RemoveAt(i);
                }
            }

            for (int i = missile.Count - 1; i >= 0; i--)
            {
                if (missile[i].Update(interval)) 
                {
                    missile.RemoveAt(i);
                }
            }
            for (int i = obstacle.Count - 1; i >= 0; i--)
            {
                if (obstacle[i].Update(interval))
                {
                    obstacle.RemoveAt(i);
                }
            }
            for (int i = 0; i < backgroundImages.Count; i++)
            {
                // On bouge l'image actuelle vers le bas
                backgroundYPositions[i] += scrollSpeed;

                // Quand l'image quite completement l'ecran, on la déplace vers le haut
                if (backgroundYPositions[i] >= HEIGHT)
                {
                    int topMostY = backgroundYPositions.Min();
                    backgroundYPositions[i] = topMostY - HEIGHT;
                }
            }
        }
        private void Interface(BufferedGraphics drawingSpace)
        {
            foreach (Player ship in fleet)
            {
                drawingSpace.Graphics.DrawString("Charge des missiles : " + ship.Chargesnow, TextHelpers.drawFont, TextHelpers.writingBrush, 25, 35);
            }
        }
        private void GameOver(BufferedGraphics drawingSpace)
        {
            foreach (Player ship in fleet) {

                drawingSpace.Graphics.DrawImage(Resources.game_over, 25, 35, 380, 84);
            }
        }
        private void CheckCollisions()
        {
            HashSet<Missile> missilesToRemove = new HashSet<Missile>(); //Liste qui contient seulement des éléments uniques 
            HashSet<Enemy> enemiesToRemove = new HashSet<Enemy>();  
            HashSet<Player> playerToRemove = new HashSet<Player>();
            HashSet<Obstacle> obstaclesToRemove = new HashSet<Obstacle>();


            // Collision d'un missile avec un avion ennemie
            foreach (Missile m in missile)
            {
                foreach (Enemy e in enemy)
                {
                    if (m.BoundingBox.IntersectsWith(e.BoundingBox)) //Vérification si les deux éléments se croisent 
                    {
                        e.Hp -= 1;
                        if (e.Hp == 0)
                        {
                            enemiesToRemove.Add(e);
                        }
                        missilesToRemove.Add(m);

                    }
                }
            }
            // Collision d'un missile avec un obstacle
            foreach (Missile m in missile)
            {
                foreach (Obstacle o in obstacle)
                {
                    if (m.BoundingBox.IntersectsWith(o.BoundingBox)) //Vérification si les deux éléments se croisent 
                    {
                        o.Hp -= 1;
                        if (o.Hp == 0)
                        {
                            obstaclesToRemove.Add(o);
                        }
                        missilesToRemove.Add(m);

                    }
                }
            }
            // Collision de joueur avec un avion ennemie
            foreach (Player p in fleet)
            {
                foreach (Enemy e in enemy)
                {
                    if (p.BoundingBox.IntersectsWith(e.BoundingBox)) //Vérification si les deux éléments se croisent 
                    {
                        GameOver(airspace);
                        Console.WriteLine("Game Over");
                        enemiesToRemove.Add(e);
                        playerToRemove.Add(p);

                    }
                }
            }
            // Collision de joueur avec un obstacle
            foreach (Player p in fleet)
            {
                foreach (Obstacle o in obstacle)
                {
                    if (p.BoundingBox.IntersectsWith(o.BoundingBox)) //Vérification si les deux éléments se croisent 
                    {
                        GameOver(airspace);
                        Console.WriteLine("Game Over");
                        obstaclesToRemove.Add(o);
                        playerToRemove.Add(p);
                    }
                }
            }

            //Suppresion des éléments qui n'ont plus de vies
            foreach (Missile m in missilesToRemove)
            {
                missile.Remove(m);
            }
            foreach (Enemy e in enemiesToRemove)
            {
                enemy.Remove(e);
            }
            foreach (Player p in playerToRemove)
            {
                fleet.Remove(p);
            }
            foreach (Obstacle o in obstaclesToRemove)
            {
                obstacle.Remove(o);
            }
        }
        // Mйthode appelйe а chaque frame
        private void NewFrame(object sender, EventArgs e)
        {
            counter++;
            this.Update(ticker.Interval);
            this.Render();
            this.CheckCollisions();

        }
    }
}