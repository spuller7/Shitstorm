using UnityEngine;
using System.Collections;
//using UnityEditor;

// For DEMO
public class BaseGUI : MonoBehaviour {

	protected 	GameObject 			cam;
	protected 	CameraMove			camScript;
	protected	delegate void 		CameraSettingGUI();
	protected 	CameraSettingGUI 	ActiveCameraSettingGUI;

	[SerializeField]
	protected 	GameObject[]		camPrefab;

	protected 	bool 				drawSettings = true;
	protected 	int 				cameraTypeIndex = 0;
	protected 	int 				lastCameraTypeIndex = -1;
	protected 	int 				rtsPresetIndex = 0;
	protected 	int 				lastRtsPresetIndex = -1;

	protected 	string[] 			toolbarStrings = {"RTS Camera", "RPG Camera", "MTP Camera"};
	protected 	string[] 			camPrefabNames;	

	void Start () {

		camScript = Camera.main.GetComponent<CameraMove>();

		ActiveCameraSettingGUI = new CameraSettingGUI(DrawSettings_RTS_Camera);

		if(camPrefab != null && camPrefab.Length > 0)
		{
			camPrefabNames = new string[camPrefab.Length]; 
			for(int idx=0; idx < camPrefab.Length; ++idx)
			{
				camPrefabNames[idx] = camPrefab[idx].name;
			}
		}
	}

	void Update()
	{
		// for exit
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
		
	/** */
	void OnGUI()
	{
		if(camScript != null)
			DrawSettingsWindow();
		else
			camScript = Camera.main.GetComponent<CameraMove>();
	}

	/** */
	protected void DrawSettingsWindow()
	{
		float resX = Screen.width/1920.0f;
		float resY = Screen.height/1080.0f;

		if(GUI.Button(new Rect(15  * resX, Screen.height - 40 * resY, 220 * resX, 40  * resY), ((!drawSettings) ? "Show Settings" : "Hide Settings")))
		{
			drawSettings = !drawSettings;
		}

		if(GUI.Button(new Rect(235  * resX, Screen.height - 40 * resY, 200 * resX, 40  * resY), "Reset"))
		{
			Application.LoadLevel ("Demo");
			return;
		}

		//
		if(drawSettings)
		{
			cameraTypeIndex = GUI.Toolbar (new Rect (10 * resX, 10 * resY, 800 * resX, 40 * resY), cameraTypeIndex, toolbarStrings);

			if(lastCameraTypeIndex != cameraTypeIndex)
			{
				lastCameraTypeIndex = cameraTypeIndex;
				switch(cameraTypeIndex)
				{
					case 0:
					ActiveCameraSettingGUI = new CameraSettingGUI(DrawSettings_RTS_Camera);
					camScript.SetMode = CameraMove.CameraModeNames.RTS_Camera; 
					break;
					case 1:
					ActiveCameraSettingGUI = new CameraSettingGUI(DrawSettings_RPG_Camera);
					// fix it
					if(camScript.target == null)
					{
						GameObject go = GameObject.FindWithTag("Player");
						if(go)
						{
							camScript.SetTarget = go.transform;
						}
					}
					camScript.SetMode = CameraMove.CameraModeNames.RPG_Camera;
			
					break;
					case 2:
					ActiveCameraSettingGUI = new CameraSettingGUI(DrawSettings_MTP_Camera);
					camScript.SetMode = CameraMove.CameraModeNames.MTP_Camera; 
					break;
				}
			}

			//
			ActiveCameraSettingGUI();
		}
	}

	/** */
	protected void DrawSettings_RTS_Camera()
	{
		float resX = Screen.width/1920.0f;
		float resY = Screen.height/1080.0f;

		if(camPrefabNames.Length > 0) // draw rts camera presets
		{
			rtsPresetIndex = GUI.Toolbar (new Rect (10 * resX, 60 * resY, 800 * resX, 40 * resY), rtsPresetIndex, camPrefabNames);
			if(lastRtsPresetIndex != rtsPresetIndex)
			{
				lastRtsPresetIndex = rtsPresetIndex;
				cam = camScript.gameObject;
				Destroy(cam);
				switch(rtsPresetIndex)
				{
				case 0:
					cam = Instantiate(camPrefab[0]) as GameObject;
					break;
				case 1:
					cam = Instantiate(camPrefab[1]) as GameObject;
					break;
				case 2:
					cam = Instantiate(camPrefab[2]) as GameObject;
					break;
				}
			}
		}

		GUI.BeginGroup(new Rect(10 * resX, 110 * resY, 1000 * resX, 1000 * resY), "       Settings");
		int elementShift = 0;
		int left = 5;
		int step = 20;
		int stepShift = 1;
		int height1 = 15;
		GUI.Box(new Rect(0, 0, 240, step * 25), " ");
		camScript.inversionX = GUI.Toggle(new Rect(left, (++elementShift) * step, 150, height1+5),camScript.inversionX, "Invert X moving direct");

		camScript.inversionY = GUI.Toggle(new Rect(left, (++elementShift) * step, 150, height1+5),camScript.inversionY, "Invert Y moving direct");

		camScript.useKeyBoardControl = GUI.Toggle(new Rect(left, (++elementShift)*step, 150, height1),camScript.useKeyBoardControl, "WASD and QE");

		camScript.canTouchObjects = GUI.Toggle(new Rect(left, (++elementShift)*step, 150, height1),camScript.canTouchObjects, "Can Touch Objects");

		GUI.Label(new Rect(left, (elementShift+1)*step, 100, height1+5), "Speed:");
		camScript.cameraSpeed = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.cameraSpeed, 0.0F, 20.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 100, height1+5), "Damping:");
		camScript.speedDampingSmoothness = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.speedDampingSmoothness, 0.0F, 1.0F);
		
		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "WASD Speed:");
		camScript.keyCamSpeed = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.keyCamSpeed, 0.0F, 40.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Max Zoom:");
		camScript.maxZoom = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.maxZoom, camScript.minZoom, 100.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Min Zoom:");
		camScript.minZoom = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.minZoom, 0.0F, camScript.maxZoom);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Zoom Sensitivity:");
		camScript.zoomSensitivity = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.zoomSensitivity, 0.0F, 50.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Tilt strength:");
		camScript.camRotInten = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.camRotInten, 0.0F, 10.0F);

		camScript.useLockPerimeter = GUI.Toggle(new Rect(left, (++elementShift)*step, 150, height1),camScript.useLockPerimeter, "Use Perimeter");
		if(camScript.useLockPerimeter)
		{
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Left Border:");
			camScript.leftBorder = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.leftBorder, -200.0F, camScript.rightBorder);

			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Right Border:");
			camScript.rightBorder = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step )+5, 100, height1), camScript.rightBorder, camScript.leftBorder, 200.0F);

			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Forward Border:");
			camScript.forwardBorder = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step )+5, 100, height1), camScript.forwardBorder, camScript.backBorder, 200.0F);

			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Back Border:");
			camScript.backBorder = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step )+5, 100, height1), camScript.backBorder, -200.0F, camScript.forwardBorder);

			camScript.useRollOutEffect = GUI.Toggle(new Rect(left, (++elementShift)*step, 150, height1),camScript.useRollOutEffect, "Use RollOut Effect");
			if(camScript.useRollOutEffect)
			{
				GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "RollOut Value:");
				camScript.rollOutValue = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.rollOutValue, 0.0F, 50);

				GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Smoothness:");
				camScript.rollOutValueSmoothness = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.rollOutValueSmoothness, 0.0F, 1.0f);
			}
		}

		camScript.useMoveToObjectEffect = GUI.Toggle(new Rect(left, (++elementShift)*step, 150, height1),camScript.useMoveToObjectEffect, "Use MoveToObject");
		if(camScript.useMoveToObjectEffect)
		{
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Smoothness:");
			camScript.moveToObjectSmoothness = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.moveToObjectSmoothness, 0.0F, 1.0f);
		}

		camScript.useComboZoom = GUI.Toggle(new Rect(left, (++elementShift)*step, 150, height1),camScript.useComboZoom, "Use ComboZoom");
		if(camScript.useComboZoom)
		{
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Zoom Rot Amount:");
			camScript.zoomRotAmount = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.zoomRotAmount, 0.0F, 70.0F);

			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "FOV Zoom Multyplier:");
			camScript.FOVZoomMultyplier = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.FOVZoomMultyplier, 0.0F, 3.0F);
		}
		GUI.EndGroup();
	}

	/** */
	protected void DrawSettings_RPG_Camera()
	{
		GUI.BeginGroup(new Rect(0, 80, 500, 500), "       Settings");
		int elementShift = 0;
		int left = 5;
		int step = 20;
		int stepShift = 1;
		int height1 = 15;
		GUI.Box(new Rect(0, 0, 240, 285), " ");

		camScript.followMode = GUI.Toggle(new Rect(left, (++elementShift)*step, 200, height1),camScript.followMode, "Follow Mode(WIP)");

		GUI.Label(new Rect(left, (elementShift+1)*step, 100, height1+5), "X Speed:");
		camScript.xSpeed = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.xSpeed, 0.0F, 90.0F);
		
		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Y Speed:");
		camScript.ySpeed = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.ySpeed, 0.0F, 180.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Y MaxLimit:");
		camScript.yMaxLimit = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.yMaxLimit, camScript.yMinLimit, 90.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Y MinLimit:");
		camScript.yMinLimit = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.yMinLimit, 0.0F, camScript.yMaxLimit);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Zoom Sensitivity:");
		camScript.zoomSensitivity = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.zoomSensitivity, 0.0F, 50.0F);
		
		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Max Distance:");
		camScript.maxDistance = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.maxDistance, camScript.minDistance, 50.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Min Distance:");
		camScript.minDistance = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.minDistance, 0.0F, 49.0F);

		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Smoothness:");
		camScript.Smoothness = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.Smoothness, 1.0F, 0.0f);

		camScript.isCamOffset = GUI.Toggle(new Rect(left, (++elementShift)*step, 200, height1),camScript.isCamOffset, "Use Offset");
		if(camScript.isCamOffset)
		{
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Offset X:");
			camScript.vectCamOffset.x = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.vectCamOffset.x, -5.0F, 5.0F);

			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Offset Y:");
			camScript.vectCamOffset.y = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.vectCamOffset.y, -5.0F, 5.0F);

			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Offset Z:");
			camScript.vectCamOffset.z = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.vectCamOffset.z, -5.0F, 5.0F);
		}
		GUI.EndGroup();
	}

	/** */
	protected void DrawSettings_MTP_Camera()
	{
		if(camScript.CamOnNewPos)
		{
			GUI.Label(new Rect(5, Screen.height - 25, 200, 25), "Press SPACE to back");

			if(GUI.Button(new Rect(Screen.width/2 - 25, Screen.height - 40, 100,40), "Back"))
			{
				camScript.SendCameraBack = true;
			}
		}

		GUI.BeginGroup(new Rect(0, 80, 500, 500), "       Settings");
		int elementShift = 0;
		int left = 5;
		int step = 20;
		int stepShift = 3;
		int height1 = 15;
		GUI.Box(new Rect(0, 0, 240, 145), " ");

		GUI.Label(new Rect(left, (elementShift+1)*step, 150, height1+5), "InOut Speed:");
		camScript.cameraInOutSpeed = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.cameraInOutSpeed, 0.0F, 1.0F);
		
		GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Offset Distance:");
		camScript.closeOffsetDistance = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step + stepShift), 100, height1), camScript.closeOffsetDistance, 0.0F, 30.0F);

		camScript.useDefaultCamPoint = GUI.Toggle(new Rect(left, (++elementShift)*step, 200, height1),camScript.useDefaultCamPoint, "Use Default Start Point");
		if(camScript.useDefaultCamPoint)
		{
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Point X:");
			camScript.startCamPoint.x = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.startCamPoint.x, -20.0F, 20.0F);
			
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Point Y:");
			camScript.startCamPoint.y = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.startCamPoint.y, 0.0F, 20.0F);
			
			GUI.Label(new Rect(left, (elementShift+1)*step, 120, height1+5), "Point Z:");
			camScript.startCamPoint.z = GUI.HorizontalSlider(new Rect(left+120, (++elementShift)*(step)+5, 100, height1), camScript.startCamPoint.z, -20.0F, 20.0F);
		}
		GUI.EndGroup();
	}
}