using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor global de audio para controlar el volumen general del juego.
/// Este script debe añadirse a un único GameObject en la escena principal
/// que se mantenga presente en todo el juego (mediante DontDestroyOnLoad).
/// </summary>
public class GestorAudioGlobal : MonoBehaviour
{
    public static GestorAudioGlobal instancia;

    // Volumen global para todo el juego (0.0f a 1.0f)
    [Range(0.0f, 1.0f)]
    public float volumenGlobal = 1.0f;

    // Volúmenes por categoría
    [Range(0.0f, 1.0f)]
    public float volumenMusica = 1.0f;
    
    [Range(0.0f, 1.0f)]
    public float volumenEfectos = 1.0f;
    
    [Range(0.0f, 1.0f)]
    public float volumenUI = 1.0f;

    // Referencias a AudioSources principales
    [SerializeField]
    private AudioSource musicaAudioSource;
    
    // Clips de música
    [Header("Música")]
    [SerializeField]
    private AudioClip musicaPrincipal;
    
    [SerializeField]
    private bool reproducirMusicaAlIniciar = true;
    
    [Header("Configuración de Escenas")]
    [Tooltip("Si está activado, detiene la música al cambiar de escena")]
    [SerializeField]
    private bool detenerMusicaAlCambiarEscena = false;
    
    [Tooltip("Si está activado, busca componentes MusicaEscena al cargar una nueva escena")]
    [SerializeField]
    private bool buscarMusicaEscenaAutomaticamente = true;
    
    // Información sobre la escena actual
    private string escenaActual;
    private MusicaEscena musicaEscenaActual;

    // Nombre de la clave PlayerPrefs para guardar configuración
    private const string PREF_VOLUMEN_GLOBAL = "VolumenGlobal";
    private const string PREF_VOLUMEN_MUSICA = "VolumenMusica";
    private const string PREF_VOLUMEN_EFECTOS = "VolumenEfectos";
    private const string PREF_VOLUMEN_UI = "VolumenUI";

    private void Awake()
    {
        // Patrón Singleton
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            
            // Cargar valores guardados anteriormente
            CargarConfiguracionAudio();
            
            // Inicializar componentes
            if (musicaAudioSource == null)
            {
                musicaAudioSource = gameObject.AddComponent<AudioSource>();
                musicaAudioSource.loop = true;
                musicaAudioSource.playOnAwake = false;
            }
            
            // Registrar para eventos de cambio de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // Almacenar escena actual
            escenaActual = SceneManager.GetActiveScene().name;
            
            Debug.Log("GestorAudioGlobal inicializado en escena: " + escenaActual);
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Desregistrar del evento de cambio de escena si este objeto es destruido
        if (instancia == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        // Aplicar volumen inicial a todos los AudioSources activos en la escena
        ActualizarVolumenTodosLosSources();
        
        // Reproducir música si está configurado y no hay una MusicaEscena específica
        if (reproducirMusicaAlIniciar && musicaEscenaActual == null)
        {
            ReproducirMusica(musicaPrincipal);
        }
    }
    
    /// <summary>
    /// Se llama automáticamente cuando se carga una nueva escena
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Guardar nombre de la escena anterior
        string escenaAnterior = escenaActual;
        escenaActual = scene.name;
        
        Debug.Log($"GestorAudioGlobal: Cambio de escena de '{escenaAnterior}' a '{escenaActual}'");
        
        // Detener música si está configurado
        if (detenerMusicaAlCambiarEscena)
        {
            DetenerMusica();
        }
        
        // Actualizar volumen para todos los AudioSources de la nueva escena
        ActualizarVolumenTodosLosSources();
        
        // Buscar componente MusicaEscena en la nueva escena
        if (buscarMusicaEscenaAutomaticamente)
        {
            // Eliminar referencia a la música de escena anterior
            musicaEscenaActual = null;
            
            // Buscar nuevo componente MusicaEscena
            MusicaEscena[] musicasEscena = FindObjectsOfType<MusicaEscena>();
            if (musicasEscena.Length > 0)
            {
                if (musicasEscena.Length > 1)
                {
                    Debug.LogWarning($"GestorAudioGlobal: Se encontraron múltiples componentes MusicaEscena en '{escenaActual}'. Se usará el primero encontrado.");
                }
                
                musicaEscenaActual = musicasEscena[0];
                Debug.Log($"GestorAudioGlobal: Se encontró MusicaEscena en '{escenaActual}'");
                
                // La reproducción se maneja en el Start() de MusicaEscena
            }
        }
    }

    /// <summary>
    /// Reproduce un clip de música específico
    /// </summary>
    /// <param name="clip">AudioClip a reproducir, si es null usa musicaPrincipal</param>
    public void ReproducirMusica(AudioClip clip = null)
    {
        // Si no se especifica clip, usar el principal
        AudioClip clipAReproducir = clip != null ? clip : musicaPrincipal;
        
        if (musicaAudioSource != null && clipAReproducir != null)
        {
            Debug.Log("GestorAudioGlobal: Reproduciendo música: " + clipAReproducir.name);
            
            // Configurar el AudioSource
            musicaAudioSource.clip = clipAReproducir;
            musicaAudioSource.volume = volumenMusica * volumenGlobal;
            musicaAudioSource.loop = true;
            
            // Reproducir
            musicaAudioSource.Play();
        }
        else
        {
            if (musicaAudioSource == null)
                Debug.LogWarning("GestorAudioGlobal: No hay AudioSource para música");
            
            if (clipAReproducir == null)
                Debug.LogWarning("GestorAudioGlobal: No hay clip de música asignado");
        }
    }
    
    /// <summary>
    /// Detiene la reproducción de música
    /// </summary>
    public void DetenerMusica()
    {
        if (musicaAudioSource != null && musicaAudioSource.isPlaying)
        {
            musicaAudioSource.Stop();
            Debug.Log("GestorAudioGlobal: Música detenida");
        }
    }
    
    /// <summary>
    /// Pausa o reanuda la reproducción de música
    /// </summary>
    public void PausarReanudarMusica(bool pausar)
    {
        if (musicaAudioSource != null)
        {
            if (pausar && musicaAudioSource.isPlaying)
            {
                musicaAudioSource.Pause();
                Debug.Log("GestorAudioGlobal: Música pausada");
            }
            else if (!pausar && !musicaAudioSource.isPlaying && musicaAudioSource.clip != null)
            {
                musicaAudioSource.UnPause();
                Debug.Log("GestorAudioGlobal: Música reanudada");
            }
        }
    }

    /// <summary>
    /// Establece el volumen global para todo el audio del juego
    /// </summary>
    public void EstablecerVolumenGlobal(float nuevoVolumen)
    {
        volumenGlobal = Mathf.Clamp01(nuevoVolumen);
        ActualizarVolumenTodosLosSources();
        
        // Guardar configuración
        PlayerPrefs.SetFloat(PREF_VOLUMEN_GLOBAL, volumenGlobal);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Establece el volumen para la música
    /// </summary>
    public void EstablecerVolumenMusica(float nuevoVolumen)
    {
        volumenMusica = Mathf.Clamp01(nuevoVolumen);
        ActualizarVolumenTodosLosSources();
        
        // Guardar configuración
        PlayerPrefs.SetFloat(PREF_VOLUMEN_MUSICA, volumenMusica);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Establece el volumen para efectos de sonido
    /// </summary>
    public void EstablecerVolumenEfectos(float nuevoVolumen)
    {
        volumenEfectos = Mathf.Clamp01(nuevoVolumen);
        ActualizarVolumenTodosLosSources();
        
        // Guardar configuración
        PlayerPrefs.SetFloat(PREF_VOLUMEN_EFECTOS, volumenEfectos);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Establece el volumen para sonidos de interfaz
    /// </summary>
    public void EstablecerVolumenUI(float nuevoVolumen)
    {
        volumenUI = Mathf.Clamp01(nuevoVolumen);
        
        // Actualizar el AudioSource de GestorSonidosUI
        if (GestorSonidosUI.instancia != null)
        {
            GestorSonidosUI.instancia.audioSource.volume = volumenUI * volumenGlobal;
        }
        
        // Guardar configuración
        PlayerPrefs.SetFloat(PREF_VOLUMEN_UI, volumenUI);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carga la configuración de audio desde PlayerPrefs
    /// </summary>
    private void CargarConfiguracionAudio()
    {
        volumenGlobal = PlayerPrefs.HasKey(PREF_VOLUMEN_GLOBAL) 
            ? PlayerPrefs.GetFloat(PREF_VOLUMEN_GLOBAL) 
            : 1.0f;
            
        volumenMusica = PlayerPrefs.HasKey(PREF_VOLUMEN_MUSICA) 
            ? PlayerPrefs.GetFloat(PREF_VOLUMEN_MUSICA) 
            : 1.0f;
            
        volumenEfectos = PlayerPrefs.HasKey(PREF_VOLUMEN_EFECTOS) 
            ? PlayerPrefs.GetFloat(PREF_VOLUMEN_EFECTOS) 
            : 1.0f;
            
        volumenUI = PlayerPrefs.HasKey(PREF_VOLUMEN_UI) 
            ? PlayerPrefs.GetFloat(PREF_VOLUMEN_UI) 
            : 1.0f;
    }

    /// <summary>
    /// Aplica la configuración de volumen a todos los AudioSource en la escena
    /// </summary>
    public void ActualizarVolumenTodosLosSources()
    {
        // Aplicar a AudioListener (afecta a todos los sonidos)
        // Unity cambia el volumen global a través de AudioListener.volume
        AudioListener.volume = volumenGlobal;
        
        // Ajustar el volumen de la música
        if (musicaAudioSource != null)
        {
            musicaAudioSource.volume = volumenMusica * volumenGlobal;
        }
        
        // Actualizar el AudioSource del GestorSonidosUI
        if (GestorSonidosUI.instancia != null)
        {
            GestorSonidosUI.instancia.audioSource.volume = volumenUI * volumenGlobal;
        }
    }

    /// <summary>
    /// Aplica la configuración de volumen a un AudioSource específico según su tipo
    /// </summary>
    public float ObtenerVolumenParaSource(AudioSource source, TipoAudio tipo)
    {
        switch (tipo)
        {
            case TipoAudio.Musica:
                return volumenMusica * volumenGlobal;
            case TipoAudio.Efectos:
                return volumenEfectos * volumenGlobal;
            case TipoAudio.UI:
                return volumenUI * volumenGlobal;
            default:
                return volumenGlobal;
        }
    }

    /// <summary>
    /// Reproduce un sonido ajustando su volumen según el tipo y controladores globales
    /// </summary>
    public void ReproducirSonido(AudioSource source, AudioClip clip, float volumenBase, TipoAudio tipo)
    {
        if (source == null || clip == null)
            return;
            
        float volumenFinal = volumenBase;
        
        // Aplicar multiplicadores según el tipo
        switch (tipo)
        {
            case TipoAudio.Musica:
                volumenFinal *= volumenMusica * volumenGlobal;
                break;
            case TipoAudio.Efectos:
                volumenFinal *= volumenEfectos * volumenGlobal;
                break;
            case TipoAudio.UI:
                volumenFinal *= volumenUI * volumenGlobal;
                break;
        }
        
        source.PlayOneShot(clip, volumenFinal);
    }
}

/// <summary>
/// Enum para identificar el tipo de sonido
/// </summary>
public enum TipoAudio
{
    Musica,
    Efectos,
    UI
} 