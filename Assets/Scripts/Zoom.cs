using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour
{
    public InputField inputField, verticalRotationInput, horizontalRotationInput, verticalMovementInput, horizontalMovementInput;
    public Camera camera;
    public GameObject cameraModule;
    private int minValue = 6, maxValue = 540;
    public int delay;
    double scaled;
    public float verticalRotation, horizontalRotation, verticalMovement, horizontalMovement, LimitAngleX = 45, LimitAngleY = 180;
    private float totalYRotation = 0.0f, totalXRotation = 0.0f, xAxis = 0.0f, yAxis = 0.0f;
    bool axisAux = true;
    float rotationAux = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        camera.focalLength = 6;
        inputField.onEndEdit.AddListener(delegate { ValueChangeCheckAsync(); });

        verticalRotationInput.onEndEdit.AddListener(delegate { ValueChangeCheckAsync(0); });
        horizontalRotationInput.onEndEdit.AddListener(delegate { ValueChangeCheckAsync(1); });
        verticalMovementInput.onEndEdit.AddListener(delegate { ValueChangeCheckAsync(2); });
        horizontalMovementInput.onEndEdit.AddListener(delegate { ValueChangeCheckAsync(3); });

        verticalRotationInput.text = verticalRotation.ToString();
        horizontalRotationInput.text = horizontalRotation.ToString();
        verticalMovementInput.text = verticalMovement.ToString();
        horizontalMovementInput.text = horizontalMovement.ToString();
    }

    private void ValueChangeCheckAsync(int v)
    {
        switch(v)
        {
            case 0:
                verticalRotation = float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
                break;
            case 1:
                horizontalRotation = float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
                break;
            case 2:
                verticalMovement = float.Parse(verticalMovementInput.text, CultureInfo.InvariantCulture.NumberFormat);
                break;
            case 3:
                horizontalMovement = float.Parse(horizontalMovementInput.text, CultureInfo.InvariantCulture.NumberFormat);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Fire2") > 0)
        {
            //print("top left");
            StartCoroutine(ChangeVerticalRotationRoutine(.1f));
        }
        if (Input.GetAxis("Fire4") > 0 && (float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat) > 0.01f))
        {
            //print("bottom left");
            StartCoroutine(ChangeVerticalRotationRoutine(-.1f));
        }

        if (Input.GetAxis("Fire3") > 0)
        {
            //print("top right");
            StartCoroutine(ChangeHorizontalRotationRoutine(.1f));
        }
        if (Input.GetAxis("Fire5") > 0 && (float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat) > 0.01f))
        {
            //print("bottom right");
            StartCoroutine(ChangeHorizontalRotationRoutine(-.1f));
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            var angles = transform.localEulerAngles;

            xAxis = Input.GetAxis("Vertical") * verticalRotation;
            yAxis = Input.GetAxis("Horizontal") * horizontalRotation;

            totalXRotation = Mathf.Clamp(totalXRotation - xAxis, -LimitAngleX, LimitAngleX);
            totalYRotation = Mathf.Clamp(totalYRotation + yAxis, -LimitAngleY, LimitAngleY);

            angles.x = totalXRotation;
            angles.y = totalYRotation;
            
            var euler = Quaternion.Euler(angles);
            
            cameraModule.transform.localRotation = euler;

            cameraModule.transform.localEulerAngles = angles;
        }

        if (Input.GetKeyDown(KeyCode.W))
            cameraModule.transform.position = new Vector3(cameraModule.transform.position.x, cameraModule.transform.position.y + (verticalMovement), cameraModule.transform.position.z);
        if (Input.GetKeyDown(KeyCode.S))
            cameraModule.transform.position = new Vector3(cameraModule.transform.position.x, cameraModule.transform.position.y + (-verticalMovement), cameraModule.transform.position.z);
        if (Input.GetKeyDown(KeyCode.A))
            cameraModule.transform.position = new Vector3(cameraModule.transform.position.x + (-horizontalMovement), cameraModule.transform.position.y, cameraModule.transform.position.z);
        if (Input.GetKeyDown(KeyCode.D))
            cameraModule.transform.position = new Vector3(cameraModule.transform.position.x + (horizontalMovement), cameraModule.transform.position.y, cameraModule.transform.position.z);
    }

    IEnumerator ChangeVerticalRotationRoutine(float v)
    {
        if (axisAux)
        {
            axisAux = false;
            ChangeVerticalRotation(v);
            yield return new WaitForSeconds(.2f);
            axisAux = true;
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }
    IEnumerator ChangeHorizontalRotationRoutine(float v)
    {
        if (axisAux)
        {
            axisAux = false;
            ChangeHorizontalRotation(v);
            yield return new WaitForSeconds(.2f);
            axisAux = true;
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }

    private void ChangeVerticalRotation(float v)
    {
        if (float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat) <= 0.1f)
        {
            if (v == 0.1f)
            {
                if (float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat) == 0.1f)
                {
                    verticalRotationInput.text = (verticalRotation + v).ToString("0.00");
                    verticalRotation = float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    verticalRotationInput.text = (verticalRotation + rotationAux).ToString("0.00");
                    verticalRotation = float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
                }
            }
            else
            {
                verticalRotationInput.text = (verticalRotation - rotationAux).ToString("0.00");
                verticalRotation = float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
            }
        }
        else
        {
            verticalRotationInput.text = (verticalRotation + v).ToString("0.00");
            verticalRotation = float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
        }
    }
    private void ChangeHorizontalRotation(float v)
    {
        if (float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat) <= 0.1f)
        {
            if (v == 0.1f)
            {
                if (float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat) == 0.1f)
                {
                    horizontalRotationInput.text = (horizontalRotation + v).ToString("0.00");
                    horizontalRotation = float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    horizontalRotationInput.text = (horizontalRotation + rotationAux).ToString("0.00");
                    horizontalRotation = float.Parse(verticalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
                }
            }
            else
            {
                horizontalRotationInput.text = (horizontalRotation - rotationAux).ToString("0.00");
                horizontalRotation = float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
            }
        }
        else
        {
            horizontalRotationInput.text = (horizontalRotation + v).ToString("0.00");
            horizontalRotation = float.Parse(horizontalRotationInput.text, CultureInfo.InvariantCulture.NumberFormat);
        }
    }

    async Task ValueChangeCheckAsync()
    {
        scaled = minValue + (double)(Int32.Parse(inputField.text) - 0) / (90 - 0) * (maxValue - minValue);
        if (camera.focalLength < (float)scaled)
        {
            for (float i = camera.focalLength; i <= (float)scaled; i++)
            {
                camera.focalLength = i;
                await Task.Delay(delay);
            }
        }
        else
        {
            for (float i = camera.focalLength; i >= (float)scaled; i--)
            {
                camera.focalLength = i;
                await Task.Delay(delay);
            }
        }
    }

}
