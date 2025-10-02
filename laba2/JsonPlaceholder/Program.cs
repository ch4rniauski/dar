using System.Net.Http.Json;
using JsonPlaceholder.Models;

var http = new HttpClient
{
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
};
var users = await http.GetFromJsonAsync<List<User>>("users");

foreach (var u in users!)
{
    Console.WriteLine($"{u.Id}:" +
                      $"\n\tName: {u.Name}" +
                      $"\n\tPhone: {u.Phone}" +
                      $"\n\tEmail: {u.Email}" +
                      $"\n\tUsername: {u.Username}" +
                      $"\n\tWebsite: {u.Website}" +

                      $"\n\tAddress:" +
                      $"\n\t\tStreet: {u.Address!.Street}" +
                      $"\n\t\tSuite: {u.Address!.Suite}" +
                      $"\n\t\tCity: {u.Address!.City}" +
                      $"\n\t\tZipcode: {u.Address!.Zipcode}" +

                      $"\n\tCompany:" +
                      $"\n\t\tName: {u.Company!.Name}" +
                      $"\n\t\tCatchPhrase: {u.Company!.CatchPhrase}" +
                      $"\n\t\tBS: {u.Company!.Bs}");
}
