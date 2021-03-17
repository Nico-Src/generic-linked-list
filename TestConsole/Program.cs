using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OwnCollections;

namespace TestConsole
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]

        // Get Current Console
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        // Method to show Window
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Codes for different Window States
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        static void Main(string[] args)
        {
            string Title = "Linked List (C) Nico Thuniot 2021";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=',Title.Length));
            // Set Console Boundaries
            Console.SetWindowSize(213, 52);
            Console.SetBufferSize(1680, 1050);
            // Maximize Console
            ShowWindow(ThisConsole, MAXIMIZE);

            MyList<string> myList = new MyList<string>();

            myList.Add("Nico");
            myList.Add("Nino");
            myList.Add("Alex");

            Title = "\nPrintToConsole - Test:";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=', Title.Length));
            myList.PrintToConsole();
            Console.WriteLine($"\nList-Count: {myList.Count}");
            Console.WriteLine($"\nList-Head: {myList.Head.Value.ToString()}");
            Console.WriteLine($"List-Tail: {myList.Tail.Value.ToString()}\n");

            Title = "PrintWithConnections - Test:";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=', Title.Length));
            myList.PrintWithConnectionsToConsole();

            Title = $"\n\nForeach - Test:";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=', Title.Length));
            foreach(var item in myList)
            {
                Console.WriteLine(item);
            }

            Title = $"\n\nClear - Test:";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=', Title.Length));
            myList.Clear();
            myList.PrintToConsole();

            myList.Add("Nico");
            myList.Add("Nino");
            myList.Add("Alex");

            Title = $"\n\nIndexer - Test: (Changing the Values of the 2. and the 3. Node)";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=', Title.Length));
            Console.WriteLine($"Before: ");

            myList.PrintToConsole();

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
            Console.WriteLine();

            myList[1] = "Zeqiraj";
            myList[2] = "Julian";

            Console.WriteLine($"\nAfter: ");

            myList.PrintToConsole();

            Console.WriteLine($"Contains 'Julian': {myList.Contains("Julian")}");

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
            Console.WriteLine();

            Title = $"\n\nRemoveAt - Test: (Removing The 2. Value)";
            Console.WriteLine(Title);
            Console.WriteLine(new String('=', Title.Length));
            Console.WriteLine($"Before: ");

            myList.PrintToConsole();

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
            Console.WriteLine();

            var result = myList.RemoveAt(1);

            Console.WriteLine($"\nAfter: ");

            myList.PrintToConsole();

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
            Console.WriteLine();
            Console.WriteLine($"\nRemoved Successful: {result.removed}, Removed Node: {result.removedNode.Value}");

            result = myList.RemoveAt(1);

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
            Console.WriteLine();
            Console.WriteLine($"\nRemoved Successful: {result.removed}, Removed Node: {result.removedNode.Value}");

            result = myList.RemoveAt(0);

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
            Console.WriteLine();
            Console.WriteLine($"\nRemoved Successful: {result.removed}, Removed Node: {result.removedNode.Value}");

            //ExecuteTasks(myList);

            Console.ReadKey();
        }

        public static async void ExecuteTasks(MyList<string> myList)
        {
            string[] names1 = new string[] { "Sebastian", "Alexander" };
            string[] names2 = new string[] { "Schindler", "Rezny" };
            string[] names3 = new string[] { "Jakupociv", "Thuniot" };

            Task task1 = new Task(() => myList.AddRange(names1));
            Task task2 = new Task(() => myList.AddRange(names2));
            Task task3 = new Task(() => myList.AddRange(names3));
            Task task4 = new Task(() => GetAndWriteNode(myList,0));
            Task task5 = new Task(() => GetAndWriteNode(myList,1));
            Task task6 = new Task(() => GetAndWriteNode(myList,2));

            await Task.Delay(2000);

            task1.Start();
            task2.Start();
            task3.Start();

            task4.Start();
            task5.Start();
            task6.Start();

            await Task.WhenAll(task1, task2, task3, task4, task5, task6);

            Console.WriteLine();
            myList.PrintWithConnectionsToConsole();
        }

        public static void GetAndWriteNode(MyList<string> myList, int pos)
        {
            var node = myList.GetNodeAt(pos);
            Console.WriteLine(node.ToString());
        }
    }
}
