using UnityEngine;

/// <summary>
/// Proporciona métodos de extensión para facilitar el uso del sistema de audio global en el juego.
/// </summary>
/// <remarks>
/// Esta clase estática contiene extensiones para AudioSource que permiten integrar
/// fácilmente cualquier sistema de sonido con el GestorAudioGlobal, asegurando que
/// todos los sonidos respeten la configuración de volumen del jugador. También incluye
/// métodos estáticos para reproducir sonidos en posiciones específicas del mundo.
/// </remarks>
public static class ExtensionesAudio
{
    /// <summary>
    /// Reproduce un clip de audio con el volumen global aplicado según la configuración del juego.
    /// </summary>
    /// <param name="source">AudioSource que reproducirá el clip.</param>
    /// <param name="clip">AudioClip a reproducir.</param>
    /// <param name="volumenBase">Volumen base del sonido (0-1).</param>
    /// <param name="tipo">Tipo de audio (afecta al multiplicador de volumen que se aplicará).</param>
    /// <remarks>
    /// Este método se asegura de que el sonido respete la configuración de volumen del jugador,
    /// utilizando el GestorAudioGlobal si está disponible.
    /// </remarks>
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
    /// Calcula y devuelve el volumen ajustado según la configuración global del audio.
    /// </summary>
    /// <param name="source">AudioSource para el que se calculará el volumen.</param>
    /// <param name="volumenBase">Volumen base original (0-1).</param>
    /// <param name="tipo">Tipo de audio que determina qué multiplicador se aplicará.</param>
    /// <returns>El volumen ajustado teniendo en cuenta la configuración global del juego.</returns>
    /// <remarks>
    /// Útil cuando se necesita conocer el volumen ajustado sin aplicarlo directamente
    /// al AudioSource, por ejemplo, para cálculos o visualizaciones.
    /// </remarks>
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
    /// Actualiza el volumen del AudioSource según la configuración global actual.
    /// </summary>
    /// <param name="source">AudioSource al que se le aplicará el nuevo volumen.</param>
    /// <param name="volumenBase">Volumen base original (0-1).</param>
    /// <param name="tipo">Tipo de audio que determina qué multiplicador se aplicará.</param>
    /// <remarks>
    /// Útil para mantener todos los AudioSources sincronizados con la configuración
    /// global cuando el jugador cambia los ajustes de volumen.
    /// </remarks>
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
    
    /// <summary>
    /// Reproduce un sonido en una posición específica del mundo respetando el volumen global.
    /// </summary>
    /// <param name="clip">El clip de audio a reproducir.</param>
    /// <param name="posicion">La posición 3D donde reproducir el sonido.</param>
    /// <param name="volumenBase">Volumen base del sonido (0-1).</param>
    /// <param name="tipo">Tipo de audio para aplicar el multiplicador de volumen adecuado.</param>
    /// <remarks>
    /// Este método crea un AudioSource temporal en la posición especificada, que será
    /// destruido automáticamente cuando termine de reproducir el sonido. Es útil para
    /// efectos de sonido que deben estar vinculados a un punto específico del espacio
    /// pero no necesitan un GameObject permanente.
    /// 
    /// El sonido seguirá reproduciéndose incluso si el objeto que lo llamó es destruido.
    /// </remarks>
    public static void ReproducirEnPosicion(AudioClip clip, Vector3 posicion, float volumenBase = 1.0f, TipoAudio tipo = TipoAudio.Efectos)
    {
        if (clip == null)
            return;
            
        float volumenFinal = volumenBase;
        
        if (GestorAudioGlobal.instancia != null)
        {
            // Calcular el volumen según la configuración global
            switch (tipo)
            {
                case TipoAudio.Musica:
                    volumenFinal = GestorAudioGlobal.instancia.volumenMusica * GestorAudioGlobal.instancia.volumenGlobal * volumenBase;
                    break;
                case TipoAudio.Efectos:
                    volumenFinal = GestorAudioGlobal.instancia.volumenEfectos * GestorAudioGlobal.instancia.volumenGlobal * volumenBase;
                    break;
                case TipoAudio.UI:
                    volumenFinal = GestorAudioGlobal.instancia.volumenUI * GestorAudioGlobal.instancia.volumenGlobal * volumenBase;
                    break;
            }
        }
        
        // Reproducir el sonido con el volumen ajustado
        AudioSource.PlayClipAtPoint(clip, posicion, volumenFinal);
    }
} 