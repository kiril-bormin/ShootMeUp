using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ShootMeUp
{
    // Cette partie de la classe ship définit ce qu'est un ship par un modèle numérique
    public partial class Enemy
    {
        private string _name;                           // Un nom
        private int _x;                                 // Position en X depuis la gauche de l'espace aérien
        private int _y;                                 // Position en Y depuis le haut de l'espace aérien
        private int _move;
        public const int HEIGHT = 79;
        public const int WIDTH = 62;
        private int _hp = 3;

        // Constructeur
        public Enemy(int x, int y, string name)
        {
            Move = 0;
            _x = x;
            _y = y;
            _name = name;
        }
        // Crée un rectangle invisible pour définir la taille de hitbox de l'objet 
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, WIDTH, HEIGHT); }
        }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public void setX(int x) { _x = x; }
        public void setY(int y) { _y = y; }
        public string Name { get { return _name; } }

        public int Move { get => _move; set => _move = value; }
        public int Hp { get => _hp; set => _hp = value; }

        // Cette méthode calcule le nouvel état dans lequel le ship se trouve après
        // que 'interval' millisecondes se sont écoulées
        public bool Update(int interval)
        {
            _x += GlobalHelpers.alea.Next(-1, 2);       // Il s'est déplacé d'une valeur aléatoire vers le haut ou le bas
            _y += 4;
            return _y >= AirSpace.HEIGHT + HEIGHT;

        }
    }
}

