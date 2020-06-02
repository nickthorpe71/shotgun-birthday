using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject slashSound;
    public GameObject swordObj;
    MeshRenderer swordSkin;
    //public Transform slashPoint;
    public Animator swordAnim;
    public bool canSlash = true;
    public BoxCollider hitBox;
    public Material laserMat;
    public Material metalMat;

    private void Start()
    {
        canSlash = true;
        swordSkin = swordObj.GetComponent<MeshRenderer>();
    }

    public void Slash()
    {
        StartCoroutine(SlashRoutine());
    }

    IEnumerator SlashRoutine()
    {
        hitBox.enabled = true;
        swordAnim.SetBool("Slash", true);
        canSlash = false;
        swordSkin.material = laserMat;
        //Instantiate(slashSound, transform.position, transform.position);
        yield return new WaitForSeconds(0.4f);
        swordAnim.SetBool("Slash", false);
        canSlash = true;
        swordSkin.material = metalMat;
        hitBox.enabled = false;
    }
}
