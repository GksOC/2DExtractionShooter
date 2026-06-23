using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaController : MonoBehaviour
{

    private Camera mainCamera;
    private Vector2 mousePos;

    public Transform pontoDeDisparo;
    public GameObject projetil;

    // Start is called before the first frame update
    void Start()
    {
        //Configurar sempre a câmera como referência
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("[Jogador] Main Camera não encontrada na cena!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcaoOlhar = mousePos - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direcaoOlhar.y, direcaoOlhar.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(projetil, pontoDeDisparo.position, pontoDeDisparo.rotation);
        }
    }
}
