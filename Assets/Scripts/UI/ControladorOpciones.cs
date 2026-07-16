using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Controlador de opciones gráficas y de rendimiento.
/// Aplica resolución, calidad, modo de pantalla, límite de FPS y VSync.
/// Compatible con Unity 6 (RefreshRate).
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

    // Aplicar al instante al cambiar dropdowns/toggles (no depende de serialización de escena).
    private const bool AplicarAlCambiar = true;

    private readonly List<Resolution> resoluciones = new List<Resolution>();
    private readonly int[] opcionesFPS = { -1, 30, 60, 120, 144, 240 };

    private bool inicializado;
    private bool bloqueandoListeners;

    /// <summary>
    /// Aplica preferencias guardadas al arrancar, aunque el menú de opciones no esté activo.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AplicarPreferenciasAlInicio()
    {
        if (PlayerPrefs.HasKey("NivelCalidad"))
        {
            int nivel = PlayerPrefs.GetInt("NivelCalidad");
            if (nivel >= 0 && nivel < QualitySettings.names.Length)
            {
                QualitySettings.SetQualityLevel(nivel, false);
            }
        }

        bool vsync = !PlayerPrefs.HasKey("VSync") || PlayerPrefs.GetInt("VSync") == 1;
        QualitySettings.vSyncCount = vsync ? 1 : 0;

        int fps = -1;
        if (!vsync && PlayerPrefs.HasKey("LimiteFPS"))
        {
            int indice = PlayerPrefs.GetInt("LimiteFPS");
            int[] opciones = { -1, 30, 60, 120, 144, 240 };
            if (indice >= 0 && indice < opciones.Length)
            {
                fps = opciones[indice];
            }
        }

        Application.targetFrameRate = fps;

        if (PlayerPrefs.HasKey("ResolucionWidth") && PlayerPrefs.HasKey("ResolucionHeight"))
        {
            int width = PlayerPrefs.GetInt("ResolucionWidth");
            int height = PlayerPrefs.GetInt("ResolucionHeight");
            FullScreenMode modo = FullScreenMode.FullScreenWindow;

            if (PlayerPrefs.HasKey("ModoVentana"))
            {
                switch (PlayerPrefs.GetInt("ModoVentana"))
                {
                    case 0: modo = FullScreenMode.ExclusiveFullScreen; break;
                    case 1: modo = FullScreenMode.FullScreenWindow; break;
                    default: modo = FullScreenMode.Windowed; break;
                }
            }

            int hz = PlayerPrefs.GetInt("ResolucionHz", 60);
            var refresh = new RefreshRate
            {
                numerator = (uint)Mathf.Max(1, hz),
                denominator = 1u
            };

            Screen.SetResolution(width, height, modo, refresh);
        }
    }

    private void Awake()
    {
        InicializarOpciones();
        CargarPreferenciasEnUI();
        AplicarConfiguracion();
        inicializado = true;
    }

    private void Start()
    {
        ConfigurarListeners();
    }

    private void OnEnable()
    {
        if (!inicializado) return;

        bloqueandoListeners = true;
        SincronizarUIConEstadoActual();
        CargarPreferenciasEnUI();
        bloqueandoListeners = false;
    }

    private void InicializarOpciones()
    {
        InicializarDropdownResolucion();
        InicializarDropdownCalidad();
        InicializarDropdownModoVentana();
        InicializarDropdownFPS();
        SincronizarToggleVSync();
    }

    private void InicializarDropdownResolucion()
    {
        if (dropdownResolucion == null) return;

        resoluciones.Clear();
        Resolution[] soportadas = Screen.resolutions;
        var vistas = new HashSet<string>();

        for (int i = 0; i < soportadas.Length; i++)
        {
            Resolution r = soportadas[i];
            string clave = $"{r.width}x{r.height}@{ObtenerHz(r)}";
            if (!vistas.Add(clave)) continue;
            resoluciones.Add(r);
        }

        if (resoluciones.Count == 0)
        {
            resoluciones.Add(Screen.currentResolution);
        }

        var opciones = new List<string>(resoluciones.Count);
        int indiceActual = resoluciones.Count - 1;

        for (int i = 0; i < resoluciones.Count; i++)
        {
            Resolution r = resoluciones[i];
            opciones.Add($"{r.width}x{r.height} @ {ObtenerHz(r)}Hz");

            if (EsMismaResolucion(r, Screen.width, Screen.height, Screen.currentResolution))
            {
                indiceActual = i;
            }
        }

        dropdownResolucion.ClearOptions();
        dropdownResolucion.AddOptions(opciones);
        dropdownResolucion.SetValueWithoutNotify(indiceActual);
        dropdownResolucion.RefreshShownValue();
    }

    private void InicializarDropdownCalidad()
    {
        if (dropdownCalidad == null) return;

        dropdownCalidad.ClearOptions();
        dropdownCalidad.AddOptions(new List<string>(QualitySettings.names));
        dropdownCalidad.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        dropdownCalidad.RefreshShownValue();
    }

    private void InicializarDropdownModoVentana()
    {
        if (dropdownModoVentana == null) return;

        dropdownModoVentana.ClearOptions();
        dropdownModoVentana.AddOptions(new List<string>
        {
            "Pantalla Completa",
            "Ventana Maximizada",
            "Ventana Normal"
        });

        dropdownModoVentana.SetValueWithoutNotify(ObtenerIndiceModoVentana(Screen.fullScreenMode));
        dropdownModoVentana.RefreshShownValue();
    }

    private void InicializarDropdownFPS()
    {
        if (dropdownFPS == null) return;

        var opciones = new List<string>(opcionesFPS.Length);
        foreach (int fps in opcionesFPS)
        {
            opciones.Add(fps == -1 ? "Sin límite" : fps + " FPS");
        }

        dropdownFPS.ClearOptions();
        dropdownFPS.AddOptions(opciones);

        int frameRateActual = Application.targetFrameRate;
        int indice = 0;
        for (int i = 0; i < opcionesFPS.Length; i++)
        {
            if (frameRateActual == opcionesFPS[i])
            {
                indice = i;
                break;
            }
        }

        dropdownFPS.SetValueWithoutNotify(indice);
        dropdownFPS.RefreshShownValue();
    }

    private void SincronizarToggleVSync()
    {
        if (toggleVSync == null) return;
        toggleVSync.SetIsOnWithoutNotify(QualitySettings.vSyncCount > 0);
    }

    private void SincronizarUIConEstadoActual()
    {
        if (dropdownCalidad != null)
        {
            dropdownCalidad.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
            dropdownCalidad.RefreshShownValue();
        }

        if (dropdownModoVentana != null)
        {
            dropdownModoVentana.SetValueWithoutNotify(ObtenerIndiceModoVentana(Screen.fullScreenMode));
            dropdownModoVentana.RefreshShownValue();
        }

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.SetIsOnWithoutNotify(Screen.fullScreenMode != FullScreenMode.Windowed);
        }

        SincronizarToggleVSync();

        if (dropdownFPS != null)
        {
            int frameRateActual = Application.targetFrameRate;
            int indice = 0;
            for (int i = 0; i < opcionesFPS.Length; i++)
            {
                if (frameRateActual == opcionesFPS[i])
                {
                    indice = i;
                    break;
                }
            }

            dropdownFPS.SetValueWithoutNotify(indice);
            dropdownFPS.RefreshShownValue();
        }
    }

    private void ConfigurarListeners()
    {
        if (botonAplicar != null)
        {
            botonAplicar.onClick.RemoveListener(AplicarConfiguracion);
            botonAplicar.onClick.AddListener(AplicarConfiguracion);
        }

        if (botonRestaurar != null)
        {
            botonRestaurar.onClick.RemoveListener(RestaurarValoresPredeterminados);
            botonRestaurar.onClick.AddListener(RestaurarValoresPredeterminados);
        }

        if (dropdownResolucion != null)
        {
            dropdownResolucion.onValueChanged.RemoveListener(OnCambioOpcion);
            dropdownResolucion.onValueChanged.AddListener(OnCambioOpcion);
        }

        if (dropdownCalidad != null)
        {
            dropdownCalidad.onValueChanged.RemoveListener(OnCambioOpcion);
            dropdownCalidad.onValueChanged.AddListener(OnCambioOpcion);
        }

        if (dropdownModoVentana != null)
        {
            dropdownModoVentana.onValueChanged.RemoveListener(OnCambioModoVentana);
            dropdownModoVentana.onValueChanged.AddListener(OnCambioModoVentana);
        }

        if (dropdownFPS != null)
        {
            dropdownFPS.onValueChanged.RemoveListener(OnCambioFPS);
            dropdownFPS.onValueChanged.AddListener(OnCambioFPS);
        }

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.onValueChanged.RemoveListener(CambiarModoVentana);
            togglePantallaCompleta.onValueChanged.AddListener(CambiarModoVentana);
        }

        if (toggleVSync != null)
        {
            toggleVSync.onValueChanged.RemoveListener(OnCambioVSync);
            toggleVSync.onValueChanged.AddListener(OnCambioVSync);
        }
    }

    private void OnCambioOpcion(int _)
    {
        if (bloqueandoListeners || !AplicarAlCambiar) return;
        AplicarConfiguracion();
    }

    private void OnCambioModoVentana(int indice)
    {
        if (bloqueandoListeners) return;

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.SetIsOnWithoutNotify(indice != 2);
        }

        if (AplicarAlCambiar)
        {
            AplicarConfiguracion();
        }
    }

    private void OnCambioFPS(int indice)
    {
        if (bloqueandoListeners) return;

        // Un límite concreto de FPS requiere VSync desactivado.
        if (indice > 0 && toggleVSync != null && toggleVSync.isOn)
        {
            toggleVSync.SetIsOnWithoutNotify(false);
        }

        if (AplicarAlCambiar)
        {
            AplicarConfiguracion();
        }
    }

    private void OnCambioVSync(bool activar)
    {
        if (bloqueandoListeners) return;

        // Con VSync activo el limitador de FPS no tiene efecto.
        if (activar && dropdownFPS != null && dropdownFPS.value > 0)
        {
            dropdownFPS.SetValueWithoutNotify(0);
            dropdownFPS.RefreshShownValue();
        }

        if (AplicarAlCambiar)
        {
            AplicarConfiguracion();
        }
        else
        {
            QualitySettings.vSyncCount = activar ? 1 : 0;
            GuardarPreferencias();
        }
    }

    /// <summary>
    /// Aplica la configuración gráfica y de rendimiento seleccionada.
    /// </summary>
    public void AplicarConfiguracion()
    {
        FullScreenMode modo = ObtenerModoVentanaSeleccionado();

        if (dropdownResolucion != null && resoluciones.Count > 0)
        {
            int indice = Mathf.Clamp(dropdownResolucion.value, 0, resoluciones.Count - 1);
            Resolution resolucion = resoluciones[indice];
            Screen.SetResolution(resolucion.width, resolucion.height, modo, resolucion.refreshRateRatio);
        }
        else
        {
            Screen.fullScreenMode = modo;
        }

        if (dropdownCalidad != null && QualitySettings.names.Length > 0)
        {
            int nivel = Mathf.Clamp(dropdownCalidad.value, 0, QualitySettings.names.Length - 1);
            // false: no aplicar el vSync del preset; lo controlamos nosotros después.
            QualitySettings.SetQualityLevel(nivel, false);
        }

        bool vsyncActivo = toggleVSync != null && toggleVSync.isOn;
        QualitySettings.vSyncCount = vsyncActivo ? 1 : 0;

        if (dropdownFPS != null && dropdownFPS.value >= 0 && dropdownFPS.value < opcionesFPS.Length)
        {
            // Con VSync, Unity ignora targetFrameRate.
            Application.targetFrameRate = vsyncActivo ? -1 : opcionesFPS[dropdownFPS.value];
        }

        GuardarPreferencias();

        Debug.Log(
            $"[Opciones] Aplicado -> {Screen.width}x{Screen.height} | modo={Screen.fullScreenMode} | " +
            $"calidad={QualitySettings.names[QualitySettings.GetQualityLevel()]} | " +
            $"fps={Application.targetFrameRate} | vSync={QualitySettings.vSyncCount}");
    }

    public void CambiarModoVentana(bool pantallaCompleta)
    {
        if (bloqueandoListeners) return;

        if (dropdownModoVentana != null)
        {
            dropdownModoVentana.SetValueWithoutNotify(pantallaCompleta ? 0 : 2);
            dropdownModoVentana.RefreshShownValue();
        }

        if (AplicarAlCambiar)
        {
            AplicarConfiguracion();
        }
    }

    public void CambiarVSync(bool activar)
    {
        OnCambioVSync(activar);
    }

    public void RestaurarValoresPredeterminados()
    {
        bloqueandoListeners = true;

        if (dropdownResolucion != null && resoluciones.Count > 0)
        {
            dropdownResolucion.SetValueWithoutNotify(resoluciones.Count - 1);
            dropdownResolucion.RefreshShownValue();
        }

        if (dropdownCalidad != null && QualitySettings.names.Length > 0)
        {
            dropdownCalidad.SetValueWithoutNotify(QualitySettings.names.Length - 1);
            dropdownCalidad.RefreshShownValue();
        }

        if (dropdownModoVentana != null)
        {
            dropdownModoVentana.SetValueWithoutNotify(0);
            dropdownModoVentana.RefreshShownValue();
        }

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.SetIsOnWithoutNotify(true);
        }

        if (dropdownFPS != null)
        {
            dropdownFPS.SetValueWithoutNotify(0);
            dropdownFPS.RefreshShownValue();
        }

        if (toggleVSync != null)
        {
            toggleVSync.SetIsOnWithoutNotify(true);
        }

        bloqueandoListeners = false;
        AplicarConfiguracion();
    }

    private void GuardarPreferencias()
    {
        if (dropdownResolucion != null)
        {
            PlayerPrefs.SetInt("ResolucionIndex", dropdownResolucion.value);
            if (resoluciones.Count > 0)
            {
                int i = Mathf.Clamp(dropdownResolucion.value, 0, resoluciones.Count - 1);
                PlayerPrefs.SetInt("ResolucionWidth", resoluciones[i].width);
                PlayerPrefs.SetInt("ResolucionHeight", resoluciones[i].height);
                PlayerPrefs.SetInt("ResolucionHz", ObtenerHz(resoluciones[i]));
            }
        }

        if (dropdownCalidad != null)
        {
            PlayerPrefs.SetInt("NivelCalidad", dropdownCalidad.value);
        }

        if (dropdownModoVentana != null)
        {
            PlayerPrefs.SetInt("ModoVentana", dropdownModoVentana.value);
        }
        else if (togglePantallaCompleta != null)
        {
            PlayerPrefs.SetInt("PantallaCompleta", togglePantallaCompleta.isOn ? 1 : 0);
        }

        if (dropdownFPS != null)
        {
            PlayerPrefs.SetInt("LimiteFPS", dropdownFPS.value);
        }

        if (toggleVSync != null)
        {
            PlayerPrefs.SetInt("VSync", toggleVSync.isOn ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void CargarPreferenciasEnUI()
    {
        bloqueandoListeners = true;

        if (dropdownResolucion != null && resoluciones.Count > 0)
        {
            int indice = -1;

            if (PlayerPrefs.HasKey("ResolucionWidth") && PlayerPrefs.HasKey("ResolucionHeight"))
            {
                int w = PlayerPrefs.GetInt("ResolucionWidth");
                int h = PlayerPrefs.GetInt("ResolucionHeight");
                int hz = PlayerPrefs.GetInt("ResolucionHz", -1);

                for (int i = 0; i < resoluciones.Count; i++)
                {
                    if (resoluciones[i].width == w &&
                        resoluciones[i].height == h &&
                        (hz < 0 || ObtenerHz(resoluciones[i]) == hz))
                    {
                        indice = i;
                        break;
                    }
                }
            }

            if (indice < 0 && PlayerPrefs.HasKey("ResolucionIndex"))
            {
                indice = PlayerPrefs.GetInt("ResolucionIndex");
            }

            if (indice >= 0 && indice < resoluciones.Count)
            {
                dropdownResolucion.SetValueWithoutNotify(indice);
                dropdownResolucion.RefreshShownValue();
            }
        }

        if (dropdownCalidad != null && PlayerPrefs.HasKey("NivelCalidad"))
        {
            int nivel = PlayerPrefs.GetInt("NivelCalidad");
            if (nivel >= 0 && nivel < QualitySettings.names.Length)
            {
                dropdownCalidad.SetValueWithoutNotify(nivel);
                dropdownCalidad.RefreshShownValue();
            }
        }

        if (dropdownModoVentana != null && PlayerPrefs.HasKey("ModoVentana"))
        {
            int modo = PlayerPrefs.GetInt("ModoVentana");
            if (modo >= 0 && modo <= 2)
            {
                dropdownModoVentana.SetValueWithoutNotify(modo);
                dropdownModoVentana.RefreshShownValue();
            }
        }
        else if (togglePantallaCompleta != null && PlayerPrefs.HasKey("PantallaCompleta"))
        {
            togglePantallaCompleta.SetIsOnWithoutNotify(PlayerPrefs.GetInt("PantallaCompleta") == 1);
        }

        if (dropdownFPS != null && PlayerPrefs.HasKey("LimiteFPS"))
        {
            int indice = PlayerPrefs.GetInt("LimiteFPS");
            if (indice >= 0 && indice < opcionesFPS.Length)
            {
                dropdownFPS.SetValueWithoutNotify(indice);
                dropdownFPS.RefreshShownValue();
            }
        }

        if (toggleVSync != null && PlayerPrefs.HasKey("VSync"))
        {
            toggleVSync.SetIsOnWithoutNotify(PlayerPrefs.GetInt("VSync") == 1);
        }

        bloqueandoListeners = false;
    }

    public void AplicarResolucion(int index)
    {
        if (index < 0 || index >= resoluciones.Count) return;

        if (dropdownResolucion != null)
        {
            dropdownResolucion.SetValueWithoutNotify(index);
            dropdownResolucion.RefreshShownValue();
        }

        AplicarConfiguracion();
    }

    private FullScreenMode ObtenerModoVentanaSeleccionado()
    {
        if (dropdownModoVentana != null)
        {
            switch (dropdownModoVentana.value)
            {
                case 0: return FullScreenMode.ExclusiveFullScreen;
                case 1: return FullScreenMode.FullScreenWindow;
                default: return FullScreenMode.Windowed;
            }
        }

        if (togglePantallaCompleta != null)
        {
            return togglePantallaCompleta.isOn
                ? FullScreenMode.FullScreenWindow
                : FullScreenMode.Windowed;
        }

        return Screen.fullScreenMode;
    }

    private static int ObtenerIndiceModoVentana(FullScreenMode modo)
    {
        switch (modo)
        {
            case FullScreenMode.ExclusiveFullScreen: return 0;
            case FullScreenMode.FullScreenWindow: return 1;
            default: return 2;
        }
    }

    private static int ObtenerHz(Resolution resolucion)
    {
        double valor = resolucion.refreshRateRatio.value;
        if (double.IsNaN(valor) || valor <= 0d)
        {
            return 60;
        }

        return Mathf.RoundToInt((float)valor);
    }

    private static bool EsMismaResolucion(Resolution candidata, int ancho, int alto, Resolution actual)
    {
        return candidata.width == ancho &&
               candidata.height == alto &&
               ObtenerHz(candidata) == ObtenerHz(actual);
    }
}
 