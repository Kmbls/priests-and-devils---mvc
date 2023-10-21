using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public int move_state;
    public int onbank;
    public static float speed = 10f;
    // Start is called before the first frame update
    void Start(){
        move_state = 0;
        onbank = 1;
    }

    public int Move(){
        move_state = 1;
        if(onbank == 1)
            return 2;
        else
            return 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (move_state == 1 && onbank == 2){
            Vector3 direction = new Vector3(-4f,0f,0f) - transform.position;
            if (direction.magnitude > 0.1f){
                transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else{
                move_state = 0;
                onbank = 1;
            }
        }
        if (move_state == 1 && onbank == 1){
            Vector3 direction = new Vector3(4f,0f,0f) - transform.position;
            if (direction.magnitude > 0.1f){
                transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else{
                move_state = 0;
                onbank = 2;
            }
        }
    }

    public void Reset(){
        move_state = 0;
        onbank = 1;
        this.transform.position = new Vector3(-4f,0f,0f);
    }
    public int GetOnBank(){
        return onbank;
    }
}
