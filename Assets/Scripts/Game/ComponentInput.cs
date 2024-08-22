using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentInput : ComponentBase
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
        //DataManager.instance.DataMove(transform, t, count, idx++, "Stack");
        base.InputData(data);
        parentBound = transform.GetComponent<Renderer>().bounds;
        len = data.GetComponent<Renderer>().bounds.size.x;
        newPos = parentBound.min.x + spacing + len / 2 + (len + spacing) * currentIndex;
        data.position = new Vector3(newPos, parentBound.center.y, parentBound.center.z);
        currentIndex += 1;
    }

    public override Transform OutputData()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("null");
            return null;
        }
        //for (int i = 0; i < transform.childCount - 1; ++i)
        //{
        //    parentBound = transform.GetComponent<Renderer>().bounds;
        //    len = transform.GetChild(i).GetComponent<Renderer>().bounds.size.x;
        //    newPos = transform.GetChild(i).position.x + len + spacing;
        //    transform.GetChild(i).position = new Vector3(newPos, parentBound.center.y, parentBound.center.z);
        //}
        return transform.GetChild(transform.childCount - 1);
    }
}
