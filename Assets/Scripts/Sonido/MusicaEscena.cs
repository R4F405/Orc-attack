using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la música específica de cada escena.
/// Añadir a un GameObject en cada escena con su música correspondiente.
/// </summary>
public class MusicaEscena : MonoBehaviour
{
    [Tooltip("Clip de música para esta escena")]
    [SerializeField] private AudioClip musicaEscena;
    
    [Tooltip("Controla si esta música debe reproducirse automáticamente al iniciar la escena")]
    [SerializeField] private bool reproducirAutomaticamente = true;
    
    [Tooltip("Volumen específico para esta música (0-1)")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float volumenEscena = 1.0f;
    
    [Tooltip("Tiempo de transición para fundido entre músicas (en segundos)")]
    [SerializeField] private float tiempoTransicion = 1.0f;
    
    private void Start()
    {
        // Reproducir automáticamente si está configurado
        if (reproducirAutomaticamente && musicaEscena != null)
        {
            ReproducirMusicaEscena();
        }
    }
    
    /// <summary>
    /// Reproduce la música de esta escena, deteniendo cualquier otra música que esté sonando
    /// </summary>
    public void ReproducirMusicaEscena()
    {
        if (GestorAudioGlobal.instancia != null && musicaEscena != null)
        {
            // Detener cualquier música actual y reproducir la de esta escena
            GestorAudioGlobal.instancia.DetenerMusica();
            GestorAudioGlobal.instancia.ReproducirMusica(musicaEscena);
            
            // Ajustar volumen específico para esta escena
            if (volumenEscena < 1.0f)
            {
                GestorAudioGlobal.instancia.EstablecerVolumenMusica(volumenEscena);
            }
            
            Debug.Log($"MusicaEscena: Reproduciendo música '{musicaEscena.name}' para escena '{SceneManager.GetActiveScene().name}' con volumen {volumenEscena}");
        }
        else
        {
            if (GestorAudioGlobal.instancia == null)
                Debug.LogWarning("MusicaEscena: No se encontró instancia de GestorAudioGlobal");
                
            if (musicaEscena == null)
                Debug.LogWarning("MusicaEscena: No hay clip de música asignado para esta escena");
        }
    }
    
    /// <summary>
    /// Detiene la música de esta escena
    /// </summary>
    public void DetenerMusicaEscena()
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.DetenerMusica();
            Debug.Log("MusicaEscena: Música detenida");
        }
    }
    
    /// <summary>
    /// Pausa la música de esta escena
    /// </summary>
    public void PausarMusicaEscena()
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.PausarReanudarMusica(true);
        }
    }
    
    /// <summary>
    /// Reanuda la música de esta escena
    /// </summary>
    public void ReanudarMusicaEscena()
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.PausarReanudarMusica(false);
        }
    }
    
    /// <summary>
    /// Realiza una transición suave a otra música
    /// </summary>
    /// <param name="nuevaMusica">Nuevo clip de audio a reproducir</param>
    public void TransicionMusica(AudioClip nuevaMusica)
    {
        if (GestorAudioGlobal.instancia != null && nuevaMusica != null)
        {
            // Implementación simple sin fundido real
            // Para implementar un fundido real, se necesitaría una coroutine
            // que gradualmente baje y suba el volumen
            Debug.Log($"MusicaEscena: Transición a nueva música con tiempo {tiempoTransicion}s");
            
            GestorAudioGlobal.instancia.DetenerMusica();
            GestorAudioGlobal.instancia.ReproducirMusica(nuevaMusica);
            
            // En una implementación completa, aquí usaríamos el tiempo de transición
            // para hacer un fundido gradual
        }
    }
    
    private void OnDestroy()
    {
        // Al destruir el objeto (cambio de escena), podemos detener la música
        // Esto es opcional y dependerá de cómo quieras manejar las transiciones
        // Si quieres que la música continúe entre escenas, comenta esta línea
        // DetenerMusicaEscena();
    }
} 