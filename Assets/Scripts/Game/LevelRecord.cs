using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRecord : MonoBehaviour
{
    public static LevelRecord instance;

    private void Awake()
    {
        instance = this;
    }
}
