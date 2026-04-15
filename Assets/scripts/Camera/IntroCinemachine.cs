using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class IntroCinemachine : MonoBehaviour
{
    [Header("Câmeras Virtuais")]
    public GameObject camMaria;  
    public GameObject camZeBreu; 
    
    [Header("Atores")]
    public GameObject zeBreuVisual; 
    public MonoBehaviour movimentoMaria; 

    [Header("Configuração")]
    public float tempoDeTensao = 3f; 

    void Start()
    {
        if(movimentoMaria != null) movimentoMaria.enabled = false;

        camZeBreu.SetActive(true);
        camMaria.SetActive(false);

        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {
        yield return new WaitForSeconds(tempoDeTensao);

        camMaria.SetActive(true);
        camZeBreu.SetActive(false);

        yield return new WaitForSeconds(2f); 

        zeBreuVisual.SetActive(false);
        if(movimentoMaria != null) movimentoMaria.enabled = true;
    }
}