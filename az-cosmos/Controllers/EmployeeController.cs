using Microsoft.Azure.Cosmos;
using az_cosmos.Model;
using Microsoft.AspNetCore.Mvc;

namespace az_cosmos.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : ControllerBase
    {
        private Container ContainerClient()
        {
            var cosmosDbClient = new CosmosClient(CosmosDBAccountUri, CosmosDBAccountPrimaryKey);
            Container containerClient = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return containerClient;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            try
            {
                var container = ContainerClient();
                var response = await container.CreateItemAsync(employee, new PartitionKey(employee.Department));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetails()
        {
            try
            {
                var container = ContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Employee> feedIterator = container.GetItemQueryIterator<Employee>(queryDefinition);
                List<Employee> employees = [];
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<Employee> currenResult = await feedIterator.ReadNextAsync();
                    foreach (Employee item in currenResult)
                    {
                        employees.Add(item);
                    }
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetailsById(string employeeId, string partitionKey)
        {
            try
            {
                var container = ContainerClient();
                ItemResponse<Employee> response = await container.ReadItemAsync<Employee>(employeeId, new PartitionKey(partitionKey));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Employee payload, string partitionKey)
        {
            try
            {
                var container = ContainerClient();
                ItemResponse<Employee> res = await container.ReadItemAsync<Employee>(payload.id, new PartitionKey(partitionKey));

                var existingItem = res.Resource;
                existingItem.Name = payload.Name;
                existingItem.Country = payload.Country;
                existingItem.City = payload.City;
                existingItem.Department = payload.Department;
                existingItem.Designation = payload.Designation;

                var updateRes = await container.ReplaceItemAsync(existingItem, payload.id, new PartitionKey(partitionKey));
                return Ok(updateRes.Resource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(string employeeId, string partitionKey)
        {
            try
            {
                var container = ContainerClient();
                var response = await container.DeleteItemAsync<Employee>(employeeId, new PartitionKey(partitionKey));
                return Ok(response.StatusCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}