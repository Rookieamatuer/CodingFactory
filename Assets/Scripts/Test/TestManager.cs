using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public static TestManager instance;

    public Transform startArea;
    public Transform resultArea;
    public Transform stackComponent;
    public Transform queueComponent;
    public bool stackOn;
    public bool queueOn;
    private bool stackMark;
    private bool queueMark;
    public Transform block;
    public int codeIndex;
    public int codeCount;

    private int[] answer = { 3, 6, 4, 5, 2, 1 };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        codeIndex = 0;
        codeCount = block.childCount;
        stackOn = false;
        queueOn = false;
        stackMark = false;
        queueMark = false;
    }

    private IEnumerator DelayProcess()
    {
        foreach (Transform slot in block)
        {
            yield return new WaitForSeconds(0.5f);
            if (slot.tag == "Untagged") break;
            ExecuteOperation(slot.tag);
        }
        if (CheckAnswer())
        {
            Debug.Log("Success!");
        } else
        {
            Debug.Log("Wrong!");
        }
    }

    public void AddOperation(GameObject button)
    {
        if (codeIndex < codeCount)
        {
            Transform currentCode = block.GetChild(codeIndex);
            currentCode.GetComponent<Image>().color = Color.green;
            currentCode.tag = button.tag;
            currentCode.GetComponentInChildren<Text>().text = currentCode.tag;
            if (currentCode.tag == "StackOn")
            {
                if (stackMark) 
                    currentCode.GetComponentInChildren<Text>().text = "StackOff";
                stackMark = !stackMark;
            }
            if (currentCode.tag == "QueueOn")
            {
                if (queueMark)
                    currentCode.GetComponentInChildren<Text>().text = "QueueOff";
                queueMark = !queueMark;
            } 
            codeIndex++;
            //Debug.Log("name:" + button.name);
            //Debug.Log("tag:" + button.tag);
        } else
        {
            Debug.Log("Out of range");
        }
    }

    public void ExecuteOperation(string codeTag)
    {
        switch (codeTag)
        {
            case "Output":
                OutputOperation();
                break;
            case "StackOutput":
                //
                if (queueOn)
                    stackComponent.gameObject.GetComponent<StackComponent>().OutputData(queueComponent);
                else
                    stackComponent.GetComponent<StackComponent>().OutputData(resultArea);
                break;
            case "QueueOutput":
                queueComponent.GetComponent<QueueComponent>().OutputData(resultArea);
                break;
            case "StackOn":
                stackOn = !stackOn;
                break;
            case "QueueOn":
                queueOn = !queueOn;
                break;
            default:
                Debug.Log("None");
                break;
        }
    }

    public void OutputOperation()
    {
        Transform currentData = startArea.GetChild(0);
        Transform targetComponent = resultArea;
        //if (targetComponent == startArea && stackOn)
        //{
        //    currentData.SetParent(stackComponent, false);
        //    currentData.localPosition = Vector2.zero;
        //} else if (queueOn)
        //{
        //    currentData.SetParent(queueComponent, false);
        //    currentData.localPosition = Vector2.zero;
        //} else
        if (stackOn)
        {
            targetComponent = stackComponent;
        } else if (queueOn)
        {
            targetComponent = queueComponent;
        }
        currentData.SetParent(targetComponent, false);
        currentData.localPosition = Vector2.zero;
        
    }

    public bool CheckAnswer()
    {
        for (int i = 0; i < resultArea.childCount; ++i)
        {
            if (int.Parse(resultArea.GetChild(i).GetComponentInChildren<Text>().text) != answer[i])
            {
                return false;
            }
        }
        return true;
    }

    public void ExecuteAll()
    {
        StartCoroutine(DelayProcess());
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
