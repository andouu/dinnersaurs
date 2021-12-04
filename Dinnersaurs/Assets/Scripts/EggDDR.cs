using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDDR : MonoBehaviour
{
    private string state;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        state = "idle";
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "hovering")
        {
            meshRenderer.material.SetFloat("_OutlineWidth", 4f);
        }
        else
        {
            meshRenderer.material.SetFloat("_OutlineWidth", 0f);
        }
    }

    public void setState(string state)
    {
        this.state = state;
    }

    public void ping()
    {
        print("ping");
    }

    public void pong()
    {
        print("pong");
    }
}
