/**
 * InputManager.cs: Script which manages input.
 *
 * @author Mars Semenova, Dany Diab
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    // params
    // actions 
    private static InputAction actionOpenHelp;
    private static InputAction actionOpenMenu;
    private static InputAction actionPause;
    private static InputAction actionBack;
    private static InputAction actionCam1;
    private static InputAction actionCam2;
    private static InputAction actionCam3;
    private static InputAction actionToggleUI;
    private static InputAction actionCamLock;
    private static InputAction actionReset;
    private static InputAction actionAccelerate; 
    private static InputAction actionMove;
    private static InputAction actionLook;
    private static InputAction actionUpDown;
    private static InputAction actionOpenMouth;
    private static InputAction actionSpeedUp;
    
    // input
    private static bool inputOpenHelp;
    private static bool inputOpenMenu;
    private static bool inputPause;
    private static bool inputBack;
    private static bool inputCam1;
    private static bool inputCam2;
    private static bool inputCam3;
    private static bool inputToggleUI;
    private static bool inputCamLock;
    private static bool inputReset;
    private static float inputAccelerate; 
    private static Vector2 inputMove;
    private static Vector2 inputLook;
    private static Vector2 inputUpDown;
    private static float inputOpenMouth;
    private static bool inputSpeedUp;
    
    // vars
    private static StateInput input;
    private static StateCamera cam;
    private static StateUI ui;
    private static StateIdle idle;
    private static StateMode mode;

    void Start() {
        // set up actions
        actionOpenHelp = InputSystem.actions.FindAction("OpenHelp");
        actionOpenMenu = InputSystem.actions.FindAction("OpenMenu"); 
        actionBack = InputSystem.actions.FindAction("BackAction");
        actionPause = InputSystem.actions.FindAction("Pause");
        actionCam1 = InputSystem.actions.FindAction("Cam1");
        actionCam2 = InputSystem.actions.FindAction("Cam2");
        actionCam3 = InputSystem.actions.FindAction("Cam3");
        actionToggleUI = InputSystem.actions.FindAction("ToggleUI");
        actionCamLock = InputSystem.actions.FindAction("CamLock");
        actionReset = InputSystem.actions.FindAction("Reset");
        actionMove = InputSystem.actions.FindAction("Move");
        actionLook = InputSystem.actions.FindAction("Look");
        actionUpDown = InputSystem.actions.FindAction("Up/Down");
        actionAccelerate = InputSystem.actions.FindAction("Accelerate"); // TODO: implement feedback
        actionOpenMouth = InputSystem.actions.FindAction("OpenMouth");
        actionSpeedUp = InputSystem.actions.FindAction("SimulationSpeedUp");

        mode = SceneManager.GetStateMode();
    }

    void Update() {
        // fetch states
        cam = CameraManager.GetStateCamera();
        input = StateManager.GetStateInput();
        ui = StateManager.GetStateUI();
        idle = StateManager.GetStateIdle();
        
        if (ButtonPressUtil.Pressed(actionOpenHelp) && input != StateInput.NOUI)  {
            UIManager.OnHelp();
        }
        if (ButtonPressUtil.Pressed(actionOpenMenu) && input != StateInput.POPUP && input != StateInput.NOUI) {
            UIManager.OnScenarios();
        }
        
        if (ButtonPressUtil.Pressed(actionBack))  {
            UIManager.OnBack();
        }
        
        // TODO: refactor whaleController to SceneManager.mode
        if (ButtonPressUtil.Pressed(actionPause) && mode != StateMode.FREESWIM && input != StateInput.POPUP)  { // paused disabled in free roam
            UIManager.OnTogglePause();
        }
        
        if (ButtonPressUtil.Pressed(actionCam1) && input != StateInput.POPUP) {
            UIManager.OnCameraStateChange(StateCamera.ORBIT);
        }
        if (ButtonPressUtil.Pressed(actionCam2) && mode != StateMode.FREESWIM && input != StateInput.POPUP) { // cam 2 disabled in free roam
            UIManager.OnCameraStateChange(StateCamera.FREE);
        }
        if (ButtonPressUtil.Pressed(actionCam3) && input != StateInput.POPUP) {
            UIManager.OnCameraStateChange(StateCamera.POV);
        }
        
        if (ButtonPressUtil.Pressed(actionToggleUI) && input != StateInput.POPUP) {
            UIManager.OnToggleUI(!(StateManager.GetStateUI() == StateUI.SHOW));
        }
       
        if (ButtonPressUtil.Pressed(actionCamLock) && mode != StateMode.FREESWIM && (cam == StateCamera.ORBIT || cam == StateCamera.FREE) && input != StateInput.POPUP) { // rot lock in cam 1
            UIManager.OnCamLock();
        }
        
        if (ButtonPressUtil.Pressed(actionReset) && input != StateInput.POPUP) {
            UIManager.OnReset();
        }
        
        // TODO
        inputMove = actionMove?.ReadValue<Vector2>() ?? Vector2.zero;
        inputLook = actionLook?.ReadValue<Vector2>() ?? Vector2.zero;
        inputUpDown = actionUpDown?.ReadValue<Vector2>() ?? Vector2.zero;
        inputAccelerate = actionAccelerate?.ReadValue<float>() ?? 0f;
        inputSpeedUp = ButtonPressUtil.Pressed(actionSpeedUp); // TODO
        inputOpenMouth = actionOpenMouth?.ReadValue<float>() ?? 0.0f;
    }
    
    // getters
    public static float GetInputAccelerate() {
        return inputAccelerate;
    }
    public static Vector2 GetInputMove() {
        return inputMove;
    }
    public static Vector2 GetInputLook() {
        return inputLook;
    }
    public static Vector2 GetInputUpDown() {
        return inputUpDown;
    }
    public static float GetInputOpenMouth() { 
        return inputOpenMouth;
    }
}