using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetColorBoton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Quitar la selección inmediatamente después de hacer clic
        EventSystem.current.SetSelectedGameObject(null); // Quitar la selección del botón
    }
}
