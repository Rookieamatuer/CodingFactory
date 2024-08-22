using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMethod : MonoBehaviour
{
    public void ExecuteMethod()
    {
        switch(gameObject.tag)
            {
            case "Input":
                ExecuteInput();
                break;
            case "Output":
                ExecuteOutput();
                break;
            case "Array":
                ExecuteArray();
                break;
            default:
                Debug.Log("None");
                break;
            }
    }

    // Array
    public void ExecuteArray()
    {
        Debug.Log("this is array");
    }

    public void ExecuteInput()
    {
        if (LevelManager.instance.inputBox.childCount == 0) {
            LevelManager.instance.terminate = true;
            Debug.Log("Out of range");
            return;
        }
        LevelManager.instance.inputBox.GetChild(0).SetParent(LevelManager.instance.Processor, false);
        //LevelManager.instance.numbers[LevelManager.instance.index] = LevelManager.instance.inputBox.GetChild(0).gameObject;
        //LevelManager.instance.numbers[LevelManager.instance.index].transform.SetParent(LevelManager.instance.Processor, false);
        //LevelManager.instance.index++;
        //transform.localPosition = Vector2.zero;

        //LevelManager.instance.numbers[LevelManager.instance.index].transform.SetParent(LevelManager.instance.Processor, false);

        //LevelManager.instance.index--;
        //transform.localPosition = Vector2.zero;
    }

    public void ExecuteOutput()
    {
        if (LevelManager.instance.Processor.childCount == 0)
        {
            LevelManager.instance.terminate = true;
            Debug.Log("No data");
            return;
        }
        LevelManager.instance.Processor.GetChild(0).SetParent(LevelManager.instance.outputBox, false);
        //LevelManager.instance.numbers[0] = null;
        //LevelManager.instance.index--;
        transform.localPosition = Vector2.zero;
    }

    public void ExecuteForLoop()
    {
        Debug.Log("for");
        //LevelManager.instance.block
    }
}
