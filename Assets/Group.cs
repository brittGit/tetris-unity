using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {
     //Time since last gravity tick
    float lastFall = 0;

    //function: helps verify each child blocks position
    //at first it loops through every child by using foreach, then it stores
    //the child's rounded position in a variable
    //afterwards it finds out if that position is inside the border and then it finds
    //out if there already is a block in the same grid entry or not
    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVec2(child.position);

            //not inside border?
            if (!Grid.insideBorder(v))
                return false;

            //Block in grid cell and not part of same group?
            if (Grid.grid[(int)v.x, (int)v.y] != null && 
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    //function: is a group changed its position then it has to remove all the old
    //block positions from the grid and add all the new block positions to the grid

    //looped through the grid then chicked if the block (if it exists) is part of the
    //group by using the parent property
    //if the blocks parent equals the current groups transform then its a child of
    //that group
    //after we loop through all children again to add them to the grid
    void updateGrid() {
        //remove old children from grid
        for (int y = 0; y < Grid.height; ++y)
            for (int x = 0; x < Grid.width; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        //add new children to the grid
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Start is called before the first frame update
    void Start() {
        //function: if a new group was spawned and it immediately collides with
        //something else, then the game is over
        if (!isValidGridPos()) {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }  
    }

    // Update is called once per frame
    void Update() {
        //Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            //Modify position
            transform.position += new Vector3(-1, 0, 0);

            //See if valid
            if (isValidGridPos())
                //Its valid. Update grid.
                updateGrid();
            else
                //Its not valiud. revert.
                transform.position += new Vector3(1, 0, 0);
        }

        //Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            //Modify position 
            transform.position += new Vector3(1, 0, 0);

            //See if valid
            if(isValidGridPos())
                //Its valid. Update grid.
                updateGrid();
            else
                //Its not valid. Revert.
                transform.position += new Vector3(-1, 0, 0);
        }

        //Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.Rotate(0, 0, -90);

            //See if valid
            if (isValidGridPos())
                //Its valid. Update grid.
                updateGrid();
            else    
                //Its not valid. revert.
                transform.Rotate(0, 0, 90);
        }

        //Fall
        else if (Input.GetKeyDown(KeyCode.DownArrow) ||
                Time.time - lastFall >= 1) {
            //Modify position
            transform.position += new Vector3(0, -1, 0);

            //See if valid
            if (!isValidGridPos()) {
                //Its valid. Update grid.
                updateGrid();
            } else {
                //Its not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                //Clear filled horizontal lines
                Grid.deleteFullRows();

                //Spawn next group
                FindObjectOfType<Spawner>().spawnNext();

                //disable script
                enabled = false;
            }   
            lastFall = Time.time;
        }
    }
}
