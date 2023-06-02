using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCurveTester : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        for (int GenerationIteration = 1; GenerationIteration <= 500; GenerationIteration++)
        {
            float timeToWaitA = 3.4f - MathfFunction.SquareRoot(GenerationIteration) / 2.5f;
            float timeToWaitB = 0.63f - MathfFunction.SquareRoot(30 + GenerationIteration) / 17f;
            Debug.Log($"Iteration {GenerationIteration} | TimeToWait: {timeToWaitA} | TimeToWait OVER 60: {timeToWaitB}");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
