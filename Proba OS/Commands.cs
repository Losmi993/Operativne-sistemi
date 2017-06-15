using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Proba_OS
{
    class Commandss
    {
        public void exeptionMistakes(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
        }

        public void wrongArgs(string input)
        {
            Console.WriteLine("'{0}'is not recognized as an internal or external command", input);
            Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
        }
        
        public void mistakes(string splits, int numberOfArguments)
        {
            if (numberOfArguments == 1)
            {
                Console.WriteLine("You should enter 1 more string to call command {0},type help for more info !", splits);
                Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
            }
            else if (numberOfArguments > 1)
            {
                Console.WriteLine("You should enter 2 more strings to call command {0},type help for more info", splits);
                Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
            }
        }

        public void HelpComandsFromFile(string splits, string AppDir)
        {

            string text = File.ReadAllText(Directory.GetParent(AppDir).ToString() + "\\HelpFiles\\" + splits + ".txt");
            Console.WriteLine(text);

            string home2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Directory.SetCurrentDirectory(home2);
            Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
        }

        public void commands(string AppDir)
        {
            WalkingDirectory di = new WalkingDirectory();
            for (; true;)
            {
                string input = Console.ReadLine();
                string[] split = input.Split(' ');
                
                //CD
                if (split.Length == 1 && split[0] == "cd")
                {
                    Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                }
                try
                {
                    if (split.Length == 2 && split[0] == "cd" && split[1] == "..")
                    {
                        string mr = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
                        Directory.SetCurrentDirectory(mr);
                        Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                    }

                }
                catch (NullReferenceException e) { exeptionMistakes(e); }

                try
                {
                    if (split.Length == 2 && split[0] == "cd" && split[1] != "..")
                    {
                        Directory.SetCurrentDirectory(@split[1]);
                        Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                    }
                }
                catch (DirectoryNotFoundException e) { exeptionMistakes(e); }
                catch (UnauthorizedAccessException e) { exeptionMistakes(e); }

                if (split.Length > 2 && split[0] == "cd" && split[1] != "..") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "cd" && split[1] == "..") { wrongArgs(input); }
                //CD


                //CLR
                if (split.Length == 1 && split[0] == "clr")
                {
                    Console.Clear();
                    Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                }
                if (split.Length > 1 && split[0] == "clr") { wrongArgs(input); }
                //CLR


                //CP
                if (split.Length == 3 && split[0] == "cp")
                {
                     string sourceFile = @split[1];
                     string destinationDirectory = @split[2];
                     string destFile = Path.Combine(destinationDirectory, sourceFile);
                     try
                     {
                         if (!Directory.Exists(destinationDirectory))
                         {
                             Directory.CreateDirectory(destinationDirectory);
                         }
                         File.Copy(sourceFile, destFile);
                         Console.WriteLine("Succesfully copied.");
                         Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                     }
                     catch (FileNotFoundException e) { exeptionMistakes(e); }
                     catch (IOException e) { exeptionMistakes(e); }
                     catch (UnauthorizedAccessException e) { exeptionMistakes(e); }
                }
                if (split.Length == 2 && split[0] == "cp") { mistakes(split[0], 1); }
                if (split.Length == 1 && split[0] == "cp") { mistakes(split[0], 2); }
                if (split.Length > 3 && split[0] == "cp") { wrongArgs(input); }
                //CP
                 

                //DELETED
                if (split.Length == 2 && split[0] == "rm")
                {
                    if (File.Exists(@split[1]))
                    {
                        try
                        {
                            File.Delete(@split[1]);
                            Console.Write("Successfully deleted \t" + @split[1] + "\t");
                        }
                        catch (IOException e) { exeptionMistakes(e); }
                        Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                    }
                    else
                    {
                        Console.WriteLine(@split[1] + " does not exists \t");
                        Console.Write(Directory.GetCurrentDirectory() + "> ");
                    }
                }
                if (split.Length == 1 && split[0] == "rm") { mistakes(split[0], 1); }
                if (split.Length > 2 && split[0] == "rm") { wrongArgs(input); }
                //DELETED


                //LS
                if (split.Length == 1 && split[0] == "ls")
                {
                    di.DirSearch(Directory.GetCurrentDirectory());
                }
                if (split.Length == 2 && split[0] == "ls")
                {
                    di.DirSearch(@split[1]);
                }
                if (split.Length > 2 && split[0] == "ls") { wrongArgs(input); }
                //LS
                

                //LSDISK
                if (split.Length == 1 && split[0] == "lsdisk")
                {
                    string s = "Drive   Free Space  TotalSpace  FileSystem   %Free Space    DriveType\n\r========================================================================================\n\r";
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        double ts = 0;
                        double fs = 0;
                        double frprcntg = 0;
                        long divts = 1024 * 1024 * 1024;
                        long divfs = 1024 * 1024 * 1024;
                        string tsunit = "GB";
                        string fsunit = "GB";
                        if (drive.IsReady)
                        {
                            fs = drive.TotalFreeSpace;
                            ts = drive.TotalSize;
                            frprcntg = (fs / ts) * 100;
                            if (drive.TotalSize < 1024) { divts = 1; tsunit = "Byte(s)"; }
                            else if (drive.TotalSize < (1024 * 1024))
                            { divts = 1024; tsunit = "KB"; }
                            else if (drive.TotalSize < (1024 * 1024 * 1024))
                            { divts = 1024 * 1024; tsunit = "MB"; }
                            //----------------------
                            if (drive.TotalFreeSpace < 1024)
                            { divfs = 1; fsunit = "Byte(s)"; }
                            else if (drive.TotalFreeSpace < (1024 * 1024))
                            { divfs = 1024; fsunit = "KB"; }
                            else if (drive.TotalFreeSpace < (1024 * 1024 * 1024))
                            { divfs = 1024 * 1024; fsunit = "MB"; }
                            s = s +
                            "[" + drive.Name.Substring(0, 2) +
                            "]" + String.Format("{0,10:0.0}", ((fs / divfs)).ToString("N2")) + fsunit +
                            String.Format("{0,10:0.0}", (ts / divts).ToString("N2")) + tsunit +
                            "\t   " + drive.DriveFormat.ToString() + "\t\t" + frprcntg.ToString("N2") + "%" +
                            "\t     " + drive.DriveType.ToString();

                            s = s + "\n\r";
                        }
                    }
                    Console.WriteLine(s);
                    Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                }
                if (split.Length == 2 && split[0] == "lsdisk")
                {
                    di.DirSearch(@split[1]);
                }
                if (split.Length > 2 && split[0] == "lsdisk") { wrongArgs(input); }
                //LSDISK


                //ECHO
                if (split[0] == "echo")
                {
                    for (int i = 1; i < split.Length; i++)
                    {
                        Console.Write(split[i] + ' ');
                    }
                    Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                }
                if (split.Length == 1 && split[0] == "echo") { mistakes(split[0], 1); }
                //ECHO


                // HELP
                if (split.Length == 1 && split[0] == "help") { HelpComandsFromFile(split[0], AppDir); }

                if (split.Length == 2 && split[0] == "help" && split[1] == "cd") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "clr") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "cp") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "rm") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "ls") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "lsdisk") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "echo") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "mkdir") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "mv") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "quit") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "rmdir") { HelpComandsFromFile(split[1], AppDir); }
                if (split.Length == 2 && split[0] == "help" && split[1] == "time") { HelpComandsFromFile(split[1], AppDir); }

                if (split.Length > 2 && split[0] == "help" && split[1] == "cd") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "clr") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "cp") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "rm") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "ls") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "lsdisk") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "echo") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "mkdir") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "mv") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "quit") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "rmdir") { wrongArgs(input); }
                if (split.Length > 2 && split[0] == "help" && split[1] == "time") { wrongArgs(input); }
                //HELP


                //MKDIR
                if (split.Length == 2 && split[0] == "mkdir")
                {
                    try
                    {
                        string folderName = Directory.GetCurrentDirectory();
                        string pathString = Path.Combine(folderName, split[1]);

                        if (!Directory.Exists(pathString))
                        {
                            Directory.CreateDirectory(pathString);
                            string fileName = Path.GetRandomFileName();
                            pathString = Path.Combine(pathString, fileName);
                            Console.WriteLine("Succesfully created directory: ");
                            Console.Write("{0}> ", Directory.GetCurrentDirectory());
                        }
                        else
                        {
                            Console.WriteLine("Directory {0} already exists: ", pathString);
                            Console.Write("{0}> ", Directory.GetCurrentDirectory());
                        }
                    }
                    catch (IOException e) { exeptionMistakes(e); }
                }
                if (split.Length == 1 && split[0] == "mkdir") { mistakes(split[0], 1); }
                if (split.Length > 2 && split[0] == "mkdir") { wrongArgs(input); }
                //MKDIR


                //MV
                if (split.Length == 3 && split[0] == "mv")
                {
                    string sourceFile = @split[1];
                    string destinationFile = @split[2];
                    string destFile = destinationFile + "\\" + sourceFile;
                    try
                    {
                        File.Copy(sourceFile, destFile);
                        Console.WriteLine("Successfully moved {0} to {1}", sourceFile, destinationFile);
                        Console.Write("{0}> ", Directory.GetCurrentDirectory());
                    }
                    catch (IOException e) { exeptionMistakes(e); }
                }
                if (split.Length == 2 && split[0] == "mv") { mistakes(split[0], 1); }
                if (split.Length == 1 && split[0] == "mv") { mistakes(split[0], 2); }
                if (split.Length > 3 && split[0] == "mv") { wrongArgs(input); }
                //MV


                //QUIT
                if (split.Length == 1 && split[0] == "quit")
                { break; }
                if (split.Length > 1 && split[0] == "quit") { wrongArgs(input); }
                //QUIT


                //RMDIR
                if (split.Length == 2 && split[0] == "rmdir")
                {
                    if (Directory.Exists(@split[1]))
                    {
                        try
                        {
                            Directory.Delete(@split[1]);
                            Console.WriteLine("Successfully deleted {0}", split[1]);
                        }
                        catch (IOException e) { exeptionMistakes(e); }
                    }
                    Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                }
                if (split.Length == 1 && split[0] == "rmdir") { mistakes(split[0], 1); }
                if (split.Length > 2 && split[0] == "rmdir") { wrongArgs(input); }
                //RMDIR


                //TIME
                if (split.Length == 1 && split[0] == "time")
                {
                    DateTime now = DateTime.Now;
                    Console.Write("The current time and date : ");
                    Console.WriteLine(now);
                    Console.Write("\n{0}> ", Directory.GetCurrentDirectory());
                }
                if (split.Length > 1 && split[0] == "time") { wrongArgs(input); }
                //TIME

                /*if (split.Length == 1 )
                {
                    System.Diagnostics.Process.Start(split[0]);
                }
                if (split.Length > 1 ) { wrongArgs(input); }*/


                //ELSE
                else if (split[0] != "cd" && split[0] != "clr" && split[0] != "cp" && split[0] != "rm" && split[0] != "ls" && split[0] != "lsdisk" && split[0] != "echo" && split[0] != "help" && split[0] != "mkdir" && split[0] != "mv" && split[0] != "quit" && split[0] != "rmdir" && split[0] != "time")
                {
                wrongArgs(input);

                }
                 //ELSE  

            
            }
        }

    }
}


