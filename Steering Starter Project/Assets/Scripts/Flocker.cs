﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Kinematic
{
    public GameObject myCohereTarget;
    BlendedSteering blendedSteering;
    Kinematic[] kBirds;

    private void Start()
    {
        //Separate
        Separation separation = new Separation();
        separation.character = this;
        GameObject[] goBirds = GameObject.FindGameObjectsWithTag("Bird");

        kBirds = new Kinematic[goBirds.Length - 1];

        int a = 0;
        for(int i = 0; i < goBirds.Length - 1; i++)
        {
            if(goBirds[i] == this)
            {
                continue;
            }
            kBirds[a++] = goBirds[i].GetComponent<Kinematic>();
        }
        separation.targets = kBirds;

        //Cohere
        Arrive cohere = new Arrive();
        cohere.character = this;
        cohere.target = myCohereTarget;

        //Rotate
        LookWhereGoing myRotateType = new LookWhereGoing();
        myRotateType.character = this;

        blendedSteering = new BlendedSteering();
        blendedSteering.behaviors = new WeightedBehavior[3];
        blendedSteering.behaviors[0] = new WeightedBehavior();
        blendedSteering.behaviors[0].behavior = separation;
        blendedSteering.behaviors[0].weight = 1f;
        blendedSteering.behaviors[1] = new WeightedBehavior();
        blendedSteering.behaviors[1].behavior = cohere;
        blendedSteering.behaviors[1].weight = 1f;
        blendedSteering.behaviors[2] = new WeightedBehavior();
        blendedSteering.behaviors[2].behavior = myRotateType;
        blendedSteering.behaviors[2].weight = 1f;
    }
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate = blendedSteering.getSteering();
        base.Update();
    }
}
