using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour, IColectable
{
    public abstract Element Colect();

    //MÉTODOS ABSTRATOS
    //Força os filhos a implementarem
    //Usado quando todos os filhos usam, mas com comportamentos diferentes
    //Não Declara o corpo, apenas a assinatura
    protected abstract void Teste1();

    //MÉTODOS VIRTUAIS
    //Permite que os filhos sobrescrevam o método, mas não obriga
    //Quando apenas alguns dos filhos tem comportamentos diferente
    protected virtual void Teste2()
    {
        //Corpo do método
    }

    //METÓDOS NORMAIS
    //Quando todos os filhos tem mesmo comportamento
    protected void Teste3()
    {

    }

}
