using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Controlador para las opciones gráficas y de rendimiento del juego.
/// Permite ajustar resolución, calidad gráfica, modo de pantalla y limitación de FPS.
/// También gestiona el guardado y carga de preferencias del usuario.
/// </summary>
public class ControladorOpciones : MonoBehaviour
{
    [Header("Opciones de Resolución")]
    [SerializeField] private TMP_Dropdown dropdownResolucion;
    [SerializeField] private Toggle togglePantallaCompleta;
    [SerializeField] private TMP_Dropdown dropdownModoVentana;

    [Header("Opciones de Calidad")]
    [SerializeField] private TMP_Dropdown dropdownCalidad;

    [Header("Limitador de FPS")]
    [SerializeField] private TMP_Dropdown dropdownFPS;
    [SerializeField] private Toggle toggleVSync;

    [Header("Referencias")]
    [SerializeField] private Button botonAplicar;
    [SerializeField] private Button botonRestaurar;

    // Lista de resoluciones disponibles
    private Resolution[] resoluciones;
    // Guarda la configuración para poder restaurarla si se cancela
    private Resolution resolucionActual;
    private int calidadActual;
    private FullScreenMode modoVentanaActual;
    private int fpsLimiteActual;
    private bool vSyncActual;

    // Opciones de limitador de FPS
    private readonly int[] opcionesFPS = { -1, 30, 60, 120, 144, 240 };

    /// <summary>
    /// Inicializa los controles de UI y carga la configuración actual del sistema.
    /// </summary>
    private void Start()
    {
        // Inicializar los componentes UI
        InicializarOpciones();

        // Cargar la configuración actual
        CargarConfiguracionActual();

        // Configurar listeners para los controles
        ConfigurarListeners();
    }

    /// <summary>
    /// Inicializa todos los controles de opciones gráficas y de rendimiento.
    /// </summary>
    private void InicializarOpciones()
    {
        // Inicializar dropdown de resoluciones
        InicializarDropdownResolucion();

        // Inicializar dropdown de calidad
        InicializarDropdownCalidad();

        // Inicializar dropdown de modo ventana
        InicializarDropdownModoVentana();

        // Inicializar dropdown de límite de FPS
        InicializarDropdownFPS();
    }

    /// <summary>
    /// Configura el dropdown de resoluciones con todas las resoluciones soportadas por el sistema.
    /// </summary>
    private void InicializarDropdownResolucion()
    {
        if (dropdownResolucion == null) return;

        // Obtener todas las resoluciones disponibles
        resoluciones = Screen.resolutions;
        List<string> opcionesResolucion = new List<string>();
        int resolucionActualIndex = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = $"{resoluciones[i].width} x {resoluciones[i].height} @ {resoluciones[i].refreshRate}Hz";
            opcionesResolucion.Add(opcion);

            // Encontrar la resolución actual
            if (resoluciones[i].width == Screen.width && 
                resoluciones[i].height == Screen.height &&
                resoluciones[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                resolucionActualIndex = i;
            }
        }

        // Limpiar opciones actuales y añadir nuevas
        dropdownResolucion.ClearOptions();
        dropdownResolucion.AddOptions(opcionesResolucion);
        dropdownResolucion.value = resolucionActualIndex;
        dropdownResolucion.RefreshShownValue();
    }

    /// <summary>
    /// Configura el dropdown de calidad con los niveles de calidad predefinidos de Unity.
    /// </summary>
    private void InicializarDropdownCalidad()
    {
        if (dropdownCalidad == null) return;

        // Obtener niveles de calidad de Unity
        string[] nombresCalidad = QualitySettings.names;
        List<string> opcionesCalidad = new List<string>(nombresCalidad);

        dropdownCalidad.ClearOptions();
        dropdownCalidad.AddOptions(opcionesCalidad);
        dropdownCalidad.value = QualitySettings.GetQualityLevel();
        dropdownCalidad.RefreshShownValue();
    }

    /// <summary>
    /// Configura el dropdown con los diferentes modos de ventana disponibles
    /// (pantalla completa exclusiva, ventana maximizada o ventana normal).
    /// </summary>
    private void InicializarDropdownModoVentana()
    {
        if (dropdownModoVentana == null) return;

        // Opciones de modo ventana
        List<string> opcionesModoVentana = new List<string>
        {
            "Pantalla Completa",
            "Ventana Maximizada",
            "Ventana Normal"
        };

        dropdownModoVentana.ClearOptions();
        dropdownModoVentana.AddOptions(opcionesModoVentana);

        // Seleccionar la opción basada en el modo actual
        int modoActual = 0;
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                modoActual = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                modoActual = 1;
                break;
            case FullScreenMode.Windowed:
                modoActual = 2;
                break;
        }

        dropdownModoVentana.value = modoActual;
        dropdownModoVentana.RefreshShownValue();
    }

    /// <summary>
    /// Configura el dropdown con las opciones de limitación de FPS y el toggle de VSync.
    /// </summary>
    private void InicializarDropdownFPS()
    {
        if (dropdownFPS == null) return;

        List<string> opcionesFPSTexto = new List<string>();
        foreach (int fps in opcionesFPS)
        {
            opcionesFPSTexto.Add(fps == -1 ? "Sin límite" : fps.ToString() + " FPS");
        }

        dropdownFPS.ClearOptions();
        dropdownFPS.AddOptions(opcionesFPSTexto);

        // Encontrar el valor más cercano al framerate actual
        int frameRateActual = Application.targetFrameRate;
        int indiceSeleccionado = 0;

        for (int i = 0; i < opcionesFPS.Length; i++)
        {
            if (frameRateActual == opcionesFPS[i] || 
                (i == 0 && frameRateActual == -1))
            {
                indiceSeleccionado = i;
                break;
            }
            // Si no hay coincidencia exacta, usar el primer valor
            indiceSeleccionado = 0;
        }

        dropdownFPS.value = indiceSeleccionado;
        dropdownFPS.RefreshShownValue();

        // Inicializar el toggle de VSync
        if (toggleVSync != null)
        {
            toggleVSync.isOn = QualitySettings.vSyncCount > 0;
        }
    }

    /// <summary>
    /// Guarda en memoria la configuración actual para poder restaurarla si es necesario.
    /// </summary>
    private void CargarConfiguracionActual()
    {
        // Guardar configuración actual
        resolucionActual = Screen.currentResolution;
        calidadActual = QualitySettings.GetQualityLevel();
        modoVentanaActual = Screen.fullScreenMode;
        fpsLimiteActual = Application.targetFrameRate;
        vSyncActual = QualitySettings.vSyncCount > 0;

        // Actualizar toggles
        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.isOn = Screen.fullScreenMode != FullScreenMode.Windowed;
        }
    }

    /// <summary>
    /// Configura los eventos para los controles de UI (botones, toggles, etc).
    /// </summary>
    private void ConfigurarListeners()
    {
        // Añadir listeners a botones
        if (botonAplicar != null)
        {
            botonAplicar.onClick.AddListener(AplicarConfiguracion);
        }

        if (botonRestaurar != null)
        {
            botonRestaurar.onClick.AddListener(RestaurarValoresPredeterminados);
        }

        // Toggle pantalla completa
        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.onValueChanged.AddListener(CambiarModoVentana);
        }

        // Toggle VSync
        if (toggleVSync != null)
        {
            toggleVSync.onValueChanged.AddListener(CambiarVSync);
        }
    }

    /// <summary>
    /// Aplica la configuración gráfica y de rendimiento seleccionada en los controles de UI.
    /// También guarda las preferencias del usuario para futuras sesiones.
    /// </summary>
    public void AplicarConfiguracion()
    {
        // Aplicar resolución
        if (dropdownResolucion != null && resoluciones.Length > 0)
        {
            Resolution resolucion = resoluciones[dropdownResolucion.value];
            
            // Aplicar modo de ventana
            FullScreenMode modo = FullScreenMode.Windowed;
            if (dropdownModoVentana != null)
            {
                switch (dropdownModoVentana.value)
                {
                    case 0:
                        modo = FullScreenMode.ExclusiveFullScreen;
                        break;
                    case 1:
                        modo = FullScreenMode.FullScreenWindow;
                        break;
                    case 2:
                        modo = FullScreenMode.Windowed;
                        break;
                }
            }
            else if (togglePantallaCompleta != null)
            {
                modo = togglePantallaCompleta.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            }

            // Aplicar resolución y modo de pantalla
            Screen.SetResolution(resolucion.width, resolucion.height, modo, resolucion.refreshRate);
        }

        // Aplicar calidad
        if (dropdownCalidad != null)
        {
            QualitySettings.SetQualityLevel(dropdownCalidad.value, true);
        }

        // Aplicar límite de FPS
        if (dropdownFPS != null && dropdownFPS.value < opcionesFPS.Length)
        {
            Application.targetFrameRate = opcionesFPS[dropdownFPS.value];
        }

        // Guardar preferencias
        GuardarPreferencias();

        Debug.Log("Configuración aplicada correctamente");
    }

    /// <summary>
    /// Actualiza el dropdown de modo ventana cuando se cambia el toggle de pantalla completa.
    /// </summary>
    /// <param name="pantallaCompleta">True si está en pantalla completa, false si está en modo ventana</param>
    public void CambiarModoVentana(bool pantallaCompleta)
    {
        if (dropdownModoVentana != null)
        {
            // Si hay dropdown de modo ventana, sincronizar con el toggle
            dropdownModoVentana.value = pantallaCompleta ? 0 : 2;
        }
    }

    /// <summary>
    /// Activa o desactiva la sincronización vertical (VSync).
    /// </summary>
    /// <param name="activar">True para activar VSync, false para desactivarlo</param>
    public void CambiarVSync(bool activar)
    {
        QualitySettings.vSyncCount = activar ? 1 : 0;
    }

    /// <summary>
    /// Restaura todas las opciones a sus valores predeterminados óptimos:
    /// - Resolución más alta disponible
    /// - Calidad gráfica máxima
    /// - Pantalla completa
    /// - Sin límite de FPS
    /// - VSync activado
    /// </summary>
    public void RestaurarValoresPredeterminados()
    {
        // Restaurar resolución a la más alta disponible
        if (dropdownResolucion != null && resoluciones.Length > 0)
        {
            dropdownResolucion.value = resoluciones.Length - 1;
        }

        // Restaurar calidad a la máxima
        if (dropdownCalidad != null)
        {
            dropdownCalidad.value = QualitySettings.names.Length - 1;
        }

        // Restaurar modo ventana a pantalla completa
        if (dropdownModoVentana != null)
        {
            dropdownModoVentana.value = 0;
        }

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.isOn = true;
        }

        // Restaurar FPS a sin límite
        if (dropdownFPS != null)
        {
            dropdownFPS.value = 0; // Sin límite
        }

        // Activar VSync
        if (toggleVSync != null)
        {
            toggleVSync.isOn = true;
        }

        // Aplicar cambios
        AplicarConfiguracion();
    }

    /// <summary>
    /// Guarda las preferencias de configuración del usuario en PlayerPrefs para recuperarlas en futuras sesiones.
    /// </summary>
    private void GuardarPreferencias()
    {
        // Guardar índice de resolución
        if (dropdownResolucion != null)
        {
            PlayerPrefs.SetInt("ResolucionIndex", dropdownResolucion.value);
        }

        // Guardar calidad
        if (dropdownCalidad != null)
        {
            PlayerPrefs.SetInt("NivelCalidad", dropdownCalidad.value);
        }

        // Guardar modo ventana
        if (dropdownModoVentana != null)
        {
            PlayerPrefs.SetInt("ModoVentana", dropdownModoVentana.value);
        }
        else if (togglePantallaCompleta != null)
        {
            PlayerPrefs.SetInt("PantallaCompleta", togglePantallaCompleta.isOn ? 1 : 0);
        }

        // Guardar límite de FPS
        if (dropdownFPS != null)
        {
            PlayerPrefs.SetInt("LimiteFPS", dropdownFPS.value);
        }

        // Guardar VSync
        if (toggleVSync != null)
        {
            PlayerPrefs.SetInt("VSync", toggleVSync.isOn ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carga las preferencias guardadas previamente por el usuario desde PlayerPrefs.
    /// </summary>
    private void CargarPreferencias()
    {
        // Cargar índice de resolución
        if (dropdownResolucion != null && PlayerPrefs.HasKey("ResolucionIndex"))
        {
            int indice = PlayerPrefs.GetInt("ResolucionIndex");
            if (indice >= 0 && indice < resoluciones.Length)
            {
                dropdownResolucion.value = indice;
            }
        }

        // Cargar calidad
        if (dropdownCalidad != null && PlayerPrefs.HasKey("NivelCalidad"))
        {
            int nivel = PlayerPrefs.GetInt("NivelCalidad");
            if (nivel >= 0 && nivel < QualitySettings.names.Length)
            {
                dropdownCalidad.value = nivel;
            }
        }

        // Cargar modo ventana
        if (dropdownModoVentana != null && PlayerPrefs.HasKey("ModoVentana"))
        {
            int modo = PlayerPrefs.GetInt("ModoVentana");
            if (modo >= 0 && modo <= 2)
            {
                dropdownModoVentana.value = modo;
            }
        }
        else if (togglePantallaCompleta != null && PlayerPrefs.HasKey("PantallaCompleta"))
        {
            togglePantallaCompleta.isOn = PlayerPrefs.GetInt("PantallaCompleta") == 1;
        }

        // Cargar límite de FPS
        if (dropdownFPS != null && PlayerPrefs.HasKey("LimiteFPS"))
        {
            int indice = PlayerPrefs.GetInt("LimiteFPS");
            if (indice >= 0 && indice < opcionesFPS.Length)
            {
                dropdownFPS.value = indice;
            }
        }

        // Cargar VSync
        if (toggleVSync != null && PlayerPrefs.HasKey("VSync"))
        {
            toggleVSync.isOn = PlayerPrefs.GetInt("VSync") == 1;
        }
    }

    /// <summary>
    /// Carga las preferencias guardadas cuando se activa el panel de opciones.
    /// </summary>
    private void OnEnable()
    {
        // Cargar preferencias cuando se active el panel
        CargarPreferencias();
    }

    /// <summary>
    /// Aplica una resolución específica basada en su índice en la lista de resoluciones disponibles.
    /// </summary>
    /// <param name="index">Índice de la resolución en el array de resoluciones</param>
    public void AplicarResolucion(int index)
    {
        if (resoluciones == null || resoluciones.Length == 0 || index < 0 || index >= resoluciones.Length) return;

        Screen.SetResolution(
            resoluciones[index].width,
            resoluciones[index].height,
            Screen.fullScreen,
            resoluciones[index].refreshRate
        );

        PlayerPrefs.SetInt("ResolucionIndex", index);
        PlayerPrefs.Save();
    }
} 