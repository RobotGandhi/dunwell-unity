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
                                
                for (int j = 0; j < intList.Count; j++)
                {
                    data[y, x] = intList[j];
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

        // Go through and make sure we only take the important parts of each layer

        // Done
        return tileDataList[0];
    }

}
