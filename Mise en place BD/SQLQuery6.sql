use GESTION_EMPLOYES
go

-- tests tranactSQL --
insert into [Services]values ('RESHU','Ressources humaines');
insert into [Services] (codeService, libelle) values ('ACHAT', 'Achat');

SELECT TOP (1000) [CodeService]
      ,[Libelle]
  FROM [GESTION_EMPLOYES].[dbo].[Services]

  insert into Employes (Nom) values('nafkha');

  update Employes set nom=upper(nom);

  delete from Employes
  where DATEDIFF(YEAR, DateNaissance, GETDATE())>= 65;


   insert into Employes (Nom) values('nafkha');

   set transaction isolation level read committed;

   begin tran [maj_data];

   insert into [Services] (codeService, libelle) values ('Serv3', 'Service 3'); 
   insert into [Services] (codeService, libelle) values ('Serv4', 'Service 4'); 

   save tran [before_delete];

   delete Employes where nom like '%emp _%' ;
   
   delete [Services] where CodeService like '%3%' ;

   select * from [Services];
   select * from Employes;

  -- rollback tran [before_delete];
   commit tran [maj_data];
