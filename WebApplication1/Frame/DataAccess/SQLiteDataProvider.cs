using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Frame.DataAccess
{
    public class SQLiteDataProvider : IDisposable
    {
        private SQLiteConnection dbConnection;

        public SQLiteDataProvider()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "MyDatabase.sqlite");
            dbConnection = new SQLiteConnection(@"Data Source="+ path + ";Version=3;");
            dbConnection.Open();
        }

        public SQLiteCommand GetCommand(string sql)
        {
            return new SQLiteCommand(sql, dbConnection);
        }
        public List<HighScore> SelectAll()
        {
            var results = new List<HighScore>();
            var command = GetCommand("select * from highscores");
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new HighScore
                {
                    Name = reader["name"].ToString(),
                    Score = int.Parse(reader["score"].ToString())
                });
            }
            return results;
        }
        public void Delete(HighScore highScore)
        {
            string sql = string.Format("delete from highscores where name='{0}' and score ={1}", highScore.Name, highScore.Score);
            GetCommand(sql).ExecuteNonQuery();
        }
        public void Insert(HighScore highScore)
        {
            string sql = string.Format("insert into highscores (name, score) values ('{0}',{1})",highScore.Name,highScore.Score);
            GetCommand(sql).ExecuteNonQuery();
        }
        public void ExecuteNonQuery(string sql)
        {
            GetCommand(sql).ExecuteNonQuery();
        }

        public void Dispose()
        {
            dbConnection.Close();
        }
    }
}