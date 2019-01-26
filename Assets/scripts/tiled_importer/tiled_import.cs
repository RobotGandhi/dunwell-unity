using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class tiled_import : MonoBehaviour
{
    
    public static void LoadTiledMap(string path)
    {
        string fileContent = "";

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

        int dataStartIndex = fileContent.IndexOf("data");

        String dataStringBad = fileContent.Substring(dataStartIndex, (fileContent.Length-dataStartIndex));
        int dataStringBracketIndex = dataStringBad.IndexOf("}");

        String dataStringAlmostGood = dataStringBad.Substring(0, dataStringBracketIndex);

        dataStringBracketIndex = dataStringAlmostGood.IndexOf("{");
        String dataStringGood = dataStringAlmostGood.Substring(dataStringBracketIndex+1, dataStringAlmostGood.Length - dataStringBracketIndex-1);


        // Get the integer list, 1D
        List<int> intList = new List<int>();

        foreach(var s in dataStringGood.Split(','))
        {
            int num;
            if(int.TryParse(s, out num))
            {
                intList.Add(num);
            }
        }

        foreach(int x in intList)
        {
            print(x);
        }
        
    }

}
