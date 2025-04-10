using CodeAnalyzerServer;
using Npgsql;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

string connectionString = "Host=localhost;Username=postgres;Password=fbje22;Database=codeanalyzerdb";

/*using var con = new NpgsqlConnection(connectionString);
con.Open();
using var command = new NpgsqlCommand("", con);
command.CommandText = "DROP TABLE IF EXISTS teachers";
command.ExecuteNonQuery();
command.CommandText = "DROP TABLE IF EXISTS groups";
command.ExecuteNonQuery();
command.CommandText = "DROP TABLE IF EXISTS students";
command.ExecuteNonQuery();
command.CommandText = "DROP TABLE IF EXISTS entries";
command.ExecuteNonQuery();*/


DatabaseHandler handler = new DatabaseHandler(connectionString);
RequestHandler requestHandler = new RequestHandler(handler);

var tcpListener = new TcpListener(IPAddress.Any, 8888);

try
{
  tcpListener.Start();
  Console.WriteLine("Сервер запущен. Ожидание подключений... ");

  while (true)
  {
    var tcpClient = await tcpListener.AcceptTcpClientAsync();
    Task.Run(async () => await ProcessClientAsync(tcpClient));
  }
}
finally
{
  tcpListener.Stop();
}


async Task ProcessClientAsync(TcpClient tcpClient)
{
  var stream = tcpClient.GetStream();
  byte[] sizeBuffer = new byte[4];
  await stream.ReadExactlyAsync(sizeBuffer, 0, sizeBuffer.Length);
  int size = BitConverter.ToInt32(sizeBuffer, 0);

  byte[] data = new byte[size];

  int bytes = await stream.ReadAsync(data);
  var message = Encoding.UTF8.GetString(data, 0, bytes);
  Console.WriteLine($"Сообщение: {message}");


  byte[] dataResponse = Encoding.UTF8.GetBytes(requestHandler.GetResponse(message));
  byte[] sizeResponse = BitConverter.GetBytes(dataResponse.Length);

  await stream.WriteAsync(sizeResponse);
  await stream.WriteAsync(dataResponse);
  Console.WriteLine("Сообщение отправлено");
  tcpClient.Close();
}