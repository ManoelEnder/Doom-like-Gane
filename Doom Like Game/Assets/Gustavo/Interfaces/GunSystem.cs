using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GunIventory
{
    [SerializeField] private List<GunElement> _guns;

    public List<GunElement> Guns { get => _guns; }

    public void AddWeapon(GunElement newGun)
    {
        Guns.Add(newGun);
    }
}

public class GunSystem : MonoBehaviour
{
    [SerializeField] private GunIventory _gunInventory;
    [SerializeField] private Transform _handGunModeParent;

    private Transform _camera;

    [SerializeField] private GunElement _handGun;

    private float _shootTimer;
    private bool _isReloading;

    void Start()
    {
        _camera = Camera.main.transform;

        _handGun.Initialize();

        _shootTimer = _handGun.ShootRate;

        _handGun.OnReload.AddListener(() => StartCoroutine(Reload()));

        _gunInventory.AddWeapon(_handGun);

        ChangeGunVisual();
    }

    void Update()
    {
        float currtenGunIndex = Input.GetAxis("Mouse ScrollWheel");

        if (currtenGunIndex != 0)
        {
            ChangeWeapon(currtenGunIndex);
        }

        if (Input.GetButtonDown("Reload"))
        {
            if (_handGun.Ammunation <= 0)
                return;

            _handGun.OnReload.Invoke();
        }

        _shootTimer += Time.deltaTime;

        if (_isReloading)
            return;

        if (_shootTimer < _handGun.ShootRate)
            return;

        if (!Input.GetButtonDown("Fire1"))
            return;

        if (!_handGun.UseAmmunation())
            return;

        if (!Physics.Raycast(_camera.position, _camera.forward, out RaycastHit target, _handGun.Range))
            return;

        if (!target.collider.TryGetComponent(out IShootable shootable))
            return;

        shootable.Hitted(_handGun.Damage, target.point);

        _shootTimer = 0;
    }

    private void ChangeWeapon(float nextIndex)
    {
        if (_gunInventory.Guns.Count <= 1)
            return;

        int currentIndex = _gunInventory.Guns.IndexOf(_handGun);

        currentIndex += (int)Mathf.Sign(nextIndex);

        if (currentIndex >= _gunInventory.Guns.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex = _gunInventory.Guns.Count - 1;
        }

        _handGun = _gunInventory.Guns[currentIndex];

        ChangeGunVisual();
    }

    IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(_handGun.ReloadTime);

        _handGun.Reload();

        _shootTimer = _handGun.ShootRate;

        _isReloading = false;
    }

    public void AddNewGun(GunElement newGun)
    {
        _gunInventory.AddWeapon(newGun);

        _handGun = newGun;

        _handGun.Initialize();

        _shootTimer = _handGun.ShootRate;

        _handGun.OnReload.AddListener(() => StartCoroutine(Reload()));

        ChangeGunVisual();
    }

    public void ChangeGunVisual()
    {
        if (_handGunModeParent.childCount > 0)
        {
            Destroy(_handGunModeParent.GetChild(0).gameObject);
        }

        GameObject gun = Instantiate(_handGun.GunModel, _handGunModeParent);

        gun.layer = LayerMask.NameToLayer("Gun");

        gun.transform.localPosition = new Vector3(0, 0, -gun.transform.localScale.z);
    }
}