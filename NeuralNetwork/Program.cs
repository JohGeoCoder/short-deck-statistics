using NeuralNetworkRunner.Structures;
using System;
using System.IO;
using System.Linq;

namespace NeuralNetworkRunner
{
    class Program
    {
        public static void Main(string[] args)
        {
            var neuralNet = new NeuralNetwork(5, 4, 100, 200, 100);

            var trainData = new decimal[] { 0.1m, 0.3m, 0.5m, 0.7m, 0.9m };

            PrintArray(neuralNet.Test(trainData));

            neuralNet.TrainInput(
                trainData, 
                new decimal[] { 0.5m, 0.5m, 0.5m, 0.5m });

            PrintArray(neuralNet.Test(trainData));

            for(int i = 0; i < 1000; i++)
            {
                neuralNet.TrainInput(
                    trainData,
                    new decimal[] { 0.5m, 0.5m, 0.5m, 0.5m }
                );

                if(i % 10 == 0)
                {
                    PrintArray(neuralNet.Test(trainData));
                    Console.ReadLine();
                }
            }

            PrintArray(neuralNet.Test(trainData));

            Console.ReadKey();
        }

        public static void PrintArray(decimal[] arr)
        {
            Console.WriteLine(string.Join(" ", arr.Select(d => d.ToString("#.##"))));
        }
    }
}
