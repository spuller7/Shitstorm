// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
// * Copyright 2015  All Rights Reserved.
// *
// * Please direct any bugs or questions to vadakuma@gmail.com
// *
// * author vadakuma
// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
using UnityEngine;
using System.Collections;
using UnityEditor;



[CustomEditor(typeof(CameraMove))]
[CanEditMultipleObjects]
[System.Serializable]
public class CameraScriptEditor : Editor 
{
	SerializedObject 		serObj;
	SerializedProperty 		cameraMode;

	/** 
	 * RPG CAMERA 
	 */
	SerializedProperty		followMode;
	SerializedProperty 		trtarget;
	SerializedProperty 		xSpeed;
	SerializedProperty 		ySpeed;
	SerializedProperty 		yMinLimit;
	SerializedProperty		yMaxLimit;
	SerializedProperty 		maxDistance;
	SerializedProperty 		minDistance;
	SerializedProperty		vectCamOffset;
	SerializedProperty 		touchThreshold;
	SerializedProperty 		fingerDistThreshold;
	SerializedProperty 		keyCamSpeed;

	/** 
	 * RTS CAMERA 
	 */
	SerializedProperty		inversionX;
	SerializedProperty		inversionY;
	SerializedProperty		useKeyBoardControl;
	SerializedProperty		useComboZoom;
	SerializedProperty		canTouchObjects;
	SerializedProperty		mouseScreenEdgeMovement;
	SerializedProperty		canMoveByClick;
	SerializedProperty		moveBorderWidth;
	SerializedProperty		useLockPerimeter;
	SerializedProperty		cameraSpeed;
	SerializedProperty		speedDampingSmoothness;
	SerializedProperty		leftBorder;
	SerializedProperty		rightBorder;
	SerializedProperty		forwardBorder;
	SerializedProperty		backBorder;
	SerializedProperty		useRollOutEffect;
	SerializedProperty		rollOutValue;
	SerializedProperty		rollOutValueSmoothness;
	SerializedProperty		moveToObjectSmoothness;
	SerializedProperty		maxZoom;
	SerializedProperty		minZoom;
	SerializedProperty		zoomRotAmount;
	SerializedProperty		FOVZoomMultyplier;
	SerializedProperty		camRotInten;
	SerializedProperty		zoomSensitivity; 
	SerializedProperty		Smoothness;

	/** 
	 * MTP CAMERA 
	 */
	SerializedProperty		startCamPoint;
	SerializedProperty 		useDefaultCamPoint;
	SerializedProperty 		cameraInOutSpeed;
	SerializedProperty 		closeOffsetDistance;

	CameraMove myScript;

	void OnEnable() {
		// Setup serialized property
		myScript = (CameraMove)target;

	}

	public override void OnInspectorGUI()
	{
		serObj = new SerializedObject (target);
		//Select Camera Mode
		cameraMode = serObj.FindProperty("Mode");

		switch(myScript.Mode)
		{
			case CameraMove.CameraModeNames.RPG_Camera:
				GUILayout.Label ("Role Play Game Camera(RPG)");
				break;
			case CameraMove.CameraModeNames.RTS_Camera:
				GUILayout.Label ("Real Time Strategy Camera(RTS)");
				break;
			case CameraMove.CameraModeNames.MTP_Camera:
				GUILayout.Label ("Move To Point Camera(MTP)");
				break;
			case CameraMove.CameraModeNames.None:
				GUILayout.Label ("None options");
				break;
		}

		EditorGUILayout.PropertyField (cameraMode, new GUIContent("Select Camera:"));	

		if(myScript.Mode == CameraMove.CameraModeNames.RPG_Camera)
		{
			////////////////////////////////////////////////////////////////////////////////////////
			EditorGUILayout.Separator ();
			////////////////////////////////////////////////////////////////////////////////////////
			trtarget = serObj.FindProperty("target");
			EditorGUILayout.PropertyField (trtarget, new GUIContent("Set Target"));	

			if(myScript.target != null)
			{
				followMode = serObj.FindProperty("followMode");
				EditorGUILayout.PropertyField (followMode, new GUIContent("Following Mode"));	
				xSpeed = serObj.FindProperty("xSpeed");
				EditorGUILayout.PropertyField (xSpeed, new GUIContent("X Mouse Speed"));	
				ySpeed = serObj.FindProperty("ySpeed");
				EditorGUILayout.PropertyField (ySpeed, new GUIContent("Y Mouse Speed"));	
				yMinLimit = serObj.FindProperty("yMinLimit");
				EditorGUILayout.PropertyField (yMinLimit, new GUIContent("Y min limit"));
				yMaxLimit = serObj.FindProperty("yMaxLimit");
				EditorGUILayout.PropertyField (yMaxLimit, new GUIContent("Y max limit"));
				zoomSensitivity = serObj.FindProperty("zoomSensitivity");
				EditorGUILayout.PropertyField (zoomSensitivity, new GUIContent("Zoom sensitivity"));
				maxDistance = serObj.FindProperty("maxDistance");
				EditorGUILayout.PropertyField (maxDistance, new GUIContent("Max zoom"));
				minDistance = serObj.FindProperty("minDistance");
				EditorGUILayout.PropertyField (minDistance, new GUIContent("Min zoom"));
				Smoothness = serObj.FindProperty("Smoothness");
				EditorGUILayout.PropertyField (Smoothness, new GUIContent("Smoothness of rotation"));
				//ToggleGroup
				myScript.isCamOffset = EditorGUILayout.BeginToggleGroup ("Use camera offset", myScript.isCamOffset);
					vectCamOffset = serObj.FindProperty("vectCamOffset");
					EditorGUILayout.PropertyField (vectCamOffset, new GUIContent("Add camera offset"));
				EditorGUILayout.EndToggleGroup ();
				/*touchThreshold = serObj.FindProperty("touchThreshold");
				EditorGUILayout.PropertyField (touchThreshold, new GUIContent("Touch threshold"));
				fingerDistThreshold = serObj.FindProperty("fingerDistThreshold");
				EditorGUILayout.PropertyField (fingerDistThreshold, new GUIContent("Finger|Mouse dist threshold"));*/
			}

			
		}
		else if(myScript.Mode == CameraMove.CameraModeNames.RTS_Camera)
		{
			////////////////////////////////////////////////////////////////////////////////////////
			EditorGUILayout.Separator ();
			////////////////////////////////////////////////////////////////////////////////////////
			inversionX = serObj.FindProperty("inversionX");
			EditorGUILayout.PropertyField (inversionX, new GUIContent("Inversion X axis"));

			inversionY = serObj.FindProperty("inversionY");
			EditorGUILayout.PropertyField (inversionY, new GUIContent("Inversion Y axis"));

			useKeyBoardControl = serObj.FindProperty("useKeyBoardControl");
			EditorGUILayout.PropertyField (useKeyBoardControl, new GUIContent("Use keyboard control"));	
			canTouchObjects = serObj.FindProperty("canTouchObjects");
			EditorGUILayout.PropertyField (canTouchObjects, new GUIContent("Can touch objects"));	

			myScript.mouseScreenEdgeMovement = EditorGUILayout.BeginToggleGroup ("Mouse screen edge movement", myScript.mouseScreenEdgeMovement);
			EditorGUI.indentLevel++;
			canMoveByClick = serObj.FindProperty("canMoveByClick");
			EditorGUILayout.PropertyField (canMoveByClick, new GUIContent("Can Move By Default"));
			EditorGUI.indentLevel--;
			EditorGUILayout.EndToggleGroup ();

			moveBorderWidth = serObj.FindProperty("moveBorderWidth");
			EditorGUILayout.PropertyField (moveBorderWidth, new GUIContent("Move edge width"));

			cameraSpeed = serObj.FindProperty("cameraSpeed");
			EditorGUILayout.PropertyField (cameraSpeed, new GUIContent("Camera speed"));
			speedDampingSmoothness = serObj.FindProperty("speedDampingSmoothness");
			EditorGUILayout.Slider(speedDampingSmoothness, 0.0F, 1.0F);
			keyCamSpeed = serObj.FindProperty("keyCamSpeed");
			EditorGUILayout.PropertyField (keyCamSpeed, new GUIContent("Keyboard camera speed"));
			maxZoom = serObj.FindProperty("maxZoom");
			EditorGUILayout.PropertyField (maxZoom, new GUIContent("Max zoom"));
			minZoom = serObj.FindProperty("minZoom");
			EditorGUILayout.PropertyField (minZoom, new GUIContent("Min zoom"));
			zoomSensitivity = serObj.FindProperty("zoomSensitivity");
			EditorGUILayout.PropertyField (zoomSensitivity, new GUIContent("Zoom sensitivity"));
			camRotInten = serObj.FindProperty("camRotInten");
			EditorGUILayout.PropertyField (camRotInten, new GUIContent("Camera tilt strength"));
			//ToggleGroup
			myScript.useLockPerimeter = EditorGUILayout.BeginToggleGroup ("Use locker perimeter", myScript.useLockPerimeter);
				EditorGUI.indentLevel++;
				leftBorder = serObj.FindProperty("leftBorder");
				EditorGUILayout.PropertyField (leftBorder, new GUIContent("Set leftBorder"));	
				rightBorder = serObj.FindProperty("rightBorder");
				EditorGUILayout.PropertyField (rightBorder, new GUIContent("Set rightBorder"));
				forwardBorder = serObj.FindProperty("forwardBorder");
				EditorGUILayout.PropertyField (forwardBorder, new GUIContent("Set forwardBorder"));
				backBorder = serObj.FindProperty("backBorder");
				EditorGUILayout.PropertyField (backBorder, new GUIContent("Set backBorder"));

				// check clamp
				myScript.leftBorder    = Mathf.Clamp(myScript.leftBorder, float.MinValue, myScript.rightBorder);
				myScript.rightBorder   = Mathf.Clamp(myScript.rightBorder, myScript.leftBorder, float.MaxValue);
				myScript.backBorder    = Mathf.Clamp(myScript.backBorder, float.MinValue, myScript.forwardBorder);
				myScript.forwardBorder = Mathf.Clamp(myScript.forwardBorder, myScript.backBorder, float.MaxValue);

				myScript.useRollOutEffect = EditorGUILayout.BeginToggleGroup ("Use RollOut Effect", myScript.useRollOutEffect);
					EditorGUI.indentLevel++;
					rollOutValue = serObj.FindProperty("rollOutValue");
					EditorGUILayout.PropertyField (rollOutValue, new GUIContent("RollOut Distance"));
					rollOutValueSmoothness = serObj.FindProperty("rollOutValueSmoothness");
					EditorGUILayout.Slider(rollOutValueSmoothness, 0.0F, 1.0F);
					EditorGUI.indentLevel--;
				EditorGUILayout.EndToggleGroup ();
				EditorGUI.indentLevel--;
			EditorGUILayout.EndToggleGroup ();

			myScript.useMoveToObjectEffect = EditorGUILayout.BeginToggleGroup ("Use MoveToObject Effect", myScript.useMoveToObjectEffect);
			EditorGUI.indentLevel++;
			moveToObjectSmoothness = serObj.FindProperty("moveToObjectSmoothness");
			EditorGUILayout.Slider(moveToObjectSmoothness, 0.0F, 1.0F);
			EditorGUI.indentLevel--;
			EditorGUILayout.EndToggleGroup ();

			//ToggleGroup
			myScript.useComboZoom = EditorGUILayout.BeginToggleGroup ("Use combo zoom", myScript.useComboZoom);
			EditorGUI.indentLevel++;
				zoomRotAmount = serObj.FindProperty("zoomRotAmount");
				EditorGUILayout.PropertyField (zoomRotAmount, new GUIContent("In zoom tilt  strength"));
				FOVZoomMultyplier = serObj.FindProperty("FOVZoomMultyplier");
				EditorGUILayout.PropertyField (FOVZoomMultyplier, new GUIContent("In zoom FOV change"));
			EditorGUI.indentLevel--;
			EditorGUILayout.EndToggleGroup ();
			////////////////////////////////////////////////////////////////////////////////////////
		}
		else if(myScript.Mode == CameraMove.CameraModeNames.MTP_Camera)
		{
			////////////////////////////////////////////////////////////////////////////////////////
			GUILayout.Label ("L Mouse Button - click on object for zoomIn");
			GUILayout.Label ("R Mouse Button|Space - click on object for zoomOut");
			EditorGUILayout.Separator ();
			////////////////////////////////////////////////////////////////////////////////////////
			cameraInOutSpeed = serObj.FindProperty("cameraInOutSpeed");
			EditorGUILayout.PropertyField (cameraInOutSpeed, new GUIContent("Camera InOut speed"));	
			closeOffsetDistance = serObj.FindProperty("closeOffsetDistance");
			EditorGUILayout.PropertyField (closeOffsetDistance, new GUIContent("Offset distance"));	
			//ToggleGroup
			myScript.useDefaultCamPoint = EditorGUILayout.BeginToggleGroup ("Use default camera point", myScript.useDefaultCamPoint);
				startCamPoint = serObj.FindProperty("startCamPoint");
				EditorGUILayout.PropertyField (startCamPoint, new GUIContent("Default cam point"));	
			EditorGUILayout.EndToggleGroup ();
		}
		else if(myScript.Mode == CameraMove.CameraModeNames.None)
		{
			EditorGUILayout.Separator ();
			GUILayout.Label ("None options");
			EditorGUILayout.Separator ();
		}

		if (GUI.changed)
			serObj.ApplyModifiedProperties();
	
		//DrawDefaultInspector();
	}
}