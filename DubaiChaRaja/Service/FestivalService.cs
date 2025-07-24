using DubaiChaRaja.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace DubaiChaRaja.Service
{
    public class FestivalService : IFestivalService
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public FestivalService(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public void InsertFestivalRecords(string description, decimal amount, string type, int year, int userId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT public.insertfestivalrecord(@input_description, @input_amount, @input_type, @input_year,@input_userid)", conn))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("input_description", NpgsqlDbType.Text, description);
                    command.Parameters.AddWithValue("input_amount", NpgsqlDbType.Numeric, amount);
                    command.Parameters.AddWithValue("input_type", NpgsqlDbType.Text, type);
                    command.Parameters.AddWithValue("input_year", NpgsqlDbType.Integer, year);
                    command.Parameters.AddWithValue("input_userid", NpgsqlDbType.Integer, userId);


                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<IEnumerable<FestivalRecord>> GetByYearAsync(int year)
        {
            var records = new List<FestivalRecord>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM getallfestivalrecords()", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var y = Convert.ToInt32(reader["Year"]);
                            if (y == year)
                            {
                                records.Add(new FestivalRecord
                                {
                                    Id = Convert.ToInt32(reader["Id"]), 
                                    Description = reader["Description"].ToString(),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    Type = reader["Type"].ToString(),
                                    Year = y
                                });
                            }
                        }
                    }
                }
            }

            return records;
        }




        public async Task<bool> DeleteFestivalRecordAsync(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var cmd = new NpgsqlCommand("DELETE FROM festivalrecords WHERE id = @id", conn))
                {
                    Console.WriteLine("Attempting to delete record with ID = " + id);

                    cmd.Parameters.AddWithValue("@id", id);

                    int affectedRows = await cmd.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }




    }
}