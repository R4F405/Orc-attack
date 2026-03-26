using UnityEngine;

/// <summary>
/// Define los niveles de rareza de las armas y sus propiedades.
/// Nivel 1: Gris (Común), Nivel 2: Verde (Poco Común), Nivel 3: Azul (Raro),
/// Nivel 4: Morado (Épico), Nivel 5: Naranja (Legendario).
/// </summary>
public static class NivelArma
{
    public const int NIVEL_MIN = 1;
    public const int NIVEL_MAX = 5;

    private static readonly Color[] colores =
    {
        Color.white,                        // 0 - no usado
        new Color(0.65f, 0.65f, 0.65f),     // 1 - Gris
        new Color(0.2f, 0.8f, 0.2f),        // 2 - Verde
        new Color(0.3f, 0.5f, 1.0f),        // 3 - Azul
        new Color(0.7f, 0.3f, 0.9f),        // 4 - Morado
        new Color(1.0f, 0.5f, 0.0f),        // 5 - Naranja
    };

    private static readonly string[] nombres =
    {
        "",             // 0
        "Común",        // 1
        "Poco Común",   // 2
        "Raro",         // 3
        "Épico",        // 4
        "Legendario"    // 5
    };

    private static readonly float[] multiplicadores =
    {
        1.0f,   // 0 - no usado
        1.0f,   // 1 - Gris
        1.25f,  // 2 - Verde
        1.55f,  // 3 - Azul
        1.9f,   // 4 - Morado
        2.5f,   // 5 - Naranja
    };

    public static Color ObtenerColor(int nivel)
    {
        return colores[Mathf.Clamp(nivel, NIVEL_MIN, NIVEL_MAX)];
    }

    public static string ObtenerNombreNivel(int nivel)
    {
        return nombres[Mathf.Clamp(nivel, NIVEL_MIN, NIVEL_MAX)];
    }

    public static float ObtenerMultiplicador(int nivel)
    {
        return multiplicadores[Mathf.Clamp(nivel, NIVEL_MIN, NIVEL_MAX)];
    }

    public static bool PuedeMejorar(int nivel)
    {
        return nivel < NIVEL_MAX;
    }
}
