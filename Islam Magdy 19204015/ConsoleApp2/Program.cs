                                    //Name:Islam Magdy Salman
                                   //ID: 19204015

using System;
using System.Linq;

namespace BankersAlgorithm
{
    class Program
    {
        static bool IsSafe(int numProcesses, int numResources, int[] available, int[,] maximum, int[,] allocation)
        {
            int[,] need = new int[numProcesses, numResources];

            for (int i = 0; i < numProcesses; i++)
            {
                for (int j = 0; j < numResources; j++)
                {
                    need[i, j] = maximum[i, j] - allocation[i, j];
                }
            }

            int[] work = available.ToArray();
            bool[] finish = new bool[numProcesses];

            while (finish.Count(x => x) < numProcesses)
            {
                bool foundProcess = false;
                for (int i = 0; i < numProcesses; i++)
                {
                    if (!finish[i] && Enumerable.Range(0, numResources).All(j => need[i, j] <= work[j]))
                    {
                        finish[i] = true;
                        work = Enumerable.Range(0, numResources).Select(j => work[j] + allocation[i, j]).ToArray();
                        foundProcess = true;
                    }
                }
                if (!foundProcess)
                {
                    return false;
                }
            }

            return true;
        }

        static void HandleRequest(int numProcesses, int numResources, int[] available, int[,] maximum, int[,] allocation)
        {
            Console.Write("Enter process number (starting from 0): ");
            int processNum = int.Parse(Console.ReadLine());

            int[] request = new int[numResources];
            for (int i = 0; i < numResources; i++)
            {
                Console.Write($"Enter request for resource {i + 1}: ");
                request[i] = int.Parse(Console.ReadLine());
            }

            if (Enumerable.Range(0, numResources).Any(i => request[i] > maximum[processNum, i]))
            {
                Console.WriteLine("Error: Request exceeds maximum need");
            }
            else if (Enumerable.Range(0, numResources).Any(i => request[i] > available[i]))
            {
                Console.WriteLine("Error: Request exceeds available resources");
            }
            else
            {
                int[] availableCopy = available.ToArray();
                int[,] allocationCopy = (int[,])allocation.Clone();

                for (int i = 0; i < numResources; i++)
                {
                    availableCopy[i] -= request[i];
                    allocationCopy[processNum, i] += request[i];
                }

                if (IsSafe(numProcesses, numResources, availableCopy, maximum, allocationCopy))
                {
                    Console.WriteLine("Request granted");
                    for (int i = 0; i < numResources; i++)
                    {
                        available[i] -= request[i];
                        allocation[processNum, i] += request[i];
                    }
                }
                else
                {
                    Console.WriteLine("Error: Request would result in an unsafe state");
                }
            }
        }

        static void Main()
        {
            Console.Write("Enter number of processes: ");
            int numProcesses = int.Parse(Console.ReadLine());

            Console.Write("Enter number of resources: ");
            int numResources = int.Parse(Console.ReadLine());

            int[] totalResources = new int[numResources];
            int[] available = new int[numResources];

            for (int i = 0; i < numResources; i++)
            {
                Console.Write($"Enter total resources for resource {i + 1}: ");
                totalResources[i] = int.Parse(Console.ReadLine());

                Console.Write($"Enter available resources for resource {i + 1}: ");
                available[i] = int.Parse(Console.ReadLine());
            }

            int[,] allocation = new int[numProcesses, numResources];
            int[,] maximum = new int[numProcesses, numResources];

            for (int i = 0; i < numProcesses; i++)
            {
                for (int j = 0; j < numResources; j++)
                {
                    Console.Write($"Enter allocation for process {i} for resource {j + 1}: ");
                    allocation[i, j] = int.Parse(Console.ReadLine());

                    Console.Write($"Enter maximum need for process {i} for resource {j + 1}: ");
                    maximum[i, j] = int.Parse(Console.ReadLine());
                }
            }

            while (true)
            {
                Console.Write("Enter '1' to make a request, or any other key to exit:");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    HandleRequest(numProcesses, numResources, available, maximum, allocation);
                }
                else
                {
                    break;
                }
            }
        }
    }
}