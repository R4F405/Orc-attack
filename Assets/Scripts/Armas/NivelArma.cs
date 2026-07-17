using UnityEngine;

/// <summary>
/// Define los niveles de rareza de las armas y sus propiedades.
/// 1 Gris, 2 Verde, 3 Azul, 4 Morado, 5 Rojo, 6 Naranja.
/// </summary>
public static class NivelArma
{
    public const int NIVEL_MIN = 1;
    public const int NIVEL_MAX = 6;

    /// <summary>Colores vivos para iconos / sprites del arma en el mundo.</summary>
    private static readonly Color[] colores =
    {
        Color.white,                        // 0 - no usado
        new Color(0.72f, 0.72f, 0.74f),     // 1 - Gris
        new Color(0.25f, 0.85f, 0.35f),     // 2 - Verde
        new Color(0.30f, 0.55f, 1.00f),     // 3 - Azul
        new Color(0.75f, 0.30f, 0.95f),     // 4 - Morado
        new Color(0.95f, 0.25f, 0.25f),     // 5 - Rojo
        new Color(1.00f, 0.55f, 0.10f),     // 6 - Naranja
    };

    /// <summary>Colores de fondo del cuadrado del inventario (más suaves).</summary>
    private static readonly Color[] coloresFondo =
    {
        Color.white,                        // 0
        new Color(0.35f, 0.35f, 0.38f, 1f), // 1 - Gris (fondo actual)
        new Color(0.18f, 0.42f, 0.22f, 1f), // 2 - Verde
        new Color(0.16f, 0.28f, 0.55f, 1f), // 3 - Azul
        new Color(0.38f, 0.18f, 0.52f, 1f), // 4 - Morado
        new Color(0.52f, 0.16f, 0.16f, 1f), // 5 - Rojo
        new Color(0.55f, 0.32f, 0.10f, 1f), // 6 - Naranja
    };

    private static readonly string[] nombres =
    {
        "",             // 0
        "Común",        // 1
        "Poco Común",   // 2
        "Raro",         // 3
        "Épico",        // 4
        "Élite",        // 5
        "Legendario"    // 6
    };

    private static readonly float[] multiplicadores =
    {
        1.0f,   // 0
        1.0f,   // 1 - Gris
        1.25f,  // 2 - Verde
        1.55f,  // 3 - Azul
        1.90f,  // 4 - Morado
        2.30f,  // 5 - Rojo
        2.80f,  // 6 - Naranja
    };

    public static Color ObtenerColor(int nivel)
    {
        return colores[Mathf.Clamp(nivel, NIVEL_MIN, NIVEL_MAX)];
    }

    public static Color ObtenerColorFondo(int nivel)
    {
        return coloresFondo[Mathf.Clamp(nivel, NIVEL_MIN, NIVEL_MAX)];
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
