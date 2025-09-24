using Scramble.Helpers;
using Scramble.Properties;

namespace Scramble
{
    // Cette partie de la classe Ship définit comment on peut voir un vaisseau

    public partial class Joueur
    {

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            if (Move == 3) 
            {
                drawingSpace.Graphics.DrawImage(Resources.ship_right, X, Y, 48, 57);//largeur, hauteur 47, 56 (taille compatible avec ship)
            }
            else if (Move == -3)
            {
                drawingSpace.Graphics.DrawImage(Resources.ship_left, X, Y, 37, 44);//37, 44 valeurs par défaut (taille d'image en pixels / 10)
            }
            else
            {
                drawingSpace.Graphics.DrawImage(Resources.ship, X, Y, 48, 61);//48, 61 valeurs par défaut, multiplié par 1,3 pour avoir un bon taille
            }
            drawingSpace.Graphics.DrawString(_name, TextHelpers.drawFont, TextHelpers.writingBrush, X + 30, Y - 25);
        }

        // De manière textuelle
        public override string ToString()
        {
            return $"{Name} ({((int)((double)_tanklevel / FULLTANK * 100)).ToString()}%)";
        }

    }
}
