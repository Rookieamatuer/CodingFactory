using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DraftGameManager : MonoBehaviour
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
    [SerializeField] Transform addOn;
    [SerializeField] Transform add;

    [SerializeField] Transform block;

    public Transform infoText=null;

    [SerializeField] GameObject guideUI;
    [SerializeField] GameObject gameExplanation=null;

    [SerializeField] int currentIndex = 0;
    [SerializeField] int codeIndex;
    [SerializeField] int codeCount;

    List<string> operationList;

    bool stackInUse;
    bool queueInUse;
    bool addInUse;

    static bool isRestart;

    public int numCount;
    private OperationManager operationManager;

    public static DraftGameManager instance;

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
    // Start is called before the first frame update
    void Start()
    {
        operationManager = new OperationManager(data, target, inputArea, outputArea, targetArea, stackOn, stack, queueOn, queue, addOn, add);
        operationList = new List<string>();
        stackInUse = false;
        queueInUse = false;
        addInUse = false;
        if (isRestart)
        {
            guideUI.gameObject.SetActive(false);
            isRestart = !isRestart;
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
            if (currentCode.tag == "StackOn")
            {
                currentCode.GetComponentInChildren<Text>().text = stackInUse ? "StackOff" : "StackOn";
                stackInUse = !stackInUse;
            } else if (currentCode.tag == "QueueOn")
            {
                currentCode.GetComponentInChildren<Text>().text = queueInUse ? "QueueOff" : "QueueOn";
                queueInUse = !queueInUse;
            } else if (currentCode.tag == "AddOn")
            {
                currentCode.GetComponentInChildren<Text>().text = addInUse ? "AddOff" : "AddOn";
                addInUse = !addInUse;
            } else
            {
                currentCode.GetComponentInChildren<Text>().text = currentCode.tag;
            }

            operationList.Add(obj.tag);

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

    public void ShowExplanation()
    {
        if (gameExplanation != null) 
            gameExplanation.SetActive(true);
    }

    public void HideExplanation()
    {
        if (gameExplanation != null)
            gameExplanation.SetActive(false);
    }

    private IEnumerator StepExecute()
    {
        if (currentIndex < codeIndex)
        {
            yield return new WaitForSeconds(0.5f);
            operationManager.AddOperation(operationList[currentIndex]);
            if (currentIndex > 0)
            {
                block.GetChild(currentIndex - 1).GetComponent<Image>().color = Color.green;
            }
            block.GetChild(currentIndex++).GetComponent<Image>().color = Color.cyan;

        }
        if (CheckAnswer())
        {
            Debug.Log("Success!");
            guideUI.GetComponent<GuidePanel>().SuccessMessage();
        }
    }

    private IEnumerator DelayProcess()
    {
        //foreach (Transform slot in block)
        for (int i = currentIndex; i < block.childCount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Transform slot = block.GetChild(i);
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
            guideUI.GetComponent<GuidePanel>().ErrorMessage();
        }
    }

    // execute button event
    public void ExecuteAll()
    {
        StartCoroutine(DelayProcess());
    }

    public void ExecuteByStep()
    {
        StartCoroutine(StepExecute());
    }

    public void ResetLevel()
    {
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
