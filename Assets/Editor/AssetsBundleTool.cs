using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetsBundleTool 
{
    [MenuItem("AssetsBundle/打包AB")]
    static void BuildABs()
    {
        // Put the bundles in a folder called "ABs" within the Assets folder.
        BuildPipeline.BuildAssetBundles("Assets/ABs", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Example/Build Asset Bundles Using BuildMap")]
    static void BuildMapABs()
    {
        // Create the array of bundle build details.
        AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

        buildMap[0].assetBundleName = "enemybundle";

        string[] enemyAssets = new string[2];
        enemyAssets[0] = "Assets/Textures/akm_diff.tga";
        enemyAssets[1] = "Assets/Textures/m870.png";

        buildMap[0].assetNames = enemyAssets;


        buildMap[1].assetBundleName = "herobundle.ab";

        string[] heroAssets = new string[1];
        heroAssets[0] = "Assets/Resources/Prefab/Player.prefab";
        buildMap[1].assetNames = heroAssets;

        BuildPipeline.BuildAssetBundles("Assets/ABs", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
