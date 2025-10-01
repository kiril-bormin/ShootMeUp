namespace ShootMeUp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Cr�ation de la flotte de ships
            List<Player> fleet = new List<Player>();
            fleet.Add(new Player(AirSpace.WIDTH / 2 - 40, 880, "Joe"));

            List<Enemy> enemy = new List<Enemy>();
            enemy.Add(new Enemy(AirSpace.WIDTH / 2 - 40, 100, "Test"));

            // D�marrage
            Application.Run(new AirSpace(fleet, enemy));
        }
    }
}