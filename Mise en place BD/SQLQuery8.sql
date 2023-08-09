use GESTION_EMPLOYES
go
 -- module 9 -- 
 select distinct CodeService from Employes;
delete from [Services]
		where CodeService not in ( select distinct CodeService from Employes); 
select  * from [Services];
select  * from Employes


select * from [Services] as S where (select distinct Codeservice  from Employes
		where Employes.CodeService = S.CodeService) = notNull; 
select nom, prenom, salaire from Employes 
	where Salaire > (select AVG(salaire) moyenne from Employes);

select nom, prenom, salaire, moyenne = Salaire + 1  from Employes 
		where Salaire > (select AVG(salaire) moyenne from Employes);

with T1 as
(
select * from Employes
)
select * from T1;

select CodeService from Employes
except
select CodeService from [Services] where CodeService='RESHU';
go

create view V1  
	as select CodeService, nom from Employes;
go

select * from V1