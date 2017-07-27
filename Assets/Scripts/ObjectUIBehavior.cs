using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUIBehavior : MonoBehaviour
{


    private delegate void UpdateList();
    private UpdateList _updatelists;

    private Transform _ts_pivot;
    private Transform _ts_child;

    private Material _mt_child;
    private Color _cl_chlid;

    private Vector3 _rotation_velocity;
    private Vector3 _rotation;

    private Vector3 _accelate;
    private Vector3 _velocity;
    private Vector3 _position;

    // Use this for initialization
    void Start()
    {
        this._ts_child = this.transform;
        this._ts_pivot = this.transform.parent.GetComponentInParent<Transform>();

        this._mt_child = this.GetComponent<Renderer>().material;
        this._cl_chlid = Color.white;

        this._position.x = Random.Range(-5.0f, 5.0f);
        this._position.y = Random.Range(1.0f, 2.0f);
        this._position.z = Random.Range(-1.0f, -2.0f);

        this._ts_pivot.localPosition = this._position;


    }

    // Update is called once per frame
    void Update()
    {
        if (this._updatelists != null) this._updatelists();


        this._mt_child.color = this._cl_chlid;

        this._ts_pivot.localPosition = this._position;
        this._ts_child.eulerAngles = this._rotation;
    }


    public void Picked()
    {
        //when is it picked, it will do once
        this._updatelists = new UpdateList(this._pickedUpdate);

        this._rotation_velocity.x = 0.0f; //Random.Range(-1.0f, 1.0f) * 180.0f;
        this._rotation_velocity.y = Random.Range(-0.2f, 0.2f) * 180.0f;
        this._rotation_velocity.z = Random.Range(-0.2f, 0.2f) * 180.0f;
        this._accelate = Vector3.zero;
    }

    private void _pickedUpdate()
    {
        this._rotation += this._rotation_velocity * Time.deltaTime;
        this._rotation.x = (this._rotation.x + 360.0f) % 360.0f;
        this._rotation.y = (this._rotation.y + 360.0f) % 360.0f;
        this._rotation.z = (this._rotation.z + 360.0f) % 360.0f;


        //Mouse Mapping With Camera
        Vector3 prev = this._position;

        Vector3 mpos = Input.mousePosition;

        mpos.z = this._position.z - Camera.main.transform.localPosition.z;

        this._position = Camera.main.ScreenToWorldPoint(mpos);//this._velocity * Time.deltaTime;
        this._position.z = prev.z;

        this._rotation += -this._rotation * Time.deltaTime;

        Vector3 oldacc = this._accelate * (1.0f - Time.deltaTime);
        Vector3 newacc = (this._position - prev);
        
        this._accelate = this._accelate * 0.9f + newacc * 6.0f;
        this._accelate.z = Mathf.Sign(this._accelate.y) * Random.Range(10.0f, 20.0f);
    }



    public void Turnback()
    {
        //when is it Released, it will do once
        this._updatelists = new UpdateList(this._turnbackUpdate);

        //this._cl_chlid.a = 0.0f;
        //this._position.x = Random.Range(-5.0f, 5.0f);
        //this._position.y = Random.Range(1.0f, 2.0f);
        //this._position.z = Random.Range(-1.0f, -2.0f);
    }

    private void _turnbackUpdate()
    {
        //this._cl_chlid.a += (1.0f - this._cl_chlid.a) * Time.deltaTime;

        //this._rotation.x = (this._rotation.x) % 360.0f;
        //this._rotation.y = (this._rotation.y) % 360.0f;
        //this._rotation.z = (this._rotation.z) % 360.0f;

        if (this._position.magnitude > 10.0)
        {
            this._velocity += -2.0f * this._position.normalized * Time.deltaTime * 10.0f;
        }
        else
        {
            this._velocity += -0.5f * (this._velocity.magnitude * this._velocity.magnitude) * this._velocity.normalized * Time.deltaTime;
        }

        this._position += this._velocity * Time.deltaTime;
        this._rotation += -this._rotation_velocity * Time.deltaTime;
    }

    public void Throwed()
    {
        //when is it Released, it will do once
        this._updatelists = new UpdateList(this._throwedUpdate);
        
        this._velocity = this._accelate;


        this._rotation_velocity.x = 0.0f; //Random.Range(-1.0f, 1.0f) * 180.0f;
        this._rotation_velocity.y = Random.Range(-1.0f, 1.0f) * 180.0f;
        this._rotation_velocity.z = Random.Range(-1.0f, 1.0f) * 180.0f;

    }

    private void _throwedUpdate()
    {
        this._velocity += - 0.5f * (this._velocity.magnitude * this._velocity.magnitude) * this._velocity.normalized * Time.deltaTime;


        this._position += this._velocity * Time.deltaTime;
        this._rotation += -this._rotation * Time.deltaTime;
        
        if (this._position.magnitude > 10.0)
        {
            this.Turnback();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null) this._velocity.y *= -1.0f;
    }
}
