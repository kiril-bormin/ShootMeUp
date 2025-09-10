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
            drawingSpace.Graphics.DrawImage(Resources.ship, X,Y,100,40);
            drawingSpace.Graphics.DrawString("Vous", TextHelpers.drawFont, TextHelpers.writingBrush, X + 30, Y - 25);
        }

        // De manière textuelle
        public override string ToString()
        {
            return $"{Name} ({((int)((double)_tanklevel / FULLTANK * 100)).ToString()}%)";
        }

    }
}
