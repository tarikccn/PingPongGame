using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class racketsMove : MonoBehaviour
{

    private PhotonView pv;
    //Tus atamalari
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    //Raket hizi ve siniri
    public float speed = 10.0f;
    public float boundY = 4.3f;

    private Rigidbody2D rb;

    
    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        if (pv.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = new Vector2(7.45f, -0.046f);
                InvokeRepeating("Control", 0, 0.05f);

            }
            else if(!PhotonNetwork.IsMasterClient)
            {
                transform.position = new Vector2(-7.45f, -0.046f);
            }
        }
        
    }
    [PunRPC]
    //Oyuncu cikinca topun konumunu yeniden ortalıyoruz ve oyun durakliyor.
    public void leavePlayer()
    {
        InvokeRepeating("Control", 0, 0.5f);
        GameObject.FindWithTag("top").GetComponent<PhotonView>().RPC("ExitPlayer", RpcTarget.All, null);
    }
    //Oyuncu controlu : 2 kisi baglandiginda oyun basliyor.
    void Control()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.Find("ball").GetComponent<PhotonView>().RPC("GoBall", RpcTarget.AllBuffered, null);
            CancelInvoke("Control");
        }
    }

    //oyun icerisinde surekli guncel olmasini istedigimiz olaylari atadik.
    void Update()
    {       
        racketLimits();

        if (pv.IsMine)
        {
            moveRacket();
        }   
    }

    private void racketLimits()
    {        
        //sınırlar
        var pozisyon = transform.position;
        if (pozisyon.y > boundY)
        {
            pozisyon.y = boundY;
        }
        else if (pozisyon.y < -boundY)
        {
            pozisyon.y = -boundY;
        }
        transform.position = pozisyon;
    }

    private void moveRacket()
    {
        //hareketler
        var vel = rb.velocity; 
        if (Input.GetKey(moveUp))
        {
            vel.y = speed;
        }
        else if (Input.GetKey(moveDown))
        {
            vel.y = -speed;
        }
        else
        {
            vel.y = 0;
        }
        rb.velocity = vel;
    }
}
