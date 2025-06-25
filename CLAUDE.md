# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 3D restaurant service game where players work as a waiter in a fast-food restaurant. The game uses Unity's Universal Render Pipeline (URP) and the new Input System.

## Core Game Architecture

### Game Loop
1. Clients at tables request trays (visual alert appears)
2. Trays become available at the bar when requested
3. Player picks up trays from bar (backpack appears)
4. Player delivers trays to waiting tables (awards 1 point)
5. Random table selection continues the cycle

### Key Systems

**GameManager** (`Assets/GameManager.cs`)
- Central game controller managing the main loop
- Controls table assignment and player state
- References all TableManagers and BarManager

**TableManager** (`Assets/TableManager.cs`)
- Manages individual table states
- Shows visual alerts when waiting
- Handles tray delivery and scoring

**BarManager** (`Assets/BarManager.cs`)
- Singleton pattern for tray availability
- Manages player pickup interactions
- Controls backpack visibility

**ScoreManager** (`Assets/ScoreManager.cs`)
- Singleton for global score tracking
- Updates UI display

### Movement System

**SpeedManager** (`Assets/SpeedManager.cs`)
- Controls player acceleration (2.0 to 10.0 speed range)
- Exponential speed increase while moving

**Collision Speed Modifiers**:
- `CollisionSpeedReducer.cs` - For solid obstacles (tables)
- `SpeedObstacle.cs` - For trigger zones (banana peels)

## Unity-Specific Commands

### Common Unity Operations
```bash
# Scene management
mcp__unityMCP__manage_scene --action "load" --name "MainScene"
mcp__unityMCP__manage_scene --action "get_hierarchy"

# GameObject operations
mcp__unityMCP__manage_gameobject --action "find" --search_term "Player" --search_method "by_name"
mcp__unityMCP__manage_gameobject --action "find" --search_term "TableManager" --search_method "by_component"

# Editor control
mcp__unityMCP__manage_editor --action "play"
mcp__unityMCP__manage_editor --action "pause"
mcp__unityMCP__manage_editor --action "stop"

# Check console for errors
mcp__unityMCP__read_console --action "get" --types ["error", "warning"]
```

### Project Structure
- **Main Scene**: `Assets/Scenes/MainScene.unity`
- **Menu Scene**: `Assets/walid/Menu.unity`
- **End Scene**: `Assets/walid/End.unity`
- **Core Scripts**: `Assets/` (root level)
- **UI Scripts**: `Assets/walid/`
- **3D Models**: `Assets/FastFoodRestaurantKit/`

## Important Configuration

1. **Player Setup**: GameObject must be tagged as "Player"
2. **Scene Order**: Menu → MainScene → End (configured in Build Settings)
3. **Input System**: New Input System package required
4. **Singleton Access**: GameManager, BarManager, ScoreManager use singleton pattern

## Code Patterns

- Use `[SerializeField]` for inspector-exposed private fields
- Prefix private fields with underscore (e.g., `_speed`)
- Singletons use static Instance property
- Component references acquired via inspector assignment
- Collision detection uses both OnCollisionEnter and OnTriggerEnter

## Testing & Development

To test gameplay changes:
1. Load MainScene.unity
2. Press Play in Unity Editor
3. Use WASD/Arrow keys for movement
4. Monitor console for debug logs and errors

For script changes:
1. Unity will auto-compile on save
2. Check console for compilation errors
3. Test in Play mode before building