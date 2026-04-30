using UnityEngine;

public class GunCollect : Item
{
    [SerializeField] private GunElement _attributes;

    public override Element Colect()
    {
        Destroy(gameObject);
        return _attributes;
    }

    protected override void Teste1()
    {
        throw new System.NotImplementedException();
    }

    //Se eu sobrescrevo o metódo virtual do Pai
    //Ao chamar no filho, o metódo do filho é executado
    protected override void Teste2()
    {
        Debug.Log("Teste2");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Teste2();
        Teste3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
