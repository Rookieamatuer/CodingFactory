using UnityEngine;
using UnityEngine.UI;

public class RectGuide : GuideBase
{
    protected float width;//Он©у©М
    protected float height;//Он©у╦ъ

    float scalewidth;
    float scaleheight;

    public override void Guide(Canvas canvas, RectTransform target, bool isInit=false)
    {
        base.Guide(canvas, target);
        
        if (isInit)
        {
            width = 0;
            height = 0;
        } else
        {
            width = (targetCorners[3].x - targetCorners[0].x) / 2;
            height = (targetCorners[1].y - targetCorners[0].y) / 2;
        }
        material.SetFloat("_SliderX", width);
        material.SetFloat("_SliderY", height);
    }

    public override void Guide(Canvas canvas, RectTransform target, float scale, float time)
    {
        this.Guide(canvas, target);

        scalewidth = width * scale;
        scaleheight = height * scale;
        material.SetFloat("_SliderX", scalewidth);
        material.SetFloat("_SliderY", scaleheight);

        this.time = time;
        isScaling = true;
        timer = 0;

    }

    protected override void Update()
    {
        base.Update();
        if (isScaling)
        {
            this.material.SetFloat("_SliderX", Mathf.Lerp(scalewidth, width, timer));
            this.material.SetFloat("_SliderY", Mathf.Lerp(scaleheight, height, timer));
        }
    }
}
