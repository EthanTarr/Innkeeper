using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static void Save()
    {
        Save save = new Save();
        save.Player = GameObject.Find("Player");
        save.Tables.Add(GameObject.Find("Table"));
        save.Tables.Add(GameObject.Find("Table (2)"));
        save.Tables.Add(GameObject.Find("Stool (2)"));
        save.Tables.Add(GameObject.Find("Stool (3)"));
        save.Tables.Add(GameObject.Find("Stool (4)"));

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.ent");
        bf.Serialize(file, save);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveGame.ent"))
        {
            GameObject oldPlayer = GameObject.Find("Player");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.ent", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            
            save.Player.transform.parent = oldPlayer.transform.parent;
            Object.Destroy(oldPlayer);

            /*foreach(GameObject table in save.Tables)
            {
                table.transform.parent = 
            }*/
        }
    }
}
