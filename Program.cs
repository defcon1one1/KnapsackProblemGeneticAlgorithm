using System;
using System.Linq;

namespace KnapsackProblemGeneticAlgorithm
{
    class Program
    {
        static void Main()
        {
            Random gen = new Random(); // object of the class Random for generating pseudo random integers

            int currentVolume;
            Console.WriteLine("Enter the maximum volume:");
            int maxVolume = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the minimum component's size:");
            int minComponent = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the maximum component's size:");
            int maxComponent = int.Parse(Console.ReadLine()) + 1;
            Console.WriteLine("Enter the maximum number of mutations:");
            int m = int.Parse(Console.ReadLine());

            int size = 100; // predefined arrays' size - components and "backpack" arrays MUST be the same size, otherwise OutOfRangeException will occur

            int[] components = new int[size]; 
            byte[] slots = new byte[size];

            Console.WriteLine($"\n\nMaximum volume: {maxVolume}\n");
            Console.WriteLine($"Components:");

            for (int i = 0; i < components.Length; i++)
            {
                components[i] = gen.Next(minComponent, maxComponent); // random maximum component sizes 
                Console.Write(components[i] + " "); // printing the components
                slots[i] = (byte)gen.Next(0, 2); // random bytes 0-1
            }
            Console.WriteLine("\n");

            void addComponents() //calculates the current total components volume
            {
                currentVolume = 0; // zeroing the component sum first
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i] == 1)
                    {
                        currentVolume += components[i];
                    }
                }
            }

            addComponents(); // first solution

            Console.WriteLine($"Greatest component: {components.Max()}\n");
            Console.WriteLine($"Smallest component: {components.Min()}\n");
            Console.WriteLine($"First random solution:");
            for (int i = 0; i < slots.Length; i++)
            {
                Console.Write($"{slots[i]}");
            }
            Console.Write($" - volume: {currentVolume}");
            Console.WriteLine(); 



            byte[] previousSolution = slots; // saving the previous solution, to use again if the new is worse (previousSolution will be "parent" and slots is "child")
            int previousVolume = currentVolume; // saving the previous solution's volume

            void Mutation()
            {
                int randomIndex = gen.Next(0, slots.Length);
                if (slots[randomIndex] == 0)
                    slots[randomIndex] = 1;
                else slots[randomIndex] = 0;
                
                addComponents(); // adding components in the new solution
               
                //checking if the new solution is better or worse
                if (Math.Abs(currentVolume - maxVolume) < Math.Abs(previousVolume - maxVolume)) 
                {
                    previousSolution = slots;
                    previousVolume = currentVolume;
                } else if (Math.Abs(currentVolume - maxVolume) == Math.Abs(previousVolume - maxVolume) && currentVolume < maxVolume) // if the solutions are equally good, choose the one below max volume
                {
                    previousSolution = slots;
                    previousVolume = currentVolume;
                } else
                {
                    slots = previousSolution;
                    currentVolume = previousVolume;
                } 
                

            }

            Console.Write("\n\n");
            

            for (int i = 1; i < m; i++)
            {
                Mutation();
                if (currentVolume == maxVolume) // stop mutating when the optimal solution is reached
                {
                    m = i;
                    break;
                }

            }

            Console.Write($"Solution after {m} mutations:\n");
            for (int i = 0; i < previousSolution.Length; i++) 
            {
                Console.Write($"{previousSolution[i]}");
            }
            Console.Write($" - volume: {previousVolume}\n");
        }
    }
}

