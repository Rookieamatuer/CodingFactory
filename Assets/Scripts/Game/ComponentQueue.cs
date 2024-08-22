using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentQueue : ComponentBase
{
    //[SerializeField] Transform child;
    [SerializeField] Transform queue;
    [SerializeField] Transform queueSwitch;
    [SerializeField] Transform route = null;
    [SerializeField] int count;
    [SerializeField] float spacing;
    bool isStackOn;

    private void Awake()
    {
        capacity = count;
        content = queue;
        moveTime = 0;
        if (route != null)
        {
            Bounds routeLine = route.GetComponent<Renderer>().bounds;
            inputPoint = routeLine.min.x;
            outputPoint = routeLine.max.x;
        }
        else
        {
            inputPoint = content.position.x;
            outputPoint = content.position.x;
        }
        isStackOn = false;
    }

    //private void Start()
    //{
    //    capacity = count;
    //    content = stack;
    //}

    public override void InputData(Transform data)
    {
        base.InputData(data);
        if (noData || outOfCapacity)
        {
            return;
        }
        parentBound = content.GetComponent<Renderer>().bounds;
        len = data.GetComponent<Renderer>().bounds.size.y;
        KeepLayout(true);
    }

    public override Transform OutputData()
    {
        if (content.childCount == 0)
            return null;
        KeepLayout(false);
        return content.GetChild(0);
    }

    public override void InputControl(bool isOn)
    {
        queueSwitch.GetComponent<SpriteRenderer>().color = isOn ? Color.green : Color.red;
        isStackOn = !isStackOn;
    }

    private void KeepLayout(bool isIn)
    {
        if (isIn)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                newPos = parentBound.min.y + spacing + len / 2 + (len + spacing) * i;
                content.GetChild(i).position = new Vector3(content.position.x, newPos, content.position.z);
            }
        } else
        {
            for (int i = 1; i < content.childCount; i++)
            {
                newPos = parentBound.min.y + spacing + len / 2 + (len + spacing) * (i - 1);
                content.GetChild(i).position = new Vector3(content.position.x, newPos, content.position.z);
            }
        }
    }

    private void Update()
    {
        if (route != null)
        {
            ComponentMove(intervalTime);
        }

    }
}
