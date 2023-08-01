using hesanta.AI.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCodeConsole.NeuronGenes
{
    public class NeuronDataGenesConverter
    {
        public static NeuralNetworkData CreateNetworkDataFromGenes(List<NeuronGene> genes, int inputNeurons, int[] hiddenLayers, int outputNeurons)
        {
            var networkData = new NeuralNetworkData();
            int currentGene = 0;

            networkData.InputLayer = CreateLayerFromGenes(genes, ref currentGene, inputNeurons);
            networkData.HiddenLayers = hiddenLayers.Select(neurons => CreateLayerFromGenes(genes, ref currentGene, neurons)).ToList();
            networkData.OutputLayer = CreateLayerFromGenes(genes, ref currentGene, outputNeurons);

            return networkData;
        }

        public static List<NeuronGene> CreateGenesFromNetworkData(NeuralNetworkData networkData)
        {
            var genes = new List<NeuronGene>();

            genes.AddRange(CreateGenesFromLayer(networkData.InputLayer));
            foreach (var layer in networkData.HiddenLayers)
            {
                genes.AddRange(CreateGenesFromLayer(layer));
            }
            genes.AddRange(CreateGenesFromLayer(networkData.OutputLayer));

            return genes;
        }

        private static List<NeuronData> CreateLayerFromGenes(List<NeuronGene> genes, ref int currentGene, int neurons)
        {
            var layer = new List<NeuronData>();
            for (int i = 0; i < neurons; i++)
            {
                layer.Add(genes[currentGene++].Value);
            }
            return layer;
        }

        private static List<NeuronGene> CreateGenesFromLayer(List<NeuronData> layer)
        {
            return layer.Select(neuronData => CreateGeneFromNeuronData(neuronData)).ToList();
        }

        private static NeuronGene CreateGeneFromNeuronData(NeuronData neuronData)
        {
            var gene = new NeuronGene();
            gene.Value.Weights = new List<double>(neuronData.Weights);
            gene.Value.Bias = neuronData.Bias;
            return gene;
        }
    }
}
