using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neural
{
    public double[] weights;

    public double value;

    public bool isOutput;

    public int index;

    public Neural(int index, int weightNum, bool isOutput)
    {
        this.index = index;
        weights = new double[weightNum];
        this.isOutput = isOutput;
        value = 0;
    }

    public void Excute(double[] inputs)
    {
        double sum = 0;
        if (weights.Length > 0)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i];
            }
        }
        else
        {
            sum += inputs[index];
        }
        value = TanhFunction(sum);

    }

    private double SigmoidFunction(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
    }
    private double TanhFunction(double x)
    {
        return Math.Tanh(x);
    }
}
