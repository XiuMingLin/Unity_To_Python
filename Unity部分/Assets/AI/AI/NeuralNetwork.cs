using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NeuralNetwork
{
    public List<NeuralLayer> neuralLayers;
    private int[] layerShape;
    public int[] splitPoints;
    public int weightNum;

    public NeuralNetwork(int[] layerShape)
    {
        this.layerShape = layerShape;
        neuralLayers = new List<NeuralLayer>();
        for (int i = 0; i < layerShape.Length; i++)
        {
            int weightNum = i == 0 ? 0 : layerShape[i - 1] + 1;
            NeuralLayer layer = new NeuralLayer(layerShape[i], weightNum, i == layerShape.Length - 1);
            neuralLayers.Add(layer);
        }
        GetSplitPoints();
    }

    public double[] Run(double[] inputs)
    {
        double[] res = new double[layerShape[layerShape.Length - 1]];
        if (inputs.Length != layerShape[0])
        {
            throw new System.Exception("输入参数个数不符合要求，必须为：" + layerShape[0] + "个，当前输入个数:" + inputs.Length);
        }
        for (int i = 0; i < layerShape.Length; i++)
        {
            if (i == 0)
            {
                neuralLayers[i].Excute(inputs);
            }
            else
            {
                neuralLayers[i].Excute(neuralLayers[i - 1].GetValues());
            }
        }
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = neuralLayers[neuralLayers.Count - 1].neurals[i].value;
        }
        return res;
    }

    public void SetWeights(double[] weights)
    {
        int index = 0;
        for (int i = 1; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                for (int k = 0; k < neuralLayers[i].neurals[j].weights.Length; k++)
                {
                    neuralLayers[i].neurals[j].weights[k] = weights[index];
                    index++;
                }
            }
        }
    }

    private void GetSplitPoints()
    {
        List<int> splitPointList = new List<int>();
        int n = 0;
        for (int i = 1; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                splitPointList.Add(n + neuralLayers[i].neurals[j].weights.Length);
                n += neuralLayers[i].neurals[j].weights.Length;
                weightNum += neuralLayers[i].neurals[j].weights.Length;
            }
        }
        splitPoints = splitPointList.ToArray();
    }

    public double[] GetWeights()
    {
        List<double> weightList = new List<double>();
        for (int i = 1; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                for (int k = 0; k < neuralLayers[i].neurals[j].weights.Length; k++)
                {
                    weightList.Add(neuralLayers[i].neurals[j].weights[k]);
                }
            }
        }
        return weightList.ToArray();
    }

    public void RandomWeights()
    {
        double[] weights = new double[weightNum];
        for (int i = 0; i < weightNum; i++)
        {
            weights[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        SetWeights(weights);
    }

    public void LoadWeights(string path)
    {
        FileInfo f = new FileInfo(path);
        StreamReader sr = f.OpenText();
        string data = sr.ReadToEnd();
        sr.Close();
        string[] arr = data.Split(',');
        List<double> list = new List<double>();
        foreach (var item in arr)
        {
            list.Add(double.Parse(item));
        }
        SetWeights(list.ToArray());
    }
}
