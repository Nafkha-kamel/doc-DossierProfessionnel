Use Locations
go
 -- BD locations --

 create table CLIENTS(
 noCli numeric(6),
 nom varchar(30),
 prenom varchar(30),
 adresse varchar(120),
 cpo  char(5),
 ville  varchar(80)
 );

 create table FICHES(
 noFic numeric(6),
 noCli numeric(6),
 dateCrea datetime,
 DatePaye datetime,
 etat char(2)
 );

 create table LIGNESFIC(
 noFic numeric(6),
 noLig numeric(6),
 refart char(8),
 Depart datetime,
 retour datetime
 );

 create table ARTICLES(
 refart datetime,
 designation  varchar(80),
 codeGam char(5),
 codeCate char(5),
 );

 create table Gammes(
 codeGam char(5),
 libelle  varchar(30)
 );
 create table GRILLETARIFS(
 codeGam char(5),
 codeCate char(5),
 codeTarif char(5)
 );

 create table CATEGORIES(
 codeCate char(5),
 libelle  varchar(30)
 );

 create table TARIFS(
 codeTarif char(5),
 libelle  varchar(30),
 prixJour numeric(5,2)
 );