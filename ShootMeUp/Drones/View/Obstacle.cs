using ShootMeUp.Helpers;
using ShootMeUp.Properties;

namespace ShootMeUp
{
    public partial class Obstacle
    {
        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.enemy_ship, X, Y, WIDTH, HEIGHT);//48, 61 valeurs par défaut, multiplié par 1,3 pour avoir un bon taille
            drawingSpace.Graphics.DrawString(Name + " HP: " + _hp, TextHelpers.drawFont, TextHelpers.writingBrush, X - 20, Y - 25);
        }
    }
}
