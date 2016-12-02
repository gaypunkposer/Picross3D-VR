using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProperties {

    public Vector3 Position;
    public bool Keep;
    public Material Material;

    public BlockProperties(Vector3 position)
    {
        Position = position;
        Keep = false;
        //If material is null, default to main material.
        Material = null;
    }

    public BlockProperties(Vector3 position, bool keep, Material material)
    {
        Position = position;
        Keep = keep;
        Material = material;
    }

}
