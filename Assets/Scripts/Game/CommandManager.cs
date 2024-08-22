using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using System;

public class CommandManager : MonoBehaviour
{
    [SerializeField] int count;
    [SerializeField] int maxVisible;
    [SerializeField] Transform content;

    bool stOn;
    bool stOn1;
    bool stOn2;
    bool stOn3;
    List<string> stackStatus;

    bool qOn;
    bool qOn1;
    bool qOn2;
    List<string> queueStatus;

    public List<GameObject> commands {  get; private set; }

    private void Start()
    {
        commands = new List<GameObject>();
        stackStatus = new List<string>();
        queueStatus = new List<string>();
    }

    public void AddCommand()
    {
        GameObject command = (GameObject)Instantiate(Resources.Load("Prefabs/Code"));
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        command.transform.SetParent(content);
        count++;
        content.GetComponent<ContentSizeFitter>().verticalFit = (count >= maxVisible ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained);
        
        if (obj.tag == "StackOn")
        {
            if (obj.name != "StackOn")
            {
                StackListCommand(obj, command);
            }
            else
            {
                command.GetComponentInChildren<Text>().text = stOn ? "StackOff" : "StackOn";
                stOn = !stOn;
            }
            if (stOn && qOn) qOn = false;
        }
        else if (obj.tag == "QueueOn" )
        {
            if (obj.name != "QueueOn")
            {
                QueueListCommand(obj, command);
            }
            else
            {
                command.GetComponentInChildren<Text>().text = qOn ? "QueueOff" : "QueueOn";
                qOn = !qOn;

            }
            if (qOn && stOn) stOn = false;
        } else
        {
            command.GetComponentInChildren<Text>().text = obj.GetComponentInChildren<Text>().text;
        }
        commands.Add(command);
    }

    private void StackListCommand(GameObject obj, GameObject command)
    {
        if (obj.name == "Stack 1 On")
        {
            command.GetComponentInChildren<Text>().text = stOn1 ? "Stack 1 Off" : "Stack 1 On";
            stOn1 = !stOn1;
            if (stOn1)
            {
                stOn2 = false;
                stOn3 = false;
            }
        }
        else if (obj.name == "Stack 2 On")
        {
            command.GetComponentInChildren<Text>().text = stOn2 ? "Stack 2 Off" : "Stack 2 On";
            stOn2 = !stOn2;
            if (stOn2)
            {
                stOn1 = false;
                stOn3 = false;
            }
        }
        else if (obj.name == "Stack 3 On")
        {
            command.GetComponentInChildren<Text>().text = stOn3 ? "Stack 3 Off" : "Stack 3 On";
            stOn3 = !stOn3;
            if (stOn3)
            {
                stOn1 = false;
                stOn2 = false;
            }
        }

        char[] tmp = new char[3];
        tmp[0] = stOn1 ? '1' : '0';
        tmp[1] = stOn2 ? '1' : '0';
        tmp[2] = stOn3 ? '1' : '0';
        stackStatus.Add(string.Concat(tmp));
    }
    private void QueueListCommand(GameObject obj, GameObject command)
    {
        if (obj.name == "Queue 1 On")
        {
            command.GetComponentInChildren<Text>().text = qOn1 ? "Queue 1 Off" : "Queue 1 On";
            qOn1 = !qOn1;
            if (qOn1) qOn2 = false;
        }
        else if (obj.name == "Queue 2 On")
        {
            command.GetComponentInChildren<Text>().text = qOn2 ? "Queue 2 Off" : "Queue 2 On";
            qOn2 = !qOn2;
            if (qOn2) qOn1 = false;
        }

        char[] tmp = new char[2];
        tmp[0] = qOn1 ? '1' : '0';
        tmp[1] = qOn2 ? '1' : '0';
        queueStatus.Add(string.Concat(tmp));
    }

    public void RemoveCommand()
    {
        if (count < 0) return;
        if (commands[count - 1].GetComponentInChildren<Text>().text == "StackOff" || commands[count - 1].GetComponentInChildren<Text>().text == "StackOn")
        {
            stOn = !stOn;
        }
        else if (commands[count - 1].GetComponentInChildren<Text>().text == "QueueOff" || commands[count - 1].GetComponentInChildren<Text>().text == "QueueOn")
        {
            qOn = !qOn;
        }
        else if (commands[count - 1].GetComponentInChildren<Text>().text == "Stack 1 Off" || commands[count - 1].GetComponentInChildren<Text>().text == "Stack 1 On" 
            || commands[count - 1].GetComponentInChildren<Text>().text == "Stack 2 Off" || commands[count - 1].GetComponentInChildren<Text>().text == "Stack 2 On" 
            || commands[count - 1].GetComponentInChildren<Text>().text == "Stack 3 Off" || commands[count - 1].GetComponentInChildren<Text>().text == "Stack 3 On")
        {
            if (count == 1) 
            { 
                stOn1 = false;
                stOn2 = false;
                stOn3 = false;
            }
            else
            {
                stOn1 = stackStatus[count - 2][0] == '1' ? true : false;
                stOn2 = stackStatus[count - 2][1] == '1' ? true : false;
                stOn3 = stackStatus[count - 2][2] == '1' ? true : false;
            }
            stackStatus.RemoveAt(count - 1);
        }
        else if (commands[count - 1].GetComponentInChildren<Text>().text == "Queue 1 Off" || commands[count - 1].GetComponentInChildren<Text>().text == "Queue 1 On"
            || commands[count - 1].GetComponentInChildren<Text>().text == "Queue 2 Off" || commands[count - 1].GetComponentInChildren<Text>().text == "Queue 2 On")
        {
            if (count == 1)
            {
                qOn1 = false;
                qOn2 = false;
            }
            else
            {
                qOn1 = queueStatus[count - 2][0] == '1' ? true : false;
                qOn2 = queueStatus[count - 2][1] == '1' ? true : false;
            }
            queueStatus.RemoveAt(count - 1);
        }
        Destroy(commands[count - 1]);
        commands.RemoveAt(count - 1);
        
        count--;
    }
}
