using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System;
using blazorWords.Models;
using blazorWords.Data;

namespace blazorWords.Data
{
    public class WordService : IWordService
    {
        private List<Words> wordsList;
        IConfiguration _config;
        public WordService(IConfiguration config)
        {
            _config = config;
        }
  
        public List<Words> GetWords()
        {
            List<Words> rtnList = new List<Words>();

            using (var connection = new SqliteConnection(_config.GetConnectionString("WordsDb")))
            {
                connection.Open();

                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT id, word FROM wordLibrary";

                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string newID = reader.GetString(0);
                        string newWord = reader.GetString(1);


                        rtnList.Add(new Words { id = newID, word = newWord });
                    }
                }
                connection.Close();
            }
            wordsList = rtnList;

            return rtnList;
        }
    }
}