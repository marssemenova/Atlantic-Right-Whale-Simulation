using UnityEngine;

public enum StateCamera {
    ORBIT,
    FREE,
    POV,
}

public class CameraManager : MonoBehaviour{
    [Header("Free Cam")]
    [SerializeField] private float moveSpeedRef = 25f;
    private static float moveSpeed;
    [SerializeField] private float fastSpeedRef = 35f;
    private static float fastSpeed;
    [SerializeField] private float sensitivityRef = 1.5f;
    private static float sensitivity;


    [Header("Orbit")]
    [SerializeField] private Transform orbitTargetRef;
    private static Transform orbitTarget;

    [SerializeField] private float orbitDistanceRef = 20f;
    private static float orbitDistance;

    [SerializeField] private float orbitSensitivityRef = 1.5f;
    private static float orbitSensitivity;

    [Header("POV")]
    [SerializeField] private GameObject POVTargetRef;
    private static GameObject POVTarget;

    // input
    Vector2 moveInput;
    Vector2 lookInput;
    Vector2 upDownInput;
    bool sprinting = false; // TODO?
    
    float yaw;
    float pitch;
    static bool rotationLocked;
    static bool freeCamReset;
    
    private static StateCamera state;
    
    public delegate void CamSwitchEvent(int index); // TODO: should be passing CameraState or smth instead for uniformity 

    public static event CamSwitchEvent OnCamSwitch;

    static bool forceLock;

    private void Awake() {
        // transfer editor input to vars
        moveSpeed = moveSpeedRef;
        fastSpeed = fastSpeedRef;
        sensitivity = sensitivityRef;
        orbitTarget = orbitTargetRef;
        orbitDistance = orbitDistanceRef;
        orbitSensitivity = orbitSensitivityRef;
        POVTarget = POVTargetRef;
    }

    void Start(){
        // starting pitch and yaw based on the current state of camera
        yaw   = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    public static void SetForceLock(bool on){
        forceLock = on;
    }

   public static void lockUnLockCamera() {
        rotationLocked = !rotationLocked;
    }


    public static void changeToCam(int index) {
        rotationLocked = false;
        switch (index) {
            case 1:
                state = StateCamera.ORBIT;
                break;
            case 2:
                state = StateCamera.FREE;
                break;
            case 3:
                state = StateCamera.POV;
                break;
        }
        
        OnCamSwitch?.Invoke(index);
    }

    void Update() {
        if(forceLock) return;
        
        moveInput = InputManager.GetInputMove();
        lookInput = InputManager.GetInputLook();
        upDownInput = InputManager.GetInputUpDown();
        
        switch (state) {
            case StateCamera.ORBIT: {
                UpdateOrbit();
                break;
            }
            case StateCamera.FREE: {
                UpdateFreeCam();
                // if the free cam reset is pressed, set pos/rot to pov cam
                if (freeCamReset){
                    UpdatePOVCam();
                    freeCamReset = false;       
                }
                break;
            }
            case StateCamera.POV: {
                UpdatePOVCam();
                break;
            }
        }
    }


    private void UpdatePOVCam() {
        transform.position = POVTarget.transform.position;
        transform.rotation = Quaternion.LookRotation(POVTarget.transform.parent.rotation * Vector3.forward);
    }

    private void UpdateFreeCam(){
        // rotate with look input 
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // move with move input and up/down input (for vertical movement)
        float speed = sprinting ? fastSpeed : moveSpeed;
        Vector3 move =  transform.forward * moveInput.y +
                        transform.right * moveInput.x +
                        transform.up * upDownInput.y;
        transform.position += move * speed * Time.unscaledDeltaTime; // TODO: use smth else bc time is set to 0 on pause 
    }
    
    private void UpdateOrbit(){
        // If no target, do nothing
        if (orbitTarget == null) return;

        // rotate with look input around the target

        float rotate = rotationLocked ? 0.0f : 1.0f;
        yaw += lookInput.x * orbitSensitivity * rotate;
        pitch -= lookInput.y * orbitSensitivity * rotate;
        pitch = Mathf.Clamp(pitch, -80, 80);
        
        // calculate zoom based on up/down input (basically how close to the target))
        float zoom   = upDownInput.y * moveSpeed * Time.unscaledDeltaTime;
        
    
        // apply zoom to base orbit distance
        orbitDistance -= zoom;
        // take the max of orbit to prevent from going through the target
        orbitDistance  = Mathf.Max(5f, orbitDistance);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);

        transform.position = orbitTarget.position - rot * Vector3.forward * orbitDistance;
        
        if(rotationLocked) return;
        
        transform.rotation = rot;
    }


    public static void resetFreeCam() {
        freeCamReset = true;
    }

    public static StateCamera GetStateCamera() {
        return state;
    }

}