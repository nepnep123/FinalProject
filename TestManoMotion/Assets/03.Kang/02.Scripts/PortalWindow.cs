﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalWindow : MonoBehaviour
{
    public Material[] materials;

    private Transform device;

    private bool hasCollided;

    private void Awake()
    {
        device = Camera.main.transform;
    }

    void Start()
    {
        SetMaterials(false);
    }

    private void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;

        foreach (var mat in materials)
        {
            mat.SetInt("_StencilTest", (int)stencilTest);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != device) return;

        hasCollided = !hasCollided;

        SetMaterials(hasCollided);
    }
}