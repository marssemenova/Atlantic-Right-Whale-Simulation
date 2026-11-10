/**
 * StateManager.cs: Script which keeps track of states.
 *
 * @author Mars Semenova
 */

using UnityEngine;

public enum StateInput { // TODO: nuke
    ENABLED, // all input valid
    POPUP, // input disabled except back btn + popup btn
    NOUI, // input disabled except toggle UI btn
    IDLE // some input disabled
}

public enum StateUI {
    SHOW,
    HIDE
}

public enum StateHelpPopup {
    SHOW,
    HIDE
}

public enum StateIdle {
    ON,
    OFF
}

public class StateManager : MonoBehaviour {
    // states
    private static StateInput stateInput = StateInput.ENABLED;
    private static StateUI stateUI = StateUI.SHOW;
    private static StateHelpPopup stateHelpPopup = StateHelpPopup.HIDE;
    private static StateIdle stateIdle = StateIdle.OFF;
    
    void Start() {
        // sub
        PopupManager.OnHelpPopup += SetStatePopup; // TODO: nuke
        PopupManager.OnHelpPopup += SetStateHelpPopup;
        TogglesManager.OnToggleUI += SetStateNoUI; // TODO: nuke
        TogglesManager.OnToggleUI += SetStateUI;
        IdleUI.OnIdle += SetStateIdle;
    }
    
    private void OnDestroy() {
        // unsub
        PopupManager.OnHelpPopup -= SetStatePopup;
        TogglesManager.OnToggleUI -= SetStateNoUI;
        TogglesManager.OnToggleUI -= SetStateUI;
    }
    
    /**
     * Sets the current state to InputState.NOUI.
     * @param on - Whether the state should be in the no UI state or not.
     */
    private void SetStateNoUI(bool on) { // TODO: nuke
        stateInput = !on ? StateInput.NOUI : StateInput.ENABLED;
    }
    
    /**
     * Sets the current state to InputState.POPUP.
     * @param on - Whether the state should be in the popup state or not.
     */
    private void SetStatePopup(bool on) { // TODO: nuke
        stateInput = on ? StateInput.POPUP : StateInput.ENABLED;
    }
    
    // setters
    public static void SetStateUI(bool on) {
        stateUI = on ? StateUI.SHOW : StateUI.HIDE;
    } 
    public void SetStateIdle(bool on) { 
        stateInput = !on ? StateInput.NOUI : StateInput.ENABLED;
    }
    public static void SetStateHelpPopup(bool on) {
        stateUI = on ? StateUI.SHOW : StateUI.HIDE;
    } 
    
    // getters
    public static StateInput GetStateInput() { // TODO: nuke
        return stateInput;
    }
    public static StateUI GetStateUI() {
        return stateUI;
    }
    public static StateIdle GetStateIdle() {
        return stateIdle;
    }
}
