// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** *
// * Copyright 2016  All Rights Reserved.
// *
// * Please direct any bugs or questions to vadakuma@gmail.com
// * version 1.2.1
// * author vadakuma 
// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
using UnityEngine;
using System.Collections;
using System;


public class CameraMove : MonoBehaviour {
	
	public enum CameraModeNames
	{
		None,
		RTS_Camera,		// real time strategy
		RPG_Camera,		// role play game
		MTP_Camera		// move to point
	};
	// current camera mode
	[Tooltip("Set current camera mode")]
	public 		CameraModeNames 	Mode 		= CameraModeNames.RTS_Camera;
	// ref on Camera.main
	protected 	Camera 				mainCamera;
	
	/** 
	 * RPG CAMERA
	*/
	#region RPG Public properties
	[Tooltip("The camera following of the object")]
	public		bool 				followMode;
	[Tooltip("The camera rotates around that object")]
	public		Transform 			target;
	[Range (0.0f,120f),Tooltip("Camera speed on X axis")]
	public		float 				xSpeed = 90.0f;
	[Range (0.0f,180f),Tooltip("Camera speed on Yaxis")]
	public		float 				ySpeed = 30.0f;
	[Tooltip("Camera limits on Y axis")]
	public 		float				yMaxLimit = 80f;
	[Tooltip("Camera limits on Y axis")]
	public 		float 				yMinLimit = 0.0f;
	[Tooltip("Max zoom distance")]
	public		float 				maxDistance = 20.0f;
	[Tooltip("Min zoom distance")]
	public		float 				minDistance = 10.0f;
	[Tooltip("Use Stable offset for camera position")]
	public 		bool 				isCamOffset = false;
	[Tooltip("Stable offset for camera position")]
	public 		Vector3				vectCamOffset = Vector3.zero;
	[Range (0.0f,1f),Tooltip("Orbit rotation smoothness")]
	public 		float 				rotationSmoothness = 0.1f;
	#endregion

	#region RPG Protected properties
	// for avoid fake touches
	protected 	float 				touchThreshold      = 30.0f;
	protected 	float 				fingerDistThreshold = 30.0f;
	protected const float 			SPEED_CORRECTION = 0.02f;
	protected const int 			MAX_ANGLE = 360;
	// last coordinates finger position
	protected 	Vector3 			lastFingerPos  = Vector3.zero;
	protected 	Vector3 			deltaFingerPos = Vector3.zero; //lastFingerPos - newFingerPos
	// parameters for tracking finger passed by.
	protected 	bool 				isResetFingerMoveDist    = false;
	protected 	float 				resetFingerMoveDistTimer = 0.2f;
	protected 	float 				resetFingerMoveDistTime  = 0.2f;
	//Camera rotation amount, depends on the displacement
	protected 	float 				xDelta = 0.0f;
	protected 	float 				yDelta = 0.0f;
	// Current camera offset (depend on vectCamOffset)
	protected 	Vector3				currVectCamOffset = Vector3.zero;
	// new camera position on next tick 
	protected 	Vector3				newPosition = Vector3.zero;
	// for keyboard events
	protected 	bool 				KeyCodeW = false;
	protected 	bool 				KeyCodeA = false;
	protected 	bool 				KeyCodeS = false;
	protected 	bool 				KeyCodeD = false;
	protected 	bool 				KeyCodeQ = false;
	protected 	bool 				KeyCodeE = false;
	// the distance to the object 
	protected	float 				Distance = 2.0f;
	// new distance to the object
	protected	float 				newDist = 0.0f;	
	/** path dist finger */
	protected 	float 				fingerPathDist = 0.0f;
	#endregion

	/** 
	 * RTS CAMERA 
	 */
	#region RTS Public properties
	[Tooltip("Invert moving side")]
	public  	bool 				inversionX = true;
	[Tooltip("Invert moving side")]
	public  	bool 				inversionY = true;
	[Tooltip("WASD for moving and QE for zooming")]
	public  	bool 				useKeyBoardControl = true;
	[Tooltip("Activate touch event methods by touching special object with tag Touch.")]
	public  	bool 				canTouchObjects = true;
	[SerializeField] [Tooltip("When pointer near the edge of screen camera is moving")]
	public  	bool 				mouseScreenEdgeMovement = false;
	[SerializeField] [Tooltip("User can move the camera by default way")]
	public  	bool 				canMoveByClick = true;
	[SerializeField] [Range (0.0f,10000), Tooltip("moveBorderWidth")]
	public  	float 				moveBorderWidth = 100;
	[SerializeField] [Tooltip("cameraSpeed")]
	public 		float 				cameraSpeed = 1.0f;
	[SerializeField] [Range (0.0f,1.0f),Tooltip("how fast camera is stopped")]
	public 		float 				speedDampingSmoothness = 0.35f;
	[Tooltip("Use lock perimeter for camera moving")]
	public 		bool 				useLockPerimeter    = true;
	[Tooltip("Lock perimeter for camera moving to the left side")]
	public 		float 				leftBorder    = -100f;
	[Tooltip("Lock perimeter for camera moving")]
	public 		float 				rightBorder   = 100f;
	[Tooltip("Lock perimeter for camera moving")]
	public 		float 				forwardBorder = 100f;
	[Tooltip("Lock perimeter for camera moving")]
	public 		float 				backBorder    = -100f;
	[Tooltip("Camera can RollOut out of perimeter")]
	public 		bool 				useRollOutEffect = false;
	[Tooltip("How far camera can roll out outside from lock perimeter")]
	public 		float 				rollOutValue = 10.0f;
	[Tooltip("roll in back in lock repimeter smoothness")]
	public 		float 				rollOutValueSmoothness = 0.8f;
	[Tooltip("Camera can move to selected object")]
	public 		bool 				useMoveToObjectEffect = false;
	[Tooltip("How fast camera should move to selected object")]
	public 		float 				moveToObjectSmoothness = 0.8f;
	[Tooltip("Change FOV in zooming")]
	public 		bool 				useComboZoom    = true;
	[Tooltip("Camera zoom settings. No zoom if Zoom Sensitivity = 0")]
	public 		float 				maxZoom       = 65f;
	[Tooltip("Camera zoom settings. No zoom if Zoom Sensitivity = 0")]
	public 		float 				minZoom       = 25f;
	[Range (0.0f,10f), Tooltip("Camera lateral inclination strength. 0 - without lateral inclinations")]
	public 		float 				camRotInten       = 0.1f;
	[Tooltip("For RTS and RPG. No zoom if Zoom Sensitivity = 0")]
	public		float 				zoomSensitivity   = 8f; 
	[Range (0.0f,70f), Tooltip("Camera tilt  in zooming")]
	public 		float 				zoomRotAmount     = 30.0f;
	[Range (0.0f,10f), Tooltip("changes FOV in zooming")]
	public 		float 				FOVZoomMultyplier = 0.0f;
	[Tooltip("Camera speed WASD moving")]
	public 		float 				keyCamSpeed = 10.0f;
	[Range (0.0f,1f),Tooltip("rotation smoothness")]
	public 		float 				Smoothness = 0.5f;
	#endregion

	#region RTS Protected properties
	protected 	int[] 				moveBorderSide = new int[]{0/*left */,0/*right*/,0/*back*/,0/*forward*/};
	// for inversion status control
	public  	int 				axisX = 1;
	public  	int 				axisY = 1;
	// indicator - camera moving(true) or not(false)
	protected 	bool 				startMove   	   = false;
	protected 	bool 				leftMouseBtnDown   = false;
	//  coordinates of the click
	protected 	Vector3 			firstClick = Vector3.zero;
	// camera damping
	protected 	float 				cameraViscosity = 0.0f;
	protected 	float 				maxCameraViscosity = 1.0f;
	protected 	float 				speedDamping = 0.0f;
	// camera damping speed
	protected	float 				cameraViscositySpeed = 0.1f;
	//Camera displacement amount, depends on mouse or keyboard 
	protected  	Vector3 			delta   = Vector3.zero;
	protected 	Vector3 			lastPos = Vector3.zero;
	// for ComboZoom, change size forward inclination 
	protected 	float 				zoomRotation = 0.0f;
	//
	protected   float 				currRollOutValue = 0.0f;
	// object position where the camera move on
	protected  	Vector3 			selectedObjectPosition   = Vector3.zero;
	// special trigger, when camera moving to selected object
	protected 	bool 				movingToObject = false;
	// initial camera Pos and Rot on Start
	protected 	Quaternion 			initialCamRot;
	protected 	float 				initialCamFOV;
	protected 	Vector3 			initialCamLoc = Vector3.zero;
	protected 	float 				deltaSpeedX = 0.0f;
	protected 	float 				deltaSpeedY = 0.0f;
	protected 	float 				camAltitude = 0.0f;
	// deltaAltitude adding to camAltitude
	protected 	float 				deltaAltitude = 0.0f;
	// add for Camera altitude
	protected 	float 				altitudeControl = 0.0f;
	// for zoom
	protected 	float 				oldTouchDeltaDist;
	//
	protected Vector3 				lastPosition = Vector3.zero;
	#endregion

	/** 
	 * MTP CAMERA 
	 */
	#region MTP Public properties
	[Tooltip("Point the camera from being start")]
	public 		Vector3 			startCamPoint = new Vector3(0,0,0);
	[Tooltip("Use own start camera point")]
	public		bool 				useDefaultCamPoint = false;
	[Range (0.0f,1f),Tooltip("Camera smoothing")]
	public		float 				cameraInOutSpeed = 0.02f;
	[Range (0.0f,100f),Tooltip("Offset Distance")]
	public		float 				closeOffsetDistance = 10.0f;
	#endregion

	#region MTP Protected properties
	// to move to new point, or from the
	protected	bool 				toPoint = false;
	protected	bool 				camOnNewPos = true;
	// move to this point 
	protected	Vector3 			thisPointLoc = new Vector3(0,0,0);
	protected	Vector3 			realPointLoc = new Vector3(0,0,0);
	// camera rotation to this Quaternion
	protected 	Quaternion 			thisPointRot;
	// camera's FOV in new point
	protected	float 				pointFOV = 60.0f;
	protected	float 				currFOV  = 60.0f;
	// distance from initial pos to new pos
	protected	float 				maxDistanceToPoint = 0.0f;
	
	// for camera modes control
	protected 	delegate void 		CameraMode();
	protected 	CameraMode 			ActiveCameraMode;
	#endregion

	// Use this for initialization
	void Start()
	{
		if(Camera.main)
		{
			mainCamera = Camera.main;
		}
		else
		{
			Debug.LogWarning("Start error. No Camera on this object. Try to create a Camera on this object!");
			CreateCamera();
		}

		if(mainCamera != null)
		{
			// All good. Set initial params
			initialCamRot = mainCamera.transform.rotation;
			initialCamFOV = mainCamera.fieldOfView;
			initialCamLoc = mainCamera.transform.position;
			camAltitude   = mainCamera.transform.position.y;
			if(!useDefaultCamPoint)
				startCamPoint = initialCamLoc;
			// set default camera
			SetMode       = Mode;
		}
		else
		{
			Debug.LogWarning("Start error. Set this script on mainCamera!");
			SetMode = CameraModeNames.None;
		}
	}

	/** */
	public bool CamOnNewPos
	{
		get { return camOnNewPos; }
		set { camOnNewPos = value; }
	}

	/** */
	public bool SendCameraBack
	{
		set { toPoint = !value; }
	}

	/** */
	protected bool InversionX
	{
		get { return inversionX; }
		set { 
			inversionX = value;
			if(inversionX)
				axisX = 1;
			else
				axisX = -1;
		}
	}

	/** */
	protected bool InversionY
	{
		get { return inversionY; }
		set { 
			inversionY = value;
			if(inversionY)
				axisY = -1;
			else
				axisY = 1;
		}
	}

	/** */
	public Transform SetTarget
	{
		get { return target; }
		set { target = value;}
	}

	/** Setting up and prepering camera modes*/
	public CameraModeNames SetMode
	{
		get { return Mode; }
		set { 
			Mode = value;

			switch(Mode)
			{
				//
				case CameraModeNames.None:
					ActiveCameraMode = new CameraMode(EmptyMode);
				break;
				//RTS_Camera
				case CameraModeNames.RTS_Camera:
					ActiveCameraMode = new CameraMode(RTSCamera);
					// Initial params RTS_Camera
					if(mainCamera)
					{
						mainCamera.transform.position = initialCamLoc;
						mainCamera.transform.rotation = initialCamRot;
					}
				break;
				//RPG_Camera
				case CameraModeNames.RPG_Camera:
					if(target)
					{
						ActiveCameraMode = new CameraMode(RPGCamera);
						// Initial params RPG_Camera
						Distance = minDistance + maxDistance/4;
						newDist  = Distance;
						xDelta = transform.eulerAngles.y;
						yDelta = transform.eulerAngles.x;
					}
					else
					{
						Debug.LogWarning("Set Target object for using RPG Camera!");
						//Mode = CameraModeNames.None;
						ActiveCameraMode = new CameraMode(WaitMode);
					}
				break;
				//MTP_Camera
				case CameraModeNames.MTP_Camera:
					ActiveCameraMode = new CameraMode(MTPCamera);
					// Initial params MTP_Camera
					if(useDefaultCamPoint)
					{
						if(mainCamera)
							mainCamera.transform.position = startCamPoint;
					}
					else
					{
						if(!useDefaultCamPoint)
							startCamPoint = initialCamLoc;
						if(mainCamera)
							mainCamera.transform.position = startCamPoint;
					}
					if(mainCamera)
					{
						mainCamera.transform.rotation = initialCamRot;
						mainCamera.fieldOfView		  = currFOV;
					}
					CamOnNewPos    = false;
					SendCameraBack = true;
				break;
			}
		}
	}

	/** */
	void OnValidate()
	{
		// watching for the Mode param
		SetMode = Mode;
		// warching on axis inversion for rts camera
		InversionX = inversionX;
		InversionY = inversionY;
	}

	/** Update is called once per frame */
	void Update () {
		//	Call current camera's mode, witch setup in SetMode  
		if(CheckExistCameraComponent())
			ActiveCameraMode();
	}

	/** */
	void OnGUI () {

	}
	
/**********************************************************************************************************************************************************************
 *  CAMERA MODES
 * *********************************************************************************************************************************************************************/
	/** null mode, no move */
	protected void EmptyMode(){ }			

	/** when target is null, this mode is activated*/
	protected void WaitMode ()
	{
		if(target != null)
		{
			SetMode = CameraModeNames.RPG_Camera;
		}
	}

	/** 
		RTS_Camera
	 */
	protected void RTSCamera()
	{
		InputMethod_RTS();	// for Mouse, WASD and Arrows control

		if(startMove)
		{
			cameraViscosity = Mathf.Lerp(cameraViscosity, maxCameraViscosity, cameraViscositySpeed);
			currRollOutValue = Mathf.Lerp(currRollOutValue, rollOutValue, rollOutValueSmoothness);
			if(Input.touchCount == 0 && leftMouseBtnDown)	// if mouse down
			{
				delta = Vector3.Lerp(delta, Input.mousePosition - lastPos, 0.25f);
			}
			else
			{
				if(Input.touchCount == 1)
					delta = Vector3.Lerp(delta, Input.GetTouch(0).deltaPosition, 0.25f);
			}

			// fake finger|mouse path, but little faster
			lastFingerPos.x += Mathf.Abs(delta.x);
			lastFingerPos.y += Mathf.Abs(delta.y);
			// smoothing turn the camera
			deltaSpeedX = Mathf.Lerp(deltaSpeedX, delta.x, 0.1f);
			deltaSpeedY = Mathf.Lerp(deltaSpeedY, delta.y, 0.1f);

			speedDamping = 1.0f;
			lastPosition = mainCamera.transform.position;
		}
		else // stoping camera
		{
			cameraViscosity = Mathf.Lerp(cameraViscosity, 0.0f, cameraViscositySpeed);
			speedDamping = Mathf.Lerp(speedDamping, 0.0f, speedDampingSmoothness);
			currRollOutValue = Mathf.Lerp(currRollOutValue, 0.0f, rollOutValueSmoothness);
			deltaSpeedX = Mathf.Lerp(deltaSpeedX, 0.0f, 0.1f);
			deltaSpeedY = Mathf.Lerp(deltaSpeedY, 0.0f, 0.1f);
		}
		
		Vector3 newPos = Vector3.zero;
		if(useMoveToObjectEffect)
		{
			if(movingToObject)
				newPos = RTSMoveToObjectModePostion();
			else
				newPos = RTSDefaultModePostion();
		}
		else
		{
			newPos = RTSDefaultModePostion();
		}

		// camera altitude control
		float currcamAltitude = mainCamera.transform.position.y;
		// clamping delta altitude
		if(deltaAltitude > 0)
			deltaAltitude = Mathf.Clamp(deltaAltitude, 0, maxZoom/4);
		if(deltaAltitude < 0)
			deltaAltitude = Mathf.Clamp(deltaAltitude, -minZoom/4, 0);
		// check camera altitude
		if( (currcamAltitude + deltaAltitude) < maxZoom &&
		   (currcamAltitude + deltaAltitude) > minZoom )
			altitudeControl = Mathf.Lerp(altitudeControl, deltaAltitude, 0.1f);
		else // camera on edge maxZoom and moving to more when maxZoom 
			altitudeControl = Mathf.Lerp(altitudeControl, 0.0f, 0.1f);


		// set offset
		Vector3		totalOffset = Vector3.zero;
		Quaternion 	rotation  	= Quaternion.Euler(0, 0, 0);
		totalOffset = new Vector3( 0 , 2.0f * altitudeControl , -altitudeControl);
		totalOffset = rotation * totalOffset;

		// set new position
		Vector3 finalPos = (newPos + totalOffset);
		finalPos.y = Mathf.Clamp(finalPos.y, minZoom, maxZoom);
		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, finalPos, 0.1f);

		lastPos = Input.mousePosition;
		// Now set new Rotation to the camera with zoom
		if(Input.touchCount > 1 ) // for windows touch monitors/tablets. multytouch zoom. support in windows 7|vista|8|8.1|10
		{
			startMove 	 = false;
			Touch touch1 = Input.GetTouch(1);
			Touch touch0 = Input.GetTouch(0);
			float fingersDistance = Vector3.Distance(touch0.position, touch1.position);

			if(touch1.phase == TouchPhase.Began)
			{
				oldTouchDeltaDist = fingersDistance;
			}
			
			float touchDeltaDist = (oldTouchDeltaDist - fingersDistance);
			if(fingersDistance != oldTouchDeltaDist)
			{
				oldTouchDeltaDist = fingersDistance;

				deltaAltitude = Mathf.Lerp(deltaAltitude, zoomSensitivity * touchDeltaDist, 0.1f);
			}
			else
			{
				deltaAltitude = Mathf.Lerp(deltaAltitude, 0.0f, 0.1f);
			}
		}
		else
		{
			// pc zooming with Mouse ScrollWheel
			if(Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				float _dist = Input.GetAxis("Mouse ScrollWheel");
				if(_dist < 0)
					deltaAltitude = zoomSensitivity;
				else
					deltaAltitude = -zoomSensitivity;
			}
			else
			{
				deltaAltitude = Mathf.Lerp(deltaAltitude, 0.0f, 0.05f);
			}
		}
		
		// changing cameras FOV depending on altitude
		if(useComboZoom)
		{
			float yPosCam = mainCamera.transform.position.y;
			zoomRotation = Mathf.Lerp(zoomRotation,(1 - yPosCam/( minZoom + (maxZoom - minZoom)/2)) * zoomRotAmount, 0.25f);
		}
		else
		{
			zoomRotation = Mathf.Lerp(zoomRotation,0, 0.25f);
		}
		mainCamera.fieldOfView = initialCamFOV + zoomRotation * FOVZoomMultyplier;

		// tilt the camera while driving (free Y axis rotation)
		Quaternion _rot = Quaternion.Euler(initialCamRot.eulerAngles.x - deltaSpeedY * cameraViscosity * camRotInten - zoomRotation,
		                                   mainCamera.transform.rotation.eulerAngles.y,
		                                   initialCamRot.eulerAngles.z + deltaSpeedX * cameraViscosity * camRotInten);
		//set rotation value
		mainCamera.transform.rotation = _rot;
	}

	/** User controlling mode */
	protected Vector3 RTSDefaultModePostion()
	{
		Vector3 newPos = Vector3.zero;

		if(startMove)
		{
			if(useLockPerimeter)
			{
				float currLeftBoard    = Mathf.Clamp(leftBorder, float.MinValue, rightBorder);
				float currRightBoard   = Mathf.Clamp(rightBorder, leftBorder, float.MaxValue);
				float currBackBoard    = Mathf.Clamp(backBorder, float.MinValue, forwardBorder);
				float currForwardBoard = Mathf.Clamp(forwardBorder, backBorder, float.MaxValue);
				
				if(useRollOutEffect)
				{
					currLeftBoard    = leftBorder + ((leftBorder > 0) ? currRollOutValue : -currRollOutValue);
					currRightBoard   = rightBorder + ((rightBorder > 0)? currRollOutValue : -currRollOutValue);
					currForwardBoard = forwardBorder + ((forwardBorder > 0) ? currRollOutValue : -currRollOutValue);
					currBackBoard    = backBorder + ((backBorder > 0) ? currRollOutValue : -currRollOutValue);
				}
				newPos 	 = mainCamera.transform.forward * delta.y + mainCamera.transform.right * delta.x;
				newPos 	*= (cameraViscosity * cameraSpeed);
				newPos   = new Vector3(Mathf.Clamp(mainCamera.transform.position.x + newPos.x * axisX, currLeftBoard, currRightBoard), // left right moves
				                       mainCamera.transform.position.y,
				                       Mathf.Clamp(mainCamera.transform.position.z + newPos.z * axisY, currBackBoard, currForwardBoard) // back and forward moves
				                       );
			}
			else
			{
				newPos = new Vector3(mainCamera.transform.position.x + delta.x * cameraViscosity * cameraSpeed * axisX,
				                     mainCamera.transform.position.y,
				                     mainCamera.transform.position.z + delta.y * cameraViscosity * cameraSpeed * axisY
				                     );
			}
		}
		else
		{
			newPos = mainCamera.transform.position;
			// stop damping effect
			Vector3 dampDir = (lastPosition - mainCamera.transform.position).normalized * cameraSpeed * speedDamping * Vector3.Distance(lastPosition,mainCamera.transform.position);
			newPos -= dampDir;
			//newPos.x -=  speedDamping * cameraSpeed * delta.x;
			//newPos.z -=  speedDamping * cameraSpeed * delta.y;

			// roll out effect when camera got edges of perimeter
			if(useLockPerimeter && useRollOutEffect)
			{
				if(mainCamera.transform.position.x > rightBorder)
					newPos.x = Mathf.Lerp(mainCamera.transform.position.x, rightBorder, rollOutValueSmoothness);
				if(mainCamera.transform.position.x < leftBorder)
					newPos.x = Mathf.Lerp(mainCamera.transform.position.x, leftBorder, rollOutValueSmoothness);
				if(mainCamera.transform.position.z < backBorder)
					newPos.z = Mathf.Lerp(mainCamera.transform.position.z, backBorder, rollOutValueSmoothness);
				if(mainCamera.transform.position.z > forwardBorder)
					newPos.z = Mathf.Lerp(mainCamera.transform.position.z, forwardBorder, rollOutValueSmoothness);
			}
		}

		return newPos;
	}

	/** Same as MTP camera but in RTS camera. Auto moving to the point*/
	protected Vector3 RTSMoveToObjectModePostion()
	{
		Quaternion 	rotation    = mainCamera.transform.rotation;
		Vector3		totalOffset = new Vector3(0.0f, 0.0f, -mainCamera.transform.position.y) + Vector3.zero * Distance;
		Vector3 	finalPos    = selectedObjectPosition + rotation * totalOffset;
		Vector3 	newPos		= Vector3.zero;

		newPos.x = Mathf.Lerp(mainCamera.transform.position.x, finalPos.x, moveToObjectSmoothness);
		newPos.y = mainCamera.transform.position.y;
		newPos.z = Mathf.Lerp(mainCamera.transform.position.z, finalPos.z, moveToObjectSmoothness);

		return newPos;
	}

	
	/**  
	 * camera rotation around the specified object (RPG_Camera)
	 */
	protected void RPGCamera()
	{ 
		if(target == null)
		{
			SetMode = CameraModeNames.RPG_Camera;
			return;
		}
		else
		{
			newPosition = Vector3.Lerp( newPosition, target.position, Smoothness);
		}
		if(Input.GetMouseButton(0))
		{
			Vector3 newDeltaFingerPos  = CheckTouchDistance(deltaFingerPos, Input.mousePosition, lastFingerPos, touchThreshold);
			deltaFingerPos = Vector3.Lerp(deltaFingerPos, newDeltaFingerPos, rotationSmoothness);

			//deltaFingerPos  = CheckTouchDistance(deltaFingerPos, Input.mousePosition, lastFingerPos, touchThreshold);
			fingerPathDist += Vector3.Distance(deltaFingerPos, Vector3.zero);
			isResetFingerMoveDist = false;
		}
		else
		{
			deltaFingerPos = Vector3.Lerp(deltaFingerPos, Vector3.zero, 0.1f);
			// we need dump FingerMoveDist, but with delay. This is for check total finger dist|length of a way
			//big length of a way - that touching to a 3d world point isn't activated. See CheckTouchDistance()
			//small length of a way - is considered that it is simple a click - touch the point will execute the action.
			// Without this we saw a very faster rotation camera if make many screen touches. This need only for "touch windows"
			// With mouse no problem 
			if(!isResetFingerMoveDist)
				ResetFingerMoveDist();
		}

		// FOR DEMO
		if(Input.GetMouseButtonUp(0))
		{
			CheckClickPoint_RTS(Input.mousePosition);
		}

		if(minDistance > maxDistance) // from Update
			minDistance = maxDistance;

		// multytouch zoom. support in windows 7|vista|8|8.1(thanks unity)
		newDist -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
		newDist  = Mathf.Clamp(newDist, minDistance, maxDistance);
		Distance = Mathf.Lerp(Distance, newDist, 0.1f);

		// add offset in camera position
		if(isCamOffset)
		{
			currVectCamOffset = Vector3.Lerp(currVectCamOffset, vectCamOffset, 0.1f);
		}
		else
		{
			currVectCamOffset = Vector3.Lerp(currVectCamOffset, Vector3.zero, 0.1f);
		}

		if(!followMode)
		{
			//new displacement of the finger|mouse
			xDelta = Mathf.Lerp(xDelta, xDelta + (deltaFingerPos.x) * xSpeed * SPEED_CORRECTION, Smoothness);
			yDelta = Mathf.Lerp(yDelta, yDelta - (deltaFingerPos.y) * ySpeed * SPEED_CORRECTION, Smoothness);
			
			yDelta = ClampAngle(yDelta, yMinLimit, yMaxLimit);

			Quaternion 	rotation    = Quaternion.Euler(yDelta, xDelta, 0);
			//Quaternion 	rotation    = Quaternion.Lerp(mainCamera.transform.rotation, Quaternion.Euler(new Vector3(yDelta, xDelta, 0)), 0.1f);
			Vector3		totalOffset = new Vector3(0.0f, 0.0f, -Distance) + currVectCamOffset * Distance;
			Vector3 	position    = newPosition +  rotation * totalOffset;
			// set new params
			mainCamera.transform.rotation = rotation;
			mainCamera.transform.position = position;
		}
		else // Follow Mode On
		{
			Vector3 	vect = newPosition + currVectCamOffset * Distance;
			Quaternion 	rot = Quaternion.LookRotation(vect - mainCamera.transform.position);
			Vector3		totOffset = new Vector3(0.0f, 0.0f, -Distance);

			mainCamera.transform.position = vect + rot * totOffset;
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, rot, Smoothness);
		}

		// update finger|mouse position
		lastFingerPos = Input.mousePosition;

		// reset values ingerMoveDist
		if(resetFingerMoveDistTimer > 0.0f)
		{
			resetFingerMoveDistTimer -= Time.deltaTime;
		}
		else if(isResetFingerMoveDist)
		{
			fingerPathDist = 0.0f;
			isResetFingerMoveDist = false;
		}
	}

	/** 
	 * driving camera to selected point (MTP_Camera)
	 */
	protected void MTPCamera()
	{
		InputMethod_MTP();

		if(toPoint) // driving camera to selected point
		{
			Vector3 newVec = mainCamera.transform.position;
			newVec.x = Mathf.Lerp(newVec.x,thisPointLoc.x, cameraInOutSpeed*2f);
			newVec.y = Mathf.Lerp(newVec.y,thisPointLoc.y, cameraInOutSpeed);
			newVec.z = Mathf.Lerp(newVec.z,thisPointLoc.z, cameraInOutSpeed);

			mainCamera.transform.position = newVec;
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.LookRotation(realPointLoc - mainCamera.transform.position), cameraInOutSpeed);
			mainCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, pointFOV, cameraInOutSpeed);
			//the camera came nearer close to a point
			if(!CamOnNewPos && Vector3.Distance(mainCamera.transform.position,thisPointLoc) < maxDistanceToPoint/10)
			{
				CamOnNewPos = true;
			}
		}
		else // driving camera to initial point
		{
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, startCamPoint, cameraInOutSpeed);
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, initialCamRot, cameraInOutSpeed);
			mainCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialCamFOV, cameraInOutSpeed);
			// the camera came nearer close to initial point
			if(CamOnNewPos && Vector3.Distance(mainCamera.transform.position, startCamPoint) < maxDistanceToPoint - 1.0f)
			{
				CamOnNewPos = false;
			}

			// 
			if( Input.GetMouseButtonDown(0) )
			{
				firstClick = Input.mousePosition;
				CheckClickPoint_MTP(firstClick);
			}
		}
	}

/**********************************************************************************************************************************************************************
 *  Other helper methods
 * *********************************************************************************************************************************************************************/

	protected void InputMethod_MTP()
	{
		// move camera back on right moise button
		if(Input.GetMouseButtonUp(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace))
		{
			if(CamOnNewPos)
				SendCameraBack = true;
		}
	}
	
	/** WASD moving (call from RTSCamera)*/
	protected void InputMethod_RTS()
	{
		
		if( Input.GetMouseButtonDown(0) )
		{
			StartMoving();
			leftMouseBtnDown = true;
			movingToObject   = false;
		}
		
		if( Input.GetMouseButtonUp(0) )
		{
			StopMoving();
			leftMouseBtnDown = false;
		}

		// Movement Of screen Edge by mouse control
		if(mouseScreenEdgeMovement && (!leftMouseBtnDown && canMoveByClick || !canMoveByClick))
		{
			float amountX = 1.0f;
			float amountY = 1.0f;
			moveBorderWidth = Mathf.Clamp(moveBorderWidth, 0.0f, Screen.width/2);
			Vector2 currMousePos = Input.mousePosition;
			//Left
			if(currMousePos.x < moveBorderWidth)
			{
				amountX = (moveBorderWidth - currMousePos.x)/moveBorderWidth;
				if(moveBorderSide[0] == 0)
				{
					StartMoving();
					moveBorderSide[0] = 1;
				}
			}
			else if(moveBorderSide[0] == 1)
			{
				moveBorderSide[0] = 0;
			}
			// Right
			if(currMousePos.x > Screen.width - moveBorderWidth)
			{ 
				amountX = (currMousePos.x - (Screen.width - moveBorderWidth))/moveBorderWidth;
				if(moveBorderSide[1] == 0)
				{
					StartMoving();
					moveBorderSide[1] = 1;
				}
			}
			else if(moveBorderSide[1] == 1)
			{
				moveBorderSide[1] = 0;
			}
			// Back
			if(currMousePos.y < moveBorderWidth)
			{
				amountY = (moveBorderWidth - currMousePos.y)/moveBorderWidth;
				if(moveBorderSide[2] == 0)
				{
					StartMoving();
					moveBorderSide[2] = 1;
				}
			}
			else if(moveBorderSide[2] == 1)
			{
				moveBorderSide[2] = 0;
			}
			// Forward
			if(currMousePos.y > Screen.height - moveBorderWidth)
			{
				amountY = (currMousePos.y - (Screen.height - moveBorderWidth))/moveBorderWidth;
				if(moveBorderSide[3] == 0)
				{
					StartMoving();
					moveBorderSide[3] = 1;
				}
			}
			else if(moveBorderSide[3] == 1)
			{
				moveBorderSide[3] = 0;
			}
			int sum = moveBorderSide[0] + moveBorderSide[1] + moveBorderSide[2] + moveBorderSide[3];

			if(sum == 0 && startMove)
			{
				if(!KeyCodeD && !KeyCodeS && !KeyCodeA && !KeyCodeW)
					StopMoving();
			}
			else
			{
				if(sum > 0)
				{
					Vector2 vectt = (new Vector2(Screen.width/2, Screen.height/2) - currMousePos).normalized;
					vectt.x *= amountX;
					vectt.y *= amountY;
					delta = vectt * keyCamSpeed;
				}
			}
		}

		if(!useKeyBoardControl)
			return;

		// for Q
		if(Input.GetKeyDown(KeyCode.Q))
		{
			KeyCodeQ = true;
		}
		if(Input.GetKeyUp(KeyCode.Q))
		{
			KeyCodeQ = false;
		}

		// for E
		if(Input.GetKeyDown(KeyCode.E))
		{
			KeyCodeE = true;
		}
		if(Input.GetKeyUp(KeyCode.E))
		{
			KeyCodeE = false;
		}

		if(KeyCodeQ)
			deltaAltitude = zoomSensitivity;
		if(KeyCodeE)
			deltaAltitude = -zoomSensitivity;


		if(!leftMouseBtnDown)
		{
			if(KeyCodeW)
				delta.y = -keyCamSpeed;
			if(KeyCodeD)
				delta.x = -keyCamSpeed;
			if(KeyCodeS)
				delta.y = keyCamSpeed;
			if(KeyCodeA)
				delta.x = keyCamSpeed;

			// for W
			if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				KeyCodeW = true;
				StartMoving();
			}
			if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			{
				KeyCodeW = false;
				if(!KeyCodeD && !KeyCodeS && !KeyCodeA)
					StopMoving();
			}
			// for D
			if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				KeyCodeD = true;
				StartMoving();
			}
			if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				KeyCodeD = false;
				if(!KeyCodeW && !KeyCodeS && !KeyCodeA)
					StopMoving();
			}
			// for A
			if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				KeyCodeA = true;
				StartMoving();
			}
			if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				KeyCodeA = false;
				if(!KeyCodeD && !KeyCodeS && !KeyCodeW)
					StopMoving();
			}
			// for S
			if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				KeyCodeS = true;
				StartMoving();
			}
			if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			{
				KeyCodeS = false;
				if(!KeyCodeD && !KeyCodeW && !KeyCodeA)
					StopMoving();
			}
		}
	}

	/** event on click or keyboard down. RTS*/
	protected void StartMoving()
	{
		startMove 		 = true;
		lastPos 		 = Input.mousePosition;
		firstClick 		 = Input.mousePosition;
		delta			 = new Vector3(0,0,0);
		lastFingerPos	 = Vector2.zero;
	}

	/** event on clickUP or keyboard UP. RTS*/
	protected void StopMoving()
	{
		startMove  = false;
		firstClick = new Vector3(0,0,0);
		if(canTouchObjects && Vector2.Distance(lastFingerPos, new Vector2(0,0)) < 30)
			CheckClickPoint_RTS(Input.mousePosition);
	}

	/** */
	static Vector3 CheckTouchDistance(Vector3 _olddelta, Vector3 _newpos, Vector3 _lastpos, float touchthreshold)
	{
		if(Vector3.Distance(_newpos, _lastpos) > touchthreshold)
		{
			return _olddelta;
		}
		else
		{
			Vector3 _newvalue = _newpos - _lastpos;
			return _newvalue;
		}
	}
	
	/** */
	static float ClampAngle (float angle, float min, float max ) {
		if (angle < -MAX_ANGLE)
			angle += MAX_ANGLE;
		if (angle > MAX_ANGLE)
			angle -= MAX_ANGLE;
		return Mathf.Clamp (angle, min, max);
	}
	
	/** */
	protected void ResetFingerMoveDist()
	{
		isResetFingerMoveDist    = true;
		resetFingerMoveDistTimer = resetFingerMoveDistTime;
	}

	/**FOR DEMO  check click hit in 3d world || For MTP Camera*/
	protected  void CheckClickPoint_MTP(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			GameObject 	hitObj = hit.collider.gameObject;
			try
			{
				if(hitObj.tag == "Touch" && !toPoint)
				{
					if(!useDefaultCamPoint)
						startCamPoint = initialCamLoc;
					thisPointRot  = hitObj.transform.rotation;
					realPointLoc = hitObj.transform.position;
					Vector3 vec =  Quaternion.LookRotation(hitObj.transform.position - startCamPoint)* Vector3.forward;
					closeOffsetDistance = Mathf.Clamp(closeOffsetDistance, 0, Vector3.Distance(startCamPoint, realPointLoc));

					thisPointLoc = hitObj.transform.position - closeOffsetDistance * vec;
					currFOV  = initialCamFOV;
					toPoint  = true;
					maxDistanceToPoint = Vector3.Distance(mainCamera.transform.position, thisPointLoc);
				}
				else if(hitObj.tag == "TouchPerson")
				{
					if(hitObj.GetComponent<Animation>().IsPlaying("idle"))
						hitObj.GetComponent<Animation>().Play("jump_pose");
					else
						hitObj.GetComponent<Animation>().Play("idle");
				}
			}
			catch
			{
				Debug.LogWarning("Object is " + hitObj + ". Null? Remeber, Object tag should be Touch.");
			}
		}
	}
	
	/**FOR DEMO check click hit in 3d world || For RTS Camera*/
	protected void CheckClickPoint_RTS(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			GameObject 	hitObj = hit.collider.gameObject;
			try
			{
				if(hitObj.tag == "Touch")
				{
					Color col = hitObj.GetComponent<Renderer>().material.GetColor("_Color");
					if(col != (new Color(1, 0, 0, 1)))
						hitObj.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
					else
						hitObj.GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1));

					if(useMoveToObjectEffect)
					{
						selectedObjectPosition = hitObj.transform.position;
						movingToObject = true;
					}
				}
				else if(hitObj.tag == "TouchPerson")
				{
					if(hitObj.GetComponent<Animation>().IsPlaying("idle"))
						hitObj.GetComponent<Animation>().Play("jump_pose");
					else
						hitObj.GetComponent<Animation>().Play("idle");

					if(useMoveToObjectEffect)
					{
						selectedObjectPosition = hitObj.transform.position;
						movingToObject = true;
					}
				}
			}
			catch
			{
				Debug.LogWarning("Object is " + hitObj + ". Null? Object Tag should be Touch or TouchPerson.");
			}
		}
	}

	/** check existence of the main camera */
	protected bool CheckExistCameraComponent()
	{
		if(mainCamera == null)
		{
			CreateCamera();
			return false;
		}
		return true;
	}

	/** check existence of the camera */
	protected void CreateCamera()
	{
		try
		{
			Camera cam = gameObject.GetComponent<Camera>();
			if(cam == null)
			{
				gameObject.AddComponent<Camera>();
				cam = gameObject.GetComponent<Camera>();
				cam.tag = "MainCamera";
			}
			mainCamera = cam;
		}
		catch
		{
			Debug.LogWarning("No Camera on this object. Try to create Camera fail!");
		}
	}
}