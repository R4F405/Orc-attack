using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BarraVida : MonoBehaviour
{
    public GameObject prefabCorazon; // Prefab de corazón asignado desde el Inspector
    public Transform panelVida; // Panel donde se mostrarán los corazones

    public Sprite corazonEntero;
    public Sprite corazonMedio;
    public Sprite corazonVacio;

    private VidaJugador vidaJugador;
    private List<Image> corazones = new List<Image>();

    private void Start()
    {
        GameObject jugador = null;
        jugador = GameObject.FindWithTag("Jugador");

        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        // Crear los corazones basados en la salud máxima
        if (vidaJugador != null)
        {
            CrearCorazones(vidaJugador.ObtenerSaludMaxima());
        }

        ActualizarVidas();
    }

    private void Update()
    {

        // Si no se ha encontrado el jugador en Start(), intentar encontrarlo en Update()
        if (vidaJugador == null)
        {
            ObtenerJugador();
        }

        if (vidaJugador != null)
        {
            ActualizarVidas();
        }
    }

    private void CrearCorazones(int cantidad)
    {
        // Limpiar la lista y eliminar corazones anteriores
        foreach (var corazon in corazones)
        {
            Destroy(corazon.gameObject);
        }
        corazones.Clear();

        // Ajustar la cantidad de corazones para que siempre haya 1 corazón por cada 2 de vida
        int numeroDeCorazones = Mathf.CeilToInt(cantidad / 2f); // Redondear hacia arriba

        // Crear los nuevos corazones según la vida máxima
        for (int i = 0; i < numeroDeCorazones; i++)
        {
            GameObject corazonObj = Instantiate(prefabCorazon, panelVida);
            Image corazonImg = corazonObj.GetComponent<Image>();
            corazonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 60, 0); // Ajusta la posición de cada corazón
            corazones.Add(corazonImg);
        }
    }

    private void ActualizarVidas()
    {
        if (vidaJugador == null) return;

        int saludActual = vidaJugador.ObtenerSalud();
        int saludMaxima = vidaJugador.ObtenerSaludMaxima();

        // Si la vida máxima ha cambiado, recreamos los corazones
        if (corazones.Count != Mathf.CeilToInt(saludMaxima / 2f))
        {
            CrearCorazones(saludMaxima);
        }

        // Cambia el cálculo de vidasCompletas y corazonMedio
        int vidasCompletas = Mathf.FloorToInt(saludActual / 2f); // Dividir por 2.0f en lugar de 2
        bool tieneCorazonMedio = saludActual % 2 != 0;

        // Recorre los corazones y actualízalos
        for (int i = 0; i < corazones.Count; i++)
        {
            if (i < vidasCompletas)
            {
                corazones[i].sprite = corazonEntero;
            }
            else if (i == vidasCompletas && tieneCorazonMedio)
            {
                corazones[i].sprite = corazonMedio;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }
        }
    }

    // Método para añadir un nuevo corazón cuando la salud máxima del jugador aumenta
    public void AñadirNuevoCorazon(int aumento)
    {
        if (vidaJugador != null)
        {
            vidaJugador.AumentarSaludMaxima(aumento); // Aumenta la vida al jugador
            CrearCorazones(vidaJugador.ObtenerSaludMaxima()); // Actualizar la barra de vida
            ActualizarVidas(); // Refrescar la visualización
        }
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