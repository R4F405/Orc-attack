using UnityEngine;

/// <summary>
/// Controla el comportamiento de cajas destructibles en el juego.
/// </summary>
/// <remarks>
/// Esta clase gestiona la destrucción de cajas y el posible drop de pociones de vida
/// cuando son golpeadas por el jugador o armas.
/// </remarks>
public class RomperCaja : MonoBehaviour
{
    /// <summary>
    /// Prefab de la poción que puede aparecer al romper la caja.
    /// </summary>
    public GameObject pocionPrefab; // Asigna la poción en el Inspector
    
    /// <summary>
    /// Cantidad de golpes que puede soportar la caja antes de romperse.
    /// </summary>
    public int vidaCaja = 1; // La cantidad de golpes que aguanta la caja

    /// <summary>
    /// Aplica daño a la caja y genera una poción si se destruye.
    /// </summary>
    /// <remarks>
    /// Este método se llama cuando la caja recibe un golpe. Si la vida de la caja
    /// llega a cero, instancia una poción en su posición y destruye la caja.
    /// </remarks>
    public void RecibirGolpe()
    {
        if (pocionPrefab != null)
        {
            Instantiate(pocionPrefab, transform.position, Quaternion.identity); //Instancia la pocion en el lugar de la caja
        }
        Destroy(gameObject); // Destruye la caja
    }
}
