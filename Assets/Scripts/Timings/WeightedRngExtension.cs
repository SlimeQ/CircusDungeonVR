using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IEnumerableExtensions
{

    public static T RandomElementByWeightDeterministic<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector, float rolledValue)
    {
        float totalWeight = sequence.Sum(weightSelector);
        // The weight we are after...
        float itemWeightIndex = rolledValue * totalWeight;
        float currentWeightIndex = 0;

        foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
        {
            currentWeightIndex += item.Weight;

            // If we've hit or passed the weight we are after for this item then it's the one we want....
            if (currentWeightIndex >= itemWeightIndex)
                return item.Value;

        }

        return default(T);

    }

    public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector, System.Random rGen)
    {
        return RandomElementByWeightDeterministic(sequence, weightSelector, (float)rGen.NextDouble());

    }

}