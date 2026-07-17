using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Controla la tienda: compra, venta (10% del precio) y fusión de armas (2 iguales = +1 nivel, máx. 6).
/// El botón Mejorar solo se muestra cuando hay otra arma del mismo tipo y nivel.
/// Crea y conecta la UI de detalle en runtime si no está asignada en el inspector.
/// </summary>
public class ControladorTienda : MonoBehaviour
{
    private InventarioJugador inventarioJugador;
    private InventarioArmas inventarioArmas;
    private int monedasJugador;
    private GameObject jugador;

    [Header("Monedas del Jugador")]
    public TextMeshProUGUI monedasJugadorTexto;

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

    [Header("Inventario Armas del Jugador - UI")]
    public Button[] botonesArmasJugador;
    public Image[] imagenesArmasJugador;
    public Image[] bordeNivelArmasJugador;

    [Header("Panel Detalle Arma (opcional — se crea solo si falta)")]
    public GameObject panelDetalleArma;
    public Image imagenDetalleArma;
    public TextMeshProUGUI nombreDetalleArma;
    public TextMeshProUGUI nivelDetalleArma;
    public TextMeshProUGUI danioDetalleArma;
    public TextMeshProUGUI criticoDetalleArma;
    public TextMeshProUGUI recargaDetalleArma;
    public TextMeshProUGUI roboSaludDetalleArma;
    public TextMeshProUGUI textoPrecioVenta;
    public Button botonVenderArma;
    public Button botonMejorarArma;
    public TextMeshProUGUI textoBotonMejorar;

    [Header("Objetos / Habilidades")]
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

    private DatosArma[] armasEnVenta;
    private OpcionObjeto opcionObjetoActual;
    private readonly List<OpcionObjeto> objetosComprados = new List<OpcionObjeto>();
    private int indiceArmaSeleccionada = -1;
    private bool uiInicializada;
    private int ultimoFrameCompraArma = -1;

    void Start()
    {
        BuscarReferencias();
        AsegurarInventarioArmas();
        AsegurarPanelDetalle();
        ConfigurarListenersBotones();
        ActualizarPreciosObjetos();
        ActualizarUI();
        GenerarArmas();
        GenerarObjetos();
        ActualizarTextosHabilidades();

        if (panelDetalleArma != null)
            panelDetalleArma.SetActive(false);

        uiInicializada = true;
    }

    void Update()
    {
        BuscarReferencias();

        if (inventarioJugador != null)
            monedasJugador = inventarioJugador.ObtenerCantidadCalaveras();

        ActualizarUI();
        ActualizarArmasJugadorUI();
        ActualizarObjetosJugadorUI();
        ActualizarTextosHabilidades();

        if (panelDetalleArma != null && panelDetalleArma.activeSelf && indiceArmaSeleccionada >= 0)
            ActualizarPanelDetalle();
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

    void AsegurarInventarioArmas()
    {
        if (InventarioArmas.instancia != null) return;

        GameObject obj = new GameObject("InventarioArmas");
        obj.AddComponent<InventarioArmas>();
    }

    /// <summary>
    /// Conecta todos los botones en código.
    /// Importante: hay que limpiar también los listeners persistentes del Inspector;
    /// RemoveAllListeners() solo quita los añadidos en runtime y si no, ComprarArma se dispara 2 veces.
    /// </summary>
    void ConfigurarListenersBotones()
    {
        if (botonesArmas != null)
        {
            for (int i = 0; i < botonesArmas.Length; i++)
            {
                if (botonesArmas[i] == null) continue;
                int indice = i;
                ReemplazarOnClick(botonesArmas[i], () => ComprarArma(indice));
            }
        }

        if (botonesArmasJugador != null)
        {
            for (int i = 0; i < botonesArmasJugador.Length; i++)
            {
                if (botonesArmasJugador[i] == null) continue;
                int indice = i;
                ReemplazarOnClick(botonesArmasJugador[i], () => SeleccionarArmaInventario(indice));
            }
        }

        if (botonVenderArma != null)
            ReemplazarOnClick(botonVenderArma, VenderArmaSeleccionada);

        if (botonMejorarArma != null)
            ReemplazarOnClick(botonMejorarArma, MejorarArmaSeleccionada);
    }

    static void ReemplazarOnClick(Button boton, UnityEngine.Events.UnityAction accion)
    {
        boton.onClick = new Button.ButtonClickedEvent();
        boton.onClick.AddListener(accion);
    }

    // ==================== ARMAS EN VENTA ====================

    void GenerarArmas()
    {
        if (catalogoArmas == null || catalogoArmas.Length == 0 || botonesArmas == null) return;

        armasEnVenta = new DatosArma[botonesArmas.Length];

        for (int i = 0; i < botonesArmas.Length; i++)
        {
            DatosArma datos = catalogoArmas[Random.Range(0, catalogoArmas.Length)];
            armasEnVenta[i] = datos;
            MostrarArmaEnVenta(i, datos);
        }
    }

    void MostrarArmaEnVenta(int i, DatosArma datos)
    {
        if (imagenesArmas != null && i < imagenesArmas.Length && imagenesArmas[i] != null)
            imagenesArmas[i].sprite = datos.icono;
        if (precioArmas != null && i < precioArmas.Length && precioArmas[i] != null)
            precioArmas[i].text = datos.ObtenerPrecio(1).ToString();
        if (nombresArmas != null && i < nombresArmas.Length && nombresArmas[i] != null)
            nombresArmas[i].text = datos.nombre;
        if (tiposArmas != null && i < tiposArmas.Length && tiposArmas[i] != null)
            tiposArmas[i].text = "Tipo: " + (datos.tipo == TipoArma.Melee ? "Melee" : "Distancia");
        if (dañosArmas != null && i < dañosArmas.Length && dañosArmas[i] != null)
            dañosArmas[i].text = "Daño: " + datos.ObtenerDanio(1);
        if (criticosArmas != null && i < criticosArmas.Length && criticosArmas[i] != null)
            criticosArmas[i].text = "Crítico: X2 (" + datos.ObtenerCritico(1) + "%)";
        if (recargasArmas != null && i < recargasArmas.Length && recargasArmas[i] != null)
            recargasArmas[i].text = "Recarga: " + datos.ObtenerRecarga(1) + "s";
        if (roboSaludArmas != null && i < roboSaludArmas.Length && roboSaludArmas[i] != null)
            roboSaludArmas[i].text = "Robo salud: " + datos.ObtenerRoboVida(1) + "%";
    }

    public void ComprarArma(int indice)
    {
        // Evita doble compra si el botón aún tiene listener del Inspector + runtime.
        if (ultimoFrameCompraArma == Time.frameCount)
            return;
        ultimoFrameCompraArma = Time.frameCount;

        BuscarReferencias();
        AsegurarInventarioArmas();
        inventarioArmas = InventarioArmas.instancia;

        if (inventarioArmas == null || !inventarioArmas.PuedeAgregarArma())
        {
            SonidosUI.ReproducirSonidoError();
            return;
        }

        if (armasEnVenta == null || indice < 0 || indice >= armasEnVenta.Length || armasEnVenta[indice] == null)
            return;

        DatosArma datos = armasEnVenta[indice];
        int precio = datos.ObtenerPrecio(1);

        if (inventarioJugador != null && inventarioJugador.ObtenerCantidadCalaveras() >= precio)
        {
            SonidosUI.ReproducirSonidoCompra();
            inventarioJugador.RestarCalaveras(precio);
            inventarioArmas.AgregarArma(datos, 1);
            if (botonesArmas != null && indice < botonesArmas.Length && botonesArmas[indice] != null)
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
        if (inventarioArmas == null || botonesArmasJugador == null) return;

        List<ArmaInstancia> armas = inventarioArmas.ObtenerArmas();

        for (int i = 0; i < botonesArmasJugador.Length; i++)
        {
            if (botonesArmasJugador[i] == null) continue;

            if (i < armas.Count && armas[i] != null)
            {
                botonesArmasJugador[i].gameObject.SetActive(true);
                AplicarColorTarjetaSlot(botonesArmasJugador[i], armas[i].nivel);

                if (bordeNivelArmasJugador != null && i < bordeNivelArmasJugador.Length && bordeNivelArmasJugador[i] != null)
                    bordeNivelArmasJugador[i].color = NivelArma.ObtenerColor(armas[i].nivel);

                if (imagenesArmasJugador != null && i < imagenesArmasJugador.Length && imagenesArmasJugador[i] != null)
                {
                    imagenesArmasJugador[i].sprite = armas[i].Icono;
                    // Icono sin tinte: el color de nivel va solo en la tarjeta de fondo.
                    imagenesArmasJugador[i].color = Color.white;
                }
            }
            else
            {
                botonesArmasJugador[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Colorea la tarjeta del slot. Hay que tocar el ColorBlock del Button:
    /// si no, el ColorTint del botón vuelve a pintar el gris cada frame.
    /// </summary>
    static void AplicarColorTarjetaSlot(Button boton, int nivel)
    {
        Color colorFondo = NivelArma.ObtenerColorFondo(nivel);
        colorFondo.a = 0.86f;

        Color resaltado = Color.Lerp(colorFondo, Color.white, 0.18f);
        resaltado.a = colorFondo.a;
        Color pulsado = Color.Lerp(colorFondo, Color.black, 0.18f);
        pulsado.a = colorFondo.a;

        ColorBlock colores = boton.colors;
        colores.normalColor = colorFondo;
        colores.highlightedColor = resaltado;
        colores.selectedColor = colorFondo;
        colores.pressedColor = pulsado;
        colores.disabledColor = colorFondo;
        colores.colorMultiplier = 1f;
        boton.colors = colores;

        // El Button con ColorTint pinta el targetGraphic; forzamos el color al instante.
        Graphic grafico = boton.targetGraphic;
        if (grafico == null)
            grafico = boton.GetComponent<Graphic>();
        if (grafico != null)
        {
            grafico.color = Color.white;
            grafico.CrossFadeColor(colorFondo, 0f, true, true);
        }
    }

    // ==================== PANEL DETALLE (VENDER / MEJORAR) ====================

    public void SeleccionarArmaInventario(int indice)
    {
        BuscarReferencias();
        if (inventarioArmas == null) return;

        ArmaInstancia arma = inventarioArmas.ObtenerArma(indice);
        if (arma == null) return;

        indiceArmaSeleccionada = indice;
        AsegurarPanelDetalle();
        if (panelDetalleArma != null)
        {
            panelDetalleArma.SetActive(true);
            ActualizarPanelDetalle();
        }
    }

    void ActualizarPanelDetalle()
    {
        if (inventarioArmas == null || indiceArmaSeleccionada < 0) return;

        ArmaInstancia arma = inventarioArmas.ObtenerArma(indiceArmaSeleccionada);
        if (arma == null)
        {
            CerrarPanelDetalleArma();
            return;
        }

        if (imagenDetalleArma != null)
        {
            imagenDetalleArma.sprite = arma.Icono;
            imagenDetalleArma.color = Color.white;

            if (imagenDetalleArma.transform.parent != null)
            {
                Transform tFondo = imagenDetalleArma.transform.parent.Find("FondoNivelIcono");
                if (tFondo != null)
                {
                    Image fondoIcono = tFondo.GetComponent<Image>();
                    if (fondoIcono != null)
                        fondoIcono.color = NivelArma.ObtenerColorFondo(arma.nivel);
                }
            }
        }
        if (nombreDetalleArma != null)
            nombreDetalleArma.text = arma.Nombre;
        if (nivelDetalleArma != null)
        {
            nivelDetalleArma.text = "Nv." + arma.nivel + " — " + NivelArma.ObtenerNombreNivel(arma.nivel);
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

        int precioVenta = InventarioArmas.CalcularPrecioVenta(arma);
        if (textoPrecioVenta != null)
            textoPrecioVenta.text = "Vender: +" + precioVenta + " calaveras (10%)";

        int parejas = inventarioArmas.ContarParejas(indiceArmaSeleccionada);
        // Solo visible si hay exactamente una pareja fusionable (2 del mismo tipo y nivel).
        bool puedeMejorar = arma.PuedeMejorar && parejas > 0;

        if (botonMejorarArma != null)
        {
            botonMejorarArma.gameObject.SetActive(puedeMejorar);
            botonMejorarArma.interactable = puedeMejorar;
        }

        if (textoBotonMejorar != null && puedeMejorar)
        {
            textoBotonMejorar.text = "Mejorar → " + NivelArma.ObtenerNombreNivel(arma.nivel + 1);
        }

        // Centrar Vender cuando Mejorar no está visible.
        if (botonVenderArma != null)
        {
            RectTransform rtVender = botonVenderArma.GetComponent<RectTransform>();
            if (rtVender != null)
                rtVender.anchoredPosition = puedeMejorar ? new Vector2(110f, -200f) : new Vector2(0f, -200f);
        }
    }

    /// <summary>
    /// Vende el arma seleccionada por el 10% de su precio de compra.
    /// </summary>
    public void VenderArmaSeleccionada()
    {
        BuscarReferencias();
        if (inventarioArmas == null || inventarioJugador == null || indiceArmaSeleccionada < 0) return;

        int reembolso = inventarioArmas.VenderArma(indiceArmaSeleccionada);
        if (reembolso > 0)
        {
            SonidosUI.ReproducirSonidoCompra();
            inventarioJugador.AgregarCalaverasDirectas(reembolso);
            CerrarPanelDetalleArma();
            ActualizarUI();
        }
        else
        {
            SonidosUI.ReproducirSonidoError();
        }
    }

    /// <summary>
    /// Compatibilidad con botones antiguos del inspector.
    /// </summary>
    public void EliminarArmaSeleccionada() => VenderArmaSeleccionada();

    public void MejorarArmaSeleccionada()
    {
        BuscarReferencias();
        if (inventarioArmas == null || indiceArmaSeleccionada < 0) return;

        int nuevoIndice = inventarioArmas.MejorarArma(indiceArmaSeleccionada);
        if (nuevoIndice >= 0)
        {
            SonidosUI.ReproducirSonidoCompra();
            indiceArmaSeleccionada = nuevoIndice;
            // Mantener el panel abierto; Mejorar se oculta si ya no hay otra pareja.
            ActualizarPanelDetalle();
            ActualizarArmasJugadorUI();
        }
        else
        {
            SonidosUI.ReproducirSonidoError();
        }
    }

    public void CerrarPanelDetalleArma()
    {
        indiceArmaSeleccionada = -1;
        if (panelDetalleArma != null)
            panelDetalleArma.SetActive(false);
    }

    // ==================== CREACIÓN UI EN RUNTIME ====================

    void AsegurarPanelDetalle()
    {
        if (panelDetalleArma != null && botonVenderArma != null) return;

        Transform padre = BuscarCanvasTienda();
        if (padre == null) return;

        TMP_FontAsset fuente = monedasJugadorTexto != null ? monedasJugadorTexto.font : null;

        panelDetalleArma = CrearPanel(padre, "PanelDetalleArma", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero);
        Image fondoOverlay = panelDetalleArma.AddComponent<Image>();
        fondoOverlay.color = new Color(0, 0, 0, 0.75f);
        Button cerrarFondo = panelDetalleArma.AddComponent<Button>();
        cerrarFondo.transition = Selectable.Transition.None;
        cerrarFondo.onClick.AddListener(CerrarPanelDetalleArma);

        GameObject caja = CrearPanel(panelDetalleArma.transform, "CajaDetalle", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(520, 620));
        Image fondoCaja = caja.AddComponent<Image>();
        fondoCaja.color = new Color(0.12f, 0.12f, 0.14f, 0.98f);

        Image fondoNivelIcono = CrearImagen(caja.transform, "FondoNivelIcono", new Vector2(0, 180), new Vector2(140, 140));
        fondoNivelIcono.color = NivelArma.ObtenerColorFondo(1);
        imagenDetalleArma = CrearImagen(caja.transform, "Icono", new Vector2(0, 180), new Vector2(120, 120));
        nombreDetalleArma = CrearTexto(caja.transform, "Nombre", new Vector2(0, 100), 28, fuente, FontStyles.Bold);
        nivelDetalleArma = CrearTexto(caja.transform, "Nivel", new Vector2(0, 60), 22, fuente);
        danioDetalleArma = CrearTexto(caja.transform, "Danio", new Vector2(0, 20), 20, fuente);
        criticoDetalleArma = CrearTexto(caja.transform, "Critico", new Vector2(0, -15), 20, fuente);
        recargaDetalleArma = CrearTexto(caja.transform, "Recarga", new Vector2(0, -50), 20, fuente);
        roboSaludDetalleArma = CrearTexto(caja.transform, "RoboSalud", new Vector2(0, -85), 20, fuente);
        textoPrecioVenta = CrearTexto(caja.transform, "PrecioVenta", new Vector2(0, -130), 18, fuente);
        textoPrecioVenta.color = new Color(0.9f, 0.85f, 0.4f);

        botonMejorarArma = CrearBoton(caja.transform, "BtnMejorar", new Vector2(-110, -200), new Vector2(200, 50), "Mejorar", fuente, new Color(0.2f, 0.55f, 0.25f));
        textoBotonMejorar = botonMejorarArma.GetComponentInChildren<TextMeshProUGUI>();
        botonMejorarArma.gameObject.SetActive(false);

        botonVenderArma = CrearBoton(caja.transform, "BtnVender", new Vector2(0, -200), new Vector2(200, 50), "Vender", fuente, new Color(0.55f, 0.25f, 0.2f));
        CrearBoton(caja.transform, "BtnCerrar", new Vector2(0, -270), new Vector2(160, 44), "Cerrar", fuente, new Color(0.3f, 0.3f, 0.35f))
            .onClick.AddListener(CerrarPanelDetalleArma);

        // Reconectar listeners tras crear los botones en runtime.
        if (botonMejorarArma != null)
            ReemplazarOnClick(botonMejorarArma, MejorarArmaSeleccionada);
        if (botonVenderArma != null)
            ReemplazarOnClick(botonVenderArma, VenderArmaSeleccionada);

        panelDetalleArma.SetActive(false);
        panelDetalleArma.transform.SetAsLastSibling();
    }

    Transform BuscarCanvasTienda()
    {
        GameObject tienda = GameObject.Find("Tienda");
        if (tienda != null) return tienda.transform;

        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas c in canvases)
        {
            if (c.gameObject.name.Contains("Tienda") || c.gameObject.name.Contains("PanelTienda"))
                return c.transform;
        }
        return canvases.Length > 0 ? canvases[0].transform : null;
    }

    static GameObject CrearPanel(Transform padre, string nombre, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size)
    {
        GameObject go = new GameObject(nombre, typeof(RectTransform));
        go.transform.SetParent(padre, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.localScale = Vector3.one;
        return go;
    }

    static Image CrearImagen(Transform padre, string nombre, Vector2 pos, Vector2 size)
    {
        GameObject go = CrearPanel(padre, nombre, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), pos, size);
        Image img = go.AddComponent<Image>();
        img.preserveAspect = true;
        return img;
    }

    static TextMeshProUGUI CrearTexto(Transform padre, string nombre, Vector2 pos, float fontSize, TMP_FontAsset fuente, FontStyles estilo = FontStyles.Normal)
    {
        GameObject go = CrearPanel(padre, nombre, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), pos, new Vector2(480, 36));
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = estilo;
        if (fuente != null) tmp.font = fuente;
        return tmp;
    }

    static Button CrearBoton(Transform padre, string nombre, Vector2 pos, Vector2 size, string texto, TMP_FontAsset fuente, Color colorFondo)
    {
        GameObject go = CrearPanel(padre, nombre, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), pos, size);
        Image img = go.AddComponent<Image>();
        img.color = colorFondo;
        Button btn = go.AddComponent<Button>();
        btn.targetGraphic = img;

        TextMeshProUGUI tmp = CrearTexto(go.transform, "Texto", Vector2.zero, 18, fuente, FontStyles.Bold);
        tmp.text = texto;
        RectTransform rt = tmp.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        return btn;
    }

    // ==================== OBJETOS / HABILIDADES ====================

    void GenerarObjetos()
    {
        if (botonObjeto == null) return;

        GestorHabilidades gestorHabilidades = null;
        if (jugador != null)
            gestorHabilidades = jugador.GetComponent<GestorHabilidades>();

        List<OpcionObjeto> objetosDisponibles = new List<OpcionObjeto>(listaObjetos);

        if (gestorHabilidades != null && gestorHabilidades.TieneMultiplicadorCalaveras())
            objetosDisponibles.RemoveAll(obj => obj.id == 10);

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
        if (opcionObjetoActual == null) return;
        OpcionObjeto objetoSeleccionado = opcionObjetoActual;

        if (inventarioJugador != null && inventarioJugador.ObtenerCantidadCalaveras() >= objetoSeleccionado.precio)
        {
            if (objetoSeleccionado.id == 10)
            {
                BuscarReferencias();
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

            BuscarReferencias();
            if (jugador != null)
            {
                GestorHabilidades gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
                gestorHabilidades?.AplicarHabilidadPorID(objetoSeleccionado.id);
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
        if (botonesObjetosJugador == null) return;

        for (int i = 0; i < botonesObjetosJugador.Length; i++)
        {
            if (botonesObjetosJugador[i] == null) continue;

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
        if (inventarioJugador != null && inventarioJugador.ObtenerCantidadCalaveras() >= 5)
        {
            SonidosUI.ReproducirSonidoCompra();
            inventarioJugador.RestarCalaveras(5);
            ActualizarUI();
            ActualizarPreciosObjetos();
            ReactivarSlotsTienda();
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
        ReactivarSlotsTienda();
        GenerarArmas();
        GenerarObjetos();
    }

    void ReactivarSlotsTienda()
    {
        if (botonesArmas != null)
        {
            foreach (Button b in botonesArmas)
            {
                if (b != null) b.gameObject.SetActive(true);
            }
        }
        if (botonObjeto != null)
            botonObjeto.gameObject.SetActive(true);
    }

    // ==================== UTILIDADES ====================

    void ActualizarUI()
    {
        if (inventarioJugador != null && monedasJugadorTexto != null)
            monedasJugadorTexto.text = inventarioJugador.ObtenerCantidadCalaveras().ToString();
    }

    void ActualizarTextosHabilidades()
    {
        if (jugador == null) return;
        GestorHabilidades gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
        gestorHabilidades?.ActualizarTextos();
    }

    void ActualizarPreciosObjetos()
    {
        if (listaObjetos == null) return;

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
                if (controladorNiveles != null && controladorNiveles.nivelActual > 1)
                {
                    float incrementoPorNivel = 0.05f * (controladorNiveles.nivelActual - 1);
                    objeto.precio = Mathf.RoundToInt(objeto.precio * (1 + incrementoPorNivel));
                }
            }
        }
    }
}
