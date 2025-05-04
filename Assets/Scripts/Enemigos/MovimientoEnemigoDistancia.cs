using UnityEngine;

/// <summary>
/// Controla el movimiento de un enemigo a distancia que mantiene cierta separación del jugador.
/// </summary>
/// <remarks>
/// Esta clase implementa el comportamiento de persecución para enemigos de ataque a distancia.
/// El enemigo sigue al jugador hasta una distancia mínima, donde se detiene para atacar,
/// manteniendo su sprite orientado hacia el jugador en todo momento.
/// </remarks>
public class MovimientoEnemigoDistancia : MonoBehaviour
{
    /// <summary>
    /// Referencia al objeto del jugador que el enemigo debe seguir.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Velocidad de movimiento del enemigo en unidades por segundo.
    /// </summary>
    public float velocidad = 2f;
    
    /// <summary>
    /// Distancia mínima que el enemigo mantendrá con el jugador.
    /// </summary>
    /// <remarks>
    /// Cuando el enemigo se encuentra a esta distancia o más cerca del jugador,
    /// deja de avanzar y puede comenzar a atacar.
    /// </remarks>
    public float distanciaMinima = 3f;

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

        if (jugador == null)
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Jugador'.");
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
    /// Mueve al enemigo hacia el jugador, deteniéndose al alcanzar la distancia mínima.
    /// </summary>
    private void IASeguimiento()
    {
        float distancia = Vector2.Distance(transform.position, jugador.transform.position);

        if (distancia > distanciaMinima)
        {
            transform.position = Vector2.MoveTowards(transform.position, jugador.transform.position, velocidad * Time.deltaTime);
        }
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
