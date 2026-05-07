using UnityEngine;
using UnityEngine.Events;

public class Element
{
}

[System.Serializable]
public class GunElement : Element
{
    public UnityEvent OnReload;
    [SerializeField] private GameObject _gunModel;
    [SerializeField] private string _name;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _ammunation; //Munição total
    [SerializeField] private float _clipSize;    //Capacidade do pente
    [SerializeField] private float _reloadTime;  //Tempo de recarga
    [SerializeField] private bool _hadScope;

    // --- NOVOS CAMPOS DE ÁUDIO ---
    [Header("Sons da Arma")]
    [SerializeField] private AudioClip _shootSound;  // Arraste o som do tiro aqui
    [SerializeField] private AudioClip _reloadSound; // Arraste o som do reload aqui

    private float _ammunationClip; // Balas no pente atual

    public GunElement(string name, float damage, float shootRate, float ammunation, float reloadTime)
    {
        _name = name;
        _damage = damage;
        _shootRate = shootRate;
        _ammunation = ammunation;
        _reloadTime = reloadTime;
    }

    public void Initialize()
    {
        _ammunationClip = _clipSize;
    }

    public bool UseAmmunation()
    {
        if (_ammunationClip <= 0)
        {
            if (_ammunation > 0)
            {
                OnReload.Invoke();
            }
            return false;
        }

        _ammunationClip--;
        return true;
    }

    public void Reload()
    {
        if (_ammunation <= 0)
            return;

        float ammunationToReload = _clipSize - _ammunationClip;

        if (ammunationToReload <= 0)
            return;

        if (_ammunation < ammunationToReload)
        {
            ammunationToReload = _ammunation;
        }

        _ammunationClip += ammunationToReload;
        _ammunation -= ammunationToReload;
    }

    // --- GETTERS PARA O GUNSYSTEM ACESSAR ---
    public string Name => _name;
    public float Damage => _damage;
    public float ShootRate => _shootRate;
    public float Ammunation => _ammunation;
    public float ReloadTime => _reloadTime;
    public bool HadScope => _hadScope;
    public GameObject GunModel => _gunModel;

    // Getters de áudio
    public AudioClip ShootSound => _shootSound;
    public AudioClip ReloadSound => _reloadSound;
}