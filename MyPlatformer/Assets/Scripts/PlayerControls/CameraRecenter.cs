using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

/// <summary>
/// this script is suppost to help with recentering player camera when the game initally starts up and whenever the play wants the camera position to reset  
/// </summary>

public class CameraRecenter : MonoBehaviour
{
    private CinemachineFreeLook cameraFreeLook;

    private CinemachineInputProvider camerainputProvider;

    [SerializeField]
    private InputActionReference recenterButton;

    public float timer = 0;
    public float waittime = 1;

    private void OnEnable()
    {
        recenterButton.action.Enable();
    }

    private void OnDisable()
    {
        recenterButton.action.Disable();
    }

    // Start is called before the first frame update 
    void Start()
    {
        cameraFreeLook = GetComponent<CinemachineFreeLook>();
        camerainputProvider = GetComponent<CinemachineInputProvider>();
        camerainputProvider.enabled = false;
        timer = 0;
        StartCoroutine(LateStart(.5f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        camerainputProvider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (recenterButton.action.IsPressed())
        {
            cameraFreeLook.m_RecenterToTargetHeading.m_enabled = true;
            cameraFreeLook.ForceCameraPosition(new Vector3(cameraFreeLook.transform.position.x, 1, cameraFreeLook.transform.position.z), Quaternion.identity);
            timer = 0;
        }
        else if(cameraFreeLook.m_RecenterToTargetHeading.m_enabled)
        {
            timer += Time.deltaTime;
            if(timer >= waittime)
            {
                cameraFreeLook.m_RecenterToTargetHeading.m_enabled = false;
                cameraFreeLook.m_YAxisRecentering.m_enabled = false;
                timer = 0;
            }
        }
    }
}
