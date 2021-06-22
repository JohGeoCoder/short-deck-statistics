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
            var neuralNet = new NeuralNetwork(5, 4,
                new LayerDefinition(5, ActivationFunction.Sigmoid),
                new LayerDefinition(5, ActivationFunction.TanH),
                new LayerDefinition(5, ActivationFunction.TanH)
            );

            var trainData = new double[] { 0.1, 0.3, 0.5, 0.7, 0.9 };

            PrintArray(neuralNet.Test(trainData));

            neuralNet.TrainInput(
                trainData, 
                new double[] { -10.0, -5.0, 25.0, -25.0 });

            PrintArray(neuralNet.Test(trainData));

            for(int i = 0; i < 100; i++)
            {
                neuralNet.TrainInput(
                    trainData,
                    new double[] { -10.0, -5.0, 25.0, -25.0 }
                );

                //if(i % 10 == 0)
                //{
                    PrintArray(neuralNet.Test(trainData));
                    //Console.ReadLine();
                //}
            }

            PrintArray(neuralNet.Test(trainData));

            Console.ReadKey();
        }

        public static void PrintArray(double[] arr)
        {
            Console.WriteLine(string.Join(" ", arr.Select(d => d.ToString("#.##"))));
        }
    }
}
