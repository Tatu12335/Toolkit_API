class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            using (var conn = new HttpClient())
            {
                var content =

                await conn.PostAsync("https://localhost:7023/User/Create_User");
            }
        }
    }
}
