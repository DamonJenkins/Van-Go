using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Paintable : MonoBehaviour
{

    MeshRenderer renderer;
    [SerializeField]
    Texture tex_unpainted, tex_painted;

    bool painted = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material.SetTexture("_MainTex", tex_unpainted);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "paintGlob")
        {
            painted = true;
            renderer.material.SetTexture("_MainTex", tex_painted);
            collision.transform.gameObject.SetActive(false);
            //TODO: add particle splat
        }
    }

    public bool IsPainted(){
        return painted;
    }

}
