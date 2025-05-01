using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public float rangoRecoleccion = 1f; // Rango en el que el jugador puede recogerlo
    public int cantidad = 1; // Cantidad que suma este objeto
    public AudioClip sonidoRecoleccion; // Sonido al recoger la calavera

    private Transform jugador; // Referencia al jugador
    private bool jugadorCerca = false; // Verifica si el jugador está dentro del rango
    private AudioSource audioSource; // Componente de audio
    private bool yaRecogido = false; // Evita recoger múltiples veces

    private void Start()
    {
        // Obtener o crear componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (jugador != null && !yaRecogido)
        {
            float distancia = Vector2.Distance(transform.position, jugador.position);
            jugadorCerca = distancia <= rangoRecoleccion; // Detecta si está cerca
        }
    }

    public void IntentarRecoger()
    {
        // Si ya fue recogido o el jugador no está cerca, no hacer nada
        if (yaRecogido || !jugadorCerca)
            return;

        InventarioJugador inventario = jugador.GetComponent<InventarioJugador>();
        if (inventario != null)
        {
            // Marcar como recogido para evitar repeticiones
            yaRecogido = true;
            
            // Reproducir sonido al recoger
            if (sonidoRecoleccion != null)
            {
                // Usar el nuevo método estático para reproducir el sonido
                ExtensionesAudio.ReproducirEnPosicion(sonidoRecoleccion, transform.position, 1.0f, TipoAudio.Efectos);
            }

            inventario.AgregarCalavera(cantidad);
            
            // Ocultar el objeto inmediatamente
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.enabled = false;
            }
            
            
            // Desactivar los colliders
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D c in colliders)
            {
                c.enabled = false;
            }
            
            // Destruir después de un pequeño delay (0.1 segundos debería ser suficiente)
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador") && !yaRecogido)
        {
            jugador = collision.transform; // Guarda la referencia del jugador
            // Intentar recoger inmediatamente al entrar en contacto
            IntentarRecoger();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador") && !yaRecogido)
        {
            jugador = null;
            jugadorCerca = false;
        }
    }
}
