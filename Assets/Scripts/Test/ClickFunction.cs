using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFunction : MonoBehaviour
{
    [SerializeField] Transform child;
    [SerializeField] Transform block;
    [SerializeField] Transform obj;
    [SerializeField] int count;
    [SerializeField] int idx;
    public void test()
    {
        Debug.Log("click");
        float len = count;
        transform.position =  new Vector3(transform.position.x, transform.position.y - ((len - transform.localScale.y) / 2), 0);
        transform.localScale = new Vector3(1, len, 0);
       child.position = new Vector2(transform.position.x, transform.position.y - len / 2 + 0.5f + idx);
    }
}
