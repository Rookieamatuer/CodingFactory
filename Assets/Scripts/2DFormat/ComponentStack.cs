using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentStack : ComponentBase
{
    //[SerializeField] Transform child;
    [SerializeField] Transform stack;
    [SerializeField] Transform stackSwitch;
    [SerializeField] Transform route=null;
    [SerializeField] int count;
    [SerializeField] int currentIndex;
    [SerializeField] float spacing;
    bool isStackOn;

    private void Awake()
    {
        capacity = count;
        content = stack;
        currentIndex = 0;
        moveTime = 0;
        if (route != null)
        {
            Bounds routeLine = route.GetComponent<Renderer>().bounds;
            inputPoint = routeLine.min.x;
            outputPoint = routeLine.max.x;
        } else
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
        //DataManager.instance.DataMove(transform, t, count, idx++, "Stack");
        base.InputData(data);
        if (noData || outOfCapacity)
        {
            return;
        }
        parentBound = content.GetComponent<Renderer>().bounds;
        len = data.GetComponent<Renderer>().bounds.size.y;
        newPos = parentBound.max.y - spacing - len / 2 - (len + spacing) * currentIndex;
        //data.transform.position = new Vector3(parentBound.center.x, newPos, parentBound.center.z);
        KeepLayout(content.childCount - 1);
        currentIndex += 1;
    }

    public override Transform OutputData()
    {
        if (content.childCount == 0)
            return null;
        currentIndex--;
        KeepLayout(content.childCount - 2);
        return content.GetChild(content.childCount - 1);
    }

    public override void InputControl(bool isOn)
    {
        stackSwitch.GetComponent<SpriteRenderer>().color = isOn ? Color.green : Color.red;
        isStackOn = !isStackOn;
    }

    private void KeepLayout(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            newPos = parentBound.max.y - spacing - len / 2 - (len + spacing) * (count - i);
            content.GetChild(i).position = new Vector3(content.position.x, newPos, content.position.z);
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
