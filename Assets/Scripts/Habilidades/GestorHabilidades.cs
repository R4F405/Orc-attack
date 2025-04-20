using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GestorHabilidades : MonoBehaviour
{
    //VARIABLES DE LAS HABILIDADES
    private int aumentarVidaMaxima = 0; //Se debe ir añadiendo la cantidad (int) de vida que quieres añadir cada vez
    private float restarSegundosRecuperar1Vida = 0f; //Se debe añadir la cantidad segundos (float) que quieres restarle al tiempo de recuperar vida {Se resta al ultimo valor, no al valor inicial}
    private int aumentarProbabilidadRobarVida = 0; //Se añade directamente el numero para aumentar la probabilidad 
    private int aumentarDanioPorPorcentaje = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    private int aumentarDanioPorPocentajeMelee = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    private int aumentarDanioPorPocentajeDistancia = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    private int disminuirRecargaPorPocentaje = 0; //Se añade directamente el procentaje (int) que quieres disminuir del valor inicial
    private int aumentarProbabilidadCritico = 0; //Se añade directamente el numero para aumentar la probabilidad 
    //private int aumentarDanioEstructuras = 0; // {{{{{{{Falta por crear}}}}}}}
    //private int aumentarDanioCompañero = 0; // {{{{{{{Falta por crear}}}}}}}
    private float restarSegundosGenerarCajas = 0f; //Se añade los segundos que se quiera restar del tiempo de generacion de cajas {Se resta al ultimo valor, no al valor inicial}
    private int multiplicadorCalaveras = 0; // Se añade 2 o 3 segun el multiplicador que se quiera

    

    private ArmasMelee armasMelee;
    private ArmasDistancia armasDistancia;
    private InventarioJugador inventarioJugador;
    private GeneradorCajas generadorCajas;
    
    private VidaJugador vidaJugador;
    
    private void Start()
    {
        vidaJugador = GetComponent<VidaJugador>();
        armasMelee = FindAnyObjectByType<ArmasMelee>();
        armasDistancia = FindAnyObjectByType<ArmasDistancia>();
        inventarioJugador = GetComponent<InventarioJugador>();
        generadorCajas = FindAnyObjectByType<GeneradorCajas>();
        
        // Actualizar los textos al inicio
        ActualizarTextos();
    }

    public void ActualizarTextos()
    {
        // Buscar el GestorTextosHabilidades y actualizar los textos
        GestorTextosHabilidades gestorTextos = FindAnyObjectByType<GestorTextosHabilidades>();
        if (gestorTextos != null)
        {
            gestorTextos.ActualizarTextos();
        }
    }

    public void AplicarHabilidades() //Se debe llamar posteriormente en la tienda
    {
        FuncionAumentarVida();
        FuncionrestarSegundosRecuperar1Vida();
        FuncionAumentarProbabilidadRobarVida();
        FuncionAumentarDanioPorPorcentaje();
        FuncionAumentarDanioPorPocentajeMelee();
        FuncionAumentarDanioPorPocentajeDistancia();
        FuncionDisminuirRecargaPorPocentaje();
        FuncionAumentarProbabilidadCritico();
        FuncionRestarSegundosGenerarCajas();
        FuncionMultiplicadorCalaveras();
        
        // Actualizar los textos después de aplicar habilidades
        ActualizarTextos();
    }

    public void AplicarHabilidadPorID(int id) //Falta añadir cuanto mejorara caada una
    {
        switch (id)
        {
            case 1:
                aumentarVidaMaxima += 10;
                FuncionAumentarVida();
                Debug.Log("Aplicada habilidad: Aumentar Vida");
                break;
            case 2:
                restarSegundosRecuperar1Vida += 0.5f;
                FuncionrestarSegundosRecuperar1Vida();
                Debug.Log("Aplicada habilidad: Reducir Tiempo Recuperación Vida");
                break;
            case 3:
                aumentarProbabilidadRobarVida += 5;
                FuncionAumentarProbabilidadRobarVida();
                Debug.Log("Aplicada habilidad: Aumentar Probabilidad Robar Vida");
                break;
            case 4:
                aumentarDanioPorPorcentaje += 10; // Acumulamos 10% cada vez
                FuncionAumentarDanioPorPorcentaje();
                Debug.Log("Aplicada habilidad: Aumentar Daño Por Porcentaje");
                break;
            case 5:
                aumentarDanioPorPocentajeMelee += 15; // Acumulamos 15% cada vez
                FuncionAumentarDanioPorPocentajeMelee();
                Debug.Log("Aplicada habilidad: Aumentar Daño Melee");
                break;
            case 6:
                aumentarDanioPorPocentajeDistancia += 15; // Acumulamos 15% cada vez
                FuncionAumentarDanioPorPocentajeDistancia();
                Debug.Log("Aplicada habilidad: Aumentar Daño Distancia");
                break;
            case 7:
                disminuirRecargaPorPocentaje += 3; // Acumulamos 1% cada vez
                FuncionDisminuirRecargaPorPocentaje();
                Debug.Log("Aplicada habilidad: Reducir Recarga");
                break;
            case 8:
                aumentarProbabilidadCritico += 5; // Acumulamos 5% cada vez
                FuncionAumentarProbabilidadCritico();
                Debug.Log("Aplicada habilidad: Aumentar Probabilidad Crítico");
                break;
            case 9:
                restarSegundosGenerarCajas += 0.5f; // Acumulamos 0.5s cada vez
                FuncionRestarSegundosGenerarCajas();
                Debug.Log("Aplicada habilidad: Reducir Tiempo Generación Cajas");
                break;
            case 10:
                // Verificar si ya tiene el multiplicador de calaveras
                if (multiplicadorCalaveras == 0)
                {
                    multiplicadorCalaveras = 2; // Multiplicador x2 (esto no se acumula, se mantiene igual)
                    FuncionMultiplicadorCalaveras();
                    Debug.Log("Aplicada habilidad: Multiplicador Calaveras");
                }
                else
                {
                    Debug.LogWarning("Ya has adquirido el multiplicador de calaveras");
                }
                break;
            default:
                Debug.LogWarning($"ID de habilidad no reconocido: {id}");
                break;
        }
        
        // También actualizar el GestorMejorasArmas si existe
        ActualizarGestorMejorasArmas();
        
        // Actualizar los textos después de aplicar una habilidad
        ActualizarTextos();
    }

    private void FuncionAumentarVida() 
    {
        if (vidaJugador != null)
        {
            vidaJugador.AumentarSaludMaxima(aumentarVidaMaxima);
            vidaJugador.Curar(vidaJugador.ObtenerSaludMaxima());
        } 
    }

    private void FuncionrestarSegundosRecuperar1Vida()
    {
        if (vidaJugador != null)
        {
            vidaJugador.DisminuirTiempoEntreRecuperaciones(restarSegundosRecuperar1Vida);
        } 
    }

    private void FuncionAumentarProbabilidadRobarVida() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasDistancia != null ) 
        {
            armasDistancia.AumentarProbabilidadRobarVida(aumentarProbabilidadRobarVida);
        } 
        if (armasMelee != null)
        {
            armasMelee.AumentarProbabilidadRobarVida(aumentarProbabilidadRobarVida);
        }
    }

    private void FuncionAumentarDanioPorPorcentaje() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasDistancia != null ) 
        {
            armasDistancia.AumentarDanioPorPocentaje(aumentarDanioPorPorcentaje);
        }
        if (armasMelee != null) 
        {
            armasMelee.AumentarDanioPorPocentaje(aumentarDanioPorPorcentaje);
        }
    }

    private void FuncionAumentarDanioPorPocentajeMelee() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasMelee != null) 
        {
            armasMelee.AumentarDanioPorPocentaje(aumentarDanioPorPocentajeMelee);
        }
    }

    private void FuncionAumentarDanioPorPocentajeDistancia() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasDistancia != null) 
        {
            armasDistancia.AumentarDanioPorPocentaje(aumentarDanioPorPocentajeDistancia);
        }
    }

    private void FuncionDisminuirRecargaPorPocentaje() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasDistancia != null ) 
        {
            armasDistancia.DisminuirRecargaPorPocentaje(disminuirRecargaPorPocentaje);
        }
        if (armasMelee != null)
        {
            armasMelee.DisminuirRecargaPorPocentaje(disminuirRecargaPorPocentaje);
        }
    }

    private void FuncionAumentarProbabilidadCritico() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasMelee != null) 
        {
            armasMelee.AumentarProbabilidadCritico(aumentarProbabilidadCritico);
        } 
        if (armasDistancia != null) 
        {
            armasDistancia.AumentarProbabilidadCritico(aumentarProbabilidadCritico);
        }
    }

    private void FuncionRestarSegundosGenerarCajas() 
    {
        if (generadorCajas != null)
        {
            generadorCajas.DismininuirTiempo(restarSegundosGenerarCajas);
        }
    }

    private void FuncionMultiplicadorCalaveras() 
    {
        if (inventarioJugador != null) 
        {
            inventarioJugador.MultiplicadorCalaveras(multiplicadorCalaveras);
        }
    }

    private void ActualizarGestorMejorasArmas()
    {
        GestorMejorasArmas gestorMejoras = FindAnyObjectByType<GestorMejorasArmas>();
        if (gestorMejoras != null)
        {
            // Actualizar el gestor con los valores acumulados
            gestorMejoras.AumentarDanioPorcentaje(aumentarDanioPorPorcentaje);
            gestorMejoras.AumentarProbabilidadCritico(aumentarProbabilidadCritico);
            gestorMejoras.AumentarProbabilidadRobarVida(aumentarProbabilidadRobarVida);
            gestorMejoras.DisminuirRecargaPorcentaje(disminuirRecargaPorPocentaje);
        }
    }

    private void BuscarArmas()
    {
        if (armasMelee == null)
        {
            armasMelee = FindAnyObjectByType<ArmasMelee>();
        }
        
        if (armasDistancia == null)
        {
            armasDistancia = FindAnyObjectByType<ArmasDistancia>();
        }
    }

    // Métodos para obtener los valores actuales
    public int ObtenerVidaMaxima()
    {
        if (vidaJugador != null)
        {
            return vidaJugador.ObtenerSaludMaxima();
        }
        return 0;
    }

    // Método para obtener el aumento de probabilidad de robo de vida
    public int ObtenerAumentoProbabilidadRoboVida()
    {
        return aumentarProbabilidadRobarVida;
    }

    // Método para obtener el aumento de daño general por porcentaje
    public int ObtenerAumentoDanioPorcentaje()
    {
        return aumentarDanioPorPorcentaje;
    }

    // Método para obtener el aumento de daño melee por porcentaje
    public int ObtenerAumentoDanioMeleePorcentaje()
    {
        return aumentarDanioPorPocentajeMelee;
    }

    // Método para obtener el aumento de daño a distancia por porcentaje
    public int ObtenerAumentoDanioDistanciaPorcentaje()
    {
        return aumentarDanioPorPocentajeDistancia;
    }

    // Método para obtener la disminución de recarga por porcentaje
    public int ObtenerDisminucionRecargaPorcentaje()
    {
        return disminuirRecargaPorPocentaje;
    }

    // Método para obtener el aumento de probabilidad de crítico
    public int ObtenerAumentoProbabilidadCritico()
    {
        return aumentarProbabilidadCritico;
    }

    // Método para obtener la disminución del tiempo de generación de cajas
    public float ObtenerDisminucionTiempoGeneracionCajas()
    {
        return restarSegundosGenerarCajas;
    }

    // Método para obtener el multiplicador de calaveras
    public int ObtenerMultiplicadorCalaveras()
    {
        return multiplicadorCalaveras;
    }

    // Método para verificar si ya se compró el multiplicador de calaveras
    public bool TieneMultiplicadorCalaveras()
    {
        return multiplicadorCalaveras > 0;
    }
}
