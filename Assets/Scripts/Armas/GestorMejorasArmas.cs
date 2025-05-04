using UnityEngine;

/// <summary>
/// Gestor centralizado de mejoras para todas las armas del juego.
/// </summary>
/// <remarks>
/// Esta clase implementa el patrón Singleton para proporcionar un punto 
/// de acceso global a las mejoras persistentes de las armas. Almacena
/// y gestiona todas las mejoras adquiridas que afectan a las armas.
/// </remarks>
public class GestorMejorasArmas : MonoBehaviour
{
    /// <summary>
    /// Referencia estática a la instancia única de este gestor.
    /// </summary>
    public static GestorMejorasArmas instancia;

    /// <summary>
    /// Porcentaje de aumento de daño acumulado para todas las armas.
    /// </summary>
    private int aumentoDanioPorcentaje = 0;
    
    /// <summary>
    /// Puntos porcentuales de aumento en la probabilidad de golpe crítico para todas las armas.
    /// </summary>
    private int aumentoProbabilidadCritico = 0;
    
    /// <summary>
    /// Puntos porcentuales de aumento en la probabilidad de robar vida para todas las armas.
    /// </summary>
    private int aumentoProbabilidadRobarVida = 0;
    
    /// <summary>
    /// Porcentaje de disminución en el tiempo de recarga para todas las armas.
    /// </summary>
    private int disminucionRecargaPorcentaje = 0;

    /// <summary>
    /// Inicializa el singleton y asegura que solo exista una instancia de este gestor.
    /// </summary>
    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Aumenta el porcentaje de daño para todas las armas.
    /// </summary>
    /// <param name="porcentaje">Porcentaje de aumento a añadir.</param>
    public void AumentarDanioPorcentaje(int porcentaje)
    {
        aumentoDanioPorcentaje += porcentaje;
    }

    /// <summary>
    /// Aumenta la probabilidad de golpe crítico para todas las armas.
    /// </summary>
    /// <param name="cantidad">Puntos porcentuales a añadir.</param>
    public void AumentarProbabilidadCritico(int cantidad)
    {
        aumentoProbabilidadCritico += cantidad;
    }

    /// <summary>
    /// Aumenta la probabilidad de robar vida para todas las armas.
    /// </summary>
    /// <param name="cantidad">Puntos porcentuales a añadir.</param>
    public void AumentarProbabilidadRobarVida(int cantidad)
    {
        aumentoProbabilidadRobarVida += cantidad;
    }

    /// <summary>
    /// Disminuye el porcentaje de tiempo de recarga para todas las armas.
    /// </summary>
    /// <param name="porcentaje">Porcentaje de disminución a añadir.</param>
    public void DisminuirRecargaPorcentaje(int porcentaje)
    {
        disminucionRecargaPorcentaje += porcentaje;
    }

    /// <summary>
    /// Obtiene el porcentaje acumulado de aumento de daño.
    /// </summary>
    /// <returns>Porcentaje total de aumento de daño.</returns>
    public int ObtenerAumentoDanioPorcentaje()
    {
        return aumentoDanioPorcentaje;
    }

    /// <summary>
    /// Obtiene los puntos porcentuales acumulados de aumento de probabilidad de crítico.
    /// </summary>
    /// <returns>Puntos porcentuales totales de aumento de probabilidad de crítico.</returns>
    public int ObtenerAumentoProbabilidadCritico()
    {
        return aumentoProbabilidadCritico;
    }

    /// <summary>
    /// Obtiene los puntos porcentuales acumulados de aumento de probabilidad de robar vida.
    /// </summary>
    /// <returns>Puntos porcentuales totales de aumento de probabilidad de robar vida.</returns>
    public int ObtenerAumentoProbabilidadRobarVida()
    {
        return aumentoProbabilidadRobarVida;
    }

    /// <summary>
    /// Obtiene el porcentaje acumulado de disminución de tiempo de recarga.
    /// </summary>
    /// <returns>Porcentaje total de disminución del tiempo de recarga.</returns>
    public int ObtenerDisminucionRecargaPorcentaje()
    {
        return disminucionRecargaPorcentaje;
    }
} 