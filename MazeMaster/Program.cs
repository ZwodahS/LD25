using System;

namespace MazeMaster
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MazeMaster game = new MazeMaster())
            {
                game.Run();
            }
        }
    }
#endif
}

