using Microsoft.Azure.Cosmos;

public class Program
{
    // Endpoint
    private static readonly string EndPointUri = "";
    // primary key
    private static readonly string PrimaryKey = "";
    // cosmos client instance
    private CosmosClient cosmosClient;
    // create a db
    private Database database;
    // container
    private Container container;
    // names of the db and container we need
    private string databaseId = "az204Database";
    private string containerId = "az204Container";

    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Beginning operations...\n");
            Program p = new Program();
            await p.CosmosAsync();
        }
        catch (CosmosException de)
        {
            Exception baseException = de.GetBaseException();
            Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        finally
        {
            Console.WriteLine("End of program, press any key to exit.");
            Console.ReadKey();
        }
    }

    public async Task CosmosAsync()
    {
        this.cosmosClient = new CosmosClient(EndPointUri, PrimaryKey); // creates a new instance of the cosmos client
        await this.CreateDatabaseAsync(); // run the database create method
        await this.CreateContainerAsync(); // runs the container create method
    }

    public async Task CreateDatabaseAsync()
    {
        this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Console.WriteLine("Created Database: {0}\n", this.database.Id);
    }

    public async Task CreateContainerAsync()
    {
        this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
        Console.WriteLine("Created Container: {0}\n", this.container.Id);
    }
}