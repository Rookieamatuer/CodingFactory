using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] Transform inputT;
    [SerializeField] Transform outputT;
    [SerializeField] Transform stack;
    [SerializeField] Transform queue;
    [SerializeField] Transform add;

    [SerializeField] Transform outputLine = null;
    [SerializeField] Transform outputLine1 = null;
    [SerializeField] Transform outputLine2 = null;

    Transform target;

    [SerializeField] string data;
    
    public void ComponentInActive(string name)
    {
        switch(name)
        {
            case "Stack":
                stack.gameObject.SetActive(true);
                if (outputLine != null) outputLine.gameObject.SetActive(true);
                target = stack.GetChild(0);
                data = "123";
                break;
            case "Queue":
                queue.gameObject.SetActive(true);
                if (outputLine1 != null && outputLine2 != null)
                {
                    outputLine1.gameObject.SetActive(true);
                    outputLine2.gameObject.SetActive(true);
                }
                target = queue.GetChild(0);
                data = "123";
                break;
            case "Add":
                add.gameObject.SetActive(true);
                target = add.GetChild(0);
                data = "12";
                break;
        }
    }

    public void TutorialInit(string name)
    {
        ComponentInActive(name);
        for (int i = 1; i <= data.Length; ++i)
        {
            GameObject obj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/num"));
            obj.name = "data" + i;
            obj.GetComponentInChildren<Text>().text = data[i - 1].ToString();
            obj.transform.SetParent(inputT);
        }
    }

    public IEnumerator ProcessInput()
    {
        while (inputT.childCount > 0) 
        {
            yield return new WaitForSeconds(0.5f);
            Transform tmp = inputT.GetChild(inputT.childCount - 1);
            tmp.SetParent(target);
            tmp.SetAsFirstSibling();
        }
        
    }

    public IEnumerator ProcessStackOutput()
    {
        yield return new WaitForSeconds(2f);
        while (target.childCount != 0)
        {
            yield return new WaitForSeconds(0.5f);
            Transform tmp = target.GetChild(0);
            tmp.SetParent(outputT);
            tmp.SetAsFirstSibling();
        }
    }

    public IEnumerator ProcessQueueOutput()
    {
        yield return new WaitForSeconds(2f);
        while (target.childCount != 0)
        {
            yield return new WaitForSeconds(0.5f);
            Transform tmp = target.GetChild(target.childCount - 1);
            tmp.SetParent(outputT);
            tmp.SetAsFirstSibling();
        }
    }

    public IEnumerator ProcessAddOutput()
    {
        yield return new WaitForSeconds(2f);

        Transform tmp = target.GetChild(0);
        tmp.GetComponentInChildren<Text>().text = "3";
        tmp.SetParent(outputT);
        tmp.SetAsFirstSibling();
        Destroy(target.GetChild(0).gameObject);
    }

    public void ProcessAnim(string name)
    {
        StartCoroutine(ProcessInput());
        switch (name)
        {
            case "Stack":
                StartCoroutine(ProcessStackOutput());
                break;
            case "Queue":
                StartCoroutine(ProcessQueueOutput());
                break;
            case "Add":
                StartCoroutine(ProcessAddOutput());
                break;
        }
    }

    public void ShowAnim(string name)
    {
        TutorialInit(name);
        ProcessAnim(name);
    }

    public void CloseUI()
    {
        Destroy(gameObject);
    }

    public void RecycleData(Transform t)
    {
        while (t.childCount != 0)
        {
            Destroy(t.GetChild(0).gameObject);
        }
    }
}
