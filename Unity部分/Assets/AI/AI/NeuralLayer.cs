using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralLayer
{
    public List<Neural> neurals;

    public NeuralLayer(int neuralNum, int weightNum, bool isOutput)
    {
        neurals = new List<Neural>();
        for (int i = 0; i < neuralNum; i++)
        {
            neurals.Add(new Neural(i, weightNum, isOutput));
        }
    }

    public void Excute(double[] inputs)
    {
        foreach (var neural in neurals)
        {
            neural.Excute(inputs);
        }
    }

    public double[] GetValues()
    {
        double[] res = new double[neurals.Count + 1];
        for (int i = 0; i < neurals.Count; i++)
        {
            res[i] = neurals[i].value;
        }
        res[res.Length - 1] = 1;
        return res;
    }
}