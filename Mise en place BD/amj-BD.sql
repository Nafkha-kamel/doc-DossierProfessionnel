use ExoLocations
go 


-- ajout des valeurs aux tables -- 
select * from Clients ;
insert into Clients(noCli, nom, prenom, adresse, cpo, ville)  values (1, 'Albert', 'Anatole', 'Rue des accacias', '61000', 'Amiens'); 
insert into Clients(noCli, nom, prenom, adresse, cpo, ville)  values (2, 'Bernard', 'Barnabé', 'Rue du bar', '01001', 'Bourg en Bresse'); 
insert into Clients(noCli, nom, prenom, adresse, cpo, ville)  values (3, 'Dupond', 'Camille', 'Rue Crébillon', '44000', 'Nantes'); 
set transaction isolation level read committed 
begin tran
delete from clients where nom like 'Dupon%';
rollback tran
update clients set adresse = 'Rue de la paix' where noCli=3; 