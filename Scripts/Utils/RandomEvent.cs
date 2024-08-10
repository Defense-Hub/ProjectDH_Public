using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomEvent
{
    private static readonly int totalPercentage  = 10000; // 100%를 나타내는 값
    
    public static int GetArrRandomResult(int[] percent)
    {
        int random = Random.Range(1, totalPercentage + 1);
        int percentSum = 0;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];
            if (random <= percentSum)
            {
                return i;
            }
        }

        Debug.LogError("Percent Error");
        return -1;
    }

    public static bool GetBoolRandomResult(int percent)
    {
        int random = Random.Range(0, 10000);

        return random <= percent ? true : false;
    }
    
}