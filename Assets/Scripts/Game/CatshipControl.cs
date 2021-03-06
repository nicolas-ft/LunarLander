﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatshipControl : MonoBehaviour
{
    public float rotSpeed, propulsion, tolerance;

    Transform tr;
    Rigidbody2D rb;
    Animator anim;

    Vector2 dirForce;
    float rbVel;

    void Awake() {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        Rotate();
        Propulsion();
        ControllAnim();
        rbVel = rb.velocity.magnitude;
    }

    void ControllAnim() {
        anim.SetBool("jetActive", dirForce.magnitude > 0.02f);
    }

    void Rotate() {
        float dirSpeed = Input.GetAxis("Horizontal");
        if (Mathf.Abs(dirSpeed) > 0.02f)
            tr.Rotate(0, 0, dirSpeed * -rotSpeed * Time.deltaTime);
    }

    void Propulsion() {
        dirForce = tr.TransformDirection(Vector3.up * Mathf.Clamp01(Input.GetAxis("Vertical")));

        if (dirForce.magnitude > 1)
            dirForce.Normalize();

        if (dirForce.magnitude > 0.02f)
            rb.AddForce(dirForce * propulsion, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D col) {
        string tag = col.gameObject.tag;

        Debug.Log(rbVel);

        if (tag == "GroundTilemap")
            GameState.instance.Perder();
        else if (tag == "GroundBase") {
            if (rbVel < tolerance)
                GameState.instance.Ganhar();
            else
                GameState.instance.Perder();
        }
    }
}
