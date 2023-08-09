use BdTest
go

-- base des données de test --
select convert(int,'1002');
select datediff(year, '18/06/1979', getdate());
select convert(numeric(4,2), round(100.0/3,2));
select DATEADD(day, 30, getdate());
select charindex('Server','SQL Server-SQL et Transact-SQL',0);
select SUBSTRING('SQL Server-SQL et Transact-SQL',select charindex('Server','SQL Server-SQL et Transact-SQL',0),6);