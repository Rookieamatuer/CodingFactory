using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentOutput : ComponentBase
{
    [SerializeField] int count;

    [SerializeField] int currentIndex;
    [SerializeField] float spacing;

    private void Awake()
    {
        capacity = count;
        content = transform;
    }

    public override void InputData(Transform data)
    {
        base.InputData(data);
        if (noData || outOfCapacity)
        {
            return;
        }
        parentBound = transform.GetComponent<Renderer>().bounds;
        len = data.GetComponent<Renderer>().bounds.size.x;
        newPos = parentBound.max.x - spacing - len / 2 - (len + spacing) * currentIndex;
        data.transform.position = new Vector3(newPos, parentBound.center.y, parentBound.center.z);
        currentIndex++;
    }
}
