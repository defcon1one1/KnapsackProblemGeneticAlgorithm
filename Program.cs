using System;
using System.Linq;

namespace KnapsackProblemGeneticAlgorithm
{
    class Program
    {
        static void Main()
        {
            Random random = new Random();
            int maxVolume = GetInput("Enter the maximum volume:");
            int minComponentSize = GetInput("Enter the minimum component's size:");
            int maxComponentSize = GetInput("Enter the maximum component's size:") + 1;
            int maxMutations = GetInput("Enter the maximum number of mutations:");

            int populationSize = 100;
            int[] componentSizes = new int[populationSize];
            byte[] solution = new byte[populationSize];

            InitializeComponents(componentSizes, solution, minComponentSize, maxComponentSize, random);

            Console.WriteLine($"\n\nMaximum volume: {maxVolume}\n");
            Console.WriteLine($"Components: {string.Join(" ", componentSizes)}\n");

            int currentVolume = CalculateVolume(componentSizes, solution);
            int greatestComponent = componentSizes.Max();
            int smallestComponent = componentSizes.Min();

            Console.WriteLine($"Greatest component: {greatestComponent}\n");
            Console.WriteLine($"Smallest component: {smallestComponent}\n");
            Console.WriteLine("First random solution:");
            PrintSolution(solution, currentVolume);

            byte[] bestSolution = solution;
            int bestVolume = currentVolume;

            for (int i = 0; i < maxMutations; i++)
            {
                MutateSolution(solution, componentSizes, random);
                currentVolume = CalculateVolume(componentSizes, solution);

                if (currentVolume == maxVolume)
                {
                    maxMutations = i + 1;
                    break;
                }

                if (IsBetterSolution(currentVolume, bestVolume, maxVolume))
                {
                    bestSolution = solution.ToArray();
                    bestVolume = currentVolume;
                }
                else
                {
                    solution = bestSolution.ToArray();
                    currentVolume = bestVolume;
                }
            }

            Console.Write($"\nSolution after {maxMutations} mutations:\n");
            PrintSolution(bestSolution, bestVolume);
        }

        static int GetInput(string message)
        {
            Console.WriteLine(message);
            return int.Parse(Console.ReadLine());
        }

        static void InitializeComponents(int[] componentSizes, byte[] solution, int minComponentSize, int maxComponentSize, Random random)
        {
            for (int i = 0; i < componentSizes.Length; i++)
            {
                componentSizes[i] = random.Next(minComponentSize, maxComponentSize);
                solution[i] = (byte)random.Next(0, 2);
            }
        }

        static int CalculateVolume(int[] componentSizes, byte[] solution)
        {
            int currentVolume = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                if (solution[i] == 1)
                {
                    currentVolume += componentSizes[i];
                }
            }
            return currentVolume;
        }

        static void MutateSolution(byte[] solution, int[] componentSizes, Random random)
        {
            int randomIndex = random.Next(0, solution.Length);
            solution[randomIndex] = (byte)(1 - solution[randomIndex]);
        }

        static bool IsBetterSolution(int currentVolume, int bestVolume, int maxVolume)
        {
            int currentDifference = Math.Abs(currentVolume - maxVolume);
            int bestDifference = Math.Abs(bestVolume - maxVolume);

            return (currentDifference < bestDifference) || (currentDifference == bestDifference && currentVolume < maxVolume);
        }

        static void PrintSolution(byte[] solution, int volume)
        {
            Console.Write(string.Join("", solution));
            Console.WriteLine($" - volume: {volume}\n");
        }
    }
}
