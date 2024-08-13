using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ComponentTmp : ComponentBase
{
    [SerializeField] int count;
    [SerializeField] Transform route;
    [SerializeField] float currentX;
    

    private void Awake()
    {
        capacity = count;
        content = transform;
        moveTime = 0;
        Bounds routeLine = route.GetComponent<Renderer>().bounds;
        inputPoint = routeLine.min.x;
        outputPoint = routeLine.max.x;
    }

    public override void InputData(Transform data)
    {
        base.InputData(data);
        if (noData || outOfCapacity)
        {
            return;
        }
        data.position = transform.position;
    }

    public override Transform OutputData()
    {
        if (transform.childCount == 0)
            return null;
        return transform.GetChild(0);
    }

    private void Update()
    {
        ComponentMove(intervalTime);
    }

}
