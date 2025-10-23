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

        // Les listes des objets qui sont présents dans l'espace 
        private List<Player> fleet; 
        private List<Enemy> enemy; 
        private List<Missile> missile;
        private List<Obstacle> obstacle;   

        private List<Image> backgroundImages;   // Liste pour les 4 images
        private List<int> backgroundYPositions; // Liste pour les positions des images 

        private int scrollSpeed = 2; // Vitesse de mouvement 
        private int counter = 0; // Le comptoir des frames 

        BufferedGraphicsContext currentContext;
        BufferedGraphics airspace;
        int[] ground = new int[WIDTH / 10+1];
        Brush groundBrush = new SolidBrush(Color.Blue);
        int scrollSmoother = 0;

        public int Counter { get => counter; set => counter = value; }

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
            // Afficher les ennemies
            foreach (Enemy enemy_ship in enemy)
            {
                enemy_ship.Render(airspace);
                Console.WriteLine(enemy_ship.Hp);
            }
            // Afficher les missiles
            foreach (Missile missile in missile)
            {
                missile.Render(airspace);
            }
            // Afficher les obstacles
            foreach (Obstacle obstacle in obstacle)
            {
                obstacle.Render(airspace);
            }
            Interface(airspace);
            airspace.Render();
        }

        // Calcul du nouvel йtat aprиs que 'interval' millisecondes se sont йcoulйes
        private void Update(int interval)
        {
            //Update la position du joueur
            foreach (Player ship in fleet)
            {
                ship.Update(interval);
                // Après 50 "ticks" (env. 5 seconds) ce code verifie si le joueur a 5 missiles en charge, sinon il lui en ajoute un de plus
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
            // Update la position des ennemies 
            for (int i = enemy.Count - 1; i >= 0; i--)
            {
                if (enemy[i].Update(interval)) 
                {
                    enemy.RemoveAt(i);
                }
            }
            // Spawn les ennemies
            if (counter % GlobalHelpers.alea.Next(100, 250) == 0)
            {
                int enemyX = GlobalHelpers.alea.Next(50, AirSpace.WIDTH - Enemy.WIDTH - 50); // X de l'ennemie est un nombre aléatoire entre 50 et 1000 - taille de l'ennemie - 50
                int enemyY = -Enemy.HEIGHT; // Y de l'ennemie => 0 - sa taille 

                // ajouter l'ennemie dans la liste
                enemy.Add(new Enemy(enemyX, enemyY, "F16"));
            }
            // Spawn les ennemies
            if (counter % GlobalHelpers.alea.Next(100, 250) == 0)
            {
                int obstacleX = GlobalHelpers.alea.Next(50, AirSpace.WIDTH - Obstacle.WIDTH - 50); // X de l'obstacle est un nombre aléatoire entre 50 et 1000 - taille de l'obstacle - 50
                int obstacleY = -Obstacle.HEIGHT; // Y de l'obstacle => 0 - sa taille 

                // ajouter l'obstacle dans la liste
                obstacle.Add(new Obstacle(obstacleX, obstacleY, "Tour"));
            }

            // Update la position des missiles 
            for (int i = missile.Count - 1; i >= 0; i--)
            {
                if (missile[i].Update(interval)) 
                {
                    missile.RemoveAt(i);
                }
            }
            // Update la position des obstacles (ils avancent en réalité) 
            for (int i = obstacle.Count - 1; i >= 0; i--)
            {
                if (obstacle[i].Update(interval))
                {
                    obstacle.RemoveAt(i);
                }
            }

            // Update le fond du jeu
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
        /// <summary>
        /// Interface du jeu, il affiche le nombre de missiles en haut à gauche de la fenêtre
        /// </summary>
        /// <param name="drawingSpace"></param>
        private void Interface(BufferedGraphics drawingSpace)
        {
            foreach (Player ship in fleet)
            {
                drawingSpace.Graphics.DrawString("Charge des missiles : " + ship.Chargesnow, TextHelpers.drawFont, TextHelpers.writingBrush, 25, 35);
            }
        }
        /// <summary>
        /// S'applique quand le joueur était vaincu
        /// </summary>
        /// <param name="drawingSpace"></param>
        private void GameOver(BufferedGraphics drawingSpace)
        {
            foreach (Player ship in fleet) {

                drawingSpace.Graphics.DrawImage(Resources.game_over, 25, 35, 380, 84);
            }
        }
        /// <summary>
        /// Verifie si des objet se croisent, et si oui, il les supprimes
        /// </summary>
        private void CheckCollisions()
        {
            // Listes qui contient seulement des éléments uniques à supprimer de la liste principale
            HashSet<Missile> missilesToRemove = new HashSet<Missile>(); 
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
                        e.Hp -= 1; // Soustraire un point de vie
                        if (e.Hp == 0)
                        {
                            enemiesToRemove.Add(e); // Si l'ennemie n'a plus de points de vie, il est supprimé 
                        }
                        missilesToRemove.Add(m); // Le missile est supprimé à chaque fois
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
                        o.Hp -= 1;// Soustraire un point de vie
                        if (o.Hp == 0)
                        {
                            obstaclesToRemove.Add(o);// Si l'obstacle n'a plus de points de vie, il est supprimé 
                        }
                        missilesToRemove.Add(m);// Le missile est supprimé à chaque fois

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
                        enemiesToRemove.Add(e); // Supprime l'ennemie
                        playerToRemove.Add(p);// Supprime le joueur
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
                        obstaclesToRemove.Add(o); // Supprime l'obstacle
                        playerToRemove.Add(p);// Supprime le joueur
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