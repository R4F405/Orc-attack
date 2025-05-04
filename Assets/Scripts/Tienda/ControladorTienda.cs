using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Controla todas las funcionalidades de la tienda del juego, incluyendo la compra de armas y objetos especiales,
/// la gestión de la interfaz de usuario y la actualización de inventarios.
/// </summary>
public class ControladorTienda : MonoBehaviour
{
    private InventarioJugador inventarioJugador;
    private PosicionarArmasJugador posicionadorArmas;
    private int monedasJugador = 0;
    private GameObject jugador;

    /// <summary>
    /// Inicializa la tienda buscando referencias al jugador, actualizando las estadísticas de armas,
    /// precios de objetos, interfaz de usuario y generando las opciones de compra.
    /// </summary>
    void Start()
    {
        BuscarInventarioJugador();
        BuscarPosicionadorArmas();
        ActualizarEstadisticasArmas();
        ActualizarPreciosObjetos();
        ActualizarUI();
        GenerarArmas();
        GenerarObjetos();
        BuscarJugador();
        
        // Actualizar textos de habilidades al iniciar
        ActualizarTextosHabilidades();
    }

    /// <summary>
    /// Actualiza continuamente el estado de la tienda, incluyendo las monedas del jugador, 
    /// las referencias a componentes necesarios y la interfaz de usuario.
    /// </summary>
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

    /// <summary>
    /// Busca el objeto del jugador y obtiene su componente InventarioJugador.
    /// </summary>
    void BuscarInventarioJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null)
        {
            inventarioJugador = jugador.GetComponent<InventarioJugador>();
        }
    }

    /// <summary>
    /// Busca el objeto del jugador y obtiene su componente PosicionarArmasJugador.
    /// </summary>
    void BuscarPosicionadorArmas()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null)
        {
            posicionadorArmas = jugador.GetComponent<PosicionarArmasJugador>();
        }
    }

    /// <summary>
    /// Busca y almacena la referencia al GameObject del jugador.
    /// </summary>
    void BuscarJugador() {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
    }

    /// <summary>
    /// Clase que define una opción de arma disponible en la tienda, con todos sus atributos y estadísticas.
    /// </summary>
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

    /// <summary>
    /// Clase que define un objeto o habilidad disponible para comprar en la tienda.
    /// </summary>
    [System.Serializable]
    public class OpcionObjeto
    {
        public int id;          // ID único para identificar el tipo de habilidad
        public int precio;
        public Sprite imagen;
        public string nombre;
        public string descripcion;
    }

    // Elementos de UI para mostrar la información del jugador
    public TextMeshProUGUI monedasJugadorTexto;
    
    // Elementos de UI para armas en venta
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

    // Elementos de UI para objetos en venta
    public OpcionObjeto[] listaObjetos;
    public Button botonObjeto;
    public Image imagenObjeto;
    public TextMeshProUGUI precioObjeto;
    public TextMeshProUGUI nombreObjeto;
    public TextMeshProUGUI descripcionObjeto;
    public Button[] botonesObjetosJugador;  // Los 16 botones para mostrar los objetos del jugador
    public Image[] imagenesObjetosJugador;  // Las imágenes dentro de cada botón

    // Variables para almacenar las opciones actuales
    private OpcionArma[] opcionesArmasActuales;
    private OpcionObjeto opcionObjetoActual;
    private List<OpcionObjeto> objetosComprados = new List<OpcionObjeto>();

    /// <summary>
    /// Genera las opciones de armas disponibles para comprar en la tienda, 
    /// seleccionándolas aleatoriamente de la lista completa de armas.
    /// </summary>
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

    /// <summary>
    /// Genera un objeto aleatorio para comprar en la tienda.
    /// Verifica previamente si el jugador ya tiene ciertas habilidades especiales como 
    /// el multiplicador de calaveras para no ofrecerlas nuevamente.
    /// </summary>
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

    /// <summary>
    /// Permite al jugador comprar un arma específica a cambio de calaveras.
    /// Verifica si el jugador tiene suficientes calaveras y no supera el límite de armas.
    /// </summary>
    /// <param name="indice">Índice del arma a comprar en la matriz de opciones actuales</param>
    public void ComprarArma(int indice)
    {
        // Validar que no supere el máximo de 5 armas
        if (posicionadorArmas != null && posicionadorArmas.numeroDeArmas >= 5)
        {
            SonidosUI.ReproducirSonidoError();
            Debug.LogWarning("Ya tienes el número máximo de armas");
            return;
        }
        OpcionArma armaSeleccionada = opcionesArmasActuales[indice];

        if (inventarioJugador.ObtenerCantidadCalaveras() >= armaSeleccionada.precio)
        {
            // Reproducir sonido de compra exitosa
            SonidosUI.ReproducirSonidoCompra();
            
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
            // Reproducir sonido de error - fondos insuficientes
            SonidosUI.ReproducirSonidoError();
            
            Debug.Log("No tienes suficientes monedas");
        }
    }

    /// <summary>
    /// Permite al jugador comprar un objeto o habilidad a cambio de calaveras.
    /// Verifica si el jugador tiene suficientes calaveras y aplica la habilidad correspondiente.
    /// </summary>
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
                        // Reproducir sonido de error - ya se tiene el objeto
                        SonidosUI.ReproducirSonidoError();
                        
                        Debug.LogWarning("Ya has comprado el multiplicador de calaveras");
                        return;
                    }
                }
            }
            
            // Proceder con la compra
            // Reproducir sonido de compra exitosa
            SonidosUI.ReproducirSonidoCompra();
            
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
            // Reproducir sonido de error - fondos insuficientes
            SonidosUI.ReproducirSonidoError();
            
            Debug.Log("No tienes suficientes calaveras para comprar este objeto");
        }
    }

    /// <summary>
    /// Actualiza la interfaz de usuario que muestra las armas que el jugador actualmente posee.
    /// </summary>
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

    /// <summary>
    /// Actualiza la interfaz de usuario que muestra los objetos que el jugador ha comprado.
    /// </summary>
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

    /// <summary>
    /// Actualiza el texto que muestra la cantidad de calaveras que tiene el jugador.
    /// </summary>
    void ActualizarUI()
    {
        if (inventarioJugador != null && monedasJugadorTexto != null)
        {
            monedasJugadorTexto.text = inventarioJugador.ObtenerCantidadCalaveras().ToString();
        }
    }

    /// <summary>
    /// Renueva los productos de la tienda a cambio de calaveras (coste de 5 calaveras).
    /// </summary>
    public void RenovarTiendaConCosto()
    {
        if (inventarioJugador.ObtenerCantidadCalaveras() >= 5) //De momento el precio de renovar siempre es 5
        {
            // Sonido de compra exitosa
            SonidosUI.ReproducirSonidoCompra();
            
            inventarioJugador.RestarCalaveras(5); // Restar calaveras
            ActualizarUI();
            ActualizarPreciosObjetos();
            botonesArmas[0].gameObject.SetActive(true);
            botonesArmas[1].gameObject.SetActive(true);
            botonObjeto.gameObject.SetActive(true);
            GenerarArmas();
            GenerarObjetos();
        }
        else
        {
            // Sonido de error - fondos insuficientes
            SonidosUI.ReproducirSonidoError();
        }
    }

    /// <summary>
    /// Renueva los productos de la tienda sin coste (utilizado al inicio o en eventos especiales).
    /// </summary>
    public void RenovarTiendaSinCosto()
    {
        ActualizarUI();
        ActualizarPreciosObjetos();
        botonesArmas[0].gameObject.SetActive(true);
        botonesArmas[1].gameObject.SetActive(true);
        botonObjeto.gameObject.SetActive(true);
        GenerarArmas();
        GenerarObjetos();
    }
    
    /// <summary>
    /// Actualiza los textos de las habilidades en el GestorHabilidades.
    /// </summary>
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

    /// <summary>
    /// Calcula y actualiza las estadísticas y precios de las armas disponibles en la tienda
    /// basándose en sus atributos (daño, crítico, recarga, robo de salud).
    /// </summary>
    void ActualizarEstadisticasArmas()
    {
        foreach (OpcionArma arma in listaArmas)
        {
            if (arma.prefabArma != null)
            {
                // Obtener componentes del arma según su tipo
                ArmasMelee armaMelee = arma.prefabArma.GetComponent<ArmasMelee>();
                ArmasDistancia armaDistancia = arma.prefabArma.GetComponent<ArmasDistancia>();
                
                if (armaMelee != null)
                {
                    // Es un arma cuerpo a cuerpo
                    arma.danio = armaMelee.danioBase;
                    arma.critico = armaMelee.probabilidadCritico;
                    arma.recarga = armaMelee.recargaBase;
                    arma.roboSalud = armaMelee.probabilidadRobarVida;
                    arma.tipo = "Melee";
                    
                    // Calcular precio basado en estadísticas
                    float precioPorDanio = arma.danio * 1.5f;
                    float precioPorCritico = arma.critico * 0.5f;
                    float precioPorRoboSalud = arma.roboSalud * 0.8f;
                    float precioPorRecarga = (1f / arma.recarga) * 3f; // Inversamente proporcional - menos recarga = más caro
                    
                    // Precio base 5 + factores
                    arma.precio = Mathf.RoundToInt(5 + precioPorDanio + precioPorCritico + precioPorRoboSalud + precioPorRecarga);
                    
                    // Ajustes para armas específicas por nombre
                    if (arma.nombre.Contains("Espada Gigante"))
                    {
                        arma.precio += 10; // Arma premium
                    }
                    
                    // Asegurar precio mínimo
                    if (arma.precio < 5) arma.precio = 5;
                    
                    Debug.Log($"Arma melee actualizada: {arma.nombre}, Daño: {arma.danio}, Precio: {arma.precio}");
                }
                else if (armaDistancia != null)
                {
                    // Es un arma a distancia
                    arma.danio = armaDistancia.danioBase;
                    arma.critico = armaDistancia.probabilidadCritico;
                    arma.recarga = armaDistancia.recargaBase;
                    arma.roboSalud = armaDistancia.probabilidadRobarVida;
                    arma.tipo = "Distancia";
                    
                    // Las armas a distancia son generalmente más caras
                    float precioPorDanio = arma.danio * 2f;
                    float precioPorCritico = arma.critico * 0.7f;
                    float precioPorRoboSalud = arma.roboSalud * 1f;
                    float precioPorRecarga = (1f / arma.recarga) * 4f;
                    
                    // Precio base 10 + factores (base más alta para armas a distancia)
                    arma.precio = Mathf.RoundToInt(10 + precioPorDanio + precioPorCritico + precioPorRoboSalud + precioPorRecarga);
                    
                    // Ajustes especiales por tipo de bastón
                    if (arma.nombre.Contains("Curacion"))
                    {
                        arma.precio += 8; // Bastón de curación es más valioso
                    }
                    else if (arma.nombre.Contains("Fuego"))
                    {
                        arma.precio += 5; // Bastón de fuego hace daño extra
                    }
                    
                    // Asegurar precio mínimo
                    if (arma.precio < 8) arma.precio = 8;
                    
                    Debug.Log($"Arma distancia actualizada: {arma.nombre}, Daño: {arma.danio}, Precio: {arma.precio}");
                }
                else
                {
                    Debug.LogWarning($"El prefab {arma.nombre} no tiene componente ArmasMelee ni ArmasDistancia.");
                }
            }
            else
            {
                Debug.LogWarning($"El arma {arma.nombre} no tiene prefab asignado.");
            }
        }
    }

    /// <summary>
    /// Establece los precios de los objetos/habilidades basándose en su ID y el nivel actual del juego.
    /// Los precios aumentan ligeramente con cada nivel para mantener el balance.
    /// </summary>
    void ActualizarPreciosObjetos()
    {
        foreach (OpcionObjeto objeto in listaObjetos)
        {
            // Establecer precio basado en ID (tipo de habilidad)
            switch (objeto.id)
            {
                case 1: // Aumentar Vida Máxima
                    objeto.precio = 15;
                    break;
                case 2: // Reducir Tiempo Recuperación Vida
                    objeto.precio = 18;
                    break;
                case 3: // Aumentar Probabilidad Robar Vida
                    objeto.precio = 22;
                    break;
                case 4: // Aumentar Daño Por Porcentaje (general)
                    objeto.precio = 25;
                    break;
                case 5: // Aumentar Daño Melee
                    objeto.precio = 22;
                    break;
                case 6: // Aumentar Daño Distancia
                    objeto.precio = 22;
                    break;
                case 7: // Reducir Recarga
                    objeto.precio = 20;
                    break;
                case 8: // Aumentar Probabilidad Crítico
                    objeto.precio = 25;
                    break;
                case 9: // Reducir Tiempo Generación Cajas
                    objeto.precio = 18;
                    break;
                case 10: // Multiplicador Calaveras
                    objeto.precio = 50; // Habilidad premium - solo se puede comprar una vez
                    break;
                default:
                    // Para cualquier ID futuro que puedas añadir
                    objeto.precio = 20; // Precio por defecto
                    Debug.LogWarning($"ID de habilidad no reconocido: {objeto.id}. Se estableció precio por defecto.");
                    break;
            }
            
            // Ajuste de precios según nivel de juego
            GameObject controladorNivelesObj = GameObject.FindWithTag("GameController");
            if (controladorNivelesObj != null)
            {
                ControladorNiveles controladorNiveles = controladorNivelesObj.GetComponent<ControladorNiveles>();
                if (controladorNiveles != null)
                {
                    int nivelActual = controladorNiveles.nivelActual;
                    
                    // Pequeño incremento de precio basado en el nivel (5% por nivel)
                    if (nivelActual > 1)
                    {
                        float incrementoPorNivel = 0.05f * (nivelActual - 1);
                        objeto.precio = Mathf.RoundToInt(objeto.precio * (1 + incrementoPorNivel));
                    }
                }
            }
            
            Debug.Log($"Objeto configurado: {objeto.nombre} (ID: {objeto.id}), Precio: {objeto.precio}");
        }
    }
}
