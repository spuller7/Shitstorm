using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private Rotate rotate;
    [SerializeField]
    private float speed = .5f;
    [SerializeField]
    private float thrusterForce = 1000f;
    [Header("Spring Settings:")]
    [SerializeField]
    private float jointSpring;
    [SerializeField]
    private float jointMaxForce;
    

    private ConfigurableJoint joint;
    private PlayerMotor motor;
    

	void Start () {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
	}

	void Update () {
        float _xMov = Input.GetAxisRaw("Horizontal");
        rotate.setXMov(_xMov);
        Vector3 _movHorizontal = Vector3.right * _xMov * speed;
        //Apply left/right movement
       motor.move(_movHorizontal);
       
        //calculate thruster force based on input
        Vector3 _thrusterForce = Vector3.zero;

        //Apply thruster force
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }else
        {
            SetJointSettings(jointSpring);
        }
        motor.ApplyThruster(_thrusterForce);
	}
    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };

    }

    private void flipCharacter()
    {

    }

}
