using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour {
    
    private ObjectUIBehavior _ui_script;

    private delegate void UpdateList();
    private UpdateList _update_statement;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        this._event_processor();

        if(this._update_statement != null) this._update_statement();
    }


    private void _event_processor()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit = new RaycastHit();
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, 100.0f))
                this._grab_setup(_hit.collider);
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            this._release_setup();
        }
    }

    private void _grab_setup(Collider other)
    {   
        

        if (this._update_statement != this._grab_update)
        {
            this._update_statement = new UpdateList(this._grab_update);

            this._ui_script = other.GetComponent<ObjectUIBehavior>() as ObjectUIBehavior;

            this._ui_script.Picked();
        }
        else return;
    }

    private void _grab_update()
    {

    }

    private void _release_setup()
    {

        if (this._update_statement != this._release_update)
        {
            this._update_statement = new UpdateList(this._release_update);

            //Throwing or Releasing is determined here.
            //this._ui_script.Released();
            this._ui_script.Throwed();
            this._ui_script = null;
        }
        else return;
    }

    private void _release_update()
    {

    }

    
}
