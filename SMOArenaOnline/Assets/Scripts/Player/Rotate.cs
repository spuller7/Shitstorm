using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    [SerializeField]
    private Animator anim;
    private float _xMov;
    private float angle;
    public float smooth = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update() {
        
        if (_xMov > 0)
        {
            anim.SetBool("rightDown", true);
            anim.SetBool("leftDown", false);
        }
        else if (_xMov < 0){
            anim.SetBool("leftDown", true);
            anim.SetBool("rightDown", false);
        }
        else
        {
            anim.SetBool("leftDown", false);
            anim.SetBool("rightDown", false);
        }
    }
    public void setXMov(float _xMov)
    {
        this._xMov = _xMov;
    }
}
