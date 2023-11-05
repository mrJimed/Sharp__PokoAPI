using PokeAPI.Models;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;

namespace PokeAPI.Services
{
    public class FtpService : IFtpService
    {
        private string host;

        public FtpService(string host)
        {
            this.host = host;
        }

        public async Task CreateFolder(string folder, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{host}/{username}/{folder}");
            request.Credentials = new NetworkCredential(username, password);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            WebResponse response = await request.GetResponseAsync();
            response.Close();
        }

        public async Task SaveMarkdownFile(string username, string password, Pokemon pokemon)
        {
            string markdownFile = $"# Name: {pokemon.Name}\n" +
                $"Id: {pokemon.Id}\n" +
                $"Hp: {pokemon.Hp}\n" +
                $"AttackPower: {pokemon.AttackPower}\n" +
                $"Height: {pokemon.Height}\n" +
                $"Weight: {pokemon.Weight}\n" +
                $"![Image]({pokemon.Image})";
            string folder = DateTime.Now.ToString("yyyyMMdd");
            await CreateFolder(folder, username, password);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{host}/{username}/{folder}/{pokemon.Name}.md");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            byte[] file = Encoding.UTF8.GetBytes(markdownFile);
            request.ContentLength = file.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                await requestStream.WriteAsync(file, 0, file.Length);
            }
        }
    }
}
