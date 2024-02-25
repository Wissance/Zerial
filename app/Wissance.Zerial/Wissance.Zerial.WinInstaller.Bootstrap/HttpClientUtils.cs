namespace Wissance.Zerial.WinInstaller.Bootstrap
{
    public static class HttpClientUtils
    {
        public static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            using (Stream s = await client.GetStreamAsync(uri))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.CreateNew))
                {
                    await s.CopyToAsync(fs);
                }
            }
        }
    }
}