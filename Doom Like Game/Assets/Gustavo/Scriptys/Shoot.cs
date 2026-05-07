using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class Shoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;

    [Header("Combat & Audio")]
    [SerializeField] private float gunDamage = 1f; // Dano da arma
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;

    private Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // O tiro "físico" e o dano são chamados pela animação através da função Shoot() abaixo
            gunAnimator.SetTrigger("Fire");
        }
    }

    // ESTA FUNÇÃO É CHAMADA PELA ANIMAÇÃO
    void Shooter()
    {
        // 1. Som de Tiro
        if (audioSource != null && shotSound != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        // 2. Lógica de Dano (Raycast)
        // Atiramos do centro da câmera para onde o jogador está olhando
        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 100f))
        {
            // Verifica se o objeto atingido é o inimigo (IShootable)
            if (hit.collider.TryGetComponent(out IShootable enemy))
            {
                enemy.Hitted(gunDamage, hit.point);
                Debug.Log("Inimigo atingido através do SimpleShoot!");
            }
        }

        // 3. Efeito Visual (Muzzle Flash)
        if (muzzleFlashPrefab)
        {
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(tempFlash, destroyTimer);
        }

        // 4. Bala física (opcional, já que estamos usando Raycast para o dano)
        if (!bulletPrefab) return;
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }

    // ESTA FUNÇÃO É CHAMADA PELA ANIMAÇÃO PARA SOLTAR A CÁPSULA
    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab) return;

        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        Destroy(tempCasing, destroyTimer);
    }
}