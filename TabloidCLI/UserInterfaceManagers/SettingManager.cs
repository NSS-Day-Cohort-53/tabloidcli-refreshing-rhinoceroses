using System;
using System.Collections.Generic;

namespace TabloidCLI.UserInterfaceManagers
{
    public class SettingManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        
        public SettingManager(IUserInterfaceManager parentUI)
        {
            _parentUI = parentUI;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Settings Menu");
            Console.WriteLine(" 1) Background Color");
            Console.WriteLine(" 2) Text Color");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    BackgroundColor();
                    return this;
                case "2":
                    ForegroundColor();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Option");
                    return this;
            }
        }
        
        private ConsoleColor ChooseColor()
        {
            Console.WriteLine("Please choose a color: ");
            ConsoleColor[] colors = (ConsoleColor[]) ConsoleColor.GetValues(typeof(ConsoleColor));

            for (int i = 0; i < colors.Length; i++)
            {
                Console.WriteLine($" {i + 1}) {colors[i]}");
            }

            while (true)
            {
            Console.Write("> ");
            string input = Console.ReadLine();
                try
                {
                    int choice = int.Parse(input);
                    return colors[choice - 1];
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid Selection");
                }
            }
        }

        private void BackgroundColor()
        {   
            ConsoleColor newBackgroundColor = ChooseColor();
            //catch that we dont have text and background be the same color
            if (newBackgroundColor == Console.ForegroundColor)
            {
                Console.WriteLine("Text and Background are not allowed to be set to the same color.");
            }
            else
            {
                Console.BackgroundColor = newBackgroundColor;
            }
        }

        private void ForegroundColor()
        {
            ConsoleColor newForegroundColor = ChooseColor();
            if (newForegroundColor == Console.BackgroundColor)
            {
                Console.WriteLine("Text and Backgrounda re not allowed to be set to the same color.");
            }
            else
            {
                Console.ForegroundColor = newForegroundColor;
            }
        }
    }
}
