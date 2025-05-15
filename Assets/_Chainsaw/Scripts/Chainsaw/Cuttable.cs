using System;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    private MeshFilter mainMeshFilter;

    public Mesh mainObjMesh_damaged;

    public Transform cutPoint;
    public GameObject[] cutIndicators;
    public GameObject[] sectionSplits;
    public ParticleSystem damageFX;

    public float maxHP;

    private float m_currentHP;
    private bool m_damaged = false;

    public bool IsDead => m_currentHP <= 0;
    
    private void Start()
    {
        mainMeshFilter = GetComponent<MeshFilter>();

        m_currentHP = maxHP;
    }

    public void ReceiveDamage(float _dmg)
    {
        if (m_currentHP > 0)
        {
            damageFX.Play();

            m_currentHP -= _dmg;

            if (m_currentHP < maxHP)
            {
                // Asign damaged mesh to cuttable object if below max HP and if not yet assigned
                if (!m_damaged)
                    mainMeshFilter.sharedMesh = mainObjMesh_damaged;

                // Handle cuttable object HP reduced to 0
                if (m_currentHP <= 0)
                {
                    gameObject.SetActive(false);
                    foreach (GameObject indicator in cutIndicators)
                    {
                        indicator.SetActive(false);
                    }
                    foreach (GameObject section in sectionSplits)
                    {
                        section.SetActive(true);
                    }
                }
            }
        }
    }
}
