using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Gestiona el panel de selección de mejoras al subir de nivel.
/// </summary>
/// <remarks>
/// Esta clase se encarga de mostrar opciones de mejora aleatorias cuando el jugador 
/// sube de nivel, permitiendo al jugador elegir una o varias mejoras para su personaje.
/// Controla la visualización del panel, la generación de opciones aleatorias, y la
/// aplicación de las mejoras seleccionadas a través del GestorHabilidades.
/// </remarks>
public class PanelMejorasNivel : MonoBehaviour
{
    /// <summary>
    /// Define una opción de mejora que se puede mostrar en el panel.
    /// </summary>
    [System.Serializable]
    public class OpcionMejora
    {
        /// <summary>
        /// Nombre de la mejora que se mostrará en la interfaz.
        /// </summary>
        public string nombre;
        /// <summary>
        /// Descripción detallada de la mejora que explica su efecto.
        /// </summary>
        public string descripcion;
        /// <summary>
        /// ID único que identifica la mejora para su aplicación.
        /// </summary>
        public int idHabilidad;
        /// <summary>
        /// Icono visual que representa la mejora en la interfaz.
        /// </summary>
        public Sprite icono;
    }

    [Header("Referencias UI")]
    /// <summary>
    /// Botones que representan las diferentes opciones de mejora.
    /// </summary>
    public Button[] botonesOpciones;
    /// <summary>
    /// Imágenes donde se mostrarán los iconos de las mejoras.
    /// </summary>
    public Image[] iconosOpciones;
    /// <summary>
    /// Textos que mostrarán los nombres de las mejoras.
    /// </summary>
    public TextMeshProUGUI[] nombresOpciones;
    /// <summary>
    /// Textos que mostrarán las descripciones detalladas de las mejoras.
    /// </summary>
    public TextMeshProUGUI[] descripcionesOpciones;
    /// <summary>
    /// Botón para confirmar y continuar al siguiente nivel después de seleccionar mejoras.
    /// </summary>
    public Button botonSiguienteNivel;
    /// <summary>
    /// Texto que muestra cuántas mejoras puede seleccionar el jugador.
    /// </summary>
    public TextMeshProUGUI textoMejorasDisponibles;

    [Header("Opciones de Mejora")]
    /// <summary>
    /// Lista completa de todas las mejoras posibles que pueden aparecer.
    /// </summary>
    public List<OpcionMejora> todasLasMejoras = new List<OpcionMejora>();
    
    [Header("Configuración")]
    /// <summary>
    /// Cantidad de mejoras que el jugador puede seleccionar por nivel de experiencia.
    /// </summary>
    public int mejorasPorNivel = 1; // Cantidad de mejoras que puede seleccionar por nivel de experiencia
    
    /// <summary>
    /// Referencia al gestor de habilidades del jugador.
    /// </summary>
    private GestorHabilidades gestorHabilidades;
    /// <summary>
    /// Referencia al controlador de niveles del juego.
    /// </summary>
    private ControladorNiveles controladorNiveles;
    /// <summary>
    /// Referencia a la barra de experiencia del jugador.
    /// </summary>
    private BarraExperiencia barraExperiencia;
    /// <summary>
    /// Lista de opciones de mejora actualmente mostradas en el panel.
    /// </summary>
    private List<OpcionMejora> opcionesActuales = new List<OpcionMejora>();
    /// <summary>
    /// Número total de mejoras disponibles para seleccionar en esta sesión.
    /// </summary>
    private int mejorasDisponibles = 0;
    /// <summary>
    /// Número de mejoras ya seleccionadas por el jugador.
    /// </summary>
    private int mejorasSeleccionadas = 0;
    /// <summary>
    /// Último nivel de experiencia donde se aplicaron mejoras.
    /// </summary>
    private int ultimoNivelProcesado = 0; // Último nivel de experiencia donde se aplicaron mejoras

    /// <summary>
    /// Inicializa los componentes y configura los botones del panel.
    /// </summary>
    private void Awake()
    {
        // Desactivar el panel al inicio
        gameObject.SetActive(false);
        
        // Buscar referencias necesarias
        controladorNiveles = FindAnyObjectByType<ControladorNiveles>();
        
        // Configurar botones
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            int index = i; // Capturar el índice para el lambda
            botonesOpciones[i].onClick.AddListener(() => SeleccionarMejora(index));
        }
        
        // Configurar botón siguiente nivel
        botonSiguienteNivel.onClick.AddListener(ContinuarASiguienteNivel);
        botonSiguienteNivel.gameObject.SetActive(false); // Ocultar hasta que se elija una mejora
    }

    /// <summary>
    /// Muestra el panel de mejoras, pausa el juego y genera opciones aleatorias.
    /// </summary>
    /// <remarks>
    /// Este método se llama cuando el jugador sube de nivel y debe seleccionar sus mejoras.
    /// Calcula cuántas mejoras están disponibles según el nivel alcanzado, configura el panel
    /// y muestra las opciones generadas aleatoriamente.
    /// </remarks>
    public void MostrarPanel()
    {
        Debug.Log("PanelMejorasNivel.MostrarPanel() llamado");
        
        // Pausar el juego al mostrar el panel
        Time.timeScale = 0;
        
        // Buscar el gestor de habilidades en el jugador
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null)
        {
            Debug.Log("Jugador encontrado con tag 'Jugador'");
            
            if (gestorHabilidades == null)
            {
                gestorHabilidades = jugador.GetComponent<GestorHabilidades>();
                if (gestorHabilidades != null)
                    Debug.Log("GestorHabilidades encontrado en el jugador");
                else
                    Debug.LogError("GestorHabilidades NO encontrado en el jugador");
            }
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con tag 'Jugador'");
        }
        
        // Buscar la barra de experiencia en cualquier objeto de la escena
        if (barraExperiencia == null)
        {
            // Primero intentar en el jugador
            if (jugador != null)
            {
                barraExperiencia = jugador.GetComponent<BarraExperiencia>();
                if (barraExperiencia != null)
                    Debug.Log("BarraExperiencia encontrada en el jugador");
            }
            
            // Si no está en el jugador, buscar en todos los objetos
            if (barraExperiencia == null)
            {
                barraExperiencia = FindAnyObjectByType<BarraExperiencia>();
                if (barraExperiencia != null)
                    Debug.Log("BarraExperiencia encontrada en otro objeto de la escena");
                else
                    Debug.LogWarning("BarraExperiencia NO encontrada en ningún objeto de la escena");
            }
        }
        
        if (gestorHabilidades == null)
        {
            Debug.LogError("No se encontró el GestorHabilidades");
            // Reanudar el juego ya que vamos a salir del método
            Time.timeScale = 1;
            return;
        }
        
        // Calcular mejoras disponibles basado en el nivel de experiencia del jugador
        if (barraExperiencia != null)
        {
            try
            {
                int nivelJugador = barraExperiencia.ObtenerNivelActual();
                // Calcular las mejoras como la diferencia entre nivel actual y el último nivel procesado
                mejorasDisponibles = nivelJugador - ultimoNivelProcesado; // Solo las mejoras nuevas desde la última vez
                Debug.Log("Nivel del jugador: " + nivelJugador + ", Último nivel procesado: " + ultimoNivelProcesado + ", Mejoras disponibles: " + mejorasDisponibles);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al obtener el nivel del jugador: " + e.Message);
                mejorasDisponibles = mejorasPorNivel; // Solo dar mejoras por nivel actual
                Debug.LogWarning("Usando nivel por defecto: mejorasPorNivel");
            }
        }
        else
        {
            // Fallback si no se encuentra la barra de experiencia
            mejorasDisponibles = mejorasPorNivel; // Solo dar mejoras por nivel actual
            Debug.LogWarning("No se encontró la BarraExperiencia, usando mejoras por defecto: mejorasPorNivel");
        }
        
        // Asegurar que al menos haya 1 mejora disponible
        if (mejorasDisponibles <= 0)
        {
            mejorasDisponibles = 1;
            Debug.LogWarning("Corregido: Estableciendo al menos 1 mejora disponible");
        }
        
        mejorasSeleccionadas = 0;
        
        // Actualizar texto de mejoras disponibles
        ActualizarTextoMejorasDisponibles();
        
        // Activar el panel
        gameObject.SetActive(true);
        Debug.Log("Panel de mejoras activado");
        
        botonSiguienteNivel.gameObject.SetActive(false);
        
        // Generar opciones aleatorias
        GenerarOpcionesAleatorias();
        
        // Configurar la UI de las opciones
        ConfigurarOpcionesUI();
    }

    /// <summary>
    /// Actualiza el texto que muestra cuántas mejoras puede seleccionar el jugador.
    /// </summary>
    private void ActualizarTextoMejorasDisponibles()
    {
        if (textoMejorasDisponibles != null)
        {
            textoMejorasDisponibles.text = "Mejoras disponibles: " + (mejorasDisponibles - mejorasSeleccionadas);
        }
    }

    /// <summary>
    /// Genera un conjunto aleatorio de opciones de mejora para mostrar al jugador.
    /// </summary>
    /// <remarks>
    /// Selecciona aleatoriamente mejoras de la lista completa de todas las mejoras,
    /// excluyendo ciertas mejoras específicas que no deben aparecer en este panel.
    /// </remarks>
    private void GenerarOpcionesAleatorias()
    {
        // Limpiar opciones actuales
        opcionesActuales.Clear();
        
        // Crear una copia de la lista de mejoras para no modificar la original
        List<OpcionMejora> mejorasDisponibles = new List<OpcionMejora>(todasLasMejoras);
        Debug.Log($"Total de mejoras antes de filtrar: {mejorasDisponibles.Count}");
        
        // IMPORTANTE: Siempre eliminar la mejora de calaveras x2 (ID 10) de las opciones de mejora por nivel
        // Esta mejora solo debe estar disponible en la tienda
        int mejoraDeCalaveraSRemovidas = mejorasDisponibles.RemoveAll(mejora => mejora.idHabilidad == 10);
        Debug.Log($"Mejoras de calaveras removidas del selector: {mejoraDeCalaveraSRemovidas}");
        
        Debug.Log($"Total de mejoras después de filtrar: {mejorasDisponibles.Count}");
        
        // Comprobar si hay mejoras disponibles después del filtrado
        if (mejorasDisponibles.Count == 0)
        {
            Debug.LogWarning("No hay mejoras disponibles para mostrar después del filtrado.");
            return;
        }
        
        // Seleccionar opciones aleatorias
        int numOpcionesAMostrar = Mathf.Min(botonesOpciones.Length, mejorasDisponibles.Count);
        
        for (int i = 0; i < numOpcionesAMostrar; i++)
        {
            // Elegir una mejora aleatoria
            int indiceAleatorio = Random.Range(0, mejorasDisponibles.Count);
            OpcionMejora mejoraSeleccionada = mejorasDisponibles[indiceAleatorio];
            opcionesActuales.Add(mejoraSeleccionada);
            Debug.Log($"Mejora seleccionada: {mejoraSeleccionada.nombre} (ID: {mejoraSeleccionada.idHabilidad})");
            
            // Remover la mejora seleccionada para no repetirla
            mejorasDisponibles.RemoveAt(indiceAleatorio);
        }
    }

    /// <summary>
    /// Configura la interfaz de usuario con las opciones de mejora seleccionadas.
    /// </summary>
    /// <remarks>
    /// Asigna las mejoras generadas aleatoriamente a los botones correspondientes,
    /// configurando textos, iconos y visibilidad de los elementos de la interfaz.
    /// </remarks>
    private void ConfigurarOpcionesUI()
    {
        // Configurar cada botón con la mejora correspondiente
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (i < opcionesActuales.Count)
            {
                // Hay una opción disponible para este botón
                OpcionMejora mejora = opcionesActuales[i];
                
                // Configurar texto e icono
                nombresOpciones[i].text = mejora.nombre;
                descripcionesOpciones[i].text = mejora.descripcion;
                
                if (mejora.icono != null)
                {
                    iconosOpciones[i].sprite = mejora.icono;
                }
                
                // Activar el botón
                botonesOpciones[i].gameObject.SetActive(true);
                botonesOpciones[i].interactable = true;
            }
            else
            {
                // No hay opción para este botón, ocultarlo
                botonesOpciones[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Procesa la selección de una mejora por parte del jugador.
    /// </summary>
    /// <param name="indice">Índice del botón seleccionado en el array de botones.</param>
    /// <remarks>
    /// Aplica la mejora seleccionada, actualiza el contador de mejoras disponibles,
    /// y determina si mostrar más opciones o el botón para continuar al siguiente nivel.
    /// </remarks>
    private void SeleccionarMejora(int indice)
    {
        if (indice >= 0 && indice < opcionesActuales.Count)
        {
            // Aplicar la mejora seleccionada
            int idHabilidad = opcionesActuales[indice].idHabilidad;
            gestorHabilidades.AplicarHabilidadPorID(idHabilidad);
            
            // Incrementar contador de mejoras seleccionadas
            mejorasSeleccionadas++;
            
            // Actualizar texto de mejoras disponibles
            ActualizarTextoMejorasDisponibles();
            
            // Si ya seleccionó todas las mejoras disponibles
            if (mejorasSeleccionadas >= mejorasDisponibles)
            {
                // Desactivar todos los botones de mejoras
                foreach (Button boton in botonesOpciones)
                {
                    boton.interactable = false;
                }
                
                // Mostrar el botón para continuar
                botonSiguienteNivel.gameObject.SetActive(true);
            }
            else
            {
                // Todavía hay mejoras disponibles, generar nuevas opciones
                GenerarOpcionesAleatorias();
                ConfigurarOpcionesUI();
            }
        }
    }

    /// <summary>
    /// Continúa al siguiente nivel después de seleccionar todas las mejoras.
    /// </summary>
    /// <remarks>
    /// Guarda el nivel de experiencia actual como el último procesado,
    /// cierra el panel de mejoras y muestra la tienda a través del controlador de niveles.
    /// </remarks>
    public void ContinuarASiguienteNivel()
    {
        // Guardar el nivel actual como el último procesado
        if (barraExperiencia != null)
        {
            ultimoNivelProcesado = barraExperiencia.ObtenerNivelActual();
            Debug.Log("Guardando último nivel procesado: " + ultimoNivelProcesado);
        }
        
        // Ocultar este panel
        gameObject.SetActive(false);
        
        // No reanudar el juego aquí, ya que la tienda también pausará el juego
        
        // Mostrar la tienda
        if (controladorNiveles != null)
        {
            controladorNiveles.MostrarTienda();
        }
    }
} 