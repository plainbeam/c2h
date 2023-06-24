using C2H.Data.Model;
using C2H.Data.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C2H.Data.Services
{
    public partial interface IProductService
    {
        List<Product> getProducts();
        bool populateRandomProducts();
    }

    public class ProductService : IProductService
    {
        private static Random random = new Random();
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Symbols = "123456789";
        private String sCmd = "";

        public List<Product> getProducts()
        {
            Database db = new Database(Constants.sqlConfig);
            return db.m_conn.Query<Product>(@"SELECT * FROM products").AsList();
        }

        public bool populateRandomProducts()
        {
            Database db = new Database(Constants.sqlConfig);
            try
            {
                string combination = "";
                for(int i=0; i<= 1000000;i++)
                {
                    string word1 = GenerateRandomWord();
                    string word2 = GenerateRandomWord();
                    combination = $"{word1} {word2}";

                    db.m_conn.Query<bool>(@"INSERT INTO products (`Name`) VALUES (@Name);", new { Name = combination });
                }
                
            }
            catch
            {
                return false;
            }
            return true;

        }

        public static string GenerateRandomWord()
        {
            StringBuilder word = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    // Add a random letter
                    char letter = Letters[random.Next(Letters.Length)];
                    word.Append(letter);
                }
                else
                {
                    // Add a random symbol
                    char symbol = Symbols[random.Next(Symbols.Length)];
                    word.Append(symbol);
                }
            }

            return word.ToString();
        }
    }
}
