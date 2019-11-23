using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    enum GunMode { NormalBullet=0, Flame};
    private int GunModeCount;

    [Header("Status")]
    public Rigidbody2D rb;
    public float MovementSpeed = 8; //Default = 8

    [Space(3)]
    public float JumpForce = 320; //Default = 320
    public float HoverForce = 10;
    public float HoverTime = .425f;
    private float HoverTime_Cur;
    public int JumpCount;
    private int JumpCount_Cur;

    //[Space(15)]
    private GunMode CurrentGunMode;
    private bool HasShotFlame;
    private bool PressedAttack;

    [Header("Shooting")]
    public GameObject Bullet;
    public Transform ShootPosition;
    public float DecayBeforeAutoShoot;
    private float DecayBeforeAutoShoot_fix;
    [Tooltip("Fire-rate per minute")]public float FireRate_Auto;
    private float DecayBetweenShoot;

    [Space(3)]
    public GameObject FlameBullet;
    private GameObject flame;
    private float FlameOffset = 0.9f;
    private ParticleSystem PS;

    //[Space(15)]
    [Header("Health")]
    public float MaxHealth;
    public float CurHealth;
    public RectTransform HealthBar;
    public RectTransform HealthBar_Bg;
    private float HealthBar_Width;

    [Header("Debug")]
    public Animator Anim;
    public SpriteRenderer CharacterSprite;
    public GameObject WaterTrails;
    Animator WaterAnim()
    {
        return WaterTrails.GetComponent<Animator>();
    }

    [Space(3)]
    public GameObject WaterSplash;
    public Transform WaterPos;

    [Space(3)]
    public LayerMask GroundLayer;

    public Vector3 ScreenPos;

    public bool OnGround;
    public bool Hop_jump;
    public bool Jump_Hold()
    {
        return Input.GetKey(btn_Jump);
    }
    public bool Hover_Jump;
    public bool IsAlive = true;

    public bool Hit = false;
    public float TempInvisTime;

    [Header("GameOver")]
    public GameObject Explosion;
    public GameOver Gameover_script()
    {
        return FindObjectOfType<GameOver>();
    }

    [Header("Button0")]
    public KeyCode btn_Jump = KeyCode.J;
    public KeyCode btn_MoveLeft = KeyCode.A;
    public KeyCode btn_MoveRight = KeyCode.D;
    public KeyCode btn_Attack1 = KeyCode.K;
    public KeyCode btn_SwitchGun = KeyCode.P;
    // Start is called before the first frame update

    void Start()
    {
        IsAlive = true;
        HealthBar_Width = HealthBar_Bg.GetComponent<RectTransform>().sizeDelta.x;

        HasShotFlame = false;
        CurrentGunMode = GunMode.NormalBullet;
        GunModeCount = (int)(GunMode.Flame) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        ScreenPos = Camera.main.WorldToViewportPoint(transform.position);
        //CONTROL BLIND
        {
            // Jumping part
            if (Input.GetKeyDown(btn_Jump))
            {
                Hop_jump = true;

                //! TEST
                //FindObjectOfType<TextGenerator>().CreateText(transform.position, "JUMP!", DisplayDamage.TextState.Green, 54);
            }

            if (Input.GetKeyUp(btn_Jump))
            {
                Hop_jump = false;
                Hover_Jump = false;
            }

            // Shooting part
            if (Input.GetKeyDown(btn_SwitchGun))
            {
                CurrentGunMode += 1;
            }

            switch ((GunMode)((int)CurrentGunMode % GunModeCount))
            {
                case GunMode.NormalBullet:
                    if (Input.GetKeyDown(btn_Attack1))
                    {
                        Shoot1();
                    }
                    if (Input.GetKey(btn_Attack1))
                    {
                        if (DecayBeforeAutoShoot_fix <= DecayBeforeAutoShoot)
                        {
                            DecayBeforeAutoShoot_fix += Time.deltaTime;
                        }
                        if (DecayBeforeAutoShoot_fix >= DecayBeforeAutoShoot)
                        {
                            ShootAuto();
                        }
                    }
                    if (Input.GetKeyUp(btn_Attack1))
                    {
                        DecayBeforeAutoShoot_fix = 0;
                    }
                    break;

                case GunMode.Flame:
                    //Debug.Log("HasShotFlame  PressedAttack");
                    //Debug.Log(HasShotFlame + " " + PressedAttack);

                    if (Input.GetKey(btn_Attack1))
                    {
                        PressedAttack = true;
                    }
                    else
                    {
                        PressedAttack = false;
                    }

                    if (!HasShotFlame && PressedAttack)
                    {
                        flame = ShootFlame();
                    }

                    if (HasShotFlame && !PressedAttack)
                    {
                        PS = flame.GetComponent<ParticleSystem>();
                        var main = PS.main;
                        main.loop = false;

                        HasShotFlame = false;
                    }
                    break;

            }
            
        }

        if (transform.position.y <= -8)
        {
            transform.position = new Vector3(transform.position.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (ScreenPos.x < -0.1)
        {
            IsAlive = false;
            Instantiate(Explosion, transform.position, Quaternion.identity);

            //Trigger GameOver script
            Gameover_script().IsGameEnd = true;
            Destroy(gameObject);
        }
    }

    // Update with smoother render
    public void FixedUpdate()
    {

        DisplayUI_Health();

        if (Input.GetKey(btn_MoveLeft))
        {
            if (ScreenPos.x >= .05f)
            {
                transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
                WaterAnim().speed = 0.5f;
            }
        }
        else if (Input.GetKey(btn_MoveRight))
        {
            if (ScreenPos.x <= .95f)
            {
                transform.Translate(Vector2.right * MovementSpeed * Time.deltaTime);
                WaterAnim().speed = 2f;
            }
        }
        else
        {
            WaterAnim().speed = 1f;
        }

        if (!OnGround && Jump_Hold())
        {
            if (Hover_Jump)
            {
                if (HoverTime_Cur > 0)
                {
                    rb.AddForce(Vector2.up * HoverForce * rb.mass * rb.gravityScale);
                    HoverTime_Cur -= Time.deltaTime;
                }
            }
        }

        //Jump command : Use !Hop_jump as command

        if (OnGround)
        {
            WaterTrails.SetActive(true);

            if (Hop_jump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * JumpForce * rb.mass * rb.gravityScale);
                Hop_jump = false; //!
                Hover_Jump = true;
            }
            HoverTime_Cur = HoverTime;
        }
        else //if(!OnGround)
        {
            WaterTrails.SetActive(false);
        }

        if(Hit)
        {
            Hit = !Hit;
            if (TempInvisTime <= 0)
            {
                TempInvisTime = 2.5f;
                StartCoroutine(TempDamagedInvincibleFrame());
            }
        }

        if(TempInvisTime > 0)
        {
            TempInvisTime -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //Create Water splash
            if(!OnGround)
            {
                var normal = collision.contacts[0].normal;
                if (normal.y > 0f)
                {
                    Instantiate(WaterSplash, WaterPos.position, Quaternion.identity);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            var normal = collision.contacts[0].normal; //! ERROR
            if (normal.y > 0f)
            {
                OnGround = true;
                Anim.SetBool("OnGround", true);
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            OnGround = false;
            Anim.SetBool("OnGround", false);
        }
    }
    
    //! Function Feature
    
    public void Shoot1()
    {
        Instantiate(Bullet, ShootPosition.position, Quaternion.identity);
    }
    public void ShootAuto()
    {
        if(Time.time >= DecayBetweenShoot)
        {
            Shoot1();
            DecayBetweenShoot = (60 / FireRate_Auto) + Time.time;
        }
        
    }
    public GameObject ShootFlame()
    {
        flame = Instantiate(FlameBullet, ShootPosition.position + new Vector3(FlameOffset, 0, 0), Quaternion.identity, transform);
        HasShotFlame = true;

        return flame;       
    }

    public void DisplayUI_Health()
    {
        float Ratio = (float)CurHealth / MaxHealth;
        HealthBar.sizeDelta = new Vector2(Ratio * HealthBar_Width, HealthBar.sizeDelta.y);

        //! GAMEOVER & Player Die
        if(CurHealth <= 0)
        {
            IsAlive = false;
            Instantiate(Explosion, transform.position, Quaternion.identity);

            //Trigger GameOver script
            Gameover_script().IsGameEnd = true;
            Destroy(gameObject);
        }
    }

    public void SayTextHit()
    {
        FindObjectOfType<TextGenerator>().CreateText(transform.position, "HIT!", DisplayDamage.TextState.Red, 54);
    }

    public IEnumerator TempDamagedInvincibleFrame()
    {
        CharacterSprite.color = new Color32(255, 255, 255, 30);
        yield return new WaitForSeconds(0.08f);
        CharacterSprite.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.08f);
        if (TempInvisTime > 0)
        {
            StartCoroutine(TempDamagedInvincibleFrame());
        }
    }

    public void GetDamage(float damage)
    {
        if (TempInvisTime <= 0)
        {
            CurHealth -= damage;
        }
    }
}
