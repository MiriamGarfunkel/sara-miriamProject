using DataEngine.app;
using DataEngine.buildTable;
using DataEngine.condition;
using DataEngine.models;
using Microsoft.AspNetCore.Mvc;

namespace DataEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private static DataBase _database = new DataBase { Name = "MainDB" };
        private static DataClient _client = new DataClient(_database);

        [HttpPost("create-table")]
        public IActionResult CreateTable([FromBody] CreateTableRequest request)
        {
            try
            {
                var result = _client.CreateTable(request.TableName, builder =>
                {
                    foreach (var col in request.Columns)
                    {
                        builder.AddColumn(col.Name, col.DataType);
                    }
                });
                return Ok(new { success = true, message = $"Table '{request.TableName}' created", affectedRows = result.Count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("insert")]
        public IActionResult Insert([FromBody] InsertRequest request)
        {
            try
            {
                Console.WriteLine($"Insert request - Table: {request.TableName}");
                Console.WriteLine($"Values: {string.Join(", ", request.Values.Select(v => $"{v.Key}={v.Value} ({v.Value?.GetType().Name})"))}");
                
                // Convert JsonElement values to proper types
                var convertedValues = new Dictionary<string, object>();
                var table = _database.GetTable(request.TableName);
                
                foreach (var col in table.Schema.Columns)
                {
                    if (request.Values.ContainsKey(col.Name))
                    {
                        var value = request.Values[col.Name];
                        
                        if (value is System.Text.Json.JsonElement jsonElement)
                        {
                            if (col.DataType == DataType.String)
                                convertedValues[col.Name] = jsonElement.GetString();
                            else if (col.DataType == DataType.Int)
                                convertedValues[col.Name] = jsonElement.GetInt32();
                            else if (col.DataType == DataType.Bool)
                                convertedValues[col.Name] = jsonElement.GetBoolean();
                        }
                        else
                        {
                            convertedValues[col.Name] = value;
                        }
                    }
                }
                
                var row = new Row { Values = convertedValues };
                var result = _client.Insert(request.TableName, row);
                
                Console.WriteLine($"Insert result count: {result.Count}");
                
                return Ok(new { success = true, message = "Row inserted", data = result, affectedRows = result.Count });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert error: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("query")]
        public IActionResult Query([FromBody] QueryRequest request)
        {
            try
            {
                var condition = BuildCondition(request.Condition);
                var result = _client.Query(request.TableName, condition);
                return Ok(new { success = true, data = result, count = result.Count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody] UpdateRequest request)
        {
            try
            {
                Console.WriteLine($"Update request - Table: {request.TableName}");
                Console.WriteLine($"Condition: {request.Condition?.Type}, Column: {request.Condition?.Column}, Operator: {request.Condition?.Operator}, Value: {request.Condition?.Value}");
                Console.WriteLine($"New values: {string.Join(", ", request.NewValues.Select(v => $"{v.Key}={v.Value}"))}");
                
                var table = _database.GetTable(request.TableName);
                
                // Convert JsonElement values to proper types
                var convertedValues = new Dictionary<string, object>();
                foreach (var kvp in request.NewValues)
                {
                    var col = table.Schema.Columns.FirstOrDefault(c => c.Name == kvp.Key);
                    if (col != null)
                    {
                        object convertedValue = kvp.Value;
                        
                        if (kvp.Value is System.Text.Json.JsonElement jsonElement)
                        {
                            if (col.DataType == DataType.String)
                                convertedValue = jsonElement.GetString();
                            else if (col.DataType == DataType.Int)
                            {
                                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                                    convertedValue = int.Parse(jsonElement.GetString());
                                else
                                    convertedValue = jsonElement.GetInt32();
                            }
                            else if (col.DataType == DataType.Bool)
                            {
                                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                                    convertedValue = bool.Parse(jsonElement.GetString());
                                else
                                    convertedValue = jsonElement.GetBoolean();
                            }
                        }
                        else if (kvp.Value is string strValue)
                        {
                            if (col.DataType == DataType.Int)
                                convertedValue = int.Parse(strValue);
                            else if (col.DataType == DataType.Bool)
                                convertedValue = bool.Parse(strValue);
                            else
                                convertedValue = strValue;
                        }
                        
                        convertedValues[kvp.Key] = convertedValue;
                    }
                }
                
                var condition = BuildCondition(request.Condition);
                var result = _client.Update(request.TableName, condition, convertedValues);
                
                Console.WriteLine($"Update result count: {result.Count}");
                
                return Ok(new { success = true, message = "Rows updated", data = result, affectedRows = result.Count });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody] DeleteRequest request)
        {
            try
            {
                var condition = BuildCondition(request.Condition);
                var result = _client.Delete(request.TableName, condition);
                return Ok(new { success = true, message = "Rows deleted", data = result, affectedRows = result.Count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("clone-table")]
        public IActionResult CloneTable([FromBody] CloneTableRequest request)
        {
            try
            {
                var result = _client.CloneTable(request.SourceTableName, request.NewTableName);
                return Ok(new { success = true, message = $"Table cloned to '{request.NewTableName}'", affectedRows = result.Count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("remove-table/{tableName}")]
        public IActionResult RemoveTable(string tableName)
        {
            try
            {
                var result = _client.Remove(tableName);
                return Ok(new { success = true, message = $"Table '{tableName}' removed", affectedRows = result.Count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("tables")]
        public IActionResult GetTables()
        {
            try
            {
                var tables = _database.Tables.Select(t => new
                {
                    name = t.Key,
                    columns = t.Value.Schema.Columns.Select(c => new { name = c.Name, dataType = c.DataType.ToString() }),
                    rowCount = t.Value.Rows.Count
                });
                return Ok(new { success = true, tables });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("table/{tableName}")]
        public IActionResult GetTableData(string tableName)
        {
            try
            {
                var table = _database.GetTable(tableName);
                if (table == null)
                    return NotFound(new { success = false, message = "Table not found" });

                return Ok(new
                {
                    success = true,
                    name = table.Name,
                    columns = table.Schema.Columns.Select(c => new { name = c.Name, dataType = c.DataType.ToString() }),
                    rows = table.Rows.Select(r => r.Values)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private ICondition BuildCondition(ConditionDto dto)
        {
            if (dto == null || dto.Left == null && dto.Right == null && string.IsNullOrEmpty(dto.Column))
            {
                throw new ArgumentException("Invalid condition");
            }
            
            if (dto.Type == "simple")
            {
                // Get the table and column to determine the correct type
                object value = dto.Value;
                
                // Convert JsonElement to string first
                if (value is System.Text.Json.JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                        value = jsonElement.GetString();
                    else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                        value = jsonElement.GetInt32();
                    else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.True)
                        value = true;
                    else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.False)
                        value = false;
                }
                
                // Now convert string to proper type if needed
                if (value is string strValue)
                {
                    if (int.TryParse(strValue, out int intValue))
                        value = intValue;
                    else if (bool.TryParse(strValue, out bool boolValue))
                        value = boolValue;
                }
                
                var op = dto.Operator switch
                {
                    "=" => ConditionOperator.Equal,
                    ">" => ConditionOperator.GreaterThan,
                    "<" => ConditionOperator.LessThan,
                    "!=" => ConditionOperator.NotEqual,
                    _ => ConditionOperator.Equal
                };
                return new BasicCondition(dto.Column, op, value);
            }
            else if (dto.Type == "and")
            {
                var andCondition = new AndCondition();
                andCondition.AddCondition(BuildCondition(dto.Left));
                andCondition.AddCondition(BuildCondition(dto.Right));
                return andCondition;
            }
            else if (dto.Type == "or")
            {
                var orCondition = new OrCondition();
                orCondition.AddCondition(BuildCondition(dto.Left));
                orCondition.AddCondition(BuildCondition(dto.Right));
                return orCondition;
            }
            throw new ArgumentException("Invalid condition type");
        }
    }

    public class CreateTableRequest
    {
        public string TableName { get; set; }
        public List<ColumnDto> Columns { get; set; }
    }

    public class ColumnDto
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
    }

    public class InsertRequest
    {
        public string TableName { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }

    public class QueryRequest
    {
        public string TableName { get; set; }
        public ConditionDto Condition { get; set; }
    }

    public class UpdateRequest
    {
        public string TableName { get; set; }
        public ConditionDto Condition { get; set; }
        public Dictionary<string, object> NewValues { get; set; }
    }

    public class DeleteRequest
    {
        public string TableName { get; set; }
        public ConditionDto Condition { get; set; }
    }

    public class CloneTableRequest
    {
        public string SourceTableName { get; set; }
        public string NewTableName { get; set; }
    }

    public class ConditionDto
    {
        public string Type { get; set; } // "simple", "and", "or"
        public string? Column { get; set; }
        public string? Operator { get; set; }
        public object? Value { get; set; }
        public ConditionDto? Left { get; set; }
        public ConditionDto? Right { get; set; }
    }
}
