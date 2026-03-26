using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Controla la tienda del juego: compra de armas (con sistema de niveles/rareza),
/// eliminación de armas del inventario, fusión de armas iguales para subir nivel,
/// y compra de habilidades/objetos.
/// </summary>
public class ControladorTienda : MonoBehaviour
{
    // ==================== REFERENCIAS INTERNAS ====================
    private InventarioJugador inventarioJugador;
    private InventarioArmas inventarioArmas;
    private int monedasJugador = 0;
    private GameObject jugador;

    // ==================== UI MONEDAS ====================
    [Header("Monedas del Jugador")]
    public TextMeshProUGUI monedasJugadorTexto;

    // ==================== CATÁLOGO Y ARMAS EN VENTA ====================
    [Header("Catálogo de Armas (DatosArma ScriptableObjects)")]
    public DatosArma[] catalogoArmas;

    [Header("Armas en Venta - UI")]
    public Button[] botonesArmas;
    public Image[] imagenesArmas;
    public TextMeshProUGUI[] precioArmas;
    public TextMeshProUGUI[] nombresArmas;
    public TextMeshProUGUI[] tiposArmas;
    public TextMeshProUGUI[] dañosArmas;
    public TextMeshProUGUI[] criticosArmas;
    public TextMeshProUGUI[] recargasArmas;
    public TextMeshProUGUI[] roboSaludArmas;

    // ==================== INVENTARIO ARMAS JUGADOR ====================
    [Header("Inventario Armas del Jugador - UI")]
    public Button[] botonesArmasJugador;
    public Image[] imagenesArmasJugador;
    public Image[] bordeNivelArmasJugador;

    // ==================== PANEL DETALLE ARMA ====================
    [Header("Panel Detalle Arma (se abre al hacer clic en arma del inventario)")]
    public GameObject panelDetalleArma;
    public Image imagenDetalleArma;
    public TextMeshProUGUI nombreDetalleArma;
    public TextMeshProUGUI nivelDetalleArma;
    public TextMeshProUGUI danioDetalleArma;
    public TextMeshProUGUI criticoDetalleArma;
    public TextMeshProUGUI recargaDetalleArma;
    public TextMeshProUGUI roboSaludDetalleArma;
    public Button botonEliminarArma;
    public Button botonMejorarArma;
    public TextMeshProUGUI textoBotonMejorar;

    // ==================== OBJETOS / HABILIDADES ====================
    [Header("Objetos en Venta - UI")]
    public OpcionObjeto[] listaObjetos;
    public Button botonObjeto;
    public Image imagenObjeto;
    public TextMeshProUGUI precioObjeto;
    public TextMeshProUGUI nombreObjeto;
    public TextMeshProUGUI descripcionObjeto;
    public Button[] botonesObjetosJugador;
    public Image[] imagenesObjetosJugador;

    [System.Serializable]
    public class OpcionObjeto
    {
        public int id;
        public int precio;
        public Sprite imagen;
        public string nombre;
        public string descripcion;
    }

    // ==================== VARIABLES INTERNAS ====================
    private DatosArma[] armasEnVenta;
    private OpcionObjeto opcionObjetoActual;
    private List<OpcionObjeto> objetosComprados = new List<OpcionObjeto>();
    private int indiceArmaSeleccionada = -1;

    // ==================== INICIALIZACIÓN ====================

    void Start()
    {
        BuscarReferencias();
        ActualizarPreciosObjetos();
        ActualizarUI();
        GenerarArmas();
        GenerarObjetos();
        ActualizarTextosHabilidades();

        if (panelDetalleArma != null)
            panelDetalleArma.SetActive(false);
    }

    void Update()
    {
        BuscarReferencias();

        if (inventarioJugador != null)
        {
            monedasJugador = inventarioJugador.ObtenerCantidadCalaveras();
        }

        ActualizarUI();
        ActualizarArmasJugadorUI();
        ActualizarObjetosJugadorUI();
        ActualizarTextosHabilidades();
    }

    void BuscarReferencias()
    {
        if (jugador == null)
            jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null && inventarioJugador == null)
            inventarioJugador = jugador.GetComponent<InventarioJugador>();
        if (inventarioArmas == null)
            inventarioArmas = InventarioArmas.instancia;
    }

    // ==================== ARMAS EN VENTA ====================

    void GenerarArmas()
    {
        if (catalogoArmas == null || catalogoArmas.Length == 0) return;

        armasEnVenta = new DatosArma[botonesArmas.Length];

        for (int i = 0; i < botonesArmas.Length; i++)
        {
            DatosArma datos = catalogoArmas[Random.Range(0, catalogoArmas.Length)];
            armasEnVenta[i] = datos;

            // Mostrar stats de nivel 1 (lo que compras)
            imagenesArmas[i].sprite = datos.icono;
            precioArmas[i].text = datos.ObtenerPrecio(1).ToString();
            nombresArmas[i].text = datos.nombre;
            tiposArmas[i].text = "Tipo: " + (datos.tipo == TipoArma.Melee ? "Melee" : "Distancia");
            dañosArmas[i].text = "Daño: " + datos.ObtenerDanio(1);
            criticosArmas[i].text = "Crítico: X2 (" + datos.ObtenerCritico(1) + "%)";
            recargasArmas[i].text = "Recarga: " + datos.ObtenerRecarga(1) + "s";
            roboSaludArmas[i].text = "Robo salud: " + datos.ObtenerRoboVida(1) + "%";
        }
    }

    /// <summary>
    /// Compra un arma de la tienda. Se añade al inventario como nivel 1 (Gris - Común).
    /// </summary>
    public void ComprarArma(int indice)
    {
        if (inventarioArmas == null || !inventarioArmas.PuedeAgregarArma())
        {
            SonidosUI.ReproducirSonidoError();
            return;
        }

        if (armasEnVenta == null || indice < 0 || indice >= armasEnVenta.Length) return;

        DatosArma datos = armasEnVenta[indice];
        int precio = datos.ObtenerPrecio(1);

        if (inventarioJugador.ObtenerCantidadCalaveras() >= precio)
        {
            SonidosUI.ReproducirSonidoCompra();
            inventarioJugador.RestarCalaveras(precio);
            inventarioArmas.AgregarArma(datos, 1);
            botonesArmas[indice].gameObject.SetActive(false);
            ActualizarUI();
        }
        else
        {
            SonidosUI.ReproducirSonidoError();
        }
    }

    // ==================== INVENTARIO DE ARMAS DEL JUGADOR ====================

    void ActualizarArmasJugadorUI()
    {
        if (inventarioArmas == null) return;

        List<ArmaInstancia> armas = inventarioArmas.ObtenerArmas();

        for (int i = 0; i < botonesArmasJugador.Length; i++)
        {
            if (i < armas.Count && armas[i] != null)
            {
                botonesArmasJugador[i].gameObject.SetActive(true);
                imagenesArmasJugador[i].sprite = armas[i].Icono;

                // Tinte de color del icono según nivel
                imagenesArmasJugador[i].color = NivelArma.ObtenerColor(armas[i].nivel);

                // Borde de color según nivel (si hay borde asignado)
                if (bordeNivelArmasJugador != null && i < bordeNivelArmasJugador.Length && bordeNivelArmasJugador[i] != null)
                {
                    bordeNivelArmasJugador[i].color = NivelArma.ObtenerColor(armas[i].nivel);
                }
            }
            else
            {
                botonesArmasJugador[i].gameObject.SetActive(false);
            }
        }
    }

    // ==================== PANEL DETALLE (ELIMINAR / MEJORAR) ====================

    /// <summary>
    /// Se llama al hacer clic en un botón del inventario de armas.
    /// Asignar cada botonesArmasJugador[i] con su índice (0, 1, 2, 3, 4) en el inspector.
    /// </summary>
    public void SeleccionarArmaInventario(int indice)
    {
        if (inventarioArmas == null) return;

        ArmaInstancia arma = inventarioArmas.ObtenerArma(indice);
        if (arma == null) return;

        indiceArmaSeleccionada = indice;
        MostrarPanelDetalleArma(arma);
    }

    void MostrarPanelDetalleArma(ArmaInstancia arma)
    {
        if (panelDetalleArma == null) return;

        panelDetalleArma.SetActive(true);

        if (imagenDetalleArma != null)
        {
            imagenDetalleArma.sprite = arma.Icono;
            imagenDetalleArma.color = NivelArma.ObtenerColor(arma.nivel);
        }
        if (nombreDetalleArma != null)
            nombreDetalleArma.text = arma.Nombre;
        if (nivelDetalleArma != null)
        {
            nivelDetalleArma.text = "Nv." + arma.nivel + " - " + NivelArma.ObtenerNombreNivel(arma.nivel);
            nivelDetalleArma.color = NivelArma.ObtenerColor(arma.nivel);
        }
        if (danioDetalleArma != null)
            danioDetalleArma.text = "Daño: " + arma.Danio;
        if (criticoDetalleArma != null)
            criticoDetalleArma.text = "Crítico: X2 (" + arma.Critico + "%)";
        if (recargaDetalleArma != null)
            recargaDetalleArma.text = "Recarga: " + arma.Recarga + "s";
        if (roboSaludDetalleArma != null)
            roboSaludDetalleArma.text = "Robo salud: " + arma.RoboVida + "%";

        // Configurar botón Mejorar
        int indicePar = inventarioArmas.BuscarParejaParaMejora(indiceArmaSeleccionada);
        bool puedeMejorar = indicePar != -1;

        if (botonMejorarArma != null)
        {
            botonMejorarArma.interactable = puedeMejorar;
        }
        if (textoBotonMejorar != null)
        {
            if (!arma.PuedeMejorar)
                textoBotonMejorar.text = "Nivel Máximo";
            else if (puedeMejorar)
                textoBotonMejorar.text = "Mejorar a " + NivelArma.ObtenerNombreNivel(arma.nivel + 1);
            else
                textoBotonMejorar.text = "Sin pareja";
        }
    }

    /// <summary>
    /// Elimina el arma seleccionada del inventario. Conectar al botón "Eliminar".
    /// </summary>
    public void EliminarArmaSeleccionada()
    {
        if (inventarioArmas == null || indiceArmaSeleccionada < 0) return;

        SonidosUI.ReproducirSonidoCompra();
        inventarioArmas.EliminarArma(indiceArmaSeleccionada);
        CerrarPanelDetalleArma();
    }

    /// <summary>
    /// Fusiona el arma seleccionada con otra igual del mismo nivel para subir de nivel.
    /// Conectar al botón "Mejorar".
    /// </summary>
    public void MejorarArmaSeleccionada()
    {
        if (inventarioArmas == null || indiceArmaSeleccionada < 0) return;

        if (inventarioArmas.MejorarArma(indiceArmaSeleccionada))
        {
            SonidosUI.ReproducirSonidoCompra();
            CerrarPanelDetalleArma();
        }
        else
        {
            SonidosUI.ReproducirSonidoError();
        }
    }

    /// <summary>
    /// Cierra el panel de detalle de arma. Conectar al botón "Cerrar" o al fondo.
    /// </summary>
    public void CerrarPanelDetalleArma()
    {
        indiceArmaSeleccionada = -1;
        if (panelDetalleArma != null)
            panelDetalleArma.SetActive(false);
    }

    // ==================== OBJETOS / HABILIDADES ====================

    void GenerarObjetos()
    {
        GestorHabilidades gestorHabilidades = null;
        if (jugador != null)
        {
            gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
        }

        List<OpcionObjeto> objetosDisponibles = new List<OpcionObjeto>(listaObjetos);

        if (gestorHabilidades != null && gestorHabilidades.TieneMultiplicadorCalaveras())
        {
            objetosDisponibles.RemoveAll(obj => obj.id == 10);
        }

        if (objetosDisponibles.Count == 0)
        {
            botonObjeto.gameObject.SetActive(false);
            return;
        }

        opcionObjetoActual = objetosDisponibles[Random.Range(0, objetosDisponibles.Count)];
        imagenObjeto.sprite = opcionObjetoActual.imagen;
        precioObjeto.text = opcionObjetoActual.precio.ToString();
        nombreObjeto.text = opcionObjetoActual.nombre;
        descripcionObjeto.text = opcionObjetoActual.descripcion;
    }

    public void ComprarObjeto()
    {
        OpcionObjeto objetoSeleccionado = opcionObjetoActual;

        if (inventarioJugador.ObtenerCantidadCalaveras() >= objetoSeleccionado.precio)
        {
            // Verificación especial para multiplicador de calaveras
            if (objetoSeleccionado.id == 10)
            {
                if (jugador == null) BuscarReferencias();

                if (jugador != null)
                {
                    GestorHabilidades gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
                    if (gestorHabilidades != null && gestorHabilidades.TieneMultiplicadorCalaveras())
                    {
                        SonidosUI.ReproducirSonidoError();
                        return;
                    }
                }
            }

            SonidosUI.ReproducirSonidoCompra();
            inventarioJugador.RestarCalaveras(objetoSeleccionado.precio);
            ActualizarUI();
            botonObjeto.gameObject.SetActive(false);

            if (jugador == null) BuscarReferencias();

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
            SonidosUI.ReproducirSonidoError();
        }
    }

    void ActualizarObjetosJugadorUI()
    {
        for (int i = 0; i < botonesObjetosJugador.Length; i++)
        {
            if (i < objetosComprados.Count && objetosComprados[i] != null)
            {
                imagenesObjetosJugador[i].sprite = objetosComprados[i].imagen;
                botonesObjetosJugador[i].gameObject.SetActive(true);
            }
            else
            {
                botonesObjetosJugador[i].gameObject.SetActive(false);
            }
        }
    }

    // ==================== RENOVAR TIENDA ====================

    public void RenovarTiendaConCosto()
    {
        if (inventarioJugador.ObtenerCantidadCalaveras() >= 5)
        {
            SonidosUI.ReproducirSonidoCompra();
            inventarioJugador.RestarCalaveras(5);
            ActualizarUI();
            ActualizarPreciosObjetos();

            foreach (Button b in botonesArmas)
                b.gameObject.SetActive(true);
            botonObjeto.gameObject.SetActive(true);

            GenerarArmas();
            GenerarObjetos();
        }
        else
        {
            SonidosUI.ReproducirSonidoError();
        }
    }

    public void RenovarTiendaSinCosto()
    {
        ActualizarUI();
        ActualizarPreciosObjetos();

        foreach (Button b in botonesArmas)
            b.gameObject.SetActive(true);
        botonObjeto.gameObject.SetActive(true);

        GenerarArmas();
        GenerarObjetos();
    }

    // ==================== UTILIDADES ====================

    void ActualizarUI()
    {
        if (inventarioJugador != null && monedasJugadorTexto != null)
        {
            monedasJugadorTexto.text = inventarioJugador.ObtenerCantidadCalaveras().ToString();
        }
    }

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

    void ActualizarPreciosObjetos()
    {
        foreach (OpcionObjeto objeto in listaObjetos)
        {
            switch (objeto.id)
            {
                case 1: objeto.precio = 15; break;
                case 2: objeto.precio = 18; break;
                case 3: objeto.precio = 22; break;
                case 4: objeto.precio = 25; break;
                case 5: objeto.precio = 22; break;
                case 6: objeto.precio = 22; break;
                case 7: objeto.precio = 20; break;
                case 8: objeto.precio = 25; break;
                case 9: objeto.precio = 18; break;
                case 10: objeto.precio = 50; break;
                default: objeto.precio = 20; break;
            }

            GameObject controladorNivelesObj = GameObject.FindWithTag("GameController");
            if (controladorNivelesObj != null)
            {
                ControladorNiveles controladorNiveles = controladorNivelesObj.GetComponent<ControladorNiveles>();
                if (controladorNiveles != null)
                {
                    int nivelActual = controladorNiveles.nivelActual;
                    if (nivelActual > 1)
                    {
                        float incrementoPorNivel = 0.05f * (nivelActual - 1);
                        objeto.precio = Mathf.RoundToInt(objeto.precio * (1 + incrementoPorNivel));
                    }
                }
            }
        }
    }
}
