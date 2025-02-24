using UnityEngine;

public class GestorHabilidades : MonoBehaviour
{
    public int aumentarVidaMaxima = 0; //Se debe ir añadiendo la cantidad (int) de vida que quieres añadir cada vez
    public float restarSegundosRecuperar1Vida = 0f; //Se debe añadir la cantidad segundos (float) que quieres restarle al tiempo de recuperar vida {Se resta al ultimo valor, no al valor inicial}
    public int aumentarProbabilidadRobarVida = 0; //Se añade directamente 
    public int aumentarDanioPorPorcentaje = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    public int aumentarDanioPorPocentajeMelee = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    public int aumentarDanioPorPocentajeDistancia = 0; //Se añade directamente el procentaje (int) que quieres aumentar del valor inicial
    public int disminuirRecargaPorPocentaje = 0; //Se añade directamente el procentaje (int) que quieres disminuir del valor inicial


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
        if (armasDistancia != null && armasMelee != null) 
        {
            armasDistancia.AumentarProbabilidadRobarVida(aumentarProbabilidadRobarVida);
            armasMelee.AumentarProbabilidadRobarVida(aumentarProbabilidadRobarVida);
        } 
    }

    private void FuncionAumentarDanioPorPorcentaje() 
    {
        if (armasDistancia != null && armasMelee != null) 
        {
            armasDistancia.AumentarDanioPorPocentaje(aumentarDanioPorPorcentaje);
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
        if (armasDistancia != null && armasMelee != null) 
        {
            armasDistancia.DisminuirRecargaPorPocentaje(disminuirRecargaPorPocentaje);
            armasMelee.DisminuirRecargaPorPocentaje(disminuirRecargaPorPocentaje);
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
