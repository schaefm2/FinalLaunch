using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public abstract class gunVariant {
    public string gunName = "gun";
    public int numberOfShotsTaken = 0;
    public int maxNumberOfShots = 0; // whatever mag size of gun is
    public float timeSinceLastShot = 0f;
    public float overheatingTime = 3;  // 3 seconds for gun to deheat and 3 second timer once overheated

    public bool isOverheating = false;

    public AudioClip gunReady;
    public Transform player;

    public virtual void GunInitialize(int setMaxNumberOfShots, float setOverheatingTime, AudioClip clip, Transform position) {
        maxNumberOfShots = setMaxNumberOfShots;
        overheatingTime = setOverheatingTime;
        gunReady = clip;
        player = position;
    }

    public abstract void shoot(); // override in each gun class for specific attributes

    public void HandleOverheating() {
        if (isOverheating)
        {
            overheatingTime -= Time.deltaTime;
            if (overheatingTime <= 0f)
            {
                isOverheating = false;
                overheatingTime = 3f;
                numberOfShotsTaken = 0;
                Debug.Log("Gun has stopped overheating...");
                playerItemController.instance.overheatText = "";
            }
            else
            {
                playerItemController.instance.overheatText = gunName + " is overheating!";
            }
            // return; // cant shoot while overheating
        }
    }

    public void shootingCooldown(ref float cooldownTimer, float cooldownInterval) {

        if (!isOverheating) {
            timeSinceLastShot += Time.deltaTime;

            if (timeSinceLastShot >= 2f) { // might need to make overheatingTime here a seperate variable incase of race condition
                cooldownTimer += Time.deltaTime;

                if (cooldownTimer >= cooldownInterval) {
                    // decreases shots and thus cools down gun if this check succeeds
                    if (numberOfShotsTaken > 0) {
                        Debug.Log("Deheating gun from " + numberOfShotsTaken + " To " + (numberOfShotsTaken - 1));
                        numberOfShotsTaken--; // deheats 1 shot from the gun every frame here
                        cooldownTimer = 0f;
                    }
                }
            }
        }
    }

}

public class Pistol: gunVariant {
    public Pistol(AudioClip clip, Transform position) {
        // maxNumberOfShots = 12;
        GunInitialize(12, 2, clip, position); // 12 shots for pistol
        gunName = "pistol";
    }

    public override void shoot() {
        if (numberOfShotsTaken >= maxNumberOfShots) {
            this.isOverheating = true;
            Debug.Log("Pistol is overheating");
            return;
        }
        numberOfShotsTaken++;
        Debug.Log("Pistol fired, total shots taken: " + numberOfShotsTaken);
    }
}


public class Rifle: gunVariant {
    public Rifle(AudioClip clip, Transform position) {
        // maxNumberOfShots = 30;
        GunInitialize(20, 3.5f, clip, position); // 30 shots for rifle
        gunName = "rifle";
    }

    public override void shoot() {
        if (numberOfShotsTaken >= maxNumberOfShots) {
            this.isOverheating = true;
            Debug.Log("Rifle is overheating");
            return;
        }
        numberOfShotsTaken++;
        Debug.Log("Rifle fired, total shots taken: " + numberOfShotsTaken);
    }
}

public class Shotgun : gunVariant
{
    public Shotgun(AudioClip clip, Transform position)
    {
        GunInitialize(10, 5, clip, position);
    }

    public override void shoot()
    {
        if (numberOfShotsTaken >= maxNumberOfShots)
        {
            this.isOverheating = true;
            Debug.Log("Shotgun is overheating");
            return;
        }
        numberOfShotsTaken+=5;
        Debug.Log("Shotgun fired, total shots taken: " + numberOfShotsTaken);
    }
}


public class Shooting : MonoBehaviour
{

    [SerializeField] private Transform projectileHitPoint;
    public Transform pistolMuzzle; // tip of the guns barrel for raycasting point
    public Transform rifleMuzzle;
    private Transform currentMuzzle;

    private Animator animator;
    private Ray ray;
    private RaycastHit hit;
    public float rayDistance = 100f;
    private LineRenderer lineRenderer;

    public Material pistolMaterial;
    public Material rifleMaterial;
    private ShootingTargetsManager targetsManager;

    private gunVariant currentGun;
    private Pistol pistol;
    private Rifle rifle;
    private Shotgun shotgun;

    // pistol over heating
    // private int numberOfShotsTaken = 0;
    // private int maxNumberOfShots = 12; // whatever mag size of gun is
    // private float timeSinceLastShot = 0f;
    // private float overheatingTime = 3;  // 3 seconds for gun to deheat and 3 second timer once overheated
    // private bool isOverheating = false;

    private float cooldownTimer = 0f;
    private float cooldownInterval = 0.25f; // every 0.25 seconds get 1 shot back

    public bool level1 = true;

    private StarterAssetsInputs _inputs;

    public AudioClip RifleSounEffect;
    public AudioClip PistolSoundEffect;
    private AudioClip LaserSoundFX;

    public AudioClip OverHeat;
    public AudioClip GunReady;

    public GameObject linePrefab;
    private List<GameObject> lines = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // GameObject GunObject = GameObject.Find("SciFiGunLightYellow");
        // gunCollider = GunObject.GetComponent<CapsuleCollider>();

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        lineRenderer.material = pistolMaterial;

        _inputs = GetComponent<StarterAssetsInputs>();

        if (level1)
        {
            targetsManager = FindObjectOfType<ShootingTargetsManager>();
        }

        pistol = new Pistol(GunReady, transform);
        rifle = new Rifle(GunReady, transform);
        shotgun = new Shotgun(GunReady, transform);

        currentGun = pistol;

        // rifleAmmo = playerItemController.instance.rifleAmmo;
        // pistolAmmo = playerItemController.instance.pistolAmmo;

        // _inputs.shoot = false; // otherwise first aiming in will shoot?

        currentMuzzle = pistolMuzzle;
    }

    void Update()
    {
        currentGun.HandleOverheating();
        currentGun.shootingCooldown(ref cooldownTimer, cooldownInterval);

        currentMuzzle = _inputs.weapon1 ? pistolMuzzle : rifleMuzzle;

        if (_inputs.weapon2)
        {
            currentGun = rifle;
            LaserSoundFX = RifleSounEffect;
            lineRenderer.material = rifleMaterial;

            if ((_inputs.rifleShoot) && _inputs.aim)
            {
                // Debug.Log("Shooting weapon");
                Debug.Log($"Player trying to shoot rifle while having {playerItemController.instance.rifleAmmo} rifle ammo");
                if ((currentGun.timeSinceLastShot >= 0.1f) && playerItemController.instance.rifleAmmo > 0)
                {
                    ShootWeapon();
                    playerItemController.instance.rifleAmmo--;
                }
                else if (playerItemController.instance.rifleAmmo < 1)
                { // out of ammo
                    Debug.Log("Rifle Ammo is Empty!");
                }
            }


        }
        else if (_inputs.weapon3)
        { // new weapon goes here
            currentGun = shotgun;
            LaserSoundFX = RifleSounEffect;
            lineRenderer.material = rifleMaterial;

            if (Input.GetMouseButtonDown(0) && _inputs.aim)
            {
                // Debug.Log("Shooting weapon");
                Debug.Log($"Player trying to shoot shotgun while having {playerItemController.instance.shotgunAmmo} shotgun ammo");
                if ((currentGun.timeSinceLastShot >= 0.1f) && playerItemController.instance.shotgunAmmo > 0)
                {
                    ShootWeapon();
                    playerItemController.instance.shotgunAmmo--;
                }
                else if (playerItemController.instance.shotgunAmmo < 1)
                { // out of ammo
                    Debug.Log("Shotgun Ammo is Empty!");
                }
            }
        }

        else
        {
            currentGun = pistol;
            LaserSoundFX = PistolSoundEffect;
            lineRenderer.material = pistolMaterial;

            // TODO: First time aiming in shoots because of how _inputs.shoot is set up I think, after first aim in its fixed though?

            if ((Input.GetMouseButtonDown(0)) && _inputs.aim)
            {
                // Debug.Log("Shooting weapon");
                Debug.Log($"Player trying to shoot pistol while having {playerItemController.instance.pistolAmmo} pistol ammo");
                if (playerItemController.instance.pistolAmmo > 0)
                {
                    ShootWeapon();
                    playerItemController.instance.pistolAmmo--;

                    _inputs.shoot = false;
                }
                else if (playerItemController.instance.pistolAmmo < 1)
                { // out of ammo
                    Debug.Log("Pistol Ammo is Empty!");
                }
            }

        }

    }

    private void ShootWeapon()
    {

        currentGun.shoot();
        currentGun.timeSinceLastShot = 0f;

        if (currentGun.isOverheating)
        {
            Debug.Log("Weapon is overheating, cannot shoot");
            AudioSource.PlayClipAtPoint(OverHeat, transform.position, 8f);
            return;
        }

        // Get the center of the screen in viewport coordinates
        // Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);
        // Vector3 worldCenter = Camera.main.ViewportToWorldPoint(screenCenter + new Vector3(0, 0, rayDistance));

        Vector3 centerOfCrosshair = projectileHitPoint.position;
        ray = new Ray(currentMuzzle.position, (centerOfCrosshair - currentMuzzle.position).normalized);

        if (currentGun is Shotgun)
        {

            for (int i = 0; i < 5; i++)
            {
                Vector3 direction = (centerOfCrosshair - currentMuzzle.position).normalized;
                Debug.Log(direction);
                Vector3 target = direction * 10f;
                Debug.Log(target);
                Vector3 randomizedV = target + Random.insideUnitSphere * .1f;
                Debug.Log(randomizedV);
                StartCoroutine(ShotGunShot(randomizedV));


                //lineRenderer.SetPosition(1, ray.origin + pelletDir * rayDistance);
            }
           

        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * rayDistance);
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                Debug.Log("Player shot raycasting hit: " + hit.collider.name);
                lineRenderer.SetPosition(1, hit.point);
                if (level1)
                {
                    targetsManager.TargetHit(hit.collider.gameObject);
                }
                if (hit.collider.CompareTag("Zombie"))
                {
                    // damage the zombie
                    hit.collider.GetComponent<ZombieController>().getShot(2f);
                }
            }
            else
            {
                Debug.Log("Player shot missed");
                lineRenderer.SetPosition(1, ray.origin + ray.direction * rayDistance);
            }
        }
        // Debug.Log("End of ShootWeapon function...");

        AudioSource.PlayClipAtPoint(LaserSoundFX, transform.position, 2f);
        StartCoroutine(DisableLineRenderer(0.05f));
    }

   

    private IEnumerator DisableLineRenderer(float time)
    {
        yield return new WaitForSeconds(time);
        lineRenderer.enabled = false;
    }

    private IEnumerator DisableShotgun(float time, GameObject line)
    {
        yield return new WaitForSeconds(time);
        Destroy(line);

    }

    private IEnumerator ShotGunShot(Vector3 newDir)
    {
        GameObject newLine = Instantiate(linePrefab);
        
        Ray shotgunRay = new Ray(currentMuzzle.position, newDir);

        LineRenderer render = new LineRenderer();
        render = newLine.GetComponent<LineRenderer>();
        render.startWidth = 0.03f;
        render.endWidth = 0.02f;
        render.positionCount = 2;
        render.enabled = true;
        render.SetPosition(0, shotgunRay.origin);
       
        if (Physics.Raycast(shotgunRay, out hit, rayDistance))
        {
            Debug.Log("Player shot raycasting hit: " + hit.collider.name);
            render.SetPosition(1, hit.point);
            if (level1)
            {
                targetsManager.TargetHit(hit.collider.gameObject);
            }
            if (hit.collider.CompareTag("Zombie"))
            {
                // damage the zombie
                hit.collider.GetComponent<ZombieController>().getShot(2f);
            }
        }
        else
        {
            Debug.Log("Player shot missed");
            render.SetPosition(1, ray.origin + ray.direction * rayDistance);
        }
        StartCoroutine(DisableShotgun(0.05f, newLine));

        //lineRenderer.SetPosition(1, ray.origin + pelletDir * rayDistance);
        yield break;
    }

}
