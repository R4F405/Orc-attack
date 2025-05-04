# 🗡️ Orc Attack

## 📖 **Descripción**
**Orc Attack** es un juego rogue-like de supervivencia donde deberás enfrentarte a hordas de enemigos mientras recolectas calaveras para comprar mejoras. Con una jugabilidad dinámica y desafiante, tendrás que mejorar tus habilidades y armas para superar niveles cada vez más difíciles.

---

## 🎯 **Características Principales**

- 🔄 **Sistema de niveles progresivos** - Cada nivel aumenta en duración y dificultad
- ⚔️ **Combate dinámico** - Enfrenta enemigos tanto cuerpo a cuerpo como a distancia
- 🛒 **Tienda y sistema de mejoras** - Gasta tus calaveras recolectadas en nuevas armas y habilidades
- 📈 **Sistema de experiencia** - Gana XP eliminando enemigos para desbloquear mejoras
- 🎮 **Controles intuitivos** - Movimiento fluido y sistema de ataque automático
- 🔊 **Sistema de audio completo** - Música adaptativa y efectos de sonido inmersivos
- 🎨 **Estética pixel art** - Gráficos retro de estilo roguelike

---

## 🚀 **Instalación**
1. Descarga el archivo `OrcAttack.zip` desde el repositorio
2. Extrae el contenido del archivo ZIP
3. Ejecuta el archivo `.exe` incluido para iniciar el juego en Windows

---

## 🎮 **Modo de Juego**

### Mecánicas Básicas
- Sobrevive a oleadas de enemigos durante un tiempo limitado por nivel
- Recoge calaveras de los enemigos derrotados para usarlas como moneda
- Al finalizar cada nivel, accede al panel de mejoras y luego a la tienda
- Adquiere hasta 5 armas diferentes y múltiples objetos con habilidades

### Progresión
- Los niveles se vuelven más largos y desafiantes a medida que avanzas
- El sistema de experiencia te permite desbloquear mejoras en cada nivel
- Combina armas a distancia y cuerpo a cuerpo para una estrategia efectiva

### Sistema de Sonido
- **Música adaptativa**:
  - Diferentes pistas para cada escena del juego
  - Sistema de transición entre músicas
  - Control de volumen personalizable
- **Efectos de juego**:
  - Sonido distintivo al recoger calaveras
  - Feedback auditivo cuando el jugador recibe daño
  - Sonido especial al completar un nivel
  - Pasos del personaje al moverse
- **Sonidos de UI**:
  - Clicks de botones
  - Sonido de compra exitosa
  - Alerta de error cuando no es posible realizar una acción

---

## 🖼️ **Imágenes**

### Menú principal
![Menú Principal](Imagenes/Menu.png)

### Gameplay
![Gameplay](Imagenes/Juego.png)

---

## 🧩 **Características Detalladas**

### Sistema de Combate
- **Armas a distancia**: 
  - Disparan proyectiles que buscan automáticamente a los enemigos
  - Daño moderado con mayor alcance
  - Probabilidad de crítico y robo de vida

- **Armas cuerpo a cuerpo**: 
  - Atacan en un radio cercano con mayor daño
  - Posibilidad de golpear a varios enemigos a la vez
  - Sistema de rotación automática hacia el enemigo más cercano

### Enemigos
- **Orcos cuerpo a cuerpo**: Se acercan para atacar directamente
- **Magos a distancia**: Lanzan proyectiles desde lejos
- **Tanques**: Enemigos con gran resistencia y daño mejorado
- **Sistema generativo**: Generadores automatizados que crean oleadas progresivas

### Sistema de Experiencia
- Barra de experiencia que se llena al eliminar enemigos
- Al subir de nivel, acceso a panel de mejoras con opciones aleatorias
- Cada nivel requiere más experiencia que el anterior

### Habilidades y Mejoras
- **Mejoras de jugador**:
  - Aumento de vida máxima
  - Reducción del tiempo entre recuperaciones de vida
  - Multiplicador de calaveras recolectadas

- **Mejoras de armas**:
  - Aumento de daño general, melee o a distancia
  - Aumento de probabilidad de crítico
  - Aumento de probabilidad de robo de vida
  - Reducción de tiempo de recarga

- **Mejoras de objetos**:
  - Reducción del tiempo de generación de cajas

### Sistema de Objetos
- **Calaveras**: Moneda principal del juego, dropeada por enemigos
- **Cajas**: Objetos destructibles que pueden contener recursos
- **Pociones de vida**: Recuperan un porcentaje de la vida del jugador

---

## 🔧 **Tecnologías Utilizadas**
- **Motor**: Unity 2022.3 o superior
- **Lenguaje**: C#
- **Gráficos**: Sprites 2D con estilo pixel art
- **Audio**: Sistema de audio completo con música y efectos
  - Gestor de audio global para control centralizado
  - Sistema de música adaptativa por escena
  - Componentes AudioSource/AudioClip para efectos localizados
  - Sistema centralizado para sonidos UI (GestorSonidosUI)
- **Documentación**: Todos los scripts incluyen documentación XML completa
  - Comentarios XML estándar de C# (`///`) para clases, métodos y propiedades
  - Documentación de parámetros con etiquetas `<param>` y `<returns>`
  - Descripciones detalladas con `<remarks>` según sea necesario
  - Facilita la generación automática de documentación y ayuda en el IDE
  - Ademas se incluye algun comentario basico (`//`) para pequeñas aclariaciones

---

## 🛠️ **Estado del Proyecto**
El proyecto está en desarrollo activo con las siguientes características ya implementadas:
- ✅ Sistema completo de niveles y oleadas
- ✅ Tienda funcional con múltiples mejoras
- ✅ Sistema de combate con diferentes tipos de armas
- ✅ Sistema de experiencia y progresión
- ✅ Sistema de música adaptativa y efectos de sonido
- ✅ Menús de navegación y pausa
- ✅ Variedad de enemigos con distintos comportamientos

Próximas mejoras:
- ⏳ Más tipos de enemigos y jefes finales
- ⏳ Sistema de guardado de partida
- ⏳ Logros y desbloqueos
- ⏳ Nuevos tipos de armas y habilidades

---

## 🔢 **Controles**
- **WASD** o **Flechas de dirección**: Movimiento del personaje
- **ESC**: Menú de pausa
  - Acceso a opciones y sonido
  - Volver al menú principal
- **Click Izquierdo del Ratón**: Interacción en menús y tienda
  - Selección de mejoras
  - Compra de artículos
  - Navegación por los diferentes paneles

---

## 🙅 **Contribuciones**
Por el momento, no se aceptan contribuciones externas.

---

## 📜 **Licencia**
Este proyecto está licenciado bajo la **GPL 3**.

---

## 📬 **Contacto**
Para dudas o colaboraciones, contacta a través del repositorio oficial en Git.

---

## 🎨 **Créditos**

### - Desarrollo: R4F405

- **Arte**: 
  - [momongaa](https://momongaa.itch.io/roguelite-dungeon-tileset) - roguelite-dungeon-tileset
  - [Free Game Assets GUI, Sprite, Tilesets](https://free-game-assets.itch.io/48-free-rpg-loot-icons-pixel-art) - 48-free-rpg-loot-icons-pixel-art
  - [Seth](https://sethbb.itch.io/32rogues) - 32rogues
  - [Emma](https://enma1890.itch.io/throwknight) - throwknight
  - [BDragon1727](https://bdragon1727.itch.io/fire-pixel-bullet-16x16) - fire-pixel-bullet-16x16

- **Efectos de sonido y Música**: Consulta el archivo [Assets/Creditos.txt](Assets/Creditos.txt) para la lista completa de atribuciones


