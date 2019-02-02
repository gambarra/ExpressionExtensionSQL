ExpressionExtensionSQL - is a sample lambda expression to sql converter .NET
========================================

Packages
--------

Nugget feed: https://www.nuget.org/packages/ExpressionExtensionSQL/

| Package | NuGet Stable | NuGet Pre-release | Downloads |
| ------- | ------------ | ----------------- | --------- | 
| [ExpressionExtensionSQL](https://www.nuget.org/packages/ExpressionExtensionSQL/) | [![ExpressionExtensionSQL](https://img.shields.io/nuget/v/ExpressionExtensionSQL.svg)](https://www.nuget.org/packages/ExpressionExtensionSQL/) | [![ExpressionExtensionSQL](https://img.shields.io/nuget/vpre/ExpressionExtensionSQL.svg)](https://www.nuget.org/packages/ExpressionExtensionSQL/) | [![ExpressionExtensionSQL](https://img.shields.io/nuget/dt/ExpressionExtensionSQL.svg)](https://www.nuget.org/packages/ExpressionExtensionSQL/) |
| [ExpressionExtensionSQL.Dapper](https://www.nuget.org/packages/ExpressionExtensionSQL.Dapper/) | [![ExpressionExtensionSQL.Dapper](https://img.shields.io/nuget/v/ExpressionExtensionSQL.Dapper.svg)](https://www.nuget.org/packages/ExpressionExtensionSQL.Dapper/) | [![ExpressionExtensionSQL.Dapper](https://img.shields.io/nuget/vpre/ExpressionExtensionSQL.Dapper.svg)](https://www.nuget.org/packages/ExpressionExtensionSQL.Dapper/) | [![ExpressionExtensionSQL.Dapper](https://img.shields.io/nuget/dt/ExpressionExtensionSQL.Dapper.svg)](https://www.nuget.org/packages/ExpressionExtensionSQL.Dapper/) |

Features
--------
ExpressionExtensionSQL is a [NuGet library](https://www.nuget.org/packages/ExpressionExtensionSQL) which you can add to your project to achieve lambda expression in SQL code. It allows your domain doesn't need to understand  SQL language mainly for creating filters. Very suitable for use with [SpecificationPattern](https://en.wikipedia.org/wiki/Specification_pattern).

Samples:

```
Example usage:
```
```csharp
 Expression<Func<Merchant, bool>> expression = x => x.Name == "merchant1" && x.CreatedAt>=DateTime.Now;
 var where = expression.ToSql();
 Console.Write(where.Sql);
 
```
Output will be:
```
([Merchant].[Name] = @1) AND ([Merchant].[CreatedAt]>=@2)
```

**where.Parameters** contains a list of parameters that can be used in ADO queries.

In many cases the name of the table or column in the database is different from the name of the entity or property. For these cases this framework allows to create a mapping through attribute or fluent mapping.

## Attribute 

Class

```csharp
   [TableName("tblOrder")]
    public class Order {

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [ColumnName("amount")]
        public int TotalAmount { get; set; }
    }
```
```
Example usage:
```
```csharp
 Expression<Func<Order, bool>> expression = x => x.TotalAmount >10 && x.CreatedAt>=DateTime.Now;
 var where = expression.ToSql();
 Console.Write(where.Sql);
 
```
Output will be:
```
([tblOrder].[amount] = @1) AND ([tblOrder].[CreatedAt]>=@2)
```

## Fluent Mapping
Class

```csharp
    public class Order {

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalAmount { get; set; }
    }
```
```
Example usage:
```
```csharp

 Configuration.GetInstance().Entity<Order>().ToTable("tblTeste");
 Configuration.GetInstance().Entity<Order>().Property(p => p.TotalAmount).ToColumn("valor");
 
 Expression<Func<Order, bool>> expression = x => x.TotalAmount >10 && x.CreatedAt>=DateTime.Now;
 var where = expression.ToSql();
 Console.Write(where.Sql);
 
```
Output will be:
```
([tblTeste].[valor] = @1) AND ([tblTeste].[CreatedAt]>=@2)
```

The **Configuration** class is static and can be used anywhere on your system and the configured mapping is reused for all expressions that contain the mapped entity

ExpressExtensionSQL.Dapper
--------
ExpressionExtensionSQL.Dapper is an dapper extension that allows you to use lambda expressions as filters in your Query.

Samples:
```csharp

        public IQueryable<Period> GetAll(Expression<Func<Order, bool> filter) {

            var query = $@"SELECT * FROM [Order]
                            {{where}}"

            var connection = this.GetConnection();

            return connection.Query<Order>(query, expression: filter);
         }
```
```csharp
var result=OrderRepository.GetAll(p=>p.CreatedAt>=DateTime.Now);
```
In the above example it is important to note the use of {where}. This keyword will tell you where to include the where clause extracted from the expression.
