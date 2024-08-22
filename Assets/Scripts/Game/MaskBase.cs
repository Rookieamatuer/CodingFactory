using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskBase : MonoBehaviour
{
    protected Material material;//材质
    protected Vector2 center;//镂空中心
    protected Transform target;//被引导的目标对象
    protected Bounds targetBounds;

    protected float timer;//计时器，来达到动画匀速播放
    protected float time;//整体动画时间
    protected bool isScaling;//是否正在缩放
                             //虚方法，子类可以去重写，里面用来判断动画是否播放，如果播放，就按照既定的时间匀速完成
    protected virtual void Update()
    {
        if (isScaling)
        {
            timer += Time.deltaTime * 1 / time;
            if (timer >= 1)
            {
                timer = 0;
                isScaling = false;
            }
        }
    }
    //这里是来获取目标物体的四个点来计算中心点，因为对于矩形或者圆形效果，他们面对的中心点是确定的
    public virtual void Guide(Canvas canvas, Transform target, bool isInit = false)
    {
        material = GetComponent<Image>().material;
        this.target = target;
        //获取四个点的世界坐标
        //target.GetWorldCorners(targetCorners);
        //世界坐标转屏幕坐标
        targetBounds = target.GetComponent<Renderer>().bounds;
        center = WorldToScreenPoints(canvas, targetBounds.center);
        //设置中心点
        material.SetVector("_Center", center);
        Debug.Log("center: " + center);
    }
    //为了让子类继承的时候直接重写就可以，因为矩形和圆形的动画方式不一样，跟长宽或者半径有关
    public virtual void Guide(Canvas canvas, Transform target, float scale, float time)
    {

    }
    //坐标的转换
    public Vector2 WorldToScreenPoints(Canvas canvas, Vector3 world)
    {
        //把世界转屏幕
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, world);
        Vector2 localPoint;
        //屏幕转局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out localPoint);
        return localPoint;
    }
}
