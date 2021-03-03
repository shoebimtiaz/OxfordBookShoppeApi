using Microsoft.Extensions.Configuration;
using OxfordBookShoppe.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OxfordBookShoppe.Data.Services
{
    public class SqlBookData : IBookData
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; set; }
        public SqlBookData(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Add(Book book)
        {
            try
            {
                var connection = new SqlConnection(_configuration.GetConnectionString("BookDb"));
                using (var command = new SqlCommand("AddBook", connection))
                {
                    command.Parameters.Add(new SqlParameter("@BookName", book.Name));
                    command.Parameters.Add(new SqlParameter("@Author", book.Author));
                    command.Parameters.Add(new SqlParameter("@Genre", book.Genre));
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@BookId",
                        Value = book.Id,
                        Direction = ParameterDirection.Output,
                        IsNullable = false,
                        DbType = DbType.Int32
                    });
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();

                    book.Id = Convert.ToInt32(command.Parameters["BookId"].Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete(int id)
        {
           try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("BookDb")))
                {
                    using (var command = new SqlCommand("DeleteBook", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@BookId", id));
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<Book> GetAll()
        {
            var bookList = new List<Book>();
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("BookDb")))
                {
                    using (var command = new SqlCommand("GetBooks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                var book = new Book
                                {
                                    Id = Convert.ToInt32(dr["BookId"]),
                                    Name = dr["BookName"].ToString(),
                                    Author = dr["Author"].ToString(),
                                    Genre = dr["Genre"].ToString()

                                };
                                bookList.Add(book);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return from b in bookList
                   orderby b.Author
                   select b;
        }

        public Book GetById(int id)
        {
            var book = new Book();
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("BookDb")))
                {
                    using (var command = new SqlCommand("GetBookById", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@BookId", id));
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                book.Id = Convert.ToInt32(dr["BookId"]);
                                book.Name = dr["BookName"].ToString();
                                book.Author = dr["Author"].ToString();
                                book.Genre = dr["Genre"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return book;
        }

        public void Update(Book book)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("BookDb")))
                {
                    using (var command = new SqlCommand("UpdateBook", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@BookId", book.Id));
                        command.Parameters.Add(new SqlParameter("@BookName", book.Name));
                        command.Parameters.Add(new SqlParameter("@Author", book.Author));
                        command.Parameters.Add(new SqlParameter("@Genre", book.Genre));

                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
