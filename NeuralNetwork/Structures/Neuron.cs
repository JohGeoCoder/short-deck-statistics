using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralNetworkRunner.Structures
{
    public class Neuron
    {
        public double? TargetValue { get; set; }
        public double ForwardPassValue { get; set; }
        public double BackwardPassValue { get; set; }
        private Neuron[] _activations;
        public Neuron[] Activations 
        { 
            get => _activations; 
            set {
                _activations = value;
                ActivationWeights = Enumerable.Range(0, value.Length)
                    .Select(i => (NeuralNetwork.RNG.NextDouble() * 2 - 1))
                    .ToArray();
            }
        }
        public double[] ActivationWeights { get; private set; }

        public ActivationFunction ActivationFunction { get; set; }

        public Neuron(ActivationFunction activationFunction = ActivationFunction.Identity)
        {
            ActivationFunction = activationFunction;
        }

        public double ActivationFunctionValue(double netValue)
        {
            switch (ActivationFunction) {
                case ActivationFunction.Identity:
                    return netValue;
                case ActivationFunction.Relu:
                    return netValue < 0 ? 0 : netValue;
                case ActivationFunction.Binary:
                    return netValue >= 0 ? 1 : 0;
                case ActivationFunction.ArcTan:
                    return Math.Atan(netValue);
                case ActivationFunction.Sigmoid:
                    return (
                        1 / (
                            1 + Math.Exp(-netValue)
                        )
                    );

                case ActivationFunction.TanH:
                    return (
                        2 / (
                            1 + Math.Exp(-2 * netValue)
                        ) - 1
                    );
                default:
                    throw new Exception("Unsupported activation function");
            }
        }

        public double ActivationDerivativeValue()
        {
            switch (ActivationFunction)
            {
                case ActivationFunction.Identity:
                    return 1;
                case ActivationFunction.Relu:
                    return ForwardPassValue < 0 ? 0 : 1;
                case ActivationFunction.Binary:
                    return 0;
                case ActivationFunction.ArcTan:
                    return 1 / (ForwardPassValue * ForwardPassValue + 1);
                case ActivationFunction.Sigmoid:
                    return ForwardPassValue * (1 - ForwardPassValue);
                case ActivationFunction.TanH:
                    return 1 - ForwardPassValue * ForwardPassValue;
                default:
                    throw new Exception("Unsupported activation function");
            }
        }
    }

    public class Layer
    {
        public static ActivationFunction ActivationFunction { get; set; }

        public Neuron[] Neurons { get; set; }
        public Neuron Bias { get; set; }

        public Layer(int size)
        {
            Neurons = Enumerable.Range(0, size).Select(i => new Neuron()).ToArray();
            Bias = new Neuron();
        }

        public Layer(LayerDefinition definition)
        {
            Neurons = Enumerable.Range(0, definition.Size).Select(i => new Neuron(definition.ActivationFunction)).ToArray();
            Bias = new Neuron();
        }
    }

    public enum ActivationFunction
    {
        Sigmoid,
        Identity,
        TanH,
        Binary,
        Relu,
        ArcTan
    }

    public struct LayerDefinition
    {
        public int Size;
        public ActivationFunction ActivationFunction;

        public LayerDefinition(int size, ActivationFunction activationFunction)
        {
            Size = size;
            ActivationFunction = activationFunction;
        }
    }

    public class NeuralNetwork
    {
        public static readonly Random RNG = new();

        public List<Layer> Layers { get; set; } = new();
        public double LearningRate { get; set; }

        public NeuralNetwork(double learningRate, int inputSize, int outputSize, params LayerDefinition[] hiddenLayerDefinitions)
        {
            LearningRate = learningRate;

            //Create the input layer
            Layers.Add(new Layer(inputSize));

            //Generate the hidden layers
            var currentLayer = Layers[0];
            foreach(var definition in hiddenLayerDefinitions)
            {
                var hiddenLayer = new Layer(definition);
                Layers.Add(hiddenLayer);

                //Create weights from each Neuron in the current layer to the new layer neurons.
                foreach (var neuron in currentLayer.Neurons)
                {
                    neuron.Activations = hiddenLayer.Neurons;
                }

                //Create the bias weights to the new layer neurons.
                currentLayer.Bias.Activations = hiddenLayer.Neurons;

                currentLayer = hiddenLayer;
            }

            //Generate the output layer
            var outputLayer = new Layer(new LayerDefinition(outputSize, ActivationFunction.Sigmoid));
            Layers.Add(outputLayer);

            //Create weights from each Neuron in the current layer to the new layer neurons.
            foreach (var neuron in currentLayer.Neurons)
            {
                neuron.Activations = outputLayer.Neurons;
            }

            //Add the output activations to the final hidden layer bias neuron.
            currentLayer.Bias.Activations = outputLayer.Neurons;
        }

        public Neuron[] Test(double[] input)
        {
            SetInput(input);

            ForwardPass();

            var outputLayer = Layers[^1];

            return outputLayer.Neurons;
        }

        public void TrainInput(double[] input, double[] target)
        {
            SetInput(input);

            ForwardPass();

            BackwardPass(target);
        }

        private void SetInput(double[] input)
        {
            var inputLayer = Layers[0];

            //Set the input layer forward pass value.
            for (int i = 0; i < input.Length; i++)
            {
                inputLayer.Neurons[i].ForwardPassValue = input[i];
            }
        }

        private void ForwardPass()
        {
            var currentLayerPos = 1;

            //Forward Pass the input values through the layers
            while (currentLayerPos < Layers.Count)
            {
                var previousLayer = Layers[currentLayerPos - 1];
                var currentLayer = Layers[currentLayerPos];

                var processesRemaining = currentLayer.Neurons.Length;
                using ManualResetEvent mre = new(false);
                for(int i = 0; i < currentLayer.Neurons.Length; i++)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                    {
                        var pos = (int)x;
                        var currentNeuron = currentLayer.Neurons[pos];

                        var neuronValue = currentNeuron.ActivationFunctionValue(
                            previousLayer.Neurons.Sum(n =>
                                n.ActivationWeights[pos] * n.ForwardPassValue
                            ) + previousLayer.Bias.ActivationWeights[pos]
                        );

                        currentNeuron.ForwardPassValue = neuronValue;

                        // Safely decrement the counter
                        if (Interlocked.Decrement(ref processesRemaining) == 0)
                        {
                            mre.Set();
                        }
                    }), i);
                }

                mre.WaitOne();

                currentLayerPos++;
            }
        }

        private void BackwardPass(double[] target)
        {
            //Set the target values in the output layer
            var outputLayer = Layers[^1];
            for(int i = 0; i < target.Length; i++)
            {
                var outputNeuron = outputLayer.Neurons[i];
                outputNeuron.TargetValue = target[i];
                outputNeuron.BackwardPassValue = 
                    outputNeuron.ActivationDerivativeValue() * (outputNeuron.ForwardPassValue - target[i]);
            }

            //Begin with the last Hidden layer (not the output layer)
            var currentLayerPos = Layers.Count - 2;

            //Update the Weights for each layer
            while(currentLayerPos >= 0)
            {
                var currentLayer = Layers[currentLayerPos];


                //Iterate over each neuron in the layer, updating each of its activation weights
                var processesRemaining = currentLayer.Neurons.Length;
                using ManualResetEvent mre = new(false);
                for (int i = 0; i < currentLayer.Neurons.Length; i++)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                    {
                        var pos = (int)x;

                        var neuron = currentLayer.Neurons[pos];

                        //Set the Backward Pass value of this neuron
                        neuron.BackwardPassValue = BackwardPassValueFunction(neuron);
                            //neuron.ForwardPassValue
                            //* (1 - neuron.ForwardPassValue)
                            //* Enumerable.Range(0, neuron.Activations.Length).Sum(k =>
                            //    neuron.ActivationWeights[k] * neuron.Activations[k].BackwardPassValue
                            //);

                        //var activationsRemaining = neuron.ActivationWeights.Length;
                        //using ManualResetEvent activationsResetEvent = new ManualResetEvent(false);
                        //for (int n = 0; n < neuron.ActivationWeights.Length; n++)
                        //{
                        //    ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                        //    {
                        //        var activationPos = (int)x;

                        //        var weightNode = neuron.Activations[activationPos];
                        //        var weightDelta = -LearningRate * (
                        //            neuron.ForwardPassValue
                        //            * BackwardPassValueFunction(weightNode)
                        //        );

                        //        neuron.ActivationWeights[activationPos] = neuron.ActivationWeights[activationPos] + weightDelta;

                        //        // Safely decrement the counter
                        //        if (Interlocked.Decrement(ref activationsRemaining) == 0)
                        //        {
                        //            activationsResetEvent.Set();
                        //        }

                        //    }), n);
                        //}

                        //activationsResetEvent.WaitOne();

                        //Update each weight of this neuron
                        for (int j = 0; j < neuron.ActivationWeights.Length; j++)
                        {
                            var weightNode = neuron.Activations[j];
                            var weightDelta = -LearningRate * (
                                neuron.ForwardPassValue
                                * BackwardPassValueFunction(weightNode)
                            );

                            neuron.ActivationWeights[j] = neuron.ActivationWeights[j] + weightDelta;
                        }

                        // Safely decrement the counter
                        if (Interlocked.Decrement(ref processesRemaining) == 0)
                        {
                            mre.Set();
                        }
                    }), i);
                }

                mre.WaitOne();

                //Update the bias weights
                for(int j = 0; j < currentLayer.Bias.ActivationWeights.Length; j++)
                {
                    var weightNode = currentLayer.Bias.Activations[j];
                    var weightDelta = -LearningRate * BackwardPassValueFunction(weightNode);

                    currentLayer.Bias.ActivationWeights[j] = currentLayer.Bias.ActivationWeights[j] + weightDelta;
                }

                currentLayerPos--;
            }
        }

        private static double BackwardPassValueFunction(Neuron neuron)
        {
            var value = neuron.ActivationDerivativeValue()
                * (
                    neuron.TargetValue.HasValue
                        ? (neuron.ForwardPassValue - neuron.TargetValue.Value)
                        : Enumerable.Range(0, neuron.Activations.Length).Sum(k =>
                            neuron.ActivationWeights[k] * neuron.Activations[k].BackwardPassValue
                        )
                );

            return value;
        }
    }
}
