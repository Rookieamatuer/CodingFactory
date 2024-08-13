using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    //[SerializeField] Transform data;
    [SerializeField] int count;
    [SerializeField] int currentIndex;
    [SerializeField] float spacing;

    private void Awake()
    {
        instance = this;
    }

    public void DataInit(Transform t)
    {
        //if (t.childCount >= count)
        //{
        //    Debug.Log("Out of range");
        //    return;
        //}
        for (int i = 0; i < count; i++)
        {
            GameObject data = (GameObject)Instantiate(Resources.Load("Prefabs/2DFormat/Data"));
            data.name = "Data" + currentIndex;
            data.GetComponentInChildren<TextMeshPro>().text = currentIndex.ToString();
            DataMove(t, data.transform, count, currentIndex++, "Input");
        }
    }

    // Move data
    public void DataMove(Transform container, Transform data, int capacity, int idx, string type)
    {
        if (container.childCount >= capacity)
        {
            Debug.Log("Out of range");
            return;
        }
        data.transform.SetParent(container, true);
        
        Bounds parentBound = container.GetComponent<Renderer>().bounds;
        float len;
        float newPos;
        switch (type)
        {
            case "Input":
                len = data.GetComponent<Renderer>().bounds.size.x;
                newPos = parentBound.min.x + spacing + len / 2 + (len + spacing) * idx;
                data.transform.position = new Vector3(newPos, parentBound.center.y, parentBound.center.z);
                break;
            case "Output":
                len = data.GetComponent<Renderer>().bounds.size.x;
                newPos = parentBound.max.x - spacing - len / 2 - (len + spacing) * idx;
                data.transform.position = new Vector3(newPos, parentBound.center.y, parentBound.center.z);
                break;
            case "Stack":
                len = data.GetComponent<Renderer>().bounds.size.y;
                newPos = parentBound.max.y - spacing - len / 2 - (len + spacing) * idx;
                data.transform.position = new Vector3(parentBound.center.x, newPos, parentBound.center.z);
                break;
            case "Queue":
                len = data.GetComponent<Renderer>().bounds.size.y;
                newPos = parentBound.min.y + spacing + len / 2 + (len + spacing) * idx;
                data.transform.position = new Vector3(parentBound.center.x, newPos, parentBound.center.z);
                break;
            case "Add":
                len = data.GetComponent<Renderer>().bounds.size.y;
                newPos = parentBound.max.y - spacing - len / 2 - (len + spacing) * idx;
                data.transform.position = new Vector3(parentBound.center.x, newPos, parentBound.center.z);
                break;
            default:
                data.transform.position = parentBound.center;
                break;
        }
    }
}
