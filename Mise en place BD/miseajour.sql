use Locations
go

-- script de test des mises � jour d'un tableau --
insert into CLIENTS values(1, 'Albert', 'Anatole', 'Rue des accacias', 61000, 'Amiens');
insert into CLIENTS values(2, 'Bernard', 'Barnab�', 'Rue du bar', 01000, 'Bourg en Bresse');
insert into CLIENTS values(2, 'Dupond', 'Camille', 'Rue Cr�billon', 44000,'Nantes');
select * from CLIENTS;