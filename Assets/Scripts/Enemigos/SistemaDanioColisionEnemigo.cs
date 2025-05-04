using UnityEngine;

/// <summary>
/// Gestiona el sistema de daño que un enemigo inflige al jugador por contacto directo.
/// </summary>
/// <remarks>
/// Esta clase permite a los enemigos cuerpo a cuerpo infligir daño al jugador
/// cuando entran en contacto con él. Incluye un sistema de enfriamiento para
/// controlar la frecuencia con la que se aplica el daño.
/// </remarks>
public class SistemaDanioColisionEnemigo : MonoBehaviour
{
    /// <summary>
    /// Cantidad de daño que el enemigo inflige al jugador en cada ataque.
    /// </summary>
    public int daño = 1;
    
    /// <summary>
    /// Tiempo mínimo en segundos entre ataques consecutivos.
    /// </summary>
    /// <remarks>
    /// Este valor evita que el enemigo cause daño constante al permanecer
    /// en contacto con el jugador, limitando la frecuencia de los ataques.
    /// </remarks>
    public float tiempoEntreAtaques = 1.5f; 

    /// <summary>
    /// Marca de tiempo para el próximo ataque disponible.
    /// </summary>
    private float proximoAtaque = 0f; 

    /// <summary>
    /// Se llama continuamente mientras otro collider permanece dentro del trigger del enemigo.
    /// </summary>
    /// <param name="col">El collider que está en contacto con este enemigo.</param>
    /// <remarks>
    /// Detecta si el collider pertenece al jugador y, si ha pasado suficiente tiempo
    /// desde el último ataque, le inflige daño.
    /// </remarks>
    private void OnTriggerStay2D(Collider2D col) 
    {
        if (col.CompareTag("Jugador"))
        {
            VidaJugador saludJugador = col.GetComponent<VidaJugador>();
            if (saludJugador != null && Time.time >= proximoAtaque)
            {
                saludJugador.RecibirDaño(daño);
                proximoAtaque = Time.time + tiempoEntreAtaques;
            }
        }
    }
}
