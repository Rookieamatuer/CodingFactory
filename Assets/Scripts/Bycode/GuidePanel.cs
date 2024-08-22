using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    GuideController guideController;
    Canvas canvas;
    [SerializeField] string welcomText;
    [SerializeField] string successText;
    [SerializeField] string errorText;
    [SerializeField] RectTransform clickLine;
    bool isFinished;
    static bool reStart;

    private void Start()
    {
        canvas = transform.GetComponentInParent<Canvas>();
        guideController = transform.GetComponent<GuideController>();
        guideController.Guide(canvas, null, GuideType.None);
        if (!isFinished && !reStart)
            gameObject.GetComponentInChildren<Text>().text = welcomText;

        
    }
    
    public void ClickToContinue()
    {
        //if (guideController.IsClickValid(Input.mousePosition))
        //{
        //    guideController.Guide(canvas, GuideType.Rect, 2, 0.5f);
        //    Debug.Log("click");
        //}
        if (guideController.endOfTutorial)
        {
            
            if (isFinished && SceneManager.GetActiveScene().buildIndex < 8) // Level clear
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (reStart) // Level failed
            {
                GameManager.instance.ResetLevel();
                return;
            }
            gameObject.SetActive(false);
            return;
        }
        if (guideController.IsClickValid(Input.mousePosition, clickLine))
            guideController.Guide(canvas, GuideType.Rect, 2, 0.5f);
    }

    public void SuccessMessage()
    {
        if (guideController == null)
        {
            guideController = transform.GetComponent<GuideController>();
        }
        gameObject.SetActive(true);
        //guideController.enabled = false;
        guideController.EndOfTutotial();
        gameObject.GetComponentInChildren<Text>().text = successText;
        
        isFinished = true;
    }

    public void ErrorMessage()
    {
        
        guideController = transform.GetComponent<GuideController>();
        guideController.endOfTutorial = true;
        guideController.Guide(canvas, null, GuideType.None);
        gameObject.SetActive(true);

        gameObject.GetComponentInChildren<Text>().text = errorText;
        reStart = true;
    }
}
