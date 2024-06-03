using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    [Header("UI items for Spell Cooldown")]
    [Tooltip("Tooltip example")]
    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private Text textCooldown;

    //variable for looking after the cooldown
    private bool isCoolDown = false;
    private float cooldownTime = 10.0f;
    private float cooldownTimer = 0.0f;
    public PlayerState playerState;
    // Start is called before the first frame update

    private void Awake(){
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {   
        if(!playerState.gameOver && Input.GetKeyDown(KeyCode.Q) && playerState.spellCooldown){
            UseSpell();
            playerState.spellCooldown = false;
            Debug.Log(playerState.spellCooldown);
        }

        if(isCoolDown){
            ApplyCooldown();
        }
        //playerState.spellCooldown = false;
    }

    void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer < 0.0f)
        {
            isCoolDown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;

        }

    }

    public bool UseSpell()
    {
        if(isCoolDown)
        {
            return false;
        }
        else
        {
            isCoolDown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = 1.0f;

            return true; 
        }
    }
}
