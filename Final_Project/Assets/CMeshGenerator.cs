using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMeshGenerator : MonoBehaviour {


    public CSquareGrid mSquareGrid;

    public void GenerateMesh(int[,] tMap, float tSquareSize)
    {
        mSquareGrid = new CSquareGrid(tMap, tSquareSize);
    }

    public void OnDrawGizmos()
    {
        if(null != mSquareGrid)
        {
            for(int tx = 0; tx < mSquareGrid.mSquares.GetLength(0); ++tx)
            {
                for(int ty = 0; ty<mSquareGrid.mSquares.GetLength(1); ++ty)
                {
                    Gizmos.color = (mSquareGrid.mSquares[tx, ty].mTopLeft.mIsActive) ? Color.black : Color.white;
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mTopLeft.mPosition, Vector3.one * 0.4f);

                    Gizmos.color = (mSquareGrid.mSquares[tx, ty].mTopRight.mIsActive) ? Color.black : Color.white;
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mTopRight.mPosition, Vector3.one * 0.4f);

                    Gizmos.color = (mSquareGrid.mSquares[tx, ty].mBottomLeft.mIsActive) ? Color.black : Color.white;
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mBottomLeft.mPosition, Vector3.one * 0.4f);

                    Gizmos.color = (mSquareGrid.mSquares[tx, ty].mBottomRight.mIsActive) ? Color.black : Color.white;
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mBottomRight.mPosition, Vector3.one * 0.4f);


                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mCenterTop.mPosition, Vector3.one * 0.15f);
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mCenterRight.mPosition, Vector3.one * 0.15f);
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mCenterBottom.mPosition, Vector3.one * 0.15f);
                    Gizmos.DrawCube(mSquareGrid.mSquares[tx, ty].mCenterLeft.mPosition, Vector3.one * 0.15f);
                }
            }
        }
    }



    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////
    /// </summary>



    public class CSquareGrid
    {
        public CSquare[,] mSquares;
        public CSquareGrid(int[,] tMap, float tSquareSize)
        {
            int tNodeCountX = tMap.GetLength(0);
            int tNodeCountY = tMap.GetLength(1);

            float tMapWidth = tNodeCountX * tSquareSize;
            float tMapHeight = tNodeCountY * tSquareSize;

            CControlNode[,] tControlNodes = new CControlNode[tNodeCountX, tNodeCountY];

            Vector3 tPos;
            for(int tx = 0; tx<tNodeCountX; tx++)
            {
                for(int ty = 0; ty<tNodeCountY; ty++)
                {
                    tPos = new Vector3(-tMapWidth / 2 + tx * tSquareSize + tSquareSize / 2, 0, -tMapHeight / 2 + ty * tSquareSize + tSquareSize / 2);
                    tControlNodes[tx, ty] = new CControlNode(tPos, tMap[tx, ty] == 1, tSquareSize);
                }
            }

            mSquares = new CSquare[tNodeCountX - 1, tNodeCountY - 1];
            for (int tx = 0; tx < tNodeCountX-1; tx++)
            {
                for (int ty = 0; ty < tNodeCountY-1; ty++)
                {
                    mSquares[tx, ty] = new CSquare(tControlNodes[tx, ty + 1], tControlNodes[tx + 1, ty + 1], tControlNodes[tx + 1, ty], tControlNodes[tx, ty]);
                }
            }
        }
    }

    public class CSquare
    {
        public CControlNode mTopLeft, mTopRight, mBottomRight, mBottomLeft;
        public CNode mCenterTop, mCenterRight, mCenterBottom, mCenterLeft;

        public CSquare(CControlNode tTopLeft, CControlNode tTopRight, CControlNode tBottomRight, CControlNode tBottomLeft)
        {
            mTopLeft = tTopLeft;
            mTopRight = tTopRight;
            mBottomRight = tBottomRight;
            mBottomLeft = tBottomLeft;

            mCenterTop = mTopLeft.mRight;
            mCenterRight = mBottomRight.mAbove;
            mCenterBottom = mBottomLeft.mRight;
            mCenterLeft = mBottomLeft.mAbove;
        }
    }
    
	public class CNode
    {
        public Vector3 mPosition;
        public int mVertexIndex = -1;

        public CNode(Vector3 tPos)
        {
            mPosition = tPos;
        }
    }

    public class CControlNode : CNode
    {
        public bool mIsActive;
        public CNode mAbove, mRight;

        public CControlNode(Vector3 tPos, bool tActive, float tSquareSize) : base(tPos)
        {
            mIsActive = tActive;
            mAbove = new CNode(mPosition + Vector3.forward * tSquareSize / 2.0f);
            mRight = new CNode(mPosition + Vector3.right * tSquareSize / 2.0f);
        }

    }
}
