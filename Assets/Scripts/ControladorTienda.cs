using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ControladorTienda : MonoBehaviour
{
    private InventarioJugador inventarioJugador;
    private PosicionarArmasJugador posicionadorArmas;
    private int monedasJugador = 0;

    void Start()
    {
        BuscarInventarioJugador();
        BuscarPosicionadorArmas();
        ActualizarUI();
        GenerarArmas();
        UpdateArmasJugadorUI();
    }

    void Update()
    {
        if (inventarioJugador == null) 
        {
            BuscarInventarioJugador();
        }
        
        if (posicionadorArmas == null) 
        {
            BuscarPosicionadorArmas();
        }

        monedasJugador = inventarioJugador.ObtenerCantidadCalaveras();
        ActualizarUI();
        UpdateArmasJugadorUI();  // Mantener actualizada la UI de armas del jugador
    }

    void BuscarInventarioJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null)
        {
            inventarioJugador = jugador.GetComponent<InventarioJugador>();
        }
    }

    void BuscarPosicionadorArmas()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null)
        {
            posicionadorArmas = jugador.GetComponent<PosicionarArmasJugador>();
        }
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
    public Button[] botonesArmasJugador;  // Los 5 botones para mostrar las armas del jugador
    public Image[] imagenesArmasJugador;  // Las imágenes dentro de cada botón

    private OpcionArma[] opcionesActuales;

    void GenerarArmas()
    {
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

        if (inventarioJugador.ObtenerCantidadCalaveras() >= armaSeleccionada.precio)
        {
            inventarioJugador.RestarCalaveras(armaSeleccionada.precio);
            ActualizarUI();
            Debug.Log("Compraste: " + armaSeleccionada.nombre);
            botonesArmas[indice].gameObject.SetActive(false);

            // Llamar al script que maneja la posición de las armas y agregarla al inventario del jugador
            GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
            if (jugador != null)
            {
                PosicionarArmasJugador posicionador = jugador.GetComponent<PosicionarArmasJugador>();
                if (posicionador != null)
                {
                    posicionador.AgregarArma(armaSeleccionada.prefabArma);
                }
            }
        }
        else
        {
            Debug.Log("No tienes suficientes monedas");
        }
    }

    void UpdateArmasJugadorUI()
    {
        if (posicionadorArmas == null) return;

        // Obtener las armas actuales del jugador
        GameObject[] armasActuales = posicionadorArmas.ObtenerArmasActuales();

        for (int i = 0; i < botonesArmasJugador.Length; i++)
        {
            if (i < armasActuales.Length && armasActuales[i] != null)
            {
                // Obtener el SpriteRenderer del arma
                SpriteRenderer spriteRenderer = armasActuales[i].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    imagenesArmasJugador[i].sprite = spriteRenderer.sprite;
                    botonesArmasJugador[i].gameObject.SetActive(true);
                }
            }
            else
            {
                // Si no hay un arma en esta posición, desactivar el botón
                botonesArmasJugador[i].gameObject.SetActive(false);
            }
        }
    }


    void ActualizarUI()
    {
        if (inventarioJugador != null && monedasJugadorTexto != null)
        {
            monedasJugadorTexto.text = inventarioJugador.ObtenerCantidadCalaveras().ToString();
        }
    }

    public void RenovarTiendaConCosto()
    {
        if (inventarioJugador.ObtenerCantidadCalaveras() >= 5) //De momento el precio de renovar siempre es 5
        {
            inventarioJugador.RestarCalaveras(5); // Restar calaveras
            ActualizarUI();
            botonesArmas[0].gameObject.SetActive(true);
            botonesArmas[1].gameObject.SetActive(true);
            GenerarArmas();
        }
    }

    public void RenovarTiendaSinCosto()
    {
        ActualizarUI();
        botonesArmas[0].gameObject.SetActive(true);
        botonesArmas[1].gameObject.SetActive(true);
        GenerarArmas();
    }
    
}
