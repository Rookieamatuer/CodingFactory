using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDetect : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggedPart draggedPart = eventData.pointerDrag.GetComponent<DraggedPart>();
            if (draggedPart != null)
            {
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.localPosition = Vector2.zero; // Align to the center of the slot
            }
        }
    }
}
