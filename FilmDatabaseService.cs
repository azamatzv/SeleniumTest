using Dapper;
using Npgsql;

namespace SeleniumTest;

public class FilmDatabaseService
{
    private readonly string _connectionString;

    public FilmDatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void AddFilm(Film film)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var query = @"INSERT INTO films 
                          (title, image_url, description, genres, year, country, imdb_rating, kinopoisk_rating, director, actors, download_link)
                          VALUES 
                          (@Title, @ImageUrl, @Description, @Genres, @Year, @Country, @ImdbRating, @KinopoiskRating, @Director, @Actors, @DownloadLink)";
            connection.Execute(query, film);
        }
    }
}