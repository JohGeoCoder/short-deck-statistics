using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkRunner.Structures
{
    public class Neuron
    {
        public decimal? TargetValue { get; set; }
        public decimal ForwardPassValue { get; set; }
        public decimal BackwardPassValue { get; set; }
        private Neuron[] _activations;
        public Neuron[] Activations 
        { 
            get => _activations; 
            set {
                _activations = value;
                ActivationWeights = Enumerable.Range(0, value.Length)
                    .Select(i => (decimal)(NeuralNetwork.RNG.NextDouble() * 2 - 1))
                    .ToArray();
            }
        }
        public decimal[] ActivationWeights { get; private set; }
    }

    public class Layer
    {
        public Neuron[] Neurons { get; set; }
        public Neuron Bias { get; set; }

        public Layer(int layerSize)
        {
            Neurons = Enumerable.Range(0, layerSize).Select(i => new Neuron()).ToArray();
            Bias = new Neuron();
        }
    }

    public class NeuralNetwork
    {
        public static readonly Random RNG = new();

        public List<Layer> Layers { get; set; } = new();

        public NeuralNetwork(int inputSize, int outputSize, params int[] hiddenLayerDefinitions)
        {
            //Create the input layer
            Layers.Add(new Layer(inputSize));

            //Generate the hidden layers
            var currentLayer = Layers[0];
            foreach(var hiddenLayerSize in hiddenLayerDefinitions)
            {
                var hiddenLayer = new Layer(hiddenLayerSize);
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
            var outputLayer = new Layer(outputSize);
            Layers.Add(outputLayer);

            //Create weights from each Neuron in the current layer to the new layer neurons.
            foreach (var neuron in currentLayer.Neurons)
            {
                neuron.Activations = outputLayer.Neurons;
            }

            //Add the output activations to the final hidden layer bias neuron.
            currentLayer.Bias.Activations = outputLayer.Neurons;
        }

        public decimal[] Test(decimal[] input)
        {
            SetInput(input);

            ForwardPass();

            var outputLayer = Layers[^1];

            return outputLayer.Neurons.Select(n => n.ForwardPassValue).ToArray();
        }

        public void TrainInput(decimal[] input, decimal[] target)
        {
            SetInput(input);

            ForwardPass();

            BackwardPass(target);
        }

        private void SetInput(decimal[] input)
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

                //Calculate the Forward Pass values in the current layer.
                for (int i = 0; i < currentLayer.Neurons.Length; i++)
                {
                    var currentNeuron = currentLayer.Neurons[i];

                    //Sigmoid
                    var neuronValue = (decimal)(
                        1.0 
                        / (
                            1.0 + Math.Exp(
                                (double)-previousLayer.Neurons.Sum(n =>
                                    n.ActivationWeights[i] * n.ForwardPassValue
                                ) + (double)previousLayer.Bias.ActivationWeights[i]
                            )
                        )
                    );

                    currentNeuron.ForwardPassValue = neuronValue;
                }

                currentLayerPos++;
            }
        }

        private void BackwardPass(decimal[] target)
        {
            //Set the target values in the output layer
            var outputLayer = Layers[^1];
            for(int i = 0; i < target.Length; i++)
            {
                var outputNeuron = outputLayer.Neurons[i];
                outputNeuron.TargetValue = target[i];
                outputNeuron.BackwardPassValue = 
                    outputNeuron.ForwardPassValue 
                    * (1 - outputNeuron.ForwardPassValue)
                    * (outputNeuron.ForwardPassValue - target[i]);
            }

            //Begin with the last Hidden layer (not the output layer)
            var currentLayerPos = Layers.Count - 2;

            //Update the Weights for each layer
            while(currentLayerPos >= 0)
            {
                var currentLayer = Layers[currentLayerPos];
                
                //Iterate over each neuron in the layer, updating each of its activation weights
                for (int i = 0; i < currentLayer.Neurons.Length; i++)
                {
                    var neuron = currentLayer.Neurons[i];

                    //Set the Backward Pass value of this neuron
                    neuron.BackwardPassValue =
                        neuron.ForwardPassValue
                        * (1 - neuron.ForwardPassValue)
                        * Enumerable.Range(0, neuron.Activations.Length).Sum(k =>
                            neuron.ActivationWeights[k] * neuron.Activations[k].BackwardPassValue
                        );

                    //Update each weight of this neuron
                    for(int j = 0; j < neuron.ActivationWeights.Length; j++)
                    {
                        var weightNode = neuron.Activations[j];
                        var weightDelta = -0.01m * (
                            neuron.ForwardPassValue 
                            * weightNode.ForwardPassValue
                            * (1 - weightNode.ForwardPassValue)
                            * (
                                weightNode.TargetValue.HasValue
                                    ? (weightNode.ForwardPassValue - weightNode.TargetValue.Value)
                                    : Enumerable.Range(0, weightNode.Activations.Length).Sum(k =>
                                        weightNode.ActivationWeights[k] * weightNode.Activations[k].BackwardPassValue
                                    )
                            )
                        );

                        neuron.ActivationWeights[j] = neuron.ActivationWeights[j] + weightDelta;
                    }
                }

                //Update the bias weights
                for(int j = 0; j < currentLayer.Bias.ActivationWeights.Length; j++)
                {
                    var weightNode = currentLayer.Bias.Activations[j];
                    var weightDelta = -0.01m * (
                        weightNode.ForwardPassValue
                        * (1 - weightNode.ForwardPassValue)
                        * (
                            weightNode.TargetValue.HasValue
                                ? (weightNode.ForwardPassValue - weightNode.TargetValue.Value)
                                : Enumerable.Range(0, weightNode.Activations.Length).Sum(k =>
                                    weightNode.ActivationWeights[k] * weightNode.Activations[k].BackwardPassValue
                                )
                        )
                    );

                    currentLayer.Bias.ActivationWeights[j] = currentLayer.Bias.ActivationWeights[j] + weightDelta;
                }

                currentLayerPos--;
            }
        }
    }
}
