using UnityEngine;

/// <summary>
/// Controla el comportamiento básico de la cámara para seguir al jugador.
/// </summary>
/// <remarks>
/// Esta clase se encarga de seguir constantemente al jugador, manteniendo
/// la cámara centrada en su posición durante el juego.
/// </remarks>
public class ControladorCamara : MonoBehaviour
{
    /// <summary>
    /// Referencia al objeto del jugador que la cámara debe seguir.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Referencia a la cuadrícula de fondo para posicionar correctamente la cámara.
    /// </summary>
    public Grid background;

    /// <summary>
    /// Inicializa la cámara buscando al jugador al comenzar.
    /// </summary>
    private void Start()
    {
        BuscarJugador();
    }

    /// <summary>
    /// Actualiza la posición de la cámara en cada frame para seguir al jugador.
    /// </summary>
    void Update()
    {
        if (jugador == null)
        {
            BuscarJugador(); // Intenta encontrar al jugador si aún no ha sido asignado
            return;
        }

        transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y, -3f);
    }

    /// <summary>
    /// Busca al jugador en la escena mediante su etiqueta "Jugador".
    /// </summary>
    /// <remarks>
    /// La advertencia de depuración se muestra después de 1 segundo para evitar falsos
    /// positivos, ya que la cámara puede inicializarse antes que el jugador.
    /// </remarks>
    void BuscarJugador()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");

        if (jugador == null && Time.timeSinceLevelLoad > 1f) // Solo muestra el mensaje después de 1 segundo, ya que la camara se instancia mas rapido que el jugador y evitar errores
        {
            Debug.LogWarning("No se encontró un GameObject con la etiqueta 'Jugador'. Intentando de nuevo...");
        }
    }
}
