using UnityEngine;

/// <summary>
/// Controla la recolección de objetos en el juego.
/// </summary>
/// <remarks>
/// Esta clase gestiona el comportamiento de objetos recolectables como calaveras,
/// incluyendo su detección, interacción con el jugador y efectos al ser recogidos.
/// </remarks>
public class RecogerObjeto : MonoBehaviour
{
    /// <summary>
    /// Distancia máxima a la que el jugador puede recoger este objeto.
    /// </summary>
    public float rangoRecoleccion = 1f;
    
    /// <summary>
    /// Cantidad de recursos que suma este objeto al inventario del jugador.
    /// </summary>
    public int cantidad = 1;
    
    /// <summary>
    /// Sonido que se reproduce cuando el objeto es recogido.
    /// </summary>
    public AudioClip sonidoRecoleccion;

    /// <summary>
    /// Referencia al transform del jugador.
    /// </summary>
    private Transform jugador;
    
    /// <summary>
    /// Indica si el jugador está dentro del rango de recolección.
    /// </summary>
    private bool jugadorCerca = false;
    
    /// <summary>
    /// Componente para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;
    
    /// <summary>
    /// Evita que el objeto sea recogido múltiples veces.
    /// </summary>
    private bool yaRecogido = false;

    /// <summary>
    /// Inicializa el componente de audio al iniciar.
    /// </summary>
    private void Start()
    {
        // Obtener o crear componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Actualiza el estado de proximidad del jugador en cada frame.
    /// </summary>
    private void Update()
    {
        if (jugador != null && !yaRecogido)
        {
            float distancia = Vector2.Distance(transform.position, jugador.position);
            jugadorCerca = distancia <= rangoRecoleccion; // Detecta si está cerca
        }
    }

    /// <summary>
    /// Intenta recoger el objeto si el jugador está cerca y el objeto no ha sido recogido.
    /// </summary>
    /// <remarks>
    /// Añade la cantidad especificada al inventario del jugador, reproduce sonido
    /// y marca el objeto para ser destruido.
    /// </remarks>
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

    /// <summary>
    /// Detecta cuando el jugador entra en contacto con el objeto.
    /// </summary>
    /// <param name="collision">Collider que ha entrado en contacto con este objeto.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador") && !yaRecogido)
        {
            jugador = collision.transform; // Guarda la referencia del jugador
            // Intentar recoger inmediatamente al entrar en contacto
            IntentarRecoger();
        }
    }

    /// <summary>
    /// Detecta cuando el jugador sale del área de contacto del objeto.
    /// </summary>
    /// <param name="collision">Collider que ha salido del contacto con este objeto.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador") && !yaRecogido)
        {
            jugador = null;
            jugadorCerca = false;
        }
    }
}
