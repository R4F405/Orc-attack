using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ControladorTienda : MonoBehaviour
{

    void Start()
    {
        GenerarArmas();
    }

   [System.Serializable]
    public class OpcionArma
    {
        public int precio;
        public Sprite imagen;
        public string nombre;
        public string tipo;
        public int danio;
        public int critico;
        public float recarga;
        public int roboSalud;
        public GameObject prefabArma;  // Referencia al arma que se entregará al comprar
    }

    public TextMeshProUGUI monedasJugadorTexto;
    public OpcionArma[] listaArmas; // Todas las armas posibles en la tienda
    public Button[] botonesArmas;  // Los botones que muestran las armas en la UI
    public Image[] imagenesArmas;  // Imagen dentro de cada botón
    public TextMeshProUGUI[] precioArmas;
    public TextMeshProUGUI[] nombresArmas;
    public TextMeshProUGUI[] tiposArmas;
    public TextMeshProUGUI[] dañosArmas;
    public TextMeshProUGUI[] criticosArmas;
    public TextMeshProUGUI[] recargasArmas;
    public TextMeshProUGUI[] roboSaludArmas;

    private OpcionArma[] opcionesActuales;
    private int monedasJugador = 200; // Referencia a las monedas del jugador (debes enlazarla con el sistema real)

    void GenerarArmas()
    {
        monedasJugadorTexto.text = monedasJugador.ToString();
        opcionesActuales = new OpcionArma[botonesArmas.Length];

        for (int i = 0; i < botonesArmas.Length; i++)
        {
            OpcionArma armaSeleccionada = listaArmas[Random.Range(0, listaArmas.Length)];

            // Guardar el arma seleccionada
            opcionesActuales[i] = armaSeleccionada;

            // Asignar la información a los textos
            imagenesArmas[i].sprite = armaSeleccionada.imagen;
            precioArmas[i].text = armaSeleccionada.precio.ToString();
            nombresArmas[i].text = armaSeleccionada.nombre;
            tiposArmas[i].text = "Tipo: " + armaSeleccionada.tipo;
            dañosArmas[i].text = "Daño: " + armaSeleccionada.danio;
            criticosArmas[i].text = "Crítico: X2 (" + armaSeleccionada.critico + "%)";
            recargasArmas[i].text = "Recarga: " + armaSeleccionada.recarga + "s";
            roboSaludArmas[i].text = "Robo salud: " + armaSeleccionada.roboSalud + "%";
        }
    }

    public void ComprarArma(int indice)
    {
        OpcionArma armaSeleccionada = opcionesActuales[indice];

        if (monedasJugador >= armaSeleccionada.precio)
        {
            monedasJugador -= armaSeleccionada.precio;
            // Aquí debes asignar el arma al jugador según tu sistema
            Debug.Log("Compraste: " + armaSeleccionada.nombre);
            botonesArmas[indice].gameObject.SetActive(false);
            monedasJugadorTexto.text = monedasJugador.ToString();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas");
        }
    }

    public void RenovarTienda()
    {
        if (monedasJugador >= 5)
        {
            botonesArmas[0].gameObject.SetActive(true);
            botonesArmas[1].gameObject.SetActive(true);
            GenerarArmas();
        }
    }
}
