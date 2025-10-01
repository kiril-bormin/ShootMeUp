using ShootMeUp.Helpers;
using ShootMeUp.Properties;

namespace ShootMeUp
{
    // Cette partie de la classe Ship définit comment on peut voir un vaisseau

    public partial class Player
    {

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            if (Move == 4) 
            {
                drawingSpace.Graphics.DrawImage(Resources.ship_right, X, Y, WIDTH - 1, HEIGHT - 5);//largeur, hauteur 47, 56 (taille compatible avec ship)
            }
            else if (Move == -4)
            {
                drawingSpace.Graphics.DrawImage(Resources.ship_left, X, Y, WIDTH - 1, HEIGHT - 5);//37, 44 valeurs par défaut (taille d'image en pixels / 10)
            }
            else
            {
                drawingSpace.Graphics.DrawImage(Resources.ship, X, Y, WIDTH, HEIGHT);//48, 61 valeurs par défaut, multiplié par 1,3 pour avoir un bon taille
            }
            drawingSpace.Graphics.DrawString(_name, TextHelpers.drawFont, TextHelpers.writingBrush, X + 30, Y - 25);
        }

        // De manière textuelle
        public override string ToString()
        {
            return $"{Name} ({((int)((double)_chargesnow / CHARGES * 100)).ToString()}%)";
        }

    }
}
