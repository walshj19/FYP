using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationWebController : MonoBehaviour {

    public string formURL = "https://docs.google.com/forms/d/e/1FAIpQLSfrT3qc4-vFcViVM_RP5LP4OPxveAIi8gL7_Swy_abGRX0jDQ/viewform";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenFormLink()
    {
        try
        {
            Application.OpenURL(formURL);
        }
        catch(Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}
