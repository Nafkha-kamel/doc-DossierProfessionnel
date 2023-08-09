use VideoLoc
go

-----

create table Clients
(
Code_client char(6) constraint pk_clients_code_client primary key ,
Titre char(4),
Prenom varchar(20),
Nom varchar(20),
Adresse_rue varchar(120),
Code_postal char(5),
Ville varchar(20),
Num_telephone char(10),
Date_naissance datetime,
Enfants int

)


create table factures(
Num_facture int constraint pk_facture_Num_facture primary key identity,  
Code_client char(6) constraint fk_factures_clients_code_client foreign key references clients(Code_client),
Date_facture date
)

create table locations
(
Num_facture int constraint fk_locations_factures_num foreign key references factures(Num_facture), 
Num_dvd int  constraint fk_locations_dvd_num foreign key references dvd(Num_dvd),
Code_type char(2) constraint fk_locations_types_locations_code foreign key references types_locations(code_type),
Date_retour datetime,
constraint pk_locations_num primary key (Num_facture, Num_dvd)
)

create table types_locations
(
Code_type char(2) constraint pk_types_locations_code_type primary key,
Libelle varchar(20),
Coefficient numeric(2,1),
Nb_jours int
)

create table dvd
(
Num_dvd int constraint pk_dvd_num primary key identity,
Titre varchar(120),
Prix_base numeric(4,2) ,
Code_realisateur char(6) constraint fk_dvd_realisateurs_code foreign key references realisateurs(Code_realisateur),
Code_genre char(2) constraint fk_dvd_genres_films_code foreign key references genres_films(Code_genre),
Annee numeric(4),
Descriptif varchar(1000),
Duree numeric(3)
)

create table realisateurs
(
Code_realisateur char(6) constraint pk_realisateurs_code primary key,
Prenom varchar(20),
Nom varchar(20),
Annee_naissance numeric(4),
Pays varchar(20),
)
create table genres_films
(
Code_genre char(2) constraint pk_genre_films_code_genre primary key,
Signification varchar(20)
)

select * from Clients where Code_client = 'DER001';
