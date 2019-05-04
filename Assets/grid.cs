using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    //the grid itself
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    //rounds a vector down
    public static Vector2 roundVec2(Vector2 v){
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }  

    //helps find out if a coordinate is in between the borders or out of the borders
    public static bool insideBorder(Vector2 pos) {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    //function: delets all blocks in a certain row
    //takes the parameter y which is the row thats supposed to be deleted, then 
    //loops through every block in that row, destroys it from the game 
    //and clears the refrence to it by setting the grid entry to null
    public static void deleteRow(int y) {
        for(int x = 0; x < width; ++x) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //function: when a row is deleted, the above rows should fall towards the bottom
    //one by one, this function does that
    public static void decreaseRow(int y) {
        //goes through every block in that row
        for (int x = 0; x < width; ++x) {
            if (grid[x,y] != null) {
                //move one towards bottom
                grid[x, y-1] = grid[x, y];
                grid[x, y] = null;

                //update block position
                grid[x, y-1].position += new Vector3(0, -1, 0);
            }
        }
    }

    //function: use decreaseRow function on every row above a certain index because
    //when a row was deleted, we want to decrease all rows above it too
    //accepts a parameter y, which is the row, then loops through all above rows 
    //by using i, starting at y and looping while i<height
    public static void decreaseRowsAbove(int y) {
        for (int i = y; i < height; ++i)
        decreaseRow(i);
    }

    //function: finds out if a row is full of blocks
    //it takes a row parameter y, loops through every grid entry and returns false as
    //soon as there is no block in a grid entry
    //if the foor loop was finished and we still didnt retrun false then the row
    //must be full of blocks, then return true
    public static bool isRowFull(int y) {
        for (int x = 0; x < width; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    //function: deletes all full rows then always decreases the above row's 
    //y coordinate by one
    public static void deleteFullRows() {
        for (int y = 0; y < height; ++y) {
            if (isRowFull(y)) {
                decreaseRow(y);
                decreaseRowsAbove(y+1);
                --y;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
