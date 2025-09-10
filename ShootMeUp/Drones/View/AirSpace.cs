namespace Scramble
{
    // La classe AirSpace repr�sente le territoire au dessus duquel les vaisseau peuvent voler
    // Il s'agit d'un formulaire (une fen�tre) qui montre une vue 2D depuis le cot�

    public partial class AirSpace : Form
    {
        public static readonly int WIDTH = 600;        // Dimensions of the airspace
        public static readonly int HEIGHT = 1000;

        // La flotte est l'ensemble des ships qui �voluent dans notre espace a�rien
        private List<Joueur> fleet;

        BufferedGraphicsContext currentContext;
        BufferedGraphics airspace;
        int[] ground = new int[WIDTH / 10+1];
        Brush groundBrush = new SolidBrush(Color.DarkGreen);
        int scrollSmoother = 0;

        // Initialisation de l'espace a�rien avec un certain nombre de ships
        public AirSpace(List<Joueur> fleet)
        {
            InitializeComponent();
            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;
            // Creates a BufferedGraphics instance associated with this form, and with
            // dimensions the same size as the drawing surface of the form.
            airspace = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            this.fleet = fleet;
            ground[0] = HEIGHT / 5;
            for (int i = 1; i < ground.Length; i++)
            {
                ground[i] = ground[i-1] + GlobalHelpers.alea.Next(0, 7)-3;
            }
            ClientSize = new Size(WIDTH, HEIGHT);
        }

        // Affichage de la situation actuelle
        private void Render()
        {
            airspace.Graphics.Clear(Color.AliceBlue);

            // draw ships
            foreach (Joueur ship in fleet)
            {
                ship.Render(airspace);
            }
            for (int i = 0; i < ground.Length; i++)
            {
                airspace.Graphics.FillRectangle(groundBrush, new Rectangle(i * 10-scrollSmoother, HEIGHT - ground[i], 10, ground[i]));
            }
            scrollSmoother = (scrollSmoother + 5) % 10;
            if (scrollSmoother == 0)
            {
                for (int i = 1;i < ground.Length; i++) ground[i-1] = ground[i];
                ground[ground.Length - 1] = ground[ground.Length - 2] + GlobalHelpers.alea.Next(0, 7) - 3;
            }

            airspace.Render();
        }

        // Calcul du nouvel �tat apr�s que 'interval' millisecondes se sont �coul�es
        private void Update(int interval)
        {
            foreach (Joueur ship in fleet)
            {
                ship.Update(interval);
            }
        }

        // M�thode appel�e � chaque frame
        private void NewFrame(object sender, EventArgs e)
        {
            this.Update(ticker.Interval);
            this.Render();
        }
    }
}