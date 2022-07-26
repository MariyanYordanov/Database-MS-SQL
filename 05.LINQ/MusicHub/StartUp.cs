namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context,4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId.Value == producerId)
                .Include(a => a.Producer)
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .ToList()
                .Select(a => new
                {
                    TotalAlbumPrice = a.Price,
                    AlbumName = a.Name,
                    AlbumReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    AlbumSongs = a.Songs
                    .Select(s => new 
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        SongWriter = s.Writer.Name,
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.SongWriter)
                    .ToList(),

                })
                .OrderByDescending(a => a.TotalAlbumPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.AlbumReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");
                int count = 0;
                foreach (var songs in album.AlbumSongs)
                {
                    count++;
                    sb.AppendLine($"---#{count}");
                    sb.AppendLine($"---SongName: {songs.SongName}");
                    sb.AppendLine($"---Price: {songs.SongPrice:f2}");
                    sb.AppendLine($"---Writer: {songs.SongWriter}");
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Include(s => s.SongPerformers)
                .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(a => a.Producer)
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    s.Duration,
                    Writer = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    FullNamePerformer = s.SongPerformers
                    .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                    .FirstOrDefault()
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.FullNamePerformer)
                .ToList();

            StringBuilder sb = new StringBuilder();

            int count = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{count++}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.Writer}");
                sb.AppendLine($"---Performer: {song.FullNamePerformer}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration:c}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
