using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapGenerator : MonoBehaviour {

    public int mWidth;
    public int mHeight;

    public string mSeed;
    public bool mUseRandomSeed;

    [Range(0,100)]
    public int mRandomFillPercent;
    int[,] mMap;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        mMap = new int[mWidth, mHeight];
        RandomFillMap();

        for(int ti = 0; ti<5; ti++)
        {
            SmoothMap();
        }

        CMeshGenerator tMeshGen = this.gameObject.GetComponent<CMeshGenerator>();
        tMeshGen.GenerateMesh(mMap, 1);
    }

    void RandomFillMap()
    {
        if(true == mUseRandomSeed)
        {
            mSeed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(mSeed.GetHashCode());

        

        for (int tx = 0; tx<mWidth; tx++)
        {
            for(int ty = 0; ty<mHeight; ty++)
            {
                if(0 == tx || mWidth - 1 == tx || 0 == ty || mHeight-1 == ty)
                {
                    mMap[tx, ty] = 1;
                }
                else
                {
                    mMap[tx, ty] = (pseudoRandom.Next(0, 100) < mRandomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int tx = 0; tx < mWidth; tx++)
        {
            for (int ty = 0; ty < mHeight; ty++)
            {
                int tNeighbourWalltiles = GetSurroundingWallCount(tx, ty);

                if(tNeighbourWalltiles>4)
                {
                    mMap[tx, ty] = 1;
                }
                else if(tNeighbourWalltiles < 4)
                {
                    mMap[tx, ty] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int tGridX, int tGridY)
    {
        int tWallCount = 0;
        for(int tNeighbourX = tGridX-1; tNeighbourX<=tGridX + 1; tNeighbourX++)
        {
            for (int tNeighbourY = tGridY - 1; tNeighbourY <= tGridY + 1; tNeighbourY++)
            {
                if(tNeighbourX>=0 && tNeighbourX<mWidth && tNeighbourY >=0 && tNeighbourY<mHeight)
                {
                    if (tNeighbourX != tGridX || tNeighbourY != tGridY)
                    {
                        tWallCount += mMap[tNeighbourX, tNeighbourY];
                    }
                }
                else
                {
                    ++tWallCount;
                }
            }
        }
        return tWallCount;
    }

    private void OnDrawGizmos()
    {
        /*
        if(null != mMap)
        {
            for (int tx = 0; tx < mWidth; tx++)
            {
                for (int ty = 0; ty < mHeight; ty++)
                {
                    Gizmos.color = (mMap[tx, ty] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-mWidth / 2 + tx + 0.5f, 0, -mHeight / 2 + ty + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
        */
    }
}
