using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor centralizado de sonidos para la interfaz de usuario.
/// Añadir a un GameObject en la escena que persista durante todo el juego.
/// </summary>
public class GestorSonidosUI : MonoBehaviour
{
    public static GestorSonidosUI instancia;

    // Sonidos básicos
    public AudioClip sonidoClic;
    public float volumenClic = 0.5f;
    
    public AudioClip sonidoError;
    public float volumenError = 0.7f;
    
    public AudioClip sonidoCompra;
    public float volumenCompra = 0.6f;
    
    private AudioSource _audioSource;
    
    // Propiedad para acceder al AudioSource desde otros scripts
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
    
    // Este método se llama cada vez que se carga una escena
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
    
    private void Start()
    {
        // Actualizar el volumen según la configuración global al inicio
        ActualizarVolumen();
    }
    
    // Actualiza el volumen según la configuración global
    private void ActualizarVolumen()
    {
        if (GestorAudioGlobal.instancia != null)
        {
            float volumen = GestorAudioGlobal.instancia.ObtenerVolumenParaSource(audioSource, TipoAudio.UI);
            audioSource.volume = volumen;
        }
    }

    // Métodos públicos para reproducir sonidos
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
    
    // Método privado para reproducir cualquier sonido
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