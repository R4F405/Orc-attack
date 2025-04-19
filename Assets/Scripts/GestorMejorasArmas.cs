using UnityEngine;

public class GestorMejorasArmas : MonoBehaviour
{
    public static GestorMejorasArmas instancia;

    private int aumentoDanioPorcentaje = 0;
    private int aumentoProbabilidadCritico = 0;
    private int aumentoProbabilidadRobarVida = 0;
    private int disminucionRecargaPorcentaje = 0;

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

    public void AumentarDanioPorcentaje(int porcentaje)
    {
        aumentoDanioPorcentaje += porcentaje;
    }

    public void AumentarProbabilidadCritico(int cantidad)
    {
        aumentoProbabilidadCritico += cantidad;
    }

    public void AumentarProbabilidadRobarVida(int cantidad)
    {
        aumentoProbabilidadRobarVida += cantidad;
    }

    public void DisminuirRecargaPorcentaje(int porcentaje)
    {
        disminucionRecargaPorcentaje += porcentaje;
    }

    public int ObtenerAumentoDanioPorcentaje()
    {
        return aumentoDanioPorcentaje;
    }

    public int ObtenerAumentoProbabilidadCritico()
    {
        return aumentoProbabilidadCritico;
    }

    public int ObtenerAumentoProbabilidadRobarVida()
    {
        return aumentoProbabilidadRobarVida;
    }

    public int ObtenerDisminucionRecargaPorcentaje()
    {
        return disminucionRecargaPorcentaje;
    }
} 