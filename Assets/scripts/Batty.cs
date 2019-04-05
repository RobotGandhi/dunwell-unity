using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batty : Enemy
{
    // They have different walking patterns
    public enum BattyType
    {
        ThreeBySeven = MapManager.TileValues.BATTY2,
        FiveByFive = MapManager.TileValues.BATTY1
    }

    int counter = 0;
    int screech_counter = 0;

    public GameObject CirclePrefab;
    private GameObject CurrentCircle;
    private SpriteRenderer CurrentCircleSpre;
    static Vector3 GoalScale = new Vector3(16.5f, 16.5f, 16.5f);

    [System.NonSerialized]
    public BattyType batType;

    // Directions list
    static List<Vector2> directionList1 = new List<Vector2>();
    static List<Vector2> directionList2 = new List<Vector2>();

    [System.NonSerialized]
    public Vector2 start_tile_pos;

    private Sprite regularSprite;
    public Sprite screechReadySprite;

    private void Start()
    {
        #region DIRECTION LIST CREATION
        directionList1.Add(new Vector2(0, 0));
        directionList1.Add(new Vector2(-2, 0));
        directionList1.Add(new Vector2(-2, -2));
        directionList1.Add(new Vector2(-2, -4));
        directionList1.Add(new Vector2(0, -4));
        directionList1.Add(new Vector2(2, -4));
        directionList1.Add(new Vector2(2, -2));
        directionList1.Add(new Vector2(2, 0));

        directionList2.Add(new Vector2(0, 0));
        directionList2.Add(new Vector2(-1, -1));
        directionList2.Add(new Vector2(-1, -3));
        directionList2.Add(new Vector2(-1, -5));
        directionList2.Add(new Vector2(0, -6));
        directionList2.Add(new Vector2(1, -5));
        directionList2.Add(new Vector2(1, -3));
        directionList2.Add(new Vector2(1, -1));
        #endregion

        CurrentCircle = null;


        // Base start to get components
        BaseStart();

        // Get the regular sprite
        regularSprite = spre.sprite;
    }

    private void Update()
    {
        if ((Vector2)transform.position != (tile_position * MapManager.GroundTileSize) + (Vector2)Offsets.BatEnemyOffset)
        {
            transform.position = Vector2.MoveTowards(transform.position, (new Vector3(tile_position.x, tile_position.y, 0) * MapManager.GroundTileSize) + Offsets.BatEnemyOffset, Constants.BattyMoveSpeed * Time.deltaTime);
        }

        // Circle logic
        if(CurrentCircle != null)
        {
            if(Mathf.Abs(CurrentCircle.transform.localScale.magnitude - GoalScale.magnitude) > 0.1f)
            {
                CurrentCircle.transform.localScale = Vector3.Slerp(CurrentCircle.transform.localScale, GoalScale, 10f * Time.deltaTime);
                CurrentCircleSpre.color = Color.Lerp(CurrentCircleSpre.color, Color.red, 1.2f * Time.deltaTime);
            }
            else
            {
                if (CurrentCircleSpre.color != Color.clear)
                {
                    CurrentCircleSpre.color = Vector4.MoveTowards(CurrentCircleSpre.color, Color.clear, 2f * Time.deltaTime);
                }
                else
                {
                    Destroy(CurrentCircle);
                    CurrentCircle = null;
                }
            }
        }
    }

    public override void TakeDamage()
    {
        // 1 hp
        HP = 0;
    }

    public override void PlayerEvent()
    {
        // Check if we screechs
        if (screech_counter == 3)
        {
            // Visual feedback thingy
            CurrentCircle = Instantiate(CirclePrefab, transform.position, Quaternion.identity);
            CurrentCircle.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.2f);
            CurrentCircle.transform.localScale = new Vector3(0, 0, 0);
            CurrentCircleSpre = CurrentCircle.GetComponent<SpriteRenderer>();

            // Stop the flashing!
            StopCoroutine("FlashingRed");
            spre.color = Color.white;

            // Check to see if the player is within the screech range
            for(int x = -2; x <= 2; x++)
            {
                for(int y = -2; y <= 2; y++)
                {
                    if((x == -2 && (y == -2 || y == 2)) || (x == 2 && (y == -2 || y == 2)))
                    {
                            
                    }
                    else 
                    {
                        Vector2 rel = tile_position + new Vector2(x, y);
                        if (rel == (Vector2)player.tile_position)
                        {
                            // Unaffected if the player is on the same tile!
                            if ((Vector2)player.tile_position != tile_position)
                            {
                                // Stun the player
                                screech_counter = 0;
                                player.player_combat.StunnedByBatty();
                            }
                        }
                    }
                }
            }
            screech_counter = 0;
        }
        else
        {
            // Increment screechers
            screech_counter++;
            if (screech_counter == 3)
                StartCoroutine("FlashingRed");
            else
            {
                StopCoroutine("FlashingRed");
                spre.color = Color.white;
            }

            // Step counter
            if (counter == 7)
            {
                counter = 0;
            }
            else
            {
                counter++;
            }

            if (batType == BattyType.FiveByFive)
            {
                // Move here using tile_position
                tile_position = start_tile_pos + directionList1[counter];
            }
            else // ThreeBySeven
            {
                tile_position = start_tile_pos + directionList2[counter];
            }

            spre.sortingOrder = Constants.MapHeight - (int)tile_position.y + 10;
        }
    }

    private IEnumerator FlashingRed()
    {
        while (true)
        {
            while(spre.color != Color.red)
            {
                spre.color = Vector4.MoveTowards(spre.color, Color.red, 2f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while(spre.color != Color.white)
            {
                spre.color = Vector4.MoveTowards(spre.color, Color.white, 2f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
