using System;
using UnityEngine;

public class DeadState : BaseState
{
    private Rigidbody2D rb;

    [SerializeField] private int MAX_CRYSTALS = 6;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView){ }

    public override void EnterState(StateManager npc, GameObject player){
        npc.gameObject.SetActive(false);

        rb = npc.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero; 

        System.Random rnd = new System.Random();

        int numCrystals = rnd.Next(MAX_CRYSTALS - 1);

        GameObject crystal = npc.Crystal;
        Quaternion rot = npc.transform.rotation;
        Vector3 scale = new Vector3(0.25f,0.25f,0.25f);

        for(int i = 0;i < numCrystals + 1;i++){
            int despX = rnd.NextDouble() >= 0.5 ? 1 : -1;
            int despY = rnd.NextDouble() >= 0.5 ? 1 : -1;
            float x = (float) rnd.NextDouble() * 2;
            float y = (float) rnd.NextDouble() * 0.5f;
            Vector3 pos = npc.transform.position + new Vector3(x * despX,y * despY);
            var crys = GameObject.Instantiate(crystal,pos,rot);
            crys.transform.localScale = scale;
        }
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}
