using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Transform block;

    public Transform inputBox;
    public Transform Processor;
    public Transform outputBox;

    public static LevelManager instance;

    [Header("count")]
    public int inputComponentsCount;
    public int outputComponentsCount;
    public int forLoopComponentCount;
    public int dataNum;

    [Header("data")]
    public GameObject[] numbers;
    public int index;

    public bool terminate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dataNum = 3;
        inputComponentsCount = 1;
        outputComponentsCount = 1;

        numbers = GameObject.FindGameObjectsWithTag("number");
        index = 0;

        terminate = false;
    }

    public void ExecuteAll()
    {
        
        StartCoroutine(DelayProcess());
        if (outputBox.childCount == dataNum)
        {
            Debug.Log("Success!");
        } else
        {
            Debug.Log("Failed");
        }
    }

    private IEnumerator DelayProcess()
    {
        int n = block.childCount;
        for (int i = 0; i < n; ++i)
        {
            Transform currentCode = block.GetChild(i);
            if (currentCode.tag == "loop" && outputBox.childCount < dataNum)
            {
                i = 0;
                continue;
            }
            ComponentMethod componentMethod = currentCode.GetComponent<ComponentMethod>();
            yield return new WaitForSeconds(0.5f);
            if (componentMethod != null)
            {
                componentMethod.ExecuteMethod();
            }
        }
        //foreach (Transform slot in block)
        //{
        //    ComponentMethod componentMethod = slot.GetComponent<ComponentMethod>();
        //    yield return new WaitForSeconds(0.5f);
        //    if (componentMethod != null)
        //    {
        //        componentMethod.ExecuteMethod();
        //    }
        //}
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
