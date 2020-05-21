﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text orbCount;

    public void updateUI(int orbs)
    {
        orbCount.text = "Orbs: " + orbs;
    }
}