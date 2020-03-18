using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingBallsManager : MonoBehaviour
{

    [SerializeField]
    Material onMat, offMat;
    [SerializeField]
    List<MeshRenderer> balls;
    [SerializeField]
    List<Paintable> paintables;

    // Start is called before the first frame update
    void Start()
    {
        foreach(MeshRenderer ball in balls)
        {
            if(ball != null) ball.material = offMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < balls.Count && i < paintables.Count; i++)
        {
            if (paintables[i] != null && paintables[i].IsPainted() && balls[i].material != onMat) balls[i].material = onMat;
        }
    }
}
