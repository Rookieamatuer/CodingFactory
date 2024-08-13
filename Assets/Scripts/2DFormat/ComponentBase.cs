using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class ComponentBase : MonoBehaviour
{
    protected Transform content;
    protected Bounds parentBound;
    protected float len;
    protected float newPos;
    protected int capacity;
    protected bool noData;
    protected bool outOfCapacity;

    public float inputPoint { get; protected set; }
    public float outputPoint { get; protected set; }
    protected float moveTime;
    public bool moveToOutput {  get; protected set; }
    public bool moveToInput { get; protected set; }
    public float intervalTime = 1f;

    public virtual void InputData(Transform data)
    {
        if (data == null)
        {
            noData = true;
            Debug.Log("No data available");
            return;
        }
        if (content.childCount >= capacity)
        {
            outOfCapacity = true;
            Debug.Log("capacity: " + capacity);
            Debug.Log("Out of range");
            return;
        }
        data.SetParent(content, true);
    }

    public virtual Transform OutputData()
    {
        return null;
    }

    public virtual void InputControl(bool t)
    {

    }

    public void MoveToOutput(float targetPos)
    {
        if (inputPoint == transform.position.x && targetPos == outputPoint)
        {
            moveToOutput = true;
            moveTime = 0;
        }

    }

    public void MoveToInput(float targetPos)
    {
        if (outputPoint == transform.position.x && targetPos == inputPoint)
        {
            moveToInput = true;
            moveTime = 0;
        }
    }

    protected void ComponentMove(float interval)
    {
        if (moveToInput && moveTime / interval <= 1)
        {
            float lerpV = Mathf.Lerp(outputPoint, inputPoint, moveTime / interval);
            transform.position = new Vector2(lerpV, transform.position.y);
            moveTime += Time.deltaTime;
        }
        else if (moveToOutput && moveTime / interval <= 1)
        {
            float lerpV = Mathf.Lerp(inputPoint, outputPoint, moveTime / interval);
            transform.position = new Vector2(lerpV, transform.position.y);
            moveTime += Time.deltaTime;
        }
        else
        {
            float newX = transform.position.x - inputPoint < outputPoint - transform.position.x ? inputPoint : outputPoint;
            transform.position = new Vector2(newX, transform.position.y);
            moveToInput = false;
            moveToOutput = false;
            moveTime = 0;
        }
    }

    public bool isEmpty()
    {
        return content.childCount == 0;
    }
}
