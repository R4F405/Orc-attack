using TMPro;
using UnityEngine;

/// <summary>
/// Controla la visualización del contador de calaveras en la interfaz de usuario.
/// </summary>
/// <remarks>
/// Esta clase se encarga de actualizar el texto que muestra la cantidad de calaveras
/// recolectadas por el jugador, manteniendo sincronizada la interfaz con el inventario.
/// </remarks>
public class UICalaveras : MonoBehaviour
{
    /// <summary>
    /// Referencia al texto donde se mostrará la cantidad de calaveras recolectadas.
    /// </summary>
    public TextMeshProUGUI textoCalaveras;

    /// <summary>
    /// Referencia al componente de inventario del jugador.
    /// </summary>
    private InventarioJugador inventarioJugador;

    /// <summary>
    /// Inicializa el componente buscando al jugador en la escena.
    /// </summary>
    private void Start()
    {
        // Intentar obtener el InventarioJugador una vez al inicio
        ObtenerJugador();
    }

    /// <summary>
    /// Actualiza el contador de calaveras en cada frame si el jugador está disponible.
    /// </summary>
    /// <remarks>
    /// Si no se encontró al jugador en el inicio, continúa intentando encontrarlo
    /// hasta que esté disponible para mantener la UI sincronizada.
    /// </remarks>
    private void Update()
    {
        // Si no se ha encontrado el jugador en Start(), intentar encontrarlo en Update()
        if (inventarioJugador == null)
        {
            ObtenerJugador();
        }

        // Si se encuentra el jugador, actualizar la UI
        if (inventarioJugador != null)
        {
            textoCalaveras.text = inventarioJugador.ObtenerCantidadCalaveras().ToString();
        }
    }

    /// <summary>
    /// Busca al jugador en la escena y obtiene su componente de inventario.
    /// </summary>
    private void ObtenerJugador()
    {
        // Intentar obtener el jugador con el tag 'Jugador'
        GameObject jugador = GameObject.FindWithTag("Jugador");
        if (jugador != null)
        {
            inventarioJugador = jugador.GetComponent<InventarioJugador>();
        }
    
    }
}
