using NeuralNetworkRunner.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuralNetworkRunner
{
    class Program
    {
        public static readonly Random RNG = new Random();

        public static void Main(string[] args)
        {
            var neuralNet = new NeuralNetwork(0.001, 28 * 28, 10,
                new LayerDefinition(1000, ActivationFunction.TanH)
            );

            //Randomize the inputs.
            var inputs = IdxReader.GetInputs().ToArray();
            for(int i = 0; i < inputs.Length; i++)
            {
                var swapPos = RNG.Next(inputs.Length);

                var temp = inputs[i];
                inputs[i] = inputs[swapPos];
                inputs[swapPos] = temp;
            }

            //Split into training and testing data.
            var trainCount = 0;
            List<Tuple<double[], double[]>> trainData = new();
            List<Tuple<double[], double[]>> testData = new();
            foreach (var input in inputs)
            {
                var tuple = Tuple.Create(input.Item1.Select(b => (double)b / 256).ToArray(), Enumerable.Range(0, 10).Select(i => i == (double)input.Item2 ? 1.0 : 0.0).ToArray());
                if (trainCount < 8000)
                {
                    trainData.Add(tuple);
                } 
                else
                {
                    testData.Add(tuple);
                }

                trainCount++;
            }

            trainCount = 0;
            foreach(var train in trainData)
            {
                neuralNet.TrainInput(train.Item1, train.Item2);
                //PrintArray(Enumerable.Range(0, 10).Select(i => i == (double)input.Item2 ? 1.0 : 0.0).ToArray());

                if(trainCount % 10 == 0)
                {
                    var correct = 0;
                    var error = 0.0;

                    for(int i = 0; i < testData.Count; i++)
                    {
                        var result = neuralNet.Test(testData[i].Item1);
                        var target = testData[i].Item2;

                        var maxResultVal = result.Select(n => n.ForwardPassValue).Max();
                        var resultPos = Enumerable.Range(0, 10).First(i => result[i].ForwardPassValue == maxResultVal);
                        var targetPos = Enumerable.Range(0, 10).First(i => target[i] == 1);

                        for(int j = 0; j < 10; j++)
                        {
                            error += (result[j].TargetValue.Value - result[j].ForwardPassValue) * (result[j].TargetValue.Value - result[j].ForwardPassValue);
                        }

                        if(resultPos == targetPos)
                        {
                            correct++;
                        }
                    }

                    Console.WriteLine($"Train Count: {trainCount}");
                    Console.WriteLine($"Correct: {correct}");
                    Console.WriteLine($"Error: {error}");
                    Console.WriteLine();
                }

                trainCount++;
            }

            //PrintArray(neuralNet.Test(trainData));

            //neuralNet.TrainInput(
            //    trainData, 
            //    new double[] { -10.0, -5.0, 25.0, -25.0 });

            //PrintArray(neuralNet.Test(trainData));

            //for(int i = 0; i < 100; i++)
            //{
            //    neuralNet.TrainInput(
            //        trainData,
            //        new double[] { -10.0, -5.0, 25.0, -25.0 }
            //    );

            //    //if(i % 10 == 0)
            //    //{
            //        PrintArray(neuralNet.Test(trainData));
            //        //Console.ReadLine();
            //    //}
            //}

            //PrintArray(neuralNet.Test(trainData));

            Console.ReadKey();
        }

        public static void PrintArray(double[] arr)
        {
            Console.WriteLine(string.Join(" ", arr.Select(d => d.ToString("#.##"))));
        }
    }
}
