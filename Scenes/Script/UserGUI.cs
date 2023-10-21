using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    public IUserAction action;
    // Start is called before the first frame update
	void Start () {
		action = SSDirector.getInstance ().currentSceneController as IUserAction;
	}

    // Update is called once per frame
    void OnGUI()
    {
        float width = Screen.width / 6;  
		float height = Screen.height / 12; 

		if (GUI.Button(new Rect(0, 0, width, height), "Reset")) {  
			action.Restart();  
		} 
        if(action.GetIsGameOver()){
            if(action.GetWinFlag()){
                GUI.Box(new Rect(2*width, 5*height, 2*width, 2*height),"YOU WIN!");
            }
            else{
                GUI.Box(new Rect(2*width, 5*height, 2*width, 2*height),"YOU LOSE!");
            }
        }
    }
}
