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

    //Arrays [] possuem tamanho fixo
    //Arrays são usados em iventarios visuais
    //armazenamento de referencias fixas


    //Listas <> possuem tamanho dinâmico
    //listas são boas para controle de inimigos
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
    [SerializeField]private GunElement _handGun;
    private float _shootTimer;
    private bool _isReloading;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main.transform;
        _handGun.Initialize();
        _shootTimer = _handGun.ShootRate;
        _handGun.OnReload.AddListener(() => StartCoroutine(Reload()));
        _gunInventory.AddWeapon(_handGun);
    }

    // Update is called once per frame
    void Update()
    {
        float currtenGunIndex = Input.GetAxis("Mouse ScrollWheel");
        if(currtenGunIndex != 0)
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
        //Verifica se o player atirou
        if (!Input.GetButtonDown("Fire1"))
            return;
        if (!_handGun.UseAmmunation())//Se não tiver munição, não é possível atirar
            return;
        //Verifica se o player acertou algo
        if (!Physics.Raycast(_camera.position, _camera.forward, out RaycastHit target))
            return;
        //Verifica se o objeto acertado implementa IShootable
        if (!target.collider.TryGetComponent(out IShootable shootable))
            return;

        //Aciona o método do contrato IShootable
        shootable.Hitted(1, target.point);
        _shootTimer = 0;
    }

    private void ChangeWeapon(float nextIndex)
    {
        if (_gunInventory.Guns.Count <= 1)
            return;

        int currentIndex = _gunInventory.Guns.IndexOf(_handGun);
        currentIndex += (int)Mathf.Sign(nextIndex);

        if (currentIndex == _gunInventory.Guns.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex == 0)
        {
            currentIndex = _gunInventory.Guns.Count - 1;
        }

        _handGun = _gunInventory.Guns[currentIndex];
        ChangeGunVisual();

    }

    IEnumerator Reload()
    {
        _isReloading = true;
        //Trava até ser verdadeiro
        //yield return new WaitUntil(() => _handGun.Ammunation > 0);
        //Trava enquanto for verdadeiro
        //yield return new WaitWhile(() => _handGun.Ammunation <= 0);
        yield return new WaitForSeconds(_handGun.ReloadTime);
        _handGun.Reload();
        _shootTimer = _handGun.ShootRate;//Deixa a arma já pronta para atirar
        _isReloading = false;
    }

    public void AddNewGun(GunElement newGun)
    {
        _handGun = newGun;
        _handGun.Initialize();
        _shootTimer = _handGun.ShootRate;
        _handGun.OnReload.AddListener(() => StartCoroutine(Reload()));
       ChangeGunVisual();
    }

    public void ChangeGunVisual()
    {
        Destroy(_handGunModeParent.GetChild(0).gameObject); //Remove o modelo da arma antiga
        GameObject gun = Instantiate(_handGun.GunModel, _handGunModeParent); //Instancia o modelo da nova arma
        gun.layer = LayerMask.NameToLayer("Gun");
        gun.transform.localPosition = new Vector3(0, 0, -gun.transform.localScale.z);
    }

}
