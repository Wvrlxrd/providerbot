using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using TelegramBot.Command.Commands;
using TelegramBot.Database.Models;

namespace TelegramBot.Database
{
    public class Database
    {
        private const string DataSource = "Data Source=provider.db";
        
        public static List<Category> GetCategory()
        {
            var list = new List<Category>();
            using var connection = new SqliteConnection(DataSource);
            connection.Open();
            var sql = "SELECT * FROM Category";
            var command = new SqliteCommand(sql, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Int64 id = (Int64)reader.GetValue(0);
                    string title = (string) reader.GetValue(1);
                    
                    list.Add(new Category(id, title));
                }
            }

            return list;
        }

        public static List<Service> GetService()
        {
            var list = new List<Service>();
            using var connection = new SqliteConnection(DataSource);
            connection.Open();
            var sql = "SELECT Service.*, Category.Title FROM Service INNER JOIN Category ON Category.id=Service.id;";
            var command = new SqliteCommand(sql, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    long id = (long)reader.GetValue(0);
                    string title = (string) reader.GetValue(1);
                    string categoryTitle = (string)reader.GetValue(3);
                    list.Add(new Service(id, title, categoryTitle));
                }
            }

            return list;
        }
        
        public static List<Product> GetProduct()
        {
            var list = new List<Product>();
            using var connection = new SqliteConnection(DataSource);
            connection.Open();
            var sql = "SELECT Product.*, Service.title FROM Product INNER JOIN Service ON Service.id=Product.service;";
            var command = new SqliteCommand(sql, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    long id = (long)reader.GetValue(0);
                    string title = (string) reader.GetValue(1);
                    long price = (long)reader.GetValue(2);
                    string description = (string)reader.GetValue(3);
                    string serviceTitle = (string)reader.GetValue(5);
                    list.Add(new Product(id, title, price, description, serviceTitle));
                }
            }
            return list;
        }

        public static bool AddProduct(string title, long price, string description, long serviceId)
        {
            using var connection = new SqliteConnection(DataSource);
            connection.Open();
            SqliteCommand insertSql = new SqliteCommand($"INSERT INTO Product(Title, Price, Description, Service) VALUES(\"{title}\", {price}, \"{description}\", {serviceId})", connection);
            try
            {
                insertSql.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
                return false;
            }

            return true;
        }

        public static void RemoveProduct(long index)
        {
            using var connection = new SqliteConnection(DataSource);
            connection.Open();
            SqliteCommand insertSql = new SqliteCommand($"DELETE FROM Product WHERE id = \"{index}\"", connection);
            try
            {
                insertSql.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateProduct(long id, string title, long price, string description)
        {
            using var connection = new SqliteConnection(DataSource);
            connection.Open();
            SqliteCommand insertSql = new SqliteCommand($"UPDATE Product SET title=\"{title}\", price=\"{price}\", description=\"{description}\" WHERE id={id}", connection);
            try
            {
                insertSql.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}