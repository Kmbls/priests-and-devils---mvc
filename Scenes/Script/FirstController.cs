using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public GameObject[] Chalist;
    public GameObject boat;
    public GameObject background;
    public GameObject cam;
    public int[] leftbank;
    public int[] rightbank;
    public int[] boatlist;
    public int remainseat;
    public bool winflag;

    bool isgameover;
    // Start is called before the first frame update
    void Awake () {
		SSDirector director = SSDirector.getInstance ();
		director.setFPS (60);
		director.currentSceneController = this;
		director.currentSceneController.LoadResources ();
	}

    public void LoadResources(){
        background = Instantiate<GameObject>(
            Resources.Load<GameObject> ("prefabs/background"),
            Vector3.zero, Quaternion.identity);
        background.name = "background";


        boat = Instantiate<GameObject>(
            Resources.Load<GameObject> ("prefabs/boat"),
            new Vector3(-4f,0,0), Quaternion.identity); 
        boat.name = "boat";
        boat.AddComponent<BoatMove>();

        Chalist = new GameObject[6];
        for (int i = 0;i<3;i++){
            Chalist[i] = Instantiate<GameObject>(
                Resources.Load<GameObject> ("prefabs/priest"),
                new Vector3(-6.5f,1f,-1f+i), Quaternion.identity);
            Chalist[i+3] = Instantiate<GameObject>(
                Resources.Load<GameObject> ("prefabs/devil"),
                new Vector3(-7.5f,1f,-1f+i), Quaternion.identity);
            Chalist[i].AddComponent<CharacterMove>();
            Chalist[i+3].AddComponent<CharacterMove>();
            Chalist[i].GetComponent<CharacterMove>().SetNumber(i);
            Chalist[i+3].GetComponent<CharacterMove>().SetNumber(i+3);
        }

        leftbank = new int[6]{1,1,1,-1,-1,-1};
        rightbank = new int[6]{0,0,0,0,0,0};

        isgameover = false;
        boatlist = new int[]{-1,-1};
        remainseat = 2;
    }

void Update () {
		if (!isgameover && Input.GetButtonDown("Fire1")) {
			Vector3 mp = Input.mousePosition;
			Camera ca;
			if (cam != null ) ca = cam.GetComponent<Camera> (); 
			else ca = Camera.main;
			Ray ray = ca.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
                if(hit.collider.gameObject != null){
                    string name = hit.collider.gameObject.name;
                    int number;
                    if (name == "boat" && remainseat < 2){
                        Debug.Log(leftbank);
                        Debug.Log(rightbank);
                        int gotobank;
                        gotobank = hit.collider.gameObject.GetComponent<BoatMove>().Move();
                        for(int i = 0;i<2;i++){
                            if(boatlist[i] != -1){
                                Chalist[boatlist[i]].GetComponent<CharacterMove>().Move();
                                if (gotobank == 1){
                                    rightbank[boatlist[i]] = 0;
                                }
                                else if (gotobank == 2){
                                    leftbank[boatlist[i]] = 0;
                                }
                            }
                        }
                        return;
                    }
                    if(name != "background"){
                        number = hit.collider.gameObject.GetComponent<CharacterMove>().GetNumber();
                        int seat = (boatlist[0] == -1) ? 1 : 2;
                        int chaonbank = hit.collider.gameObject.GetComponent<CharacterMove>().GetOnBank();
                        int boatonbank = boat.GetComponent<BoatMove>().GetOnBank();
                        if(chaonbank != boatonbank)
                            return;
                        bool onboat = hit.collider.gameObject.GetComponent<CharacterMove>().GetOnBoat();
                        if(remainseat <=0 && onboat == false)
                            return;
                        int onbank = hit.collider.gameObject.GetComponent<CharacterMove>().Move(seat,false);
                        if (!onboat){
                            boatlist[boatlist[0] == -1 ? 0 : 1] = number;
                            remainseat--;
                        }
                        else{
                            boatlist[boatlist[0] == number ? 0 : 1] = -1;
                            remainseat++;
                        }
                        if(onbank == 1)
                            leftbank[number] = number<3 ? 1 : -1;
                        if(onbank == 2)
                            rightbank[number] = number<3 ? 1 : -1;
                        check_if_lose();
                        check_if_win();
                    }
                }
			}
		}
	}

    void check_if_lose(){
        if ((leftbank.Sum() < 0  && leftbank.Take(3).Sum() > 0)|| (rightbank.Sum() < 0 && rightbank.Take(3).Sum() > 0)){
            winflag = false;
            GameOver();
        }
    }

    void check_if_win(){
        winflag = false;
        for(int i = 0;i<6;i++){
            if(rightbank[i] == 0)
                return;
        }
        winflag = true;
        GameOver();
    }
    public void GameOver(){
        if(winflag)
            Debug.Log("you win!");
        else
            Debug.Log("you lose!");
        isgameover = true;
    }

    public void Restart(){
        for(int i = 0;i<6;i++){
            Chalist[i].GetComponent<CharacterMove>().Reset();
        }
        boat.GetComponent<BoatMove>().Reset();
        leftbank = new int[6]{1,1,1,-1,-1,-1};
        rightbank = new int[6]{0,0,0,0,0,0};
        remainseat = 2;
        boatlist[0] = -1;
        boatlist[1] = -1;
        isgameover = false;
        winflag = false;
    }
    public bool GetIsGameOver(){
        return isgameover;
    }
    public bool GetWinFlag(){
        return winflag;
    }
}
