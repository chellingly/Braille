using UnityEngine;
using System.Collections;

public class MultiCamSwitch : MonoBehaviour
{

    public GameObject[] cameras;
    public bool buttonEnabled;
    public bool mouseEnabled;
    public int mouseBtnIndex = 2;
    public bool keyEnabled;
    public string keyName;
	public string keyNameBack;
    public bool numericKeyEnabled;


    private int camIndex;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    // Use this for initialization
    void Start()
    {
        camIndex = 0;
        cameras[camIndex].SetActive(true);

        if (camIndex == 0)
        {
            for (int i = 1; i < cameras.Length; i++)
            {
                cameras[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseEnabled)
        {
            if (Input.GetMouseButtonDown(mouseBtnIndex))
            {
                switchCam();
            }
        }

		if ((keyEnabled) && (keyName != null) && (keyNameBack != null))
        {
            if (Input.GetKeyDown(keyName))
            {
                switchCam();
            }
			if (Input.GetKeyDown(keyNameBack))
			{
				switchCamMinus();
			}
			Debug.Log (camIndex);
        }

        if (numericKeyEnabled)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    int numberPressed = i;

                    if (numberPressed <= (cameras.Length-1))
                    {
                        for (int k = 0; k < cameras.Length; k++)
                        {
                            cameras[k].SetActive(false);
                        }

                        for (int j = 0; j < numberPressed; j++)
                        {
                            cameras[j].SetActive(false);
                        }

                        cameras[numberPressed].SetActive(true);

                        camIndex = numberPressed;
                    }
                }
            }
        }
    }


    public void CameraSwitch()
    {
        if (buttonEnabled)
        {
            switchCam();
        }
    }

    private void switchCam()
    {
        for (int k = 0; k < cameras.Length; k++)
        {
            cameras[k].SetActive(false);
        }

        if (camIndex < cameras.Length - 1)
        {
            camIndex += 1;
        }

        else if (camIndex >= cameras.Length - 1)
        {
            camIndex = 0;
        }

        cameras[camIndex].SetActive(true);

        if (camIndex > 0)
        {
            cameras[camIndex - 1].SetActive(false);
        }

        if (camIndex == 0)
        {
            for (int i = 1; i < cameras.Length; i++)
            {
                cameras[i].SetActive(false);
            }
        }
    }

	private void switchCamMinus()
	{
		for (int k = 0; k < cameras.Length; k++)
		{
			cameras[k].SetActive(false);
		}

		if (camIndex > 0)
		{
			camIndex -= 1;
		}

		else if (camIndex <= 0)
		{
			camIndex = cameras.Length-1;
		}

		cameras[camIndex].SetActive(true);

		if (camIndex > 0)
		{
			cameras[camIndex - 1].SetActive(false);
		}

		if (camIndex == 0)
		{
			for (int i = 1; i < cameras.Length; i++)
			{
				cameras[i].SetActive(false);
			}
		}

	}

}
