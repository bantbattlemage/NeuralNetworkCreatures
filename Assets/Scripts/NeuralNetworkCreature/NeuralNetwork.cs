using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    public float Fitness = 0;
    public int[] Layers { get; private set; }
    private float[][] _neurons;
    private float[][] _biases;
    private float[][][] _weights;

    public NeuralNetwork(int[] layers)
    {
        Layers = new int[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            Layers[i] = layers[i];
        }

        //  Initialize the neurons
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < Layers.Length; i++)
        {
            neuronsList.Add(new float[Layers[i]]);
        }
        _neurons = neuronsList.ToArray();

        InitializeWeightsAndBiases();
    }

    /// <summary>
    /// Initialize the network weights & biases with random values between -0.5 and 0.5, with a 50% chance of being set to 0.
    /// </summary>
    private void InitializeWeightsAndBiases()
    {
        //  initialize biases
        List<float[]> biasList = new List<float[]>();

        for (int i = 0; i < Layers.Length; i++)
        {
            float[] bias = new float[Layers[i]];
            for (int j = 0; j < Layers[i]; j++)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
				{
                    bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                else
				{
                    bias[j] = 0;
				}
            }

            biasList.Add(bias);
        }

        _biases = biasList.ToArray();

        //  initialzie weights
        List<float[][]> weightsList = new List<float[][]>();

        for (int i = 1; i < Layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = Layers[i - 1];

            for (int j = 0; j < _neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    if (UnityEngine.Random.Range(0, 100) < 50)
                    {
                        neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else
                    {
                        neuronWeights[k] = 0;
                    }
                }

                layerWeightsList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightsList.ToArray());
        }

        _weights = weightsList.ToArray();
    }

    /// <summary>
    /// Activate the NeuralNetwork, running through the input neurons with the given inputs and feeding forward throughout the whole network to activate the output neurons.
    /// </summary>
    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            _neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < Layers.Length; i++)
        {
            for (int j = 0; j < _neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < _neurons[i - 1].Length; k++)
                {
                    value += _weights[i - 1][j][k] * _neurons[i - 1][k];
                }

                _neurons[i][j] = Activate(value + _biases[i][j]);
            }
        }

        return _neurons[_neurons.Length - 1];
    }

    /// <summary>
    /// Activate the neuron with the given value, returning the hyperbolic tanget of the value to give a float between -1 and 1
    /// </summary>
    public float Activate(float value)
    {
        return (float)Math.Tanh(value);
    }

    public void Mutate(int chance, float strength)
    {
        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                _biases[i][j] = (UnityEngine.Random.Range(0, 100) <= chance) ? _biases[i][j] += UnityEngine.Random.Range(-strength, strength) : _biases[i][j];
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    _weights[i][j][k] = (UnityEngine.Random.Range(0, 100) <= chance) ? _weights[i][j][k] += UnityEngine.Random.Range(-strength, strength) : _weights[i][j][k];
                }
            }
        }
    }

    /// <summary>
    /// Return a new NeuralNetwork using a random mix of this and the otherParent Network's weights and biases.
    /// </summary>
    public NeuralNetwork CreateChildNetwork(NeuralNetwork otherParent)
	{
        NeuralNetwork childNetwork = new NeuralNetwork(Layers);

        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                childNetwork._biases[i][j] = UnityEngine.Random.Range(0, 100) < 50 ? _biases[i][j] : otherParent._biases[i][j];
            }
        }
        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    childNetwork._weights[i][j][k] = UnityEngine.Random.Range(0, 100) < 50 ? _weights[i][j][k] : otherParent._weights[i][j][k];
                }
            }
        }

        return childNetwork;
    }

    /// <summary>
    /// Returns a deep copy of the NeuralNetwork.
    /// </summary>
    public NeuralNetwork CreateDeepCopy()
    {
        NeuralNetwork deepCopy = new NeuralNetwork(Layers);

        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                deepCopy._biases[i][j] = _biases[i][j];
            }
        }
        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    deepCopy._weights[i][j][k] = _weights[i][j][k];
                }
            }
        }

        return deepCopy;
    }

    public void Load(string path)
    {
        TextReader tr = new StreamReader(path);
        int NumberOfLines = (int)new FileInfo(path).Length;
        string[] ListLines = new string[NumberOfLines];
        int index = 1;
        for (int i = 1; i < NumberOfLines; i++)
        {
            ListLines[i] = tr.ReadLine();
        }
        tr.Close();
        if (new FileInfo(path).Length > 0)
        {
            for (int i = 0; i < _biases.Length; i++)
            {
                for (int j = 0; j < _biases[i].Length; j++)
                {
                    _biases[i][j] = float.Parse(ListLines[index]);
                    index++;
                }
            }

            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        _weights[i][j][k] = float.Parse(ListLines[index]); ;
                        index++;
                    }
                }
            }
        }
    }

    public void Save(string path)
    {
        File.Create(path).Close();
        StreamWriter writer = new StreamWriter(path, true);

        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                writer.WriteLine(_biases[i][j]);
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    writer.WriteLine(_weights[i][j][k]);
                }
            }
        }
        writer.Close();
    }

    public float SumWeightAndBiases()
    {
        float sum = 0;

        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                sum += _biases[i][j];
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    sum += _weights[i][j][k];
                }
            }
        }

        return sum;
    }

    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (Fitness > other.Fitness)
            return 1;
        else if (Fitness < other.Fitness)
            return -1;
        else
            return 0;
    }
}