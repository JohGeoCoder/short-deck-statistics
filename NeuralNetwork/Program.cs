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
            var neuralNet = new NeuralNetwork(new LearningRateDefinition(0.01, 5000, 1.0), 14, new LayerDefinition(7, ActivationFunction.Binary),
                new LayerDefinition(50, ActivationFunction.TanH),
                new LayerDefinition(50, ActivationFunction.TanH),
                new LayerDefinition(50, ActivationFunction.TanH)
            );

            ////Randomize the inputs.
            //var inputs = IdxReader.GetInputs().ToArray();
            //for(int i = 0; i < inputs.Length; i++)
            //{
            //    var swapPos = RNG.Next(inputs.Length);

            //    var temp = inputs[i];
            //    inputs[i] = inputs[swapPos];
            //    inputs[swapPos] = temp;
            //}

            //Split into training and testing data.
            //var trainCount = 0;
            //List<Tuple<double[], double[]>> trainData = new();
            //List<Tuple<double[], double[]>> testData = new();
            //foreach (var input in inputs)
            //{
            //    var tuple = Tuple.Create(input.Item1.Select(b => (double)b / 256).ToArray(), Enumerable.Range(0, 10).Select(i => i == (double)input.Item2 ? 1.0 : 0.0).ToArray());
            //    if (trainCount < 8000)
            //    {
            //        trainData.Add(tuple);
            //    } 
            //    else
            //    {
            //        testData.Add(tuple);
            //    }

            //    trainCount++;
            //}

            var trainCount = 0;
            while(trainCount < 100000)
            {
                var factor1 = (decimal)(int)(RNG.NextDouble() * 10 + 1);
                var factor2 = (decimal)(int)(RNG.NextDouble() * 10 + 1);

                var boolArrayFactor1 = ConvertNumberToBoolArray(factor1);
                var boolArrayFactor2 = ConvertNumberToBoolArray(factor2);
                var input = new List<double>();
                input.AddRange(boolArrayFactor1);
                input.AddRange(boolArrayFactor2);
                var boolArrayTarget = ConvertNumberToBoolArray(factor1 * factor2);

                neuralNet.TrainInput(input.ToArray(), boolArrayTarget);
                //PrintArray(Enumerable.Range(0, 10).Select(i => i == (double)input.Item2 ? 1.0 : 0.0).ToArray());

                if(trainCount % 100 == 0)
                {
                    var correct = 0;
                    var error = 0.0;

                    var testData = Enumerable.Range(0, 100).Select(i =>
                    {
                        var testFactor1 = (decimal)(int)(RNG.NextDouble() * 10 + 1);
                        var testFactor2 = (decimal)(int)(RNG.NextDouble() * 10 + 1);

                        var a1 = ConvertNumberToBoolArray(factor1);
                        var a2 = ConvertNumberToBoolArray(factor2);
                        var testInput = new List<double>();
                        testInput.AddRange(boolArrayFactor1);
                        testInput.AddRange(boolArrayFactor2);

                        return Tuple.Create(testInput.ToArray(), ConvertNumberToBoolArray(testFactor1 * testFactor2));
                    })
                    .ToList();

                    for(int i = 0; i < testData.Count; i++)
                    {
                        var result = neuralNet.Test(testData[i].Item1);
                        var resultValueArray = result.Select(n => (int)n.ForwardPassValue).ToArray();
                        var targetValueArray = testData[i].Item2;

                        var totalTargetValue = Enumerable.Range(0, 7).Sum(i => Math.Pow(2.0, 6 - i) * targetValueArray[i]);
                        var totalResultValue = Enumerable.Range(0, 7).Sum(i => Math.Pow(2.0, 6-i) * resultValueArray[i]);

                        error += (totalResultValue - totalTargetValue) * (totalResultValue - totalTargetValue);

                        if(Math.Abs(totalResultValue - totalTargetValue) < 0.1)
                        {
                            correct++;
                        }
                    }

                    Console.WriteLine($"Train Count: {trainCount}");
                    Console.WriteLine($"Learning Rate: {neuralNet.LearningRate}");
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

        public static double[] ConvertNumberToBoolArray(decimal number)
        {
            var result = new double[7];

            if(number >= 64)
            {
                result[0] = 1.0;
                number -= 64;
            }

            if (number >= 32)
            {
                result[1] = 1.0;
                number -= 32;
            }

            if (number >= 16)
            {
                result[2] = 1.0;
                number -= 16;
            }

            if (number >= 8)
            {
                result[3] = 1.0;
                number -= 8;
            }

            if (number >= 4)
            {
                result[4] = 1.0;
                number -= 4;
            }

            if (number >= 2)
            {
                result[5] = 1.0;
                number -= 2;
            }

            if (number >= 1)
            {
                result[6] = 1.0;
                number -= 1;
            }

            return result;
        }

        public static void PrintArray(double[] arr)
        {
            Console.WriteLine(string.Join(" ", arr.Select(d => d.ToString("#.##"))));
        }
    }
}
