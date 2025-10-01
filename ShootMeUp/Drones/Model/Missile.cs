using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public void setX(int x) { _x = x; }
        public void setY(int y) { _y = y; }

        public void Update(int interval)
        {
            //_x += GlobalHelpers.alea.Next(-1, 2);       // Il s'est déplacé d'une valeur aléatoire vers le haut ou le bas
            _y -= 7;
        }
    }
}
