using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


public class ControladorTienda : MonoBehaviour
{
    private InventarioJugador inventarioJugador;
    private PosicionarArmasJugador posicionadorArmas;
    private int monedasJugador = 0;
    private GameObject jugador;

    void Start()
    {
        BuscarInventarioJugador();
        BuscarPosicionadorArmas();
        ActualizarUI();
        GenerarArmas();
        GenerarObjetos();
        BuscarJugador();
        
        // Actualizar textos de habilidades al iniciar
        ActualizarTextosHabilidades();
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
        if (jugador == null) 
        {
            BuscarJugador();
        }

        monedasJugador = inventarioJugador.ObtenerCantidadCalaveras();
        ActualizarUI();
        UpdateArmasJugadorUI();  // Mantener actualizada la UI de armas del jugador
        UpdateObjetosJugadorUI();
        
        // Actualizar textos de habilidades regularmente
        ActualizarTextosHabilidades();
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

    void BuscarJugador() {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
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

    [System.Serializable]
    public class OpcionObjeto
    {
        public int id;
        public int precio;
        public Sprite imagen;
        public string nombre;
        public string descripcion;
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


    public OpcionObjeto[] listaObjetos;
    public Button botonObjeto;
    public Image imagenObjeto;
    public TextMeshProUGUI precioObjeto;
    public TextMeshProUGUI nombreObjeto;
    public TextMeshProUGUI descripcionObjeto;
    public Button[] botonesObjetosJugador;  // Los 16 botones para mostrar los objetos del jugador
    public Image[] imagenesObjetosJugador;  // Las imágenes dentro de cada botón

    private OpcionArma[] opcionesArmasActuales;
    private OpcionObjeto opcionObjetoActual;
    private List<OpcionObjeto> objetosComprados = new List<OpcionObjeto>();

    void GenerarArmas()
    {
        opcionesArmasActuales = new OpcionArma[botonesArmas.Length];

        for (int i = 0; i < botonesArmas.Length; i++)
        {
            OpcionArma armaSeleccionada = listaArmas[Random.Range(0, listaArmas.Length)];

            // Guardar el arma seleccionada
            opcionesArmasActuales[i] = armaSeleccionada;

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

    void GenerarObjetos()
    {
        // Buscar GestorHabilidades para verificar si ya tiene el multiplicador
        GestorHabilidades gestorHabilidades = null;
        if (jugador != null)
        {
            gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
        }
        
        // Crear lista temporal de objetos disponibles
        List<OpcionObjeto> objetosDisponibles = new List<OpcionObjeto>(listaObjetos);
        
        // Si ya tiene el multiplicador de calaveras, eliminar esa opción
        if (gestorHabilidades != null && gestorHabilidades.TieneMultiplicadorCalaveras())
        {
            objetosDisponibles.RemoveAll(obj => obj.id == 10);
            Debug.Log("Multiplicador de calaveras ya comprado, removido de objetos de tienda.");
        }
        
        // Si no hay objetos disponibles, desactivar el botón
        if (objetosDisponibles.Count == 0)
        {
            botonObjeto.gameObject.SetActive(false);
            Debug.LogWarning("No hay objetos disponibles para mostrar en la tienda.");
            return;
        }
        
        // Seleccionar un objeto aleatorio
        opcionObjetoActual = objetosDisponibles[Random.Range(0, objetosDisponibles.Count)];
        imagenObjeto.sprite = opcionObjetoActual.imagen;
        precioObjeto.text = opcionObjetoActual.precio.ToString();
        nombreObjeto.text = opcionObjetoActual.nombre;
        descripcionObjeto.text = opcionObjetoActual.descripcion;
    }

    public void ComprarArma(int indice)
    {
        OpcionArma armaSeleccionada = opcionesArmasActuales[indice];

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

    public void ComprarObjeto()
    {
        OpcionObjeto objetoSeleccionado = opcionObjetoActual;

        // Verificar si tiene suficientes calaveras
        if (inventarioJugador.ObtenerCantidadCalaveras() >= objetoSeleccionado.precio)
        {
            // Si es el multiplicador de calaveras, verificar si ya lo tiene
            if (objetoSeleccionado.id == 10)
            {
                if (jugador == null)
                {
                    BuscarJugador();
                }
                
                if (jugador != null)
                {
                    GestorHabilidades gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
                    if (gestorHabilidades != null && gestorHabilidades.TieneMultiplicadorCalaveras())
                    {
                        Debug.LogWarning("Ya has comprado el multiplicador de calaveras");
                        return;
                    }
                }
            }
            
            // Proceder con la compra
            inventarioJugador.RestarCalaveras(objetoSeleccionado.precio);
            ActualizarUI();
            Debug.Log("Compraste: " + objetoSeleccionado.nombre);
            botonObjeto.gameObject.SetActive(false);

            // Aplicar la habilidad correspondiente usando el ID del objeto
            if (jugador == null)
            {
                BuscarJugador();
            }
            
            if (jugador != null)
            {
                GestorHabilidades gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
                if (gestorHabilidades != null)
                {
                    gestorHabilidades.AplicarHabilidadPorID(objetoSeleccionado.id);
                }
            }

            objetosComprados.Add(opcionObjetoActual);
        }
        else
        {
            Debug.Log("No tienes suficientes calaveras para comprar este objeto");
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

    void UpdateObjetosJugadorUI()
    {
        List<OpcionObjeto> objetosActuales = objetosComprados;

        for (int i = 0; i < botonesObjetosJugador.Length; i++)
        {
            if (i < objetosActuales.Count && objetosActuales[i] != null)
            {
                // Obtener el sprite del arma
                imagenesObjetosJugador[i].sprite = objetosActuales[i].imagen; // Asignación directa del sprite
                botonesObjetosJugador[i].gameObject.SetActive(true);
            }
            else
            {
                // Si no hay un arma en esta posición, desactivar el botón
                botonesObjetosJugador[i].gameObject.SetActive(false);
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
            botonObjeto.gameObject.SetActive(true);
            GenerarArmas();
            GenerarObjetos();
        }
    }

    public void RenovarTiendaSinCosto()
    {
        ActualizarUI();
        botonesArmas[0].gameObject.SetActive(true);
        botonesArmas[1].gameObject.SetActive(true);
        botonObjeto.gameObject.SetActive(true);
        GenerarArmas();
        GenerarObjetos();
    }
    
    // Método para actualizar los textos de habilidades
    private void ActualizarTextosHabilidades()
    {
        if (jugador != null)
        {
            GestorHabilidades gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
            if (gestorHabilidades != null)
            {
                gestorHabilidades.ActualizarTextos();
            }
        }
    }
}
