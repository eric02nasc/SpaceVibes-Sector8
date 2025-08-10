using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    public Sprite[] sprites;
    public GameManeger gameManager;
    // Start is called before the first frame update


    public GameObject player;
    public Vector3 cometDirection;
    public float cometSpeed = 5f;
    public bool appliedForce = false;
    private bool setDirection = false;
    private Vector3 cometMovimentFinal;
    public float spinSpeed = 180f;
    void Start()
    {
        player = gameManager.Player;
        int sorteio = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[sorteio];
        GameObject effect = Instantiate(gameManager.visualEffectPrefab[2], this.gameObject.transform.position, Quaternion.identity, transform);
        effect.transform.localScale = new Vector3(4, 4, 4);
        cometDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
      
        if (!appliedForce)
        {
            if(!setDirection)
            {
                Vector3 playerDirection = (player.transform.position - transform.position).normalized;
                cometMovimentFinal = playerDirection * cometSpeed * Time.deltaTime;
                setDirection = true;
            }
            
            
            transform.Translate(cometMovimentFinal);
           // transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
            // appliedForce = true;
        }
    }
}
