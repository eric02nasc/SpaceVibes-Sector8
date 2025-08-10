using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class Planet : MonoBehaviour
{
    
    public enum planetType { standart, oneTex, twoTex }
    public planetType PlanetType;

    public enum planetSize { small, medium, big }
    public planetSize PlanetSize;

    [Header("GameObjects")]
    public GameObject planet;
    public GameObject planetTexture;
    public GameObject planetTexture2;
    public GameObject moon;
    public GameObject player;

    public VisualEffect Vfx;
    public GameObject efeitoAtmosfera;

    private ParticleSystem planetParticles; // Referência para o sistema de partículas
    public Transform localEffect;

    [Header("Scripts")]
    public GameManeger gameManeger;
    

    [Header("Variables")]
    public bool haveMoon;
    public bool planetOk;
    public Color[] colorsBase;
    public Color[] colorsTexture;
    public Color[] colorsTexture2;
    public float distance;
    


    [Header("Sprites")]
    public Sprite planetSprite;
    public Sprite planetTextureSprite;
    public Sprite planetTexture2Sprite;
    public Sprite moonSprite;

    public bool specialPlanet;



   

    // Start is called before the first frame update
    void Start()
    {
        // Vfx.visualEffectAsset = gameManeger.visualEffect[Random.Range(0, gameManeger.visualEffect.Length - 1)].visualEffectAsset;
        //Color rosaNeon = new Color(253 / 255, 67 / 255, 255 / 255);
        //Vfx.SetVector4("color", rosaNeon);
        //Vfx.Reinit();
        

    }

    public void planetStart()
    {
        //CreatePlanetParticles();
        // Vfx = gameManeger.visualEffect[Random.Range(0, gameManeger.visualEffect.Length - 1)];
        
        Instantiate(gameManeger.visualEffectPrefab[Random.Range(0, gameManeger.visualEffectPrefab.Length )],localEffect.position,Quaternion.identity,transform);
        switch (PlanetType)
        {
            case planetType.standart:
                planet.GetComponent<SpriteRenderer>().sprite = planetSprite;
                planetTexture.SetActive(false);
                planetTexture2.SetActive(false);
                break;
            case planetType.oneTex:
                planet.GetComponent<SpriteRenderer>().sprite = planetSprite;
                planetTexture.GetComponent<SpriteRenderer>().sprite = planetTextureSprite;
                planetTexture.SetActive(true);
                planetTexture2.SetActive(false);
                break;
            case planetType.twoTex:
                planet.GetComponent<SpriteRenderer>().sprite = planetSprite;
                planetTexture.GetComponent<SpriteRenderer>().sprite = planetTextureSprite;
                planetTexture2.GetComponent<SpriteRenderer>().sprite = planetTexture2Sprite;
                planetTexture.SetActive(true);
                planetTexture2.SetActive(true);
                break;

        }

        if (haveMoon)
        {
            if (moon == null) { return; }
            moon.SetActive(true);
            moon.GetComponent<SpriteRenderer>().sprite = moonSprite;

            float planetRadius = GetComponent<CircleCollider2D>().radius;
            Vector2 moonPosition = Random.insideUnitCircle.normalized * (planetRadius + distance);

          //  Debug.Log("RAIO DO PLANETA: " + planetRadius);
            //Debug.Log("MOON POSITION " + moonPosition);

            while (Vector2.Distance(moonPosition, transform.position) < planetRadius)
            {
                moonPosition = Random.insideUnitCircle.normalized * (planetRadius + distance);
             //   Debug.Log("MOON POSITION " + moonPosition);

            }

            moon.transform.position = transform.position + new Vector3(moonPosition.x, moonPosition.y, 0f);



        }


        // Gerar um ângulo aleatório para a rotação em torno do eixo Z
        float randomAngle = Random.Range(0f, 360f);
        float randomAngle1 = Random.Range(0f, 360f);
        float randomAngle2 = Random.Range(0f, 360f);
        // Converter o ângulo para um Quaternion de rotação
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        Quaternion randomRotation1 = Quaternion.Euler(0f, 0f, randomAngle1);
        Quaternion randomRotation2 = Quaternion.Euler(0f, 0f, randomAngle2);

        // Aplicar a rotação aleatória ao objeto "planet"
        planet.transform.rotation = randomRotation;
        planetTexture.transform.rotation = randomRotation1;
        planetTexture.transform.rotation = randomRotation2;

        if (!planetOk)
        {

            int sorteioColorBase = Random.Range(0, colorsBase.Length );
            Debug.Log("COR BASE É : " + sorteioColorBase);
            planet.GetComponent<SpriteRenderer>().color = colorsBase[sorteioColorBase];
        }
        if (PlanetType == planetType.oneTex)
        {

            int sorteioColorText1 = Random.Range(0, colorsTexture.Length );
            planetTexture.GetComponent<SpriteRenderer>().color = colorsTexture[sorteioColorText1];
        }
        if (PlanetType == planetType.twoTex)
        {
            int sorteioColorText2 = Random.Range(0, colorsTexture2.Length );
            planetTexture2.GetComponent<SpriteRenderer>().color = colorsTexture2[sorteioColorText2];
        }
        switch (PlanetSize)
        {
            case planetSize.small:
                planet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            case planetSize.medium:
                planet.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                break;
            case planetSize.big:
                planet.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                break;

        }

        int special = Random.Range(0, 99);
        if(special > 90 )
        {
            specialPlanet = true;
        }

        if(specialPlanet)
        {
            efeitoAtmosfera.SetActive(true);
            efeitoAtmosfera.GetComponent<VisualEffect>().Play(); 
        }
        else
        {
            efeitoAtmosfera.SetActive(false);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void planetDestroy()
    {
        gameManeger.planets.Remove(this);
        Destroy(this.gameObject);

    }

    private void CreatePlanetParticles()
    {
        // Adiciona o componente ParticleSystem ao objeto do planeta
        planetParticles = gameObject.AddComponent<ParticleSystem>();

        // Configura as propriedades do sistema de partículas
        var mainModule = planetParticles.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color(0.5f, 0f, 1f, 1f), new Color(1f, 0f, 0.5f, 1f));
        mainModule.startSize = 0.2f;
        mainModule.startSpeed = 0f;
        mainModule.maxParticles = 50;
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(0.2f, 0.7f);

        // Configura as propriedades do módulo de emissão
        var emissionModule = planetParticles.emission;
        emissionModule.rateOverTime = 10f;

        // Configura as propriedades do módulo de forma
        var shapeModule = planetParticles.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Circle;
        shapeModule.radius = GetComponent<CircleCollider2D>().radius;

        // Configura as propriedades do módulo de luz
        var lightModule = planetParticles.lights;
        lightModule.enabled = true;
        lightModule.intensityMultiplier = 2f;
        lightModule.rangeMultiplier = 2f;
        lightModule.useRandomDistribution = true;
        var lightObject = new GameObject("ParticleLight");
        var lightComponent = lightObject.AddComponent<Light>();
        lightComponent.color = new Color(0.5f, 0f, 1f, 1f);
        lightModule.light = lightComponent;


        // Configura as propriedades do módulo de renderização
        var rendererModule = planetParticles.GetComponent<ParticleSystemRenderer>();
        rendererModule.material = new Material(Shader.Find("Sprites/Default"));

        // Configura a ordem de renderização do sistema de partículas
        rendererModule.sortingOrder = 1;

        // ativa o sistema de partículas
        planetParticles.Play();
    }

}
