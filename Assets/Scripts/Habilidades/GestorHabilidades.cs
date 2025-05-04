using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Gestiona todas las habilidades y mejoras del jugador en el juego.
/// </summary>
/// <remarks>
/// Esta clase controla la aplicación de diferentes tipos de mejoras que afectan
/// al jugador y sus armas, como aumentos de vida, daño, probabilidades de efectos
/// especiales y tiempos de recarga. Actúa como el punto central para todas las
/// mejoras del personaje obtenidas mediante subida de nivel o compras.
/// </remarks>
public class GestorHabilidades : MonoBehaviour
{
    //VARIABLES DE LAS HABILIDADES
    /// <summary>
    /// Tiempo en segundos que se resta a la recuperación de vida del jugador.
    /// </summary>
    /// <remarks>
    /// Se resta al último valor calculado, no al valor inicial.
    /// </remarks>
    private float restarSegundosRecuperar1Vida = 0f; //Se debe añadir la cantidad segundos (float) que quieres restarle al tiempo de recuperar vida {Se resta al ultimo valor, no al valor inicial}
    
    /// <summary>
    /// Porcentaje de aumento en la probabilidad de robar vida al dañar enemigos.
    /// </summary>
    /// <remarks>
    /// Se añade directamente como valor numérico para aumentar la probabilidad.
    /// </remarks>
    private int aumentarProbabilidadRobarVida = 0; //Se añade directamente el numero para aumentar la probabilidad 
    
    /// <summary>
    /// Porcentaje de aumento del daño para todas las armas.
    /// </summary>
    /// <remarks>
    /// Se añade directamente como porcentaje al valor inicial de daño.
    /// </remarks>
    private int aumentarDanioPorPorcentaje = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    
    /// <summary>
    /// Porcentaje de aumento del daño específico para armas cuerpo a cuerpo.
    /// </summary>
    /// <remarks>
    /// Se añade directamente como porcentaje al valor inicial de daño melee.
    /// </remarks>
    private int aumentarDanioPorPocentajeMelee = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    
    /// <summary>
    /// Porcentaje de aumento del daño específico para armas a distancia.
    /// </summary>
    /// <remarks>
    /// Se añade directamente como porcentaje al valor inicial de daño a distancia.
    /// </remarks>
    private int aumentarDanioPorPocentajeDistancia = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    
    /// <summary>
    /// Porcentaje de disminución del tiempo de recarga de todas las armas.
    /// </summary>
    /// <remarks>
    /// Se añade directamente como porcentaje a restar del tiempo de recarga inicial.
    /// </remarks>
    private int disminuirRecargaPorPocentaje = 0; //Se añade directamente el procentaje (int) que quieres disminuir del valor inicial
    
    /// <summary>
    /// Porcentaje de aumento en la probabilidad de realizar golpes críticos.
    /// </summary>
    /// <remarks>
    /// Se añade directamente como valor numérico para aumentar la probabilidad.
    /// </remarks>
    private int aumentarProbabilidadCritico = 0; //Se añade directamente el numero para aumentar la probabilidad 
    
    //private int aumentarDanioEstructuras = 0; // {{{{{{{Falta por crear}}}}}}}
    //private int aumentarDanioCompañero = 0; // {{{{{{{Falta por crear}}}}}}}
    
    /// <summary>
    /// Tiempo en segundos que se resta a la generación de cajas de recursos.
    /// </summary>
    /// <remarks>
    /// Se resta al último valor calculado, no al valor inicial.
    /// </remarks>
    private float restarSegundosGenerarCajas = 0f; //Se añade los segundos que se quiera restar del tiempo de generacion de cajas {Se resta al ultimo valor, no al valor inicial}
    
    /// <summary>
    /// Factor multiplicador para las calaveras obtenidas como recurso.
    /// </summary>
    /// <remarks>
    /// Valor 0 significa sin multiplicador, 2 significa x2, 3 significa x3, etc.
    /// </remarks>
    private int multiplicadorCalaveras = 0; // Se añade 2 o 3 segun el multiplicador que se quiera

    

    /// <summary>
    /// Referencia al componente que gestiona las armas cuerpo a cuerpo.
    /// </summary>
    private ArmasMelee armasMelee;
    
    /// <summary>
    /// Referencia al componente que gestiona las armas a distancia.
    /// </summary>
    private ArmasDistancia armasDistancia;
    
    /// <summary>
    /// Referencia al inventario del jugador para gestionar recursos.
    /// </summary>
    private InventarioJugador inventarioJugador;
    
    /// <summary>
    /// Referencia al generador de cajas de recursos en el juego.
    /// </summary>
    private GeneradorCajas generadorCajas;
    
    /// <summary>
    /// Referencia al componente que gestiona la vida del jugador.
    /// </summary>
    private VidaJugador vidaJugador;
    
    /// <summary>
    /// Inicializa las referencias necesarias y actualiza los textos al inicio.
    /// </summary>
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

    /// <summary>
    /// Actualiza los textos informativos de las habilidades en la interfaz de usuario.
    /// </summary>
    public void ActualizarTextos()
    {
        // Buscar el GestorTextosHabilidades y actualizar los textos
        GestorTextosHabilidades gestorTextos = FindAnyObjectByType<GestorTextosHabilidades>();
        if (gestorTextos != null)
        {
            gestorTextos.ActualizarTextos();
        }
    }

    /// <summary>
    /// Aplica todas las habilidades acumuladas al jugador y sus armas.
    /// </summary>
    /// <remarks>
    /// Se debe llamar cuando se quiera actualizar todas las mejoras, por ejemplo, después de comprar en la tienda.
    /// </remarks>
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

    /// <summary>
    /// Aplica una habilidad específica según su ID.
    /// </summary>
    /// <param name="id">ID único de la habilidad a aplicar.</param>
    /// <remarks>
    /// Cada ID corresponde a un tipo diferente de mejora con valores predefinidos.
    /// Esta función es llamada principalmente desde el sistema de selección de mejoras al subir de nivel.
    /// </remarks>
    public void AplicarHabilidadPorID(int id) //Falta añadir cuanto mejorara caada una
    {
        switch (id)
        {
            case 1:
                FuncionAumentarVida(); // Siempre +10
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

    /// <summary>
    /// Aumenta la salud máxima del jugador y lo cura completamente.
    /// </summary>
    private void FuncionAumentarVida() 
    {
        if (vidaJugador != null)
        {
            vidaJugador.AumentarSaludMaxima(10);
            vidaJugador.Curar(vidaJugador.ObtenerSaludMaxima());
        } 
    }

    /// <summary>
    /// Reduce el tiempo entre recuperaciones de vida del jugador.
    /// </summary>
    private void FuncionrestarSegundosRecuperar1Vida()
    {
        if (vidaJugador != null)
        {
            vidaJugador.DisminuirTiempoEntreRecuperaciones(restarSegundosRecuperar1Vida);
        } 
    }

    /// <summary>
    /// Aumenta la probabilidad de robar vida al dañar enemigos para todas las armas.
    /// </summary>
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

    /// <summary>
    /// Aumenta porcentualmente el daño de todas las armas.
    /// </summary>
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

    /// <summary>
    /// Aumenta porcentualmente el daño de las armas cuerpo a cuerpo.
    /// </summary>
    private void FuncionAumentarDanioPorPocentajeMelee() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasMelee != null) 
        {
            armasMelee.AumentarDanioPorPocentaje(aumentarDanioPorPocentajeMelee);
        }
    }

    /// <summary>
    /// Aumenta porcentualmente el daño de las armas a distancia.
    /// </summary>
    private void FuncionAumentarDanioPorPocentajeDistancia() 
    {
        // Asegurarnos de encontrar las armas si no existen
        BuscarArmas();
        
        if (armasDistancia != null) 
        {
            armasDistancia.AumentarDanioPorPocentaje(aumentarDanioPorPocentajeDistancia);
        }
    }

    /// <summary>
    /// Disminuye porcentualmente el tiempo de recarga de todas las armas.
    /// </summary>
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

    /// <summary>
    /// Aumenta la probabilidad de golpes críticos para todas las armas.
    /// </summary>
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

    /// <summary>
    /// Reduce el tiempo entre generaciones de cajas de recursos.
    /// </summary>
    private void FuncionRestarSegundosGenerarCajas() 
    {
        if (generadorCajas != null)
        {
            generadorCajas.DismininuirTiempo(restarSegundosGenerarCajas);
        }
    }

    /// <summary>
    /// Aplica el multiplicador de calaveras recibidas.
    /// </summary>
    private void FuncionMultiplicadorCalaveras() 
    {
        if (inventarioJugador != null) 
        {
            inventarioJugador.MultiplicadorCalaveras(multiplicadorCalaveras);
        }
    }

    /// <summary>
    /// Actualiza el GestorMejorasArmas si existe en la escena.
    /// </summary>
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

    /// <summary>
    /// Busca las referencias a los componentes de armas si no están asignadas.
    /// </summary>
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
    /// <summary>
    /// Obtiene la vida máxima actual del jugador.
    /// </summary>
    /// <returns>Valor de salud máxima.</returns>
    public int ObtenerVidaMaxima()
    {
        if (vidaJugador != null)
        {
            return vidaJugador.ObtenerSaludMaxima();
        }
        return 0;
    }

    // Método para obtener el aumento de probabilidad de robo de vida
    /// <summary>
    /// Obtiene el aumento actual en la probabilidad de robo de vida.
    /// </summary>
    /// <returns>Porcentaje de aumento en la probabilidad de robo de vida.</returns>
    public int ObtenerAumentoProbabilidadRoboVida()
    {
        return aumentarProbabilidadRobarVida;
    }

    // Método para obtener el aumento de daño general por porcentaje
    /// <summary>
    /// Obtiene el aumento porcentual de daño general.
    /// </summary>
    /// <returns>Porcentaje de aumento en el daño general.</returns>
    public int ObtenerAumentoDanioPorcentaje()
    {
        return aumentarDanioPorPorcentaje;
    }

    // Método para obtener el aumento de daño melee por porcentaje
    /// <summary>
    /// Obtiene el aumento porcentual de daño para armas cuerpo a cuerpo.
    /// </summary>
    /// <returns>Porcentaje de aumento en el daño cuerpo a cuerpo.</returns>
    public int ObtenerAumentoDanioMeleePorcentaje()
    {
        return aumentarDanioPorPocentajeMelee;
    }

    // Método para obtener el aumento de daño a distancia por porcentaje
    /// <summary>
    /// Obtiene el aumento porcentual de daño para armas a distancia.
    /// </summary>
    /// <returns>Porcentaje de aumento en el daño a distancia.</returns>
    public int ObtenerAumentoDanioDistanciaPorcentaje()
    {
        return aumentarDanioPorPocentajeDistancia;
    }

    // Método para obtener la disminución de recarga por porcentaje
    /// <summary>
    /// Obtiene la disminución porcentual en los tiempos de recarga.
    /// </summary>
    /// <returns>Porcentaje de disminución en la recarga.</returns>
    public int ObtenerDisminucionRecargaPorcentaje()
    {
        return disminuirRecargaPorPocentaje;
    }

    // Método para obtener el aumento de probabilidad de crítico
    /// <summary>
    /// Obtiene el aumento en la probabilidad de golpes críticos.
    /// </summary>
    /// <returns>Porcentaje de aumento en la probabilidad de crítico.</returns>
    public int ObtenerAumentoProbabilidadCritico()
    {
        return aumentarProbabilidadCritico;
    }

    // Método para obtener la disminución del tiempo de generación de cajas
    /// <summary>
    /// Obtiene la disminución en el tiempo de generación de cajas.
    /// </summary>
    /// <returns>Segundos de disminución en el tiempo de generación.</returns>
    public float ObtenerDisminucionTiempoGeneracionCajas()
    {
        return restarSegundosGenerarCajas;
    }

    // Método para obtener el multiplicador de calaveras
    /// <summary>
    /// Obtiene el multiplicador actual de calaveras.
    /// </summary>
    /// <returns>Valor del multiplicador de calaveras (0 = sin multiplicador, 2 = x2, 3 = x3).</returns>
    public int ObtenerMultiplicadorCalaveras()
    {
        return multiplicadorCalaveras;
    }

    // Método para verificar si ya se compró el multiplicador de calaveras
    /// <summary>
    /// Verifica si el jugador tiene activo el multiplicador de calaveras.
    /// </summary>
    /// <returns>True si el multiplicador está activo, false en caso contrario.</returns>
    public bool TieneMultiplicadorCalaveras()
    {
        return multiplicadorCalaveras > 0;
    }
}
