using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PanelMejorasNivel : MonoBehaviour
{
    [System.Serializable]
    public class OpcionMejora
    {
        public string nombre;
        public string descripcion;
        public int idHabilidad;
        public Sprite icono;
    }

    [Header("Referencias UI")]
    public Button[] botonesOpciones;
    public Image[] iconosOpciones;
    public TextMeshProUGUI[] nombresOpciones;
    public TextMeshProUGUI[] descripcionesOpciones;
    public Button botonSiguienteNivel;
    public TextMeshProUGUI textoMejorasDisponibles;

    [Header("Opciones de Mejora")]
    public List<OpcionMejora> todasLasMejoras = new List<OpcionMejora>();
    
    [Header("Configuración")]
    public int mejorasPorNivel = 1; // Cantidad de mejoras que puede seleccionar por nivel de experiencia
    
    private GestorHabilidades gestorHabilidades;
    private ControladorNiveles controladorNiveles;
    private BarraExperiencia barraExperiencia;
    private List<OpcionMejora> opcionesActuales = new List<OpcionMejora>();
    private int mejorasDisponibles = 0;
    private int mejorasSeleccionadas = 0;

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
                // Calcular las mejoras por el nivel actual, no acumulativamente
                mejorasDisponibles = mejorasPorNivel; // Una mejora por cada vez que suba de nivel
                Debug.Log("Nivel del jugador: " + nivelJugador + ", Mejoras disponibles: " + mejorasDisponibles);
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

    private void ActualizarTextoMejorasDisponibles()
    {
        if (textoMejorasDisponibles != null)
        {
            textoMejorasDisponibles.text = "Mejoras disponibles: " + (mejorasDisponibles - mejorasSeleccionadas);
        }
    }

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

    private void ContinuarASiguienteNivel()
    {
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