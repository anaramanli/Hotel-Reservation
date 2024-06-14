namespace Hotel.Extensions
{
    public static class FileExtensions
    {
        public static bool IsValidType(this IFormFile file, string type)
        {
            return file.ContentType.StartsWith(type);
        }

        public static bool IsValidLength(this IFormFile file, long maxSize)
        {
            return file.Length <= maxSize * 1024;
        }

        public static async Task<string> SaveFileAsync(this IFormFile file, string uploadPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName)
                          + "_"
                          + Path.GetRandomFileName().Substring(0, 8)
                          + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
