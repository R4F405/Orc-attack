using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor centralizado de sonidos para la interfaz de usuario.
/// </summary>
/// <remarks>
/// Este componente debe añadirse a un GameObject que persistirá durante todo el juego
/// mediante el patrón Singleton. Se encarga de reproducir sonidos comunes de la interfaz
/// como clics, errores y compras, garantizando una experiencia auditiva consistente en
/// todas las pantallas y menús del juego.
/// 
/// Interactúa con el GestorAudioGlobal para respetar la configuración de volumen del jugador.
/// </remarks>
public class GestorSonidosUI : MonoBehaviour
{
    /// <summary>
    /// Referencia estática a la instancia única del gestor de sonidos UI (patrón Singleton).
    /// </summary>
    public static GestorSonidosUI instancia;

    /// <summary>
    /// Clip de audio para el sonido de clic en botones y elementos interactivos.
    /// </summary>
    public AudioClip sonidoClic;
    
    /// <summary>
    /// Volumen base para el sonido de clic (0-1).
    /// </summary>
    public float volumenClic = 0.5f;
    
    /// <summary>
    /// Clip de audio para el sonido de error o acción no permitida.
    /// </summary>
    public AudioClip sonidoError;
    
    /// <summary>
    /// Volumen base para el sonido de error (0-1).
    /// </summary>
    public float volumenError = 0.7f;
    
    /// <summary>
    /// Clip de audio para el sonido de compra o transacción exitosa.
    /// </summary>
    public AudioClip sonidoCompra;
    
    /// <summary>
    /// Volumen base para el sonido de compra (0-1).
    /// </summary>
    public float volumenCompra = 0.6f;
    
    /// <summary>
    /// Referencia interna al componente AudioSource utilizado para reproducir los sonidos.
    /// </summary>
    private AudioSource _audioSource;
    
    /// <summary>
    /// Propiedad para acceder al AudioSource desde otros scripts, garantizando su existencia.
    /// </summary>
    /// <remarks>
    /// Si el AudioSource no existe, lo crea automáticamente, asegurando que siempre
    /// haya un componente válido para reproducir sonidos.
    /// </remarks>
    public AudioSource audioSource 
    {
        get 
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    _audioSource = gameObject.AddComponent<AudioSource>();
                    _audioSource.playOnAwake = false;
                }
            }
            return _audioSource;
        }
    }

    /// <summary>
    /// Inicializa el gestor de sonidos UI, configurando el patrón Singleton.
    /// </summary>
    /// <remarks>
    /// Establece esta instancia como persistente entre escenas y configura
    /// el AudioSource necesario para reproducir sonidos.
    /// </remarks>
    private void Awake()
    {
        // Patrón Singleton simple
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            
            // Asegurarnos de tener un AudioSource
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }
            
            // Registrar para eventos de cambio de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            Debug.Log("GestorSonidosUI: Inicializado como singleton");
        }
        else if (instancia != this)
        {
            Debug.Log("GestorSonidosUI: Ya existe otra instancia, destruyendo esta");
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Limpia las referencias y suscripciones cuando se destruye el objeto.
    /// </summary>
    /// <remarks>
    /// Cancela la suscripción al evento de carga de escenas y limpia la referencia
    /// singleton si esta instancia es la global.
    /// </remarks>
    private void OnDestroy()
    {
        // Desuscribirse de eventos si este objeto es destruido
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        // Si esta instancia es la global, limpiar la referencia
        if (instancia == this)
        {
            Debug.Log("GestorSonidosUI: La instancia singleton se está destruyendo");
            instancia = null;
        }
    }
    
    /// <summary>
    /// Manejador de evento que se ejecuta cada vez que se carga una nueva escena.
    /// </summary>
    /// <param name="scene">La escena que se acaba de cargar.</param>
    /// <param name="mode">El modo en que se cargó la escena.</param>
    /// <remarks>
    /// Asegura que el AudioSource esté habilitado después de cargar una nueva escena
    /// y actualiza el volumen de acuerdo con la configuración global.
    /// </remarks>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GestorSonidosUI: Nueva escena cargada - " + scene.name);
        
        // Asegurarnos de que el AudioSource esté habilitado después de cargar una nueva escena
        if (_audioSource != null && !_audioSource.enabled)
        {
            _audioSource.enabled = true;
            Debug.Log("GestorSonidosUI: Reactivado AudioSource después de cargar escena");
        }
        
        // Actualizar el volumen desde el GestorAudioGlobal si existe
        ActualizarVolumen();
    }
    
    /// <summary>
    /// Inicializa el AudioSource y actualiza su volumen según la configuración global.
    /// </summary>
    private void Start()
    {
        // Actualizar el volumen según la configuración global al inicio
        ActualizarVolumen();
    }
    
    /// <summary>
    /// Actualiza el volumen del AudioSource de acuerdo con la configuración global.
    /// </summary>
    /// <remarks>
    /// Este método sincroniza el volumen del AudioSource con el GestorAudioGlobal,
    /// aplicando los modificadores de volumen para sonidos de UI.
    /// </remarks>
    private void ActualizarVolumen()
    {
        if (GestorAudioGlobal.instancia != null)
        {
            float volumen = GestorAudioGlobal.instancia.ObtenerVolumenParaSource(audioSource, TipoAudio.UI);
            audioSource.volume = volumen;
        }
    }

    /// <summary>
    /// Reproduce el sonido estándar de clic para elementos de interfaz.
    /// </summary>
    /// <remarks>
    /// Se utiliza para proporcionar retroalimentación auditiva cuando el jugador
    /// interactúa con elementos como botones, toggles, etc.
    /// </remarks>
    public void ReproducirSonidoClic()
    {
        if (sonidoClic == null)
        {
            Debug.LogWarning("GestorSonidosUI: No hay clip para reproducir sonido de clic");
            return;
        }
        
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.ReproducirSonido(audioSource, sonidoClic, volumenClic, TipoAudio.UI);
        }
        else
        {
            ReproducirSonido(sonidoClic, volumenClic);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de error para acciones no permitidas.
    /// </summary>
    /// <remarks>
    /// Este sonido debe utilizarse para notificar al jugador cuando una acción
    /// no se puede realizar, como intentar una compra sin suficientes recursos.
    /// </remarks>
    public void ReproducirSonidoError()
    {
        if (sonidoError == null)
        {
            Debug.LogWarning("GestorSonidosUI: No hay clip para reproducir sonido de error");
            return;
        }
        
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.ReproducirSonido(audioSource, sonidoError, volumenError, TipoAudio.UI);
        }
        else
        {
            ReproducirSonido(sonidoError, volumenError);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de compra o transacción exitosa.
    /// </summary>
    /// <remarks>
    /// Este sonido debe utilizarse para confirmar transacciones completadas exitosamente,
    /// como compras de objetos o mejoras en la tienda del juego.
    /// </remarks>
    public void ReproducirSonidoCompra()
    {
        if (sonidoCompra == null)
        {
            Debug.LogWarning("GestorSonidosUI: No hay clip para reproducir sonido de compra");
            return;
        }
        
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.ReproducirSonido(audioSource, sonidoCompra, volumenCompra, TipoAudio.UI);
        }
        else
        {
            ReproducirSonido(sonidoCompra, volumenCompra);
        }
    }
    
    /// <summary>
    /// Método interno para reproducir un clip de audio con un volumen específico.
    /// </summary>
    /// <param name="clip">El clip de audio a reproducir.</param>
    /// <param name="volumen">El volumen base para la reproducción (0-1).</param>
    /// <remarks>
    /// Este método es una implementación de respaldo que se utiliza cuando no está
    /// disponible el GestorAudioGlobal. Se asegura de que el AudioSource esté disponible
    /// y habilitado antes de reproducir el sonido.
    /// </remarks>
    private void ReproducirSonido(AudioClip clip, float volumen)
    {
        // Si no tenemos clip o audiosource, salir
        if (clip == null)
        {
            Debug.LogWarning("GestorSonidosUI: Intentando reproducir un clip nulo");
            return;
        }
            
        // Obtener el AudioSource usando la propiedad
        AudioSource source = audioSource;
        
        // Si el AudioSource está deshabilitado, habilitarlo
        if (source == null)
        {
            Debug.LogError("GestorSonidosUI: AudioSource es nulo, recreándolo");
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            _audioSource = source;
        }
        
        if (!source.enabled)
        {
            Debug.LogWarning("GestorSonidosUI: AudioSource deshabilitado, habilitándolo");
            source.enabled = true;
        }
        
        // Reproducir el sonido
        source.PlayOneShot(clip, volumen);
    }
} 