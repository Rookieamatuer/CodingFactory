using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoTextDisplay : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
   
    private void OnMouseEnter()
    {
        GameManager.instance.infoText.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
        GameManager.instance.infoText.gameObject.SetActive(true);
        GameManager.instance.infoText.GetComponentInChildren<Text>().text = this.name;
    }
    private void OnMouseExit()
    {
        GameManager.instance.infoText.gameObject.SetActive(false);
    }
    private void OnMouseOver()
    {
        GameManager.instance.infoText.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);
        GameManager.instance.infoText.gameObject.SetActive(true);
        GameManager.instance.infoText.GetComponentInChildren<Text>().text = this.name;
    }
}

