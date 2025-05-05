using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public Material dissolveMaterial;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>().ToList<SpriteRenderer>();

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material = dissolveMaterial;
            sprite.material.SetTexture("_MainTex", sprite.sprite.texture);
            sprite.material.SetFloat("_dissolveAmount", -0.2f);
        }
    }
    void Start()
    {

    }

    public IEnumerator UniverseFadeOut()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material.SetFloat("_dissolveAmount", 1f);
            StartCoroutine(FadeTo(-0.2f, sprite.material));
        }

        yield return new WaitForSeconds(0.85f);
        gameObject.SetActive(false);
    }

    public IEnumerator UniverseFadeIn()
    {
        gameObject.SetActive(true);

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material.SetFloat("_dissolveAmount", -0.2f);
            StartCoroutine(FadeTo(1f, sprite.material));
        }

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator FadeTo(float targetValue, Material mat)
    {
        float currentValue = mat.GetFloat("_dissolveAmount");

        while (!Mathf.Approximately(currentValue, targetValue))
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * 1f);
            mat.SetFloat("_dissolveAmount", currentValue);
            yield return null;
        }
    }

    public void AddNewSprite(SpriteRenderer sprite)
    {
        sprites.Add(sprite);
        sprite.material = dissolveMaterial;
        sprite.material.SetTexture("_MainTex", sprite.sprite.texture);
        sprite.material.SetFloat("_dissolveAmount", 1f);
    }

}
