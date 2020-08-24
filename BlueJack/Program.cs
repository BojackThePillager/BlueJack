using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace BlueJack
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Author: Brad Richardson
             * Title: BlueJack version 0.1
             * Simple and safe tool for simulating ransomware attack on Windows for Red Team simulations
             * Or you could have fun with Blue Team by changing their desktop images ;)
             * Note: The user won't see the new desktop you give them until they log in next
            */
            Console.WriteLine("This will change the desktop background when the user logs in next time...\n");

            Console.WriteLine("Is the target image \n 1.) on disk \n 2.) web URL ?");
            int myChoice =  Int32.Parse(Console.ReadLine());

            if (myChoice == 1)
            {
                string theFile = getLocalFile();
                getCurrentRegistryValue();
                writeRegistryValue(theFile);
            }
            else
            {
                getCurrentRegistryValue();
                getWebAddress();
            }

            //Get file locally
            static string getLocalFile()
            {
                Console.WriteLine("What's the full path to the image file: ?");
                string myPathfile = Console.ReadLine();
                Console.WriteLine("\n");
                return myPathfile;
            }

            //Get file from web
            static void getWebAddress()
            {
                Console.WriteLine("What is the web address of the image file");
                string webURL = Console.ReadLine();
                getImage(webURL);
                string myPathfile = "c:\\windows\\temp\\myImage.jpg";
                writeRegistryValue(myPathfile);
            }

            //method to grab file from web as web client and save locally
            static void getImage(string webAddress)
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(webAddress, @"c:\windows\temp\myImage.jpg");
            }

            static void linePrinter()
            {
                Console.WriteLine("------------------------------");
            }

            static void getCurrentRegistryValue()
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("WallPaper");
                        Console.WriteLine("Current Registry Value");
                        linePrinter();
                        Console.WriteLine(o.ToString());
                    }
                }
            }

            static void writeRegistryValue(string myPathfile)
            {
                if (File.Exists(myPathfile))
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("Updated Value");
                        key.SetValue("WallPaper", myPathfile);
                        linePrinter();
                        Object o = key.GetValue("WallPaper");
                        Console.WriteLine(o.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Image file doesn't exist...Skipping");
                }

            }
        }
    }
}