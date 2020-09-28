using System.IO;
using UnityEngine;

namespace UI.Data
{
    //-------------------------------------------------------------------------
    public class JsonConverter
    {
        #region Private Fields

        private static readonly string _myFileName = "saveData.sav";

        #endregion


        #region Private Methods

        //-------------------------------------------------
        private static string GetFileName() => $"{Application.persistentDataPath}/{_myFileName}";

        #endregion


        #region Public Methods

        //-------------------------------------------------
        public void Save(SaveData someData)
        {
            string json = JsonUtility.ToJson(someData);
            string fileName = GetFileName();

            FileStream fileStream = new FileStream(path: fileName, mode: FileMode.Create);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        //-------------------------------------------------
        public bool Load(SaveData someData)
        {
            string fileName = GetFileName();

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                JsonUtility.FromJsonOverwrite(json, someData);

                return true;
            }

            return false;
        }

        //-------------------------------------------------
        public void Delete() => File.Delete(GetFileName());

        #endregion
    }
}