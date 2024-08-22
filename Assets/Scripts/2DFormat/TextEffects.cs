using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffects : MonoBehaviour
{
    [SerializeField] private int letterPerSecond;//显示的速度

    [SerializeField] Text dialogText;
    [SerializeField] string dialog;

    [SerializeField] GameObject clickToContinue;

    private void Start()
    {
        dialogText = GetComponent<Text>();
        clickToContinue.SetActive(false);
        Debug.Log(dialogText.text.Length);
        StartCoroutine(TypeDialog(dialog));
    }

    public IEnumerator TypeDialog(string dialog)//协程
    {
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);//字体显示停顿时间
        }
    }

    private void Update()
    {
        if (dialogText.text.Length == (dialog.Length + 2))
        {
            clickToContinue.SetActive(true);
        }
    }
}
