using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackComponent : MonoBehaviour 
{
    Transform resultArea;

    public void InputData(Transform currentData)
    {
        currentData.SetParent(transform, false);
        currentData.transform.localPosition = Vector2.zero;
    }

    public void OutputData(Transform outputArea)
    {
        if (transform.childCount > 0)
            transform.GetChild(transform.childCount - 1).SetParent(outputArea, false);
    }
}
