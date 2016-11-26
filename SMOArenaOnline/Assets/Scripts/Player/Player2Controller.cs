using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PhotonView))]

public class Player2Controller : MonoBehaviour
{
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

    private bool isBoosting;
    private ConfigurableJoint joint;
    private PlayerMotor motor;
    private PlayerEffects pE;
    private PlayerBoost pB;
    [SerializeField]
    private PlayerShoot pS;


    void Start()
    {
        if (pS == null)
        {
            Debug.LogError("PlayerShoot needs to be assigned");
        }
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        isBoosting = false;
        SetJointSettings(jointSpring);
        pE = GetComponent<PlayerEffects>();
        pB = GetComponent<PlayerBoost>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //need to check if on cooldown

            Debug.Log("Noticed");
            pS.shoot();
            pE.onShoot();
        }

        float _xMov = Input.GetAxisRaw("Horizontal2");
        rotate.setXMov(_xMov);
        Vector3 _movHorizontal = Vector3.right * _xMov * speed;

        //Apply left/right movement
        motor.move(_movHorizontal);

        //calculate thruster force based on input
        Vector3 _thrusterForce = Vector3.zero;


        //Apply thruster force (up force)
        if (Input.GetButton("Jump2"))
        {
            if (pB.getFuel() > 0.0f)
            {
                pB.depleteFuel();
                if (isBoosting == false)
                {
                    isBoosting = true;
                    pE.setBoost(isBoosting);
                }

                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
            else
            {
                Debug.Log("Out of fuel");
            }
        }
        else
        {
            if (isBoosting == true)
            {
                isBoosting = false;
                PlayerEffects pE = GetComponent<PlayerEffects>();
                pE.setBoost(isBoosting);
            }
            SetJointSettings(jointSpring);
        }
        motor.ApplyThruster(_thrusterForce);
    }
    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };

    }

}
