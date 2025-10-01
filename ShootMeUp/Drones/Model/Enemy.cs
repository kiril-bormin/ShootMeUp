using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp
{
    // Cette partie de la classe ship définit ce qu'est un ship par un modèle numérique
    public partial class Enemy
    {
        private string _name;                           // Un nom
        private int _x;                                 // Position en X depuis la gauche de l'espace aérien
        private int _y;                                 // Position en Y depuis le haut de l'espace aérien
        private int _move;
        private const int HEIGHT = 79;
        private const int WIDTH = 62;

        // Constructeur
        public Enemy(int x, int y, string name)
        {
            Move = 0;
            _x = x;
            _y = y;
            _name = name;
        }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public void setX(int x) { _x = x; }
        public void setY(int y) { _y = y; }
        public string Name { get { return _name; } }

        public int Move { get => _move; set => _move = value; }

        // Cette méthode calcule le nouvel état dans lequel le ship se trouve après
        // que 'interval' millisecondes se sont écoulées
        public void Update(int interval)
        {
            _x += GlobalHelpers.alea.Next(-1, 2);       // Il s'est déplacé d'une valeur aléatoire vers le haut ou le bas
            _y += 4;
        }
    }
}

