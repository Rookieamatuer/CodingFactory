using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationManager
{
    enum OperationType
    {
        Input = 0,
        Output = 1,
        AddOn = 2,
        MinorOn = 3,
        StackOn = 4,
        QueueOn = 5,
        AddOutput = 6,
        MinorOutput = 7,
        StackOutput = 8, 
        QueueOutput = 9,
    }
    [SerializeField] string data;
    private List<int> opertaionList;

    Transform currentData;
    Transform inputTarget;
    Transform outputTarget;
    Transform output;
    Transform st;
    Transform stackOn;
    Transform q;
    Transform queueOn;
    Transform ad;
    Transform addOn;

    bool isStackOn;
    bool isQueueOn;
    bool isAddOn;

    public OperationManager(string s, string t, Transform inputT, Transform outputT, Transform target, 
                                                Transform stackO=null, Transform stack=null, 
                                                Transform queueO=null, Transform queue=null, 
                                                Transform addO=null, Transform add=null)
    {
        inputTarget = inputT;
        outputTarget = outputT;
        output = outputTarget;
        stackOn = stackO;
        st = stack;
        queueOn = queueO;
        q = queue;
        data = s;
        ad = add;
        addOn = addO;
        opertaionList = new List<int>();
        isStackOn = false;
        isQueueOn = false;
        isAddOn = false;

        for (int i = 1; i <= data.Length; ++i)
        {
            GameObject obj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/num"));
            obj.name = "data" + i;
            obj.GetComponentInChildren<Text>().text = data[i - 1].ToString();
            obj.transform.SetParent(inputT);
        }

        for (int i = 1; i <= t.Length; ++i)
        {
            GameObject obj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/num"));
            obj.name = "target" + i;
            obj.GetComponentInChildren<Text>().text = t[i - 1].ToString();
            obj.transform.SetParent(target);
        }
    }

    public void AddOperation(string opName)
    {
        switch(opName)
        {
            case "Output":
                InputAction();
                //if (output == outputTarget)
                    output = isStackOn ? st : outputTarget;
                if (output == outputTarget)
                    output = isQueueOn ? q : outputTarget;
                if (output == outputTarget && ad != null)
                    if (ad.childCount < 2)
                        output = isAddOn ? ad : outputTarget;
                OutputAction(output);
                break;
            case "StackOn":
                if (stackOn != null)
                {
                    StackOnAction(stackOn);
                }
                break;
            case "StackOutput":
                StackOutputAction();
                break;
            case "QueueOn":
                if (queueOn != null)
                {
                    QueueOnAction(queueOn);
                }
                break;
            case "QueueOutput":
                QueueOutputAction();
                break;
            case "AddOn":
                if (addOn != null)
                {
                    AddOnAction(addOn);
                }
                break;
            case "AddOutput":
                AddOutputAction();
                break;
        }
    }

    public void ProcessOperation()
    {
        for (int i = 0; i < opertaionList.Count; i++)
        {
            switch ((OperationType)opertaionList[i])
            {
                case OperationType.Input:
                    break;
            }
        }
    }

    private void InputAction()
    {
        if (inputTarget.childCount > 0)
        {
            currentData = inputTarget.GetChild(inputTarget.childCount - 1);
        } else
        {
            currentData = null;
        }
        
    }

    private void OutputAction(Transform t)
    {
        if (currentData != null)
        {
            currentData.SetParent(t);
            Debug.Log(t.name);

            currentData.transform.SetAsFirstSibling();
        }
        
    }

    private void OutputTarget(Transform t)
    {
        output = t;
    }

    private void StackOnAction(Transform t)
    {
        isStackOn = !isStackOn;
        if (isStackOn)
        {
            //OutputTarget(st);
            t.GetComponent<Image>().color = Color.green;
        } else
        {
            //OutputTarget(outputTarget);
            t.GetComponent<Image>().color = Color.red;
        }
    }

    private void StackOutputAction()
    {
        if (st.childCount > 0)
        {
            currentData = st.GetChild(0);
            if (ad != null && isAddOn)
            {
                OutputAction(ad);
            }
            else
            {
                OutputAction(outputTarget);
            }
        }
    }

    private void QueueOnAction(Transform t)
    {
        isQueueOn = !isQueueOn;
        if (isQueueOn)
        {
            //OutputTarget(st);
            t.GetComponent<Image>().color = Color.green;
        }
        else
        {
            //OutputTarget(outputTarget);
            t.GetComponent<Image>().color = Color.red;
        }
    }

    private void QueueOutputAction()
    {
        if (q.childCount > 0)
        {
            currentData = q.GetChild(q.childCount - 1);
            if (ad != null && isAddOn)
            {
                OutputAction(ad);
            }
            else
            {
                OutputAction(outputTarget);
            }
        }
    }

    private void AddOnAction(Transform t)
    {
        isAddOn = !isAddOn;
        if (isAddOn)
        {
            //OutputTarget(st);
            t.GetComponent<Image>().color = Color.green;
        }
        else
        {
            //OutputTarget(outputTarget);
            t.GetComponent<Image>().color = Color.red;
        }
    }

    private void AddOutputAction()
    {
        if (ad.childCount == 2)
        {
            currentData = ad.GetChild(0);
            int sum = 0;
            Transform tmp;
            for (int i = 0; i < ad.childCount; i++)
            {
                tmp = ad.GetChild(i);
                sum += int.Parse(tmp.GetComponentInChildren<Text>().text);
            }
            Object.Destroy(ad.GetChild(1).gameObject);
            currentData.GetComponentInChildren<Text>().text = sum.ToString();
            OutputAction(outputTarget);
        }
    }

    private void MinorOn(Transform t)
    {
        if (t.childCount < 2)
            OutputTarget(t);
    }
}
