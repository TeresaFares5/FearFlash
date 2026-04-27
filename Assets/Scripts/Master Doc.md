## To Do
- [ ] Fill in master doc with info on scripts, characters, tools, etc.
- [ ] Redo pause screen
- [ ] Improve buttons for home screen and menus
- [ ] Decide where to take story after level 2
- [x] Remake voices

---

## Scripts

### Buttons
- **Button Click Handler**  
  Handles UI button interactions for menus. Plays a click sound and toggles UI panels on/off.  
  Each button is assigned a `panelIndex` which links to a panel in a shared `panels` array.  
  Only one panel can be open at a time — opening a new panel automatically closes the previous one.  
  Clicking the same button again will close its panel.  
  Pressing **Escape** will close any currently open panel.  

  **Setup:**
  - Attach script to each button  
  - Assign the same `panels` array to all buttons  
  - Set `panelIndex` per button (e.g. 0 = Controls, 1 = Levels)  
  - Assign a click sound  

  **Important:**  
  The button click should only be assigned once (either in code or in the Inspector).  
  Assigning it in both will cause the function to run twice and instantly close the panel.



- **Cutscene Over** 
  Automatically ends a cutscene after a set amount of time and transitions to the next scene.  
  Uses a coroutine to wait for `_cutsceneTime` seconds, then loads the next scene in the build order.  
  If the current scene is the final cutscene (build index 6), it instead returns to the Main Menu.

  **Setup:**
  - Attach to any GameObject in the cutscene scene  
  - Set `_cutsceneTime` to match the length of the cutscene  
  - Ensure scenes are correctly ordered in Build Settings  

  **Behaviour:**
  - Starts a timer on scene load  
  - After the timer ends:
    - Loads next scene (build index + 1)  
    - OR loads `"MainMenu"` if on the final cutscene  

  **Note:**  
  Relies on correct build index order — changing scene order may break flow.



- **Hover Button**  
  Shows or hides a UI element when the mouse hovers over a button.  
  Uses Unity’s event system (`IPointerEnterHandler` and `IPointerExitHandler`) to detect when the cursor enters or leaves the button area.

  **Setup:**
  - Attach to a UI Button (or any UI element with a Raycast target)  
  - Assign `objectToShow` (e.g. tooltip, image, text)  

  **Behaviour:**
  - Object is hidden on start  
  - When the cursor hovers over the button → object appears  
  - When the cursor leaves → object disappears  

  **Use Case:**  
  Useful for tooltips, hints, or highlighting UI elements on hover.


  
- **Levels Button**  
  Handles menu button actions for loading specific scenes and quitting the game.  
  Each public function is linked to a UI button and loads a corresponding scene using its name.

  **Setup:**
  - Attach to a GameObject (e.g. an empty object or menu manager)  
  - In each UI Button → On Click(), assign this script and select the desired function (e.g. `Tutorial()`, `Interior()`)  

  **Functions:**
  - `Tutorial()` → Loads the Tutorial scene  
  - `Interior()` → Loads the Interior (prologue) scene  
  - `Level2()` → Loads Level 2 (diner) scene  
  - `Cutscene()` → Loads the Cutscene scene  
  - `Quit()` → Exits the game (only works in build, not in editor)  

  **Behaviour:**
  - Instantly switches scenes when a button is pressed  
  - Logs "Quit" in console when quitting (for testing in editor)  

  **Note:**  
  Scene names must exactly match those in Build Settings or loading will fail.


---

### Enemy Scripts
- **Enemy AI**  
  Controls enemy behaviour using Unity’s NavMesh system.  
  The enemy continuously follows the player, but if hit by a camera flash, it retreats back to its spawn point before resuming the chase.

  **Setup:**
  - Attach to enemy GameObject  
  - Assign:
    - `player` (player GameObject)  
    - `agent` (NavMeshAgent component)  
    - `anim` (Animator with running animation)  
    - `spawn` (Transform for enemy reset position)  
    - Optional: `audioSource` and `enemySounds`  

  **Behaviour:**
  - Enemy constantly moves toward the player  
  - When hit by `"CameraFlash"`:
    - Switches target to spawn point  
    - Sets `hasHitTrigger = true`  
  - Once it reaches spawn:
    - Resets and starts chasing player again  
  - Running animation is always active  

  **Note:**  
  - Requires a baked NavMesh to function  
  - `CameraFlash` must have a collider with trigger enabled  
  - Direct position check (`==`) may be unreliable — distance checks are usually safer



- **Jumpscare 1**  
  Triggers a fast jumpscare when the player enters a trigger zone.  
  The zombie plays a running animation, moves briefly, plays a screech sound, shows a black screen, and quickly transitions to the next scene.

  **Setup:**
  - Attach to a trigger zone (Collider with “Is Trigger” enabled)  
  - Assign:
    - `zombie` (enemy GameObject)  
    - `zombieAnimator` (Animator with "IsRunning" bool)  
    - `screech` (AudioSource for jumpscare sound)  
    - `blackScreen` (UI object for instant black screen)  

  **Behaviour:**
  - When player enters trigger:
    - Zombie starts running animation  
    - Screech sound plays  
    - Zombie moves toward a fixed position  
    - After `_zombietime` → black screen appears  
    - After `_SceneChangetime` → loads `"Level2"`  

  **Notes:**  
  - Uses very short timers for a quick jumpscare effect  
  - Black screen is toggled instantly (no fade)  
  - Scene name must match Build Settings  

  

- **Light Flicker**  
  Creates a flickering light effect by repeatedly turning a light on and off at a set interval.

  **Setup:**
  - Attach to a GameObject (e.g. empty object or the lamp itself)  
  - Assign `lamp` (GameObject that has a Light component)  

  **Behaviour:**
  - On start:
    - Gets the Light component from the lamp  
    - Sets initial on/off state using `isLightOn`  
  - Every 0.5 seconds:
    - Toggles the light on/off  
    - Updates `isLightOn` to match  

  **Notes:**  
  - Flicker speed can be adjusted by changing `timer` reset value  
  - Works best for horror/atmospheric lighting effects  

---

### Interactions
- **Car Interaction Level 2**  
  Handles player interaction with the truck in Level 2.  
  Displays a prompt when the player looks at the truck and loads the end cutscene when interacted with.

  **Setup:**
  - Inherits from `Interactable` (requires base interaction system)  
  - Attach to the truck GameObject  
  - Assign `prompt` (UI element like “Press Q to enter”)  

  **Behaviour:**
  - When player looks at the truck (`OnFocus`):
    - Shows interaction prompt  
  - When player interacts (`OnInteract`):
    - Loads `"EndCutscene"` scene  
    - Unlocks and shows cursor  
  - When player looks away (`OnLoseFocus`):
    - Hides interaction prompt  

  **Notes:**  
  - Requires working `Interactable` system (raycast or similar)  
  - Scene name must match Build Settings  
  - Cursor is unlocked for menu/cutscene interaction after scene change  



- **Enter House**  
  Handles player interaction with a door to transition into the interior scene.  
  Plays a door sound and fade-out animation before loading the next scene.

  **Setup:**
  - Inherits from `Interactable` (requires base interaction system)  
  - Attach to the door GameObject  
  - Assign:
    - `prompt` (UI element like “Press Q to enter”)  
    - `fadeOut` (Animator with "EntersHouse" trigger)  
    - `door` (AudioSource for door sound)  

  **Behaviour:**
  - When player looks at the door (`OnFocus`):
    - Shows interaction prompt  
  - When player interacts (`OnInteract`):
    - Plays fade-out animation  
    - Plays door sound  
    - Waits `_enter` seconds  
    - Loads `"Interior"` scene  
  - When player looks away (`OnLoseFocus`):
    - Hides interaction prompt  

  **Notes:**  
  - Scene name must match Build Settings  
  - Fade timing (`_enter`) should match animation length  
  - `teleportTarget` and `thePlayer` are currently unused  


- **Interactable**  
  Abstract base class used for all interactable objects in the game.  
  Provides a shared structure for interaction behaviour such as focusing, interacting, and losing focus.

  **Setup:**
  - Inherit from this class when creating interactable objects (e.g. doors, cars, NPCs)  
  - Implement all required methods in child scripts:
    - `OnFocus()`  
    - `OnInteract()`  
    - `OnLoseFocus()`  

  **Behaviour:**
  - On Awake:
    - Automatically assigns the object to **layer 6** (used for interaction detection)  
  - Defines required interaction methods:
    - `OnFocus()` → when player looks at object  
    - `OnInteract()` → when player presses interact key  
    - `OnLoseFocus()` → when player looks away  

  **Notes:**  
  - Cannot be attached directly to objects (abstract class)  
  - Requires a separate system (e.g. raycasting from player) to call these functions  
  - Layer 6 should be reserved for interactable objects in the project  



- **Open Door**  
  Handles interaction with doors, allowing the player to open them with an animation and sound.  
  Displays a prompt when the player looks at a closed door.

  **Setup:**
  - Inherits from `Interactable`  
  - Attach to a door GameObject  
  - Assign:
    - `anim` (Animator with "DoorOpens" trigger)  
    - `prompt` (UI element like “Press Q to open”)  
    - `doorSound` (AudioSource for door sound)  

  **Behaviour:**
  - When player looks at the door (`OnFocus`):
    - Shows prompt only if the door is not already open  
  - When player interacts (`OnInteract`):
    - Plays door opening animation  
    - Plays door sound  
    - Marks door as open (`isDooropen = true`)  
  - When player looks away (`OnLoseFocus`):
    - Hides prompt  

  **Notes:**  
  - Door can only be opened once (no close functionality)  
  - Requires Animator with correct trigger setup  


---

### NPC
- **NPC Dialogue**  
  Triggers NPC dialogue when the player enters a specific area.  
  Plays an audio clip and displays on-screen text while the player is nearby.

  **Setup:**
  - Attach to an NPC or trigger zone (Collider with “Is Trigger” enabled)  
  - Assign:
    - `audioSource` (NPC voice/audio)  
    - `npcText` (UI element containing dialogue text)  

  **Behaviour:**
  - When player enters trigger:
    - Plays dialogue audio  
    - Shows dialogue text  
  - When player exits trigger:
    - Hides dialogue text  

  **Notes:**  
  - Player must have the "Player" tag  
  - `npcText` should be disabled by default  
  - Text content is managed separately (not set in this script)  



- **NPC Head Look**  
  Makes an NPC smoothly rotate to face the player.  
  Calculates the angle between the NPC and the player, then rotates toward them within a limited range.

  **Setup:**
  - Attach to the NPC (or head object)  
  - Assign `target` (player Transform)  

  **Behaviour:**
  - Continuously tracks player position  
  - Rotates toward player using smooth interpolation (`Slerp`)  
  - Rotation is limited using `minAngle` and `maxAngle`  

  **Notes:**  
  - Best used on a head bone for realistic movement  
  - Angle clamp prevents unnatural full rotation  
  - Runs every frame (`Update`)



- **Text Audio Player**  
  Displays a sequence of text lines with corresponding audio clips.  
  Each line is shown for a set duration, plays its audio, then automatically moves to the next.

  **Setup:**
  - Attach to a GameObject (e.g. UI manager or camera)  
  - Assign:
    - `text` (TextMeshProUGUI element)  
    - `audioSource` (AudioSource for dialogue)  
    - `objectToDisable` (UI or object to hide after each line)  
  - Populate `lines` array:
    - `text` (dialogue line)  
    - `clip` (audio for line)  
    - `color` (optional text color)  

  **Behaviour:**
  - On start:
    - Displays first line and plays audio  
  - For each line:
    - Shows text and sets color  
    - Plays associated audio clip  
    - Waits `displayTime` seconds  
  - After time expires:
    - Hides text and disables assigned object  
    - Moves to next line  
  - Stops after all lines are played  

  **Notes:**  
  - All lines use the same `displayTime` (not per-line timing)  
  - Text object is toggled on/off each cycle  
  - Useful for cutscenes, narration, or dialogue sequences


### Player Scripts

- **Controller**  
  Main first-person player controller that handles movement, looking, sprinting, stamina, jumping, crouching, head bob, footsteps, and object interaction.

  **Setup:**
  - Attach to the player GameObject  
  - Player must have a `CharacterController` component  
  - Assign:
    - Player camera  
    - Stamina UI image and canvas group  
    - Footstep AudioSource and footstep clips  
    - Interaction ray settings and interaction layer  

  **Behaviour:**
  - Locks and hides the cursor during gameplay  
  - Allows the player to walk, sprint, jump, crouch, and look around  
  - Uses stamina when sprinting and regenerates stamina when not sprinting  
  - Plays footstep sounds while moving  
  - Adds camera head bob while walking, sprinting, or crouching  
  - Uses raycasting to detect interactable objects  
  - Calls `OnFocus`, `OnInteract`, and `OnLoseFocus` from the `Interactable` system  

  **Controls:**
  - `Left Shift` → Sprint  
  - `Space` → Jump  
  - `Left Control` → Crouch  
  - `Q` → Interact  

  **Notes:**
  - Requires interactable objects to be on layer 6  
  - Works with scripts that inherit from `Interactable`  
  - Stamina and interaction settings can be adjusted in the Inspector  
  - `CanMove` and `CanLook` can be used to disable player control during menus, cutscenes, or jumpscares


- **Health System**  
  Tracks player health, updates health UI, plays damage effects, and triggers the death scene when health reaches zero.

  **Setup:**
  - Attach to the monster target on the player
  - Assign:
    - `fullHealth`, `halfHealth`, `noHealth` UI objects  
    - `bloodSplatterAnimator`  
    - `healthBar` Animator  
    - `damageSound` AudioSource  
    - `zombie` EnemyAi reference  

  **Behaviour:**
  - Player starts with 3 health  
  - When hit by `"zombie1"`:
    - Health decreases by 1  
    - Blood splatter animation plays  
    - Damage sound plays  
    - Zombie is sent back using `hasHitTrigger = true`  
  - Health UI changes based on remaining health  
  - At 0 health:
    - Loads `"JumpscareDeath"` scene  
    - Unlocks and shows cursor  

  **Notes:**
  - Enemy object must be named `"zombie1"`  
  - Death scene must be added to Build Settings  
  - Health bar animations require `"HitOnce"` and `"HitTwice"` triggers


- **Jumpscare Into Endscreen**  
  Automatically transitions from a jumpscare scene to the end screen after a set duration.

  **Setup:**
  - Attach to any GameObject in the jumpscare scene  
  - Set `_jumpscareLength` to match the length of the jumpscare  

  **Behaviour:**
  - On scene start:
    - Begins a timer  
  - After `_jumpscareLength` seconds:
    - Loads `"EndScreen"` scene  

  **Notes:**
  - Scene name must match Build Settings  
  - Runs automatically (no trigger required)  



- **Jumpscare Into Endscreen**  
  Automatically transitions from a jumpscare scene to the end screen after a set duration.

  **Setup:**
  - Attach to any GameObject in the jumpscare scene  
  - Set `_jumpscareLength` to match the length of the jumpscare  

  **Behaviour:**
  - On scene start:
    - Begins a timer  
  - After `_jumpscareLength` seconds:
    - Loads `"EndScreen"` scene  

  **Notes:**
  - Scene name must match Build Settings  
  - Runs automatically (no trigger required)  



- **Missing Poster**  
  Handles interaction with a collectible or disappearing object.  
  When the player touches it, the object disappears, plays a sound, and briefly displays a message on screen.

  **Setup:**
  - Attach to the object (e.g. poster) with a trigger collider  
  - Assign:
    - `soundEffect` (audio clip)  
    - `messageToShow` (text to display)  
    - Ensure a `TextMeshProUGUI` exists as a child  
    - Ensure an `AudioSource` is attached  

  **Behaviour:**
  - On player trigger:
    - Object is deactivated (disappears)  
    - Sound effect plays  
    - Message appears on screen  
  - After `messageDuration`:
    - Message is hidden  

  **Notes:**
  - Player must have "Player" tag  
  - Text object is hidden by default  
  - Useful for collectibles, clues, or story elements  



- **Pause Menu**  
  Controls pausing and unpausing the game, including UI, cursor state, and player input.  
  Also provides options to restart, quit, or return to the main menu.

  **Setup:**
  - Attach to a GameObject (e.g. UI manager)  
  - Assign:
    - `pauseMenu` (pause menu UI panel)  
    - `player` (Controller script reference)  

  **Behaviour:**
  - Press **Escape**:
    - Toggles pause menu on/off  
  - When paused:
    - `Time.timeScale = 0` (game freezes)  
    - Shows pause menu  
    - Unlocks and shows cursor  
    - Disables player look  
  - When unpaused:
    - `Time.timeScale = 1` (game resumes)  
    - Hides pause menu  
    - Locks and hides cursor  
    - Re-enables player look  

  **Extra Functions:**
  - `RestartGame()` → Reloads current scene  
  - `QuitGame()` → Exits game (works in build only)  
  - `GoToMainMenu()` → Loads `"MainMenu"` scene  

  **Notes:**
  - Requires working `Controller` script reference  
  - Time scale must always be reset to 1 when leaving pause  
  - Cursor state is managed for smooth UI interaction  



- **Photo Capture**  
  Main camera/photo system used for taking pictures of required objects, tracking objectives, showing captured photos, managing shots/reload, and triggering special horror events.

  **Setup:**
  - Attach to the player camera or camera system object  
  - Assign:
    - `photoDisplayArea` and `photoFrame`
    - `cameraFlash` and flash timing
    - Main camera
    - Required objects in `objectsToTakePicturesOf`
    - Objective UI text, shots text, reload UI, and prompts
    - Audio sources and animation references
    - Monster/hidden objects that appear during photo capture

  **Behaviour:**
  - Player left-clicks to take a photo  
  - Each photo:
    - Plays camera sound and flash
    - Temporarily shows hidden monsters/objects
    - Captures the screen into a photo frame
    - Uses raycast from screen centre to check photographed object
    - Removes completed objects from the objective list
    - Reduces shots left
  - Pressing **R** reloads when shots reach 0  
  - Pressing **E** hides/shows the objective list  
  - Special photographed objects can trigger sounds, animations, or door events  

  **Completion Logic:**
  - Tutorial complete → objective changes to “Enter House”
  - Level 2 complete → objective changes to “Enter Truck”
  - Interior progress → opens door after required photos are taken

  **Notes:**
  - Required objects must be assigned in `objectsToTakePicturesOf`
  - Object names are used for special events, so names must match exactly
  - Uses centre-screen raycast to detect photographed objects
  - Shots reset to 5 after reload
  - Hidden monsters are briefly enabled so they appear in captured photos