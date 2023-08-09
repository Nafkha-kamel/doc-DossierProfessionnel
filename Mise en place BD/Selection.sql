use VideoLoc
go

--- Requêtes Sélection ---
select Titre, Nom, prenom, ville from Clients; 
select * from Clients order by Ville asc, Nom desc;
select titre, annee from dvd order by titre asc
select * from realisateurs  order by Annee_naissance asc
select * from Clients where Code_postal like '44%'
select * from Clients where Prenom like 'a%'
select * from Clients where datediff(year,Date_naissance, getdate())  between 42 and 52
select * from realisateurs where  pays in ('USA', 'ANGleterre')
select  * from realisateurs where Annee_naissance<1900
select * from dvd where Duree <= 120

-- Statistiques ---

select dvd.Titre, somme=count(Clients.Code_client) from Clients inner join factures on Clients.Code_client = factures.Code_client
			 inner join locations on factures.Num_facture = locations.Num_facture
				inner join dvd on locations.Num_dvd = dvd.Num_dvd group by(dvd.titre) order by somme


select genres_films.Signification, somme=COUNT(num_dvd) from dvd inner join genres_films on dvd.Code_genre=genres_films.Code_genre group by (genres_films.Signification)

select pays,count(dvd.Code_realisateur) from dvd inner join realisateurs on dvd.Code_realisateur = realisateurs.Code_realisateur group by (Pays)


select signification, COUNT(dvd.Num_dvd) from dvd inner join genres_films on dvd.Code_genre = genres_films.Code_genre where Annee in (1970,1980) group by (genres_films.Signification) 

select signification, avg(dvd.Duree) from dvd inner join genres_films on dvd.Code_genre = genres_films.Code_genre group by (genres_films.Signification) 

select signification, max(dvd.Duree) from dvd inner join genres_films on dvd.Code_genre = genres_films.Code_genre where Annee in (1970,1980) group by (genres_films.Signification) 

select MONTH(Date_naissance), COUNT(Clients.Code_client) from Clients inner join factures on Clients.Code_client = factures.Code_client group by MONTH(Date_naissance)

 --- requêtes multitables ---

select signification, Titre from dvd inner join genres_films on dvd.Code_genre = genres_films.Code_genre 

select titre, nom, prenom, pays, signification from dvd inner join genres_films on dvd.Code_genre = genres_films.Code_genre 
												inner join realisateurs on realisateurs.Code_realisateur = dvd.Code_realisateur 

select Nom, Prenom from Clients inner join factures on Clients.Code_client = factures.Code_client where YEAR(Date_facture) >=2006

select clients.Titre, Clients.Nom, clients.Prenom, realisateurs.Nom, realisateurs.Prenom, dvd.Titre  from Clients inner join factures on Clients.Code_client = factures.Code_client 
						inner join locations on factures.Num_facture = locations.Num_facture
						inner join dvd on locations.Num_dvd=dvd.Num_dvd
						inner join realisateurs on realisateurs.Code_realisateur = dvd.Code_realisateur
select * from factures
select clients.Titre, Clients.Nom, clients.Prenom from Clients inner join factures on Clients.Code_client = factures.Code_client 
						inner join locations on factures.Num_facture = locations.Num_facture
						inner join dvd on locations.Num_dvd=dvd.Num_dvd
						inner join realisateurs on realisateurs.Code_realisateur = dvd.Code_realisateur where Pays='allemagne' and year(Date_facture) = 2006 and MONTH(Date_facture) =06
select * from clients
select clients.Titre, Clients.Nom, clients.Prenom from Clients inner join factures on Clients.Code_client = factures.Code_client 
						inner join locations on factures.Num_facture = locations.Num_facture
						inner join dvd on locations.Num_dvd=dvd.Num_dvd
						inner join genres_films on genres_films.Code_genre = dvd.Code_genre where YEAR(Date_naissance) in (1960, 1970) and Clients.Titre = 'M.'
			
select clients.Code_client, nom,prenom, COUNT(dvd.Num_dvd) from Clients inner join factures on Clients.Code_client = factures.Code_client 
						inner join locations on factures.Num_facture = locations.Num_facture
						inner join dvd on locations.Num_dvd=dvd.Num_dvd group by Clients.Code_client, Nom, Prenom order by COUNT(dvd.Num_dvd) desc
						
insert into Clients (Code_client, Titre,Prenom,Nom, Adresse_rue, Code_postal, Ville, Num_telephone, Date_naissance, Enfants) 
			values('NAF000', 'M.', 'Kamel', 'NAFKHA', '2 rue bng', '75018', 'Paris', '0101010101', '1980-12-02', 2);

select * from Clients left outer join factures on Clients.Code_client=factures.Code_client where Num_facture is null

--- Analyse croisée ---

select pays, dvd.Code_genre, count(num_dvd) from dvd inner join realisateurs on dvd.Code_realisateur= realisateurs.Code_realisateur
							   inner join genres_films on genres_films.Code_genre = dvd.Code_genre
							group by pays, dvd.Code_genre

select  dvd.Titre, COUNT(clients.Code_client) from Clients inner join factures on Clients.Code_client = factures.Code_client 
						inner join locations on factures.Num_facture = locations.Num_facture
						inner join dvd on locations.Num_dvd=dvd.Num_dvd group by dvd.Titre

select Code_postal/1000, COUNT(clients.Code_client) from Clients group by Code_postal/1000

select  pays, Signification, somme=SUM(Duree) from dvd inner join realisateurs on dvd.Code_realisateur=realisateurs.Code_realisateur
						inner join genres_films on genres_films.Code_genre = dvd.Code_genre group by Pays, Signification
						
-- Action --
 select Titre, Nom, Prenom, age=year(getdate()) - YEAR(Date_naissance), Code_postal into #R1 from Clients where Code_postal/1000 = 44 

 set transaction isolation level read committed; 
 begin tran
 select * from #R1
 delete from #R1 where age >40
 select * from #R1
 rollback tran
