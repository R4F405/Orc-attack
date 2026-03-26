using UnityEngine;

public enum TipoArma
{
    Melee,
    Distancia
}

/// <summary>
/// ScriptableObject que define los datos base de un tipo de arma.
/// Crear desde Assets > Create > Armas > Datos de Arma.
/// </summary>
[CreateAssetMenu(fileName = "NuevaArma", menuName = "Armas/Datos de Arma")]
public class DatosArma : ScriptableObject
{
    [Header("Identidad")]
    public string nombre;
    public Sprite icono;
    public GameObject prefab;
    public TipoArma tipo;

    [Header("Estadísticas Base (Nivel 1)")]
    public int danioBase = 1;
    public float recargaBase = 1f;
    public int probabilidadCritico = 0;
    public int probabilidadRobarVida = 0;

    /// <summary>
    /// Calcula el daño para un nivel dado (escalado por multiplicador de nivel).
    /// </summary>
    public int ObtenerDanio(int nivel)
    {
        return Mathf.RoundToInt(danioBase * NivelArma.ObtenerMultiplicador(nivel));
    }

    /// <summary>
    /// Calcula la recarga para un nivel dado (menor recarga a mayor nivel).
    /// </summary>
    public float ObtenerRecarga(int nivel)
    {
        float recarga = recargaBase / NivelArma.ObtenerMultiplicador(nivel);
        return Mathf.Max(0.1f, Mathf.Round(recarga * 100f) / 100f);
    }

    /// <summary>
    /// Calcula la probabilidad de crítico para un nivel dado (+5% por nivel).
    /// </summary>
    public int ObtenerCritico(int nivel)
    {
        return probabilidadCritico + (nivel - 1) * 5;
    }

    /// <summary>
    /// Calcula la probabilidad de robo de vida para un nivel dado (+3% por nivel).
    /// </summary>
    public int ObtenerRoboVida(int nivel)
    {
        return probabilidadRobarVida + (nivel - 1) * 3;
    }

    /// <summary>
    /// Calcula el precio del arma basado en sus stats y nivel.
    /// </summary>
    public int ObtenerPrecio(int nivel)
    {
        float mult = NivelArma.ObtenerMultiplicador(nivel);
        float precioBase;

        if (tipo == TipoArma.Melee)
        {
            precioBase = 5 + (danioBase * 1.5f) + (probabilidadCritico * 0.5f)
                        + (probabilidadRobarVida * 0.8f) + (1f / recargaBase * 3f);
        }
        else
        {
            precioBase = 10 + (danioBase * 2f) + (probabilidadCritico * 0.7f)
                        + (probabilidadRobarVida * 1f) + (1f / recargaBase * 4f);
        }

        return Mathf.Max(5, Mathf.RoundToInt(precioBase * mult));
    }
}
