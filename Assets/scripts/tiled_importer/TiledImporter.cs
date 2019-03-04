using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class TiledImporter : MonoBehaviour
{
    
    public static int[,] LoadTiledMap(string level_name)
    {
        string fileContent = ResourceLoader.GetLevelTextFile(level_name).text;

        // Trim down to only the neccesary parts of the string 
        int temp = fileContent.IndexOf("layers");
        fileContent = fileContent.Substring(temp, fileContent.Length-temp);

        // Tile Data List
        List<int[,]> tileDataList = new List<int[,]>();

        // Constants
        uint width = Constants.MapWidth;
        uint height = Constants.MapHeight;

        // Go layer by layer and extract data
        int i = 0;
        foreach(char c in fileContent)
        {
            // Found tile data
            if(fileContent[i] == 'd' && fileContent[i+1] == 'a' && fileContent[i+2] == 't')
            {
                int endIndex = fileContent.IndexOf("}", i)+1;
                string dataString = fileContent.Substring(i, endIndex - i);

                List<int> intList = new List<int>();
                
                // Get the integer list, 1D
                foreach (var s in dataString.Split(','))
                {
                    int num;
                    if (int.TryParse(s, out num))
                    {
                        intList.Add(num);
                    }
                }

                int[,] data = new int[Constants.MapHeight, Constants.MapWidth];

                // Convert to a 2d tiled map representable in the game
                int x = 0;
                int y = 0;
                                
                foreach(int num in intList)
                {
                    print(num);
                }

                for (int j = 0; j < intList.Count; j++)
                {
                    data[height-1-y, x] = intList[j];
                    x++;
                    if (x > width-1)
                    {
                        x = 0;
                        y++;
                    }
                }  
                // Insert into our list
                tileDataList.Add(data);
            }

            i++;
        }

        // Layers 
        int[,] final_map = new int[height, width];

        // Grab everything from the first layer
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                final_map[height-1-y, x] = tileDataList[0][height-1-y, x];
            }
        }

        if (tileDataList.Count > 1)
        {
            // Go through and make sure we only take the important parts of each layer
            for (int z = 1; z < tileDataList.Count; z++)
            {
                // 0 - ground 
                // 1 - items etc
                int[,] map = tileDataList[z];
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        if (MapManager.ShouldReplace(final_map[height-1-y,x], map[height-1-y,x]))
                        {
                            final_map[height-1-y, x] = map[height-1-y, x];
                        }
                    }
                }
            }
        }


        // Done
        return final_map;
    }

}
