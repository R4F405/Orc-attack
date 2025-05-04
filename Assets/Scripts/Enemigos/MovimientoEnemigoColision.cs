using UnityEngine;

/// <summary>
/// Controla el movimiento de un enemigo que persigue al jugador mediante colisiones físicas.
/// </summary>
/// <remarks>
/// Esta clase implementa el comportamiento básico de persecución para enemigos cuerpo a cuerpo.
/// El enemigo se mueve directamente hacia el jugador y gira su sprite para mirar en la dirección correcta.
/// </remarks>
public class MovimientoEnemigoColision : MonoBehaviour
{
    /// <summary>
    /// Referencia al objeto del jugador que el enemigo debe perseguir.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Velocidad de movimiento del enemigo en unidades por segundo.
    /// </summary>
    public float velocidad = 2f;

    /// <summary>
    /// Indica si el sprite del enemigo está mirando hacia la derecha.
    /// </summary>
    private bool estaMirandoDerecha = true;

    /// <summary>
    /// Inicializa el enemigo buscando al jugador en la escena.
    /// </summary>
    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");

        // Verificar si se encontró el jugador para evitar errores
        if (jugador == null)
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Player'.");
        }
    }

    /// <summary>
    /// Actualiza la posición del enemigo a intervalos fijos para seguir al jugador.
    /// </summary>
    /// <remarks>
    /// Se ejecuta a una frecuencia constante, independiente de la velocidad de fotogramas,
    /// lo que lo hace más adecuado para el movimiento físico.
    /// </remarks>
    void FixedUpdate()
    {
        if (jugador != null)
        {
            IASeguimiento();
        }

        // Verificar si se encontró el jugador para evitar errores
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Jugador");    
        }
    }

    /// <summary>
    /// Actualiza la orientación del sprite del enemigo en cada fotograma.
    /// </summary>
    void Update()
    {
        if (jugador != null)
        {
            bool estaJugadorDerecha = transform.position.x < jugador.transform.position.x;
            Girar(estaJugadorDerecha);
        }
    }

    /// <summary>
    /// Mueve al enemigo hacia la posición actual del jugador.
    /// </summary>
    private void IASeguimiento()
    {
        transform.position = Vector2.MoveTowards(transform.position, jugador.transform.position, velocidad * Time.deltaTime);
    }

    /// <summary>
    /// Gira el sprite del enemigo para que mire hacia el jugador.
    /// </summary>
    /// <param name="estaJugadorDerecha">Indica si el jugador está a la derecha del enemigo.</param>
    private void Girar(bool estaJugadorDerecha)
    {
        if ((estaMirandoDerecha && !estaJugadorDerecha) || (!estaMirandoDerecha && estaJugadorDerecha))
        {
            estaMirandoDerecha = !estaMirandoDerecha;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
