using UnityEngine;

public class GestorHabilidades : MonoBehaviour
{
    public int aumentarVidaMaxima = 0; //Se debe ir añadiendo la cantidad (int) de vida que quieres añadir cada vez
    public float restarSegundosRecuperar1Vida = 0f; //Se debe añadir la cantidad segundos (float) que quieres restarle al tiempo de recuperar vida {Se resta al ultimo valor, no al valor inicial}
    public int aumentarProbabilidadRobarVida = 0; //Se añade directamente el numero para aumentar la probabilidad 
    public int aumentarDanioPorPorcentaje = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    public int aumentarDanioPorPocentajeMelee = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    public int aumentarDanioPorPocentajeDistancia = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    public int disminuirRecargaPorPocentaje = 0; //Se añade directamente el procentaje (int) que quieres disminuir del valor inicial
    public int aumentarProbabilidadCritico = 0; //Se añade directamente el numero para aumentar la probabilidad 
    public int aumentarDanioEstructuras = 0; // {{{{{{{Falta por crear}}}}}}}
    public int aumentarDanioCompañero = 0; // {{{{{{{Falta por crear}}}}}}}


    public float restarSegundosGenerarCajas = 0f; //Se añade los segundos que se quiera restar del tiempo de generacion de cajas {Se resta al ultimo valor, no al valor inicial}
    public int multiplicadorCalaveras = 0; // Se añade 2 o 3 segun el multiplicador que se quiera

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

        //Resetear variables
        aumentarVidaMaxima = 0;
        restarSegundosRecuperar1Vida = 0f;
        aumentarProbabilidadRobarVida = 0;
        aumentarDanioPorPorcentaje = 0;
        aumentarDanioPorPocentajeMelee = 0;
        aumentarDanioPorPocentajeDistancia = 0;
        disminuirRecargaPorPocentaje = 0;
        aumentarProbabilidadCritico = 0;






        
        restarSegundosGenerarCajas = 0f;
        multiplicadorCalaveras = 0;
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
        if (armasMelee != null) 
        {
            armasMelee.AumentarDanioPorPocentaje(aumentarDanioPorPocentajeMelee);
        }
    }

    private void FuncionAumentarDanioPorPocentajeDistancia() 
    {
        if (armasDistancia != null) 
        {
            armasDistancia.AumentarDanioPorPocentaje(aumentarDanioPorPocentajeDistancia);
        }
    }

    private void FuncionDisminuirRecargaPorPocentaje() 
    {
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
        if ( armasMelee != null) 
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


}
