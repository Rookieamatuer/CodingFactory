using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DraggedPart : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector2 originalPosition;
    private GameObject newComponent;

    private void Awake()
    {
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        transform.SetParent(transform.root, true);
        
    }

    private void ChooseComponent()
    {
        switch(gameObject.tag)
        {
            case "Input":
                InputComponent();
                break;
            case "Output":
                OutputComponent();
                break;
            case "ForLoop":
                ForLoopComponent(); 
                break;
            default:
                Debug.Log("None");
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ChooseComponent();
        transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Transform block = GameObject.FindGameObjectWithTag("blank").transform;
        if (RectTransformUtility.RectangleContainsScreenPoint(block as RectTransform, Input.mousePosition))
        {
            
            if (gameObject.tag == "ForLoop")
            {
                newComponent = (GameObject)Instantiate(Resources.Load("Prefabs/codeForLoop"));
                newComponent.GetComponentInChildren<Text>().text = "";
                newComponent.tag = "LoopEnd";
                newComponent.transform.SetParent(block, false);
                newComponent.transform.localPosition = Vector2.zero;
                //newComponent.transform.localPosition = Vector2.zero;
            }
            if (transform.parent != block)
                transform.SetParent(block, false);
        } else
        {
            //if (gameObject.tag == "Input")
            //    LevelManager.instance.inputComponentsCount--;
            //else if (gameObject.tag == "Output")
            //    LevelManager.instance.outputComponentsCount--;
            transform.SetParent(originalParent, false);
            transform.localPosition = originalPosition;
        }
    }

    private void InputComponent()
    {
        if (LevelManager.instance.inputComponentsCount < LevelManager.instance.dataNum && gameObject.transform.parent == originalParent)
        {
            newComponent = (GameObject)Instantiate(Resources.Load("Prefabs/codeInput"), transform.position, Quaternion.identity, transform.parent);
            newComponent.transform.name = transform.name + LevelManager.instance.inputComponentsCount;
            LevelManager.instance.inputComponentsCount++;
            //Debug.Log("Instantiate " + gameObject.name + ":" + LevelManager.instance.inputComponentsCount);
        }
    }

    private void OutputComponent()
    {
        if (LevelManager.instance.outputComponentsCount < LevelManager.instance.dataNum && gameObject.transform.parent == originalParent)
        {
            newComponent = (GameObject)Instantiate(Resources.Load("Prefabs/codeOutput"), transform.position, Quaternion.identity, transform.parent);
            newComponent.transform.name = transform.name + LevelManager.instance.outputComponentsCount;
            LevelManager.instance.outputComponentsCount++;
            //Debug.Log("Instantiate " + gameObject.name + ":" + LevelManager.instance.inputComponentsCount);
        }
    }

    private void ForLoopComponent()
    {
        if (LevelManager.instance.forLoopComponentCount < 2 && gameObject.transform.parent == originalParent)
        {
            newComponent = (GameObject)Instantiate(Resources.Load("Prefabs/codeForLoop"), transform.position, Quaternion.identity, transform.parent);
        }
    }
}
