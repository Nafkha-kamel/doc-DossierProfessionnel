Use Locations
go
 -- Creation BD locations --

 create table CLIENTS(
 noCli numeric(6) not null constraint PK_CLIENTS_noCli primary key,
 nom varchar(30) not null,
 prenom varchar(30),
 adresse varchar(120),
 cpo  numeric(5) not null constraint CK_CLIENTS_cpo check (cpo >= 1000 and cpo<=95999),
 ville  varchar(80) not null constraint DF_CLIENTS_ville default 'Nantes'
 );

 create table FICHES(
 noFic numeric(6) constraint PK_FICHES_noFic primary key,
 noCli numeric(6) not null,
 dateCrea datetime not null constraint DF_FICHES_datecrea default getdate(),
 DatePaye datetime,
 etat char(2) not null constraint DF_FICHES_etat default 'EC'
						constraint CK_FICHES_etat check (etat='EC' or etat ='RE' or etat = 'SO')
 );
 -- contrainte pour alter with nochek
 alter table FICHES with check
  add constraint CK_FICHES_DatePaye check ( etat='SO' and DatePaye >= dateCrea);
 --  constraint CK_FICHES_DatePaye check (DatePaye >= dateCrea),

 create table LIGNESFIC(
 noFic numeric(6),
 noLig numeric(6),
 refart char(8) not null,
 Depart datetime not null constraint DF_LIGNESFIC_Depart default getdate(),
 retour datetime, 
 constraint PK_LIGNESFIC_noFic_noLig primary key (noFic, noLig)
 );
  alter table LIGNESFIC with check
  add constraint CK_LIGNESFIC_retour check ( retour >= Depart);
 -- contrainte pour alter with nochek
 --  constraint CK_LIGNESFIC_retour check (retour >= Depart),

 create table ARTICLES(
 refart char(8) not null constraint PK_ARTICLES_refart primary key,
 designation  varchar(80) not null,
 codeGam char(5) not null,
 codeCate char(5) not null,
 );

 create table Gammes(
 codeGam char(5) not null constraint PK_Gammes_codeGam primary key,
 libelle  varchar(30) not null constraint UN_Gammes_libelle unique
 );
 create table GRILLETARIFS(
 codeGam char(5) not null,
 codeCate char(5) not null,
 codeTarif char(5) not null,
 constraint PK_GRILLETARIFS_codeGam_codeCate primary key (codeGam, codeCate)
 );

 create table CATEGORIES(
 codeCate char(5) not null constraint PK_CATEGORIES_codecate primary key, 
 libelle  varchar(30) not null constraint UN_CATEGORIES_libelle unique
 );

 create table TARIFS(
 codeTarif char(5) not null constraint PK_TARIFS_codeTarif primary key,
 libelle  varchar(30) not null constraint UN_TARFIS_libelle unique,
 prixJour numeric(5,2) not null constraint CK_TARIFS_prixJour check (prixJour >=0)
 );