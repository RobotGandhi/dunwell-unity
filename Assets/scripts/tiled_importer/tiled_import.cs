using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;


class Pee
{

}

class Balls
{
    Pee pee; // We store the pee in the balls
}




public class tiled_import : MonoBehaviour
{
    
    public static int[,] LoadTiledMap(string path)
    {
        string fileContent = "";

        // Read the file content
        try {
            using (StreamReader sr = new StreamReader(path)) {

                string line = sr.ReadLine();
                while(line != null)
                {
                    fileContent += line + "\n";
                    line = sr.ReadLine();
                }

            }

        } catch (Exception e) {
            print("The file could not be read:");
            print(e.Message);
        }

        // Find and isolate data etc
        int dataStartIndex = fileContent.IndexOf("data");

        String dataStringBad = fileContent.Substring(dataStartIndex, (fileContent.Length-dataStartIndex));
        int dataStringBracketIndex = dataStringBad.IndexOf("}");

        String dataStringAlmostGood = dataStringBad.Substring(0, dataStringBracketIndex);

        dataStringBracketIndex = dataStringAlmostGood.IndexOf("{");
        String dataStringGood = dataStringAlmostGood.Substring(dataStringBracketIndex+1, dataStringAlmostGood.Length - dataStringBracketIndex-1);


        // Get the integer list, 1D
        List<int> intList = new List<int>();

        foreach (var s in dataStringGood.Split(','))   
        {
            int num;
            if (int.TryParse(s, out num))
            {
                intList.Add(num);
            }
        }

        // Now we have a 1d list of the map tile values
        // All we need to do now is convert it to a 2d list used to render in the game!
        // We will use the defined map values from the constants.cs
        uint width = Constants.MapWidth;
        uint height = Constants.MapHeight;
        int[,] map =     
        {
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3, 3 }
        };

        int temp_counter = 0;
        int counter = 0;
        
        for(int i = 0; i < intList.Count; i++)
        {
            if(temp_counter > width-1)
            {
                temp_counter = 0;
                counter++;
            }

            map[counter, temp_counter] = intList[i];

            temp_counter++;
        }

        return map;
    }

}
