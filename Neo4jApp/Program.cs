using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Person
{
    public string Name { get; set; } = string.Empty;
    public int? Born { get; set; }
}

class Movie
{
    public string Title { get; set; } = string.Empty;
    public int? Released { get; set; }
    public long? Budget { get; set; }
    public string? Genre { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        const string dbUri = "neo4j+s://a5b75dc8.databases.neo4j.io";
        const string dbUser = "neo4j";
        const string dbPassword = "77S3LM8Ly4CPF7lYjYu-nLx_6Ad40v0FHUiHXXnA_sY";

        await using var driver = GraphDatabase.Driver(dbUri, AuthTokens.Basic(dbUser, dbPassword));

        try
        {
            await driver.VerifyConnectivityAsync();
            Console.WriteLine("Connected to Neo4j.");

            await using var session = driver.AsyncSession(o => o.WithDatabase("neo4j"));

            while (true)
            {
                PrintMenu();
                var choice = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(choice)) continue;

                switch (choice)
                {
                    case "1": // Create person
                        await CreatePersonCli(session);
                        break;
                    case "2": // Read all persons
                        await ListPersonsCli(session);
                        break;
                    case "3": // Update person
                        await UpdatePersonCli(session);
                        break;
                    case "4": // Delete person
                        await DeletePersonCli(session);
                        break;
                    case "5": // Create ACTED_IN
                        await CreateActedInCli(session);
                        break;
                    case "6": // Create DIRECTED
                        await CreateDirectedCli(session);
                        break;
                    case "7": // List relationships
                        await ListRelationshipsCli(session);
                        break;
                    case "8": // Delete relationship
                        await DeleteRelationshipCli(session);
                        break;
                    case "9": // Initialize sample dataset (tasks provided)
                        await InitializeSampleDataCli(session);
                        break;
                    case "10": // List movies
                        await ListMoviesCli(session);
                        break;
                    case "11": // List actors with roles
                        await ListActorsWithRolesCli(session);
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }

                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1) Create Person");
        Console.WriteLine("2) List Persons");
        Console.WriteLine("3) Update Person (born)");
        Console.WriteLine("4) Delete Person");
        Console.WriteLine("5) Create ACTED_IN relationship");
        Console.WriteLine("6) Create DIRECTED relationship");
        Console.WriteLine("7) List relationships (ACTED_IN, DIRECTED)");
        Console.WriteLine("8) Delete relationship (by person + movie)");
        Console.WriteLine("9) Initialize sample dataset (Task scripts)");
        Console.WriteLine("10) List Movies");
        Console.WriteLine("11) List Actors with Roles");
        Console.WriteLine("0) Exit");
        Console.Write("> ");
    }

    static async Task CreatePersonCli(IAsyncSession session)
    {
        Console.Write("Name: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name)) { Console.WriteLine("Name required."); return; }

        Console.Write("Born (year, optional): ");
        var bornText = Console.ReadLine()?.Trim();
        int? born = null;
        if (int.TryParse(bornText, out var b)) born = b;

        var person = new Person { Name = name, Born = born };

        var created = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "CREATE (p:Person {name:$name, born:$born}) RETURN p.name AS name, p.born AS born",
                new { name = person.Name, born = person.Born });
            var rec = await cursor.SingleAsync();
            return new Person { Name = rec.Get<string>("name"), Born = rec["born"]?.As<int?>() };
        });

        Console.WriteLine($"Created: {created.Name} ({created.Born?.ToString() ?? "Unknown"})");
    }

    static async Task ListPersonsCli(IAsyncSession session)
    {
        var persons = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync("MATCH (p:Person) RETURN p.name AS name, p.born AS born ORDER BY p.name");
            var records = await cursor.ToListAsync();
            var list = new List<Person>();
            foreach (var r in records)
            {
                var name = r.Get<string>("name");
                var born = r["born"]?.As<int?>();
                list.Add(new Person { Name = name, Born = born });
            }
            return list;
        });

        Console.WriteLine("Persons:");
        foreach (var p in persons)
        {
            Console.WriteLine($"- {p.Name} ({p.Born?.ToString() ?? "Unknown"})");
        }
    }

    static async Task UpdatePersonCli(IAsyncSession session)
    {
        Console.Write("Name of person to update: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name)) { Console.WriteLine("Name required."); return; }

        Console.Write("New born year (leave empty to clear): ");
        var bornText = Console.ReadLine()?.Trim();
        int? born = null;
        if (int.TryParse(bornText, out var b)) born = b;

        var updated = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (p:Person {name:$name}) SET p.born = $born RETURN p.name AS name, p.born AS born",
                new { name, born });
            var recs = await cursor.ToListAsync();
            if (recs.Count == 0) return null;
            var r = recs[0];
            return new Person { Name = r.Get<string>("name"), Born = r["born"]?.As<int?>() };
        });

        if (updated == null) Console.WriteLine("Person not found.");
        else Console.WriteLine($"Updated: {updated.Name} ({updated.Born?.ToString() ?? "Unknown"})");
    }

    static async Task DeletePersonCli(IAsyncSession session)
    {
        Console.Write("Name of person to delete: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name)) { Console.WriteLine("Name required."); return; }

        var deleted = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (p:Person {name:$name}) DETACH DELETE p RETURN count(p) AS deleted",
                new { name });
            var rec = await cursor.SingleAsync();
            return rec.Get<long>("deleted");
        });

        Console.WriteLine(deleted > 0 ? "Person deleted." : "Person not found.");
    }

    static async Task CreateActedInCli(IAsyncSession session)
    {
        Console.Write("Person name: ");
        var person = Console.ReadLine()?.Trim();
        Console.Write("Movie title: ");
        var movie = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(person) || string.IsNullOrEmpty(movie)) { Console.WriteLine("Both values required."); return; }

        Console.Write("Role: ");
        var role = Console.ReadLine()?.Trim();

        var created = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (p:Person {name:$person}), (m:Movie {title:$movie}) CREATE (p)-[r:ACTED_IN {role:$role}]->(m) RETURN p.name AS person, m.title AS movie, r.role AS role",
                new { person, movie, role });
            var rec = await cursor.SingleAsync();
            return (Person: rec.Get<string>("person"), Movie: rec.Get<string>("movie"), Role: rec["role"]?.As<string?>());
        });

        Console.WriteLine($"Created ACTED_IN: {created.Person} as {created.Role ?? "(unknown)"} in {created.Movie}");
    }

    static async Task CreateDirectedCli(IAsyncSession session)
    {
        Console.Write("Person name: ");
        var person = Console.ReadLine()?.Trim();
        Console.Write("Movie title: ");
        var movie = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(person) || string.IsNullOrEmpty(movie)) { Console.WriteLine("Both values required."); return; }

        var created = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (p:Person {name:$person}), (m:Movie {title:$movie}) CREATE (p)-[:DIRECTED]->(m) RETURN p.name AS person, m.title AS movie",
                new { person, movie });
            var rec = await cursor.SingleAsync();
            return (Person: rec.Get<string>("person"), Movie: rec.Get<string>("movie"));
        });

        Console.WriteLine($"Created DIRECTED: {created.Person} -> {created.Movie}");
    }

    static async Task ListRelationshipsCli(IAsyncSession session)
    {
        var items = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(@"MATCH (p:Person)-[r]->(m:Movie)
WHERE type(r) IN ['ACTED_IN','DIRECTED']
RETURN p.name AS person, type(r) AS rel, r.role AS role, m.title AS movie
ORDER BY person, movie");
            var records = await cursor.ToListAsync();
            var list = new List<(string Person, string Rel, string? Role, string Movie)>();
            foreach (var r in records)
            {
                list.Add((r.Get<string>("person"), r.Get<string>("rel"), r["role"]?.As<string?>(), r.Get<string>("movie")));
            }
            return list;
        });

        Console.WriteLine("Relationships:");
        foreach (var it in items)
        {
            if (it.Rel == "ACTED_IN") Console.WriteLine($"- {it.Person} acted as {it.Role ?? "(unknown)"} in {it.Movie}");
            else Console.WriteLine($"- {it.Person} {it.Rel} {it.Movie}");
        }
    }

    static async Task DeleteRelationshipCli(IAsyncSession session)
    {
        Console.Write("Person name: ");
        var person = Console.ReadLine()?.Trim();
        Console.Write("Movie title: ");
        var movie = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(person) || string.IsNullOrEmpty(movie)) { Console.WriteLine("Both values required."); return; }

        var deleted = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "MATCH (p:Person {name:$person})-[r]->(m:Movie {title:$movie}) WHERE type(r) IN ['ACTED_IN','DIRECTED'] DELETE r RETURN count(r) AS deleted",
                new { person, movie });
            var rec = await cursor.SingleAsync();
            return rec.Get<long>("deleted");
        });

        Console.WriteLine(deleted > 0 ? "Relationship(s) deleted." : "No matching relationship found.");
    }

    static async Task InitializeSampleDataCli(IAsyncSession session)
    {
        Console.WriteLine("Initializing sample dataset (this may create duplicate nodes if run multiple times)...");

        await session.ExecuteWriteAsync(async tx =>
        {
            // Task 1 dataset
            var q1 = @"CREATE (charlie:Person:Actor {name: 'Charlie Sheen'}),
       (martin:Person:Actor {name: 'Martin Sheen'}),
       (michael:Person:Actor {name: 'Michael Douglas'}),
       (oliver:Person:Director {name: 'Oliver Stone'}),
       (rob:Person:Director {name: 'Rob Reiner'}),
       (wallStreet:Movie {title: 'Wall Street'}),
       (charlie)-[:ACTED_IN {role: 'Bud Fox'}]->(wallStreet),
       (martin)-[:ACTED_IN {role: 'Carl Fox'}]->(wallStreet),
       (michael)-[:ACTED_IN {role: 'Gordon Gekko'}]->(wallStreet),
       (oliver)-[:DIRECTED]->(wallStreet),
       (thePresident:Movie {title: 'The American President'}),
       (martin)-[:ACTED_IN {role: 'A.J. MacInerney'}]->(thePresident),
       (michael)-[:ACTED_IN {role: 'President Andrew Shepherd'}]->(thePresident),
       (rob)-[:DIRECTED]->(thePresident)";
            await tx.RunAsync(q1);

            // Task 2 additional data (run statements separately)
            var q2 = @"CREATE (newActor1:Person:Actor {name: 'Jennifer Lawrence', born: 1990});
CREATE (newActor2:Person:Actor {name: 'Leonardo DiCaprio', born: 1974});
CREATE (newMovie1:Movie {title: 'Inception', released: 2010});
CREATE (newMovie2:Movie {title: 'The Hunger Games', released: 2012});

MATCH (m:Movie {title: 'Inception'})
SET m.budget = 160000000, m.genre = 'Sci-Fi';

MATCH (a:Person {name: 'Leonardo DiCaprio'})
MATCH (m:Movie {title: 'Inception'})
CREATE (a)-[:ACTED_IN {role: 'Dom Cobb'}]->(m);

MATCH (a:Person {name: 'Jennifer Lawrence'})
MATCH (m:Movie {title: 'The Hunger Games'})
CREATE (a)-[:ACTED_IN {role: 'Katniss Everdeen'}]->(m);

MATCH (m:Movie {title: 'The Hunger Games'})
SET m.released = 2013;";
            var parts = q2.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                await tx.RunAsync(trimmed);
            }

            // Task 3-5 helpers
            var q3 = @"CREATE (lonelyActor:Person:Actor {name: 'Nobody Famous', born: 1990});
CREATE (titanic:Movie {title: 'Titanic', released: 1997});
CREATE (revenant:Movie {title: 'The Revenant', released: 2015});
WITH titanic, revenant
MATCH (leo:Person {name: 'Leonardo DiCaprio'})
CREATE (leo)-[:ACTED_IN {role: 'Jack Dawson'}]->(titanic)
CREATE (leo)-[:ACTED_IN {role: 'Hugh Glass'}]->(revenant)";
            var parts3 = q3.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts3)
            {
                var trimmed = part.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                await tx.RunAsync(trimmed);
            }

            return true;
        });

        Console.WriteLine("Sample data initialization finished.");
    }

    static async Task ListMoviesCli(IAsyncSession session)
    {
        var movies = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync("MATCH (m:Movie) RETURN m.title AS title, m.released AS released, m.budget AS budget, m.genre AS genre ORDER BY title");
            var records = await cursor.ToListAsync();
            var list = new List<Movie>();
            foreach (var r in records)
            {
                list.Add(new Movie
                {
                    Title = r.Get<string>("title"),
                    Released = r["released"]?.As<int?>(),
                    Budget = r["budget"]?.As<long?>(),
                    Genre = r["genre"]?.As<string?>()
                });
            }
            return list;
        });

        Console.WriteLine("Movies:");
        foreach (var m in movies)
        {
            Console.WriteLine($"- {m.Title} (released: {m.Released?.ToString() ?? "Unknown"}, genre: {m.Genre ?? "Unknown"}, budget: {m.Budget?.ToString() ?? "Unknown"})");
        }
    }

    static async Task ListActorsWithRolesCli(IAsyncSession session)
    {
        var items = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync("MATCH (p:Person)-[r:ACTED_IN]->(m:Movie) RETURN p.name AS actor, r.role AS role, m.title AS movie ORDER BY actor, movie");
            var records = await cursor.ToListAsync();
            var list = new List<(string Actor, string? Role, string Movie)>();
            foreach (var r in records)
            {
                list.Add((r.Get<string>("actor"), r["role"]?.As<string?>(), r.Get<string>("movie")));
            }
            return list;
        });

        Console.WriteLine("Actors and roles:");
        foreach (var it in items)
        {
            Console.WriteLine($"- {it.Actor} as {it.Role ?? "(unknown)"} in {it.Movie}");
        }
    }
}