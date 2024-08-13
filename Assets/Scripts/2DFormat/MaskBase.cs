using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskBase : MonoBehaviour
{
    protected Material material;//����
    protected Vector2 center;//�ο�����
    protected Transform target;//��������Ŀ�����
    protected Bounds targetBounds;

    protected float timer;//��ʱ�������ﵽ�������ٲ���
    protected float time;//���嶯��ʱ��
    protected bool isScaling;//�Ƿ���������
                             //�鷽�����������ȥ��д�����������ж϶����Ƿ񲥷ţ�������ţ��Ͱ��ռȶ���ʱ���������
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
    //����������ȡĿ��������ĸ������������ĵ㣬��Ϊ���ھ��λ���Բ��Ч����������Ե����ĵ���ȷ����
    public virtual void Guide(Canvas canvas, Transform target, bool isInit = false)
    {
        material = GetComponent<Image>().material;
        this.target = target;
        //��ȡ�ĸ������������
        //target.GetWorldCorners(targetCorners);
        //��������ת��Ļ����
        targetBounds = target.GetComponent<Renderer>().bounds;
        center = WorldToScreenPoints(canvas, targetBounds.center);
        //�������ĵ�
        material.SetVector("_Center", center);
        Debug.Log("center: " + center);
    }
    //Ϊ��������̳е�ʱ��ֱ����д�Ϳ��ԣ���Ϊ���κ�Բ�εĶ�����ʽ��һ������������߰뾶�й�
    public virtual void Guide(Canvas canvas, Transform target, float scale, float time)
    {

    }
    //�����ת��
    public Vector2 WorldToScreenPoints(Canvas canvas, Vector3 world)
    {
        //������ת��Ļ
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, world);
        Vector2 localPoint;
        //��Ļת�ֲ�����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out localPoint);
        return localPoint;
    }
}
