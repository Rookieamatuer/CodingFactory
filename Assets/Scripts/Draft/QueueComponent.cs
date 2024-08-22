using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueComponent : MonoBehaviour
{
    public void InputData(Transform currentData)
    {
        currentData.SetParent(transform, false);
        currentData.transform.localPosition = Vector2.zero;
    }

    public void OutputData(Transform outputArea)
    {
        if (transform.childCount > 0)
            transform.GetChild(0).SetParent(outputArea, false);
    }
}
