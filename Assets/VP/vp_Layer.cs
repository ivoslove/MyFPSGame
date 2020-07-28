using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class defines the layers used on objects,you may want to modify the integers assigned here to suit your needs.
/// for example, you may want to keep 'LocalPlayer' in another layer or you may want to address rendering or physics issues
/// related to incompatibility with other systems.
/// </summary>
public sealed class vp_Layer {
    public static readonly vp_Layer Instance = new vp_Layer();
    //built-in Unity layers
    public const int Default = 0;
    public const int TransparentFX = 1;
    public const int IgnoreRaycast = 2;
    public const int Water = 4;
    public const int UI = 5;

    //standard layers
    public const int LocalPlayer = 27;
    public const int Border= 28;
    public const int Monster = 29;
    public const int SceneTerrian = 30;
    public const int Any= 31;

    public static class Mask
    {
        public static int BaseTerria = 1 << SceneTerrian;
        public static int BowTarget = 1 << Monster|1<< Border;
        public static int PlayerLayer = 1 << LocalPlayer;
        public static int BulletBlockers = 1 << Monster & 1 << Border & 1 << LocalPlayer & 1 << SceneTerrian;
    }
    static vp_Layer() { }
    private vp_Layer() { }
    /// <summary>
	/// sets the layer of a gameobject and optionally its descendants
	/// </summary>
	public static void Set(GameObject obj, int layer, bool recursive = false)
    {

        if (layer < 0 || layer > 31)
        {
            Debug.LogError("vp_Layer: Attempted to set layer id out of range [0-31].");
            return;
        }

        obj.layer = layer;

        if (recursive)
        {
            foreach (Transform t in obj.transform)
            {
                Set(t.gameObject, layer, true);
            }
        }

    }
    public static bool IsInMask(int layer,int layerMask)
    {
        return (layerMask & 1 << layer) == 0;
    }
}
