using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCurveTester : MonoBehaviour
{

    public int MaxGenerationIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int GenerationIteration = 1; GenerationIteration <= MaxGenerationIndex; GenerationIteration++)
        {
            float timeToWaitA = 2 - MathfFunction.Exponential(1.4f, GenerationIteration)/1000; // max index 22
            float timeToWaitB = MathfFunction.SquareRoot(GenerationIteration) * 2;
            Debug.Log($"Iteration {GenerationIteration} | TimeToWaitA: {timeToWaitA} | TimeToWaitB: {timeToWaitB}");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
