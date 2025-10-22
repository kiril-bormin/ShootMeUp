using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;

namespace ShootMeUp
{
    public partial class Missile
    {
        private int _x;                                 // Position en X depuis la gauche de l'espace aérien
        private int _y;                                 // Position en Y depuis le haut de l'espace aérien
        private const int HEIGHT = 20;
        private const int WIDTH = 15;
        public Missile(int x, int y)
        {
            _x = x;
            _y = y;
        }
        // Crée un rectangle invisible pour définir la taille de hitbox d'objet 
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, WIDTH, HEIGHT); }
        }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public void setX(int x) { _x = x; }
        public void setY(int y) { _y = y; }

        public bool Update(int interval)
        {
            //_x += GlobalHelpers.alea.Next(-1, 2);       // Il s'est déplacé d'une valeur aléatoire vers le haut ou le bas
            _y -= 8;
            Console.WriteLine(_y);
            return _y <= -HEIGHT;
        }
    }
}
