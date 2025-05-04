using UnityEngine;

/// <summary>
/// Controla el movimiento del jugador en el juego.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar la entrada del usuario para mover al personaje,
/// aplicar la física del movimiento y reproducir los efectos de sonido correspondientes.
/// </remarks>
public class MovimientoJugador : MonoBehaviour 
{
    /// <summary>
    /// Velocidad de movimiento del jugador en unidades por segundo.
    /// </summary>
    public float velocity = 5f;
    
    /// <summary>
    /// Dirección actual del movimiento del jugador (normalizada).
    /// </summary>
    /// <remarks>
    /// Es pública para lectura pero solo puede ser modificada dentro de esta clase.
    /// </remarks>
    public Vector2 direction { get; private set; } // Es publico, pero solo se puede modificar dentro de PlayerMovement
    
    /// <summary>
    /// Sonido que se reproduce cuando el jugador se mueve.
    /// </summary>
    public AudioClip sonidoMovimiento; // Sonido de pasos al moverse
    
    /// <summary>
    /// Componente para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;
    
    /// <summary>
    /// Componente Rigidbody2D para el movimiento físico.
    /// </summary>
    private Rigidbody2D rigidBody;
    
    /// <summary>
    /// Volumen original del sonido de movimiento.
    /// </summary>
    private float volumenOriginal = 1.0f;

    /// <summary>
    /// Inicializa los componentes necesarios al inicio.
    /// </summary>
    private void Start() 
    {   
        rigidBody = GetComponent<Rigidbody2D>(); //Se obtiene el componente 
        // Obtener o crear AudioSource para pasos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        if (sonidoMovimiento != null) {
            audioSource.clip = sonidoMovimiento;
            audioSource.loop = true;
            volumenOriginal = audioSource.volume;
        }
    }

    /// <summary>
    /// Aplica el movimiento físico al Rigidbody2D del jugador.
    /// </summary>
    /// <remarks>
    /// Se ejecuta a una frecuencia constante, ideal para operaciones físicas.
    /// </remarks>
    private void FixedUpdate() 
    {
        //Crea el movimiento segun direccion y velocidad
        Vector2 targetPosition = rigidBody.position + direction * velocity * Time.fixedDeltaTime; 
        rigidBody.MovePosition(targetPosition);
    }

    /// <summary>
    /// Actualiza la dirección del movimiento y gestiona los efectos de sonido.
    /// </summary>
    /// <remarks>
    /// Se ejecuta una vez por cada fotograma renderizado.
    /// </remarks>
    private void Update() 
    {
        Movimiento();
        // Reproducir o detener sonido de movimiento
        if (audioSource != null && sonidoMovimiento != null) {
            // Actualizar el volumen según el sistema global
            audioSource.ActualizarVolumen(volumenOriginal, TipoAudio.Efectos);
            
            if (direction != Vector2.zero) {
                if (!audioSource.isPlaying)
                    audioSource.Play();
            } else {
                if (audioSource.isPlaying)
                    audioSource.Stop();
            }
        }
    }

    /// <summary>
    /// Obtiene la dirección del movimiento a partir de las entradas del usuario.
    /// </summary>
    private void Movimiento() 
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; //Obtiene la direccion del movimiento atraves de WASD o flechas
    }

    

}