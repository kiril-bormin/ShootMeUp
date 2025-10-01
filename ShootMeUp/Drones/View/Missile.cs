using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp
{
    public partial class Missile
    {
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.missile, X, Y, WIDTH, HEIGHT);//48, 61 valeurs par défaut, multiplié par 1,3 pour avoir un bon taille
        }
    }
}
