using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UIElements;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchForce;
    [SerializeField] float swordGravity;
    [SerializeField] float returnSpeed;

    [SerializeField] float freezeTimeDuration = 0.7f;

    [Header("Bounce info")]
    [SerializeField] bool isBouncing;
    [SerializeField] int bounceAmount = 4;
    [SerializeField] float bounceSpeed;
    [SerializeField] float bounceGravity;
    

    [Header("Pierce info")]
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;

    [Header("Spin info")]
    [SerializeField] float hitCooldown = .35f;
    [SerializeField] float maxTravelDistance = 7f;
    [SerializeField] float spinDuration = 2;
    [SerializeField] float spinGravity = 1;

    Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] int numberOfDots;
    [SerializeField] float spaceBetweenDots;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] Transform dotParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();
        GenerateDots();

        SetupGravity();

    }
    void SetupGravity()
    {
        if (this.swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (this.swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (this.swordType == SwordType.Spin) {
            swordGravity = this.spinGravity;
        }
    }

    override protected void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < this.dots.Length; i++)
            {
                this.dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }


    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, this.player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordController = newSword.GetComponent<Sword_Skill_Controller>();

        if (this.swordType == SwordType.Bounce)
        {
            newSwordController.SetupBounce(this.isBouncing, this.bounceAmount, this.bounceSpeed);
        }
        else if (this.swordType == SwordType.Pierce)
        {
            newSwordController.SetupPires(this.pierceAmount);
        }
        else if (this.swordType == SwordType.Spin) {
            newSwordController.SetupSpin(true, maxTravelDistance, spinDuration, this.hitCooldown);
        }

        newSwordController.SetupSword(finalDir, swordGravity, player, this.freezeTimeDuration, this.returnSpeed);

        player.AssignTheSword(newSword);

        DotsActive(false);
    }


    #region Aiming
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePositon - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < this.dots.Length; i++)
        {
            this.dots[i].SetActive(_isActive);
        }
    }

    void GenerateDots()
    {
        this.dots = new GameObject[numberOfDots];
        for (int i = 0; i < this.numberOfDots; i++)
        {
            this.dots[i] = Instantiate(this.dotPrefab, player.transform.position, Quaternion.identity, this.dotParent);
            this.dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position
            + new Vector2(
                AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y
              ) * t
            + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
