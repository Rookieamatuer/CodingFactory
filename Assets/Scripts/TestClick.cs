using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClick : MonoBehaviour
{
    //[SerializeField] GameObject tutorialUI;

    public void ShowUI()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/2DFormat/TutorialAnim"));
        obj.SetActive(true);
        obj.GetComponent<TutorialUI>().ShowAnim(this.name);
    }

    public void GameClose()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
