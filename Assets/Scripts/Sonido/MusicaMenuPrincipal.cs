using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la música del menú principal.
/// </summary>
public class MusicaMenuPrincipal : MonoBehaviour
{
    [Tooltip("Clip de música para el menú principal")]
    [SerializeField] private AudioClip musicaMenu;
    
    [Tooltip("Volumen específico para la música del menú (0-1)")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float volumenMenu = 1.0f;
    
    private void Start()
    {
        // Asegurarnos de que la música del juego se detiene
        if (GestorAudioGlobal.instancia != null)
        {
            // Detener cualquier música que esté sonando
            GestorAudioGlobal.instancia.DetenerMusica();
            
            // Reproducir música del menú
            if (musicaMenu != null)
            {
                GestorAudioGlobal.instancia.ReproducirMusica(musicaMenu);
                Debug.Log("MusicaMenuPrincipal: Reproduciendo música del menú");
            }
            else
            {
                Debug.LogWarning("MusicaMenuPrincipal: No hay clip de música asignado para el menú");
            }
        }
        else
        {
            Debug.LogWarning("MusicaMenuPrincipal: No se encontró instancia de GestorAudioGlobal");
        }
    }
    
    /// <summary>
    /// Detiene la música del menú
    /// </summary>
    public void DetenerMusicaMenu()
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.DetenerMusica();
        }
    }
    
    /// <summary>
    /// Llamar esta función cuando se inicie un nivel desde el menú
    /// para detener la música del menú y preparar la transición
    /// </summary>
    public void PrepararTransicionANivel()
    {
        DetenerMusicaMenu();
    }
    
    /// <summary>
    /// Este método puede ser llamado por botones del menú
    /// </summary>
    public void IniciarNivel(string nombreNivel)
    {
        PrepararTransicionANivel();
        SceneManager.LoadScene(nombreNivel);
    }
} 