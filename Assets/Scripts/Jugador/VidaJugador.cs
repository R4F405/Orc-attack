using System.Collections;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int saludMaxima = 15;
    public float tiempoEntreRecuperaciones = 10f; // Intervalo en segundos
    public AudioClip sonidoDaño; // Sonido cuando el jugador recibe daño
    public AudioClip sonidoMuerte; // Sonido cuando el jugador muere

    private int cantidadRecuperacion = 1;         // Cantidad de vida que se recupera
    private MovimientoJugador movimientoJugador; 
    private int saludActual;
    private AudioSource audioSource;
    private PanelEliminadoController panelEliminado;
    private bool haMuerto = false; // Variable para evitar que se ejecute dos veces

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

    // Coroutine que maneja la recuperación de vida cada X segundos
    private IEnumerator RecuperacionVidaAutomatica()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreRecuperaciones); // Espera el tiempo especificado
            Curar(cantidadRecuperacion); // Recupera vida
        }
    }

    public void DisminuirTiempoEntreRecuperaciones(float cantidad)
    {
        if (tiempoEntreRecuperaciones > cantidad) {
            tiempoEntreRecuperaciones -= cantidad;
        }
        
    }

    public void AumentarSaludMaxima(int cantidad)
    {
        saludMaxima += cantidad;
        if (saludActual > saludMaxima)
        {
            saludActual = saludMaxima;  // Ajusta la salud actual si supera la nueva máxima
        }
    }

    public void Curar(int cantidad)
    {
        saludActual += cantidad;
        if (saludActual > saludMaxima)
        {
            saludActual = saludMaxima; // Evita que la salud supere el máximo
        }
    }

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

    public int ObtenerSalud()
    {
        return saludActual;
    }

    public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
}
