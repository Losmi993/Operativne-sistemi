using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace Proba_OS
{
    class TestiranjePrograma
    {
        static void Main(string[] args)
        {
            Console.Title = "Proba_OS";
            Console.ForegroundColor = ConsoleColor.Red;
            

            string AppDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            Directory.SetCurrentDirectory(userProfile);
            Console.Write("\n{0}> ", Directory.GetCurrentDirectory());

            Commandss comi = new Commandss();
            comi.commands(AppDir);
        }
    }
}
