using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GuideType
{
    Rect,
    None,
}

[RequireComponent(typeof(RectGuide))]
public class GuideController : MonoBehaviour
{
    private RectGuide rectGuide;

    public Material rectMat;

    public List<RectTransform> guideList;

    [SerializeField]private List<string> textList;

    private int currentIndex;
    public bool endOfTutorial;

    [SerializeField] private string startText;

    private Image mask; // guideui
    private RectTransform target;
    private void Awake()
    {
        mask = transform.GetComponent<Image>();
        if (mask == null) { throw new System.Exception("mask initialize failed"); }
        if (rectMat == null) { throw new System.Exception("No material"); }
        rectGuide = transform.GetComponent<RectGuide>();
        currentIndex = 0;
        endOfTutorial = false;
    }

    public void Guide(Canvas canvas, RectTransform target, GuideType guideType)
    {
        this.target = target;
        switch (guideType)
        {
            case GuideType.Rect:
                mask.material = rectMat;
                rectGuide.Guide(canvas, target);
                break;
            default: 
                rectGuide.Guide(canvas, target, true);
                break;

        }
    }

    public void Guide(Canvas canvas, GuideType guideType, float scale, float time)
    {     
        if (currentIndex >= guideList.Count) {
            showIntroductionText(startText);
            endOfTutorial = true;
            return; 
        }
        this.target = guideList[currentIndex];
        switch (guideType)
        {
            case GuideType.Rect:
                mask.material = rectMat;
                rectGuide.Guide(canvas, target, scale, time);
                break;

        }
        showIntroductionText(textList[currentIndex++]);
    }

    private void showIntroductionText(string intro)
    {
        gameObject.GetComponentInChildren<Text>().text = intro;
    }
    public bool IsClickValid(Vector2 sp, RectTransform clickPoint)
    {
        if (target == null) { return true; }
        return RectTransformUtility.RectangleContainsScreenPoint(clickPoint, sp);
    }

    public void EndOfTutotial()
    {
        endOfTutorial = true;
    }
}
