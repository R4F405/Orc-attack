using UnityEngine;

/// <summary>
/// Métodos de extensión para facilitar el uso del sistema de audio global
/// </summary>
public static class ExtensionesAudio
{
    /// <summary>
    /// Reproduce un clip de audio con el volumen global aplicado
    /// </summary>
    /// <param name="source">AudioSource que reproducirá el clip</param>
    /// <param name="clip">AudioClip a reproducir</param>
    /// <param name="volumenBase">Volumen base (0-1)</param>
    /// <param name="tipo">Tipo de audio (afecta al multiplicador de volumen)</param>
    public static void ReproducirConVolumenGlobal(this AudioSource source, AudioClip clip, float volumenBase = 1.0f, TipoAudio tipo = TipoAudio.Efectos)
    {
        if (source == null || clip == null)
            return;
            
        // Si existe el gestor global, usar su sistema
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.ReproducirSonido(source, clip, volumenBase, tipo);
        }
        else
        {
            // De lo contrario, reproducir normalmente
            source.PlayOneShot(clip, volumenBase);
        }
    }
    
    /// <summary>
    /// Asigna el volumen correcto según el tipo de audio y la configuración global
    /// </summary>
    /// <param name="source">AudioSource al que se le aplicará el volumen</param>
    /// <param name="volumenBase">Volumen base original (0-1)</param>
    /// <param name="tipo">Tipo de audio</param>
    /// <returns>El volumen base si no hay gestor global, o el volumen ajustado si existe</returns>
    public static float ObtenerVolumenAjustado(this AudioSource source, float volumenBase = 1.0f, TipoAudio tipo = TipoAudio.Efectos)
    {
        if (source == null)
            return volumenBase;
            
        if (GestorAudioGlobal.instancia != null)
        {
            return GestorAudioGlobal.instancia.ObtenerVolumenParaSource(source, tipo) * volumenBase;
        }
        
        return volumenBase;
    }
    
    /// <summary>
    /// Aplica el volumen global actual al AudioSource según su tipo
    /// </summary>
    /// <param name="source">AudioSource al que se le aplicará el volumen</param>
    /// <param name="volumenBase">Volumen base original (0-1)</param>
    /// <param name="tipo">Tipo de audio</param>
    public static void ActualizarVolumen(this AudioSource source, float volumenBase = 1.0f, TipoAudio tipo = TipoAudio.Efectos)
    {
        if (source == null)
            return;
            
        if (GestorAudioGlobal.instancia != null)
        {
            source.volume = GestorAudioGlobal.instancia.ObtenerVolumenParaSource(source, tipo) * volumenBase;
        }
        else
        {
            source.volume = volumenBase;
        }
    }
} 