using UnityEngine;

public class MovimientoJugador : MonoBehaviour 
{

    public float velocity = 5f;
    public Vector2 direction { get; private set; } // Es publico, pero solo se puede modificar dentro de PlayerMovement
    public AudioClip sonidoMovimiento; // Sonido de pasos al moverse
    private AudioSource audioSource;
    private Rigidbody2D rigidBody;
    private float volumenOriginal = 1.0f;

    //Se llama al empezar
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

    //Se llama 50/s
    private void FixedUpdate() 
    {
        //Crea el movimiento segun direccion y velocidad
        Vector2 targetPosition = rigidBody.position + direction * velocity * Time.fixedDeltaTime; 
        rigidBody.MovePosition(targetPosition);
    }

    //Se llama tras cada frame
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

    //Funcion de movimiento
    private void Movimiento() 
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; //Obtiene la direccion del movimiento atraves de WASD o flechas
    }

    

}