using UnityEngine;

/// <summary>
/// Controla el comportamiento de las pociones de vida.
/// </summary>
/// <remarks>
/// Esta clase gestiona la interacción de las pociones con el jugador,
/// curando un porcentaje de su salud máxima cuando son recogidas.
/// </remarks>
public class PocionVida : MonoBehaviour
{
    /// <summary>
    /// Porcentaje de la salud máxima que restaurará la poción al ser recogida.
    /// </summary>
    public float porcentajeCuracion = 20f;

    /// <summary>
    /// Detecta cuando el jugador entra en contacto con la poción.
    /// </summary>
    /// <param name="otro">Collider del objeto que entró en contacto con la poción.</param>
    /// <remarks>
    /// Cuando el jugador toca la poción, se calcula la cantidad de salud a restaurar
    /// basada en un porcentaje de su salud máxima y se aplica la curación.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Jugador"))
        {
            VidaJugador vida = otro.GetComponent<VidaJugador>();
            if (vida != null)
            {
                int cantidadCuracion = Mathf.RoundToInt(vida.ObtenerSaludMaxima() * (porcentajeCuracion / 100f));
                vida.Curar(cantidadCuracion);
            }
            Destroy(gameObject); // Se destruye la poción al recogerla
        }
    }
}
