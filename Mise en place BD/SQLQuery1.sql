USE GESTION_EMPLOYES
GO
-- Exemple de cours 

CREATE TABLE Employes(
CodeEmp INT				not null identity(1,1), 
Nom VARCHAR(20)			not null,
Prenom CHAR(20)			not null,
DateNaissance DATE		null,
DateEmbauche DATE		not null constraint DF_Employes_DateEmbauche default getdate(),
DateModif TIMESTAMP		null,
Salaire DECIMAL(8,2)	not null constraint DF_Employes_Salaire default 0
								 constraint CK_Employes_Salaire check (Salaire >=0),
CodeService CHAR(5)		not null,
CodeChef INT			null,
 constraint PK_Employes primary key(CodeEmp)
);

CREATE TABLE [Services] (
CodeService char(5) not null,
Libelle varchar(30) not null constraint UN_Services_Libelle UNIQUE,
constraint PK_Services primary key(CodeService)
);

CREATE TABLE Conges_Mens (
CodeEmp int					not null,
Annee numeric(4)			not null,
Mois numeric(2)			not null constraint CK_Conges_Mens_Mois check (Mois >=1 and Mois <=12),
NbJoursPris numeric(2,0)	null constraint DF_Conges_Mens_NbJoursPris default 0,
-- constraint UN_Conges_Mens_Annee unique(Annee),							
-- constraint UN_Conges_Mens_Mois unique(Mois),
constraint PK_Conges_Mens_CodeEmp primary key(CodeEmp, Annee, Mois)
);

create table Conges(
CodeEmp int					not null, 
Annee numeric(4)			not null, -- constraint UN_Conges_Annee unique,
NbJoursAcquis numeric(2)	null constraint DF_Conges_NbJoursAcquis default 0,
constraint PK_Conges_CodeEmp primary key(CodeEmp, Annee)
);

alter table Employes
	alter column Prenom varchar(50) not null;

alter table Employes
	drop column DateModif;

-- ajout/suppression contraintes

alter table Employes
	with nocheck add constraint CK_Employes_Salaire_2 check (Salaire <=9999);

alter table Employes
	nocheck constraint CK_Employes_Salaire_2;

alter table Employes
	drop constraint CK_Employes_Salaire_2;

alter table Employes with check add
	constraint FK_Employes_CodeService foreign key (CodeService)
		references Services(CodeService),
	constraint FK_Employes_CodeChef foreign key (CodeChef)
		references Employes(CodeEmp);
alter table Conges with check add
	constraint FK_Conges_Employes foreign key (CodeEmp)
		references Employes(CodeEmp); 
alter table Conges_Mens with check add
	constraint FK_conges_Mens foreign key (CodeEmp)
		references Conges(CodeEmp);

alter table Conges
	drop constraint FK_Conges_Employes;

alter table Conges_Mens
	drop constraint FK_conges_Mens;

alter table Conges with check add
	constraint FK_Conges_Employes foreign key (CodeEmp) 
	references Employes(CodeEmp) ON DELETE CASCADE; 




alter table Conges_Mens with check add
	constraint FK_Conges_Mens foreign key (CodeEmp, Annee) references Conges(CodeEmp, Annee)  ON DELETE CASCADE;

create nonclustered index FK_Employes_Services on Employes(CodeService asc) ;
create nonclustered index FK_Employes8Employes on Employes(CodeChef asc);
create nonclustered index FK_Conges_Employes on Conges(CodeEmp asc);
create nonclustered index FK_Conges_mens_Conges on Conges_Mens(CodeEmp asc, Annee asc);





