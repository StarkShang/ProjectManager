using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFinder.Manager
{
    public class InteractiveManager
    {
        public static int QueryUserForSelection<T>(IEnumerable<T> collection,Func<T, string> func)
        {
            var errorInfo = new StringBuilder("Error: Multiple alternatives are matched!\n");
            var index = 1;
            foreach (var row in collection)
                errorInfo.AppendLine($"  {index++} - " + func(row));
            errorInfo.AppendLine($"  0 - exit\n");
            errorInfo.Append("Please input index to continue: ");

            Console.Write(errorInfo);
            return ReadUserInput(index - 1);
        }

        private static int ReadUserInput(int upperBound)
        {
            try
            {
                var selection  = int.Parse(Console.ReadLine());
                if (selection < 0 || selection > upperBound) throw new Exception();
                return selection;                
            }
            catch 
            {
                System.Console.Write("Invalid input. Please try again: ");
                return ReadUserInput(upperBound);
            }
        }
    }
}