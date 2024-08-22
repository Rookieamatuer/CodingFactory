using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform block;
    [SerializeField] Transform input;
    [SerializeField] Transform output;
    [SerializeField] Transform tmpArea=null;
    [SerializeField] Transform stack = null;
    [SerializeField] Transform queue = null;
    [SerializeField] Transform currentData;

    [Header("Execute Button")]
    [SerializeField] Button executeBtn;
    [SerializeField] Button stepBtn;

    [Header("Component List for level with duplicate components")]
    [SerializeField] List<Transform> stackList;
    [SerializeField] List<Transform> queueList;

    [SerializeField] string rawData;
    [SerializeField] string target;
    [SerializeField] float intervalTime = 1f;
    static bool isRestart;
    bool isAccelerate;

    bool isWorking;

    [SerializeField] bool isHanoiLevel=false;

    float tmpTime;

    // Instruction
    [SerializeField] GameObject guideUI = null;
    [SerializeField] GameObject gameExplanation = null;
    [SerializeField] GameObject targetArea = null;
    public Transform infoText = null;

    public int currentCommand { get; private set; }

    ComponentManager componentManager;
    public static GameManager instance;

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
        componentManager = new ComponentManager(input, output, tmpArea, stack, queue, stackList, queueList);
        if (isHanoiLevel)
        {
            componentManager.DataInit(rawData, stackList[0]);
        }
        else
        {
            componentManager.DataInit(rawData, input);
        }

        if (targetArea != null)
        {
            for (int i = 1; i <= target.Length; ++i)
            {
                GameObject obj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/num"));
                obj.name = "target" + i;
                obj.GetComponentInChildren<Text>().text = target[i - 1].ToString();
                obj.transform.SetParent(targetArea.transform);
            }
        }

        if (isRestart)
        {
            guideUI.gameObject.SetActive(false);
            isRestart = !isRestart;
        }

        tmpTime = Time.time;
    }

    private void FixedUpdate()
    {
        // restrict click interval time
        if (isWorking)
        {
            tmpTime += Time.deltaTime;
            
            if (tmpTime > intervalTime * 1.5f)
            {
                tmpTime = 0;
                executeBtn.enabled = true;
                stepBtn.enabled = true;
                isWorking = false;
            }
        }
    }

    public bool CheckAnswer()
    {
        Transform tmp = isHanoiLevel ? stackList[2].GetChild(0) : output;
        if (tmp.childCount != target.Length)
        {
            return false;
        }
        for (int i = 0; i < tmp.childCount; ++i)
        {
            if (tmp.GetChild(tmp.childCount - 1 - i).GetComponentInChildren<TextMeshPro>().text != target[i].ToString())
            {
                Debug.Log(i);
                return false;
            }
        }
        return true;
    }

    public bool CheckHanoi()
    {
        for (int i = 0; i < stackList.Count; ++i)
        {
            if (stackList[i].GetChild(0).childCount < 2) continue;
            //if (int.Parse(stackList[i].GetChild(0).GetChild(stackList[i].childCount - 1).GetComponentInChildren<TextMeshPro>().text) < int.Parse(stackList[i].GetChild(0).GetChild(stackList[i].childCount - 2).GetComponentInChildren<TextMeshPro>().text))
            if (stackList[i].GetChild(0).GetChild(stackList[i].childCount - 1).GetComponentInChildren<TextMeshPro>().text.CompareTo(stackList[i].GetChild(0).GetChild(stackList[i].childCount - 2).GetComponentInChildren<TextMeshPro>().text) < 0)
            {
                return false;
            }
        }
        return true;
    }

    public IEnumerator ExecuteCommand()
    {
        if (isHanoiLevel && !CheckHanoi())
        {
            Debug.Log("Wrong!");
        }
        List<GameObject> commands = block.GetComponent<CommandManager>().commands;
        for (; currentCommand < commands.Count; currentCommand++)
        {
            isWorking = true;
            executeBtn.enabled = false;
            stepBtn.enabled = false;
            string txt = commands[currentCommand].GetComponentInChildren<Text>().text;
            block.GetChild(0).GetChild(0).GetChild(currentCommand).GetComponent<Image>().color = Color.cyan;
            if (currentCommand > 0)
            {
                block.GetChild(0).GetChild(0).GetChild(currentCommand - 1).GetComponent<Image>().color = Color.green;
            }
            
            Debug.Log("id: " + currentCommand + " time: " + (Time.time - tmpTime));
            tmpTime = Time.time;
            StartCoroutine(componentManager.ExecuteCommand(txt, intervalTime, isHanoiLevel));
            yield return new WaitForSeconds(intervalTime * 1.5f);
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

    private IEnumerator ExecuteCommandByStep()
    {
        List<GameObject> commands = block.GetComponent<CommandManager>().commands;
        if (currentCommand < commands.Count)
        {
            //yield return new WaitForSeconds(0.5f);
            isWorking = true;
            executeBtn.enabled = false;
            stepBtn.enabled = false;

            string txt = commands[currentCommand].GetComponentInChildren<Text>().text;
            block.GetChild(0).GetChild(0).GetChild(currentCommand).GetComponent<Image>().color = Color.cyan;
            if (currentCommand > 0)
            {
                block.GetChild(0).GetChild(0).GetChild(currentCommand - 1).GetComponent<Image>().color = Color.green;
            }
            StartCoroutine(componentManager.ExecuteCommand(txt, intervalTime, isHanoiLevel));
            yield return new WaitForSeconds(intervalTime * 1.5f);
            ++currentCommand;

        }
        if (CheckAnswer())
        {
            Debug.Log("Success!");
            guideUI.GetComponent<GuidePanel>().SuccessMessage();
        }
    }

    public void ExecuteAll()
    {
        StartCoroutine(ExecuteCommand()); 
    }

    public void ExecuteByStep()
    {
        StartCoroutine(ExecuteCommandByStep());
    }

    public void ResetLevel()
    {
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeAnimSpeed()
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if (isAccelerate)
        {
            intervalTime = 1f;
            componentManager.ChangeComponentSpeed(intervalTime);
            obj.GetComponent<Image>().color = Color.white;
            isAccelerate = false;
        } else
        {
            intervalTime = 0.5f;
            componentManager.ChangeComponentSpeed(intervalTime);
            obj.GetComponent<Image>().color = Color.green;
            isAccelerate = true;
        }
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
}
