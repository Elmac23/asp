using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Lista4.Services;
using Lista4.Models;

namespace Lista4
{
    public class MySettings
    {
        public string? OptionA { get; set; }
        public NestedSettings? Nested { get; set; }
    }

    public class NestedSettings
    {
        public string? Value { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.custom.json", optional: true, reloadOnChange: true)
                .AddXmlFile("appsettings.custom.xml", optional: true, reloadOnChange: true);

            builder.Services.AddScoped<IDapperRepository, DapperRepository>();

            builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    if (ex.GetType().Name == "AmbiguousMatchException")
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync($"AmbiguousMatchException caught: {ex.Message}");
                        return;
                    }

                    throw;
                }
            });

            app.MapGet("/param/{value}", (string value) =>
                Results.Ok(new { route = "/param/{value}", value })
            );

            app.MapGet("/type/{id:int}", (int id) =>
                Results.Ok(new { route = "/type/{id:int}", id })
            );

            app.MapGet("/length/{code:maxlength(3)}", (string code) =>
                Results.Ok(new { route = "/length/{code:maxlength(3)}", code })
            );

            app.MapGet("/required/{name}", (string name) =>
                Results.Ok(new { route = "/required/{name}", name })
            );

            app.MapGet("/optional/{name?}", (string? name) =>
                Results.Ok(new { route = "/optional/{name?}", name })
            );


            app.MapGet("/regex/{code:regex(^[A-Z]{{3}}\\d{{2}}$)}", (string code) =>
                Results.Ok(new { route = "/regex/{code:regex(^[A-Z]{{3}}\\d{{2}}$)}", code })
            );

            app.MapGet("/overlap/{x}", (string x) =>
                Results.Ok(new { route = "/overlap/{x}", x, handler = "string" })
            ).WithName("OverlapGeneric");

            app.MapGet("/overlap/{x:int}", (int x) =>
                Results.Ok(new { route = "/overlap/{x:int}", x, handler = "int" })
            ).WithName("OverlapInt");

            app.MapGet("/", (IDapperRepository repo) =>
            {
                var all = repo.GetAll();
                return Results.Ok(all);
            });

            app.MapGet("/repo/{id:int}", (int id, IDapperRepository repo) =>
            {
                var p = repo.GetById(id);
                return p is null ? Results.NotFound() : Results.Ok(p);
            });

            app.MapPost("/repo", (Person person, IDapperRepository repo) =>
            {
                if (string.IsNullOrWhiteSpace(person.Name) || person.Age < 0)
                {
                    return Results.BadRequest("Invalid person data");
                }

                repo.Add(person);
                return Results.Created($"/repo/{person.Id}", person);
            });

            app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

            app.MapGet("/form", (HttpContext ctx) =>
            {
                var error = ctx.Request.Query["error"].ToString();
                var name = ctx.Request.Query["name"].ToString();
                var age = ctx.Request.Query["age"].ToString();

                var errorHtml = string.IsNullOrEmpty(error) ? "" : $"<div style='color:red'>{System.Net.WebUtility.HtmlEncode(error)}</div>";

                var html = $@"<!doctype html>
<html>
<head>
  <meta charset=""utf-8""> 
  <title>Simple Form</title>
</head>
<body>
  <h1>Simple Form</h1>
  {errorHtml}
  <form method=""post"" action=""/submit"">
    <label>Name: <input type=""text"" name=""name"" value=""{System.Net.WebUtility.HtmlEncode(name)}"" /></label><br/>
    <label>Age: <input type=""text"" name=""age"" value=""{System.Net.WebUtility.HtmlEncode(age)}"" /></label><br/>
    <button type=""submit"">Submit</button>
  </form>
</body>
</html>";

                return Results.Content(html, "text/html");
            });

            app.MapPost("/submit", async (HttpContext ctx) =>
            {
                var form = await ctx.Request.ReadFormAsync();
                var name = form["name"].ToString();
                var ageStr = form["age"].ToString();

                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(name)) errors.Add("Name is required.");
                if (!int.TryParse(ageStr, out var age) || age < 0) errors.Add("Age must be a non-negative integer.");

                if (errors.Count > 0)
                {
                    var errorMsg = string.Join(" ", errors);
                    var query = $"?error={Uri.EscapeDataString(errorMsg)}&name={Uri.EscapeDataString(name)}&age={Uri.EscapeDataString(ageStr)}";
                    return Results.Redirect($"/form{query}");
                }

                var qs = $"?name={Uri.EscapeDataString(name)}&age={age}";
                return Results.Redirect($"/print{qs}");
            });

            app.MapGet("/print", (HttpContext ctx) =>
            {
                var q = ctx.Request.Query;
                var name = q["name"].ToString();
                var age = q["age"].ToString();

                var html = $@"<!doctype html>
<html>
<head>
  <meta charset=""utf-8""> 
  <title>Print</title>
</head>
<body>
  <h1>Print Page</h1>
  <p><strong>Name:</strong> {System.Net.WebUtility.HtmlEncode(name)}</p>
  <p><strong>Age:</strong> {System.Net.WebUtility.HtmlEncode(age)}</p>
  <p><a href=""/form"">Back to form</a></p>
</body>
</html>";

                return Results.Content(html, "text/html");
            });

            app.MapGet("/config/indexer", (IConfiguration config) =>
            {
                var appName = config["AppName"];
                return Results.Ok(new { AppName = appName });
            });

            app.MapGet("/config/section", (IConfiguration config) =>
            {
                var section = config.GetSection("MySettings");
                var optionA = section["OptionA"];
                var nestedValue = section.GetSection("Nested")["Value"];
                return Results.Ok(new { OptionA = optionA, Nested = nestedValue });
            });

            app.MapGet("/config/options", (IOptions<MySettings> opts) =>
            {
                var s = opts.Value;
                return Results.Ok(s);
            });

            app.Run();
        }
    }
}
