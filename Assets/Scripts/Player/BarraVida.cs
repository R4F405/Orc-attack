using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraVida2 : MonoBehaviour
{
    public Slider barraVida;
    public TextMeshProUGUI textoVida; // Texto donde se mostrará la vida

    private int vidaActual = 0;
    private int vidaMaxima = 100;
    private VidaJugador vidaJugador;

    private void Start()
    {
        GameObject jugador = null;
        jugador = GameObject.FindWithTag("Jugador");

        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }
    }

    private void Update()
    {

        // Si no se ha encontrado el jugador en Start(), intentar encontrarlo en Update()
        if (vidaJugador == null)
        {
            ObtenerJugador();
        }

        vidaActual = vidaJugador.ObtenerSalud();
        vidaMaxima = vidaJugador.ObtenerSaludMaxima();

        barraVida.value = (float)vidaActual / vidaMaxima;
        textoVida.text = vidaActual.ToString() + " / " + vidaMaxima.ToString();

    }

    private void ObtenerJugador()
    {
        // Intentar obtener el jugador con el tag 'Jugador'
        GameObject jugador = GameObject.FindWithTag("Jugador");

        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }
    }
}
