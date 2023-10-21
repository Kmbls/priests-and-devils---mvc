using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    // Start is called before the first frame update
    public int move_state;
    public int sitplace;
    public int onbank;
    public bool onboat;
    public int number;
    public static float speed = 10f;
    void Start(){
        move_state = 0;//0：静止，1：上船，2：坐船，3：下船
        onbank = 1;//1：处于左岸，2：处于右岸
        onboat = false;//0：不在船上，1：在船上
    }

    public void SetNumber(int number){
        this.number = number;
    }

    public int Move(int sitplace = -1, bool isfromboat = true){
        if(!isfromboat){
            if(sitplace != -1)
                this.sitplace = sitplace;
            if(move_state == 0 && onboat == false){
                move_state =1;
                onboat = true;
            }
            if(move_state == 0 && onboat == true){
                Debug.Log("readytogetoff");
                move_state = 3;
                onboat = false;
                return onbank;
            }
        }
        else if(move_state == 0 && onboat == true){
            move_state = 2;
        }
        return -1;
    }
    // Update is called once per frame
    void Update()
    {
        if((move_state == 1 && onboat == true && onbank == 1) || (move_state == 2 && onboat == true && onbank == 2)){
            Vector3 direction = new Vector3(-1f-2*sitplace,0.5f,0f) - transform.position;
            if (direction.magnitude > 0.1f){
                transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else{
                move_state = 0;
                onbank = 1;
            }
        }
        if((move_state == 1 && onboat == true && onbank == 2) || (move_state == 2 && onboat == true && onbank == 1)){
            Vector3 direction = new Vector3(7f-2*sitplace,0.5f,0f) - transform.position;
            if (direction.magnitude > 0.1f){
                transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else{
                move_state = 0;
                onbank = 2;
            }
        }
        if (move_state == 3 && onboat == false && onbank == 2){
            Vector3 direction = new Vector3(6.5f+(number>2?1:0),1f,-1f+number%3) - transform.position;
            if (direction.magnitude > 0.1f){
                transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else{
                move_state = 0;
            }
        }
        if (move_state == 3 && onboat == false && onbank == 1){
            Vector3 direction = new Vector3(-6.5f+(number>2?-1:0),1f,-1f+number%3) - transform.position;
            if (direction.magnitude > 0.1f){
                transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else{
                move_state = 0;
            }
        }
    }

    public void Reset(){
        move_state = 0;
        onbank = 1;
        onboat = false;
        this.transform.position = new Vector3(-6.5f+(number>2?-1:0),1f,-1f+number%3);
    }

    public int GetNumber(){
        return number;
    }

    public bool GetOnBoat(){
        return onboat;
    }
    public int GetOnBank(){
        return onbank;
    }
}
