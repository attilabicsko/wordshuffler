using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace WordShufflerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 5;

            var w = new WordShuffler.WordShuffler("Res/wordsEn.txt", size);

            var esc = false;

            while (!esc)
            {
                Console.WriteLine("Press a key for new shuffle!");
                var key = Console.ReadKey();
                w.GetNextModel();
                WriteTest(w, size);

                if (key.Key == ConsoleKey.Escape)
                {
                    esc = true;
                }
            }
        }


        public static void WriteTest(WordShuffler.WordShuffler w, int size)
        {
            var model = w.GetNextModel();

            Console.WriteLine("-------------RESULT--------------\n");

            var matrix = model.Matrix.GetMatrix();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(" [" + matrix[i, j] + "] ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("--------------------------------------\n");

            foreach (var word in model.MatrixWords)
            {

                var pathString = "";

                foreach(var charCoordinate in word.Path)
                {
                    pathString += charCoordinate.ToString() + ", ";
                }
                pathString = pathString.Substring(0, pathString.Length - 2);


                Console.WriteLine(word.Word + "|" +pathString);
            }
            Console.WriteLine("--------------------------------------\n");




        }


    }
}
