using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI orbCount;
    public Texture2D crosshair;

    private void Start()
    {
        setCursor(crosshair);
    }

    void setCursor(Texture2D tex)
    {
        CursorMode mode = CursorMode.ForceSoftware;
        float xspot = tex.width / 2f;
        float yspot = tex.height / 4f;
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(tex, hotSpot, mode);
    }

    public void updateUI(int orbs)
    {
        orbCount.text = "Orbs: " + orbs;
    }
}
