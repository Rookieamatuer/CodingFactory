using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoTextController : MonoBehaviour
{
    public static InfoTextController instance_;
    public Transform uitextobj;
    public Text text;
    private void Awake()
    {
        instance_ = this;
    }
}