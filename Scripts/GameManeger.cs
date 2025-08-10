using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameManeger : MonoBehaviour
{
    public List<Planet> planets;
    public PlayerController playerController;
    public GameObject atualPlanet;
    public Planet planet;
    

    public GameObject planetPrefab;
    public GameObject cometPrefab;
    public GameObject NebulaPrefab;
    public GameObject AtualComet;
    public GameObject AtualNebula;
    public GameObject Player;
    
    public bool isCreating;


    [Header("Texturas")]
    public Sprite[] planetOkSprites;
    public Sprite[] planetSprites;
    public Sprite[] textureSprite;
    public Sprite[] texture2Sprite;



    public VisualEffect[] visualEffect;
    public GameObject[] visualEffectPrefab;

    public float effectDistance = 5f;
    public float spawnDistance = 10f; // Distância de spawn do planeta em relação ao jogador
    public int maxPlanetCount = 10; // Número máximo de planetas permitidos no raio em torno do jogador
    public float minPlanetDistance = 5f; // Distância mínima entre os planetas
    public float variantSpawn;
    private Vector2 previousPlayerPosition; // Posição anterior do jogador


    public int criados;
    public int destruidos;
    public bool canCreate;
    public float createTime;

    public bool AndroidGame;

    // Start is called before the first frame update
    void Start()
    {
        previousPlayerPosition = Player.transform.position; // Inicializa a posição anterior do jogador
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (CountPlanetsInRadius() < maxPlanetCount)
        {
            Vector2 spawnPosition = GetSpawnPosition();

            if (!IsTooCloseToOtherPlanets(spawnPosition))
            {
                if (!canCreate) { return; }
                if (isCreating) { return; }
                isCreating = true;
                canCreate = false;

               
               
                {
                    GameObject newPlanet = Instantiate(planetPrefab, spawnPosition, Quaternion.identity);
                    planets.Add(newPlanet.GetComponent<Planet>());
                    createPlanet(newPlanet);
                    criados++;
                }

                if(criados == 120)
                {
                    if (AtualNebula != null)
                    {
                        Invoke("Locked", createTime);
                        isCreating = false;
                    }
                    else
                    {
                        if(!AndroidGame)
                        {
                            float randomAngle = Random.Range(0f, 360f);
                            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
                            GameObject newNebula = Instantiate(NebulaPrefab, spawnPosition, Quaternion.identity);
                            newNebula.transform.rotation = randomRotation;
                            Invoke("Locked", createTime);
                        }
                        
                        isCreating = false;
                        criados = 0;
                    }
                }

                if(criados == 60 || criados == 80 || criados == 100)
                {
                    if(AtualComet != null)
                    {
                        Invoke("Locked", createTime);
                        isCreating = false;
                    }
                    else
                    {
                        
                        GameObject newComet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);
                        AtualComet = newComet;
                        createComet(newComet);
                    }
                    
                }
                
            }
        }

        previousPlayerPosition = Player.transform.position;

        if(AtualComet != null)
        {
            float distanceComet = Vector2.Distance(AtualComet.transform.position, Player.transform.position);
            if (distanceComet > spawnDistance)
            {
                Destroy(AtualComet);
                AtualComet = null;
            }
        }

        if (AtualNebula != null)
        {
            float distanceNebula = Vector2.Distance(AtualNebula.transform.position, Player.transform.position);
            if (distanceNebula > spawnDistance)
            {
                Destroy(AtualNebula);
                AtualNebula = null;

            }
        }

        for (int i = planets.Count - 1; i>= 0; i--)
        {
            Planet planet = planets[i];
            float distance = Vector2.Distance(planet.transform.position, Player.transform.position);
            if (distance > spawnDistance)
            {
                if (planets.Contains(planet))
                {
                  //  Debug.Log("DESTRUINDO");
                    planets.Remove(planet);
                    Destroy(planet.gameObject);
                }
                destruidos++;
                //DestroyPlanet(planet);
                //planets.RemoveAt(i);
            }
            if(planet.specialPlanet)
            {
                if(distance > effectDistance)
                {
                    planet.efeitoAtmosfera.SetActive(false);
                    planet.efeitoAtmosfera.GetComponent<VisualEffect>().Stop();
                }
                else
                {
                    planet.efeitoAtmosfera.SetActive(true);
                    planet.efeitoAtmosfera.GetComponent<VisualEffect>().Play();
                }
            }
           
        }
    }

    private int CountPlanetsInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.transform.position, spawnDistance);

        int count = 0;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Planeta"))
            {
                count++;
            }
        }

        return count;
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 playerPosition = Player.transform.position;
        Vector2 playerDirection = playerPosition - previousPlayerPosition;

        if (playerDirection != Vector2.zero)
        {
            playerDirection.Normalize();
        }
        else
        {
            playerDirection = Vector2.up;
        }

        Vector2 ramdomizedDirection = Quaternion.Euler(0, 0, Random.Range(-variantSpawn, variantSpawn)) * playerDirection;
        Vector2 spawnPosition = playerPosition + ramdomizedDirection * spawnDistance;

        return spawnPosition;
    }

    private bool IsTooCloseToOtherPlanets(Vector2 position)
    {
        foreach (Planet planet in planets)
        {
            float distance = Vector2.Distance(position, planet.transform.position);
            if (distance < minPlanetDistance)
            {
                return true;
            }
        }

        return false;
    }


    public void createPlanet(GameObject newPlanet)
    {
        // if (isCreating) { return; }
        //isCreating = true;
        atualPlanet = newPlanet;
        planet = atualPlanet.GetComponent<Planet>();
        planet.player = Player;
        planet.gameManeger = GetComponent<GameManeger>();
       // planets.Add(atualPlanet.GetComponent<Planet>());

        int sorteioType = Random.Range(0, 100);
        int sorteioSize = Random.Range(0, 100);
        int sorteioMoon = Random.Range(0, 100);

        if (sorteioType < 33) 
        {
            planet.PlanetType = Planet.planetType.standart;

        }

        if (sorteioType >= 33 && sorteioType < 66)
        {
            planet.PlanetType = Planet.planetType.oneTex;

        }

        if (sorteioType >= 66)
        {
            planet.PlanetType = Planet.planetType.twoTex;

        }

        if (sorteioSize < 33)
        {
            planet.PlanetSize = Planet.planetSize.small;

        }

        if (sorteioSize >= 33 && sorteioSize < 66)
        {
            planet.PlanetSize = Planet.planetSize.medium;

        }

        if (sorteioSize >= 66)
        {
            planet.PlanetSize = Planet.planetSize.big;

        }

        if (sorteioMoon < 70)
        {
            planet.haveMoon = false;

        }

        if (sorteioMoon >= 70)
        {
            planet.haveMoon = true;

        }

        int lenghtSprites = planetSprites.Length;
        int lenghtTexture = textureSprite.Length;
        int lenghtTexture2 = texture2Sprite.Length;
        int lenghtSpriteOk = planetOkSprites.Length;

        int sorteioSprite = Random.Range(0, lenghtSprites );
        int sorteioTexture = Random.Range(0, lenghtTexture );
        int sorteioTexture2 = Random.Range(0, lenghtTexture2 );
        int sorteioSpriteOk = Random.Range(0, lenghtSpriteOk );

        if (planet.PlanetType == Planet.planetType.standart) 
        {
            
            int sorteioStandard = Random.Range(0, 100);
            if (sorteioStandard > 70)
            {
                planet.planetOk = false;
                planet.planetSprite = planetSprites[sorteioSprite];
            }
            else
            {
                planet.planetOk = true;
                planet.planetSprite = planetOkSprites[sorteioSpriteOk];
            }

        }

        if(planet.PlanetType == Planet.planetType.oneTex)
        {
            planet.planetSprite = planetSprites[sorteioSprite];
            planet.planetTextureSprite = textureSprite[sorteioTexture];
        }

        if (planet.PlanetType == Planet.planetType.twoTex)
        {
            planet.planetSprite = planetSprites[sorteioSprite];
            planet.planetTextureSprite = textureSprite[sorteioTexture];
            planet.planetTexture2Sprite = texture2Sprite[sorteioTexture2];
        }
        planet.planetStart();

        Invoke("Locked", createTime);
        isCreating = false;
    }

    public void createComet(GameObject newComet)
    {

        newComet.GetComponent<Comet>().gameManager = this;
        Invoke("Locked", createTime);
        isCreating = false;
    }

   


    private void DestroyPlanet(Planet planet)
    {
        if(planets.Contains(planet))
        {
          //  Debug.Log("DESTRUINDO");
            planets.Remove(planet);
            Destroy(planet.gameObject);
        }
       
    }

    public void Locked()
    {
        canCreate = true;
    }
}
