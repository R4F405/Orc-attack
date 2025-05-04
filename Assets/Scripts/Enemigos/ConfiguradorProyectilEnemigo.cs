using UnityEngine;

/// <summary>
/// Controla el comportamiento de los proyectiles lanzados por enemigos.
/// </summary>
/// <remarks>
/// Esta clase gestiona la configuración, movimiento, duración y efecto
/// de los proyectiles disparados por enemigos a distancia. Maneja la colisión
/// con el jugador para aplicar daño.
/// </remarks>
public class ConfiguradorProyectilEnemigo : MonoBehaviour
{
    /// <summary>
    /// Velocidad a la que se mueve el proyectil en unidades por segundo.
    /// </summary>
    public float velocidad = 5f;
    
    /// <summary>
    /// Tiempo en segundos que el proyectil permanece activo antes de autodestruirse.
    /// </summary>
    public float tiempoVida = 3f;

    /// <summary>
    /// Cantidad de daño que el proyectil inflige al jugador al impactar.
    /// </summary>
    private int daño;
    
    /// <summary>
    /// Dirección normalizada en la que se mueve el proyectil.
    /// </summary>
    private Vector2 direccion;

    /// <summary>
    /// Configura la dirección y el daño del proyectil, e inicia su movimiento.
    /// </summary>
    /// <param name="nuevaDireccion">Dirección normalizada en la que se moverá el proyectil.</param>
    /// <param name="nuevoDaño">Cantidad de daño que el proyectil infligirá al jugador.</param>
    public void Configurar(Vector2 nuevaDireccion, int nuevoDaño)
    {
        direccion = nuevaDireccion;
        daño = nuevoDaño;
        GetComponent<Rigidbody2D>().linearVelocity = direccion * velocidad;
    }

    /// <summary>
    /// Configura la autodestrucción del proyectil después de un tiempo determinado
    /// y configura las capas de colisión.
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, tiempoVida);
        
        // Ajustar el tamaño del collider para reducir colisiones accidentales
        AjustarCollider();
        
        // Asegurarse que el proyectil esté en la capa correcta
        int capaProyectilesEnemigos = LayerMask.NameToLayer("ProyectilesEnemigos");
        
        // Si la capa existe (no es -1)
        if (capaProyectilesEnemigos != -1)
        {
            // Asignar la capa al proyectil
            gameObject.layer = capaProyectilesEnemigos;
            
            // Ignorar colisiones entre proyectiles de la misma capa
            Physics2D.IgnoreLayerCollision(capaProyectilesEnemigos, capaProyectilesEnemigos, true);
            
            // Opcional: hacer que los proyectiles ignoren colisiones con enemigos
            int capaEnemigos = LayerMask.NameToLayer("Enemigos");
            if (capaEnemigos != -1)
            {
                Physics2D.IgnoreLayerCollision(capaProyectilesEnemigos, capaEnemigos, true);
            }
        }
        else
        {
            // Si la capa no existe, usar el método alternativo por gameObject.layer
            Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true);
            Debug.LogWarning("La capa 'ProyectilesEnemigos' no existe. Por favor, crea esta capa en el editor de Unity.");
        }
    }
    
    /// <summary>
    /// Ajusta el tamaño del collider del proyectil para evitar colisiones accidentales.
    /// </summary>
    private void AjustarCollider()
    {
        // Intentar obtener el collider del proyectil
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        
        // Ajustar el collider circular si existe
        if (circleCollider != null)
        {
            // Reducir un poco el radio para evitar colisiones accidentales
            circleCollider.radius *= 0.9f;
        }
        // Ajustar el collider de caja si existe
        else if (boxCollider != null)
        {
            // Reducir el tamaño para evitar colisiones accidentales
            boxCollider.size *= 0.9f;
        }
    }

    /// <summary>
    /// Detecta colisiones con el jugador, aplica daño y destruye el proyectil.
    /// </summary>
    /// <param name="col">Collider con el que ha colisionado el proyectil.</param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Jugador"))
        {
            VidaJugador saludJugador = col.GetComponent<VidaJugador>();
            if (saludJugador != null)
            {
                saludJugador.RecibirDaño(daño);
            } 
            Destroy(gameObject);
        } 
    }
}
