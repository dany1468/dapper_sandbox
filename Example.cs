using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Shouldly;

public class ExampleTests
{
    public class ExampleWithSetter
    {
        public bool ExampleInt { get; set; }
        public bool ExampleBit { get; set; }
    }

    public class ExampleWithDefaultConstructor
    {
        public ExampleWithDefaultConstructor() {}
        public bool ExampleInt { get; }
        public bool ExampleBit { get; }
    }

    public class ExampleWithGetOnly
    {
        public ExampleWithGetOnly() {}
        public bool ExampleInt { get; }
        public bool ExampleBit { get; }
    }

    public class ExampleWithConstructor
    {
        public ExampleWithConstructor(bool exampleInt, bool exampleBit) =>
            (ExampleInt, ExampleBit) = (exampleInt, exampleBit);

        public bool ExampleInt { get; }
        public bool ExampleBit { get; }
    }

    public class ExampleWithConstructorAndDefaultConstructor
    {
        public ExampleWithConstructorAndDefaultConstructor() {}
        public ExampleWithConstructorAndDefaultConstructor(bool exampleInt, bool exampleBit) =>
            (ExampleInt, ExampleBit) = (exampleInt, exampleBit);

        public bool ExampleInt { get; }
        public bool ExampleBit { get; }
    }

    public class ExampleWithSameTypeMappedConstructor
    {
        public ExampleWithSameTypeMappedConstructor(int exampleInt, bool exampleBit) =>
            (ExampleInt, ExampleBit) = (exampleInt != 0, exampleBit);

        public bool ExampleInt { get; }
        public bool ExampleBit { get; }
    }

    public async Task ShouldAdd()
    {
        using (var connection = new SqlConnection("Server=127.0.0.1,14331;Database=main;User Id=SA;Password=P@55w0rd;MultipleActiveResultSets=True;"))
        {
            await connection.OpenAsync();

            connection.ExecuteAsync("INSERT INTO Example (ExampleInt, ExampleBit) VALUES (1, 1)");

            var sql = @"
SELECT
    ExampleInt, ExampleBit
FROM
    Example
";
            var result1 = (await connection.QueryAsync<ExampleWithSetter>(sql)).First();
            result1.ExampleInt.ShouldBeTrue();
            result1.ExampleBit.ShouldBeTrue();

            var result2 = (await connection.QueryAsync<ExampleWithDefaultConstructor>(sql)).First();
            result2.ExampleInt.ShouldBeTrue();
            result2.ExampleBit.ShouldBeTrue();

            var result3 = (await connection.QueryAsync<ExampleWithGetOnly>(sql)).First();
            result3.ExampleInt.ShouldBeTrue();
            result3.ExampleBit.ShouldBeTrue();

            Should.Throw<InvalidOperationException>(async () =>
            {
                await connection.QueryAsync<ExampleWithConstructor>(sql);
            });

            var result4 = (await connection.QueryAsync<ExampleWithConstructorAndDefaultConstructor>(sql)).First();
            result4.ExampleInt.ShouldBeTrue();
            result4.ExampleBit.ShouldBeTrue();

            var result5 = (await connection.QueryAsync<ExampleWithSameTypeMappedConstructor>(sql)).First();
            result5.ExampleInt.ShouldBeTrue();
            result5.ExampleBit.ShouldBeTrue();
        }
    }
}
