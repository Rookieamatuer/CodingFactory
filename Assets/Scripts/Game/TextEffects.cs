using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffects : MonoBehaviour
{
    [SerializeField] private int letterPerSecond;// print speed

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

    public IEnumerator TypeDialog(string dialog)
    {
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);// print interval time
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
