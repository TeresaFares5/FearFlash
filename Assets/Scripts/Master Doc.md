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
- **Car Interaction**  
  *(Add description)*

- **Car Interaction Level 2**  
  *(Add description)*

- **Enter House**  
  *(Add description)*

- **Interactable**  
  *(Add description)*

- **Open Door**  
  *(Add description)*

---

### NPC
- **NPC Dialogue**  
  *(Add description)*

- **NPC