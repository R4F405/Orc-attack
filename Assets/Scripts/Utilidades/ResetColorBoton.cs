using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Componente utilitario para resetear la selección de un botón inmediatamente después de hacer clic.
/// Esto evita que el botón mantenga el color de "seleccionado" después de la interacción.
/// Se debe añadir a cualquier botón que requiera volver a su estado visual normal después del clic.
/// </summary>
/// <remarks>
/// En interfaces de usuario complejas con muchos botones, este componente ayuda a mantener una apariencia
/// visual consistente, evitando que los botones queden resaltados después de hacer clic en ellos. Esto es
/// especialmente útil en menús de juego que se muestran durante el gameplay, donde mantener un botón
/// seleccionado podría ser visualmente confuso.
/// 
/// Este script trabaja bien en conjunto con SonidosUI para proporcionar tanto retroalimentación visual
/// como auditiva a las interacciones del usuario, creando una experiencia más completa.
/// </remarks>
public class ResetColorBoton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Se ejecuta cuando el usuario hace clic sobre el botón.
    /// Elimina la selección actual del EventSystem para que el botón vuelva a su estado visual normal.
    /// </summary>
    /// <param name="eventData">Datos del evento de puntero que contiene información sobre el clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Quitar la selección inmediatamente después de hacer clic
        EventSystem.current.SetSelectedGameObject(null); // Quitar la selección del botón
    }
}
