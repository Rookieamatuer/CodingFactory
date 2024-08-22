using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClick : MonoBehaviour
{
    [SerializeField] GameObject menu=null;

    public void ShowUI()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/2DFormat/TutorialAnim"));
        obj.SetActive(true);
        obj.GetComponent<TutorialUI>().ShowAnim(this.name);
    }

    public void GameStart()
    {
        SceneManager.LoadScene(2);
    }

    public void GameClose()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene(0);
    }
}
