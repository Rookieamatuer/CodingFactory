using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] string data;
    [SerializeField] string target;
    [SerializeField] Transform inputArea;
    [SerializeField] Transform outputArea;
    [SerializeField] Transform targetArea;
    [SerializeField] Transform stackOn;
    [SerializeField] Transform stack;
    [SerializeField] Transform queueOn;
    [SerializeField] Transform queue;

    [SerializeField] Transform block;

    [SerializeField] GameObject guideUI;

    [SerializeField] int codeIndex;
    [SerializeField] int codeCount;

    public int numCount;
    private OperationManager operationManager;
    // Start is called before the first frame update
    void Start()
    {
        operationManager = new OperationManager(data, target, inputArea, outputArea, targetArea, stackOn, stack, queueOn, queue);
    }

    private IEnumerator DelayProcess()
    {
        foreach (Transform slot in block)
        {
            yield return new WaitForSeconds(0.5f);
            if (slot.tag == "Untagged") break;
            operationManager.AddOperation(slot.tag);
        }
        if (CheckAnswer())
        {
            Debug.Log("Success!");
            guideUI.GetComponent<GuidePanel>().SuccessMessage();
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }

    public void AddOperation(GameObject button)
    {
        if (codeIndex < codeCount)
        {
            Transform currentCode = block.GetChild(codeIndex);
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            currentCode.GetComponent<Image>().color = Color.green;
            currentCode.tag = obj.tag;
            currentCode.GetComponentInChildren<Text>().text = currentCode.tag;
            
            codeIndex++;
            //Debug.Log("name:" + button.name);
            //Debug.Log("tag:" + button.tag);
        }
        else
        {
            Debug.Log("Out of range");
        }
    }

    public bool CheckAnswer()
    {
        if (outputArea.childCount != target.Length) return false;
        for (int i = 0; i < outputArea.childCount; ++i)
        {
            if (outputArea.GetChild(i).GetComponentInChildren<Text>().text != target[i].ToString())
            {
                return false;
            }
        }
        return true;
    }

    // execute button event
    public void ExecuteAll()
    {
        StartCoroutine(DelayProcess());
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
