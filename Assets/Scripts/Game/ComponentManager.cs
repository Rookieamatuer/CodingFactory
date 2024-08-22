using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComponentManager 
{
    Transform stack;
    Transform queue;
    //Transform block;
    Transform input;
    Transform output;
    Transform tmpArea;

    Transform currentData;
    Transform currentComponent;
    Transform targetComponent;

    List<Transform> sList = null;
    List<Transform> qList = null;

    float tmpTime;

    //bool stOn;
    //bool qOn;

    //int maxVisible = 14;
    //int count = 0;
    
    public ComponentManager(Transform inputT=null, Transform outputT=null, 
        Transform tmp=null, Transform st=null, Transform q=null, 
        List<Transform> stackList=null, List<Transform> queueList=null)
    {
        input = inputT; 
        output = outputT;
        tmpArea = tmp;
        stack = st;
        queue = q;
        currentComponent = tmp;
        targetComponent = outputT;
        sList = stackList;
        qList = queueList;
    }

    public void DataInit(string dataString, Transform t)
    {
        for (int i = 0; i < dataString.Length; ++i)
        {
            GameObject tmp = (GameObject)Object.Instantiate(Resources.Load("Prefabs/2DFormat/Data"));
            tmp.name = "Data " + i;
            tmp.GetComponentInChildren<TextMeshPro>().text = dataString[i].ToString();
            t.GetComponent<ComponentBase>().InputData(tmp.transform);
        }
    }

    public void InputCommand(Transform dataT, Transform targetT)
    {
        targetT.GetComponent<ComponentTmp>().InputData(dataT);
    }

    public void OutputCommand(Transform inputData, Transform outputT)
    {
        outputT.GetComponent<ComponentTmp>().InputData(inputData);
    }

    public IEnumerator ExecuteCommand(string command, float t, bool isHanoi)
    {
        switch (command)
        {
            case "Input":
                //yield return new WaitForSeconds(.5f);
                //Debug.Log("command: " + currentComponent.name);
                if (currentComponent == null) break;
                currentComponent.GetComponent<ComponentBase>().MoveToInput(currentComponent.GetComponent<ComponentBase>().inputPoint);
                if (currentComponent.GetComponent<ComponentBase>().moveToInput)
                {
                    yield return new WaitForSeconds(t);
                    //currentData = input.GetComponent<ComponentBase>().OutputData();
                    //currentComponent.GetComponent<ComponentBase>().InputData(currentData);
                }
                //else
                {
                    currentData = input.GetComponent<ComponentBase>().OutputData();
                    currentComponent.GetComponent<ComponentBase>().InputData(currentData);
                    //yield return new WaitForSeconds(t * .5f);
                }
                
                //if (stOn)
                //{
                //    currentData = input.GetComponent<ComponentInput>().OutputData();
                //    stack.GetComponent<ComponentBase>().InputData(currentData);
                //} else if (qOn)
                //{
                //    currentData = input.GetComponent<ComponentBase>().OutputData();
                //    stack.GetComponent<ComponentBase>().InputData(currentData);
                //} else
                //{
                //    currentData = input.GetComponent<ComponentBase>().OutputData();
                //    tmpArea.GetComponent<ComponentBase>().InputData(currentData);
                //}
                break;
            case "Output":
                Debug.Log("command: " + currentComponent.name + " " + currentComponent.GetComponent<ComponentBase>().isEmpty());
                if (currentComponent != null && !currentComponent.GetComponent<ComponentBase>().isEmpty())
                {
                    tmpArea.GetComponent<ComponentBase>().MoveToOutput(tmpArea.GetComponent<ComponentBase>().outputPoint);
                    if (currentComponent.GetComponent<ComponentBase>().moveToOutput)
                        yield return new WaitForSeconds(t);
                    //else
                        //yield return new WaitForSeconds(t * .5f);
                    currentData = currentComponent.GetComponent<ComponentBase>().OutputData();
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                    //yield return new WaitForSeconds(t * .5f);
                }
                break;
            case "StackOn":
                if (queue != null) QueueOffMethod(queue);
                StackOnMethod(stack);
                break;
            case "StackOff":
                StackOffMethod(stack);
                break;
            case "StackOutput":
                if (!stack.GetComponent<ComponentBase>().isEmpty())
                {
                    Debug.Log("in");
                    float posX = targetComponent.tag == "Output" ? stack.GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    stack.GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = stack.GetComponent<ComponentBase>().OutputData();
                    Debug.Log(currentData.name + " outputTime: " + (Time.time - tmpTime));
                    tmpTime = Time.time;
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            case "QueueOn":
                //qOn = true;
                //stOn = false;
                if (stack != null) StackOffMethod(stack);
                QueueOnMethod(queue);
                break;
            case "QueueOff":
                //qOn = false;
                QueueOffMethod(queue);
                break;
            case "QueueOutput":
                if (!queue.GetComponent<ComponentBase>().isEmpty())
                {
                    float posX = targetComponent.tag == "Output" ? queue.GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    queue.GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = queue.GetComponent<ComponentBase>().OutputData();
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            case "Stack 1 On":
                if (sList != null)
                {
                    StackOffMethod(sList[1]);
                    if (sList.Count > 2)
                        StackOffMethod(sList[2]);
                    StackOnMethod(sList[0]);
                }
                break;
            case "Stack 2 On":
                if (sList.Count >= 2)
                {
                    StackOffMethod(sList[0]);
                    if (sList.Count > 2)
                        StackOffMethod(sList[2]);
                    StackOnMethod(sList[1]);
                }
                break;
            case "Stack 3 On":
                if (sList.Count >= 3)
                {
                    StackOffMethod(sList[0]);
                    StackOffMethod(sList[1]);
                    StackOnMethod(sList[2]);
                }
                break;
            case "Stack 1 Off":
                if (sList != null)
                {
                    StackOffMethod(sList[0]);
                }
                break;
            case "Stack 2 Off":
                if (sList != null)
                {
                    StackOffMethod(sList[1]);
                }
                break;
            case "Stack 3 Off":
                if (sList != null)
                {
                    StackOffMethod(sList[2]);
                }
                break;
            case "Stack 1 Output":
                if (isHanoi && IsDefaultTarget()) break;
                if (sList != null && !sList[0].GetComponent<ComponentBase>().isEmpty())
                {
                    float posX = targetComponent.tag == "Output" ? sList[0].GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    sList[0].GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = sList[0].GetComponent<ComponentBase>().OutputData();
                    //Debug.Log(currentData.name + " outputTime: " + (Time.time - tmpTime));
                    //tmpTime = Time.time;
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            case "Stack 2 Output":
                if (isHanoi && IsDefaultTarget()) break;
                if (sList.Count >= 2 && !sList[1].GetComponent<ComponentBase>().isEmpty())
                {
                    float posX = targetComponent.tag == "Output" ? sList[1].GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    sList[1].GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = sList[1].GetComponent<ComponentBase>().OutputData();
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            case "Stack 3 Output":
                if (isHanoi && IsDefaultTarget()) break;
                if (sList.Count >= 3 && !sList[2].GetComponent<ComponentBase>().isEmpty())
                {
                    float posX = targetComponent.tag == "Output" ? sList[2].GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    sList[2].GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = sList[2].GetComponent<ComponentBase>().OutputData();
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            case "Queue 1 On":
                if (qList.Count > 1)
                    QueueOffMethod(qList[1]);
                if (qList != null)
                    QueueOnMethod(qList[0]);
                break;
            case "Queue 2 On":
                if (qList.Count > 1)
                {
                    QueueOffMethod(qList[0]);
                    QueueOnMethod(qList[1]);
                }
                break;
            case "Queue 1 Off":
                if (qList != null)
                    QueueOffMethod(qList[0]);
                break;
            case "Queue 2 Off":
                if (qList.Count > 1)
                    QueueOffMethod(qList[1]);
                break;
            case "Queue 1 Output":
                if (qList != null && !qList[0].GetComponent<ComponentBase>().isEmpty())
                {
                    float posX = targetComponent.tag == "Output" ? qList[0].GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    qList[0].GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = qList[0].GetComponent<ComponentBase>().OutputData();
                    //Debug.Log(currentData.name + " outputTime: " + (Time.time - tmpTime));
                    //tmpTime = Time.time;
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            case "Queue 2 Output":
                if (qList != null && !qList[1].GetComponent<ComponentBase>().isEmpty())
                {
                    float posX = targetComponent.tag == "Output" ? qList[1].GetComponent<ComponentBase>().outputPoint : targetComponent.position.x;
                    qList[1].GetComponent<ComponentBase>().MoveToOutput(posX);

                    yield return new WaitForSeconds(t);

                    currentData = qList[1].GetComponent<ComponentBase>().OutputData();
                    //Debug.Log(currentData.name + " outputTime: " + (Time.time - tmpTime));
                    //tmpTime = Time.time;
                    targetComponent.GetComponent<ComponentBase>().InputData(currentData);
                    currentData = null;
                }
                break;
            default:
                currentComponent = tmpArea;
                break;
        }
        
    }

    private void StackOnMethod(Transform s)
    {
        s.GetComponent<ComponentStack>().InputControl(true);
        currentComponent = s;
        targetComponent = s;
    }

    private void StackOffMethod(Transform s)
    {
        s.GetComponent<ComponentBase>().InputControl(false);
        currentComponent = tmpArea;
        targetComponent = output;
    }

    private void QueueOnMethod(Transform q)
    {
        q.GetComponent<ComponentQueue>().InputControl(true);
        currentComponent = q;
        targetComponent = q;
    }

    private void QueueOffMethod(Transform s)
    {
        s.GetComponent<ComponentBase>().InputControl(false);
        currentComponent = tmpArea;
        targetComponent = output;
    }


    public void ChangeComponentSpeed(float t)
    {
        if (tmpArea != null)
        {
            tmpArea.GetComponent<ComponentBase>().intervalTime = t;
        }
        if (stack != null)
        {
            stack.GetComponent<ComponentBase>().intervalTime = t;
        } 
        if (queue != null)
        {
            queue.GetComponent<ComponentBase>().intervalTime = t;
        } 
        if (sList.Count > 0)
        {
            foreach (Transform s in sList)
            {
                s.GetComponent<ComponentBase>().intervalTime = t;
            }
        }
        if (qList.Count > 0)
        {
            foreach (Transform q in qList)
            {
                q.GetComponent<ComponentBase>().intervalTime = t;
            }
        }
    }

    public bool IsDefaultTarget()
    {
        return targetComponent == output;
    }
}
