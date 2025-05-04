using UnityEngine;

/// <summary>
/// Gestiona la salud y el comportamiento de muerte de un enemigo.
/// </summary>
/// <remarks>
/// Esta clase controla la salud del enemigo, el procesamiento del daño recibido,
/// y las acciones a realizar cuando el enemigo es eliminado, como reproducir efectos,
/// soltar objetos y otorgar experiencia al jugador.
/// </remarks>
public class VidaEnemigo : MonoBehaviour
{
    /// <summary>
    /// Cantidad máxima de salud que puede tener el enemigo.
    /// </summary>
    public int saludMaxima = 100;
    
    /// <summary>
    /// Sonido que se reproduce cuando el enemigo muere.
    /// </summary>
    public AudioClip sonidoMuerte;

    /// <summary>
    /// Cantidad actual de salud del enemigo.
    /// </summary>
    private int saludActual;
    
    /// <summary>
    /// Componente para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;
    
    /// <summary>
    /// Referencia al componente que gestiona los objetos que suelta el enemigo al morir.
    /// </summary>
    private DropCalaveras dropObjeto;
    
    /// <summary>
    /// Referencia a la barra de experiencia del jugador.
    /// </summary>
    private BarraExperiencia barraExp;

    /// <summary>
    /// Inicializa los componentes y establece la salud inicial del enemigo.
    /// </summary>
    private void Start()
    {
        saludActual = saludMaxima;
        dropObjeto = GetComponent<DropCalaveras>();
        barraExp = FindAnyObjectByType<BarraExperiencia>();
        
        // Obtener o crear AudioSource para reproducir sonidos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Reduce la salud del enemigo y comprueba si debe morir.
    /// </summary>
    /// <param name="cantidad">Cantidad de daño a aplicar.</param>
    public void RecibirDaño(int cantidad)
    {
        saludActual -= cantidad;
        if (saludActual <= 0f)
        {
            Muerte();
        }
    }

    /// <summary>
    /// Maneja la muerte del enemigo, desactivando componentes, reproduciendo efectos
    /// y otorgando recompensas antes de destruir el objeto.
    /// </summary>
    private void Muerte()
    {
        // Desactivar visuales
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
        // Desactivar colisiones
        foreach (Collider c in GetComponents<Collider>())
            c.enabled = false;
        foreach (Collider2D c2d in GetComponents<Collider2D>())
            c2d.enabled = false;
        // Desactivar scripts de movimiento
        var movCol = GetComponent<MovimientoEnemigoColision>();
        if (movCol != null) movCol.enabled = false;
        var movDist = GetComponent<MovimientoEnemigoDistancia>();
        if (movDist != null) movDist.enabled = false;

        // Reproducir sonido de muerte antes de destruir
        if (sonidoMuerte != null)
        {
            // Usar el método estático para reproducir el sonido
            ExtensionesAudio.ReproducirEnPosicion(sonidoMuerte, transform.position, 1.0f, TipoAudio.Efectos);
        }

        // Llamar a SoltarObjeto si el enemigo tiene el script DropObjeto
        if (dropObjeto != null)
        {
            dropObjeto.SoltarObjeto();
        }
        
        // Aumenta la experiencia si está bien configurado
        if (barraExp != null)
        {
            barraExp.GanarExperiencia();
        }

        // Destruir el objeto inmediatamente, ya que el sonido continuará reproduciéndose
        Destroy(gameObject);
    }

    /// <summary>
    /// Devuelve la cantidad actual de salud del enemigo.
    /// </summary>
    /// <returns>Salud actual del enemigo.</returns>
    public int ObtenerSalud()
    {
        return saludActual;
    }

    /// <summary>
    /// Devuelve la cantidad máxima de salud del enemigo.
    /// </summary>
    /// <returns>Salud máxima del enemigo.</returns>
    public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
    
    /// <summary>
    /// Restablece la salud actual del enemigo a su valor máximo.
    /// </summary>
    /// <remarks>
    /// Útil para reiniciar el estado de un enemigo o cuando se reutilizan objetos.
    /// </remarks>
    public void ReiniciarSalud()
    {
        saludActual = saludMaxima;
    }
}
