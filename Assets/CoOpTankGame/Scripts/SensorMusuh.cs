using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorMusuh : MonoBehaviour {

    // Use this for initialization
    
    

	void Start () {
		

    }
	
	// Update is called once per frame
	void Update () {
       GameObject [] tanks  =  GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i] != null)
            {
                if (tanks[i].transform.GetComponent<Tank>().id == this.transform.GetComponent<Tank>().id)
                {
                    continue;
                }
                else
                {
                    Vector3 vecMytank = this.transform.position;
                    Vector3 vecDirToEnemytank = tanks[i].transform.position - vecMytank;
                    Vector3 vecMytankUp = this.transform.up;

                    if (Vector3.Dot(vecMytankUp, vecDirToEnemytank.normalized) > 0.9 && vecDirToEnemytank.magnitude <= 13)
                    {
                        this.GetComponent<Tank>()._infEnemyLastPos = tanks[i].transform.position;
                        this.GetComponent<Tank>()._infEnemyLastdirection = tanks[i].transform.up;
                        this.GetComponent<Tank>()._infIsEnemySeen = true;

                      


                    }
                    else {
                        this.GetComponent<Tank>()._infIsEnemySeen = false;
                    }

                   // Debug.Log(this.GetComponent<Tank>()._infEnemyLastdirection + "  " + this.GetComponent<Tank>()._infEnemyLastPos + " " + this.GetComponent<Tank>()._infIsEnemySeen);
                    //        //Debug.Log(other.gameObject.name+ "  " + this.GetComponent<Tank>()._infEnemyLastPos);
                }

            }


        }


	}


    //void OnTriggerStay2D(Collider2D other)
    //{
    //    cekEnemy(other);
    //    //if (other.gameObject.tag == "Tank" && !other.isTrigger)
    //    //{
    //    //    this.GetComponent<Tank>()._infEnemyLastPos = other.gameObject.transform.position;
    //    //    this.GetComponent<Tank>()._infEnemydirection = other.gameObject.transform.up;
    //    //    //Debug.Log(other.gameObject.name+ "  " + this.GetComponent<Tank>()._infEnemyLastPos);
    //    //}
    //}

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    cekEnemy(col);
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    cekEnemy(other);
    //}


    //void cekEnemy(Collider2D other)
    //{

    //    if (other.gameObject.tag == "Tank" && !other.isTrigger)
    //    {
    //        this.GetComponent<Tank>()._infEnemyLastPos = other.gameObject.transform.position;
    //        this.GetComponent<Tank>()._infEnemyLastdirection = other.gameObject.transform.up;
    //        //Debug.Log(other.gameObject.name+ "  " + this.GetComponent<Tank>()._infEnemyLastPos);
    //    }
    //}

}
