using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoTextDisplay : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    //GameObject infoText = Instantiate<GameObject>(Resources.Load<GameObject>(""));
    //    GameManager.instance.infoText.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
    //    GameManager.instance.infoText.gameObject.SetActive(true);
    //    GameManager.instance.infoText.GetComponentInChildren<Text>().text = this.name;
    //}
    ////鼠标离开
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    GameManager.instance.infoText.gameObject.SetActive(false);
    //}
    ////鼠标在ui里滑动
    //public void OnPointerMove(PointerEventData eventData)
    //{
    //    GameManager.instance.infoText.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
    //    GameManager.instance.infoText.gameObject.SetActive(true);
    //    GameManager.instance.infoText.GetComponentInChildren<Text>().text = this.name;
    //}
    private void OnMouseEnter()
    {
        TestGameManager.instance.infoText.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
        TestGameManager.instance.infoText.gameObject.SetActive(true);
        TestGameManager.instance.infoText.GetComponentInChildren<Text>().text = this.name;
    }
    private void OnMouseExit()
    {
        TestGameManager.instance.infoText.gameObject.SetActive(false);
    }
    private void OnMouseOver()
    {
        TestGameManager.instance.infoText.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
        TestGameManager.instance.infoText.gameObject.SetActive(true);
        TestGameManager.instance.infoText.GetComponentInChildren<Text>().text = this.name;
    }
}

