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
        if (movimientoJugador != null)
        {
            movimientoJugador.enabled = false; // Desactiva el script de movimiento
        }

        // Reproducir sonido de muerte
        if (sonidoMuerte != null && audioSource != null)
        {
            audioSource.ReproducirConVolumenGlobal(sonidoMuerte, 1.0f, TipoAudio.Efectos);
        }
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
