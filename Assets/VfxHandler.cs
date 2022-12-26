using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VfxHandler : MonoBehaviour
{
    #region Singleton
    public static VfxHandler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

    }

    #endregion
    public VFX[] vfx;

    public void PlayParticle(string name,Vector3 position)
    {
        GameObject _vfx = Array.Find(vfx, vfx => vfx.name == name).particle;
        _vfx.SetActive(true);
        StartCoroutine (DeActive(_vfx));
    }
    IEnumerator DeActive(GameObject obj)
    {
        yield return new WaitForSeconds(3);
        obj.SetActive(false);
    }
}

[System.Serializable] 
public class VFX
{
    public string name;
    public GameObject particle;
}
