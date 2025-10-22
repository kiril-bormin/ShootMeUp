using System.Diagnostics.Metrics;
using System.Windows.Forms;

namespace ShootMeUp
{
    // Cette partie de la classe ship définit ce qu'est un ship par un modèle numérique
    public partial class Player
    {
        public int CHARGES = 5;   // Charge maximale de la batterie
        private int _chargesnow;                            // La charge actuelle de la batterie
        private string _name;                           // Un nom
        private int _x;                                 // Position en X depuis la gauche de l'espace aérien
        private int _y;                                 // Position en Y depuis le haut de l'espace aérien
        private int _move;                              // Permet de définir la vitesse de déplacement de l'avion
        private int _fire;                              //
        private const int HEIGHT = 79;
        private const int WIDTH = 62;
        private int _hp = 5;


        // Constructeur
        public Player(int x, int y, string name)
        {
            Fire = 0;
            Move = 0;
            _x = x;
            _y = y;
            _name = name;
            _chargesnow = CHARGES; // La charge initiale de la batterie est choisie aléatoirement
        }
        // Crée un rectangle invisible pour définir la taille de hitbox d'objet 
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, WIDTH, HEIGHT); }
        }
        public int X { get { return _x;} }
        public int Y { get { return _y;} }
        public void setX(int x) { _x = x; }
        public void setY(int y) { _y = y; }
        public string Name { get { return _name;} }

        public int Move { get => _move; set => _move = value; }
        public int Fire { get => _fire; set => _fire = value; }
        public int Chargesnow { get => _chargesnow; set => _chargesnow = value; }

        // Cette méthode calcule le nouvel état dans lequel le ship se trouve après
        // que 'interval' millisecondes se sont écoulées
        public void Update(int interval)
        {
            _y += GlobalHelpers.alea.Next(-1, 2);       // Il s'est déplacé d'une valeur aléatoire vers le haut ou le bas
            if (_y >= 886)
            {
                _y -=2;
            }
            else if (_y <= 874)
            {
                _y +=2;
            }

            if (_x <= 8)
            {
                Move = 0;
                _x += 2;
            }
            if (_x >= AirSpace.WIDTH - WIDTH - 8)
            {
                Move = 0;
                _x -= 2;
            }
            else
            {
                _x += Move; //déplacement de l'avion en horizontal
            }

        }
    }
}