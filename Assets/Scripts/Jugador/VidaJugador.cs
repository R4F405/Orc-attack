using System.Collections;
using UnityEngine;

/// <summary>
/// Gestiona la salud y la muerte del jugador.
/// </summary>
/// <remarks>
/// Esta clase se encarga de controlar la vida del jugador, incluyendo la recepción de daño,
/// la recuperación automática de salud, la muerte y sus consecuencias.
/// </remarks>
public class VidaJugador : MonoBehaviour
{
    /// <summary>
    /// Cantidad máxima de salud que puede tener el jugador.
    /// </summary>
    public int saludMaxima = 15;
    
    /// <summary>
    /// Tiempo en segundos entre cada recuperación automática de salud.
    /// </summary>
    public float tiempoEntreRecuperaciones = 10f; // Intervalo en segundos
    
    /// <summary>
    /// Sonido que se reproduce cuando el jugador recibe daño.
    /// </summary>
    public AudioClip sonidoDaño; // Sonido cuando el jugador recibe daño
    
    /// <summary>
    /// Sonido que se reproduce cuando el jugador muere.
    /// </summary>
    public AudioClip sonidoMuerte; // Sonido cuando el jugador muere

    /// <summary>
    /// Cantidad de salud que se recupera en cada intervalo automático.
    /// </summary>
    private int cantidadRecuperacion = 1;         // Cantidad de vida que se recupera
    
    /// <summary>
    /// Referencia al componente que controla el movimiento del jugador.
    /// </summary>
    private MovimientoJugador movimientoJugador; 
    
    /// <summary>
    /// Cantidad actual de salud del jugador.
    /// </summary>
    private int saludActual;
    
    /// <summary>
    /// Componente para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;
    
    /// <summary>
    /// Referencia al controlador del panel que se muestra cuando el jugador es eliminado.
    /// </summary>
    private PanelEliminadoController panelEliminado;
    
    /// <summary>
    /// Indica si el jugador ya ha muerto para evitar ejecutar la muerte múltiples veces.
    /// </summary>
    private bool haMuerto = false; // Variable para evitar que se ejecute dos veces

    /// <summary>
    /// Inicializa los componentes y la salud del jugador.
    /// </summary>
    private void Start()
    {
        saludActual = saludMaxima; // Inicia con la salud máxima

        movimientoJugador = GetComponent<MovimientoJugador>();
        
        // Obtener o crear componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Buscar el panel de eliminado en la escena
        panelEliminado = FindObjectOfType<PanelEliminadoController>();
        if (panelEliminado == null)
        {
            Debug.LogWarning("No se encontró el PanelEliminadoController en la escena. Asegúrate de que existe y tiene el script asignado.");
        }

        // Iniciar la rutina de recuperación de vida
        StartCoroutine(RecuperacionVidaAutomatica());
    }

    /// <summary>
    /// Rutina que maneja la recuperación automática de salud a intervalos regulares.
    /// </summary>
    /// <returns>Objeto IEnumerator para la corrutina.</returns>
    private IEnumerator RecuperacionVidaAutomatica()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreRecuperaciones); // Espera el tiempo especificado
            Curar(cantidadRecuperacion); // Recupera vida
        }
    }

    /// <summary>
    /// Reduce el tiempo entre recuperaciones automáticas de salud.
    /// </summary>
    /// <param name="cantidad">Segundos a restar del tiempo entre recuperaciones.</param>
    public void DisminuirTiempoEntreRecuperaciones(float cantidad)
    {
        if (tiempoEntreRecuperaciones > cantidad) {
            tiempoEntreRecuperaciones -= cantidad;
        }
        
    }

    /// <summary>
    /// Aumenta la salud máxima del jugador.
    /// </summary>
    /// <param name="cantidad">Cantidad a aumentar en la salud máxima.</param>
    public void AumentarSaludMaxima(int cantidad)
    {
        saludMaxima += cantidad;
        if (saludActual > saludMaxima)
        {
            saludActual = saludMaxima;  // Ajusta la salud actual si supera la nueva máxima
        }
    }

    /// <summary>
    /// Restaura salud al jugador sin exceder su salud máxima.
    /// </summary>
    /// <param name="cantidad">Cantidad de salud a restaurar.</param>
    public void Curar(int cantidad)
    {
        saludActual += cantidad;
        if (saludActual > saludMaxima)
        {
            saludActual = saludMaxima; // Evita que la salud supere el máximo
        }
    }

    /// <summary>
    /// Reduce la salud del jugador y verifica si debe morir.
    /// </summary>
    /// <param name="cantidad">Cantidad de daño a aplicar.</param>
    public void RecibirDaño(int cantidad)
    {
        saludActual -= cantidad;
        
        // Reproducir sonido de daño
        if (sonidoDaño != null && audioSource != null)
        {
            audioSource.ReproducirConVolumenGlobal(sonidoDaño, 1.0f, TipoAudio.Efectos);
        }
        
        if (saludActual <= 0f)
        {
            Muerte();
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador, desactivando controles y mostrando la pantalla de fin de juego.
    /// </summary>
    private void Muerte()
    {
        if (haMuerto) return; // Evita que se ejecute dos veces
        haMuerto = true;

        if (movimientoJugador != null)
        {
            movimientoJugador.enabled = false; // Desactiva el script de movimiento
        }

        // Reproducir sonido de muerte
        if (sonidoMuerte != null && audioSource != null)
        {
            audioSource.ReproducirConVolumenGlobal(sonidoMuerte, 1.0f, TipoAudio.Efectos);
        }

        // Notificar al panel de eliminado
        if (panelEliminado != null)
        {
            panelEliminado.MostrarPanel();
        }
        else
        {
            // Si no se encuentra el controller, intentamos cargar la escena del menú directamente
            Time.timeScale = 0f;
            StartCoroutine(VolverAlMenuSinPanel());
        }
    }

    /// <summary>
    /// Rutina de emergencia para volver al menú principal cuando no se encuentra el panel de eliminado.
    /// </summary>
    /// <returns>Objeto IEnumerator para la corrutina.</returns>
    private System.Collections.IEnumerator VolverAlMenuSinPanel()
    {
        // Esperar 5 segundos en tiempo real (no afectado por Time.timeScale)
        float tiempoEspera = 5f;
        float tiempoPasado = 0f;
        while (tiempoPasado < tiempoEspera)
        {
            yield return null;
            tiempoPasado += Time.unscaledDeltaTime;
        }
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Asume que el menú principal es la escena 0
    }

    /// <summary>
    /// Devuelve la cantidad actual de salud del jugador.
    /// </summary>
    /// <returns>Salud actual del jugador.</returns>
    public int ObtenerSalud()
    {
        return saludActual;
    }

    /// <summary>
    /// Devuelve la cantidad máxima de salud del jugador.
    /// </summary>
    /// <returns>Salud máxima del jugador.</returns>
    public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
}
